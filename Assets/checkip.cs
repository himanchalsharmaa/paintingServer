using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine.UI;
using TMPro;

public class checkip : MonoBehaviour
{
    public TMP_Text hintText;
    private void Start()
    {
        GetLocalIPAddress();
    }
    public string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        int count = 0;
        foreach (var ip in host.AddressList)
        {
            count += 1;
            if (ip.AddressFamily == AddressFamily.InterNetwork || ip.AddressFamily == AddressFamily.InterNetworkV6)
            {
                hintText.text += ip.ToString()+": Count : " + count;
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }
}
