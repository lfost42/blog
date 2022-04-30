using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Databases.Interfaces
{
    public interface IImageService
    {
        Task<byte[]> EncodeImageAsync(IFormFile file);
        Task<byte[]> EncodeImageAsync(string fileName);
        string DecodeImage(byte[] data, string type);
        string ContentType(IFormFile file);
        int Size(IFormFile file);
    }
}
