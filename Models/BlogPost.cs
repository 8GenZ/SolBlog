using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolBlog.Models
{
    public class BlogPost
    {
        private DateTime _created;
        private DateTime? _updated;

        //Primary Key
        public int Id { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and max {1} character long", MinimumLength = 2)]
        public string? Title { get; set; }

        [StringLength(200, ErrorMessage = "The {0} must be at most {1} character long")]
        public string? Abstract{ get; set; }

        [Required]
        public string? Content { get; set; }

        
        public DateTime Created 
        {
            get  { return _created; }
            set { _created = value.ToUniversalTime(); }  
        }

        public DateTime? Updated 
        {
            get => _updated;
            set 
            { if (value.HasValue)
                {
                    _updated = value.Value.ToUniversalTime();
                }
                else
                {
                    _updated = null;
                }
            } 
        }

        [Required]
        public string? Slug { get; set; }

        [Display(Name ="Published?")]
        public bool IsPublished { get; set; }

        [Display(Name ="Deleted")]
        public bool IsDeleted { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public byte[]? ImageData { get; set; }
        public string? ImageType { get; set; }

        //Navigation Properties
        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }

         public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

         public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
    }
}
