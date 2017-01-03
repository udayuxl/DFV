using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Data;
using ACE;

public partial class Admin_AssignBudget : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadGrid();
    }

    private void LoadGrid()
    {
        DataTable dt = DataOperations.GetDataTable("SELECT * FROM aspnet_Users U JOIN aspnet_UsersInRoles UIR ON UIR.UserId=U.UserId JOIN aspnet_Roles R ON R.RoleId = UIR.RoleId WHERE RoleName = 'DVP'");
        gridDVPUsers.DataSource = dt;
        gridDVPUsers.DataBind();
    }

    protected void gridDVPUsers_ItemDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.Footer)
        //{
        //    Label lblBudgetTotal = e.Row.FindControl("lblBudgetTotal") as Label;
        //    lblBudgetTotal.Text = String.Format("{0:C}", GridTotal);
        //}

        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        string strColumn1 = e.Row.Cells[0].Text;
        if (!strColumn1.Contains("Western"))
            ;// e.Row.Cells[0].Text = strColumn1 + " - " + Convert.ToString(DataBinder.Eval(e.Row.DataItem, "DVPName"));
        TextBox txtAddBudget = e.Row.FindControl("txtAddBudget") as TextBox;
        HiddenField hdnUserID = e.Row.FindControl("hdnUserID") as HiddenField;
        Label lblBudget = e.Row.FindControl("lblBudget") as Label;

        txtAddBudget.Text = "0";
        hdnUserID.Value = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "UserID"));
        txtAddBudget.Attributes.Add("placeholdertext", txtAddBudget.Text);
        //if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "SortWeight")) == "1") //Indicates Self
          //  txtAddBudget.Visible = false;

        //e.Row.Cells[2].Text = String.Format("{0:C}",BudgetServices.GetProgramBudgetForUser(hdnUserID.Value, ddlProgram.SelectedItem.Value));

        //Decimal theBudget = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Budget").ToString());
        //GridTotal += theBudget;

        //lblBudget.Text = String.Format("{0:C}", theBudget);
    }



    


}