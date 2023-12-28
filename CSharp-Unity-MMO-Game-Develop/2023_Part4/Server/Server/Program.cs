using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;

namespace Server
{
	class Program
	{
		static Listener _listener = new Listener();
		public static GameRoom Room = new GameRoom();

		static void FlushRoom()
		{
            Room.Push(() => Room.Flush());
			// FlushRoom 함수를 250ms 마다 호출하도록 예약
            JobTimer.Instance.Push(FlushRoom, 250);
        }

		static void Main(string[] args)
		{
			// DNS (Domain Name System)
			string host = Dns.GetHostName();
			IPHostEntry ipHost = Dns.GetHostEntry(host);
			IPAddress ipAddr = ipHost.AddressList[0];
			IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

			_listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
			Console.WriteLine("Listening...");

			// FlushRoom();
			// 카운트를 0으로 설정하여 바로 실행되도록 한다.
			JobTimer.Instance.Push(FlushRoom);

			while (true)
			{
				JobTimer.Instance.Flush();
			}
		}
	}
}
