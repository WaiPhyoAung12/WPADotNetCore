using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPADotNetCore.Shared
{
    public class AdoDotNetService
    {
        private readonly string _connectionString;

        public AdoDotNetService(string connectionString)
        {
            _connectionString = connectionString;
        }
        public DataTable Query(string query,params SqlParameterModel[] sqlParameters)
        {
            SqlConnection sqlConnection=new SqlConnection(_connectionString);
            sqlConnection.Open();
            SqlCommand cmd=new SqlCommand(query, sqlConnection);
            foreach(var sqlParameter in sqlParameters)
            {
                cmd.Parameters.AddWithValue(sqlParameter.Name, sqlParameter.Value);
            }
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt=new DataTable();
            dataAdapter.Fill(dt);
            return dt;
        }
        public int Execute(string query, params SqlParameterModel[] sqlParameters)
        {
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            foreach (var sqlParameter in sqlParameters)
            {
                cmd.Parameters.AddWithValue(sqlParameter.Name, sqlParameter.Value);
            }
            var result=cmd.ExecuteNonQuery();
           sqlConnection.Close();
            return result;
        }

    }
    public class SqlParameterModel
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public SqlParameterModel(string name,object value)
        {
            Name = name;
            Value = value;
        }
    }
}
