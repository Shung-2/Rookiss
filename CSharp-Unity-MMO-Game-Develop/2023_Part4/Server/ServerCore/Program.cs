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
        static ReaderWriterLockSlim _lock3 = new ReaderWriterLockSlim();

        class Reward
        {

        }

        static Reward GetRewardById(int id)
        {
            _lock3.EnterReadLock();
            _lock3.ExitReadLock();

            return null;
        }

        static void AddReward(Reward reward)
        {
            _lock3.EnterWriteLock();
            _lock3.ExitWriteLock();
        }

        static void Main(string[] args)
        {
           
        }
    }
}