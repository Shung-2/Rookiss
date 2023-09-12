using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ServerCore
{
    class FastLock
    {
        public int id;
    }

    class SessionManager
    {
        FastLock l;

        static object _lock = new object();

        public static void TestSession()
        {
            lock (_lock)
            {

            }
        }

        public static void Test()
        {
            lock (_lock)
            {
                UserManager.TestUser();
            }
        }
    }

    class UserManager
    {
        FastLock l;

        static object _lock = new object();

        public static void Test()
        {
            lock (_lock)
            {
                SessionManager.TestSession();
            }
        }

        public static void TestUser()
        {
            lock( _lock)
            {

            }
        }
    }

    class Program
    {
        static void Thread_1()
        {
            for (int i = 0; i < 100; i++)
            {
                SessionManager.Test();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 100; i++)
            {
                UserManager.Test();
            }
        }

        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            t1.Start();

            Thread.Sleep(100);

            t2.Start();

            Task.WaitAll(t1, t2);
        }
    }
}