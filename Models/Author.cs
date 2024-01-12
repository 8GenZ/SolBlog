using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolBlog.Models
{
    public class Author
    {
        //primary key
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Subjects { get; set; }

        public string? Bio { get; set; }

        [Required]
        public string? Email { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public byte[]? ImageData { get; set; }
        public string? ImageType { get; set; }

        public virtual ICollection<BlogPost>? BlogPosts { get; set; }



    }
}
