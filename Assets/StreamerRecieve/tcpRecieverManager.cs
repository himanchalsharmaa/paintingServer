using UnityEngine;
using System.Net.Sockets;
using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;


static class tcpRecieverManager 
{
    static TcpListener tcpListener;
    private static Dictionary<byte,TcpClient> tcpClientsConnected = new Dictionary<byte, TcpClient>();

    public const UInt16 DiscoveryPort = 53123;
    public static UInt16 ServerPort = 2456;

    public static byte connectedClients = 1;
    private static bool closethreads = false;

    public static Dictionary<byte, Camera> clientCameras = new Dictionary<byte, Camera>();
    public static bool newConnection = false;

    public static void GetStarted()
    {
        Debug.Log("Creating listeners");
        tcpListener = new TcpListener(IPAddress.Any,0);
        tcpListener.Start();

        _ = Task.Factory.StartNew(ServiceDiscovery, TaskCreationOptions.LongRunning);
        _ = Task.Factory.StartNew(clientstreamasync, TaskCreationOptions.LongRunning);
    }

    private static void ServiceDiscovery()
    {
        using (UdpClient udpClient = new UdpClient())
        {
            udpClient.EnableBroadcast = true;

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, DiscoveryPort);

            ServerPort = UInt16.Parse(tcpListener.LocalEndpoint.ToString().Split(':')[1]);

            while (true)
            {
                if (closethreads) { break; }
                byte[] data = Encoding.ASCII.GetBytes($"TheDirector:{ServerPort}");
                udpClient.Send(data, data.Length, endPoint);
                Debug.Log("Speaking on: "  + endPoint.ToString());
                Thread.Sleep(1500); // Adjust the interval as needed
            }
        }
    }
    private static void clientstreamasync()
    {
        while (true)
        {
            if (closethreads) { break; }
            try
            {
                Debug.Log("Here1");
                tcpClientsConnected.Add(connectedClients, tcpListener.AcceptTcpClient());
                Debug.Log("Here2");
                newConnection = true;
                Debug.Log("connectedClients count: " + (connectedClients));
            }
            catch (Exception e)
            {
                Debug.Log("Exception connecting clients: " + e);
            }
        }
    }

    public static bool GetTcpClient(byte instanceNum, out TcpClient tcpClient)
    {
        return tcpClientsConnected.TryGetValue(instanceNum, out tcpClient);
    }

    public static void EndAcceptingThreads()
    {
        Debug.Log("Ending");
        tcpListener.Server.Close();
        tcpListener.Stop();
        closethreads = true;
    }

}
