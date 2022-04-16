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
    public class ImageModel
    {
        public int Id { get; set; }

        [NotMapped, DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
        public string ImageExtension { get; set; }
        public byte[] ImageData { get; set; }

        [Required, Display(Name = "Uploaded")]
        public DateTimeOffset DateUploaded { get; set; }
    }
}
