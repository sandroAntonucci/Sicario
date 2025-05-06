using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoHandler : MonoBehaviour
{

    private GameObject player;

    [SerializeField] private TextMeshProUGUI ammoText;

    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");

    }

    private void Update()
    {
        if (player.GetComponentInChildren<BaseGun>() != null)
        {
            ammoText.text = player.GetComponentInChildren<BaseGun>().currentAmmo.ToString() + "/" + player.GetComponentInChildren<BaseGun>().maxAmmo.ToString();
        }
        else
        {
            ammoText.text = "";
        }
    }

}
