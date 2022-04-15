using BlogLibrary.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogLibrary.Models
{
    public class PostModel
    {
        public int Id { get; set; }

        
        [Required, StringLength(50, ErrorMessage = "The {0} must be atleast {2} and at most {1} characters.", MinimumLength = 2)]
        public string Title { get; set; }

        
        [Required, StringLength(250, ErrorMessage = "The {0} must be atleast {2} and at most {1} characters.", MinimumLength = 2)]
        public string Summary { get; set; }

        
        [Required]
        public string PostBody { get; set; }
        public Status Status { get; set; } = Status.New;


        [Required, DataType(DataType.Date)]
        public DateTime Created { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? Updated { get; set; }


        public string Slug { get; set; }

        [Display(Name = "Image")]
        public int FileModelId { get; set; }

        
        [Display(Name = "Blog")]
        public int BlogModelId { get; set; }

        
        [Display(Name = "Creator")]
        public string CreatorId { get; set; }

        
        //Navigation
        public virtual FileModel FileModel { get; set; }
        public virtual BlogModel BlogModel { get; set; }
        public virtual UserModel Creator { get; set; }
        public virtual ICollection<TagModel> Tags { get; set; } = new HashSet<TagModel>();
        public virtual ICollection<CommentModel> Comments { get; set; } = new HashSet<CommentModel>();


    }
}
