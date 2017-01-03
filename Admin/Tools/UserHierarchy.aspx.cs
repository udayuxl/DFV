using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ACE;
using System.Data;

public partial class Admin_Tools_UserHierarchy : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
            BindGrid();
    }

    private void BindGrid()
    {        
        gridUserHierarchy.DataSource = DataOperations.GetDataTable("EXEC [Emerge].[SP_UserHierarchy]");
        gridUserHierarchy.DataBind();        
    }

    protected void gridUserHierarchy_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if(e.Row.RowType != DataControlRowType.DataRow)
            return;
        
        DropDownList ddlAllUsers = e.Row.FindControl("ddlAllUsers") as DropDownList;
        HiddenField hdnUserId = e.Row.FindControl("hdnUserId") as HiddenField;

        hdnUserId.Value = Convert.ToString( DataBinder.Eval(e.Row.DataItem, "UserId"));
                
        ddlAllUsers.DataSource = DataOperations.GetDataTable("SELECT Userid, Username from aspnet_users where userid <> '" + hdnUserId.Value + "'");
        ddlAllUsers.DataTextField = "Username";
        ddlAllUsers.DataValueField = "UserId";
        ddlAllUsers.DataBind();

        ddlAllUsers.Text = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "ManagerID"));
    }

    protected void ReportToSelected(object sender, EventArgs e)
    {
        GridViewRow theRow = ((DropDownList)sender).Parent.Parent as GridViewRow;
        DropDownList ddlAllUsers = theRow.FindControl("ddlAllUsers") as DropDownList;
        HiddenField hdnUserId = theRow.FindControl("hdnUserId") as HiddenField;
        string strUserID = hdnUserId.Value;
        string strManagerID = ddlAllUsers.SelectedItem.Value;

        if (strManagerID == "0") //Delete the manager
            DataOperations.ExecuteSQL("DELETE Emerge.tblUserPortfolio WHERE fk_user_id='" + strUserID + "'");

        else
        {
            DataRow drExists = DataOperations.GetSingleRow("SELECT COUNT(*) FROM Emerge.tblUserPortfolio WHERE fk_user_id = '" + strUserID + "' AND fk_manager_id = '" + strManagerID + "'");
            if (drExists[0].ToString() == "0")
                DataOperations.ExecuteSQL("INSERT INTO Emerge.tblUserPortfolio(fk_user_id,fk_manager_id) VALUES('" + strUserID + "','" + strManagerID + "')");
            else
                DataOperations.ExecuteSQL("UPDATE Emerge.tblUserPortfolio SET fk_manager_id='" + strManagerID + "' WHERE fk_user_id='" + strUserID + "'");
        }
        BindGrid();
    }

    protected void Save(object sender, EventArgs e)
    {

    }
}