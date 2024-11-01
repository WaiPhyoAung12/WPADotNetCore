using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPADotNetCore.ConsoleApp.Models;
using WPADotNetCore.Shared;

namespace WPADotNetCore.ConsoleApp
{
    public class DapperExampleWithService
    {
        private readonly string _connectionString = "Data Source=DESKTOP-17S3R54;Initial Catalog=DotNetTrainingBatch5;user Id=sa;password=wai123!@#;";
        private readonly DapperService _dapperService;
        public DapperExampleWithService()
        {
            _dapperService = new DapperService(_connectionString);
        }
        public void Read()
        {
            string query = "SELECT * FROM Tbl_Blog where DeleteFlag=0;";
            var lst = _dapperService.Query<BlogDapperDataModel>(query).ToList();
            if(lst.Count <= 0)
            {
                Console.WriteLine("No Record Found");
                return;
            }
            foreach (var item in lst)
            {
                Console.WriteLine(item.BlogAuthor);
                Console.WriteLine(item.BlogTitle);
                Console.WriteLine(item.BlogContent);

            }
        }
        public void Edit(int blogId)
        {
            string query = $@"SELECT [BlogId]
                      ,[BlogTitle]
                      ,[BlogAuthor]
                      ,[BlogContent]
                      ,[DeleteFlag]
                  FROM [dbo].[Tbl_Blog] where BlogId=@BlogId and DeleteFlag=0
                ";
            var item = _dapperService.QueryFirstOrDefault<BlogDapperDataModel>(query, new BlogDapperDataModel
            {
                BlogId = blogId
            });
            if (item is null)
            {
                Console.WriteLine("No Record Found!");
                return;
            }
            Console.WriteLine(item.BlogContent);
            Console.WriteLine(item.BlogAuthor);
            Console.WriteLine(item.BlogTitle);
        }
        public void Create(string blogAuthor, string blogTitle, string blogContent)
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
            int result = _dapperService.Execute(query, new BlogDapperDataModel
            {
                BlogAuthor = blogAuthor,
                BlogTitle = blogTitle,
                BlogContent = blogContent,
            });
            Console.WriteLine(result > 0 ? "Successfully Created" : "Failed in Creation");
        }
        public void Delete(int blogId)
        {
            string query = $@"DELETE FROM [dbo].[Tbl_Blog]
                 WHERE BlogId=@BlogId";
            var result = _dapperService.Execute(query, new BlogDapperDataModel
            {
                BlogId = blogId,
            });
            Console.WriteLine(result > 0 ? "Successfully Deleted" : "Failed Deletion");
        }
    }
}
