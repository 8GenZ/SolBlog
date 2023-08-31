using SolBlog.Enums;

namespace SolBlog.Services.Interfaces
{
    public interface IImageService
    {
        public Task<byte[]> ConvertFileToByteArrayAsync(IFormFile? file);

        public string? ConvertByteArrayToFile(byte[] fileData, string? extension, DefaultImages defaultImage);
    }
}
