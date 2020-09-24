using System;
using System.Threading;
using System.Threading.Tasks;

namespace TimerTasks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            #region Thread
            //Thread th = new Thread(Do);
            //th.Start();
            //Timer tm = new Timer(new TimerCallback((object c) => Console.WriteLine(DateTime.Now))) ;
            //tm.Change(0, 3000);

            #endregion

            #region Timer
            //System.Timers.Timer tms = new System.Timers.Timer(3000);
            //tms.Elapsed += Tms_Elapsed;
            //tms.AutoReset=true;
            //tms.Enabled = true;
            //tms.Start();

            //ThreadPool.QueueUserWorkItem(new WaitCallback(Do));  
            #endregion

            #region Task
            //Task t = new Task(() =>
            //   {
            //       Console.WriteLine("任务开始。。。");
            //       Thread.Sleep(3000);
            //   });
            //t.Start();
            //t.ContinueWith((t) =>
            //{
            //    Console.WriteLine("任务完成 ");
            //    Console.WriteLine($"任务完成状态: IsCanceled={0},IsCompleted={1},IsFaulted={2}", t.IsCanceled, t.IsCompleted, t.IsFaulted);
            //});

            //CancellationTokenSource cts = new CancellationTokenSource();

            //Task<int> task = new Task<int>(() => Add(cts.Token), cts.Token);
            //task.Start();
            //task.ContinueWith(TaskEnded);
            ////等待按任意键取消任务
            //Console.ReadKey();
            //cts.Cancel();
            #endregion

            #region TaskFactory
            CancellationTokenSource ct = new CancellationTokenSource();
            //等待按下任意一个键取消任务
            TaskFactory fac = new TaskFactory();
            Task[] tasks = new Task[] {
                fac.StartNew(()=>Add(ct.Token)),
                fac.StartNew(()=>Add(ct.Token)),
                fac.StartNew(()=>Add(ct.Token))

            };
            //CancellationToken.None指示TasksEnded不能被取消
            fac.ContinueWhenAll(tasks, TaskEndedAll, CancellationToken.None);
            Console.ReadKey();
            ct.Cancel(); 
            #endregion

            Console.ReadKey();
           
        }
        static void TaskEndedAll(Task[] tasks) {
            Console.WriteLine("所有任务已完成！");
        }
        static void TaskEnded(Task<int> task) {

            Console.WriteLine("任务完成，完成时候的状态为：");
            Console.WriteLine("IsCanceled={0}\tIsCompleted={1}\tIsFaulted={2}",
                            task.IsCanceled, task.IsCompleted, task.IsFaulted);
            Console.WriteLine("任务的返回值为：{0}", task.Result);

        }
        static int Add(CancellationToken cts) {

            int res = 0;
            Console.WriteLine("任务开始。。。");
            if (!cts.IsCancellationRequested) {
                res++;
                Thread.Sleep(3);
            }
            return res;
        
        }

        private static void Tms_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine($"{DateTime.Now} --任务开始-----");
            Thread.Sleep(3000);
            
            Console.WriteLine($"{DateTime.Now} --任务结束-----");
        }

        static void Do(object c) {
            while (true)
            {
                Console.WriteLine(DateTime.Now);
                Thread.Sleep(TimeSpan.FromSeconds(3));
            }
            
        }
    }
}
