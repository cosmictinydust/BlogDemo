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
        public async Task<IEnumerable<Post>> GetAllPostsAsync(PostParameters postParameters)
        {
            var query = _myContext.Posts.OrderBy(x => x.Id);
            return await query
                .Skip(postParameters.PageIndex * postParameters.PageSize)
                .Take(postParameters.PageSize)
                .ToListAsync();
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
