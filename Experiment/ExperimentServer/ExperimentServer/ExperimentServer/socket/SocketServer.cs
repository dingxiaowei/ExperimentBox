using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ExperimentServer.socket
{
    public class SocketServer
    {
        private byte[] result = new byte[1024];
        //private static int myProt = 8885;   //端口  
        private Socket serverSocket;
        private int myPort = 8885;

        /// <summary>
        /// 服务器否关闭
        /// </summary>
        private bool _isStart;

        public SocketServer()
        {
            _isStart =  false;
        }
        public SocketServer(int serverPort)
        {
            _isStart = false;
            myPort = serverPort;
        }
        //监听客户端
        public void Init()
        {

        }
        
        public void StartListen()
        {
            //对外面内网都可访问
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, myPort);
            //
            serverSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            //绑定ip 端口
            serverSocket.Bind(localEndPoint);
            ////设定最多10个排队连接请求  
            serverSocket.Listen(10);

            Console.WriteLine("启动监听{0}成功", serverSocket.LocalEndPoint.ToString());

            //通过Clientsoket发送数据  
            Thread myThread = new Thread(ListenClientConnect);
            //监听线程启动
            myThread.Start();
            Console.ReadLine();
        }

        //断开连接
        public void ListenClientConnect()
        {
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                clientSocket.Send(Encoding.ASCII.GetBytes("Server Say Hello"));
                Thread receiveThread = new Thread(receiveMessage);


                //启动监听客户端消息线程
                receiveThread.Start(clientSocket);
            }
        }

        public void receiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            int receiveNumber = myClientSocket.Receive(result);

            while (true)
            {

                try
                {
                    //通过clientSocket接收数据  
                    

                    Console.WriteLine("接收客户端{0}消息{1}", myClientSocket.RemoteEndPoint.ToString(), Encoding.ASCII.GetString(result, 0, receiveNumber));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                }

            }
        }

    }
}
