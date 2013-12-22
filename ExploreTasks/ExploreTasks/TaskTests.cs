using System;
using System.Linq;
using System.Diagnostics;
using System.Threading;
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

        [Fact]
        public async void TaskDelayDoesNotDelayOutsideOfATask()
        {
            var millisecondDelay = 100;

            var stopwatch = Stopwatch.StartNew();
            await Task.Delay(millisecondDelay);
            stopwatch.Stop();

            Assert.NotEqual(0, stopwatch.ElapsedMilliseconds);
            Assert.True(stopwatch.ElapsedMilliseconds < millisecondDelay, "Expected elapsed time to take longer than the delay");
        }

        [Fact]
        // http://msdn.microsoft.com/en-us/library/hh228607(v=vs.110).aspx
        public async void CanCreateATaskFromPreComputedValue()
        {
            var test = await Task.FromResult(true);

            Assert.Equal(true, test);
        }

        [Fact]
        public void WaitAllWaitsUntilAllTasksAreComplete()
        {
            var stopwatch = Stopwatch.StartNew();
            var tasks = new Task[]
            {
                Task.Factory.StartNew(() => Task.Delay(500)),
                Task.Factory.StartNew(() => Task.Delay(500)),
                Task.Factory.StartNew(() => Task.Delay(500)),
                Task.Factory.StartNew(() => Task.Delay(500))
            };
            stopwatch.Stop();
            Task.WaitAll(tasks);

            Assert.True(tasks.All(x => x.IsCompleted));
            Assert.True(stopwatch.ElapsedMilliseconds < 510);
        }

        [Fact]
        public void WhenAnyWaitsUntilFirstTaskIsComplete()
        {
            var stopwatch = Stopwatch.StartNew();
            var tasks = new Task[]
            {
                // TODO - Surprised this didn't work with Task.Factory.StartNew and Task.Delay(ms), figure out why
                Task.Run(() => Thread.Sleep(1)),
                Task.Run(() => Thread.Sleep(15000)),
                Task.Run(() => Thread.Sleep(1000)),
                Task.Run(() => Thread.Sleep(1000))
            };
            stopwatch.Stop();
            Task.WhenAny(tasks);

            Thread.Sleep(100);
            
            Assert.True(tasks.Any(x => x.Status == TaskStatus.Running || x.Status == TaskStatus.WaitingToRun), "Expected at least one task to be running");
            Assert.True(tasks.Any(x => x.IsCompleted), "Expected at least one task to be complete");
        }
    }
}
