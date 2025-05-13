using Humanizer;
using Microsoft.EntityFrameworkCore;
using SolBlog.Data;
using SolBlog.Models;
using SolBlog.Services.Interfaces;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Text;

namespace SolBlog.Services
{
    public class BlogService : IBlogService
    {

        private readonly ApplicationDbContext _context;

        public BlogService(ApplicationDbContext context)
        {
            _context = context;
        }
        #region BlogPost
        public async Task AddBlogPostAsync(BlogPost? blogPost)
        {
            if (blogPost == null) { return; }

            try
            {
                _context.Add(blogPost);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task UpdateBlogPostAsync(BlogPost? blogPost)
        {
            if (blogPost == null) { return; }
            try
            {
                _context.Update(blogPost);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<BlogPost>> GetBlogPostsAsync()
        {
            try
            {

                IEnumerable<BlogPost> blogPosts = await _context.BlogPosts.Include(b => b.Category)
                                                                                        .Where(b => b.IsPublished == true && b.IsDeleted == false)
                                                                                        .Include(b => b.Comments)
                                                                                        .ThenInclude(c => c.Author)
                                                                                        .Include(b => b.Tags)
                                                                                        .OrderByDescending(b => b.Created)
                                                                                        .ToListAsync();
                return blogPosts;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync()
        {
            try
            {

                IEnumerable<BlogPost> blogPosts = await _context.BlogPosts.Include(b => b.Category)
                                                                                        .Include(b => b.Comments)
                                                                                        .ThenInclude(c => c.Author)
                                                                                            .OrderByDescending(b => b.Created)
                                                                                            .ToListAsync();               

                return blogPosts;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<BlogPost> GetBlogPostAsync(int? id)
        {
            if (id == null) { return new BlogPost(); }
            try
            {

                BlogPost? blogPost = await _context.BlogPosts
                .Include(b => b.Category)
                .Include(b => b.Comments)
                .ThenInclude(c => c.Author)
                .FirstOrDefaultAsync(m => m.Id == id);

                return blogPost!;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<BlogPost> GetBlogPostAsync(string? slug)
        {
            if (string.IsNullOrEmpty(slug)) { return new BlogPost(); }
            try
            {

                BlogPost? blogPost = await _context.BlogPosts
                .Include(b => b.Category)
                .Include(b => b.Comments)
                .ThenInclude(c => c.Author)
                .Include(b => b.Tags)
                .FirstOrDefaultAsync(m => m.Slug == slug);

                return blogPost!;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Task DeleteBlogPostAsync(int? id)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<BlogPost>> GetBlogPostsByCategoryAsync(int? categoryId)
        {
            try
            {
                IEnumerable<BlogPost> blogPosts = new List<BlogPost>();


                if (categoryId == null)
                {
                    return blogPosts;
                }
                else
                {
                    blogPosts = await _context.BlogPosts.Where(b => b.CategoryId == categoryId)
                                         .Include(b => b.Category)
                                         .Include(b => b.Tags)
                                         .Include(b => b.Comments)
                                            .ThenInclude(c => c.Author)
                                         .OrderByDescending(b => b.Created)
                                         .ToListAsync();
                    return blogPosts;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task RemoveAllBlogPostTagsAsync(int? blogPostId)
        {
            try
            {
                BlogPost? blogPost = await _context.BlogPosts.Include(b => b.Tags).FirstOrDefaultAsync(b => b.Id == blogPostId);

                if (blogPost != null)
                {
                    blogPost.Tags.Clear();
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IEnumerable<BlogPost> SearchBlogPosts(string? searchstring)
        {
            try
            {
                IEnumerable<BlogPost> blogPosts = new List<BlogPost>();

                if (string.IsNullOrEmpty(searchstring)) { return blogPosts; }
                else
                {
                    searchstring = searchstring.Trim().ToLower();

                    blogPosts = _context.BlogPosts.Where(b => b.Title!.ToLower().Contains(searchstring) ||
                                                         b.Abstract!.ToLower().Contains(searchstring) ||
                                                         b.Content!.ToLower().Contains(searchstring) ||
                                                         b.Category!.Name!.ToLower().Contains(searchstring) ||
                                                         b.Comments.Any(c => c.Body!.ToLower().Contains(searchstring) ||
                                                            c.Author!.FirstName!.ToLower().Contains(searchstring) ||
                                                            c.Author!.LastName!.ToLower().Contains(searchstring)) ||
                                                         b.Tags.Any(t => t.Name!.ToLower().Contains(searchstring)))
                                                   .Include(b => b.Category)
                                                   .Include(b => b.Comments)
                                                   .ThenInclude(c => c.Author)
                                                   .Include(b => b.Tags)
                                                   .Where(b => b.IsPublished == true && b.IsDeleted == false)
                                                   .AsNoTracking()
                                                   .OrderByDescending(b => b.Created)
                                                   .AsEnumerable();
                    return (blogPosts);
                }


            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<BlogPost>> GetPopularBlogPostsAsync(int? count = null)
        {
            try
            {
                //gets the blog posts and their properties
                IEnumerable<BlogPost> blogPosts = await _context.BlogPosts.Include(b => b.Category)
                                                                                        .Where(b => b.IsPublished == true && b.IsDeleted == false)
                                                                                        .Include(b => b.Comments)
                                                                                        .ThenInclude(c => c.Author)
                                                                                        .Include(b => b.Tags)
                                                                                        .ToListAsync();
                if (count == null)
                {
                    //orders by the amount of comments
                    blogPosts = blogPosts.OrderByDescending(b => b.Comments.Count);
                }
                else
                {
                    blogPosts = blogPosts.OrderByDescending(b => b.Comments.Count).Take(count!.Value);
                }
                return blogPosts;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Category
        public async Task<IEnumerable<Category>> GetSortedCategoriesAsync(int? count = null)
        {
            try
            {
                IEnumerable<Category> categories = await _context.Categories.ToListAsync();

                categories = categories.OrderBy(c => c.Name).Take(count.Value);

                return categories;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<List<Category>> GetCategoriesAsync(int? count = null)
        {
            try
            {
                List<Category> categories = await _context.Categories.ToListAsync();

                return categories;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Category> GetCategoryAsync(int? id)
        {
            if (id == null) { return new Category(); }
            try
            {
                Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
                return category!;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task AddCategoryAsync(Category? category)
        {
            try
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task DeleteCategoryAsync(int? id)
        {
            var category = await _context.Categories.FindAsync(id);
            try
            {
                _context.Remove(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

            #region Tags
        public async Task<IEnumerable<Tag>> GetTagsAsync()
        {
            try
            {
                IEnumerable<Tag> tags = await _context.Tags.ToListAsync();
                return tags;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task AddTagsToBlogPostAsync(IEnumerable<string>? tags, int? blogPostId)
        {
            if (blogPostId == null || tags == null) { return; }

            try
            {
                //Gets the blog post to assign tag to 
                BlogPost? blogPost = await _context.BlogPosts.Include(b => b.Tags).FirstOrDefaultAsync(b => b.Id == blogPostId);

                //Check if Blog post dosent exist
                if (blogPost == null) { return; }

                foreach (string tagName in tags)
                {
                    if (string.IsNullOrEmpty(tagName.Trim())) continue;

                    Tag? tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name!.Trim().ToLower() == tagName.Trim().ToLower());

                    //If Tag dosent exists
                    if (tag == null)
                    {
                        //Create new tag and add to database
                        tag = new Tag() { Name = tagName.Trim().Titleize() };
                        await _context.AddAsync(tag);

                    }
                    //adds the tag to the blogpost
                    blogPost.Tags.Add(tag);
                }
                //updates to database
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> IsTagOnBlogPostAsync(int? tagId, int? blogPostId)
        {
            if (tagId == null || blogPostId == null) { return false; }

            try
            {
                Tag? tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == tagId);
                BlogPost? blogPost = await _context.BlogPosts.Include(b => b.Tags).FirstOrDefaultAsync(b => b.Id == blogPostId);

                if (blogPost != null && tag != null)
                {
                    return blogPost.Tags.Contains(tag);
                }

                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        public async Task<bool> ValidSlugAsync(string? title, int? blogPostId)
        {
            try
            {
                //NewPost
                if (blogPostId == null || blogPostId == 0)
                {
                    return !await _context.BlogPosts.AnyAsync(b => b.Slug == title);
                }
                else
                {
                    BlogPost? blogPost = await _context.BlogPosts.AsNoTracking().FirstOrDefaultAsync(b => b.Id == blogPostId);

                    string? oldSlug = blogPost?.Slug;

                    if (!string.Equals(oldSlug, title))
                    {
                        return !await _context.BlogPosts.AnyAsync(b => b.Id != blogPost!.Id && b.Slug == title);
                    }

                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }









    }

}