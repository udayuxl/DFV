using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using Lumen;
using ACE;

public partial class Admin_AllocateBudget : System.Web.UI.Page
{
    private Decimal GridTotal = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        //bool Allowed = Roles.IsUserInRole("Admin") || Roles.IsUserInRole("DVP") || Roles.IsUserInRole("SVP") || Roles.IsUserInRole("DM") || Roles.IsUserInRole("SM") || Roles.IsUserInRole("UXL");
        //bool Allowed = Roles.IsUserInRole("Admin") || Roles.IsUserInRole("MM") || Roles.IsUserInRole("Level1") || Roles.IsUserInRole("Level2") || Roles.IsUserInRole("UXL");
        bool Allowed = Roles.IsUserInRole("Admin") || Roles.IsUserInRole("UXL") || Roles.IsUserInRole("MM") || AdminServices.IsUserInLevel("Level1") || AdminServices.IsUserInLevel("Level2") || AdminServices.IsUserInLevel("Level3");

        if (!Allowed)
            Response.Redirect("~/Home.aspx");

        if (!IsPostBack)
        {
            BindPrograms();
        }
        if (AdminServices.IsUserInLevel("Level3"))
        {
            Button1.Visible = false;
            Button2.Visible = false;
        }
    }

    private void BindPrograms()
    {
        DataTable dtPrograms = DataOperations.GetDataTable("SELECT * FROM Emerge.tblProgram");
        ddlProgram.Items.Add(new ListItem("Select","0"));
        ddlProgram.AppendDataBoundItems = true;
        ddlProgram.DataSource = dtPrograms;
        ddlProgram.DataTextField = "Name";
        ddlProgram.DataValueField = "pk_Program_Id";
        ddlProgram.DataBind();
    }

    protected void gridRVPUsers_ItemDataBound(object sender, GridViewRowEventArgs e)
    {
        string strProgramID = ddlProgram.SelectedItem.Value;
        if (AdminServices.IsUserInLevel("Level3", strProgramID))
            e.Row.Cells[3].Visible = false;
        //if (AdminServices.IsUserInLevel("Level2", strProgramID))
        //    e.Row.Cells[3].Visible = false;
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblBudgetTotal = e.Row.FindControl("lblBudgetTotal") as Label;                        
            lblBudgetTotal.Text = String.Format("{0:C}", GridTotal);
            Label lblBudgetTotalUpd = e.Row.FindControl("lblBudgetTotalUpd") as Label;
            lblBudgetTotalUpd.Text = String.Format("{0:C}", GridTotal);
        }

        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Level1")) % 2 == 1)
        {
            foreach (TableCell tcell in e.Row.Cells)
                tcell.Attributes.Add("style", "background:#F1D9B8"); //F1D9B8
        }
        else
        {
            foreach (TableCell tcell in e.Row.Cells)
                tcell.Attributes.Add("style", "background:#E9DD87"); //E9DD87
        }

        string strColumn1 = e.Row.Cells[0].Text;
        if (!strColumn1.Contains("Western"))
            ;// e.Row.Cells[0].Text = strColumn1 + " - " + Convert.ToString(DataBinder.Eval(e.Row.DataItem, "DVPName"));
        TextBox txtAddBudget = e.Row.FindControl("txtAddBudget") as TextBox;
        HiddenField hdnBudRecID = e.Row.FindControl("hdnBudRecID") as HiddenField;
        HiddenField hdnLevel = e.Row.FindControl("hdnLevel") as HiddenField;
        HiddenField hdnLevel1 = e.Row.FindControl("hdnLevel1") as HiddenField;
        HiddenField hdnLevel2 = e.Row.FindControl("hdnLevel2") as HiddenField;
        HiddenField hdnLevel3 = e.Row.FindControl("hdnLevel3") as HiddenField;
        Label lblBudget = e.Row.FindControl("lblBudget") as Label;
        HiddenField hdnMktBud = e.Row.FindControl("hdnMktBud") as HiddenField;
        Label lblBudgetUpd = e.Row.FindControl("lblBudgetUpd") as Label;

        txtAddBudget.Text = "0";
        hdnBudRecID.Value = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Level_Budget_id"));
        hdnLevel.Value = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Level"));
        hdnLevel1.Value = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Level1"));
        hdnLevel2.Value = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Level2"));
        hdnLevel3.Value = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Level3"));

        txtAddBudget.Attributes.Add("placeholdertext", txtAddBudget.Text);
        if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "SortWeight")) == "1") //Indicates Self
            txtAddBudget.Visible = false;

        //e.Row.Cells[2].Text = String.Format("{0:C}",BudgetServices.GetProgramBudgetForUser(hdnUserID.Value, ddlProgram.SelectedItem.Value));

        Decimal theBudget = Convert.ToDecimal( DataBinder.Eval(e.Row.DataItem, "Budget").ToString() );
        GridTotal += theBudget;

        lblBudget.Text = String.Format("{0:C}", theBudget);
        hdnMktBud.Value = Convert.ToString(theBudget);
        lblBudgetUpd.Text = String.Format("{0:C}", theBudget);

    }

   

    protected void ProgramSelected(object sender, EventArgs e)
    {
        string strProgramID = ddlProgram.SelectedItem.Value;
        if (strProgramID == "0")
        {
            divAssignBudget.Visible = false;
            divAssignPrgBudget.Visible = false;
            return;
        }
        divAssignBudget.Visible = true;
        divAssignPrgBudget.Visible = true;
        hdnProgramStarted.Value = BudgetServices.IsProgramStarted(strProgramID);

        if (Roles.IsUserInRole("Admin") || Roles.IsUserInRole("MM") )
        {
            gridRVPUsers.Columns[0].HeaderText = "Position";

            DataTable dtDVPBudgetsForProgram = BudgetServices.GetTopLevelBudgetAllocations(strProgramID);
            gridRVPUsers.DataSource = dtDVPBudgetsForProgram;
            gridRVPUsers.DataBind();
                        
            divYourBudget.Visible = false;
            txtProgramBudget.Enabled = true;
            btnProgramBudget.Visible = true;
        }
        //else if (Roles.IsUserInRole("DVP") || Roles.IsUserInRole("DM"))
        else if (AdminServices.IsUserInLevel("Level1", strProgramID) || AdminServices.IsUserInLevel("Level2", strProgramID) || AdminServices.IsUserInLevel("Level3", strProgramID))
        {
            gridRVPUsers.Columns[0].HeaderText = "Position";

            DataTable dtDVPBudgetsForProgram = BudgetServices.GetMyTeamBudgetAllocations(strProgramID);
            gridRVPUsers.DataSource = dtDVPBudgetsForProgram;
            gridRVPUsers.DataBind();
                        
            //Response.Write(drBudgetParts["BudgetAllocatedToMe"].ToString() + "<br/>");
            //Response.Write(drBudgetParts["BudgetSpent"].ToString());
            //Response.Write(drBudgetParts["BudgetRemaining"].ToString());

            DataRow drMyAllocatedBudget = BudgetServices.GetMyAllocatedBudget(strProgramID);
            if (drMyAllocatedBudget == null)
                txtYourBudget.Text = "$0";
            else
                txtYourBudget.Text = String.Format("{0:C}", Convert.ToDecimal(drMyAllocatedBudget["budget_Allocated"].ToString()));
            divYourBudget.Visible = true;
        }
        DataRow drProgramBudget = BudgetServices.GetBudgetForProgram(strProgramID);
        if (drProgramBudget == null)
            txtProgramBudget.Text = "$0";
        else
            txtProgramBudget.Text = String.Format("{0:C}", Convert.ToDecimal(drProgramBudget["Budget"].ToString()));

        if (AdminServices.IsUserInLevel("Level3", strProgramID))
        {
            Button1.Visible = false;
            Button2.Visible = false;
        }
        else
        {
            Button1.Visible = true;
            Button2.Visible = true;
        }
    }

    protected void Save(object sender, EventArgs e)
    {
        string strMessage = "true";
        Decimal marketBudgetAvailableForAllocation = Convert.ToDecimal(BudgetServices.GetMyBudgetDetails(ddlProgram.SelectedItem.Value, "0")["BudgetAllocatedToMe"].ToString());
            
            

        // First Loop to validate sum of all text boxes
        Decimal totalBudgetBeingAllocated = 0;
        foreach (GridViewRow row in gridRVPUsers.Rows)
        {
            TextBox txtAddBudget = row.FindControl("txtAddBudget") as TextBox;            
            HtmlGenericControl divMsg = row.FindControl("divMsg") as HtmlGenericControl;
            Decimal addBudget = Convert.ToDecimal(txtAddBudget.Text);
            totalBudgetBeingAllocated += addBudget;            
        }

        

//        if (!Roles.IsUserInRole("Admin")) // if admin no need to check current user's budget first

        if ((Roles.IsUserInRole("Admin") || Roles.IsUserInRole("MM")) == false)
        {
            if (marketBudgetAvailableForAllocation - totalBudgetBeingAllocated < 0)
            {
                //failuremsg.Text = "Could not save the allocations because:<br/><br/> <b>The sum of budgets being allocated (" + String.Format("{0:C}", totalBudgetBeingAllocated) + ") <br/>is more than Marketing Budget available (" + String.Format("{0:C}", marketBudgetAvailableForAllocation) + ")";
                failuremsg.Text = "Amount allocated exceeds the budget available.  Please reduce allocation.";
                return;
            }
        }

        // Second Loop that actually adds the budget to each user
        foreach(GridViewRow row in gridRVPUsers.Rows)
        {
            TextBox txtAddBudget = row.FindControl("txtAddBudget") as TextBox;
            HiddenField hdnBudRecID = row.FindControl("hdnBudRecID") as HiddenField;
            HiddenField hdnLevel = row.FindControl("hdnLevel") as HiddenField;
            HiddenField hdnLevel1 = row.FindControl("hdnLevel1") as HiddenField;
            HiddenField hdnLevel2 = row.FindControl("hdnLevel2") as HiddenField;
            HiddenField hdnLevel3 = row.FindControl("hdnLevel3") as HiddenField;

            HtmlGenericControl divMsg = row.FindControl("divMsg") as HtmlGenericControl;
            HiddenField hdnMktBud = row.FindControl("hdnMktBud") as HiddenField;
            Decimal currUtilizedBudget = 0;

            DataRow drProgramUtilizedBudget = BudgetServices.GetBudgetUtilizedForProgram(ddlProgram.SelectedItem.Value);
            if (drProgramUtilizedBudget == null)
                currUtilizedBudget = 0;
            else
                currUtilizedBudget = Convert.ToDecimal(drProgramUtilizedBudget["BudgetUtilized"]);
            Decimal currPrgBudget = 0;
            DataRow drProgramBudget = BudgetServices.GetBudgetForProgram(ddlProgram.SelectedItem.Value);
            if (drProgramBudget == null)
                currPrgBudget = 0;
            else
                currPrgBudget =Convert.ToDecimal(drProgramBudget["Budget"]);
            /* Commented to Enable to User to reduce even if program is started
            if ((hdnProgramStarted.Value == "Yes") && (Convert.ToDecimal(txtAddBudget.Text) < 0) )
            {
                strMessage = "The budget cannot be reduced as the program window is open";
            }
            else 
                */
            if ( (Convert.ToDecimal(txtAddBudget.Text) < 0)  &&  ((Convert.ToDecimal(txtAddBudget.Text) + Convert.ToDecimal(hdnMktBud.Value))  <0 ))
            {
                strMessage = "Marketing Budget cannot be less than 0.";
            }
            else if ( ((currUtilizedBudget + Convert.ToDecimal(txtAddBudget.Text)) > currPrgBudget) && (!( (AdminServices.IsUserInLevel("Level1")) || (AdminServices.IsUserInLevel("Level2")) || (AdminServices.IsUserInLevel("Level3"))) ))
            {
                failuremsg.Text = "You are exceeding the budget allocated for the program.<br/>Please allocate an amount within the Program’s budget.";
                return;
            }
            else
            {
                strMessage = BudgetServices.AddProgramBudgetToPosition(hdnLevel.Value, hdnLevel1.Value, hdnLevel2.Value, hdnLevel3.Value, hdnBudRecID.Value, ddlProgram.SelectedItem.Value, Convert.ToDecimal(txtAddBudget.Text));
            }
            if (strMessage == "true")
            {
                //divMsg.InnerHtml = "<font color='green'>Saved</font>";
                //row.Cells[2].Text = Convert.ToString(BudgetServices .GetBudgetAllocated(hdnUserID.Value, ddlProgram.SelectedItem.Value));
                txtAddBudget.Text = "0";
                divMsg.InnerHtml = "";
            }
            else if (strMessage == "Insufficient Budget")
            {
                divMsg.InnerHtml = "<font color='red'>Insufficient budget</font>";
                break;
            }
            else
            {
                divMsg.InnerHtml = "<font color='red'>" + strMessage + "</font>";
                break;
            }
        }

        if (strMessage != "true")
            failuremsg.Text = "Could not save all budget allocations. <br/><br/> Please see below";
        else
        {
            successmsg.Text = "Saved successfully";
            ProgramSelected(null, null);  //Reloading Grid
        }
        DataRow drMyAllocatedBudget = BudgetServices.GetMyAllocatedBudget(ddlProgram.SelectedItem.Value);
        if (drMyAllocatedBudget == null)
            txtYourBudget.Text = "$0";
        else
            txtYourBudget.Text = String.Format("{0:C}", Convert.ToDecimal(drMyAllocatedBudget["budget_Allocated"].ToString()));
    }

    protected void Cancel(object sender, EventArgs e)
    {
        ProgramSelected(null, null);  //Reloading Grid
    }

    protected void UpdatePrgBudget(object sender, EventArgs e)
    {
        Decimal currUtilizedBudget = 0;
        Decimal newBudget = 0;
        DataRow drProgramUtilizedBudget = BudgetServices.GetBudgetUtilizedForProgram(ddlProgram.SelectedItem.Value);
        if (drProgramUtilizedBudget == null)
            currUtilizedBudget = 0;
        else
            currUtilizedBudget = Convert.ToDecimal(drProgramUtilizedBudget["BudgetUtilized"]);

        newBudget = Convert.ToDecimal((txtProgramBudget.Text).Replace("$", ""));
        if (currUtilizedBudget > newBudget)
        {
            failuremsg.Text = "You have already allocated $ " + currUtilizedBudget.ToString() + ".<br/>Please reduce allocation to individuals before reducing the budget for the selected Program.";
            return;
        }

        string strMessage;
        strMessage = BudgetServices.AddProgramBudget(ddlProgram.SelectedItem.Value, Convert.ToDecimal((txtProgramBudget.Text).Replace("$", "") ));
        if (strMessage != "Updated")
            failuremsg.Text = "Failed to Update Program Budget due to following reason "+strMessage;
        else
            successmsg.Text = "Budget updated successfully!";

    }
}