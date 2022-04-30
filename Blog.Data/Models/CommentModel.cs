using Blog.Data.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Models
{
    public class CommentModel
    {
        public int Id { get; set; }

        [Required, StringLength(500, ErrorMessage = "The {0} must be atleast {2} and at most {1} characters.", MinimumLength = 2)]
        public string Comment { get; set; }

        
        [Required, DataType(DataType.Date)]
        public DateTime Created { get; set; }

        
        [DataType(DataType.Date)]
        public DateTime? Updated { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Moderated { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Deleted { get; set; }


        [Display(Name = "Article")]
        public int ArticleId { get; set; }
        [Display(Name = "Creator")]
        public string CreatorId { get; set; }


        [StringLength(500, ErrorMessage = "The {0} must be atleast {2} and at most {1} characters.", MinimumLength = 2)]
        public string ModeratedComment { get; set; }

        //Navigation
        public virtual ArticleModel Article { get; set; }
        public virtual UserModel Creator { get; set; }

    }
}
