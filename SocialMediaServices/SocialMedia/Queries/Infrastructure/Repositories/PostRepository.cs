using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public PostRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateAsync(Post post)
        {
            using (DatabaseContext context = _contextFactory.CreateDbContext())
            {
                context.Posts.Add(post);
                _ = await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid postId)
        {
            using (DatabaseContext context = _contextFactory.CreateDbContext())
            {
                Post entity = await context.Posts.FindAsync(postId);

                if (entity is null)
                {
                    return;
                }

                context.Posts.Remove(entity);
                _ = await context.SaveChangesAsync();
            }
        }

        public async Task<Post> GetByIdAsync(Guid postId)
        {
            using (DatabaseContext context = _contextFactory.CreateDbContext())
            {
                Post entity = await context.Posts
                    .Include(x => x.Comments)
                    .FirstOrDefaultAsync(x => x.PostId == postId);

                return entity;
            }
        }

        public async Task<List<Post>> ListAllAsync()
        {
            using (DatabaseContext context = _contextFactory.CreateDbContext())
            {
                List<Post> entities = await context.Posts.AsNoTracking()
                    .Include(x => x.Comments).AsNoTracking()
                    .ToListAsync();

                return entities;
            }
        }

        public async Task<List<Post>> ListByAuthorAsync(string author)
        {
            using (DatabaseContext context = _contextFactory.CreateDbContext())
            {
                List<Post> entities = await context.Posts.AsNoTracking()
                    .Include(x => x.Comments).AsNoTracking()
                    .Where(x => x.Author.Contains(author))
                    .ToListAsync();

                return entities;
            }
        }

        public async Task<List<Post>> ListByCommentsAsync()
        {
            using (DatabaseContext context = _contextFactory.CreateDbContext())
            {
                List<Post> entities = await context.Posts.AsNoTracking()
                    .Include(x => x.Comments).AsNoTracking()
                    .Where(x => x.Comments != null && x.Comments.Any())
                    .ToListAsync();

                return entities;
            }
        }

        public async Task<List<Post>> ListWithLikesAsync(int numberOfLikes)
        {
            using (DatabaseContext context = _contextFactory.CreateDbContext())
            {
                List<Post> entities = await context.Posts.AsNoTracking()
                    .Include(x => x.Comments).AsNoTracking()
                    .Where(x => x.Likes >= numberOfLikes)
                    .ToListAsync();

                return entities;
            }
        }

        public async Task UpdateAsync(Post post)
        {
            using (DatabaseContext context = _contextFactory.CreateDbContext())
            {
                context.Posts.Update(post);
                _ = await context.SaveChangesAsync();
            }
        }
    }
}