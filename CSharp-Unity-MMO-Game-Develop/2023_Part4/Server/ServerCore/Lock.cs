using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    class Lock
    {
        const int EMPTY_FLAG = 0x00000000;
        const int WRITE_MASK = 0x7FFF0000;
        const int READ_MASK = 0x0000FFFF;
        const int MAX_SPIN_COUNT = 5000;

        // 시작 값을 0으로 맞춰준다.
        int _flag = EMPTY_FLAG;
        // 재귀적으로 몇 개의 Write를 관리하기 위한 변수
        int _writeCount = 0;

        public void WriteLock()
        {
            // 동일 스레드가 WriteLock을 이미 획득하고 있는지 확인한다.
            int lockThreadId = (_flag & WRITE_MASK) >> 16;
            if (lockThreadId == Thread.CurrentThread.ManagedThreadId)
            {
                // 이미 획득하고 있으면 _writeCount를 증가시키고 return
                _writeCount++;
                return;
            }

            // desired = 내가 원하는 값
            int desired = (Thread.CurrentThread.ManagedThreadId << 16) & WRITE_MASK;

            // 스핀 락 구조
            while (true)
            {
                for (int i = 0; i < MAX_SPIN_COUNT; i++)
                {
                    // 시도해서 성공하면 return;
                    if (_flag == EMPTY_FLAG)
                    {
                        // _flag = desired;
                        // 근데 사실은 안됌 ^^!
                        // 값을 집어 넣는 행위가 원자적이지 않기 때문!

                        // 어떻게 해야할까? InterLocked을 사용한다
                        if (Interlocked.CompareExchange(ref _flag, desired, EMPTY_FLAG) == EMPTY_FLAG)
                        {
                            _writeCount = 1;
                            return;
                        }
                    }
                }

                Thread.Yield();
            }
        }

        public void WriteUnlock()
        {
            // 무조건적으로 값을 바꿔주면 안된다.
            // 내가 WriteCount를 늘린 만큼, 회수해줘야 한다.
            // 즉. 짝을 맞춰주는 행위이다.
            int lockCount = --_writeCount;
            if (lockCount == 0)
                Interlocked.Exchange(ref _flag, EMPTY_FLAG);
        }

        public void ReadLock()
        {
            // 동일 쓰레드가 WriteLock을 이미 획득하고 있는지 확인한다.
            int lockThreadId = (_flag & WRITE_MASK) >> 16;
            if (lockThreadId == Thread.CurrentThread.ManagedThreadId)
            {
                // ReadCount를 늘려준다.
                Interlocked.Increment(ref _flag);
                return;
            }

            while (true)
            {
                for (int i = 0; i < MAX_SPIN_COUNT; i++)
                {
                    // 아무도 WriteLock을 획득하고 있지 않을 때 ==
                    //if ((_flag & WRITE_MASK) == 0)
                    //{
                    //    _flag = (_flag + 1) & READ_MASK;
                    //    return;
                    //}

                    // 근데 위와 같이 행동하면 안됨. 또 원자성을 보장하고 있지 않기 때문!
                    // 따라서 아래와 같이 수정한다.

                    // expected == 예상하고 있는 값
                    int expected = (_flag & READ_MASK);
                    if (Interlocked.CompareExchange(ref _flag, expected + 1, expected) == expected)
                    {
                        return;
                    }
                }

                Thread.Yield();
            }

        }

        public void ReadUnlock()
        {
            // Decrement에서 1을 빼준다.
            Interlocked.Decrement(ref _flag);
        }
    }
}
