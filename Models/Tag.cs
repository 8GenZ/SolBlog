using System.ComponentModel.DataAnnotations;

namespace SolBlog.Models
{
    public class Tag
    {
        //Primary Key
        public int Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at most {1} character long")]
        public string? Name { get; set; }



        public ICollection<BlogPost> blogPosts { get; set; } = new HashSet<BlogPost>();
    }
}
