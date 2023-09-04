using System;
using System.Threading;

namespace ServerCore
{
    class Program
    {
        static void MainThread(object state)
        {
            for (int i = 0; i < 5; i++)
                Console.WriteLine("Hello, Thread!");
        }

        // 메인 직원
        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(5, 5);

            for (int i = 0; i < 5; i++)
            {
                Task ta = new Task(() => { while (true) { } }, TaskCreationOptions.LongRunning);
                ta.Start();
            }

            //for (int i = 0; i < 5; i++)
            //{
            //    ThreadPool.QueueUserWorkItem((obj) => { while (true) { } });
            //}

            ThreadPool.QueueUserWorkItem(MainThread);

            for (int i = 0; i < 1000; i ++)
            {
                // 직원을 한명 더 고용하고, 일을 시킨다.
                Thread t = new Thread(MainThread);
                // 스레드의 이름을 설정한다
                // t.Name = "Test Thread";

                // 스레드를 백그라운드 스레드 또는 포그라운드 스레드로 만들지 결정한다.
                // true = 백그라운드 스레드, false = 포그라운드 스레드
                t.IsBackground = true;
                t.Start();
            }


            Console.WriteLine("Waiting for Thread");
            // 백그라운드 스레드의 종료를 기다린다.
            t.Join();

            //Console.WriteLine("Hello, World!");
            while (true)
            {

            }
        }
    }
}