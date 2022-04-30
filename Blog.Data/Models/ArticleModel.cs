using Blog.Data.Databases.Interfaces;
using Blog.Data.Models.Enum;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Models
{
    public class ArticleModel
    {
        public int Id { get; set; }

        
        [Required, Display(Name = "Article"), StringLength(50, ErrorMessage = "The {0} must be atleast {2} and at most {1} characters.", MinimumLength = 2)]
        public string Title { get; set; }

        
        [Required, StringLength(250, ErrorMessage = "The {0} must be atleast {2} and at most {1} characters.", MinimumLength = 2)]
        public string Summary { get; set; }

        
        [Required]
        public string Body { get; set; }
        public Status Status { get; set; } = Status.Draft;


        [Required, DataType(DataType.Date)]
        public DateTime Created { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? Updated { get; set; }


        public string Slug { get; set; }

        [Display(Name = "Image")]
        public int? ImageId { get; set; }

        
        [Display(Name = "Series")]
        public int SeriesModelId { get; set; }

        
        [Display(Name = "Creator")]
        public string CreatorId { get; set; }


        //Navigation
        public virtual ImageModel Image { get; set; }
        public virtual SeriesModel SeriesModel { get; set; }
        public virtual UserModel Creator { get; set; }
        public virtual ICollection<TagModel> Tags { get; set; } = new HashSet<TagModel>();
        public virtual ICollection<CommentModel> Comments { get; set; } = new HashSet<CommentModel>();


    }
}
