using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ServerCore;

namespace DummyClient
{


    class ServerSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            // 로그
            Console.WriteLine($"OnConnected : {endPoint}");

            PlayerInfoReq packet = new PlayerInfoReq() { playerId = 1001, name = "ABCD" };
            packet.skills.Add(new PlayerInfoReq.SkillInfo() { id = 101, level = 1, duration = 1.0f });
            packet.skills.Add(new PlayerInfoReq.SkillInfo() { id = 201, level = 2, duration = 2.0f });
            packet.skills.Add(new PlayerInfoReq.SkillInfo() { id = 301, level = 3, duration = 3.0f });
            packet.skills.Add(new PlayerInfoReq.SkillInfo() { id = 401, level = 4, duration = 4.0f });

            // 보낸다
            //for (int i = 0; i < 5; i++)
            {
                ArraySegment<byte> s = packet.Write();
                if (s != null)
                {
                    Send(s);
                }
            }
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            // 로그
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override int OnRecv(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[From Server] {recvData}");
            return buffer.Count;
        }

        public override void OnSend(int numOfBytes)
        {
            // 로그
            Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}
