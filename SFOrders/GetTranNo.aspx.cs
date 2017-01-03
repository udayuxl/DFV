using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;

public partial class GetTranNo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        string strOrderName="";
        string strCustName = "";
        if ( (Request.Form["OrderName"] != null) || (Request.QueryString["OrderName"] != null) )
        {
            if (Request.Form["OrderName"] != null) 
                strOrderName = Request.Form["OrderName"].ToString();
            else if (Request.QueryString["OrderName"] != null)
                strOrderName = Request.QueryString["OrderName"].ToString();
            //strOrderName = strOrderName.Replace("IO", "Test");

            if (Request.Form["CustName"] != null)
                strCustName = Request.Form["CustName"].ToString();
            else if (Request.QueryString["CustName"] != null)
                strCustName = Request.QueryString["CustName"].ToString();

            string strCmd = "SELECT ISNULL(TransactionNo,0) AS TransactionNo FROM tblOrderDestination WHERE CustName ='" + strCustName + "'  AND OrderName = '" + strOrderName + "' ";
            try
            {
                DataRow drOrder = GetSingleRow(strCmd);
                if (drOrder != null)
                {
                    string strTranNo = drOrder[0].ToString();
                    Response.Write("SUCESS#Emerge#"+strTranNo + "#Emerge#");
                }
                else
                {
                    Response.Write("NOSUCHORDER");
                }
            }
            catch (Exception ex)
            {
                Response.Write("EXCEPTION#Emerge#TRYAGAIN#Emerge#" + ex.ToString());
            }
        }
    }


    public DataRow GetSingleRow(string strCommand)
    {
        string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["ComplemarBridge"].ConnectionString;
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

    public static int ExecuteSQL(string strCommand)
    {
        SqlConnection conn = null;
        try
        {
            string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["ComplemarBridge"].ConnectionString;
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
            conn.Close();
            conn.Dispose();
            throw exSql;
        }
        catch (Exception ex)
        {
            conn.Close();
            conn.Dispose();
            throw ex;
        }
    }
}

    

