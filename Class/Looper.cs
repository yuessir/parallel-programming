using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CancelTasks.Class
{
     abstract class Looper
    {
        protected static List<Looper> Loopers = new List<Looper>();

        protected static int Seed = Environment.TickCount;
        protected const int Boundery = 10000000;
        protected readonly int Counter;

        public string Name
        {
            get { return Counter.ToString(); }
        }

        protected Looper()
        {
            var random = new Random(Interlocked.Increment(ref Seed));
            Counter = random.Next(Boundery);
            Loopers.Add(this);
        }

        public void Execute()
        {
            for (int i = 0; i < Boundery; i++)
            {
                if (i == Counter)
                {
                    Console.WriteLine("Fail at counter: {0}", Counter);
                    throw new Exception();
                }

                IfTriggerCancel();
            }
        }

        public abstract void IfTriggerCancel();
        public abstract void RequestCancel();
        public abstract void StopOtherLoopers();
    }
}
