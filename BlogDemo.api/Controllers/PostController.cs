using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogDemo.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using BlogDemo.Core.Interfaces;
using BlogDemo.Infrastructure.Repositories;
using BlogDemo.Core.Entities;
using Microsoft.Extensions.Logging;

namespace BlogDemo.api.Controllers
{
    [Route("api/posts")]
    public class PostController:Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PostController> _logger;

        public PostController(IPostRepository postRepository,IUnitOfWork unitOfWork,ILogger<PostController> logger)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var posts = await _postRepository.GetAllPostsAsync();
            //_logger.LogError("这里是 Get all posts...");
            throw new Exception("Error........!!!!!");
            return Ok(posts);
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var newpost = new Post
            {
                Title = "Admin",
                Body = "这是一个内容",
                Author = "无名",
                LastModified = DateTime.Now
            };
            _postRepository.AddPost(newpost);
            await _unitOfWork.SaveAsync();
            return Ok();
        }
    }
}
