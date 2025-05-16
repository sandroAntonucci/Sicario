using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpilepsyWarning : MonoBehaviour
{

    public GameObject menuCanvas;

    public void DisableCanvas()
    {
        gameObject.SetActive(false);
        menuCanvas.SetActive(true);
    }

}
