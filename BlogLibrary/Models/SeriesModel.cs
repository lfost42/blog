using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogLibrary.Models
{
    public class SeriesModel
    {
        public int Id { get; set; }

        [Display(Name = "Series"), Required, StringLength(50, ErrorMessage = "The {0} must be atleast {2} and at most {1} characters.", MinimumLength = 2)]
        public string Title { get; set; }

        [Required, StringLength(500, ErrorMessage = "The {0} must be atleast {2} and at most {1} characters.", MinimumLength = 2)]
        public string Description { get; set; }


        [Required, DataType(DataType.Date)]
        public DateTime Created { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Updated { get; set; }

        [Display(Name = "Image")]
        public int? ImageId { get; set; }

        [Display(Name = "Creator")]
        public string CreatorId { get; set; }


        //Navigation
        public virtual ImageModel Image { get; set; } = new ImageModel();
        public virtual UserModel Creator { get; set; }
        public virtual ICollection<ArticleModel> Articles { get; set; } = new HashSet<ArticleModel>();


    }
}
