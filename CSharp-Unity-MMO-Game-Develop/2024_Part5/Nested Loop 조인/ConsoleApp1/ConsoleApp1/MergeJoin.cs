using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ConsoleApp1
{
    class Player : IComparable<Player>
    {
        public int playerId;

        public int CompareTo(Player other)
        {
            if (playerId == other.playerId)
                return 0;
            return (playerId > other.playerId) ? 1 : -1;
        }
    }

    class Salary : IComparable<Salary>
    {
        public int playerId;

        public int CompareTo(Salary other)
        {
            if (playerId == other.playerId)
                return 0;
            return (playerId > other.playerId) ? 1 : -1;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Player> players = new List<Player>();
            players.Add(new Player() { playerId = 0 });
            players.Add(new Player() { playerId = 9 });
            players.Add(new Player() { playerId = 1 });
            players.Add(new Player() { playerId = 3 });
            players.Add(new Player() { playerId = 4 });

            List<Salary> salaries = new List<Salary>();
            salaries.Add(new Salary() { playerId = 0 });
            salaries.Add(new Salary() { playerId = 5 });
            salaries.Add(new Salary() { playerId = 0 });
            salaries.Add(new Salary() { playerId = 2 });
            salaries.Add(new Salary() { playerId = 9 });

            // 1단계) Sort (이미 정렬되어 있으면 Skip)
            // O(nlogn)
            players.Sort();
            salaries.Sort();

            // One-To-Many (players는 중복이 없다.)
            // 2단계) Merge
            // 어떤식으로 합쳐줘야 할까?
            // outer - [ 0, 1, 3, 4, 9 ] → N
            // inner - [ 0, 0, 2, 5, 9 ] → M

            int p = 0;
            int s = 0;

            // Merge Sort 의사코드 구현
            // O(N+M)
            List<int> result = new List<int>();
            while (p < players.Count && s < salaries.Count)
            {
                if (players[p].playerId == salaries[s].playerId)
                {
                    Console.WriteLine($"playerId: {players[p].playerId}");
                    result.Add(players[p].playerId); // 성공!
                    s++;
                }
                else if (players[p].playerId < salaries[s].playerId)
                {
                    p++;
                }
                else
                {
                    s++;
                }
            }

            // Many-To-Many (players는 중복이 있다.)
            // outer - [ 0, 0, 0, 0, 0 ] → N
            // inner - [ 0, 0, 0, 0, 0 ] → M
            // O(N * M)
            // 물론 그렇다고 Many-To-Mnay가 나쁜 것은 아니지만 위 예는 최악중의 최악인 상황임.
        }
    }
}