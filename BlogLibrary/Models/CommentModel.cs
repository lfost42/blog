using BlogLibrary.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogLibrary.Models
{
    public class CommentModel
    {
        public int Id { get; set; }

        [Required, StringLength(20, ErrorMessage = "The {0} must be atleast {2} and at most {1} characters.", MinimumLength = 2)]
        public string Subject { get; set; }

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


        [Display(Name = "Image")]
        public int FileModelId { get; set; }
        public int PostId { get; set; }
        public int CreatorId { get; set; }


        [Required, StringLength(500, ErrorMessage = "The {0} must be atleast {2} and at most {1} characters.", MinimumLength = 2)]
        public string ModeratedComment { get; set; }
        public ModeratedType Type { get; set; } = ModeratedType.None;

        //Navigation
        public virtual FileModel FileModel { get; set; }
        public virtual PostModel Post { get; set; }
        public virtual UserModel Creator { get; set; }

    }
}
