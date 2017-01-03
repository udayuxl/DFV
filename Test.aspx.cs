using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataRow dr = ACE.DataOperations.GetSingleRow("SELECT * FROM Emerge.tblClient");
        if (dr != null)
        {
            Response.Write(dr["ClientCode"].ToString() + "<br/>");
            Response.Write(dr["Name"].ToString() + "<br/>");
        }
        else
        {
            Response.Write("Client Table empty <br/>");
        }

        int x = ACE.DataOperations.ExecuteSQL("DELETE Emerge.tblProgram");
    }
}