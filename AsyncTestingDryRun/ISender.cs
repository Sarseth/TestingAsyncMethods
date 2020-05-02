using System.Threading;
using System.Threading.Tasks;

namespace AsyncTestingDryRun {
    public interface ISender {
        Task Send(string[] stringList, CancellationToken cancellationToken);
    }
}