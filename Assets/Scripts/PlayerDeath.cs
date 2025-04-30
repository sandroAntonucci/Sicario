using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerDeath : MonoBehaviour
{

    public Volume deathEffect;
    public GameObject playerInterface;
    public GameObject deathInterface;
    public bool isDead = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<BaseBullet>().isEnemyBullet && collision.gameObject.GetComponent<BaseBullet>().canDamage)
        {
            isDead = true;
            StartCoroutine(Death());
        }
    }

    private IEnumerator Death()
    {

        Time.timeScale = 0.5f;

        playerInterface.SetActive(false);



        Destroy(GetComponent<PlayerMovement>());
        Destroy(GetComponent<PlayerRotate>());
        Destroy(GetComponent<PlayerRotateSmooth>());
        
        GetComponent<PlayerAim>().StopAllCoroutines();

        Destroy(GetComponent<PlayerAim>());

        Destroy(GetComponentInChildren<CameraStep>());
        Destroy(GetComponentInChildren<ProceduralRecoil>());
        Destroy(GetComponentInChildren<GunSway>());

        if (gameObject.GetComponentInChildren<BaseWeapon>() != null)
        {
            Destroy(GetComponentInChildren<BaseWeapon>());
        }

        Destroy(GetComponent<CapsuleCollider>());

        float time = 0f;
        float duration = 0.1f;

        while (time < duration)
        {
            time += Time.deltaTime;
            deathEffect.weight = Mathf.Lerp(0f, 1f, time / duration);
            yield return null;
        }

        deathInterface.SetActive(true);

    }

}
