using ServerCore;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DummyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            Connector connector = new Connector();
            connector.Connect(endPoint, () => { return new ServerSession(); });

            while (true)
            {
                try
                {

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                Thread.Sleep(1000);
            }
        }
    }
}

// 231031 퇴근 후 공부는 이론 공부로 인한 코드내용 변경 없음! 단순 커밋을 위한 주석을 추가적으로 작성한다.