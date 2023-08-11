using System;

namespace Exercise
{
    class Program
    {
        class Graph
        {
            int[,] adj = new int[6, 6]
            {
                { 0, 1, 0, 1, 0, 0 },
                { 1, 0, 1, 1, 0, 0 },
                { 0, 1, 0, 0, 0, 0 },
                { 1, 1, 0, 0, 1, 0 },
                { 0, 0, 0, 1, 0, 1 },
                { 0, 0, 0, 0, 1, 0 },
            };

            List<int>[] adj2 = new List<int>[]
            {
                new List<int>() { 1, 3 },
                new List<int>() { 0, 2, 3 },
                new List<int>() { 1 },
                new List<int>() { 0, 1, 4 },
                new List<int>() { 3, 5 },
                new List<int>() { 4 },
            };

            bool[] visited = new bool[6];
            // 1) 우선 now부터 방문하고,
            // 2) now와 연결된 정점들을 하나씩 확인해서, [아직 미방문 상태라면] 방문한다.
            public void DFS(int now) // 2차원 배열 버전
            {
                Console.WriteLine(now);
                visited[now] = true;

                for(int next = 0; next < 6; next++)
                {
                    if (adj[now, next] == 0) continue; // 연결되어 있지 않으면 스킵
                    if (visited[next]) continue; // 이미 방문했으면 스킵
                    DFS(next);
                }
            }

            public void DFS2(int now) // 리스트 버전
            {
                Console.WriteLine(now);
                visited[now] = true;

                foreach (int next in adj2[now])
                {
                    if (visited[next]) continue;
                    DFS2(next);
                }
            }

            public void SearchAll()
            {
                visited = new bool[6];
                for (int now = 0; now < 6; now++)
                {
                    if (visited[now] == false)
                        DFS(now);
                }
            }

            public void Dijikstra(int start)
            {
                bool[] visited = new bool[6];
                int[] distance = new int[6];
                int[] parent = new int[6];
                Array.Fill(distance, int.MaxValue);

                distance[start] = 0;
                parent[start] = start;

                while (true)
                {
                    // 제일 좋은 후보를 찾는다 (가장 가까이에 있는)

                    // 가장 유력한 후보의 거리와 번호를 저장한다.
                    int closet = int.MaxValue;
                    int now = -1;
                    for (int i = 0; i < 6; i++)
                    {
                        // 이미 방문한 정점은 스킵
                        if (visited[i]) continue;

                        // 아직 발견된 적이 없거나, 기존 후보보다 지금 후보가 더 가까우면, now와 closet을 업데이트한다.
                        if (distance[i] < closet)
                        {
                            now = i;
                            closet = distance[i];
                        }
                    }

                    // 다음 후보가 없으면 종료한다.
                    if (now == -1) break; 

                    // 제일 좋은 후보를 찾았으니까 방문한다.
                    visited[now] = true;

                    // 방문한 정점과 인접한 정점들을 조사해서, 상황에 따라 발견한 최단거리를 업데이트한다.
                    for (int next = 0; next < 6; next++)
                    {
                        // 연결되지 않은 정점 스킵
                        if (adj[now, next] == 0) continue;

                        // 이미 방문한 정점 스킵
                        if (visited[next]) continue;

                        // 새로 조사된 정점의 최단거리를 계산한다.
                        int nextDist = distance[now] + adj[now, next];

                        // 만약에 기존에 발견한 최단거리가 새로 조사된 최단거리보다 크면, 값을 업데이트한다.
                        if (nextDist < distance[next])
                        {
                            distance[next] = nextDist;
                            parent[next] = now;
                        }
                    }
                }
            }
        }

        #region TreeNode 구현
        class TreeNode<T>
        {
            public T Data { get; set; }
            public List<TreeNode<T>> Children { get; set; } = new List<TreeNode<T>>();
        }

        static TreeNode<string> MakeTree()
        {
            TreeNode<string> root = new TreeNode<string>() { Data = "R1 개발실" };
            {
                {
                    TreeNode<string> node = new TreeNode<string>() { Data = "디자인팀" };
                    node.Children.Add(new TreeNode<string>() { Data = "전투" });
                    node.Children.Add(new TreeNode<string>() { Data = "경제" });
                    node.Children.Add(new TreeNode<string>() { Data = "스토리" });
                    root.Children.Add(node);
                }
                {
                    TreeNode<string> node = new TreeNode<string>() { Data = "프로그래밍팀" };
                    node.Children.Add(new TreeNode<string>() { Data = "서버" });
                    node.Children.Add(new TreeNode<string>() { Data = "클라" });
                    node.Children.Add(new TreeNode<string>() { Data = "엔진" });
                    root.Children.Add(node);
                }
                {
                    TreeNode<string> node = new TreeNode<string>() { Data = "아트팀" };
                    node.Children.Add(new TreeNode<string>() { Data = "배경" });
                    node.Children.Add(new TreeNode<string>() { Data = "캐릭터" });
                    root.Children.Add(node);
                }
            }

            return root;
        }

        static void PrintTree(TreeNode<string> root)
        {
            // 접근
            Console.WriteLine(root.Data);

            // 재귀 호출
            foreach (TreeNode<string> child in root.Children)
                PrintTree(child);
        }

        static int GetHeight(TreeNode<string> root)
        {
            // 높이 설정
            int height = 0;

            // 재귀 호출
            foreach (TreeNode<string> child in root.Children)
            {
                int newHeight = GetHeight(child) + 1;
                //if (height < newHeight)
                //    height = newHeight;
                height = Math.Max(height, newHeight);
            }

            return height;
        }
        #endregion

        #region 우선순위 큐 구현
        class PriorityQueue<T> where T : IComparable<T>
        {
            List<T> _heap = new List<T>();

            // 0(logN)
            public void Push(T data)
            {
                // 힙의 맨 끝에 새로운 데이터를 삽입한다.
                _heap.Add(data);

                int now = _heap.Count - 1;
                // 도장깨기를 시작한다.
                while (now > 0)
                {
                    // 도장깨기를 시도한다.
                    int next = (now - 1) / 2; // 부모 노드 접근
                    if (_heap[now].CompareTo(_heap[next]) < 0) // 부모보다 강한가?
                        break;

                    // 두 값을 교체한다.
                    T temp = _heap[now];
                    _heap[now] = _heap[next];
                    _heap[next] = temp;

                    // 검사 위치를 이동한다.
                    now = next;
                }
            }

            // 0(logN)
            public T Pop()
            {
                // 반환할 데이터를 따로 저장한다
                T ret = _heap[0];

                // 마지막 데이터를 루트로 이동한다.
                int lastIndex = _heap.Count - 1;
                _heap[0] = _heap[lastIndex];
                _heap.RemoveAt(lastIndex);
                lastIndex--;

                // 역으로 내려가는 도장깨기 시작
                int now = 0;
                while (true)
                {
                    int left = 2 * now + 1;
                    int right = 2 * now + 2;

                    int next = now;
                    // 왼쪽값이 현재값보다 크면, 왼쪽으로 이동
                    if (left <= lastIndex && _heap[next].CompareTo(_heap[left]) < 0)
                        next = left;
                    // 오른쪽값이 현재값(왼쪽 이동 포함)보다 크면, 오른쪽으로 이동
                    if (right <= lastIndex && _heap[next].CompareTo(_heap[right]) < 0)
                        next = right;

                    // 왼쪽/오른쪽 모두 현재값보다 작으면 종료
                    if (next == now)
                        break;

                    // 두 값을 교체한다.
                    T temp = _heap[now];
                    _heap[now] = _heap[next];
                    _heap[next] = temp;

                    // 검사 위치를 이동한다.
                    now = next;
                }

                return ret;
            }

            public int Count()
            {
                return _heap.Count;
            }
        }

        class Knight : IComparable<Knight>
        {
            public int Id { get; set; }

            public int CompareTo(Knight other)
            {
                if (Id == other.Id)
                    return 0;
                return Id > other.Id ? 1 : -1;
            }
        }
        #endregion

        static void Main(string[] args)
        {
            #region 그래프 초기화
            Graph graph = new Graph();

            // DFS 테스트
            // graph.DFS(0);
            // graph.DFS2(0);
            // graph.SearchAll(); // 정점이 끊어져 있어도 방문하도록.

            // BFS 테스트
            #endregion

            #region 트리노드 초기화
            TreeNode<string> root = MakeTree();
            // PrintTree(root);
            // Console.WriteLine(GetHeight(root));
            #endregion

            #region 우선순위 큐 구현
            PriorityQueue<Knight> q = new PriorityQueue<Knight>();
            q.Push(new Knight() { Id = 20 });
            q.Push(new Knight() { Id = 30 });
            q.Push(new Knight() { Id = 40 });
            q.Push(new Knight() { Id = 50 });
            q.Push(new Knight() { Id = 60 });

            while (q.Count() > 0)
            {
                Console.WriteLine(q.Pop());
            }

            //q.Push(-20);
            //q.Push(-10);
            //q.Push(-30);
            //q.Push(-90);
            //q.Push(-40);

            //while (q.Count() > 0)
            //{
            //    Console.WriteLine(-q.Pop());
            //}
            #endregion


        }
    }
}