using System;
using System.Collections.Generic;
using System.Text;
using BlogDemo.Core.Interfaces;
using BlogDemo.Core.Entities;
using BlogDemo.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace BlogDemo.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private MyContext _myContext;
        public PostRepository(MyContext myContext)
        {
            _myContext = myContext;
        }
        public async Task<PaginatedList<Post>> GetAllPostsAsync(PostParameters postParameters)
        {
            var query = _myContext.Posts.AsQueryable();
                if (!string.IsNullOrEmpty(postParameters.Title))
                {
                    var title = postParameters.Title.ToLowerInvariant();
                    query = query.Where(x => x.Title.ToLowerInvariant()==title);
                }

                query=query.OrderBy(x => x.Id);

            var count = await query.CountAsync();

            var data = await query
                .Skip(postParameters.PageIndex * postParameters.PageSize)
                .Take(postParameters.PageSize)
                .ToListAsync();
            return new PaginatedList<Post>(postParameters.PageIndex, postParameters.PageSize, count, data);
        }
    

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _myContext.Posts.FindAsync(id);
        }


        public void AddPost(Post post)
        {
            _myContext.Posts.Add(post);
        }
    }
}
