using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;

public class SensorWebClient : MonoBehaviour {
    private Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private byte[] _recieveBuffer = new byte[8142];
    public string recString;

    //true = walking
    //false = landing
    public FootState tempLeftFoot;
    public FootState tempRightFoot;

    //1 == walking
    //0 == landing
    const string FOOT_LOW = "0";
    const string FOOT_HIGH = "1";

    public UnityEngine.UI.Text sensorStatusText;

    private void SetupServer()
    {
        try
        {
            _clientSocket.Connect(new IPEndPoint(IPAddress.Parse("163.152.161.243"), 12345));
            //_clientSocket.Connect(new IPEndPoint(IPAddress.Loopback, 21570));
        }
        catch (SocketException ex)
        {
            Debug.Log(ex.Message);
        }

        _clientSocket.BeginReceive(_recieveBuffer, 0, _recieveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
    }

    private void ReceiveCallback(IAsyncResult AR)
    {
        //Check how much bytes are recieved and call EndRecieve to finalize handshake
        int recieved = _clientSocket.EndReceive(AR);

        if (recieved <= 0)
            return;

        //Copy the recieved data into new buffer , to avoid null bytes
        byte[] recData = new byte[recieved];
        Buffer.BlockCopy(_recieveBuffer, 0, recData, 0, recieved);

        var rawReceiveData = System.Text.Encoding.Default.GetString(recData);
        if (rawReceiveData.Contains("\r\n"))
            recString = rawReceiveData.Substring(0, rawReceiveData.IndexOf("\r\n"));

        try
        {
            string[] foot_reading = recString.Split(',');

            if (foot_reading.Length == 2)
            {
                tempLeftFoot = (foot_reading[0] == FOOT_LOW) ? FootState.LAND : FootState.WALK;
                tempRightFoot = (foot_reading[1] == FOOT_LOW) ? FootState.LAND : FootState.WALK;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        //Start receiving again
        _clientSocket.BeginReceive(_recieveBuffer, 0, _recieveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
    }

    private void SendData(byte[] data)
    {
        SocketAsyncEventArgs socketAsyncData = new SocketAsyncEventArgs();
        socketAsyncData.SetBuffer(data, 0, data.Length);
        _clientSocket.SendAsync(socketAsyncData);
    }

    void Awake()
    {
        SetupServer();
    }

    void Update()
    {
        string resultText = "";
        if (tempLeftFoot == FootState.LAND)
            resultText += "LAND";
        else if (tempLeftFoot == FootState.WALK)
            resultText += "WALK";
        else
            resultText += "UNKNOWN";

        resultText += " / ";

        if (tempRightFoot == FootState.LAND)
            resultText += "LAND";
        else if (tempRightFoot == FootState.WALK)
            resultText += "WALK";
        else
            resultText += "UNKNOWN";

        sensorStatusText.text = resultText;
    }
}
