using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;


namespace ExperimentServer.socket
{
    public class ClientSocket
    {
        private static byte[] result = new byte[1024];
        private int myPort = 8885;
        private Socket clientSocket;

        public ClientSocket(int mPort)
        {
            myPort = mPort;
        }

        public void clientRequest()
        {
            //  IPAddress ip = IPAddress.Parse(getLocalmachineIPAddress());    

            IPAddress ip = IPAddress.Parse("192.168.1.144");
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {

                clientSocket.Connect(new IPEndPoint(ip, myPort));
                Console.WriteLine("连接服务器成功");
            }
            catch
            {
                Console.WriteLine("连接服务器失败，请按回车键退出！");
                return;
            }
            //通过 clientSocket 接收数据  
            int receiveLength = clientSocket.Receive(result);
            Console.WriteLine("接收服务器消息：{0}", Encoding.ASCII.GetString(result, 0, receiveLength));
            //通过 clientSocket 发送数据  
            for (int i=0; i<10; i++)
            {
                
                try
                {
                    Thread.Sleep(1000);
                    //DateTime now = DateTime.Now;
                    string sendMessage = "client send Message Hellp" + DateTime.Now;
                    clientSocket.Send(Encoding.ASCII.GetBytes(sendMessage));
                    Console.WriteLine("向服务器发送消息：{0}",sendMessage);

                }
                catch
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    break;
                }
            }
            Console.WriteLine("发送完毕，按回车键退出");
            Console.ReadLine();

        }

        //获取本地IP地址
        public string getLocalmachineIPAddress()
        {
            string localAddressIP = string.Empty;
            string hostname = Dns.GetHostName();
            IPAddress[] ips = Dns.GetHostAddresses(hostname);
            foreach (IPAddress ip in ips)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localAddressIP = ip.ToString();
                    break;

                }
            }

            return localAddressIP;
        }

    }
}
