using System;

namespace AdvanceSyntax
{
    // async/await
    // async 이름만 봐도.. 비동기 프로그래밍
    // 게임서버를 수강했다면 비동기 = 멀티쓰레드? → 꼭 그렇지는 않습니다.
    // 유니티) Coroutine = 일종의 비동기, 그러나 100% 싱글쓰레드

    class Program
    {
        static Task Test()
        {
            Console.WriteLine("Start Test");
            Task t = Task.Delay(3000);
            return t;
        }

        // 아이스 아메리카노를 제조중 (1분)
        // 주문 대기 상태
        static async Task<int> TestAsync()
        {
            Console.WriteLine("Start TestAsync");
            // Task t = Task.Delay(3000); // 복잡한 작업 (ex. DB나 파일 작업)

            // 1.
            // t.Wait();
            // 2.
            // await t; // Main문으로 제어권을 넘겨준다. (비동기)
            // 3. 
            await Task.Delay(3000); // 비동기로 만들어준다. (비동기)

            // 작업 스레드
            Console.WriteLine("End TestAsync");
            return 1;
        }

        static async Task Main(string[] args)
        {
            // Task t = Test();
            // t.Wait();

            int ret = await TestAsync();

            Console.WriteLine("while start");
            Console.WriteLine(ret);

            // 주 스레드
            while (true)
            {

            }
        }
    }
}