using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WPADotNetCore.Databases.Models;
using WPADotNetCore.RestApi.DataModels;
using WPADotNetCore.RestApi.ViewModels;

namespace WPADotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class BlogsDapperController : Controller
    {
        private readonly string _connectionString = "Data Source=DESKTOP-17S3R54;Initial Catalog=DotNetTrainingBatch5;user Id=sa;password=wai123!@#;TrustServerCertificate=True;";

        [HttpPost("CreateBlog")]
        public IActionResult CreateBlog(BlogViewModel blogViewModel)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = $@"INSERT INTO [dbo].[Tbl_Blog]
                               ([BlogTitle]
                               ,[BlogAuthor]
                               ,[BlogContent]
                               ,[DeleteFlag])
                         VALUES
                               (@BlogTitle
                               ,@BlogAuthor
                               ,@BlogContent
                               ,0)";
                int result = db.Execute(query, new BlogDataModel
                {
                    BlogTitle = blogViewModel.Title,
                    BlogAuthor = blogViewModel.Author,
                    BlogContent = blogViewModel.Content,
                });

                return Ok(result > 0 ? "Successfully Created" : "Failed in Creation");
            } 
        }

        [HttpGet("GetBlogs")]
        public IActionResult GetBlogs()
        {
            using(IDbConnection db=new SqlConnection(_connectionString))
            {
                List<BlogViewModel>blogList= new List<BlogViewModel>();
                string query = "SELECT * FROM Tbl_Blog where DeleteFlag=0;";
                 var lst = db.Query<BlogDataModel>(query).ToList();
                foreach(var blog in lst)
                {
                    BlogViewModel blogViewModel = new();
                    blogViewModel.Author = blog.BlogAuthor;
                    blogViewModel.Content = blog.BlogContent;
                    blogViewModel.Title = blog.BlogTitle;
                    blogViewModel.Id = blog.BlogId;
                    blogList.Add(blogViewModel);
                    
                }
                return Ok( blogList.Count > 0 ? blogList : BadRequest());
            }
        }
        [HttpGet("GetBlogById/{id}")]
        public IActionResult EditBlog(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                BlogViewModel blogViewModel = new BlogViewModel();
                string query = $@"SELECT [BlogId]
                      ,[BlogTitle]
                      ,[BlogAuthor]
                      ,[BlogContent]
                      ,[DeleteFlag]
                  FROM [dbo].[Tbl_Blog] where BlogId=@BlogId and DeleteFlag=0
                ";
                var item = db.Query<BlogDataModel>(query, new BlogDataModel
                {
                    BlogId = id,
                }).FirstOrDefault();
                if(item is not null)
                {
                    blogViewModel.Author = item.BlogAuthor;
                    blogViewModel.Title = item.BlogTitle;
                    blogViewModel.Content = item.BlogContent;
                    blogViewModel.Id = item.BlogId;
                }
                return Ok(blogViewModel is not null? item : NotFound());
            }
        }
        [HttpPatch("UpdateBlog")]
        public IActionResult UpdateBlog(BlogViewModel model)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string conditions = "";
                if (!string.IsNullOrEmpty(model.Title))
                {
                    conditions += " [BlogTitle]=@BlogTitle, ";
                 
                }
                if (!string.IsNullOrEmpty(model.Content))
                {
                    conditions += " [BlogContent]=@BlogContent, ";
                }
                if (!string.IsNullOrEmpty(model.Author))
                {
                    conditions += " [BlogAuthor]=@BlogAuthor, ";
                }
                if (conditions.Length < 0)
                {
                    return BadRequest();
                }
                conditions = conditions.Substring(0, conditions.Length - 2);
                string query = $@"UPDATE [dbo].[Tbl_Blog] SET {conditions}
             WHERE BlogId=@BlogId and DeleteFlag=0";
                var result = db.Execute(query, new BlogDataModel
                {
                    BlogTitle = model.Title,
                    BlogAuthor = model.Author,
                    BlogContent = model.Content,
                    BlogId = model.Id,
                });
                return Ok(result > 0 ? "Successfully Updated" : "Failed to update");
            }
        }
        [HttpDelete("DeleteBlog/{id}")]
        public IActionResult DeleteBlog(int id)
        {
            using(IDbConnection db=new SqlConnection(_connectionString))
            {
                string query= $@"DELETE FROM [dbo].[Tbl_Blog]
                 WHERE BlogId=@BlogId";
                var result = db.Execute(query, new BlogDataModel
                {
                    BlogId = id,
                });
                return Ok(result > 0 ? "Successfully Deleted" : "Failed to Delete");
            }
        }
    }
}
