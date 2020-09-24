using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TimerTasks
{
	public class HelloJob : IJob
	{
		public async Task Execute(IJobExecutionContext context)
		{
			await Console.Out.WriteLineAsync("Greetings from HelloJob!");
		}
	}
}
