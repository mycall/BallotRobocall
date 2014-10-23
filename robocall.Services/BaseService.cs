using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace robocall.Services
{
    public class BaseService: IDisposable
    {
        public const string ConnectionString = "Server=(local);Database=BallotRobocall;User Id=ballot;Password=t9j964";

        protected SqlConnection connection;

        public BaseService()
        {
            connection = new SqlConnection(ConnectionString);
            connection.Open();
        }

        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                connection.Close();
                connection.Dispose();
            }

            disposed = true;
        }
    }
}
