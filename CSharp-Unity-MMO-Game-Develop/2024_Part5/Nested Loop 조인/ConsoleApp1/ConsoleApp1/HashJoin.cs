using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Player
    {
        public int playerId;
    }

    class Salary
    {
        public int playerId;
    }

    // HashTable을 직접 만들어보자
    // 통이 10개 있다고 가정한다
    // [] [] [] [] [] [] [] [] [] []
    // 특정 숫자 N을 10으로 나눈 나머지의 값을 인덱스로 사용한다
    // [0] [1] [2] [3] [4] [5] [6] [7] [8] [9]
    // ex) 41 % 10 = 1 (Key) → [1]에는 41이 들어간다.
    // ex) 51 % 10 = 1 (Key) → 마찬가지로 [1]에 51이 들어간다.
    // 따라서 일련의 숫자들이 주르륵 저장되어 있을 것이다.

    // 나중에 누군가 '41' 있나요? 할 경우, 키 값을 구해야 한다. 키는 1 (Key)
    // 따라서 1번 상자를 뒤져서 41이 있을 경우 있는것이고, 없으면 없는 것이다.
    // 해시 = 공간을 내주고, 속도를 얻는다

    // 따라서 동일한 값 → 동일한 Bucket (YES)
    // 동일한 Bucket → 동일한 값 (NO)이 된다.
    // 해시는 일방향 소통이다.

    class HashTable
    {
        int _bucketCount;
        List<int>[] _buckets;

        public HashTable(int bucketCount = 100)
        {
            _bucketCount = bucketCount;
            _buckets = new List<int>[bucketCount];

            for (int i = 0; i < bucketCount; i++)
            {
                _buckets[i] = new List<int>();
            }
        }

        public void Add(int value)
        {
            int key = value % _bucketCount;
            _buckets[key].Add(value);
        }

        public bool Find(int value)
        {
            int key = value % _bucketCount;
            foreach (int v in _buckets[key])
            {
                if (v == value)
                    return true;
            }

            return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();

            List<Player> players = new List<Player>();
            for (int i = 0; i < 10000; i++)
            {
                if (rand.Next(0, 2) == 0)
                    continue;

                players.Add(new Player() { playerId = i });
            }

            List<Salary> salaries = new List<Salary>();
            for (int i = 0; i < 10000; i++)
            {
                if (rand.Next(0, 2) == 0)
                    continue;

                salaries.Add(new Salary() { playerId = i });
            }

            // 해시는 해시테이블을 이용해서 사용하는 방법이다.
            // 딕셔너리가 바로 해시테이블이다
            Dictionary<int, Salary> hash = new Dictionary<int, Salary>();
            foreach (Salary s in salaries)
            {
                hash.Add(s.playerId, s);
            }

            List<int> result = new List<int>();
            foreach (Player p in players)
            {
                if (hash.ContainsKey(p.playerId))
                {
                    result.Add(p.playerId);
                }
            }

            // NL이랑 다른 점.
            // NL은 아우터와 이너의 개념이 존재해서 양쪽 모두를 순회하면서 찾지만 해시조인은 그렇지 않다.
            // NL은 애초에 Dictionary를 만들지 않는다.

            // 직접 만든 해시 테이블을 이용해보자.
            HashTable hashtable = new HashTable();
            foreach (Salary s in salaries)
            {
                hashtable.Add(s.playerId);
            }

            List<int> result2 = new List<int>();
            foreach (Player p in players)
            {
                if (hashtable.Find(p.playerId))
                {
                    result2.Add(p.playerId);
                }
            }
        }
    }
}
