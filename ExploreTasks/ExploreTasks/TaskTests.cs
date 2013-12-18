using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace ExploreTasks
{
    public class TaskTests
    {
        [Fact]
        public void CurrentIdIsNullWhenNotExecutingATask()
        {
            var currentTaskId = Task.CurrentId;

            // Since I'm not in a task
            Assert.Equal(null, currentTaskId);
        }

        [Fact]
        public async void CurrentIdIsNotNullWhenATaskIsRunning()
        {
            var currentTaskId = await Task.Run(() => Task.CurrentId);

            Assert.True(currentTaskId != null, "Expecting the task to have an ID");
            Assert.True(currentTaskId > 0, "Expecting the task to have a positive integer ID");
        }

        [Fact]
        public async void TaskDelayDelaysTheRunningTask()
        {
            var millisecondDelay = 100;

            var stopwatch = Stopwatch.StartNew();
            await Task.Run(() => { Task.Delay(millisecondDelay); });
            stopwatch.Stop();

            Assert.NotEqual(0, stopwatch.ElapsedMilliseconds);
            Assert.True(stopwatch.ElapsedMilliseconds >= millisecondDelay, "Expected elapsed time to take longer than the delay");
        }
    }
}
