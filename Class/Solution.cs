using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace InterviewExam_Thread.Class
{
    class Solution
    {
        public static string ErrorFormat(int whichOne) {
         return   $"Thread {whichOne} is canceled!!";
        }
        /// <summary>
        /// 請實作LooperThreading類別，不可修改Looper類別。
        /// Solution.Execute()要實作的邏輯是:
        /// 啟動多個執行緒，讓每個執行緒執行LooperThreading的Execute方法。
        /// 任何執行緒出錯後，在Console紀錄出錯Looper的Name，並取消其他執行中的執行緒。
        /// 任何執行緒被取消執行後，在Console紀錄被取消Looper的Name。
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
