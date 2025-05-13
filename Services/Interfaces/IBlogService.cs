using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolBlog.Models;

namespace SolBlog.Services.Interfaces
{
    public interface IBlogService
    {
        #region BlogPost
        public Task AddBlogPostAsync(BlogPost? blogPost);
        public Task UpdateBlogPostAsync(BlogPost? blogPost);
        public Task<IEnumerable<BlogPost>> GetBlogPostsAsync();
        public Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync();
        public Task<BlogPost> GetBlogPostAsync(int? id);
        public Task<BlogPost> GetBlogPostAsync(string? slug);
        public Task DeleteBlogPostAsync(int? id);
        public Task<IEnumerable<BlogPost>> GetPopularBlogPostsAsync(int? count = null);
        public Task<IEnumerable<BlogPost>> GetBlogPostsByCategoryAsync(int? categoryId);
        public IEnumerable<BlogPost> SearchBlogPosts(string? searchstring);
        #endregion

        #region Category
        public Task<IEnumerable<Category>> GetSortedCategoriesAsync(int? count = null);
        public Task<List<Category>> GetCategoriesAsync(int? count = null);

        #endregion

        public Task<IEnumerable<Tag>> GetTagsAsync();
        public Task AddTagsToBlogPostAsync(IEnumerable<string>? tags, int? blogPostId);
        public Task RemoveAllBlogPostTagsAsync(int? blogPostId);
        public Task<bool> IsTagOnBlogPostAsync(int? tagId, int? blogPostId);
        public Task<bool> ValidSlugAsync(string? title, int? blogPostId);





    }
}
