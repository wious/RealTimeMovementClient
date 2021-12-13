using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedClient : MonoBehaviour
{
    int connectionID;
    int maxConnections = 1000;
    int reliableChannelID;
    int unreliableChannelID;
    int hostID;
    int socketPort = 9003;
    byte error;
    bool isConnected = false;
    int ourClientID;

    void Start()
    {
        if (NetworkedClientProcessing.GetNetworkedClient() == null)
        {
            DontDestroyOnLoad(this.gameObject);
            NetworkedClientProcessing.SetNetworkedClient(this);
            Connect();
        }
        else
        {
            Debug.Log("Singleton-ish architecture violation detected, investigate where NetworkedClient.cs Start() is being called.  Are you creating a second instance of the NetworkedClient game object or has the NetworkedClient.cs been attached to more than one game object?");
            Destroy(this.gameObject);
        }
    }
    void Update()
    {
        if (isConnected)
        {
            int recHostID;
            int recConnectionID;
            int recChannelID;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostID, out recConnectionID, out recChannelID, recBuffer, bufferSize, out dataSize, out error);

            switch (recNetworkEvent)
            {
                case NetworkEventType.ConnectEvent:
                    NetworkedClientProcessing.ConnectionEvent();
                    break;
                case NetworkEventType.DataEvent:
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    NetworkedClientProcessing.ReceivedMessageFromServer(msg);
                    break;
                case NetworkEventType.DisconnectEvent:
                    isConnected = false;
                    NetworkedClientProcessing.DisconnectionEvent();
                    break;
            }
        }
    }
    public void Connect()
    {
        if (!isConnected)
        {
            NetworkTransport.Init();
            ConnectionConfig config = new ConnectionConfig();
            reliableChannelID = config.AddChannel(QosType.Reliable);
            unreliableChannelID = config.AddChannel(QosType.Unreliable);
            HostTopology topology = new HostTopology(config, maxConnections);
            hostID = NetworkTransport.AddHost(topology, 0);
            connectionID = NetworkTransport.Connect(hostID, "192.168.2.20", socketPort, 0, out error); // server is local on network

            if (error == 0)
            {
                isConnected = true;
                Debug.Log("Network client init successful, waiting for server connection.");
            }
            else
                Debug.Log("Network client init failed!");
        }
    }
    public void Disconnect()
    {
        NetworkTransport.Disconnect(hostID, connectionID, out error);
    }
    public void SendMessageToServer(string msg)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(msg);
        NetworkTransport.Send(hostID, connectionID, reliableChannelID, buffer, msg.Length * sizeof(char), out error);
    }
    public bool IsConnected()
    {
        return isConnected;
    }

}
