using System.Linq;
using BlogLibrary.Models;

namespace BlogLibrary.Databases.Interfaces
{
    public interface ISearchService
    {
        IQueryable<ArticleModel> Search(string searchTerm);
    }
}