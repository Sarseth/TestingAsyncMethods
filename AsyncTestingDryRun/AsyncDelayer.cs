using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTestingDryRun {
    public class AsyncDelayer : IAsyncDelayer {
        public async Task Delay(TimeSpan timeSpan, CancellationToken cancellationToken) {
            await Task.Delay(timeSpan, cancellationToken);
        }
    }
}