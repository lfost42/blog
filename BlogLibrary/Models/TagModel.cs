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


        public int PostId { get; set; }
        public string CreatorId { get; set; }


        //Navigation
        public virtual PostModel Post { get; set; }
        public UserModel Creator { get; set; }

    }
}
