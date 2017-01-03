using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class Admin_Query : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void getresults(object sender, EventArgs e)
    {
        try
        {
            string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["superadmin"].ConnectionString;
            
            SqlConnection conn = new SqlConnection(connstring);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(txtQuery.Text, conn);
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            conn.Close();

            grdResults.DataSource = ds;
            grdResults.DataBind();

            Response.Write("Rows: " + ds.Tables[0].Rows.Count.ToString());
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
    
}
