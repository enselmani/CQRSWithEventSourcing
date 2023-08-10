using Domain.Entities;
using SharedKernel.Infrastructure;
using SharedKernel.Queries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Dispatchers
{
    public class QueryDispatcher : IQueryDispatcher<Post>
    {
        private readonly Dictionary<Type, Func<BaseQuery, Task<List<Post>>>> _handlers = new();

        public void RegisterHandler<TQuery>(Func<TQuery, Task<List<Post>>> handler) where TQuery : BaseQuery
        {
            if (_handlers.ContainsKey(typeof(TQuery)))
            {
                throw new IndexOutOfRangeException("You cannot register the same query handler twice!");
            }

            _handlers.Add(typeof(TQuery), x => handler((TQuery)x));
        }

        public async Task<List<Post>> SendAsync(BaseQuery query)
        {
            if (_handlers.TryGetValue(query.GetType(), out Func<BaseQuery, Task<List<Post>>> handler))
            {
                return await handler(query);
            }

            throw new ArgumentNullException(nameof(handler), "No query handler was registered!");
        }
    }
}