using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hitmarker : MonoBehaviour
{

    [SerializeField] private Image hitmarker; 

    public IEnumerator ShowHitmarker()
    {
        /*
        float fadeTime = 0.1f;
        float showTime = 0.5f;

        while (hitmarker.color.a < 1)
        {
            Color color = hitmarker.color;
            color.a += Time.deltaTime / fadeTime;
            hitmarker.color = color;
            yield return null;
        }

        yield return new WaitForSeconds(showTime);

        while (hitmarker.color.a > 0)
        {
            Color color = hitmarker.color;
            color.a -= Time.deltaTime / fadeTime;
            hitmarker.color = color;
            yield return null;
        }
        */

        hitmarker.enabled = true;

        yield return new WaitForSeconds(0.4f);

        hitmarker.enabled = false;

    }

}
