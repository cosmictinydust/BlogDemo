using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogDemo.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using BlogDemo.Core.Interfaces;
using BlogDemo.Infrastructure.Repositories;

namespace BlogDemo.api.Controllers
{
    [Route("api/posts")]
    public class PostController:Controller
    {
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var posts = await _postRepository.GetAllPostsAsync();
            return Ok(posts);
        }
    }
}
