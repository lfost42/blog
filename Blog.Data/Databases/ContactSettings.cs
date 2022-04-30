using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Databases
{
    public class ContactSettings
    {
        [StringLength(50, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Name { get; set; }
        [Required, EmailAddress, StringLength(50, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Email { get; set; }
        [StringLength(50, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 2)]
        public string Subject { get; set; }
        [Required, StringLength(300, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Message { get; set; }
    }
}
