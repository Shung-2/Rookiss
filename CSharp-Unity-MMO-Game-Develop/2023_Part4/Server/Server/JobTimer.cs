using System;
using System.Collections.Generic;
using System.Text;
using ServerCore;

namespace Server
{
    struct JobTimerElem : IComparable<JobTimerElem>
    {
        public int execTick; // 실행 시간
        public Action action;

        public int CompareTo(JobTimerElem other)
        {
            return other.execTick - execTick;
        }
    }

    class JobTimer
    {
        PriorityQueue<JobTimerElem> _pq = new PriorityQueue<JobTimerElem>();
        object _lock = new object();

        public static JobTimer Instance { get; } = new JobTimer();

        public void Push(Action action, int tickAfter = 0)
        {
            JobTimerElem job;
            job.execTick = System.Environment.TickCount + tickAfter;
            job.action = action;

            lock (_lock)
            {
                _pq.Push(job);
            }
        }

        // 큐를 비워주는 용도
        public void Flush()
        {
            while (true)
            {
                int now = System.Environment.TickCount;
                JobTimerElem job;

                lock (_lock)
                {
                    // 데이터가 없고, 비어있는 상태
                    if (_pq.Count == 0)
                    {
                        break;
                    }

                    job = _pq.Peek();

                    // 아직 실행할 시간이 아님.
                    if (job.execTick > now)
                        break;

                    _pq.Pop();
                }

                job.action.Invoke();
            }
        }
    }
}
