using System.Data.SqlClient;

namespace H5Security.Services.Interfaces
{
    public interface IMicrosoftSQL
    {
        void OpenConnection(MicrosoftSQL.SqlQueryCallBack callback);
    }
}