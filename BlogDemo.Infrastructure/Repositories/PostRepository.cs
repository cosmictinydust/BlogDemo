using System;
using System.Collections.Generic;
using System.Text;
using BlogDemo.Core.Interfaces;
using BlogDemo.Core.Entities;
using BlogDemo.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlogDemo.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private MyContext _myContext;
        public PostRepository(MyContext myContext)
        {
            _myContext = myContext;
        }
        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _myContext.Posts.ToArrayAsync();
        }
        
        public void AddPost(Post post)
        {
            _myContext.Posts.Add(post);
        }
    }
}
