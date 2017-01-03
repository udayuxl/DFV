using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using ACE;

public partial class Admin_Level1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            divCloneFrom.Visible = false;
            BindPrograms();
            BindGrid();
            BindProgramsWithHierarchy();
        }
    }

    private void BindPrograms()
    {
        DataTable dtPrograms = DataOperations.GetDataTable("SELECT * FROM Emerge.tblProgram ORDER BY pk_Program_Id");
        //ddlProgram.Items.Add(new ListItem("Select", "0"));
        ddlProgram.AppendDataBoundItems = true;
        ddlProgram.DataSource = dtPrograms;
        ddlProgram.DataTextField = "Name";
        ddlProgram.DataValueField = "pk_Program_Id";
        ddlProgram.DataBind();
    }

    private void BindProgramsWithHierarchy()
    {
        DataTable dtPrograms = DataOperations.GetDataTable("SELECT * FROM Emerge.tblProgram WHERE pk_Program_Id IN (SELECT DISTINCT fk_Program_Id FROM Emerge.tblSM_Level1) ORDER BY pk_Program_Id DESC");
     
        //ddlProgram.Items.Add(new ListItem("Select", "0"));
        ddlProgramToCloneFrom.AppendDataBoundItems = true;
        ddlProgramToCloneFrom.DataSource = dtPrograms;
        ddlProgramToCloneFrom.DataTextField = "Name";
        ddlProgramToCloneFrom.DataValueField = "pk_Program_Id";
        ddlProgramToCloneFrom.DataBind();
    }

    protected void gridLevel1Desg_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        string strID = DataBinder.Eval(e.Row.DataItem, "pk_Sm_Level1_id").ToString();
        ((TextBox)e.Row.FindControl("txtID")).Text = strID;

        string strSMID = DataBinder.Eval(e.Row.DataItem, "level1_sales_manager_id").ToString();
        ((TextBox)e.Row.FindControl("txtSMID")).Text = strSMID;

        HyperLink lnkManageMgrs = e.Row.FindControl("lnkManageMgrs") as HyperLink;
        lnkManageMgrs.NavigateUrl = "Level2Designation.aspx?LevlMgr1D=" + DataBinder.Eval(e.Row.DataItem, "pk_Sm_Level1_id").ToString();

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
        popDesgname.Text = row.Cells[0].Text;
        txtExistDesgName.Text = row.Cells[0].Text;
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
        ddlUsers.SelectedValue = ((TextBox)row.FindControl("txtSMID")).Text;
    }
    protected void Delete(object sender, EventArgs e)
    {
        ImageButton Button = sender as ImageButton;
        GridViewRow row = Button.Parent.Parent as GridViewRow;
        string strID = ((TextBox)row.FindControl("txtID")).Text;
        string strCmd = "UPDATE Emerge.tblSM_Level1 SET Active = 0 WHERE pk_Sm_Level1_id = " + strID;
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

        string strProgramID = "0";
        strProgramID = ddlProgram.SelectedItem.Value;
        string strID = popAddEditID.Text;
        string strUseriD = ddlUsers.SelectedItem.Value;
        
        string strCmd="";
        if (strID == "0")
        {
            strCmd = @"INSERT INTO Emerge.tblSM_Level1 (designation_name,level1_sales_manager_id,fk_Program_Id)
                VALUES ('" + popDesgname.Text.Replace("'", "''") + "','" + strUseriD + "', " + strProgramID + ")";
        }
        else
        {
            strCmd = "UPDATE Emerge.tblSM_Level1 SET designation_name = '" + popDesgname.Text.Replace("'", "''") + "', ";
            strCmd += "level1_sales_manager_id='" + strUseriD + "' WHERE pk_Sm_Level1_id = " + strID;
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
        string strCmd = "";
//        strCmd = @"SELECT SM1.pk_Sm_Level1_id, SM1.designation_name,SM1.budget, SM1.level1_sales_manager_id, SM1.Active, U.UserName 
//                    FROM Emerge.tblSM_Level1 SM1 LEFT JOIN aspnet_Users U ON ( U.UserId = SM1.level1_sales_manager_id)";

        String selectedPrgValue = ddlProgram.SelectedItem.Value.ToString();
        strCmd = @"SELECT SM1.pk_Sm_Level1_id, SM1.designation_name,SM1.budget, SM1.level1_sales_manager_id, SM1.Active, U.UserName 
                    , count( SM2.pk_Sm_Level2_id) AS CntSubOrdinates
                    FROM Emerge.tblSM_Level1 SM1 LEFT JOIN aspnet_Users U ON ( U.UserId = SM1.level1_sales_manager_id)
                    LEFT JOIN Emerge.tblSM_Level2 SM2 ON ( SM2.fk_Sm_Level1_id = SM1.pk_Sm_Level1_id)
                    WHERE SM1.Active = 1 AND SM1.fk_Program_Id= "+selectedPrgValue;
         strCmd += " GROUP BY SM1.pk_Sm_Level1_id, SM1.designation_name,SM1.budget, SM1.level1_sales_manager_id, SM1.Active, U.UserName ";


        DataTable dtLevel1Desg = DataOperations.GetDataTable(strCmd);
        gridLevel1Desg.DataSource = dtLevel1Desg;
        gridLevel1Desg.DataBind();
        if (dtLevel1Desg.Rows.Count == 0)
            divCloneFrom.Visible = true;
        else
            divCloneFrom.Visible = false;

    }

    private void BindUsers()
    {
        string strProgramID = "0";
        strProgramID =ddlProgram.SelectedItem.Value;
        string strCmd = "";
        string strCurrId = popAddEditID.Text;
        strCmd = "SELECT U.UserId, U.UserName, R.RoleName FROM aspnet_Users U ";
        strCmd += "LEFT JOIN aspnet_UsersInRoles UIR ON ( UIR.UserId =U.UserId) ";
        strCmd += "LEFT JOIN aspnet_Roles R ON (R.RoleId = UIR.RoleId) ";
        strCmd += "WHERE U.UserId NOT IN ( ";
        strCmd += "SELECT MgrId FROM (SELECT level1_sales_manager_id AS MgrId FROM Emerge.tblSM_Level1 WHERE fk_Program_Id = " + strProgramID + " AND pk_Sm_Level1_id != " + strCurrId + " ";
        strCmd += "UNION SELECT level2_sales_manager_id AS MgrId FROM Emerge.tblSM_Level2 WHERE fk_Sm_Level1_id IN ( SELECT pk_Sm_Level1_id FROM Emerge.tblSM_Level1 WHERE fk_Program_Id = " + strProgramID + ") ";
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


    protected void ProgramSelected(object sender, EventArgs e)
    {
        string strProgramID = ddlProgram.SelectedItem.Value;
        BindGrid();
    }
    protected void CloneHierarchy(object sender, EventArgs e)
    {
        string strProgramIDToClone = ddlProgram.SelectedItem.Value;
        string strProgramIDToCloneFrom = ddlProgramToCloneFrom.SelectedItem.Value;



        string strCmd = "";
        strCmd = "INSERT INTO Emerge.tblSM_Level1 (designation_name, level1_sales_manager_id, Active, fk_Program_Id) ";
        strCmd +=" SELECT designation_name, level1_sales_manager_id, Active, "+strProgramIDToClone+" AS fk_Program_Id  FROM Emerge.tblSM_Level1 WHERE fk_Program_Id = "+strProgramIDToCloneFrom;
        DataOperations.ExecuteSQL(strCmd);

        strCmd = "INSERT INTO Emerge.tblSM_Level2 ( designation_name,level2_sales_manager_id, Active, fk_Sm_Level1_id) ";
        strCmd += " SELECT designation_name, level2_sales_manager_id, Active, Mapping.New_pk_Sm_Level1_id AS fk_Sm_Level1_id";
        strCmd +=" FROM Emerge.tblSM_Level2";
        strCmd += " LEFT JOIN ( SELECT OLD.pk_Sm_Level1_id AS Old_pk_Sm_Level1_id,  NEW.pk_Sm_Level1_id AS New_pk_Sm_Level1_id FROM  (SELECT * FROM Emerge.tblSM_Level1 WHERE fk_Program_Id = " + strProgramIDToCloneFrom + ") OLD";
        strCmd += " LEFT JOIN (SELECT * FROM Emerge.tblSM_Level1 WHERE fk_Program_Id = " + strProgramIDToClone + ") NEW ON (OLD.designation_name = NEW.designation_name) AND (OLD.level1_sales_manager_id = NEW.level1_sales_manager_id)";
        strCmd +=" ) Mapping ON Mapping.Old_pk_Sm_Level1_id =  Emerge.tblSM_Level2.fk_Sm_Level1_id";
        strCmd += " WHERE fk_Sm_Level1_id IN (SELECT pk_Sm_Level1_id FROM Emerge.tblSM_Level1 WHERE fk_Program_Id = " + strProgramIDToCloneFrom + ")";
        DataOperations.ExecuteSQL(strCmd);



        strCmd = "INSERT INTO Emerge.tblSM_Level3 ( designation_name,level3_sales_manager_id, Active, fk_Sm_Level2_id) ";
        strCmd += " SELECT designation_name, level3_sales_manager_id, Active, Mapping.New_pk_Sm_Level2_id AS fk_Sm_Level2_id ";
        strCmd += " FROM Emerge.tblSM_Level3 ";
        strCmd += " LEFT JOIN ( ";
        strCmd += " SELECT OLD.pk_Sm_Level2_id AS Old_pk_Sm_Level2_id,  NEW.pk_Sm_Level2_id AS New_pk_Sm_Level2_id FROM  ";
        strCmd += " (SELECT * FROM Emerge.tblSM_Level2 WHERE fk_Sm_Level1_id IN (SELECT pk_Sm_Level1_id FROM Emerge.tblSM_Level1 WHERE fk_Program_Id = " + strProgramIDToCloneFrom + ")) OLD ";
        strCmd += " LEFT JOIN (SELECT * FROM Emerge.tblSM_Level2 WHERE fk_Sm_Level1_id IN (SELECT pk_Sm_Level1_id FROM Emerge.tblSM_Level1 WHERE fk_Program_Id = " + strProgramIDToClone + ") ";
        strCmd += " ) NEW ON (OLD.designation_name = NEW.designation_name) AND (OLD.level2_sales_manager_id = NEW.level2_sales_manager_id) ";
        strCmd += " ) Mapping ON Mapping.Old_pk_Sm_Level2_id =  Emerge.tblSM_Level3.fk_Sm_Level2_id ";
        strCmd += " WHERE fk_Sm_Level2_id IN (SELECT pk_Sm_Level2_id FROM Emerge.tblSM_Level2 WHERE fk_Sm_Level1_id IN (SELECT pk_Sm_Level1_id FROM Emerge.tblSM_Level1 WHERE fk_Program_Id = " + strProgramIDToCloneFrom + "))";
        DataOperations.ExecuteSQL(strCmd);

        BindGrid();
    }

}