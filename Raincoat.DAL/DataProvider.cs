using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Raincoat.DAL
{
    public class DataProvider
    {
        // SqlDataAdapter
        protected SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
        // Logger
        private readonly ILog logger = LogManager.GetLogger(typeof(DataProvider));

        /// <summary>
        /// Get ConnectionString
        /// </summary>
        /// <returns></returns>
        protected string GetConnectionString()
        {
            try
            {
                return ConfigurationManager.ConnectionStrings[Constants.CONNECTION_NAME].ConnectionString.ToString();
            }
            catch (Exception ex)
            {
                logger.Error("GetConnectionString", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Open database connection if closed or broken
        /// </summary>
        /// <param name="sqlConnection"></param>
        protected void OpenConnection(SqlConnection sqlConnection)
        {
            try
            {
                logger.Debug("Start OpenConnection");
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
                logger.Debug("End OpenConnection");
            }
            catch (Exception ex)
            {
                logger.Error("OpenConnection", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Close Connection
        /// </summary>
        /// <param name="sqlConnection"></param>
        protected void CloseConnection(SqlConnection sqlConnection)
        {
            try
            {
                logger.Debug("Start CloseConnection");
                if (sqlConnection.State != ConnectionState.Closed)
                {
                    sqlConnection.Close();
                }
                logger.Debug("End CloseConnection");
            }
            catch (Exception ex)
            {
                logger.Error("CloseConnection", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Begin Transaction
        /// </summary>
        /// <param name="sqlConnection"></param>
        protected SqlTransaction BeginTransaction(SqlConnection sqlConnection)
        {
            try
            {
                logger.Debug("Start BeginTransaction");
                logger.Debug("End BeginTransaction");
                return sqlConnection.BeginTransaction();
            }
            catch (Exception ex)
            {
                logger.Error("BeginTransaction", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Commit Transaction
        /// </summary>
        /// <param name="sqlConnection"></param>
        protected void CommitTransaction(SqlTransaction sqlTransaction)
        {
            try
            {
                logger.Debug("Start CommitTransaction");
                sqlTransaction.Commit();
                logger.Debug("End CommitTransaction");
            }
            catch (Exception ex)
            {
                logger.Error("CommitTransaction", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Rollback Transaction
        /// </summary>
        /// <param name="sqlConnection"></param>
        protected void RollbackTransaction(SqlTransaction sqlTransaction)
        {
            try
            {
                logger.Debug("Start RollbackTransaction");
                sqlTransaction.Rollback();
                logger.Debug("End RollbackTransaction");
            }
            catch (Exception ex)
            {
                logger.Error("RollbackTransaction", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Create SqlCommand
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="sqlConnection"></param>
        /// <returns></returns>
        protected SqlCommand CreateSqlCommand(string storeName, SqlConnection sqlConnection, SqlParameter[] commandParameters)
        {
            try
            {
                logger.Debug("Start CreateSqlCommand");
                SqlCommand command = new SqlCommand(storeName, sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                if (commandParameters != null)
                {
                    foreach (SqlParameter parm in commandParameters)
                    {
                        command.Parameters.Add(parm);
                    }
                }
                logger.Debug("End CreateSqlCommand");
                return command;
            }
            catch (Exception ex)
            {
                logger.Error("CreateSqlCommand", ex);
                throw ex;
            }
        }

        /// <summary>
        /// ExecuteSelectQuery
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="sqlParameter"></param>
        /// <returns></returns>
        public DataTable ExecuteSelectQuery(string storeName, SqlParameter[] sqlParameter)
        {
            logger.Debug("Start ExecuteSelectQuery");
            DataTable dataTable = new DataTable();
            DataSet dataSet = new DataSet();
            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                try
                {
                    // Open connection
                    OpenConnection(connection);

                    // Create sql command
                    SqlCommand command = CreateSqlCommand(storeName, connection, sqlParameter);
                    command.CommandTimeout = 0;

                    // Execute query
                    command.ExecuteNonQuery();

                    // Fill dataset to data adapter
                    sqlDataAdapter.SelectCommand = command;
                    sqlDataAdapter.Fill(dataSet);

                    // add data to datatable
                    dataTable = dataSet.Tables[0];
                }
                catch (SqlException ex)
                {
                    // Write error log
                    logger.ErrorFormat("{0} {1} : {2}", Constants.EXECUTE_SELECT_QUERY, storeName, ex.Message);
                    // Close connection
                    CloseConnection(connection);
                    throw ex;
                }
                finally
                {
                    // Close connection
                    CloseConnection(connection);
                }
            }
            logger.Debug("End ExecuteSelectQuery");
            return dataTable;
        }

        /// <summary>
        /// ExecuteInsertQuery
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="sqlParameter"></param>
        /// <returns></returns>
        public int ExecuteInsertQuery(string storeName, SqlParameter[] sqlParameter)
        {
            logger.Debug("Start ExecuteInsertQuery");
            int result = -1;
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                // Open connection
                OpenConnection(sqlConnection);

                // Begin transaction.
                SqlTransaction sqlTransaction = BeginTransaction(sqlConnection);

                try
                {
                    SqlCommand command = CreateSqlCommand(storeName, sqlConnection, sqlParameter);
                    command.CommandTimeout = 0;
                    sqlDataAdapter.InsertCommand = command;
                    command.Transaction = sqlTransaction;

                    // Execute non query
                    result = command.ExecuteNonQuery();

                    // Commit transaction
                    CommitTransaction(sqlTransaction);
                }
                catch (Exception ex)
                {
                    // Write error log
                    logger.ErrorFormat("{0} {1} : {2}", Constants.EXECUTE_INSERT_QUERY, storeName, ex.Message);

                    // Rollback transaction
                    RollbackTransaction(sqlTransaction);

                    // Close connection
                    CloseConnection(sqlConnection);

                    throw ex;
                }
                finally
                {
                    // Close connection
                    CloseConnection(sqlConnection);
                }
            }
            logger.Debug("End ExecuteInsertQuery");
            return result;
        }

        /// <summary>
        /// Execute sql query
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="sqlParameter"></param>
        /// <returns></returns>
        public int ExecuteInsertQuery(string storeName, SqlParameter[] sqlParameter, string ouputName)
        {
            logger.Debug("Start ExecuteInsertQuery");
            int result = -1;
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                // Open connection
                OpenConnection(sqlConnection);

                // Begin transaction.
                SqlTransaction sqlTransaction = BeginTransaction(sqlConnection);

                try
                {
                    SqlCommand command = CreateSqlCommand(storeName, sqlConnection, sqlParameter);
                    command.CommandTimeout = 0;
                    sqlDataAdapter.InsertCommand = command;
                    command.Transaction = sqlTransaction;

                    // Execute non query
                    command.ExecuteNonQuery();
                    result = Convert.ToInt32(command.Parameters[ouputName].Value);
                    // Commit transaction
                    CommitTransaction(sqlTransaction);
                }
                catch (Exception ex)
                {
                    // Write error log
                    logger.ErrorFormat("{0} {1} : {2}", Constants.EXECUTE_INSERT_QUERY, storeName, ex.Message);

                    // Rollback transaction
                    RollbackTransaction(sqlTransaction);

                    // Close connection
                    CloseConnection(sqlConnection);

                    throw ex;
                }
                finally
                {
                    // Close connection
                    CloseConnection(sqlConnection);
                }
            }
            logger.Debug("End ExecuteInsertQuery");
            return result;
        }

        /// <summary>
        /// ExecuteUpdateQuery
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="sqlParameter"></param>
        /// <returns></returns>
        public int ExecuteUpdateQuery(string storeName, SqlParameter[] sqlParameter)
        {
            logger.Debug("Start ExecuteUpdateQuery");
            int result = -1;
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                // Open connection
                OpenConnection(sqlConnection);

                // Begin transaction.
                SqlTransaction sqlTransaction = BeginTransaction(sqlConnection);

                try
                {
                    SqlCommand command = CreateSqlCommand(storeName, sqlConnection, sqlParameter);
                    command.CommandTimeout = 0;
                    sqlDataAdapter.UpdateCommand = command;
                    command.Transaction = sqlTransaction;

                    // Execute query
                    result = command.ExecuteNonQuery();

                    // Commit transaction
                    CommitTransaction(sqlTransaction);
                }
                catch (Exception ex)
                {
                    // Write error log
                    logger.ErrorFormat("{0} {1} : {2}", Constants.EXECUTE_UPDATE_QUERY, storeName, ex.Message);

                    // Rollback transaction
                    RollbackTransaction(sqlTransaction);

                    // Close connection
                    CloseConnection(sqlConnection);

                    throw ex;
                }
                finally
                {
                    // Close connection
                    CloseConnection(sqlConnection);
                }
            }
            logger.Debug("End ExecuteUpdateQuery");
            return result;
        }

        /// <summary>
        /// Add parameter
        /// </summary>
        /// <param name="param"></param>
        /// <param name="sqlDbType"></param>
        /// <param name="Value"></param>
        public static SqlParameter AddParameters(string param, SqlDbType sqlDbType, object Value, string typename = "")
        {
            try
            {
                SqlParameter sqlParameter = new SqlParameter(param, sqlDbType);
                sqlParameter.Value = Value;
                if (typename != string.Empty)
                {
                    sqlParameter.TypeName = typename;
                }
                return sqlParameter;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get value type boolean
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columName"></param>
        /// <returns></returns>
        public static bool GetDBBoolean(DataRow row, string columName)
        {
            bool bRet = false;
            string sValueString = row[columName].ToString();
            bRet = string.IsNullOrEmpty(sValueString) ? false : bool.Parse(sValueString);
            return bRet;
        }

        /// <summary>
        /// Get value type string, with input param default
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetDBString(DataRow row, string columName, string defaultValue = "")
        {
            string sRet = string.Empty;
            string sValueString = row[columName].ToString();
            sRet = string.IsNullOrEmpty(sValueString) ? defaultValue : sValueString;
            return sRet;
        }

        /// <summary>
        /// Get value type int, with input param default
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetDBInteger(DataRow row, string columName, int defaultValue = -1)
        {
            int iRet = defaultValue;
            string sValueString = row[columName].ToString();
            iRet = string.IsNullOrEmpty(sValueString) ? defaultValue : int.Parse(sValueString);
            return iRet;
        }

        /// <summary>
        /// Get value type decimal, with input param default
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal GetDBDecimal(DataRow row, string columName, decimal defaultValue = -1)
        {
            decimal dRet = defaultValue;
            string sValueString = row[columName].ToString();
            dRet = string.IsNullOrEmpty(sValueString) ? defaultValue : decimal.Parse(sValueString);
            return dRet;
        }

        /// <summary>
        /// Get value type datetime type, with input param default
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime GetDBDateTime(DataRow row, string columName, DateTime defaultValue = default(DateTime))
        {
            DateTime dRet = defaultValue;
            string sValueString = row[columName].ToString();
            dRet = string.IsNullOrEmpty(sValueString) ? defaultValue : DateTime.Parse(sValueString);
            return dRet;
        }

        /// <summary>
        /// ExecuteDeleteQuery
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="sqlParameter"></param>
        /// <returns></returns>
        public int ExecuteDeleteQuery(string storeName, SqlParameter[] sqlParameter)
        {
            logger.Debug("Start ExecuteDeleteQuery");
            int result = -1;
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                // Open connection
                OpenConnection(sqlConnection);

                // Begin transaction.
                SqlTransaction sqlTransaction = BeginTransaction(sqlConnection);

                try
                {
                    SqlCommand command = CreateSqlCommand(storeName, sqlConnection, sqlParameter);
                    sqlDataAdapter.DeleteCommand = command;
                    command.Transaction = sqlTransaction;

                    // Execute query
                    result = command.ExecuteNonQuery();

                    // Commit transaction
                    CommitTransaction(sqlTransaction);
                }
                catch (Exception ex)
                {
                    // Write error log
                    logger.ErrorFormat("{0} {1} : {2}", Constants.EXECUTE_UPDATE_QUERY, storeName, ex.Message);

                    // Rollback transaction
                    RollbackTransaction(sqlTransaction);

                    // Close connection
                    CloseConnection(sqlConnection);

                    throw ex;
                }
                finally
                {
                    // Close connection
                    CloseConnection(sqlConnection);
                }
            }
            logger.Debug("End ExecuteDeleteQuery");
            return result;
        }
    }
}