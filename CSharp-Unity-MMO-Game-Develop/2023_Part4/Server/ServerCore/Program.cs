using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class Program
    {
        static void Main(string[] args)
        {
            // 내 로컬 컴퓨터의 호스트 이름을 가져온다.
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);

            // 구글과 같은 트래픽이 엄청 많은 곳은 IP 주소 여러 개를 사용하여 부하를 줄여준다.
            IPAddress ipAddr = ipHost.AddressList[0];

            // 식당으로 비유하자면 ipAddr은 식당의 주소이고, 7777은 식당의 문 번호이다.
            // 식당 입장시 7777 포트를 식당/손님과 동일하게 사용해줘야 입장 가능하다.
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            // 문지기 구현
            Socket listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // 인자 값
            // 1. 네트워크 주소 (DNS - Domain Name System)
            // DNS - 도메인 주소를 IP 주소로 변경해주는 것
            // 모든 웹사이트의 주소를 도메인 대신 IP로 외운다고 생각하면 벌써 머리가 아프다.
            // 서버의 주소가 변경될 수 있으므로 도메인을 사용하는 것이 좋다.
            // IP 버전 (IPv4, IPv6)
            // 2. TCP, UDP 중 어떠한 것을 사용할 것인가?
            // 3. TCP

            try
            {
                // 문지기 교육
                listenSocket.Bind(endPoint);

                // 영업 시작
                // backlog - 최대 대기수, 초과 시 접속 불가(fail)
                listenSocket.Listen(10);


                // 접속 할 때 까지 대기하기 위한 while
                while (true)
                {
                    Console.WriteLine("Listening...");

                    // 손님을 입장시킨다
                    Socket clientSocket = listenSocket.Accept();

                    // 손님의 하고 싶은 말을 듣는다.

                    // 얼마나 많은 데이터를 받을 지 모르기에 큰 배열로 설정한다.
                    byte[] recvBuff = new byte[1024];
                    // clientSocket.Receive를 통해 recvBuff에 실제 정보가 얼마나 담겼는지 확인한다.
                    int recvBytes = clientSocket.Receive(recvBuff);
                    // 문자열을 이용할 것이므로 Encoding 한다.
                    string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
                    Console.WriteLine($"[From Client] {recvData}");

                    // 보낸다.

                    // 환영 메시지를 보낸다.
                    byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to MMORPG Server!");
                    clientSocket.Send(sendBuff);

                    // 쫓아낸다.
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}