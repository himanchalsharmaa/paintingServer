using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class beginning : MonoBehaviour
{
    private List<Camera> primecamera;
    private RawImage rawImages;
    public RawImage rawImage;
    public Canvas canvas;
    private void Start()
    {
        tcpRecieverManager.GetStarted();       
    }
    private void Update()
    {
        if (tcpRecieverManager.newConnection)
        {
            SceneManager.LoadSceneAsync("Painting", LoadSceneMode.Additive);
            tcpRecieverManager.newConnection = false;
        }
    }

    public void NewClientConnected()
    {
        Debug.Log("NewClientConnected");
        RenderTexture renderTexture = new RenderTexture(512,288,8);
        tcpRecieverManager.clientCameras[(byte)1].targetTexture = renderTexture;
        rawImage.texture = renderTexture;
        /*rawImages = new RawImage[primecamera.Count];
        foreach (Transform child in canvas.transform) { Destroy(child.gameObject); }
        for (int i=0;i<primecamera.Count;i++)
        {
            rawImages[i].texture = primecamera[i].targetTexture;
        }*/
    }


}
