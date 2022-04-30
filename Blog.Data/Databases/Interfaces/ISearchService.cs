using System.Linq;
using Blog.Data.Models;

namespace Blog.Data.Databases.Interfaces
{
    public interface ISearchService
    {
        IQueryable<ArticleModel> Search(string searchTerm);
    }
}