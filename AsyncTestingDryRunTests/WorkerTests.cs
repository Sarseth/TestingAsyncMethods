using System;
using System.Threading;
using System.Threading.Tasks;
using AsyncTestingDryRun;
using Moq;
using Xunit;

namespace AsyncTestingDryRunTests {
    public class WorkerTests {

        [Fact]
        public async Task ShouldCallDelayWith5MinutesWhenNotEnoughEvents() {
            // given
            CancellationTokenSource cts = new CancellationTokenSource();
            Mock<ISender> senderMock = new Mock<ISender>();
            Mock<IAsyncDelayer> asyncDelayerMock = new Mock<IAsyncDelayer>();
            asyncDelayerMock
                .Setup(_ => _.Delay(TimeSpan.FromMinutes(5), It.IsAny<CancellationToken>()))
                .Callback(() => {
                    cts.Cancel();
                });
            Worker worker = new Worker(senderMock.Object, asyncDelayerMock.Object);

            // when
            for (int i = 0; i < 99; i++) {
                worker.AddToQueue("Event");
            }
            await worker.Run(cts.Token);

            // then
            asyncDelayerMock.Verify(_ => _.Delay(TimeSpan.FromMinutes(5), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}