﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ACE;
using Lumen;
using System.Data;
using System.Web.Security;

public partial class Order_AddressBook : System.Web.UI.Page
{
    string strOrderID = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadData();
            LoadStateDropdown();
            LoadRegionDropdown();

            if (Request.QueryString["mode"] == null && Request.QueryString["ProgramID"] == null && Request.QueryString["OrderID"] == null)
            {                
                gridAddressBook.Columns[0].Visible = false;
                divProceedToOrder.Visible = false;
            }
            if (Roles.IsUserInRole("Admin") || Roles.IsUserInRole("DVP") || Roles.IsUserInRole("RDM"))
            {
                divAddBtn.Visible = true;
            }
        }
    }

    private void LoadData()
    {
        gridAddressBook.DataSource = AddressBookServices.GetAddressBook((Request.QueryString["ProgramID"] != null), null);

        gridAddressBook.DataBind();

        if ( ! ((Roles.IsUserInRole("Admin") || Roles.IsUserInRole("MM") || Roles.IsUserInRole("UXL"))))
        {
            gridAddressBook.Columns[10].Visible = false;// Edit
            gridAddressBook.Columns[11].Visible = false;// Delete
            gridAddressBook.Columns[1].Visible = false;// Address Owner
        }
    }

    private void LoadStateDropdown()
    {
        popDdlState.AppendDataBoundItems = true;
        popDdlState.DataSource = DataOperations.GetDataTable("SELECT * FROM Emerge.tblLocation WHERE LocationType = 'State'");
        popDdlState.DataTextField = "Location";
        popDdlState.DataBind();
    }
    private void LoadRegionDropdown()
    {
        string strUserID = Membership.GetUser().ProviderUserKey.ToString();
        popDdlRegion.AppendDataBoundItems = true;
        popDdlRegion.DataSource = DataOperations.GetDataTable("SELECT * FROM Emerge.tblLocation WHERE LocationType = 'Region' AND pk_location_ID IN ( SELECT fk_Location_ID FROM Emerge.tblUserPortfolio WHERE fk_User_ID = '" + strUserID + "')");
        popDdlRegion.DataTextField = "Location";
        popDdlRegion.DataValueField = "pk_location_ID";
        popDdlRegion.DataBind();
    }
    protected void Search(object sender, EventArgs e)
    {
        string strSearchTerm = txtSearch.Text;
        DataTable dtFilterAddressBook = AddressBookServices.GetAddressBook((Request.QueryString["ProgramID"] != null), strSearchTerm);
        gridAddressBook.DataSource = dtFilterAddressBook;
        gridAddressBook.DataBind();
    }
    

    protected void Proceed(object sender, EventArgs e)
    {
        string strCommaSeparatedAddressIDs = "";
        string strCommaSeparatedShippingInstructionIDs = "";
        foreach (GridViewRow row in gridAddressBook.Rows)
        {
            if (((CheckBox)row.FindControl("chkSelect")).Checked)
            {
                strCommaSeparatedAddressIDs += ((HiddenField)row.FindControl("hdnAddressId")).Value + ",";
                strCommaSeparatedShippingInstructionIDs += ((DropDownList)row.FindControl("ddlShipInstr")).SelectedValue + ",";
            }            
        }
        strCommaSeparatedAddressIDs = strCommaSeparatedAddressIDs.TrimEnd(',');
        strCommaSeparatedShippingInstructionIDs = strCommaSeparatedShippingInstructionIDs.TrimEnd(',');

        if (strCommaSeparatedAddressIDs == "")
        {
            // JQuery based message
        }
        else
        {
            Session["strCommaSeparatedAddressIDs"] = strCommaSeparatedAddressIDs;
            Session["strCommaSeparatedShippingInstructionIDs"] = strCommaSeparatedShippingInstructionIDs;
            if (Request.QueryString["mode"] != null)
            {
                if (Request.QueryString["mode"].ToUpper() == "INVENTORY")
                    Response.Redirect("InventoryOrder.aspx?OrderID=" + Request.QueryString["OrderID"], false);
                }
            else
                Response.Redirect("SeasonalOrder.aspx?ProgramID=" + Request.QueryString["ProgramID"] + "&OrderID=" + Request.QueryString["OrderID"], false);
      
        }
    }

    /* ########################  GRID EVENT HANDLERS  ################# */
    protected void gridAddressBook_RowDataBound(object sender, GridViewRowEventArgs e)  
    {        
        string strOrderID = Convert.ToString(Request.QueryString["OrderID"]);
        if (e.Row.RowType != DataControlRowType.DataRow) 
            return;        

        Literal litLocationName = e.Row.FindControl("litLocationName") as Literal;
        Literal litShipToName = e.Row.FindControl("litShipToName") as Literal;
        Literal litShipToCompany = e.Row.FindControl("litShipToCompany") as Literal;
        Literal litState = e.Row.FindControl("litState") as Literal;
        Label lblAddress = e.Row.FindControl("lblAddress") as Label;
        Literal litCity = e.Row.FindControl("litCity") as Literal;
        Literal litCostCenter = e.Row.FindControl("litCostCenter") as Literal;

        Literal litUserName = e.Row.FindControl("litUserName") as Literal;
        litUserName.Text = DataBinder.Eval(e.Row.DataItem, "UserName").ToString(); 

        HiddenField hdnAddressId = e.Row.FindControl("hdnAddressId") as HiddenField;
        HiddenField hdnShippingInstructionId = e.Row.FindControl("hdnShippingInstructionId") as HiddenField;
        CheckBox chkSelect = e.Row.FindControl("chkSelect") as CheckBox;
        ImageButton imgBtnEdit = e.Row.FindControl("imgBtnEdit") as ImageButton ;
        ImageButton imgBtnDelete = e.Row.FindControl("imgBtnDelete") as ImageButton;
        DropDownList ddlShipInstr = (DropDownList)e.Row.FindControl("ddlShipInstr");

        litLocationName.Text = DataBinder.Eval(e.Row.DataItem, "LocationName").ToString();
        litShipToName.Text = DataBinder.Eval(e.Row.DataItem, "ShipToName").ToString();
        litShipToCompany.Text = DataBinder.Eval(e.Row.DataItem, "ShipToCompany").ToString();
        litState.Text = DataBinder.Eval(e.Row.DataItem, "State").ToString();                
        lblAddress.Text = DataBinder.Eval(e.Row.DataItem, "Address").ToString();
        litCity.Text = DataBinder.Eval(e.Row.DataItem, "City").ToString();
        litCostCenter.Text = DataBinder.Eval(e.Row.DataItem, "Region").ToString();

        hdnAddressId.Value = DataBinder.Eval(e.Row.DataItem, "pk_address_id").ToString();
        hdnShippingInstructionId.Value = ddlShipInstr.SelectedValue.ToString();


      

        if (strOrderID != null)
        {

            if (AddressBookServices.IsAddressUsedInOrder(strOrderID, hdnAddressId.Value))
            {
                e.Row.CssClass = "disabled";
                chkSelect.Checked = true;
                chkSelect.Enabled = false;
                imgBtnDelete.Enabled = false;
                imgBtnEdit.Enabled = false;
            }
        }

        DataTable dt;
        dt = DataOperations.GetDataTable("SELECT pk_shipping_instruction_id, shipping_instruction FROM Emerge.tblShipping_Instruction");
        if (Request.QueryString["mode"] != null)
        {
            if (Request.QueryString["mode"].ToUpper() == "INVENTORY")
            {
                ddlShipInstr.DataSource = dt.DataSet;
                ddlShipInstr.DataTextField = "shipping_instruction";
                ddlShipInstr.DataValueField = "pk_shipping_instruction_Id";
                ddlShipInstr.DataBind();
            }
            else
            {
                // Hide Shipping Instruction Column for Seasonal Orders and Admin Address Book Flow
                gridAddressBook.Columns[9].Visible = false; 

            }
        }
        else
        {
            // Seasonal
            // Hide Shipping Instruction Column for Seasonal Orders and Admin Address Book Flow
            gridAddressBook.Columns[9].Visible = false;

        }
        

    }

    protected void gridAddressBook_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //DECLARATION needed
    }
    protected void gridAddressBook_RowCommand(object sender, GridViewRowEventArgs e)
    {        
    }    
    protected void gridAddressBook_Sorting(object sender, GridViewSortEventArgs e)
    {
    }
    protected void SelectDeselectAll(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gridAddressBook.Rows)
        {
            CheckBox chkSelect = row.FindControl("chkSelect") as CheckBox;
            if(chkSelect.Enabled)
                chkSelect.Checked = ((CheckBox)sender).Checked;
        }
    }
    /* ################## END OF GRID EVENT HANDLERS ################## */


    // JQuery Popup
    protected void Add(object sender, EventArgs e)
    {
        popAddEditID.Text = "0";
        litValidationError.Text = "";

        popTxtShipToName.Text = "";
        popTxtShipToCompany.Text = "";
        popTxtAddress1.Text = "";
        popTxtAddress2.Text = "";
        popTxtCity.Text = "";
        popDdlState.Text = "Select";
        popTxtZIP.Text = "";
        popTxtPhone.Text = "";
        popTxtEmail.Text = "";
        popTxtDestinationCode.Text = "(Auto Filled)";
        PopChkActive.Checked = true;
    }


    protected void Delete(object sender, EventArgs e)
    {
        GridViewRow row = ((ImageButton)sender).Parent.Parent as GridViewRow;        

        string strCmd = "DELETE Emerge.tblAddress WHERE pk_Address_Id = " + ((HiddenField)row.FindControl("hdnAddressId")).Value;
        string strResult = "";
        DataOperations.ExecuteSQL(strCmd, out strResult);
        if (strResult == "INUSE")
            failuremsg.Text = "This address cannot be deleted as it is in use.";
        else
        {
            successmsg.Text = "Address deleted.";            
        }
        LoadData();
        litValidationError.Text = "";
    }


    protected void Edit(object sender, EventArgs e)
    {
        GridViewRow row = ((ImageButton)sender).Parent.Parent as GridViewRow;
        popAddEditID.Text = ((HiddenField)row.FindControl("hdnAddressId")).Value;

        DataRow drAddress = DataOperations.GetSingleRow("SELECT * FROM Emerge.tblAddress WHERE pk_Address_ID = " + popAddEditID.Text);
        
        popTxtShipToName.Text = Convert.ToString( drAddress["shipto_name"] );
        popTxtShipToCompany.Text = Convert.ToString( drAddress["shipto_company"] );
        popTxtAddress1.Text = Convert.ToString( drAddress["address1"] );
        popTxtAddress2.Text = Convert.ToString( drAddress["address2"] );
        popTxtCity.Text = Convert.ToString( drAddress["city"] );
        popDdlState.Text = Convert.ToString( drAddress["state"] );
        popTxtZIP.Text = Convert.ToString( drAddress["zip"] );
        popTxtPhone.Text = Convert.ToString( drAddress["phone"] );
        popTxtEmail.Text = Convert.ToString( drAddress["email"] );
        popTxtDestinationCode.Text = Convert.ToString( drAddress["destination_name"] );
        PopChkActive.Checked= Convert.ToBoolean( drAddress["active"] );
        popDdlRegion.Text = Convert.ToString(drAddress["fk_location_ID"]);
        
        
        litValidationError.Text = "";
    }

    protected void Cancel(object sender, EventArgs e)
    {
        popAddEditID.Text = "";        
        litValidationError.Text = "";
    }

    protected void Save(object sender, EventArgs e)
    {
        litValidationError.Text = ValidateForm();
        if (litValidationError.Text != "")
            return;
        string strActive = Convert.ToInt16(PopChkActive.Checked).ToString();

        string strLocation = "0"; // Not Implemented in Rodney

        AddressBookServices.SaveAddress(popAddEditID.Text, popTxtShipToName.Text, popTxtShipToCompany.Text, popTxtAddress1.Text, popTxtAddress2.Text,
                                        popTxtCity.Text, popTxtDestinationCode.Text, popDdlState.SelectedItem.Text, popTxtZIP.Text, popTxtPhone.Text, popTxtEmail.Text, strActive,strLocation);
      
        popAddEditID.Text = ""; //To close the popup
        LoadData();
    }

    private string ValidateForm()
    {
        string strValidation = "";

        if (String.IsNullOrEmpty(popTxtShipToName.Text))
            strValidation += "Ship to Name is required <br/> ";

        if (String.IsNullOrEmpty(popTxtShipToCompany.Text))
            strValidation += "Ship to Company is required <br/> ";

        if (String.IsNullOrEmpty(popTxtAddress1.Text))
            strValidation += "Address 1 is required <br/> ";

        if (String.IsNullOrEmpty(popTxtCity.Text))
            strValidation += "City is required <br/> ";

        if (String.IsNullOrEmpty(popTxtPhone.Text))
            strValidation += "Phone is required <br/> ";

        if (popDdlState.SelectedValue.ToUpper() == "SELECT")
            strValidation += "Please select State<br/>";

        if (String.IsNullOrEmpty(popTxtZIP.Text))
            strValidation += "Zip is required <br/> ";

        /*
         * if (popDdlRegion.SelectedValue.ToUpper() == "")
            strValidation += "Please select the region this address belongs to<br/>";
         * */

        return strValidation;
    }
}   