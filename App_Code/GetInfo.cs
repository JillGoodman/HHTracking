using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for GetInfo
/// </summary>
public class GetInfo
{
    public DataSet PopulatePriorCareData(string sisnumber)
    {
        DBHelper helper = new DBHelper();
        SqlParameter param1 = new SqlParameter("@SIS_ID", System.Data.SqlDbType.VarChar);
        param1.Value = sisnumber;
        DataSet ds = helper.ExecuteStoredProcedureReturnDataset("HCP_PVUE_PRIORCARE", new SqlParameter[] { param1 });
        return ds;
    }

    public void DeletePriorCareRow(string s)
    {
        DBHelper helper = new DBHelper();
        SqlParameter p1 = new SqlParameter("@ROWGUID", System.Data.SqlDbType.VarChar);
        p1.Value = s;
        DataSet ds = helper.ExecuteStoredProcedureReturnDataset("HCP_PVUE_DELETEPRIORCARE", new SqlParameter[] { p1 });
        return;
    }

    public void InsertPriorCareRow(string priorCare, string timeSpent, string sisNumber)
    {
        DBHelper helper = new DBHelper();
        SqlParameter p1 = new SqlParameter("@priorCare", System.Data.SqlDbType.VarChar);
        SqlParameter p2 = new SqlParameter("@timeSpent", System.Data.SqlDbType.VarChar);
        SqlParameter p3 = new SqlParameter("@sis_number", System.Data.SqlDbType.VarChar);
        p1.Value = priorCare;
        p2.Value = timeSpent;
        p3.Value = sisNumber;
        DataSet ds = helper.ExecuteStoredProcedureReturnDataset("HCP_PVUE_INSERTPRIORCARE", new SqlParameter[] { p1, p2, p3 });
        return;
    }

}