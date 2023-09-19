using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DummyClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            // 휴대폰 설정(=손님 소켓 생성)
            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // 문지기에게 연결한다
                // endPoint로 입장을 요청 문의한다.
                socket.Connect(endPoint);
                // 로그를 통해 연결 확인
                Console.WriteLine($"Connected To {socket.RemoteEndPoint.ToString()}");

                // 보낸다
                byte[] sendBuff = Encoding.UTF8.GetBytes("Hello World!");
                int sendBytes = socket.Send(sendBuff);

                // 받는다
                // 서버가 나한테 얼마를 보낼 지 모르므로 크게 설정한다
                byte[] recvBuff = new byte[1024];
                int recvBytes = socket.Receive(recvBuff);
                // 받은 데이터를 문자열로 변환한다.
                string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
                Console.WriteLine($"[From Server] {recvData}");

                // 닫는다
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}