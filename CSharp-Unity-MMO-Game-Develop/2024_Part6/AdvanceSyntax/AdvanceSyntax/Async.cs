using System;

namespace AdvanceSyntax
{
    // async/await
    // async 이름만 봐도.. 비동기 프로그래밍
    // 게임서버를 수강했다면 비동기 = 멀티쓰레드? → 꼭 그렇지는 않습니다.
    // 유니티) Coroutine = 일종의 비동기, 그러나 100% 싱글쓰레드

    //class Async
    //{
    //    // 1. Task 기초
    //    //static Task Test()
    //    //{
    //    //    Console.WriteLine("Start Test");
    //    //    Task t = Task.Delay(3000);
    //    //    return t;
    //    //}

    //    // 2. Async 기초
    //    //static void TestAsync()
    //    //{
    //    //    Console.WriteLine("Start TestAsync");
    //    //    Task t = Task.Delay(3000);
    //    //    t.Wait();
    //    //    Console.WriteLine("End TestAsync");
    //    //}

    //    // Async 기초의 문제점
    //    //static void TestAsync2()
    //    //{
    //    //    Console.WriteLine("Start TestAsync");
    //    //    Task t = Task.Delay(3000); // 복잡한 작업 (ex. DB/파일 작업)일 경우 꼼짝없이 기다려야 하므로, 시스템이 먹통이 될 수 있다.
    //    //    t.Wait();
    //    //    Console.WriteLine("End TestAsync");
    //    //}

    //    // 3. Async 기초 문제점 해결
    //    //static async void TestAsync3()
    //    //{
    //    //    Console.WriteLine("Start TestAsync");
    //    //    // await을 통해 주도권 전달(=비동기 처리)을 할 수있도록 처리하낟.
    //    //    Task t = Task.Delay(3000);
    //    //    await t;
    //    //    // await Task.Delay(3000); 위 두 줄을 한 줄로 줄일 수 있다.
    //    //    Console.WriteLine("End TestAsync");
    //    //}

    //    // 4. 값을 반환하는 Async
    //    //static async Task<int> TestAsync4()
    //    //{
    //    //    Console.WriteLine("Start TestAsync");
    //    //    await Task.Delay(3000);
    //    //    Console.WriteLine("End TestAsync");
    //    //    return 100;
    //    //}

    //    // 아이스 아메리카노를 제조중 (1분)
    //    // 주문 대기 상태
    //    //static async Task<int> TestAsync()
    //    //{
    //    //    Console.WriteLine("Start TestAsync");
    //    //    // Task t = Task.Delay(3000); // 복잡한 작업 (ex. DB나 파일 작업)

    //    //    // 1.
    //    //    // t.Wait();
    //    //    // 2.
    //    //    // await t; // Main문으로 제어권을 넘겨준다. (비동기)
    //    //    // 3. 
    //    //    await Task.Delay(3000); // 비동기로 만들어준다. (비동기)

    //    //    // 작업 스레드
    //    //    Console.WriteLine("End TestAsync");
    //    //    return 1;
    //    //}

    //    //static async Task Main(string[] args)
    //    //{
    //    //    // 1. Task 기초
    //    //    // Task t = Test();
    //    //    // t.Wait();

    //    //    // 2. Async 기초
    //    //    // TestAsync();

    //    //    // 3. Async 기초 문제점 해결
    //    //    // TestAsync3();

    //    //    // 4. 값을 반환하는 Async
    //    //    int ret = await TestAsync4();

    //    //    Console.WriteLine("while start");
    //    //    Console.WriteLine(ret);

    //    //    // 주 스레드
    //    //    while (true)
    //    //    {

    //    //    }
    //    //}
    //}
}