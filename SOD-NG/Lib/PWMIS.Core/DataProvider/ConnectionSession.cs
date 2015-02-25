using System;
using System.Data;

namespace PWMIS.DataProvider.Data
{
    /// <summary>
    ///     连接会话对象类
    /// </summary>
    public class ConnectionSession : IDisposable
    {
        /// <summary>
        ///     以一个使用的连接初始化本类
        /// </summary>
        /// <param name="conn"></param>
        public ConnectionSession(IDbConnection conn)
        {
            Connection = conn;
        }

        public IDbConnection Connection { get; private set; }

        /// <summary>
        ///     释放连接
        /// </summary>
        public void Dispose()
        {
            if (Connection != null && Connection.State == ConnectionState.Open)
            {
                Connection.Close();
                Connection.Dispose();
                Connection = null;
            }
        }
    }
}