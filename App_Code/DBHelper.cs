using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for DBHelper
/// </summary>
public class DBHelper
{
    public SqlConnection _connection;
    private bool _isConnected = false;
    private int _batchSize = 1000;
    string _connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

    public bool Connect()
    {
        _connection = new SqlConnection(_connectionString);
        if (_connection.State == ConnectionState.Open)
        {
            _isConnected = true;
        }
        return _isConnected;
    }

    public void BatchInsert(DataTable dataTable, SqlCommand insertCommand)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            insertCommand.Connection = connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = insertCommand;
            adapter.UpdateBatchSize = _batchSize;
            adapter.Update(dataTable);
            connection.Close();
        }
    }

    public void ExecuteCommand(SqlCommand command)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            command.Connection = connection;
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void ExecuteStoredProcedure(string storedProcedureName, SqlParameter[] parameters)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;
            cmd.Connection = connection;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storedProcedureName;
            cmd.Parameters.AddRange(parameters);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
        }
    }

    public DataSet ExecuteStoredProcedureReturnDataset(string storedProcedureName, SqlParameter[] parameters)
    {
        DataSet ds = new DataSet("Result");
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;
            cmd.Connection = connection;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storedProcedureName;
            cmd.Parameters.AddRange(parameters);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
        }
        return ds;
    }

    public DataSet ExecuteQueryReturnDataSet(string commandString)
    {
        DataSet ds = new DataSet();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(commandString,connection);
            adapter.Fill(ds);
            connection.Close();

        }
        return ds;
    }
}