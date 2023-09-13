using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace CSharp
{
    class SpinLock
    {
        // 상태
        // true = 사용하는 중(잠금)
        // false = 사용하지 않는 중(미잠금)
        //volatile bool _locked = false;

        // 0 = 사용하지 않는 중(미잠금)
        // 1 = 사용하는 중(잠금)
        volatile int _locked = 0;


        // 획득
        public void Acquire()
        {
            //while (_locked)
            //{
            //}

            //_locked = true;

            // int original = Interlocked.Exchange(ref _locked, 1);

            //while (true)
            //{
            //    int original = Interlocked.Exchange(ref _locked, 1);
            //    if (original == 0)
            //        break;
            //}

            //{
            //    int original = _locked;
            //    _locked = 1;
            //    if (original == 0)
            //        break;
            //}

            //{
            //    if (_locked == 0)
            //        _locked = 1;
            //}

            while (true)
            {
                // CAS Compare-And-Swap 라고 한다.
                //    if (_locked == 0)
                //        _locked = 1;
                // 위와 같다.


                //int original = Interlocked.CompareExchange(ref _locked, 1, 0);
                //if (original == 0)
                //    break;

                // 뭘 비교하는지 ㅈㄴ 헷갈린다
                // C++에서는 expected, desired를 이용하는데
                // 이를 착안해서 사용하면 조금 더 코드를 읽기 쉽다.

                // expected = 예상 하는 값이 무엇이냐
                // desired = 내가 원하는 값은 무엇이냐
                int expected = 0;
                int desired = 1;
                if (Interlocked.CompareExchange(ref _locked, desired, expected) == expected)
                    break;

                // 근데 expected, desired를 사용하면 원자성 보장이 되나?
                // CAS 류 함수랑 사용하면 또 문제가 없다고 한다.
                // 알다가도 모르겠네 시팔.

            }
        }

        // 반환
        public void Release()
        {
            _locked = 0;
        }
    }

    class Program
    {
        static int _num = 0;
        static SpinLock _lock = new SpinLock();

        static void Thread_1()
        {
            for (int i = 0; i < 100000; i++)
            {
                _lock.Acquire();
                _num++;
                _lock.Release();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 100000; i++)
            {
                _lock.Acquire();
                _num--;
                _lock.Release();
            }
        }

        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);

            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);
            Console.WriteLine(_num);
        }
    }
}