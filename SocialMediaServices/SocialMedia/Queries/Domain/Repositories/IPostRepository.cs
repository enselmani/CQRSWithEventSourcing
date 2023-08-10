using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IPostRepository
    {
        Task CreateAsync(Post post);

        Task UpdateAsync(Post post);

        Task DeleteAsync(Guid postId);

        Task<Post> GetByIdAsync(Guid postId);

        Task<List<Post>> ListAllAsync();

        Task<List<Post>> ListByAuthorAsync(string author);

        Task<List<Post>> ListWithLikesAsync(int numberOfLikes);

        Task<List<Post>> ListByCommentsAsync();
    }
}