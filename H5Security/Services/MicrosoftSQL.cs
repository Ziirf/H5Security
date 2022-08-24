using H5Security.Models;
using H5Security.Services.Interfaces;
using System.Data.SqlClient;

namespace H5Security.Services
{
    public class MicrosoftSQL : IMicrosoftSQL
    {
        public delegate void SqlQueryCallBack(SqlConnection connection);

        public void OpenConnection(SqlQueryCallBack callback)
        {
            SqlConnection connection = new SqlConnection(HostConfig.DatabaseConnectionString);
            using (connection)
            {
                connection.Open();
                callback(connection);
            }
        }
    }
}
