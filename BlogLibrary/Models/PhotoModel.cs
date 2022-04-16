using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogLibrary.Models
{
    public class PhotoModel
    {
        public int Id { get; set; }

        [NotMapped, DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
        public string PhotoName { get; set; }
        public string Photoxtension { get; set; }
        public byte[] PhotoData { get; set; }

        [Required, Display(Name = "Uploaded")]
        public DateTimeOffset DateUploaded { get; set; }
    }
}
