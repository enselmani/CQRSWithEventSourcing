using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Queries.QueryHandler
{
    public interface IQueryHandler
    {
        Task<List<Post>> HandleAsync(FindAllPostsQuery query);

        Task<List<Post>> HandleAsync(FindPostByIdQuery query);

        Task<List<Post>> HandleAsync(FindPostsByAuthorQuery query);

        Task<List<Post>> HandleAsync(FindPostsWithCommentsQuery query);

        Task<List<Post>> HandleAsync(FindPostsWithLikesQuery query);
    }
}