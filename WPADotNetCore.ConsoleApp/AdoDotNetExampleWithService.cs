using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPADotNetCore.Shared;

namespace WPADotNetCore.ConsoleApp
{
    class AdoDotNetExampleWithService
    {
        private readonly string _connectionString = "Data Source=DESKTOP-17S3R54;Initial Catalog=DotNetTrainingBatch5;user Id=sa;password=wai123!@#;";
        private readonly AdoDotNetService _adoDotNetService;
        public AdoDotNetExampleWithService()
        {
            _adoDotNetService = new AdoDotNetService(_connectionString);
        }
        public void Read()
        {
            string query = @"SELECT [BlogId]
                              ,[BlogTitle]
                              ,[BlogAuthor]
                              ,[BlogContent]
                              ,[DeleteFlag]
                          FROM [dbo].[Tbl_Blog] where DeleteFlag=0";
            var dt=_adoDotNetService.Query(query);
            foreach (DataRow dr in dt.Rows)
            {
                Console.WriteLine(dr["BlogId"]);
                Console.WriteLine(dr["BlogTitle"]);
                Console.WriteLine(dr["BlogAuthor"]);
                Console.WriteLine(dr["BlogContent"]);

            }
        }
        public void Edit()
        {
            Console.WriteLine("Please Enter BlogId : ");
            string blogId = Console.ReadLine();
            string query = @"SELECT [BlogId]
                  ,[BlogTitle]
                  ,[BlogAuthor]
                  ,[BlogContent]
                  ,[DeleteFlag]
              FROM [dbo].[Tbl_Blog] where BlogId=@BlogId and DeleteFlag=0";
            var dt = _adoDotNetService.Query(query, new SqlParameterModel("@BlogId", blogId));
           if(dt.Rows.Count <= 0) {
                Console.WriteLine("No Record Found");
                return;
            }
            DataRow dr = dt.Rows[0];
            Console.WriteLine(dr["BlogId"]);
            Console.WriteLine(dr["BlogTitle"]);
            Console.WriteLine(dr["BlogAuthor"]);
            Console.WriteLine(dr["BlogContent"]);
        }
        public void Create()
        {
            Console.Write("Please Enter BlogTitle:  ");
            string blogTitle = Console.ReadLine();
            Console.Write("Please Enter BlogAuthor:  ");
            string blogAuthor = Console.ReadLine();
            Console.Write("Please Enter BlogContent:  ");
            string blogContent = Console.ReadLine();

            string query = @"INSERT INTO [dbo].[Tbl_Blog]
                       ([BlogTitle]
                       ,[BlogAuthor]
                       ,[BlogContent]
                       ,[DeleteFlag])
                 VALUES
                       (@BlogTitle
                       ,@BlogAuthor
                       ,@BlogContent
                       ,0)";
            int result = _adoDotNetService.Execute(query, new SqlParameterModel("@BlogTitle", blogTitle), new SqlParameterModel("@BlogAuthor", blogAuthor), new SqlParameterModel("@BlogContent", blogContent));
           
            Console.WriteLine(result > 0 ? "Successfully Created" : "Failed in Creation");
        }

        public void Delete()
        {
            Console.WriteLine("Please Enter Blog Id");
            string blogId = Console.ReadLine();
            string query = @"DELETE FROM [dbo].[Tbl_Blog]
                                  WHERE BlogId=@BlogId";
            int result = _adoDotNetService.Execute(query, new SqlParameterModel("@BlogId", blogId));
            Console.WriteLine(result > 0 ? "Successfully Deleted" : "Failed to Delete");
        }
    }
}
