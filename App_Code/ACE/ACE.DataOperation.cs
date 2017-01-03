using System;
using System.Data;
using System.Data.SqlClient;

namespace ACE
{
    public static class DataOperations
    {
        public static int ExecuteSQL(string strCommand)
        {
            SqlConnection conn = null;
            try
            {
                //LogAndTrace.CodeTrace("strCommand - " + strCommand);
                string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["LumenConnectionString"].ConnectionString;                
                conn = new SqlConnection(connstring);
                conn.Open();
                SqlCommand sqlcmd = new SqlCommand(strCommand, conn);
                int intRowsAffected = sqlcmd.ExecuteNonQuery();
                conn.Close();
                conn.Dispose();                
                return intRowsAffected;
            }            
            catch (SqlException exSql)
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                LogAndTrace.CodeTrace(exSql.Message);
                LogAndTrace.CodeTrace(exSql.LineNumber.ToString());
                throw exSql;
            }
            catch (Exception ex)
            {
                if (conn != null)
                {            
                    conn.Close();
                    conn.Dispose();
                }
                throw ex;                
            }
        }
        public static DataTable GetDataTable(string strCommand)
        {            
            string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["LumenConnectionString"].ConnectionString;         
            SqlConnection conn = new SqlConnection(connstring);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(strCommand, conn);
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            conn.Close();
            da.Dispose();
            conn.Dispose();
            if (ds == null)
            {                
                return null;
            }            
            return ds.Tables[0];
        }

        public static DataRow GetSingleRow(string strCommand)
        {            
            string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["LumenConnectionString"].ConnectionString;         
            SqlConnection conn = new SqlConnection(connstring);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(strCommand, conn);
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            conn.Close();
            da.Dispose();
            conn.Dispose();
            if (ds == null)
            {
                return null;
            }
            if (ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            return ds.Tables[0].Rows[0];
        }
    
        public static int ExecuteSQL(string strCommand, out string strResult)
        {
            SqlConnection conn = null;
            try
            {
                string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["LumenConnectionString"].ConnectionString;
                conn = new SqlConnection(connstring);
                conn.Open();
                SqlCommand sqlcmd = new SqlCommand(strCommand, conn);
                int intRowsAffected = sqlcmd.ExecuteNonQuery();
                conn.Close();
                conn.Dispose();
                strResult = "SUCCESSFUL";
                return intRowsAffected;
            }            
            catch (SqlException exSql)
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                LogAndTrace.CodeTrace(exSql.Message);
                LogAndTrace.CodeTrace(exSql.LineNumber.ToString());

                if (exSql.Message.Contains("REFERENCE constraint"))
                {
                    strResult = "INUSE";
                    return 0;
                }
                throw exSql;
            }
            catch (Exception ex)
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                throw ex;                
            }
        }
    }
}