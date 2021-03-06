﻿using System;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using BlogDemo.Infrastructure.Extensions;
using BlogDemo.Infrastructure.Services;

namespace BlogDemo.api.Controllers
{
    [Route("api/posts")]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PostController> _logger;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IPropertyMappingContainer _propertyMappingContainer;

        public PostController(
            IPostRepository postRepository,
            IUnitOfWork unitOfWork,
            ILogger<PostController> logger,
            IMapper mapper,
            IUrlHelper urlHelper,
            ITypeHelperService typeHelperService,
            IPropertyMappingContainer propertyMappingContainer)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
            _propertyMappingContainer = propertyMappingContainer;
        }
        [HttpGet(Name = "GetPosts")]
        public async Task<IActionResult> Get(PostParameters postParameters)
        {
            if (!_typeHelperService.TypeHasProperties<PostResource>(postParameters.Fields))
            {
                return BadRequest("Fields not exist.");
            }

            if (!_propertyMappingContainer.ValidateMappingExistsFor<PostResource, Post>(postParameters.OrderBy))
            {
                return BadRequest("Can't finds fields for sorting.");
            }

            var postList = await _postRepository.GetAllPostsAsync(postParameters);

            //_logger.LogError("这里是 Get all posts...");
            //throw new Exception("Error........!!!!!");
            var postResources = _mapper.Map<IEnumerable<Post>, IEnumerable<PostResource>>(postList);

            var shapedPostResources = postResources.ToDynamicIEnumerable(postParameters.Fields);

            var previousPageLink = postList.HasPrevious ? CreatePostUri(postParameters, PaginationResourceUriType.PreviousPage) : null;
            var nextPageLink = postList.HasNext ? CreatePostUri(postParameters, PaginationResourceUriType.NextPage) : null;

            var meta = new
            {
                PageSize = postList.PageSize,
                PageIndex = postList.PageIndex,
                TotalItemCount = postList.TotalItemCount,
                PageCount = postList.PageCount,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("X-pagination", JsonConvert.SerializeObject(meta, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));

            return Ok(shapedPostResources);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id,string fields)
        {

            if (!_typeHelperService.TypeHasProperties<PostResource>(fields))
            {
                return BadRequest("Fields not exist.");
            }

            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            var postResources = _mapper.Map<Post, PostResource>(post);
            var shapedPostResources = postResources.ToDynamic(fields);
            return Ok(postResources);
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

        private string CreatePostUri(PostParameters parameters, PaginationResourceUriType uriType)
        {
            switch (uriType)
            {
                case PaginationResourceUriType.PreviousPage:
                    var previousParameters = new
                    {
                        pageIndex = parameters.PageIndex - 1,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields
                    };
                    return _urlHelper.Link("GetPosts", previousParameters);
                case PaginationResourceUriType.NextPage:
                    var nextParameters = new
                    {
                        pageIndex = parameters.PageIndex + 1,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields
                    };
                    return _urlHelper.Link("GetPosts", nextParameters);
                default:
                    var currentParameters = new
                    {
                        pageIndex = parameters.PageIndex,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields
                    };
                    return _urlHelper.Link("GetPosts", currentParameters);
            }
        }

    }
}
