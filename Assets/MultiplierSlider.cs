using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplierSlider : MonoBehaviour
{

    [SerializeField] private Image background;
    [SerializeField] private Image foreground;

    [SerializeField] private Sprite blueEffect;
    [SerializeField] private Sprite blueEffectGlow;

    [SerializeField] private Sprite greenEffect;
    [SerializeField] private Sprite greenEffectGlow;

    [SerializeField] private Sprite orangeEffect;
    [SerializeField] private Sprite orangeEffectGlow;

    [SerializeField] private Sprite goldEffect;
    [SerializeField] private Sprite goldEffectGlow;

    public void ChangeMultiplierEffect(float multiplier)
    {

        if (multiplier <= 1)
        {
            background.sprite = blueEffect;
            foreground.sprite = blueEffectGlow;
        }
        else if (multiplier == 1.5)
        {
            background.sprite = greenEffect;
            foreground.sprite = greenEffectGlow;
        }
        else if (multiplier == 3)
        {
            background.sprite = orangeEffect;
            foreground.sprite = orangeEffectGlow;
        }
        else
        {
            background.sprite = goldEffect;
            foreground.sprite = goldEffectGlow;
        }


    }

}
