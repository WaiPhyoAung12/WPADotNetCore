using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPADotNetCore.ConsoleApp.Models;

namespace WPADotNetCore.ConsoleApp
{
    public class DapperExample
    {
        private readonly string _connectionString = "Data Source=DESKTOP-17S3R54;Initial Catalog=DotNetTrainingBatch5;user Id=sa;password=wai123!@#;";
        public void Read()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Tbl_Blog where DeleteFlag=0;";
                var lst = db.Query<BlogDapperDataModel>(query).ToList();
                foreach (var item in lst) {
                    Console.WriteLine(item.BlogAuthor);
                    Console.WriteLine(item.BlogTitle);
                    Console.WriteLine(item.BlogContent);

                }
            }
        }
        public void Create(string blogAuthor, string blogTitle, string blogContent)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query= $@"INSERT INTO [dbo].[Tbl_Blog]
                               ([BlogTitle]
                               ,[BlogAuthor]
                               ,[BlogContent]
                               ,[DeleteFlag])
                         VALUES
                               (@BlogTitle
                               ,@BlogAuthor
                               ,@BlogContent
                               ,0)";
                int result = db.Execute(query, new BlogDapperDataModel
                {
                    BlogTitle = blogTitle,
                    BlogAuthor = blogAuthor,
                    BlogContent = blogContent,
                });
                Console.WriteLine(result>0?"Successfully Created":"Failed in Creation");
            }
        }
        public void Edit(int blogId)
        {
            using(IDbConnection db=new SqlConnection(_connectionString))
            {
                string query = $@"SELECT [BlogId]
                      ,[BlogTitle]
                      ,[BlogAuthor]
                      ,[BlogContent]
                      ,[DeleteFlag]
                  FROM [dbo].[Tbl_Blog] where BlogId=@BlogId and DeleteFlag=0
                ";
                var item = db.Query<BlogDapperDataModel>(query, new BlogDapperDataModel
                {
                    BlogId=blogId,
                }).FirstOrDefault();
                if(item is null)
                {
                    Console.WriteLine("No Record Found!");
                    return;
                }
                Console.WriteLine(item.BlogContent);
                Console.WriteLine(item.BlogAuthor);
                Console.WriteLine(item.BlogTitle);
            }
        }
        public void Delete(int blogId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = $@"DELETE FROM [dbo].[Tbl_Blog]
                 WHERE BlogId=@BlogId";
                int result = db.Execute(query, new BlogDapperDataModel
                {
                    BlogId = blogId,
                });
                Console.WriteLine(result > 0 ? "Successfully Deleted" : "Failed Deletion");
            }
        }
    }
}
