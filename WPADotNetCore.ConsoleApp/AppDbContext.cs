using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPADotNetCore.ConsoleApp.Models;

namespace WPADotNetCore.ConsoleApp
{
    public class AppDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString= "Data Source=DESKTOP-17S3R54;Initial Catalog=DotNetTrainingBatch5;user Id=sa;password=wai123!@#;TrustServerCertificate=true";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        public DbSet<BlogDataModel> blogData { get; set; }
    }
}
