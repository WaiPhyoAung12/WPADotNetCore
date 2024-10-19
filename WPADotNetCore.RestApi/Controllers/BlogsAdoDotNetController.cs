using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WPADotNetCore.RestApi.ViewModels;

namespace WPADotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsAdoDotNetController : Controller
    {
       
        private readonly string _connectionString = "Data Source=DESKTOP-17S3R54;Initial Catalog=DotNetTrainingBatch5;user Id=sa;password=wai123!@#;TrustServerCertificate=True;";

        [HttpPost("CreateBlog")]
        public IActionResult CreateBlog(BlogViewModel blogViewModel)
        {

            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();
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
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            cmd.Parameters.AddWithValue("@BlogTitle", blogViewModel.Title);
            cmd.Parameters.AddWithValue("@BlogAuthor", blogViewModel.Author);
            cmd.Parameters.AddWithValue("@BlogContent", blogViewModel.Content);
            int result = cmd.ExecuteNonQuery();
            sqlConnection.Close();
            string response = (result > 0 ? "Successfully Created" : "Failed in Creation");
            return Ok(response);
        }

        [HttpGet("GetBlogs")]
        public IActionResult Read()
        {
            List<BlogViewModel> lst = new List<BlogViewModel>();
            SqlConnection sqlconnection = new SqlConnection(_connectionString);
            sqlconnection.Open();
            string query = @"SELECT [BlogId]
                              ,[BlogTitle]
                              ,[BlogAuthor]
                              ,[BlogContent]
                              ,[DeleteFlag]
                          FROM [dbo].[Tbl_Blog] where DeleteFlag=0";
            SqlCommand cmd = new SqlCommand(query, sqlconnection);
            SqlDataReader reader = cmd.ExecuteReader();
            
                while (reader.Read())
                {
                    lst.Add (new BlogViewModel
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Author = reader.GetString(2),
                        Content = reader.GetString(3),
                    });
                    
                }
            
            return Ok(lst);
        }

        [HttpGet("EditBlog/{id}")]
        public IActionResult Edit(int id)
        {
            BlogViewModel viewModel = new BlogViewModel();
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();
            string query = @"SELECT [BlogId]
                  ,[BlogTitle]
                  ,[BlogAuthor]
                  ,[BlogContent]
                  ,[DeleteFlag]
              FROM [dbo].[Tbl_Blog] where BlogId=@BlogId and DeleteFlag=0";
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            cmd.Parameters.AddWithValue("@BlogId", id);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            sqlConnection.Close();
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                viewModel.Id = Convert.ToInt32(row["BlogId"]);
                viewModel.Title = row["BlogTitle"].ToString();
                viewModel.Author = row["BlogAuthor"].ToString();
                viewModel.Content = row["BlogContent"].ToString();
                return Ok(viewModel);
            }
            return NotFound();
        }

        [HttpPatch("UpdateBlog")]
        public IActionResult Update(BlogViewModel blogViewModel)
        {
           
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();
            string conditions = "";
            if (!string.IsNullOrEmpty(blogViewModel.Title))
            {
                conditions += " [BlogTitle]=@BlogTitle, ";
            }
            if (!string.IsNullOrEmpty(blogViewModel.Content))
            {
                conditions += " [BlogContent]=@BlogContent, ";
            }
            if (!string.IsNullOrEmpty(blogViewModel.Author))
            {
                conditions += " [BlogAuthor]=@BlogAuthor, ";
            }
            if (conditions.Length == 0)
            {
                return BadRequest();
            }
            conditions=conditions.Substring(0, conditions.Length-2);
            string query = $@"UPDATE [dbo].[Tbl_Blog] SET {conditions}
             WHERE BlogId=@BlogId and DeleteFlag=0";
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            if (!string.IsNullOrEmpty(blogViewModel.Title))
            {
                cmd.Parameters.AddWithValue("@BlogTitle", blogViewModel.Title);
            }
            if (!string.IsNullOrEmpty(blogViewModel.Content))
            {
                cmd.Parameters.AddWithValue("@BlogContent", blogViewModel.Content);
            }
            if (!string.IsNullOrEmpty(blogViewModel.Author))
            {
                cmd.Parameters.AddWithValue("@BlogAuthor", blogViewModel.Author);
            }
            
            cmd.Parameters.AddWithValue("@BlogId", blogViewModel.Id);
            int result = cmd.ExecuteNonQuery();
            sqlConnection.Close();
            return Ok(result > 0 ? "Update Successfully" : "Fail Update");
        }

        [HttpDelete("DeleteBlog/{id}")]
        public IActionResult Delete(int id)
        {
           
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();
            string query = @"DELETE FROM [dbo].[Tbl_Blog]
                                  WHERE BlogId=@BlogId";
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            cmd.Parameters.AddWithValue("@BlogId", id);
            int result = cmd.ExecuteNonQuery();
            sqlConnection.Close();
            return Ok(result > 0 ? "Successfully Deleted" : "Failed to Delete");
            
        }
    }
}
