using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class RecvBuffer
    {
        ArraySegment<byte> _buffer;
        int _readPos;
        int _writePos;

        public RecvBuffer(int bufferSize)
        {
            _buffer = new ArraySegment<byte>(new byte[bufferSize], 0, bufferSize);
        }

        // 버퍼의 유효 사이즈. 데이터가 얼마나 쌓였는지를 체크한다.
        public int DataSize { get { return _writePos - _readPos; } }
        // 버퍼의 남은 공간
        public int FreeSize { get { return _buffer.Count - _writePos; } } 

        // 유효 범위의 Segment를 어디부터 데이터를 읽으면 되는가?
        public ArraySegment<byte> ReadSegment
        {
            get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _readPos, DataSize); }
        }

        // 버퍼의 남은 공간 크기
        public ArraySegment<byte> WriteSegment
        {
            get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _writePos, FreeSize); }
        }
        
        // 중간 정리를 하며, R/W 커서를 맨 앞으로 당겨준다.
        public void Clean()
        {
            int dataSize = DataSize;
            if (dataSize == 0) 
            {
                // 남은 데이터가 없어 복사하지 않고 커서 위치만 리셋한다. (=클라이언트에서 보낸 데이트를 모두 처리한 상태)
                _readPos = _writePos = 0;
            }   
            else
            {
                // 남은 데이터가 있으면 시작 위치로 복사한다.
                Array.Copy(_buffer.Array, _buffer.Offset + _readPos, _buffer.Array, _buffer.Offset, dataSize);
                _readPos = 0;
                _writePos = dataSize;
            }
        }

        // 데이터를 성공적으로 처리했을 경우 OnRead를 호출하여 R 커서 이동
        public bool OnRead(int numOfBytes)
        {
            if (numOfBytes > DataSize)
            {
                return false;
            }

            _readPos += numOfBytes;
            return true;
        }

        // 클라이언트에서 데이트럴 보낸 상황에서 성공적으로 처리했을 경우 OnWrite를 호출하여 W 커서 이동
        public bool OnWrite(int numOfBytes)
        {
            if (numOfBytes > FreeSize)
            {
                return false;
            }

            _writePos += numOfBytes;
            return true;
        }
    }
}
