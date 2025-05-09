using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TVTurnEffect : MonoBehaviour
{

    public VideoPlayer videoPlayer;

    public RawImage videoImage;

    public RenderTexture videoRender;


    private void Start()
    {
        StartCoroutine(ChangeImageTexture());
    }

    private void Update()
    {
        if (videoPlayer.isPaused)
            Destroy(gameObject);
    }

    private IEnumerator ChangeImageTexture()
    {
        yield return new WaitForSeconds(0.05f);
        videoImage.color = new Color(1, 1, 1, 1);
        videoImage.texture = videoRender;
    }

}
