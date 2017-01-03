using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using ACE;
using Lumen;

public partial class Admin_ManageUsers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();
            DataTable lstRegions;
            lstRegions = DataOperations.GetDataTable(" SELECT pk_location_ID, Location, 1 AS displayOrder FROM Emerge.tblLocation WHERE LocationType = 'State' UNION SELECT 9999 AS pk_location_ID, 'ALL' AS Location, 0 AS displayOrder ORDER BY displayOrder, Location ");
            lvwSelectedRegion.DataSource = lstRegions.DataSet;
            lvwSelectedRegion.DataBind();
        }
    }        

    protected void gridUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        string strUserId = DataBinder.Eval(e.Row.DataItem, "UserId").ToString();
        ((TextBox)e.Row.FindControl("txtUserId")).Text = strUserId;

        string strActive = DataBinder.Eval(e.Row.DataItem, "Active").ToString();
        ((TextBox)e.Row.FindControl("txtActive")).Text = strActive;


        string strSMgr = DataBinder.Eval(e.Row.DataItem, "SalesManager").ToString();
        ((TextBox)e.Row.FindControl("txtSalesRole")).Text = strSMgr;


        string strMMgr = DataBinder.Eval(e.Row.DataItem, "MarketingManager").ToString();
        ((TextBox)e.Row.FindControl("txtMarMgrRole")).Text = strMMgr;

        string strAdminMgr = DataBinder.Eval(e.Row.DataItem, "Admin").ToString();
        ((TextBox)e.Row.FindControl("txtAdminRole")).Text = strAdminMgr;

        string strUserName = DataBinder.Eval(e.Row.DataItem, "UserName").ToString();
        ((TextBox)e.Row.FindControl("txtUserName")).Text = strUserName;

    }

    // **** [buttons] ************************* 

    protected void Reset(object sender, EventArgs e)
    {

        LinkButton lnkReset = (LinkButton)(sender);
        GridViewRow row = lnkReset.Parent.Parent as GridViewRow;
        string strUserName = ((TextBox)row.FindControl("txtUserName")).Text;
        try
        {
            string temppwd;
            MembershipUser mu = Membership.GetUser(strUserName);
            if (mu.IsLockedOut)
                mu.UnlockUser();
            temppwd = mu.ResetPassword();
            mu.ChangePassword(temppwd, "password!");

            MailServices.SendPasswordEmail(strUserName);
        }
        catch (Exception ex)
        {
        }
        BindGrid();
        
    }
    protected void Add(object sender, EventArgs e)
    {
        popAddEditID.Text = "0";
        popFName.Enabled = true;
        popLName.Enabled = true;
        popEmail.Enabled = true;
        foreach (ListViewDataItem listItem in lvwSelectedRegion.Items)
        {
            CheckBox chkRegion = (CheckBox)listItem.FindControl("chkRegion");
            chkRegion.Checked = false;
        }
    }

    // **** [Row buttons] *************************
    protected void Edit(object sender, EventArgs e)
    {
        
        ImageButton EditButton = sender as ImageButton;
        GridViewRow row = EditButton.Parent.Parent as GridViewRow;
        popAddEditID.Text = ((TextBox)row.FindControl("txtUserId")).Text;
        popTxtActive.Text = ((TextBox)row.FindControl("txtActive")).Text;
        popTxtAdminRole.Text = ((TextBox)row.FindControl("txtAdminRole")).Text;
        popTxtMarMgrRole.Text = ((TextBox)row.FindControl("txtMarMgrRole")).Text;
        popTxtSalesRole.Text = ((TextBox)row.FindControl("txtSalesRole")).Text;

        string[] arrUserName;
        string strUserName = row.Cells[0].Text;
        arrUserName = (strUserName.Trim()).Split(' ');
        if (arrUserName.Length > 0)
           popFName.Text = arrUserName[0];
        if (arrUserName.Length > 1)
        {
            if (arrUserName.Length == 2)
                popLName.Text = arrUserName[1];
            if (arrUserName.Length == 3)
                popLName.Text = (arrUserName[1] == "" ? "" : arrUserName[1]+" ") + arrUserName[2];
        }
        popEmail.Text = row.Cells[1].Text;

        if (popTxtActive.Text.ToLower().Equals("true"))
            popActive.Checked = true;
        else
            popActive.Checked = false;
        if (popTxtSalesRole.Text.ToLower().Equals("true"))
            popSalesRole.Checked = true;
        else
            popSalesRole.Checked = false;

        if (popTxtMarMgrRole.Text.ToLower().Equals("true"))
            popMarMgrRole.Checked = true;
        else
            popMarMgrRole.Checked = false;
        if (popTxtAdminRole.Text.ToLower().Equals("true"))
            popAdminRole.Checked = true;
        else
            popAdminRole.Checked = false;

        foreach (ListViewDataItem listItem in lvwSelectedRegion.Items)
        {
            CheckBox chkRegion = (CheckBox)listItem.FindControl("chkRegion");
            chkRegion.Checked = false;
            string strListOfRegions = "";
            string strUserid = popAddEditID.Text.ToString();
            DataRow dr = DataOperations.GetSingleRow("SELECT [Lumen].[fnGetRegionsForUser]('" + strUserid + "') AS Regions");
            strListOfRegions = dr["Regions"].ToString();
            if (strListOfRegions != "")
            {
                char[] delimiters = new char[] { ',' };
                string[] arrListofRegions = strListOfRegions.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                for (int nCount = 0; nCount < arrListofRegions.Length; nCount++)
                {
                    if (arrListofRegions[nCount].ToString().Trim() == chkRegion.Attributes["pk_location_ID"].ToString().Trim())
                        chkRegion.Checked = true;
                }
            }
        }
    }
    protected void Delete(object sender, EventArgs e)
    {
        ImageButton Button = sender as ImageButton;
        GridViewRow row = Button.Parent.Parent as GridViewRow;
        string strID = ((TextBox)row.FindControl("txtUserId")).Text;
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

        string strID = popAddEditID.Text;
        string strUserName = popFName.Text + " " + popLName.Text;
        string strUserID = popAddEditID.Text;
        string strEmail = popEmail.Text;
        string strActive = popActive.Checked.ToString();
        
        string strCmd="";
        if (strUserID == "0") // New User
        {
            MembershipUser mu = Membership.CreateUser(strUserName, "password!", strEmail);
            if (popAdminRole.Checked)
                Roles.AddUserToRole(strUserName, "Admin");
            if (popMarMgrRole.Checked)
                Roles.AddUserToRole(strUserName, "MM");
            //if (popSalesRole.Checked)
            //    Roles.AddUserToRole(strUserName, "Level1");
            if (popSalesRole.Checked)
                Roles.AddUserToRole(strUserName, "SM");
        }
        else // Edit user
        {
            strCmd = "UPDATE aspnet_Users SET  UserName = '" + strUserName + "', LoweredUserName = '" + strUserName.ToLower() + "' WHERE UserId = '" + strUserID + "'";
            DataOperations.ExecuteSQL(strCmd);
            MembershipUser mu = Membership.GetUser(strUserName);
            mu.Email = strEmail;
            Membership.UpdateUser(mu);
            if (strActive.ToLower().Equals("true"))
                mu.UnlockUser();
            if ((strActive.ToLower().Equals("false")) && (mu.IsLockedOut == false))
            {
                 strCmd = "UPDATE aspnet_Membership SET isLockedOut = 1 WHERE UserId = '"+strUserID+"'";
                 DataOperations.ExecuteSQL(strCmd);
            }

           
            if (popAdminRole.Checked)
            {
                if (!Roles.IsUserInRole(strUserName, "Admin"))
                {
                    Roles.AddUserToRole(strUserName, "Admin");
                }
            }
            else
            {
                if (Roles.IsUserInRole(strUserName, "Admin"))
                {
                    Roles.RemoveUserFromRole(strUserName, "Admin");
                }
            }

            if (popMarMgrRole.Checked)
            {
                if (!Roles.IsUserInRole(strUserName, "MM"))
                {
                    Roles.AddUserToRole(strUserName, "MM");
                }
            }
            else
            {
                if (Roles.IsUserInRole(strUserName, "MM"))
                {
                    Roles.RemoveUserFromRole(strUserName, "MM");
                }
            }

            if (popSalesRole.Checked)
            {
                if (!Roles.IsUserInRole(strUserName, "SM"))
                {
                    Roles.AddUserToRole(strUserName, "SM");
                }
                /*
                if (!Roles.IsUserInRole(strUserName, "Level1"))
                {
                    Roles.AddUserToRole(strUserName, "Level1");
                }
                */
            }
            else
            {
                if (Roles.IsUserInRole(strUserName, "SM"))
                {
                    Roles.RemoveUserFromRole(strUserName, "SM");
                }

                if (Roles.IsUserInRole(strUserName, "Level1"))
                {
                    Roles.RemoveUserFromRole(strUserName, "Level1");
                }
                if (Roles.IsUserInRole(strUserName, "Level2"))
                {
                    Roles.RemoveUserFromRole(strUserName, "Level2");
                }
                if (Roles.IsUserInRole(strUserName, "Level3"))
                {
                    Roles.RemoveUserFromRole(strUserName, "Level3");
                }
            }

            // Regions Code
            string strListOfRegions = "";
            foreach (ListViewDataItem listItem in lvwSelectedRegion.Items)
            {
                CheckBox chkRegion = (CheckBox)listItem.FindControl("chkRegion");
                if (chkRegion.Checked == true)
                {
                    strListOfRegions += chkRegion.Attributes["pk_location_ID"].ToString().Trim() + ", ";
                }
            }
            if (strListOfRegions.Length >= 2)
            {
                strListOfRegions = strListOfRegions.Substring(0, strListOfRegions.Length - 2);
                if (strListOfRegions.Contains("9999"))
                {
                    //strListOfRegions = "1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53 "; // All values of states in tblLocation
                    strCmd = "DECLARE @StateIds VARCHAR(8000) ";
                    strCmd += "SELECT @StateIds =  COALESCE(@StateIds+', ', '')+CAST(pk_location_ID AS varchar(10)) FROM Emerge.tblLocation WHERE LocationType = 'State' ";
                    strCmd +="SELECT  @StateIds AS Stateids ";
                    DataRow dr = DataOperations.GetSingleRow(strCmd);
                    strListOfRegions = dr["Stateids"].ToString();
                }
                //Insert Regions which are in seleted not and not exists in system
                strCmd = "INSERT INTO Emerge.tblUserPortfolio (fk_User_ID, fk_location_ID) ";
                strCmd += "SELECT '" + strUserID + "' AS fk_User_ID, L.pk_location_ID AS fk_location_ID ";
                strCmd += " FROM  Emerge.tblLocation L LEFT JOIN ";
                strCmd += " Emerge.tblUserPortfolio UP ON (UP.fk_Location_ID= L.pk_location_ID) AND UP.fk_User_ID = '" + strUserID + "' ";
                strCmd += " WHERE pk_location_ID IN (" + strListOfRegions + ") ";
                strCmd += " AND UP.pk_UserPortfolio_ID IS NULL";
                DataOperations.ExecuteSQL(strCmd);

                //DELETE Regions  where Not in Selected List		
                strCmd = "DELETE FROM Emerge.tblUserPortfolio WHERE fk_User_ID = '" + strUserID + "' AND fk_Location_ID  NOT IN (" + strListOfRegions + ")";
                DataOperations.ExecuteSQL(strCmd);
            }
            else
            {
                //DELETE All Regions as none of check box is selected.
                strCmd = "DELETE FROM Emerge.tblUserPortfolio WHERE fk_User_ID = '" + strUserID + "'";
                DataOperations.ExecuteSQL(strCmd);
            }
        }
        popAddEditID.Text = "";
        popFName.Text = "";
        popLName.Text = "";
        popEmail.Text = "";

        BindGrid();
    }
    protected void Cancel(object sender, EventArgs e)
    {
        popAddEditID.Text = "";
        popFName.Text = "";
        popLName.Text = "";
        popEmail.Text = "";
        litValidationError.Text = "";
    }
    
    // ######### VALIDATION #############
    private string Validate()
    {
        string strErrorMessage = "";
        if (popFName.Text.Trim() == "")
            strErrorMessage += "First Name is required <br/>";
        if (popLName.Text.Trim() == "")
            strErrorMessage += "Last Name is required <br/>";
        if (popEmail.Text.Trim() == "")
            strErrorMessage += "Email Name is required <br/>";
        
        string strUserID = popAddEditID.Text;
        if (strUserID != "0" && !popMarMgrRole.Checked)
        {
            if (AdminServices.IsUserAnApprover(strUserID))
                strErrorMessage += "This user is a brand manager, hence he must remain a marketing manager. <br/>You cannot change is role from marketing manager, until you remove him from the role of Brand Manager";
        }

        litValidationError.Text = strErrorMessage;
        return strErrorMessage;
    }


    private void BindGrid()
    {
        string strCmd = "";
        strCmd = @"SELECT U.UserId, U.UserName, AM.Email, ~AM.IsLockedOut AS Active, 
                    CAST(([Lumen].[fnIsUserIDInRole](U.UserId, 'Level1') | [Lumen].[fnIsUserIDInRole](U.UserId, 'Level2') | [Lumen].[fnIsUserIDInRole](U.UserId, 'Level3') | [Lumen].[fnIsUserIDInRole](U.UserId, 'SM')) AS BIT) AS SalesManager,  
                    CAST([Lumen].[fnIsUserIDInRole](U.UserId, 'MM') AS BIT) AS MarketingManager, CAST([Lumen].[fnIsUserIDInRole](U.UserId, 'Admin') AS BIT) AS [Admin]
                    , [Lumen].[fnGetRegionNamesForUser](U.UserId) AS RegionNames
                    FROM aspnet_Membership AM 
                    LEFT JOIN aspnet_Users U ON ( U.UserId = AM.UserId) ";
        DataTable dtUsers = DataOperations.GetDataTable(strCmd);
        gridUsers.DataSource = dtUsers;
        gridUsers.DataBind();        
    }

    protected void SearchUsers(object sender, EventArgs e)
    {
        string searchTxt = "";
        searchTxt = txtSearch.Text;
        string strCmd = "";

        string strCmdPattern = @"SELECT U.UserId, U.UserName, AM.Email, ~AM.IsLockedOut AS Active, 
                    CAST(([Lumen].[fnIsUserIDInRole](U.UserId, 'Level1') | [Lumen].[fnIsUserIDInRole](U.UserId, 'Level2') | [Lumen].[fnIsUserIDInRole](U.UserId, 'Level3') | [Lumen].[fnIsUserIDInRole](U.UserId, 'SM')) AS BIT) AS SalesManager,  
                    CAST([Lumen].[fnIsUserIDInRole](U.UserId, 'MM') AS BIT) AS MarketingManager, CAST([Lumen].[fnIsUserIDInRole](U.UserId, 'Admin') AS BIT) AS [Admin]
                    , [Lumen].[fnGetRegionNamesForUser](U.UserId) AS RegionNames
                    FROM aspnet_Membership AM 
                    LEFT JOIN aspnet_Users U ON ( U.UserId = AM.UserId) 
                    WHERE U.UserName like '%{0}%' OR AM.Email like '%{1}%'";
        strCmd = string.Format(strCmdPattern, searchTxt, searchTxt);
        DataTable dtUsers = DataOperations.GetDataTable(strCmd);
        gridUsers.DataSource = dtUsers;
        gridUsers.DataBind();

    }

    protected void lvwSelectedRegion_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        CheckBox chkRegion = (CheckBox)e.Item.FindControl("chkRegion");
        chkRegion.Text = DataBinder.Eval(e.Item.DataItem, "Location").ToString();
        chkRegion.Attributes.Add("pk_location_ID", DataBinder.Eval(e.Item.DataItem, "pk_location_ID").ToString());
    }

}