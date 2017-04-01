using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Text;

public class ConnectSocket{
    private Socket mySocket;

    public static ConnectSocket instance;
    private byte[] receiveMess = new byte[1024];
    public static ConnectSocket getSocketInstance()
    {
        if (instance == null)
        {
            instance = new ConnectSocket();
        }
        return instance;
    }
    ConnectSocket()
    {

        mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        IPAddress ip = IPAddress.Parse("127.0.0.1");
        IPEndPoint ipe = new IPEndPoint(ip, 8885);
        //mySocket.Connect(ipe);
        System.IAsyncResult result = mySocket.BeginConnect(ipe, new System.AsyncCallback(ConnectCallBack), mySocket);
        bool connectsucces = result.AsyncWaitHandle.WaitOne(5000, true);//超时检测

        if (connectsucces)
        {
            Thread thread = new Thread(GetMess);
            thread.Start();
        }
        else//没有连接成功
        {
            Debug.Log("==========连接超时=======");
        }
        //mySocket.Connect(ipe);
        //Thread thread = new Thread(GetMess);
        //thread.Start();
    }

    private void ConnectCallBack(System.IAsyncResult ar)
    {
        Debug.Log("============服务器连接成功==========");
    }

    private void GetMess()
    {
        while(true)
        {
            if (!mySocket.Connected)
            {
                Debug.Log("============断开连接了==========");
                mySocket.Close();
                break;
            }
            try
            {
                int mesLength = mySocket.Receive(receiveMess);

                Debug.Log("========================"+ Encoding.ASCII.GetString(receiveMess, 0, mesLength));

            }
            catch (Exception ex)
            {
                mySocket.Shutdown(SocketShutdown.Both);
                mySocket.Close();
            }

        }

    }

    public void SendMess(byte[] mess)
    { 
        if (!mySocket.Connected)
        {
            Debug.Log("============断开连接了==========");
            mySocket.Close();
        }
        try
        {
            mySocket.Send(mess);
        }
        catch (Exception ex)
        {
            Debug.Log("====" + ex.Message);
        }


    }
}
