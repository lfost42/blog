using System;
using System.Linq;
using Blog.Data.Data;
using Blog.Data.Models;
using Blog.Data.Models.Enum;
using Blog.Data.Databases.Interfaces;

namespace Blog.Data.Databases
{
    public class SearchService : ISearchService
    {
        private readonly BlogContext _context;

        public SearchService(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<ArticleModel> Search(string searchTerm)
        {
            var articles = _context.Articles
                .Where(a => a.Status == Status.Published)
                .AsQueryable();

            if (searchTerm != null)
            {
                searchTerm = searchTerm.ToLower();

                articles = articles.Where(
                    a => a.Title.ToLower().Contains(searchTerm) ||
                    a.Summary.ToLower().Contains(searchTerm) ||
                    a.Body.ToLower().Contains(searchTerm) ||
                    a.Comments.Any(c => c.Comment.ToLower().Contains(searchTerm) ||
                    c.Creator.FirstName.ToLower().Contains(searchTerm) ||
                    c.Creator.LastName.ToLower().Contains(searchTerm) ||
                    c.Creator.Email.ToLower().Contains(searchTerm)));
            }

            return articles.OrderByDescending(a => a.Created);
        }
    }
}
