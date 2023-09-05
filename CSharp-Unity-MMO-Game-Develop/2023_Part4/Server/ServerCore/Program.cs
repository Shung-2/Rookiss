using System;
using System.Threading;

namespace ServerCore
{
    class Program
    {
        static void Main(string[] args)
        {
            // 캐시가 정말 잘 작동하는가? 확인을 위한 테스트
            // 5*5라고 가정해보자
            int[,] arr = new int[10000, 10000];

            // 2개의 테스트

            // 순차적으로 접근, 캐시에서는 지역성 법칙 때문에 인접 주소를 접근하겠구나~ 싶어서 들고있다
            // 캐시에 이미 있으므로 캐시 히트 상황이라 빠르게 접근할 수 있다
            // [] [] [] [] [] [] [] [] [] [] [] [] [] [] [] [] [] [] [] [] [] [] [] [] [] 

            // 시간 측정
            {
                long now = DateTime.Now.Ticks;
                for (int i = 0; i < 10000; i++)
                    for (int j = 0; j < 10000; j++)
                        arr[i, j] = 1;
                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(i, j 순서 걸린 시간 {end - now}");
            }


            // [] [] [] [] []
            // [] [] [] [] []
            // [] [] [] [] []
            // [] [] [] [] []
            // [] [] [] [] [] 
            // ↑ 방향으로 접근하여 상대적으로 느리다. 따라서 공간 지역성이 떨어진다. 캐시를 활용할 수 없는 상황.
            // → 방향이 아니라 
            {
                long now = DateTime.Now.Ticks;
                for (int i = 0; i < 10000; i++)
                    for (int j = 0; j < 10000; j++)
                        arr[j, i] = 1;
                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(j, i 순서 걸린 시간 {end - now}");
            }



            // 따라서 멀티스레드가 아닌환경에서도 캐시는 잘 작동하며,
            // 멀티 쓰레드로 넘어갈수록 이 상황은 더욱 끔찍해진다 ^^..
        }
    }
}