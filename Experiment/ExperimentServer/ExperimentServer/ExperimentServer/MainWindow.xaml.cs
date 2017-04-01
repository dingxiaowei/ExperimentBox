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

namespace ExperimentServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        object o = new object();
        private static byte[] result = new byte[1024];
        private Socket serverSocket;
        private int myPort = 8885;


        public MainWindow()
        {
            InitializeComponent();
        }

        public void InitServerSetting()
        {
            //端口号
            myPort = 8885;
        }

        private void OpenServer_Click(object sender, RoutedEventArgs e)
        {
            StartListen();
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

            // Console.WriteLine("启动监听{0}成功", serverSocket.LocalEndPoint.ToString());
            ShowLogMessage("启动监听"+ serverSocket.LocalEndPoint.ToString()  + "成功");
            //通过Clientsoket发送数据  
            Thread myThread = new Thread(ListenClientConnect);
            //监听线程启动
            myThread.Start();
            //Console.ReadLine();
        }

        //断开连接
        public void ListenClientConnect()
        {
            while (true)
            {
                //阻塞监听
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
            int receiveNumber = 0;
            while (true)
            {

                try
                {
                    //通过clientSocket接收数据  
                    receiveNumber = myClientSocket.Receive(result);
                    for (int i=0; i< receiveNumber; i++)
                    {
                        ShowLogMessage(result[i].ToString());
                    }
                    //Console.WriteLine("接收客户端{0}消息{1}", myClientSocket.RemoteEndPoint.ToString(), Encoding.ASCII.GetString(result, 0, receiveNumber));
                   // ShowLogMessage("接收客户端" + myClientSocket.RemoteEndPoint.ToString() + "消息" + Encoding.BigEndianUnicode.GetString(result, 0, receiveNumber));


                }
                catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                ShowLogMessage(ex.Message);
                myClientSocket.Shutdown(SocketShutdown.Both);
                myClientSocket.Close();
                break;
            }

        }

        }

        public void ShowLogMessage(string  mess)
        {


            this.LogText.Dispatcher.Invoke(
                new Action (
                   delegate {
                       Paragraph p = new Paragraph();
                       Run r = new Run(mess);
                       p.Inlines.Add(r);
                       LogText.Document.Blocks.Add(p);
                   }
                ));



        }
    }
}
