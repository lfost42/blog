using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Databases.Interfaces
{
    public interface ISlugService
    {
        string UrlRoute(string title);
        bool IsUnique(string slug);

    }
}
