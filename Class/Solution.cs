using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CancelTasks.Class
{
    class Solution
    {
        public static string ErrorFormat(int whichOne) {
         return   $"Thread {whichOne} is canceled!!";
        }
        /// <summary>
        /// 
        ///This example shows how to terminate a Task and its children in response to a cancellation request.
        ///It also shows that when a user delegate terminates by throwing a TaskCanceledException, 
        ///the calling thread can optionally use the Wait method or WaitAll method to wait for the tasks to finish. 
        ///In this case, you must use a try/catch block to handle the exceptions on the calling thread.
        /// </summary>
        public static void Execute()
        {
            var threadCount = Environment.ProcessorCount+1;
            var tasks = new Task[threadCount];
            var tokenSource = new System.Threading.CancellationTokenSource();
            var cToken = tokenSource.Token;
            var looperThd= new LooperThreading(tokenSource, cToken);
          
            for (int taskNumber = 0; taskNumber < threadCount; taskNumber++)
            {

                tasks[taskNumber] = Task.Factory.StartNew(
                    () =>
                    {
                        looperThd.Execute();
                    }, cToken);
                Console.WriteLine("Task {0} executing", tasks[taskNumber].Id);
            }
           
            try
            {
                Task.WaitAll(tasks);
            }
            catch (AggregateException aEx)
            {
               
                foreach (var ae in aEx.InnerExceptions)
                {
                    if (ae is TaskCanceledException)
                        Console.WriteLine($"TaskCanceledException:  { ErrorFormat(((TaskCanceledException)ae).Task.Id)}");
                    else
                        Console.WriteLine($"Exception: { ae.GetType().Name}");
                }
               
            }
            finally {
                if (looperThd.TokenSource!=null)
                {
                    looperThd.TokenSource.Dispose();
                }
            }
            GetAllTaskStatus(tasks);


        }
        public static void GetAllTaskStatus(Task[] tasks)
        {
            foreach (var task in tasks)
                Console.WriteLine($"Task { task.Id} status is now { task.Status}");
        }
    }
}
