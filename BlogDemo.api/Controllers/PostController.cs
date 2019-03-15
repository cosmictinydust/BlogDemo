using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogDemo.Core.Interfaces;
using BlogDemo.Infrastructure.Repositories;
using BlogDemo.Core.Entities;
using Microsoft.Extensions.Logging;
using AutoMapper;
using BlogDemo.Infrastructure.Resource;
using System.Runtime.InteropServices;

namespace BlogDemo.api.Controllers
{
    [Route("api/posts")]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PostController> _logger;
        private readonly IMapper _mapper;

        public PostController(
            IPostRepository postRepository,
            IUnitOfWork unitOfWork,
            ILogger<PostController> logger,
            IMapper mapper)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var posts = await _postRepository.GetAllPostsAsync();
            //_logger.LogError("这里是 Get all posts...");
            //throw new Exception("Error........!!!!!");
            var postResources = _mapper.Map<IEnumerable<Post>, IEnumerable<PostResource>>(posts);
            return Ok(postResources);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            var postResource=_mapper.Map<Post,PostResource> (post);
            return Ok(postResource);
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
