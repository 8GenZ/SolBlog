using System.ComponentModel.DataAnnotations;

namespace SolBlog.Models
{
    public class Comment
    {
        private DateTime _created;
        private DateTime? _updated;
       
        //primary key
        public int Id { get; set; }

        public DateTime Created
        {
            get { return _created; }
            set { _created = value.ToUniversalTime(); }
        }

        public DateTime? Updated
        {
            get => _updated;
            set
            {
                if (value.HasValue)
                {
                    _updated = value.Value.ToUniversalTime();
                }
                else
                {
                    _updated = null;
                }
            }
        }

        [Display(Name ="Update Reason")]
        public string? UpdateReason { get; set; }

        [Required]
        [Display(Name ="Comment")]
        [StringLength(5000, ErrorMessage = "The {0} must be at most {1} character long")]
        public string? Body { get; set;}

        [Required]
        public string? AuthorId { get; set; }

        public int BlogPostId { get; set; }

        //Navigational Properties
        public virtual BlogUser? Author { get; set; }

        public virtual BlogPost? BlogPost { get; set; }
    }
    
}
