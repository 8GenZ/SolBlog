using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolBlog.Models;

namespace SolBlog.Services.Interfaces
{
    public interface IBlogService
    {
        public Task AddBlogPostAsync(BlogPost? blogPost);

        public Task UpdateBlogPostAsync(BlogPost? blogPost);

        public Task<IEnumerable<BlogPost>> GetBlogPostsAsync();

        public Task<BlogPost> GetBlogPostAsync(int? id);

        public Task<BlogPost> GetBlogPostAsync(string? slug);

        public Task DeleteBlogPostAsync(int? id);

        public Task<IEnumerable<Category>> GetCategoriesAsync(int? count = null);

        public Task<IEnumerable<BlogPost>> GetBlogPostsByCategoryAsync(int? categoryId);

        public Task<IEnumerable<BlogPost>> GetPopularBlogPostsAsync(int? count = null);

        public Task<IEnumerable<Tag>> GetTagsAsync();

        public Task AddTagsToBlogPostAsync(IEnumerable<string>? tags, int? blogPostId);

        public Task RemoveAllBlogPostTagsAsync(int? blogPostId);

        public Task<bool> IsTagOnBlogPostAsync(int? tagId, int? blogPostId);

        public IEnumerable<BlogPost> SearchBlogPosts(string? searchstring);

        public Task<bool> ValidSlugAsync(string? title, int? blogPostId);


    }
}
