using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    public class SendBufferHelper
    {
        // 전역변수처럼 사용할 수 있지만, 나만의 스레드에서만 고유하게 사용할 수 있는 ThreadLocal을 사용
        public static ThreadLocal<SendBuffer> CurrentBuffer = new ThreadLocal<SendBuffer>(() => { return null; });

        public static int ChunkSize { get; set; } = 65535 * 100;

        public static ArraySegment<byte> Open(int reserveSize)
        {
            // 한번도 사용하지 않았으므로 SendBuffer를 생성합니다.
            if (CurrentBuffer.Value == null)
            {
                CurrentBuffer.Value = new SendBuffer(ChunkSize);
            }

            // 기존에 있는 Chunk를 날려버린 다음 새로운 아이를 생성합니다.
            if (CurrentBuffer.Value.FreeSize < reserveSize)
            { 
                CurrentBuffer.Value = new SendBuffer(ChunkSize);
            }

            // 공간이 남아있으므로, Open 메서드를 이용합니다.
            return CurrentBuffer.Value.Open(reserveSize);
        }

        public static ArraySegment<byte> Close(int usedSize)
        {
            return CurrentBuffer.Value.Close(usedSize);
        }
    }

    public class SendBuffer
    {
        byte[] _buffer;
        int _usedSize = 0;

        public int FreeSize { get { return _buffer.Length - _usedSize; } }

        public SendBuffer(int chunkSize)
        {
            _buffer = new byte[chunkSize];
        }

        // 사이즈를 얼마만큼 사용할 것인가?
        public ArraySegment<byte> Open(int reserveSize)
        {
            if (reserveSize > FreeSize)
            {
                return null;
            }

            return new ArraySegment<byte>(_buffer, _usedSize, reserveSize);
        }

        // 버퍼를 다 썼다고 반환하는 개념
        public ArraySegment<byte> Close(int usedSize)
        {
            ArraySegment<byte> segment = new ArraySegment<byte>(_buffer, _usedSize, usedSize);
            _usedSize = usedSize;
            return segment;
        }
    }
}
