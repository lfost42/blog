using BlogLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogLibrary.Data
{
    public class BlogContext : IdentityDbContext<UserModel, IdentityRole, string>
    {
        public BlogContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<TopicModel> Topics { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<ImageModel> Images { get; set; }
        public DbSet<ArticleModel> Articles { get; set; }
        public DbSet<TagModel> Tags { get; set; }
        public DbSet<UserModel> Users { get; set; }


    }

}
