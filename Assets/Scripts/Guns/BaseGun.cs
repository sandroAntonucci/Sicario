using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Experimental.GlobalIllumination;

public abstract class BaseGun : BaseWeapon
{

    public AudioManager shootSFX;
    public AudioManager hitBodySFX;

    public string gunName;

    public GameObject bloodEffectPrefab;

    public Vector3 aimPosition;

    bool shootSoundActive;

    public float fireRate;
    public int maxAmmo;
    public int currentAmmo;
    public int weaponBulletDamage = 100;

    public GameObject bulletPrefab;  // Bullet prefab is still needed for pool creation
    public Transform shootPosition;

    public float recoilStrength = 5f; // Adjust this value to control how strong the recoil is
    public float recoilRecoverySpeed = 1.0f;
    private Quaternion startingRotation = Quaternion.Euler(0f, 0f, 0f);


    [SerializeField] private GameObject muzzleFlash;

    [SerializeField] private Light shotLight;

    public bool isShotgun = false;

    public InputActionAsset PlayerControls;
    private InputAction shootAction;

    public bool canShoot = true;
    private bool isShooting = false;
    public float recoilStrengthMultiplier = 30;

    private Coroutine ShootCooldownCoroutine;

    private void Awake()
    {

        pickUpController = GetComponent<PickUpController>();

        if (!isEnemyWeapon)
        {
            SetUpPlayerWeapon();
        }
        else
        {
            SetUpEnemyGun();
        }

    }

    private void OnEnable()
    {
        if (shootAction != null)
        {
            shootAction.Enable();
        }
    }

    private void OnDisable()
    {
        if (shootAction != null && !PickUpController.slotFull)
        {
            shootAction.Disable();
        }
    }

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    public void Update()
    {
        if (isShooting && canShoot && currentAmmo > 0)
        {
            Shoot();
        }

        if (startingRotation != null && !isEnemyWeapon)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, startingRotation, Time.deltaTime * recoilRecoverySpeed);
        }
    }

    public virtual void Shoot()
    {
        if (ShootCooldownCoroutine != null)
        {
            StopCoroutine(ShootCooldownCoroutine);
        }

        ShootCooldownCoroutine = StartCoroutine(ShootCooldown());

        if (!isEnemyWeapon) currentAmmo--;

        StartCoroutine(ShowMuzzleFlash());


        if (isShotgun)
        {
            for (int i = 0; i < 5; i++)
            {

                // Get a bullet from the pool
                GameObject bullet = BulletPool.Instance.GetBullet(shootPosition.position, shootPosition.rotation);
                BaseBullet bulletComp = bullet.GetComponent<BaseBullet>();

                bulletComp.gun = GetComponent<BaseGun>();

                if (isEnemyWeapon) bulletComp.isEnemyBullet = true;
                else bulletComp.isEnemyBullet = false;

                bulletComp.bulletDamage = weaponBulletDamage;

                bullet.SetActive(true);
            }
        }
        else
        {
            // Get a bullet from the pool
            GameObject bullet = BulletPool.Instance.GetBullet(shootPosition.position, shootPosition.rotation);
            BaseBullet bulletComp = bullet.GetComponent<BaseBullet>();

            bulletComp.gun = GetComponent<BaseGun>();

            

            if (isEnemyWeapon) bulletComp.isEnemyBullet = true;
            else bulletComp.isEnemyBullet = false;

            bulletComp.bulletDamage = weaponBulletDamage;

            bullet.SetActive(true);
        }

        if (shootSFX != null)
        {
            shootSFX.PlayRandomPitch();
        }


        if (!isEnemyWeapon) ApplyRecoil();
    }

    private IEnumerator ShowMuzzleFlash()
    {
        muzzleFlash.SetActive(true);

        // Randomize the rotation of the muzzle flash
        muzzleFlash.transform.localEulerAngles = new Vector3(Random.Range(0, 360), 180, 0);

        while (muzzleFlash.transform.localScale.x < 0.01)
        {
            shotLight.intensity += 0.3f;
            muzzleFlash.transform.localScale += new Vector3(0.008f, 0.008f, 0.008f);
            yield return null;
        }

        yield return new WaitForSeconds(0.05f);

        while (muzzleFlash.transform.localScale.x > 0)
        {
            shotLight.intensity -= 0.3f;
            muzzleFlash.transform.localScale -= new Vector3(0.004f, 0.004f, 0.004f);
            yield return null;
        }

        muzzleFlash.SetActive(false);

    }

    public virtual void StartShooting()
    {
        isShooting = true;
    }

    public virtual void StopShooting()
    {
        isShooting = false;
    }

    private IEnumerator ShootCooldown()
    {
        canShoot = false;

        if (!isEnemyWeapon)
        {
            yield return new WaitForSeconds(fireRate);
        }
        else
        {
            yield return new WaitForSeconds(fireRate * 4);
        }

        canShoot = true;
    }

    private void ApplyRecoil()
    {
        recoilStrength = fireRate * recoilStrengthMultiplier;
        recoilRecoverySpeed = 2 / fireRate;
        CameraEffects.Instance.recoil.recoil(-recoilStrength);
        float rand = Random.Range(-recoilStrength / 3f, recoilStrength / 3f);
        transform.rotation = transform.rotation * Quaternion.Euler(0f, rand, -recoilStrength);
    }

    private void SetUpEnemyGun()
    {
        pickUpController.enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<BoxCollider>().isTrigger = false;
    }


    public override void SetUpPlayerWeapon()
    {
        shootAction = PlayerControls.FindAction("Shoot");
        shootAction.performed += ctx => StartShooting();
        shootAction.canceled += ctx => StopShooting();

        isEnemyWeapon = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (GetComponent<Rigidbody>().velocity.magnitude > 2f)
            {

                if (bloodEffectPrefab != null)
                {
                    GameObject bloodEffect = Instantiate(bloodEffectPrefab, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
                    Destroy(bloodEffect, 2f);
                }

                if (hitBodySFX != null && shootSoundActive)
                {
                    hitBodySFX.PlayRandomPitch();
                }

                AIHandler aiHandlerComponent = collision.gameObject.GetComponent<AIHandler>();
                if (aiHandlerComponent != null)
                {
                    aiHandlerComponent.DealDamage(weaponBulletDamage, gunName, true);
                }
            }
        }

    }

    private IEnumerator SoundCooldown()
    {

        shootSoundActive = false;
        yield return new WaitForSeconds(1f);
        shootSoundActive = true;

    }
}
