using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using ACE;

public partial class Admin_Brands : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();
            BindDDLs();
        }
    }        

    protected void gridBrands_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        string strID =  DataBinder.Eval(e.Row.DataItem, "pk_Brand_Id").ToString();
        ((TextBox)e.Row.FindControl("txtID")).Text = strID;
    }

    // **** [buttons] ************************* 
    protected void Add(object sender, EventArgs e)
    {
        popAddEditID.Text = "0";
    }

    // **** [Row buttons] *************************
    protected void Edit(object sender, EventArgs e)
    {
        ImageButton EditButton = sender as ImageButton;
        GridViewRow row = EditButton.Parent.Parent as GridViewRow;
        popAddEditID.Text = ((TextBox)row.FindControl("txtID")).Text;
        popBrandname.Text = row.Cells[0].Text;

        string strBrandManager = row.Cells[1].Text;
        
        ListItem CurrentBrandManager = popMMUsers.Items.FindByText(strBrandManager);
        string BMUserID = "";
        if (CurrentBrandManager != null)
        {
            BMUserID = popMMUsers.Items.FindByText(strBrandManager).Value;
            popMMUsers.Text = BMUserID;
        }

    }
    protected void Delete(object sender, EventArgs e)
    {
        ImageButton Button = sender as ImageButton;
        GridViewRow row = Button.Parent.Parent as GridViewRow;
        string strID = ((TextBox)row.FindControl("txtID")).Text;
        string strCmd = "DELETE Emerge.tblBrand WHERE pk_Brand_ID = " + strID;
        string strResult = "";
        DataOperations.ExecuteSQL(strCmd,out strResult);
        if (strResult == "INUSE")
            failuremsg.Text = "This brand cannot be deleted as it is in use.";
        else
        {
            successmsg.Text = "Brand deleted.";
            BindGrid();
        }
    }

    // **** [Popup buttons] ************************* 
    protected void Save(object sender, EventArgs e)
    {
        if (Validate() != "")
            return;

        string strID = popAddEditID.Text;
                              
        string strCmd="";
        if (strID == "0")
        {
            strCmd = @"INSERT INTO Emerge.tblBrand (Brand, last_updated_on, last_updated_by) 
                VALUES ('" + popBrandname.Text.Replace("'", "''") + "', GETDATE(),'" + Membership.GetUser().ProviderUserKey.ToString() + "')";
        }
        else
        {
            strCmd = "UPDATE Emerge.tblBrand SET Brand = '" + popBrandname.Text.Replace("'", "''") + "', ";
            strCmd += "last_updated_on = GETDATE(), last_updated_by='" + Membership.GetUser().ProviderUserKey.ToString() + "' WHERE pk_Brand_ID = " + strID;
        }
        DataOperations.ExecuteSQL(strCmd);

        string BMUserID = popMMUsers.SelectedItem.Value;        
        strCmd = "EXEC Emerge.SP_SetBrandManager @USERID = '" + BMUserID + "', @BrandName = '" + popBrandname.Text.Replace("'", "''") + "'";
        try
        {
            DataOperations.ExecuteSQL(strCmd);
        }
        catch (Exception ex) { }

        //Response.Write(strCmd);
        popAddEditID.Text = "";
        popBrandname.Text = "";
        BindGrid();
    }
    protected void Cancel(object sender, EventArgs e)
    {
        popAddEditID.Text = "";
        popBrandname.Text = "";
        litValidationError.Text = "";
    }
    
    // ######### VALIDATION #############
    private string Validate()
    {
        string strErrorMessage = "";
        if (popBrandname.Text.Trim() == "")
            strErrorMessage += "Brand Name is required <br/>";

        litValidationError.Text = strErrorMessage;
        return strErrorMessage;
    }

    private void BindDDLs()
    {
        string strSQL_MMUsers = @"SELECT U.UserId, U.UserName FROM aspnet_Users U 
                        JOIN aspnet_UsersInRoles UIR ON UIR.UserId = U.UserId 
                        JOIN aspnet_Roles R on R.RoleId = UIR.RoleId
                        WHERE R.RoleName = 'MM'";
        DataTable dtMMUsers = DataOperations.GetDataTable(strSQL_MMUsers);
        popMMUsers.DataTextField = "UserName";
        popMMUsers.DataValueField = "UserID";
        popMMUsers.DataSource = dtMMUsers;
        popMMUsers.DataBind();        
    }

    private void BindGrid()
    {
        DataTable dtBrands = DataOperations.GetDataTable(@"SELECT B.*, ISNULL(U.UserName,'') BrandManager, U.UserId FROM Emerge.tblBrand B
                                LEFT JOIN Emerge.tblApprover BM on BM.fk_Brand_id = B.pk_Brand_id
                                LEFT JOIN aspnet_Users U on U.UserId = BM.fk_Approver");
        gridBrands.DataSource = dtBrands;
        gridBrands.DataBind();                
    }
}