using System.Collections;
using JetBrains.Annotations;
using TMPro.Examples;
using UnityEngine;


public class BaseBullet : MonoBehaviour
{

    public GameObject bloodEffectPrefab;

    public AudioManager hitSFX;

    public TrailRenderer trailRenderer;

    // The bullet's movement speed
    public float speed = 5f;
    public float lifeDuration = 2f;
    public int bulletDamage;

    public BaseGun gun;

    public bool canDamage = true;

    public bool isEnemyBullet = false;

    public BulletPool bulletPool;

    public ParticleSystem hitEffectOne;
    public ParticleSystem hitEffectTwo;

    private Rigidbody rb;

    private Coroutine DestroyCoroutine;

    private void Awake()
    {
        gameObject.SetActive(false); // Deactivate the bullet initially
    }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();

        GetComponent<Collider>().enabled = true;

        trailRenderer.enabled = true;

        canDamage = true;

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;

        // Draws raycast from the camera to know where the bullet is going
        Transform cameraHolder = GameObject.FindGameObjectWithTag("MainCamera").transform;

        Ray ray = new Ray(cameraHolder.position + cameraHolder.forward * 0.1f, cameraHolder.forward);
        RaycastHit hit;


        if (isEnemyBullet)
        {
            if (gun.isShotgun)
            {
                // Base direction towards player
                Vector3 playerDirection = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;

                // Add random spread
                float spreadAngle = 10f; // You can adjust this
                Vector3 spreadDirection = Quaternion.Euler(
                    Random.Range(-spreadAngle, spreadAngle),
                    Random.Range(-spreadAngle, spreadAngle),
                    0f // Usually we don't rotate on Z for spread
                ) * playerDirection;

                rb.velocity = spreadDirection.normalized * speed * 0.1f;
            }
            else
            {
                Vector3 playerDirection = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;
                rb.velocity = playerDirection * speed * 0.1f;
            }
        }

        else
        {
            if (Physics.Raycast(ray, out hit) && !GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAim>().isAiming)
            {
                if (!gun.isShotgun)
                {
                    Vector3 direction = (hit.point - transform.position).normalized;
                    rb.velocity = direction * speed;
                }
                else
                {
                    // For shotgun bullets, we want to apply a random spread
                    Vector3 randomSpread = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
                    Vector3 direction = (hit.point - transform.position).normalized + randomSpread;
                    rb.velocity = direction * speed;
                }
            }
            else
            {

                if (gun.isShotgun)
                {
                    Vector3 randomSpread = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
                    Vector3 direction = (cameraHolder.forward).normalized + randomSpread;
                    rb.velocity = direction * speed;
                }
                else
                {
                    rb.velocity = cameraHolder.forward * speed;
                }

            }

            trailRenderer.enabled = false;
        }

    }

    private void FixedUpdate()
    {
        float moveDistance = speed * Time.deltaTime;
        RaycastHit hit;
        float detectionDistance = moveDistance + 0.5f; // or some small buffer

        if (Physics.Raycast(transform.position, rb.velocity.normalized, out hit, detectionDistance) && !isEnemyBullet) 
        { 
            if (hit.collider.CompareTag("Enemy") && canDamage)
            {


                GetComponent<Collider>().enabled = false;
                if (bloodEffectPrefab != null)
                {
                    GameObject bloodEffect = Instantiate(bloodEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(bloodEffect, 2f);
                }

                
                GameObject enemyObject = hit.collider.gameObject;
                AIHandler aiHandlerComponent = hit.collider.GetComponent<AIHandler>();

                while (enemyObject != null && aiHandlerComponent == null)
                {
                    enemyObject = enemyObject.transform.parent.gameObject;
                    aiHandlerComponent = enemyObject.GetComponent<AIHandler>();
                }

                if (aiHandlerComponent != null)
                {
                    if (gun.hitBodySFX != null)
                    {
                        gun.hitBodySFX.PlayRandomPitch();
                    }

                    int damageApplied = bulletDamage;

                    int layerIndex = hit.collider.gameObject.layer;
                    string layerName = LayerMask.LayerToName(layerIndex);

                    if (layerName == "Head")
                    {
                        damageApplied = Mathf.RoundToInt(bulletDamage * 2.5f);
                    }
                    else if (layerName == "Body")
                    {
                        damageApplied = Mathf.RoundToInt(bulletDamage * 1.5f);
                    }
                    else if (layerName == "Legs")
                    {
                        damageApplied = Mathf.RoundToInt(bulletDamage * 0.5f);
                    }

                    GameObject.FindGameObjectWithTag("Hitmarker").GetComponent<Hitmarker>().StartCoroutine("ShowHitmarker");
                    aiHandlerComponent.DealDamage(damageApplied, gun.gunName);
                    canDamage = false;
                    ReturnToPool();
                }
            }
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Enemy"))
        {
            if (DestroyCoroutine != null) return;

            BulletHoleDecalPool.Instance.SpawnBulletHole(collision.contacts[0].point, collision.contacts[0].normal);

            trailRenderer.enabled = false;

            if (hitSFX != null)
            {
                hitSFX.PlayRandomPitch();
            }

            canDamage = false;

            DestroyCoroutine = StartCoroutine(DestroyAfterDelay());
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        rb.velocity *= 0.1f;

        hitEffectOne.transform.parent = null;
        hitEffectOne.Play();

        hitEffectTwo.transform.parent = null;
        hitEffectTwo.Play();

        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        yield return new WaitForSeconds(lifeDuration);
        DestroyCoroutine = null;

        hitEffectOne.transform.parent = gameObject.transform;
        hitEffectOne.transform.localPosition = Vector3.zero;

        hitEffectTwo.transform.parent = gameObject.transform;
        hitEffectTwo.transform.localPosition = Vector3.zero;


        ReturnToPool();
    }

    private void ReturnToPool()
    {
        BulletPool.Instance.ReturnBullet(gameObject);
    }
}
