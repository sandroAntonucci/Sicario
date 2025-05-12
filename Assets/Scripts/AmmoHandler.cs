using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoHandler : MonoBehaviour
{

    private GameObject player;

    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI ammoText2;

    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");

    }

    private void Update()
    {
        if (player.GetComponentInChildren<BaseGun>() != null)
        {
            ammoText.text = player.GetComponentInChildren<BaseGun>().currentAmmo.ToString() + "/" + player.GetComponentInChildren<BaseGun>().maxAmmo.ToString();
            ammoText2.text = player.GetComponentInChildren<BaseGun>().currentAmmo.ToString() + "/" + player.GetComponentInChildren<BaseGun>().maxAmmo.ToString();
        }
        else
        {
            ammoText.text = "";
            ammoText2.text = "";
        }
    }

}
