using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTestingDryRun {
    public class Worker {
        private const int THRESHOLD = 100;

        private readonly ConcurrentQueue<string> queue;
        private readonly ISender sender;
        private readonly IAsyncDelayer asyncDelayer;

        public Worker(ISender sender, IAsyncDelayer asyncDelayer) {
            this.sender = sender;
            this.asyncDelayer = asyncDelayer;
            queue = new ConcurrentQueue<string>();
        }

        public Task Run(CancellationToken cancellationToken) {
            return Task.Factory.StartNew(() =>
                AnalyticsSenderTask(cancellationToken), TaskCreationOptions.LongRunning);
        }

        public void AddToQueue(string newString) {
            queue.Enqueue(newString);
        }

        private async Task AnalyticsSenderTask(CancellationToken cancellationToken) {
            while (!cancellationToken.IsCancellationRequested) {
                await asyncDelayer.Delay(TimeSpan.FromSeconds(30), cancellationToken);
                if (queue.Count < THRESHOLD) {
                    await asyncDelayer.Delay(TimeSpan.FromMinutes(5), cancellationToken);
                } else {
                    SendHundred(cancellationToken);
                }
            }
        }

        private void SendHundred(CancellationToken cancellationToken) {
            string[] stringBatch = new string[THRESHOLD];
            int i = 0;
            while (i < THRESHOLD && queue.TryDequeue(out string stringToSend)) {
                stringBatch[i++] = stringToSend;
            }
            sender.Send(stringBatch, cancellationToken);
        }
    }
}