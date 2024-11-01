using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPADotNetCore.Shared
{
    public class DapperService
    {
        private readonly string _connectionString;

        public DapperService(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<T> Query<T>(string query,object? param=null)
        {
            using IDbConnection dbConnection = new SqlConnection(_connectionString);
            var lst=dbConnection.Query<T>(query,param).ToList();
            return lst;
        }
        public T QueryFirstOrDefault<T>(string query, object? param = null)
        {
            using IDbConnection dbConnection = new SqlConnection(_connectionString);
            var item = dbConnection.Query<T>(query, param).FirstOrDefault();
            return item;
        }
        public int Execute(string query,object? param = null)
        {
            using IDbConnection dbConnection = new SqlConnection(_connectionString);
            var result=dbConnection.Execute(query, param);
            return result;
        }
        public void Delete(int blogId)
        {

        }
     }
}
