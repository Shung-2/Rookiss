using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvanceSyntax
{
    public enum ClassType
    {
        Knight,
        Archer,
        Mage
    }

    public class Player
    {
        public ClassType ClassType { get; set; }
        public int Level { get; set; }
        public int Hp { get; set; }
        public int Attack { get; set; }
    }

    class LINQ
    {
        static List<Player> _players = new List<Player>();

        static void Main(string[] args)
        {
            Random rand = new Random();

            for (int i = 0; i < 100; i++)
            {
                // 랜덤으로 직업 선택
                ClassType type = ClassType.Knight;
                switch (rand.Next(0, 3))
                {
                    case 0:
                        type = ClassType.Knight;
                        break;
                    case 1:
                        type = ClassType.Archer;
                        break;
                    case 2:
                        type = ClassType.Mage;
                        break;
                }

                // 플레이어 생성
                Player player = new Player()
                {
                    ClassType = type,
                    Level = rand.Next(1, 100),
                    Hp = rand.Next(100, 1000),
                    Attack = rand.Next(10, 100)
                };

                _players.Add(player);
            }

            // Q. 레벨이 50 이상인 Knight만 추려내서, 레벨을 낮음 → 높은 순서로 정렬할 경우
            
            // 일반 버전
            {
                List<Player> players = GetHighLevelKnights();
                foreach (Player p in players)
                {
                    // 레벨과 체력 출력
                    Console.WriteLine($"Level: {p.Level}, HP: {p.Hp}");
                }
            }

            Console.WriteLine("**********");

            // LINQ 버전
            {
                // from (foreach)
                // where (필터 역할 = 조건에 부합하는 데이터만 걸러낸다.)
                // order by (정렬을 수행. 기본적으로 오름차순 ascending / descending)
                // select (최종 결과를 추출 → 가공해서 추출도 가능!)

                var players =
                    from p in _players
                    where p.ClassType == ClassType.Knight && p.Level >= 50
                    orderby p.Level
                    select p;

                foreach (Player p in players)
                {
                    // 레벨과 체력 출력
                    Console.WriteLine($"Level: {p.Level}, HP: {p.Hp}");
                }
            }
        }

        static public List<Player> GetHighLevelKnights()
        {
            List<Player> players = new List<Player>();

            foreach (Player player in _players)
            {
                // 방법 1
                if (player.ClassType != ClassType.Knight)
                    continue;

                if (player.Level < 50)
                    continue;

                players.Add(player);
            }

            players.Sort((lhs, rhs) => { return lhs.Level - rhs.Level; });

            return _players;
        }
    }
}
