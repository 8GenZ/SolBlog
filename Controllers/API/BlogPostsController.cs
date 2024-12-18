﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolBlog.Data;
using SolBlog.Models;
using SolBlog.Services.Interfaces;

namespace SolBlog.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
       
        private readonly IBlogService _blogService;

        public BlogPostsController(IBlogService blogService )
        {
            _blogService = blogService;
        }

        /// <summary>
        /// This endpoint will return the most recently published blog posts.
        /// The count parameter indicates the number of recent posts to return
        /// The maximum number of blog posts allowed per-request is 10.
        /// </summary>
        /// <param name="count">The number of blog bosts to retrieve</param>
        /// <returns></returns>


        // GET: api/BlogPosts
        [HttpGet("{count:int}")]
        public async Task<ActionResult<IEnumerable<BlogPost>>> GetBlogPosts(int count)
        {
            if (count > 10)
            {
                count = 10;
            }
            
            IEnumerable<BlogPost> blogPosts = (await _blogService.GetBlogPostsAsync()).Take(count);

            return Ok(blogPosts);
        }

    }
}
