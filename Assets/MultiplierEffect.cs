using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MultiplierEffect : MonoBehaviour
{

    [SerializeField] private GameObject multiplierNum;

    public void ChangeMultiplier(float multiplier)
    {

        GameObject multiplierNumInstance = Instantiate(multiplierNum, transform.position, Quaternion.identity);

        multiplierNumInstance.transform.SetParent(transform);

        multiplierNumInstance.GetComponent<TextMeshProUGUI>().text = multiplier.ToString() + "x";

        Destroy(multiplierNumInstance, 1.5f); 
            
    }

}
