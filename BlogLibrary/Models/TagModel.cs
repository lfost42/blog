using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogLibrary.Models
{
    public class TagModel
    {
        public int Id { get; set; }

        [Required, StringLength(30, ErrorMessage = "The {0} must be atleast {2} and at most {1} characters.", MinimumLength = 2)]
        public string Tag { get; set; }

        [Display(Name = "Article")]
        public int ArticleId { get; set; }


        //Navigation
        public virtual ArticleModel Article { get; set; }

    }
}
