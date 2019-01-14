using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InterviewExam_Thread.Class
{
     class LooperThreading : Looper
    {
        public LooperThreading(CancellationTokenSource tokenSource, CancellationToken token)
        {
            this.TokenSource = tokenSource;
            this.ct = token;
        }

        /// <summary>
        /// 從Execute方法中的迴圈跳出
        /// </summary>
        public override void IfTriggerCancel()
        {
            StopOtherLoopers();
        }
        public CancellationTokenSource TokenSource { get; set; }
        public CancellationToken ct { get; set; }
        /// <summary>
        /// 請求取消(用來觸發TriggerCancel方法)
        /// </summary>
        public override void RequestCancel()
        {
          
            if (ct.IsCancellationRequested)
            {
                //Console.WriteLine("ThrowIfCancellationRequested");
              ct.ThrowIfCancellationRequested();
            }
         
        }

        /// <summary>
        /// 停止其他執行中的Looper
        /// </summary>
        public override void StopOtherLoopers()
        {
            TokenSource.Cancel();
          
        }
    }
}
