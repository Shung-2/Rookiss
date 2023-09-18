using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace ServerCore
{
    class Program
    {
        // 아래와 같이 매핑하여 사용한다.
        // static ThreadLocal<string> ThreadName = new ThreadLocal<string>();
        static ThreadLocal<string> ThreadName = new ThreadLocal<string>(() => { return $"My Name is {Thread.CurrentThread.ManagedThreadId}"; });

        static void WhoAmI()
        {
            // 고유의 영역에서 이름을 설정한다.
            //ThreadName.Value = $"My Name is {Thread.CurrentThread.ManagedThreadId}";
            //Thread.Sleep(1000);
            //Console.WriteLine(ThreadName.Value);

            bool repeat = ThreadName.IsValueCreated;
            if (repeat)
                Console.WriteLine(ThreadName.Value + " repeat ");
            else
                Console.WriteLine(ThreadName.Value);
        }

        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(3, 3);
            Parallel.Invoke(WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI);

            // 날려준다.
            ThreadName.Dispose();
        }
    }
}