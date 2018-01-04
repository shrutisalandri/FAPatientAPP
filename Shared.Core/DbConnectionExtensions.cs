using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using Dapper;

namespace Shared.Core
{
    /// <summary>
    /// Description of DbConnectionExtensions.
    /// </summary>
    public static class DbConnectionExtensions
    {
        /// <summary>
        /// Wraps the DBConnection open method in a retry block
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="interval"></param>
        /// <param name="retries"></param>
        public static void OpenRobust(this IDbConnection conn, TimeSpan interval, int retries = 3)
        {
            Retry.Do(() =>
                     {
                         if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                         {
                             conn.Open();
                         }
                     }
                     , interval, retries);
        }

        public static void OpenRobust(this IDbConnection conn, int retries = 3)
        {
            OpenRobust(conn, TimeSpan.FromMilliseconds(100), retries);
        }

        /// <summary>
        /// Wraps the Dapper Query method in a retry block
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="interval"></param>
        /// <param name="retries"></param>
        /// <returns></returns>
        public static IEnumerable<T> QueryRobust<T>(this IDbConnection conn, string sql, object param, TimeSpan interval, int retries = 3)
        {
            return Retry.Do<IEnumerable<T>>(() =>
                                      {
                                          return conn.Query<T>(sql, param);

                                      }, interval, retries);



        }

        /// <summary>
        /// Wraps the Dapper Query method in a retry block
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="retries"></param>
        /// <returns></returns>
        public static IEnumerable<T> QueryRobust<T>(this IDbConnection conn, string sql, object param, int retries = 3)
        {
            return QueryRobust<T>(conn, sql, param, TimeSpan.FromMilliseconds(100), retries);
        }

        public static int ExecuteRobust(this IDbConnection conn, string sql, object param, int retries = 3, IDbTransaction txn = null)
        {
            return ExecuteRobust(conn, sql, param, TimeSpan.FromMilliseconds(100), retries, txn);
        }

        public static int ExecuteRobust(this IDbConnection conn, string sql, object param, TimeSpan interval, int retries = 3, IDbTransaction txn = null)
        {
            return Retry.Do(() =>
                            {
                                //, CommandType.Text
                                return conn.Execute(sql, param, txn);
                            }, interval, retries);
        }

        public static int ExecuteScalarRobust(this IDbConnection conn, string sql, object param, int retries = 3, IDbTransaction txn = null)
        {
            return ExecuteScalarRobust(conn, sql, param, TimeSpan.FromMilliseconds(100), retries, txn);
        }

        public static int ExecuteScalarRobust(this IDbConnection conn, string sql, object param, TimeSpan interval, int retries = 3, IDbTransaction txn = null)
        {
            return Retry.Do(() =>
            {
                return Convert.ToInt32(conn.ExecuteScalar(sql, param, txn, null, CommandType.Text));
            }, interval, retries);
        }

        public static IDataReader ExecuteReaderRobust(this IDbConnection conn, string sql, object param, int retries = 3)
        {
            return ExecuteReaderRobust(conn, sql, param, TimeSpan.FromMilliseconds(100), retries);
        }

        public static IDataReader ExecuteReaderRobust(this IDbConnection conn, string sql, object param, TimeSpan interval, int retries = 3)
        {
            return Retry.Do(() =>
            {
                return conn.ExecuteReader(sql, param);
            }, interval, retries);
        }

    }
}
