using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ExperimentClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        object o = new object();
        private byte[] result = new byte[1024];
        private int myPort = 8885;
        private Socket clientSocket;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void InitClientSetting()
        {            
            //端口号
            myPort = 8885;
        }

        public void clientRequest()
        {
            //  IPAddress ip = IPAddress.Parse(getLocalmachineIPAddress());    

            //IPAddress ip = IPAddress.Parse("192.168.1.144");
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {

                clientSocket.Connect(new IPEndPoint(ip, myPort));
                //Console.WriteLine("连接服务器成功");
                ShowLogMessage("连接服务器成功");
            }
            catch
            {
                //Console.WriteLine("连接服务器失败，请按回车键退出！");
                ShowLogMessage("连接服务器失败，请按回车键退出！");
                return;
            }
            //通过 clientSocket 接收数据  
            int receiveLength = clientSocket.Receive(result);
            //Console.WriteLine("接收服务器消息：{0}", Encoding.ASCII.GetString(result, 0, receiveLength));
            ShowLogMessage("接收服务器消息：" + Encoding.ASCII.GetString(result, 0, receiveLength));
            //通过 clientSocket 发送数据  
            //for (int i = 0; i < 10; i++)
            //{

                try
                {
                    Thread.Sleep(1000);
                    //DateTime now = DateTime.Now;
                    //string sendMessage = "老婆 我爱你！！！" + DateTime.Now;
                    //clientSocket.Send(Encoding.ASCII.GetBytes(sendMessage));
                    //clientSocket.Send(Encoding.BigEndianUnicode.GetBytes(sendMessage));
                    byte[] messByte = new byte[13];
                    //帧头
                    messByte[0] = 0x54;
                    messByte[1] = 0x43;
                    //流水号
                    messByte[2] = 0x00;
                    messByte[3] = 0x00;
                    messByte[4] = 0x00;
                    messByte[5] = 0x00;
                    // 设备类型 的灯
                    messByte[6] = 0x01;
                    //设备编号
                    messByte[7] = 0x01;
                    //功能码
                    messByte[8] = 0x01;
                    //数据长度
                    messByte[9] = 0x00;

                    //CRC16校验
                    messByte[10] = 0x54;
                    messByte[11] = 0x54;
                    //帧尾
                    messByte[12] = 0xED;

                    clientSocket.Send(messByte);


                    //Console.WriteLine("向服务器发送消息：{0}", sendMessage);
                    //ShowLogMessage("向服务器发送消息：" + sendMessage);

                }
                catch
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    //break;
                }
            //}
            //Console.WriteLine("发送完毕，按回车键退出");
            //Console.ReadLine();
            ShowLogMessage("发送完毕，按回车键退出");

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

        public void ShowLogMessage(string mess)
        {

                Paragraph p = new Paragraph();
                Run r = new Run(mess);
                p.Inlines.Add(r);
                LogText.Document.Blocks.Add(p);


        }

        private void SendMess_Click(object sender, RoutedEventArgs e)
        {
            clientRequest();
        }
    }


}
