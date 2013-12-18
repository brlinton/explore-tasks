using System.Threading.Tasks;
using System.Linq;
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
    }
}
