using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using SharedKernel.Queries;

namespace SharedKernel.Infrastructure
{
    public interface IQueryDispatcher<TEntity>
    {
        void RegisterHandler<TQuery>(Func<TQuery, Task<List<TEntity>>> handler) where TQuery : BaseQuery;

        Task<List<TEntity>> SendAsync(BaseQuery query);
    }
}