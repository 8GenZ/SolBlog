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

        // GET: api/BlogPosts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPost>>> GetBlogPosts()
        {
            IEnumerable<BlogPost> blogPosts = (await _blogService.GetBlogPostsAsync()).Take(4);

            return Ok(blogPosts);
        }

    }
}
