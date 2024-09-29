using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPADotNetCore.ConsoleApp
{
    public class AdoDotNetExample
    {
        private readonly string _connectionString = "Data Source=DESKTOP-17S3R54;Initial Catalog=DotNetTrainingBatch5;user Id=sa;password=wai123!@#;";
        public void Read()
        {
            Console.WriteLine("Connection String" + _connectionString);
            SqlConnection sqlconnection = new SqlConnection(_connectionString);
            Console.ReadKey();
            Console.WriteLine("Opening Connection String");
            sqlconnection.Open();
            string query = @"SELECT [BlogId]
                              ,[BlogTitle]
                              ,[BlogAuthor]
                              ,[BlogContent]
                              ,[DeleteFlag]
                          FROM [dbo].[Tbl_Blog] where DeleteFlag=0";
            SqlCommand cmd=new SqlCommand(query, sqlconnection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt=new DataTable();
            adapter.Fill(dt);   
            Console.WriteLine("Connection Opened");
            Console.WriteLine("Closing Connection String");
            sqlconnection.Close();
            Console.WriteLine("Connection Closed");
            if(dt.Rows.Count <= 0)
            {
                Console.WriteLine("No Data For Record");
                return;
            }
            foreach (DataRow dr in dt.Rows) 
            {
                Console.WriteLine(dr["BlogId"]);
                Console.WriteLine(dr["BlogTitle"]);
                Console.WriteLine(dr["BlogAuthor"]);
                Console.WriteLine(dr["BlogContent"]);

            }
            Console.ReadKey();
        }
        public void Create()
        {
            Console.Write("Please Enter BlogTitle:  ");
            string blogTitle = Console.ReadLine();
            Console.Write("Please Enter BlogAuthor:  ");
            string blogAuthor=Console.ReadLine();
            Console.Write("Please Enter BlogContent:  ");
            string blogContent=Console.ReadLine();

            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();
            string query=@"INSERT INTO [dbo].[Tbl_Blog]
           ([BlogTitle]
           ,[BlogAuthor]
           ,[BlogContent]
           ,[DeleteFlag])
     VALUES
           (@BlogTitle
           ,@BlogAuthor
           ,@BlogContent
           ,0)";
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            cmd.Parameters.AddWithValue("@BlogTitle", blogTitle);
            cmd.Parameters.AddWithValue("@BlogAuthor", blogAuthor);
            cmd.Parameters.AddWithValue("@BlogContent", blogContent);
            int result= cmd.ExecuteNonQuery();
            sqlConnection.Close();
            Console.WriteLine(result>0?"Successfully Created":"Failed in Creation");
            Console.ReadKey();

        }
        public void Edit()
        {
            Console.WriteLine("Please Enter BlogId : ");
            string blogId=Console.ReadLine();
            SqlConnection sqlConnection=new SqlConnection(_connectionString);
            sqlConnection.Open();
            string query = @"SELECT [BlogId]
                  ,[BlogTitle]
                  ,[BlogAuthor]
                  ,[BlogContent]
                  ,[DeleteFlag]
              FROM [dbo].[Tbl_Blog] where BlogId=@BlogId and DeleteFlag=0";
            SqlCommand cmd=new SqlCommand(query, sqlConnection);
            cmd.Parameters.AddWithValue("@BlogId",blogId);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            sqlConnection.Close();
            if (dt.Rows.Count <= 0)
            {
                Console.WriteLine("No Record Found");
                return;
            }
            DataRow dr = dt.Rows[0];
            Console.WriteLine(dr["BlogId"]);
            Console.WriteLine(dr["BlogTitle"]);
            Console.WriteLine(dr["BlogAuthor"]);
            Console.WriteLine(dr["BlogContent"]);
            Console.ReadKey();
        }
        public void Update()
        {
            Console.WriteLine("Please Enter Blog Id");
            string blogId=Console.ReadLine();
            Console.WriteLine("Please Enter Blog Author");
            string blogAuthor=Console.ReadLine();
            Console.WriteLine("Please Enter Blog Content");
            string blogContent = Console.ReadLine();
            Console.WriteLine("Please Enter Blog Title");
            string blogTitle=Console.ReadLine();
            SqlConnection sqlConnection= new SqlConnection(_connectionString);
            sqlConnection.Open();
            string query = @"UPDATE [dbo].[Tbl_Blog]
               SET [BlogTitle] = @BlogTitle
                  ,[BlogAuthor] = @BlogAuthor
                  ,[BlogContent] = @BlogContent
     
             WHERE BlogId=@BlogId and DeleteFlag=0";
            SqlCommand cmd=new SqlCommand(query, sqlConnection);
            cmd.Parameters.AddWithValue("@BlogId", blogId);
            cmd.Parameters.AddWithValue("@BlogTitle", blogTitle);
            cmd.Parameters.AddWithValue("@BlogAuthor", blogAuthor);
            cmd.Parameters.AddWithValue("@BlogContent", blogContent);
            int result=cmd.ExecuteNonQuery();
            sqlConnection.Close();
            Console.WriteLine(result > 0 ? "Successfully Updated" : "Failed to Update");
            Console.ReadKey();

        }
        public void Delete()
        {
            Console.WriteLine("Please Enter Blog Id");
            string blogId = Console.ReadLine();
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();
            string query = @"DELETE FROM [dbo].[Tbl_Blog]
                                  WHERE BlogId=@BlogId";
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            cmd.Parameters.AddWithValue("@BlogId",blogId);
            int result = cmd.ExecuteNonQuery();
            sqlConnection.Close();
            Console.WriteLine(result > 0 ? "Successfully Deleted" : "Failed to Delete");
            Console.ReadKey();
        }
    }
}
