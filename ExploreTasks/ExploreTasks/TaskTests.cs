using System.Threading.Tasks;
using System.Linq;
using Xunit;
using System;

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

            Console.WriteLine(currentTaskId);
            Assert.True(currentTaskId != null, "Expecting the task to have an ID");
            Assert.True(currentTaskId > 0, "Expecting the task to have a positive integer ID");
        }
    }
}
