using Ozzy.Core.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ozzy.Core.Tests
{
    public class StartStopManagerTest
    {
        [Fact]
        public void BasicStartStopManager()
        {
            int startCalls = 0;
            int startExceptions = 0;
            int finishCalls = 0;
            int finishExceptions = 0;
            int checkThreadCount = 100;

            var manager = new StartStopManager(() => {
                Thread.Sleep(100);
                Interlocked.Increment(ref startCalls);
            }, () =>
            {
                Thread.Sleep(100);
                Interlocked.Increment(ref finishCalls);
            });            

           Parallel.ForEach(new int[checkThreadCount], (x) => {
                try
                {
                    manager.Start();
                }
               catch
               {
                   Interlocked.Increment(ref startExceptions);
               }
            });

            Parallel.ForEach(new int[checkThreadCount], (x) => {
                try
                {
                    manager.Stop();
                }
                catch
                {
                    Interlocked.Increment(ref finishExceptions);
                }
            });

            Assert.Equal(startCalls, 1);
            Assert.Equal(finishCalls, 1);
            Assert.Equal(startExceptions, checkThreadCount - 1);
            Assert.Equal(finishExceptions, checkThreadCount - 1);
        }
    }
}
