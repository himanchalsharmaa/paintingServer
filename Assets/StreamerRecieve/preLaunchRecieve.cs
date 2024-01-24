using System.Collections;
using UnityEngine;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using System;
using System.Collections.Concurrent;
using UnityEngine.SceneManagement;

using System.Threading;
public class preLaunchRecieve : MonoBehaviour
{
    private TcpClient tcpClient;
    public GameObject[] xrrig;
    NetworkStream stream;
    ConcurrentQueue<deconstructincomingdata> arraytoread = new ConcurrentQueue<deconstructincomingdata>();
    bool threadreading = false;
    controllerMimick controlMock;
    activeControl currentcontrller = activeControl.None;
    public Camera camofthisscene;
    public byte instancenumber;

    IEnumerator runthis()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("Instance Number: " + instancenumber);
        tcpRecieverManager.clientCameras.TryAdd(instancenumber,camofthisscene);
        Debug.Log("Added camera");
        GameObject.Find("Beginner").GetComponent<beginning>().NewClientConnected();
        tcpRecieverManager.connectedClients += 1;
        // Debug.Log(RenderTexture.active.height + ":" + RenderTexture.active.width + ":" + RenderTexture.active.depth);
        //Debug.Log(camofthisscene.targetTexture.height + ":" + camofthisscene.targetTexture.width + ":" + camofthisscene.targetTexture.depth);        
        //camofthisscene.rect = new Rect(instancenumber / tcpRecieverManager.connectedClients,0,1, 1 / tcpRecieverManager.connectedClients) ;
    }
    void Start()
    {
        controlMock = GetComponent<controllerMimick>();
        DontDestroyOnLoad(gameObject);
        instancenumber = tcpRecieverManager.connectedClients;
       // StartCoroutine(runthis());
      //  StartCoroutine(getclientandstream());
    }
    IEnumerator getclientandstream()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            if (tcpRecieverManager.GetTcpClient(instancenumber, out tcpClient)) {
                Debug.Log("GOT CONNECTION");
                stream = tcpClient.GetStream();  // Null until connection made
                if (tcpClient.Connected && stream.CanRead)
                {
                    _ = Task.Factory.StartNew(ReadStreamOnThread, TaskCreationOptions.LongRunning);
                    StartCoroutine(updatepositionrecieved());
                    break;
                }
            }
        }
    }

    void ReadStreamOnThread()
    {
        threadreading = true;
        while (true)
        {
                byte[] prsofgo = new byte[256];
                byte[] scenenamearr = new byte[512];
                try
                {
                    int bytesread = stream.Read(prsofgo, 0, 196);
                    if (prsofgo != null)
                    {
                        float[] prsinfo = new float[49];
                        Buffer.BlockCopy(prsofgo, 0, prsinfo, 0, 196);

                        string btninfo = "";
                        if (stream.ReadByte() == 0)
                        {
                            btninfo = "";
                        }
                        else
                        {
                            int btninfocount = stream.ReadByte(); // all are bytes following each other so just attach to string
                            int readbyte;
                            for (int i = 0; i < btninfocount; i++)
                            {
                                readbyte = stream.ReadByte();
                                btninfo += readbyte;  // each byte is as it is as the string was
                            }
                            Debug.Log(btninfo);
                        }
                        string scenename;
                        if (stream.ReadByte() == 0)
                        {
                            scenename = "";
                        }
                        else
                        {
                            int sceneNamelength = stream.ReadByte();
                            stream.Read(scenenamearr, 0, sceneNamelength * 4);
                            scenename = Encoding.UTF8.GetString(scenenamearr, 0, sceneNamelength * 4);
                        }
                        arraytoread.Enqueue(new deconstructincomingdata { prsinf = prsinfo, btninf = btninfo, sceneinf = scenename });
                    }

                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    //StartCoroutine(AcceptClients());
                    break;
                }
        }
    }
    IEnumerator updatepositionrecieved()
    {
        int emptycount = 0;
        while (true)
        {
            if (threadreading)
            {
                deconstructincomingdata valuestocopy;
                if (arraytoread.TryDequeue(out valuestocopy))       //Concurrent queue is not bad performance
                {
                    float[] prsinfo = valuestocopy.prsinf;
                    string btninfo = valuestocopy.btninf;
                    string scenename = valuestocopy.sceneinf;
                    int gotrack = 0;
                    for (int i = 0; i < prsinfo.Length;)
                    {
                        xrrig[gotrack].transform.localPosition = new Vector3(prsinfo[i++], prsinfo[i++], prsinfo[i++]);
                        xrrig[gotrack].transform.rotation = new Quaternion(prsinfo[i++], prsinfo[i++], prsinfo[i++], prsinfo[i++]);
                        if (i % 7 == 0)
                        {
                            gotrack += 1;
                        }
                    }
                    if (btninfo != "")
                    {
                        pressingButtons(btninfo);
                    }
                    if (scenename != "")
                    {
                        Debug.Log("Scene change detected");
                    }
                    emptycount = 0;
                    yield return null;
                }
                else
                {
                    emptycount += 1;
                    if (emptycount > 1000)
                    {
                        yield return new WaitForSeconds(1);
                        emptycount = 0;
                    }
                }

                /*if (jsonData != SceneManager.GetActiveScene().name)
                {
                    if (jsonData.Contains("Quest"))
                    {
                        string scenenamewithoutquest = jsonData.Substring(0, jsonData.IndexOf("Quest")) + jsonData.Substring(jsonData.IndexOf("Quest"), jsonData.Length - 1);
                        Debug.Log("SCENE NAME:" + scenenamewithoutquest);
                        SceneManager.LoadScene(scenenamewithoutquest);
                    }
                    else { SceneManager.LoadScene(jsonData); Debug.Log("Scene name :" + jsonData); }
                } */

            }
            else { Debug.Log("Waiting for thread to read"); yield return new WaitForSeconds(1); }

        }
    }

    enum activeControl
    {
        Left,
        Right,
        None
    }

    void pressingButtons(string btnspress)
    {
        for (int i = 0; i < btnspress.Length - 1; i++)
        {
            if (btnspress[i] == '0' && currentcontrller != activeControl.Left)
            {
                StartCoroutine(controlMock.keyboardMimic('T'));
                currentcontrller = activeControl.Left;
            }
            else if (btnspress[i] == '1' && currentcontrller != activeControl.Right)
            {
                StartCoroutine(controlMock.keyboardMimic('Y'));
                currentcontrller = activeControl.Right;
            }
            switch (btnspress[++i])
            {
                case '0': StartCoroutine(controlMock.mousePress()); break;
                case '1': StartCoroutine(controlMock.keyboardMimic('B')); break;
                case '2': StartCoroutine(controlMock.keyboardMimic('N')); break;
                default: continue;
            }
        }
    }
    private void OnApplicationQuit()
    {
        tcpClient.Close();
        tcpRecieverManager.EndAcceptingThreads();
    }
    private void OnDisable()
    {
        tcpRecieverManager.EndAcceptingThreads();
    }
}
public class deconstructincomingdata
{
    public float[] prsinf { get; set; }
    public string btninf { get; set; }
    public string sceneinf { get; set; }
}

