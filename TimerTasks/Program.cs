using Quartz;
using Quartz.Impl;
using Quartz.Logging;
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
            //CancellationTokenSource ct = new CancellationTokenSource();
            ////等待按下任意一个键取消任务
            //TaskFactory fac = new TaskFactory();
            //Task[] tasks = new Task[] {
            //    fac.StartNew(()=>Add(ct.Token)),
            //    fac.StartNew(()=>Add(ct.Token)),
            //    fac.StartNew(()=>Add(ct.Token))

            //};
            ////CancellationToken.None指示TasksEnded不能被取消
            //fac.ContinueWhenAll(tasks, TaskEndedAll, CancellationToken.None);
            //Console.ReadKey();
            //ct.Cancel();
            #endregion

            QuartzJobs();
            Console.ReadKey();
           
        }

        private async   static void QuartzJobs()
        {
            //// Grab the Scheduler instance from the Factory
            //StdSchedulerFactory factory = new StdSchedulerFactory();
            //IScheduler scheduler = await factory.GetScheduler( );

            //// and start it off
            //await scheduler.Start();

            //// some sleep to show what's happening
            //await Task.Delay(TimeSpan.FromSeconds(3));

            //// and last shut down the scheduler when you are ready to close your program
            //await scheduler.Shutdown();

            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

            // Grab the Scheduler instance from the Factory
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();

            // and start it off
            await scheduler.Start();

            // define the job and tie it to our HelloJob class Job任务
            IJobDetail job = JobBuilder.Create<HelloJob>()
                .WithIdentity("job1", "group1")
                .Build();

            // Trigger the job to run now, and then repeat every 10 seconds 触发器
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                //.WithCronSchedule("") // 使用Corn表达式来实现间隔时间
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)
                    .RepeatForever())
                .Build();

            // Tell quartz to schedule the job using our trigger 作业调度器
            await scheduler.ScheduleJob(job, trigger);

            // some sleep to show what's happening
            await Task.Delay(TimeSpan.FromSeconds(60));

            // and last shut down the scheduler when you are ready to close your program
            await scheduler.Shutdown();

            Console.WriteLine("Press any key to close the application");

        }



        // simple log provider to get something to the console
        private class ConsoleLogProvider : ILogProvider
        {
            public Logger GetLogger(string name)
            {
                return (level, func, exception, parameters) =>
                {
                    if (level >= LogLevel.Info && func != null)
                    {
                        Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
                    }
                    return true;
                };
            }

            public IDisposable OpenNestedContext(string message)
            {
                throw new NotImplementedException();
            }

            public IDisposable OpenMappedContext(string key, string value)
            {
                throw new NotImplementedException();
            }

            public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
            {
                throw new NotImplementedException();
            }
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
