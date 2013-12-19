﻿using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ExploreTasks
{
    public class CancellationTokenTests
    {
        [Fact]
        // http://stackoverflow.com/questions/3712939/cancellation-token-in-task-constructor-why
        public async void CancellationTokenIsMonitored()
        {
            var token = new CancellationTokenSource();
            token.Cancel();

            var result = await Task.Run(() =>
            {
                var count = 0;

                while (true)
                {
                    if (token.IsCancellationRequested)
                        break;

                    count++;
                }

                return count;
            }, token.Token);

            Assert.Equal(0, result);
        }

        [Fact]
        public async void CancellationTokenStopsRequestAfterStarting()
        {
            var token = new CancellationTokenSource();
            
            var result = await Task.Run(() =>
            {
                var count = 0;

                while (true)
                {
                    if (token.IsCancellationRequested)
                        break;

                    count++;
                }

                return count;
            }, token.Token);
            
            token.Cancel();

            Assert.True(result > 0, "Should have cancelled after the method already started");
        }

        [Fact]
        // http://stackoverflow.com/questions/3712939/cancellation-token-in-task-constructor-why
        public async void CancellingBeforeInvocationPreventsActionExecution()
        {
            var count = -10;
            var token = new CancellationTokenSource();
            
            var task = new Task<int>(() =>
            {
                count = 0;

                while (true)
                {
                    if (token.IsCancellationRequested)
                        break;

                    count++;
                }

                return count;
            }, token.Token);
            
            token.Cancel();

            if (!task.IsCanceled)
                await Task.Run(() => task); ;

            Assert.Equal(-10, count);
        }
    }
}