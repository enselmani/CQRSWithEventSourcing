using SharedKernel.Queries;
using System;

namespace WebAPI.Queries
{
    public class FindPostByIdQuery : BaseQuery
    {
        public Guid Id { get; set; }
    }
}