using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTestingDryRun {
    public interface IAsyncDelayer {
        Task Delay(TimeSpan timeSpan, CancellationToken cancellationToken);
    }
}