using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerDeath : MonoBehaviour
{

    public Volume deathEffect;
    public GameObject playerInterface;
    public bool isDead = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<BaseBullet>().isEnemyBullet)
        {
            isDead = true;
            StartCoroutine(Death());
        }
    }

    private IEnumerator Death()
    {

        playerInterface.SetActive(false);

        gameObject.GetComponent<PlayerMovement>().enabled = false;
        gameObject.GetComponent<PlayerRotate>().enabled = false;
        gameObject.GetComponent<PlayerRotateSmooth>().enabled = false;
        gameObject.GetComponent<PlayerAim>().enabled = false;

        gameObject.GetComponentInChildren<CameraStep>().enabled = false;
        gameObject.GetComponentInChildren<ProceduralRecoil>().enabled = false;
        gameObject.GetComponentInChildren<GunSway>().enabled = false;

        if (gameObject.GetComponentInChildren<PickUpController>() != null)
        {
            gameObject.GetComponentInChildren<PickUpController>().enabled = false;
        }

        if (gameObject.GetComponentInChildren<BaseWeapon>() != null)
        {
            gameObject.GetComponentInChildren<BaseWeapon>().enabled = false;
        }

        gameObject.GetComponent<CapsuleCollider>().enabled = false;

        float time = 0f;
        float duration = 0.3f;

        while (time < duration)
        {
            time += Time.deltaTime;
            deathEffect.weight = Mathf.Lerp(0f, 1f, time / duration);
            yield return null;
        }

    }

}
