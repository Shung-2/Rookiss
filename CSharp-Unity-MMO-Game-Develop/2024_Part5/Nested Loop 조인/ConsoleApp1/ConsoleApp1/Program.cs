using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    #region NestedLoop
    class NL
    {
        class Player
        {
            public int playerId;
            // ...
        }

        class Salary
        {
            public int playerId;
            // ...
        }

        public void NestedLoop()
        {
            Random rand = new Random();

            List<Player> players = new List<Player>();
            for (int i = 0; i < 1000; i++)
            {
                if (rand.Next(0, 2) == 0)
                    continue;

                players.Add(new Player() { playerId = i });
            }

            List<Salary> Lsalaries = new List<Salary>();
            Dictionary<int, Salary> Dsalaries = new Dictionary<int, Salary>();
            for (int i = 0; i < 1000; i++)
            {
                if (rand.Next(0, 2) == 0)
                    continue;

                Lsalaries.Add(new Salary() { playerId = i });
                Dsalaries.Add(i, new Salary() { playerId = i });
            }

            // Q) 어떻게 하면 ID가 players에도 있고, salaries에도 있는 정보를 추출 할 수 있을까?

            // Nested Loop
            // Nested(내포하는) Loop(루프)
            // 시간 복잡도 : O(N^2)
            List<int> result = new List<int>();
            foreach (Player p in players)
            {
                foreach (Salary s in Lsalaries)
                {
                    if (p.playerId == s.playerId)
                    {
                        result.Add(p.playerId);
                        break;
                    }
                }
            }

            // 어떻게 개선을 하면 좋을까?
            // 리스트가 아닌 Dictionary를 사용하면 어떨까?
            // 시간 복잡도 : O(N)
            foreach (Player p in players)
            {
                Salary s = null;
                if (Dsalaries.TryGetValue(p.playerId, out s))
                {
                    result.Add(p.playerId);
                }
            }

            // NL을 사용할 때 유리한 경우가 무엇이 있을까?
            // result를 그냥 아무거나 최대 5개만 찾아줘
            foreach (Player p in players)
            {
                Salary s = null;
                if (Dsalaries.TryGetValue(p.playerId, out s))
                {
                    result.Add(p.playerId);
                    if (result.Count >= 5)
                        break;
                }
            }

            // 결론
            // 집합이 2개가 있을 때 이중 포문을 도는 느낌으로 외부에서 내부를 하나하나씩 서칭하는게 NL의 개념이다.
            // 내부에 있는 집합을 빠르게 찾을 수 있다면 성능이 훨씬 더 좋아진다.
            // 개수 제한이 있을 경우 굉장히 유용하다.
        }
    }
    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            var NL = new NL();
            NL.NestedLoop();
        }
    }
}