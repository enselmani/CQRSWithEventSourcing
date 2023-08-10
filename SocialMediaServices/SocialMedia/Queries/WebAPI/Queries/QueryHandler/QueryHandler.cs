using Domain.Entities;
using Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Queries.QueryHandler
{
    public class QueryHandler : IQueryHandler
    {
        private readonly IPostRepository _postRepository;

        public QueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<List<Post>> HandleAsync(FindAllPostsQuery query)
        {
            return await _postRepository.ListAllAsync();
        }

        public async Task<List<Post>> HandleAsync(FindPostByIdQuery query)
        {
            Post post = await _postRepository.GetByIdAsync(query.Id);
            return new List<Post> { post };
        }

        public async Task<List<Post>> HandleAsync(FindPostsByAuthorQuery query)
        {
            return await _postRepository.ListByAuthorAsync(query.Author);
        }

        public async Task<List<Post>> HandleAsync(FindPostsWithCommentsQuery query)
        {
            return await _postRepository.ListByCommentsAsync();
        }

        public async Task<List<Post>> HandleAsync(FindPostsWithLikesQuery query)
        {
            return await _postRepository.ListWithLikesAsync(query.NumberOfLikes);
        }
    }
}