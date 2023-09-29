using SolBlog.Enums;
using SolBlog.Services.Interfaces;

namespace SolBlog.Services
{

    public class ImageService : IImageService
    {
        private readonly string? _defaultBlogImage = "/img/DefaultBlogImage.jpg";
        private readonly string? _defaultUserImage = "/img/DefaultUserImage.png";
        private readonly string? _defaultCategoryImage = "/img/DefaultCategoryImage.png";
        private readonly string? _authorImage = "/img/Author-Image.png";
        public string? ConvertByteArrayToFile(byte[]? fileData, string? extension, DefaultImages defaultImage)
        {

            try
            {
                if (fileData == null || fileData.Length == 0)
                {
                    switch (defaultImage)
                    {
                        case DefaultImages.AuthorImage: return _authorImage;
                        case DefaultImages.BlogPostImage: return _defaultBlogImage;
                        case DefaultImages.UserImage: return _defaultUserImage;
                        case DefaultImages.CategoryImage: return _defaultCategoryImage;
                    }
                    return _defaultUserImage;
                }
                string? imageBase64Data = Convert.ToBase64String(fileData!);
                imageBase64Data = string.Format($"data:{extension};base64,{imageBase64Data}");

                return imageBase64Data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile? file)
        {
            try
            {
                using MemoryStream memoryStream = new MemoryStream();
                await file!.CopyToAsync(memoryStream);
                byte[] byteFile = memoryStream.ToArray();
                memoryStream.Close();
                return byteFile;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
