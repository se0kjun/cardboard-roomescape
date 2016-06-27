using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;

public class MyoWebClient : MonoBehaviour {
    private Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private byte[] _recieveBuffer = new byte[8142];
    public string recString;

    public Thalmic.Myo.Pose recPose = Thalmic.Myo.Pose.Unknown;
    public Quaternion recOrientation = Quaternion.identity;
    public Thalmic.Myo.XDirection recXdir = Thalmic.Myo.XDirection.Unknown;

    private void SetupServer()
    {
        try
        {
            _clientSocket.Connect(new IPEndPoint(IPAddress.Parse("163.152.162.230"), 21570));
            //_clientSocket.Connect(new IPEndPoint(IPAddress.Loopback, 21570));
            //_clientSocket.Connect(new IPEndPoint(IPAddress.Parse("220.126.41.220"), 21574));
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
        recString = rawReceiveData.Substring(0, rawReceiveData.IndexOf("\r\n"));

        try {
            var data = recString.Split(':');
            recOrientation = new Quaternion(float.Parse(data[0].Split(',')[1]),
                float.Parse(data[0].Split(',')[2]),
                -float.Parse(data[0].Split(',')[0]),
                -float.Parse(data[0].Split(',')[3]));

            switch (data[1].Trim())
            {
                case "rest":
                    recPose = Thalmic.Myo.Pose.Rest;
                    break;
                case "fist":
                    recPose = Thalmic.Myo.Pose.Fist;
                    break;
                case "wavein":
                    recPose = Thalmic.Myo.Pose.WaveIn;
                    break;
                case "waveout":
                    recPose = Thalmic.Myo.Pose.WaveOut;
                    break;
                case "doubletap":
                    recPose = Thalmic.Myo.Pose.DoubleTap;
                    break;
                case "fingerspread":
                    recPose = Thalmic.Myo.Pose.FingersSpread;
                    break;
                default:
                    break;
            }

            switch (data[2].Trim())
            {
                case "toward_wrist":
                    recXdir = Thalmic.Myo.XDirection.TowardWrist;
                    break;
                case "toward_elbow":
                    recXdir = Thalmic.Myo.XDirection.TowardElbow;
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
        _clientSocket.BeginReceive(_recieveBuffer, 0, _recieveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
    }

    private void SendData(byte[] data)
    {
        SocketAsyncEventArgs socketAsyncData = new SocketAsyncEventArgs();
        socketAsyncData.SetBuffer(data, 0, data.Length);
        _clientSocket.SendAsync(socketAsyncData);
    }

	// Use this for initialization
	void Awake ()
    {
        SetupServer();
    }
	
	// Update is called once per frame
    void Update()
    {
        //SetupServer();
	}
}
