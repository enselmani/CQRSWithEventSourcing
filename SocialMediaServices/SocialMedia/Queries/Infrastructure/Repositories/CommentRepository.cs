using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public CommentRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateAsync(Comment comment)
        {
            using (DatabaseContext context = _contextFactory.CreateDbContext())
            {
                context.Comments.Add(comment);
                _ = await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid commentId)
        {
            using (DatabaseContext context = _contextFactory.CreateDbContext())
            {
                Comment entity = await context.Comments.FindAsync(commentId);

                if (entity is null)
                {
                    return;
                }

                context.Comments.Remove(entity);
                _ = await context.SaveChangesAsync();
            }
        }

        public async Task<Comment> GetByIdAsync(Guid commentId)
        {
            using (DatabaseContext context = _contextFactory.CreateDbContext())
            {
                return await context.Comments.FirstOrDefaultAsync(x => x.CommentId == commentId);
            }
        }

        public async Task UpdateAsync(Comment comment)
        {
            using (DatabaseContext context = _contextFactory.CreateDbContext())
            {
                context.Comments.Update(comment);
                _ = await context.SaveChangesAsync();
            }
        }
    }
}