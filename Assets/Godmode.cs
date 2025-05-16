using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Godmode : MonoBehaviour
{
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private TextMeshProUGUI text;

    private bool isGodmodeActive = false;

    public void ToggleGodmode()
    {
        isGodmodeActive = !isGodmodeActive;

        if (isGodmodeActive)
        {
            EnableGodmode();
        }
        else
        {
            DisableGodmode();
        }
    }

    private void EnableGodmode()
    {
        playerCollider.enabled = false;
        Color c = text.color;
        c.a = 1f;
        text.color = c;
    }

    private void DisableGodmode()
    {
        playerCollider.enabled = true;
        Color c = text.color;
        c.a = 0.2f;
        text.color = c;
    }
}
