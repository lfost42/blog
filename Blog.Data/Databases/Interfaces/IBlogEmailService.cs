using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Databases.Interfaces
{
    public interface IBlogEmailService : IEmailSender 
    {
        Task SendContactEmailAsync(string emailFrom, string name, string subject, string message);
    
    }
}
