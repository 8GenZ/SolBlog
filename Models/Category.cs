using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolBlog.Models
{
    public class Category
    {
        //Primary Key
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        [StringLength(30, ErrorMessage = "The {0} must be at most {1} character long")]
        public string? Name { get; set; }

        [StringLength(200, ErrorMessage = "The {0} must be at most {1} character long")]
        public string? Description { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public byte[]? ImageData { get; set; }
        public string? ImageType { get; set; }

        public ICollection<BlogPost> blogPosts { get; set; } = new HashSet<BlogPost>();
    }
}