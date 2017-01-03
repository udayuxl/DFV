using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using ACE;

public partial class Admin_Level2 : System.Web.UI.Page
{
    string strMgrID = "0";
    string strProgramID = "0";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["LevlMgr1D"] != null)
        {
            strMgrID = Request.QueryString["LevlMgr1D"];
            string strCmd = "SELECT fk_Program_Id FROM Emerge.tblSM_Level1 WHERE pk_Sm_Level1_id = " + strMgrID;
            DataRow drPrgId = DataOperations.GetSingleRow(strCmd);
            strProgramID = (drPrgId[0]).ToString();
        }

        if (!IsPostBack)
        {
            BindGrid();
        }
    }        

    protected void gridLevel2Desg_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        string strID = DataBinder.Eval(e.Row.DataItem, "pk_Sm_Level2_id").ToString();
        ((TextBox)e.Row.FindControl("txtID")).Text = strID;

        string strSMID = DataBinder.Eval(e.Row.DataItem, "level2_sales_manager_id").ToString();
        ((TextBox)e.Row.FindControl("txtSMID")).Text = strSMID;

        HyperLink lnkManageMgrs = e.Row.FindControl("lnkManageMgrs") as HyperLink;
        lnkManageMgrs.NavigateUrl = "Level3Designation.aspx?LevlMgr1D=" + DataBinder.Eval(e.Row.DataItem, "pk_Sm_Level2_id").ToString();

        string strSubOrdCnt = DataBinder.Eval(e.Row.DataItem, "CntSubOrdinates").ToString();
        ((TextBox)e.Row.FindControl("txtSubOrdCnt")).Text = strSubOrdCnt;
    }

    // **** [buttons] ************************* 
    protected void Add(object sender, EventArgs e)
    {
        popAddEditID.Text = "0";
        popDesgname.Enabled = true;
        BindUsers();
    }

    // **** [Row buttons] *************************
    protected void Edit(object sender, EventArgs e)
    {
        
        ImageButton EditButton = sender as ImageButton;
        GridViewRow row = EditButton.Parent.Parent as GridViewRow;
        popAddEditID.Text = ((TextBox)row.FindControl("txtID")).Text;
        BindUsers();
        popDesgname.Text = row.Cells[1].Text;
        txtExistDesgName.Text = row.Cells[1].Text;
        if (((TextBox)row.FindControl("txtSubOrdCnt")).Text != "0")
        {
            popDesgname.Enabled = false;
            litUpdateDesgMag.Text = "The designation name can be changed only if there <br/> are no subordinates";
        }
        else
        {
            popDesgname.Enabled = true;
            litUpdateDesgMag.Text = "";
        }
        ddlUsers.SelectedValue = ((TextBox)row.FindControl("txtSMID")).Text; ;
    }
    protected void Delete(object sender, EventArgs e)
    {
        ImageButton Button = sender as ImageButton;
        GridViewRow row = Button.Parent.Parent as GridViewRow;
        string strID = ((TextBox)row.FindControl("txtID")).Text;
        string strCmd = "UPDATE Emerge.tblSM_Level2 SET Active = 0 WHERE pk_Sm_Level2_id = " + strID;
        string strResult = "";
        DataOperations.ExecuteSQL(strCmd,out strResult);
        if (strResult == "INUSE")
            failuremsg.Text = "This Designation cannot be deleted as it is in use.";
        else
        {
            successmsg.Text = "Designation deleted.";
            BindGrid();
        }
    }

    // **** [Popup buttons] ************************* 
    protected void Save(object sender, EventArgs e)
    {
        if (Validate() != "")
            return;

        string strID = popAddEditID.Text;
        string strUseriD = ddlUsers.SelectedItem.Value;
        
        string strCmd="";
        if (strID == "0")
        {
            strCmd = @"INSERT INTO Emerge.tblSM_Level2 (designation_name,level2_sales_manager_id, fk_Sm_Level1_id)
                VALUES ('" + popDesgname.Text.Replace("'", "''") + "','" + strUseriD + "', "+strMgrID+")";
        }
        else
        {
            strCmd = "UPDATE Emerge.tblSM_Level2 SET designation_name = '" + popDesgname.Text.Replace("'", "''") + "', ";
            strCmd += "level2_sales_manager_id='" + strUseriD + "' WHERE pk_Sm_Level2_id = " + strID;
        }
        DataOperations.ExecuteSQL(strCmd);
        popAddEditID.Text = "";
        popDesgname.Text = "";
        BindGrid();
    }
    protected void Cancel(object sender, EventArgs e)
    {
        popAddEditID.Text = "";
        popDesgname.Text = "";
        litValidationError.Text = "";
    }
    
    // ######### VALIDATION #############
    private string Validate()
    {
        string strErrorMessage = "";
        if (popDesgname.Text.Trim() == "")
            strErrorMessage += "Designation Name is required <br/>";
        if (ddlUsers.SelectedItem.Value.Trim() == "0")
            strErrorMessage += "Manager Name is required <br/>";
        litValidationError.Text = strErrorMessage;
        return strErrorMessage;
    }


    private void BindGrid()
    {
//        DataTable dtLevel1Desg = DataOperations.GetDataTable("SELECT SM2.pk_Sm_Level2_id, SM2.designation_name,SM2.budget, SM2.level2_sales_manager_id, SM2.Active, U.UserName FROM Emerge.tblSM_Level2 SM2 LEFT JOIN aspnet_Users U ON ( U.UserId = SM2.level2_sales_manager_id) WHERE SM2.fk_Sm_Level1_id = " + strMgrID);
       // DataTable dtLevel2Desg = DataOperations.GetDataTable("SELECT SM2.pk_Sm_Level2_id, SM2.designation_name,SM2.budget, SM2.level2_sales_manager_id, SM2.Active, U.UserName, MgrUsr.UserName AS MgrName FROM Emerge.tblSM_Level2 SM2 LEFT JOIN aspnet_Users U ON ( U.UserId = SM2.level2_sales_manager_id) LEFT JOIN Emerge.tblSM_Level1 SM1 ON ( SM1.pk_Sm_Level1_id  = SM2.fk_Sm_Level1_id) LEFT JOIN aspnet_Users MgrUsr ON ( MgrUsr.UserId = SM1.level1_sales_manager_id) WHERE SM2.fk_Sm_Level1_id = " + strMgrID);


        string strCmd = "";
//        strCmd = @"SELECT SM2.pk_Sm_Level2_id, SM2.designation_name,SM2.budget, SM2.level2_sales_manager_id, SM2.Active, U.UserName, 
//                    MgrUsr.UserName AS MgrName FROM Emerge.tblSM_Level2 SM2 
//                    LEFT JOIN aspnet_Users U ON ( U.UserId = SM2.level2_sales_manager_id) 
//                    LEFT JOIN Emerge.tblSM_Level1 SM1 ON ( SM1.pk_Sm_Level1_id  = SM2.fk_Sm_Level1_id) 
//                    LEFT JOIN aspnet_Users MgrUsr ON ( MgrUsr.UserId = SM1.level1_sales_manager_id) WHERE SM2.fk_Sm_Level1_id = " + strMgrID;


         strCmd = @"SELECT SM2.pk_Sm_Level2_id, SM2.designation_name,SM2.budget, SM2.level2_sales_manager_id, SM2.Active, U.UserName, 
                    MgrUsr.UserName AS MgrName, count( SM3.pk_Sm_Level3_id) AS CntSubOrdinates 
                    FROM Emerge.tblSM_Level2 SM2 
                    LEFT JOIN aspnet_Users U ON ( U.UserId = SM2.level2_sales_manager_id) 
                    LEFT JOIN Emerge.tblSM_Level1 SM1 ON ( SM1.pk_Sm_Level1_id  = SM2.fk_Sm_Level1_id) 
                    LEFT JOIN aspnet_Users MgrUsr ON ( MgrUsr.UserId = SM1.level1_sales_manager_id) 
                    LEFT JOIN Emerge.tblSM_Level3 SM3 ON ( SM3.fk_Sm_Level2_id = SM2.pk_Sm_Level2_id)
                    WHERE SM2.Active = 1 AND SM2.fk_Sm_Level1_id = " + strMgrID;
         strCmd += " GROUP BY SM2.pk_Sm_Level2_id, SM2.designation_name,SM2.budget, SM2.level2_sales_manager_id, SM2.Active, U.UserName, MgrUsr.UserName";


        DataTable dtLevel2Desg = DataOperations.GetDataTable(strCmd);


        gridLevel2Desg.DataSource = dtLevel2Desg;
        gridLevel2Desg.DataBind();        
    }

    private void BindUsers()
    {
        string strCmd = "";
        string strCurrId = popAddEditID.Text;
        strCmd = "SELECT U.UserId, U.UserName, R.RoleName FROM aspnet_Users U ";
        strCmd += "LEFT JOIN aspnet_UsersInRoles UIR ON ( UIR.UserId =U.UserId) ";
        strCmd += "LEFT JOIN aspnet_Roles R ON (R.RoleId = UIR.RoleId) ";
        strCmd += "WHERE U.UserId NOT IN ( ";
        strCmd += "SELECT MgrId FROM (SELECT level1_sales_manager_id AS MgrId FROM Emerge.tblSM_Level1 WHERE fk_Program_Id = " + strProgramID + " ";
        strCmd += "UNION SELECT level2_sales_manager_id AS MgrId FROM Emerge.tblSM_Level2 WHERE pk_Sm_Level2_id != " + strCurrId + " AND fk_Sm_Level1_id IN ( SELECT pk_Sm_Level1_id FROM Emerge.tblSM_Level1 WHERE fk_Program_Id = " + strProgramID + ") ";
        strCmd += "UNION SELECT level3_sales_manager_id AS MgrId FROM Emerge.tblSM_Level3 WHERE fk_Sm_Level2_id IN (SELECT pk_Sm_Level2_id FROM Emerge.tblSM_Level2 WHERE fk_Sm_Level1_id IN ( SELECT pk_Sm_Level1_id FROM Emerge.tblSM_Level1 WHERE fk_Program_Id = " + strProgramID + "))) ChartUsers) ";

        strCmd += "AND R.RoleName IN ( 'Level1', 'Level2','Level3','SM') ";
        DataTable dtUsers = DataOperations.GetDataTable(strCmd);
        ddlUsers.Items.Clear();
        ddlUsers.Items.Add(new ListItem("Select", "0"));
        ddlUsers.AppendDataBoundItems = true;
        ddlUsers.DataSource = dtUsers;
        ddlUsers.DataTextField = "UserName";
        ddlUsers.DataValueField = "UserId";
        ddlUsers.DataBind();
    }
    protected void GoBack(object sender, EventArgs e)
    {
        Response.Redirect("Level1Designation.aspx");
    }
}