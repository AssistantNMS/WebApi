using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence;

namespace NMS.Assistant.Persistence.Repository
{
    public class StoredProcedureRepository
    {
        public static async Task<Result> Execute(NmsAssistantContext db, string query, List<StoredProcedureParameter> parameters, Action<DbDataReader> dbReader, Action<SqlCommand> sqlCommand = null, Action<Exception> exception = null, int numberOfRetries = 0, bool allowZeroRows = true)
        {
            int retries = 0;
            bool passed = false;
            Guid guid = Guid.NewGuid();
            string myException = $"Initialized. Guid:{guid}";

            do
            {
                if (!(db.Database.GetDbConnection() is SqlConnection conn)) return new Result(false, "DBConnection null");

                try
                {
                    SqlCommand command = new SqlCommand(query, conn){ CommandType = CommandType.StoredProcedure };
                    sqlCommand?.Invoke(command);
                    foreach (StoredProcedureParameter param in parameters ?? new List<StoredProcedureParameter>())
                    {
                        object dbValue = param.Value ?? DBNull.Value;
                        SqlParameter storedProcParam = command.Parameters.AddWithValue(param.Name, dbValue);
                        storedProcParam.SqlDbType = param.DataType;
                        if (!string.IsNullOrEmpty(param.TypeName)) storedProcParam.TypeName = param.TypeName;
                    }
                    await conn.OpenAsync();

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            dbReader(reader);

                            passed = true;
                        }
                    }
                    else
                    {
                        retries = numberOfRetries;
                        if (allowZeroRows) passed = true;
                    }

                    retries++;

                    await reader.CloseAsync();
                }
                catch (Exception ex)
                {
                    retries++;
                    myException = ex.Message;
                    exception?.Invoke(ex);

                    passed = false;
                }
                finally
                {
                    conn.Close();
                }
            }
            while ((retries < numberOfRetries) && (passed == false));

            return new Result(passed, myException);
        }

        public static async Task<Result> Execute(NmsAssistantContext db, string query, List<StoredProcedureParameter> parameters, Action<DataSet> dbDataSetReader = null, Action<SqlCommand> sqlCommand = null, Action<Exception> exception = null)
        {
            bool passed;
            Guid guid = Guid.NewGuid();
            string myException = $"Initialized. Guid:{guid}";

            if (!(db.Database.GetDbConnection() is SqlConnection conn)) return new Result(false, "DBConnection null");
            DataSet dataSet = new DataSet();

            try
            {
                SqlCommand command = new SqlCommand(query, conn) { CommandType = CommandType.StoredProcedure };
                sqlCommand?.Invoke(command);
                foreach (StoredProcedureParameter param in parameters ?? new List<StoredProcedureParameter>())
                {
                    object dbValue = param.Value ?? DBNull.Value;
                    SqlParameter storedProcParam = command.Parameters.AddWithValue(param.Name, dbValue);
                    storedProcParam.SqlDbType = param.DataType;
                    if (!string.IsNullOrEmpty(param.TypeName)) storedProcParam.TypeName = param.TypeName;
                }
                await conn.OpenAsync();

                using SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataSet);
                dbDataSetReader?.Invoke(dataSet);
                passed = true;
            }
            catch (Exception ex)
            {
                myException = ex.Message;
                exception?.Invoke(ex);

                passed = false;
            }
            finally
            {
                conn.Close();
            }

            return new Result(passed, myException);
        }
    }
}
