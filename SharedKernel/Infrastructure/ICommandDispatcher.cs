using SharedKernel.Commands;
using System;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure
{
    public interface ICommandDispatcher
    {
        void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand;

        Task SendAsync(BaseCommand command);
    }
}