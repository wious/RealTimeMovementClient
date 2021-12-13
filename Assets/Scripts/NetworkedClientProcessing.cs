using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class NetworkedClientProcessing
{

    #region Send and Receive Data Functions
    static public void ReceivedMessageFromServer(string msg)
    {
        Debug.Log("msg received = " + msg + ".");

        string[] csv = msg.Split(',');
        int signifier = int.Parse(csv[0]);

        if (signifier == ServerToClientSignifiers.VelocityAndPosition)
        {
            Vector2 vel = new Vector2(float.Parse(csv[1]), float.Parse(csv[2]));
            Vector2 pos = new Vector2(float.Parse(csv[3]), float.Parse(csv[4]));

            gameLogic.GetComponent<GameLogic>().SetVelocityAndPosition(vel, pos);
        }
        // else if (signifier == ServerToClientSignifiers.asd)
        // {

        // }

        //gameLogic.DoSomething();

    }

    static public void SendMessageToServer(string msg)
    {
        networkedClient.SendMessageToServer(msg);
    }

    #endregion

    #region Connection Related Functions and Events
    static public void ConnectionEvent()
    {
        Debug.Log("Network Connection Event!");
    }
    static public void DisconnectionEvent()
    {
        Debug.Log("Network Disconnection Event!");
    }
    static public bool IsConnectedToServer()
    {
        return networkedClient.IsConnected();
    }
    static public void ConnectToServer()
    {
        networkedClient.Connect();
    }
    static public void DisconnectFromServer()
    {
        networkedClient.Disconnect();
    }

    #endregion

    #region Setup
    static NetworkedClient networkedClient;
    static GameLogic gameLogic;

    static public void SetNetworkedClient(NetworkedClient NetworkedClient)
    {
        networkedClient = NetworkedClient;
    }
    static public NetworkedClient GetNetworkedClient()
    {
        return networkedClient;
    }
    static public void SetGameLogic(GameLogic GameLogic)
    {
        gameLogic = GameLogic;
    }

    #endregion

}

#region Protocol Signifiers
static public class ClientToServerSignifiers
{
    public const int KeyBoardInputUpdate = 1;

    //public const int 
}

static public class ServerToClientSignifiers
{
    public const int VelocityAndPosition = 1;
}

#endregion

static public class KeyboardInputUpdate
{
    public const int Up = 1;

    public const int Down = 2;

    public const int left = 3;
    
    public const int right =4 ;

   

    public const int Upright = 5;

    public const int Upleft = 6;
    
    public const int DownRight = 7;

    public const int Downleft = 8;

    public const int Nothing = 100;

}