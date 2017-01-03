using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using ACE;
using System.Web.Security;
using Lumen;

public partial class Admin_OrderApproval : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateInventoryOrderItemsForApproval();
        }
    }

    private void PopulateInventoryOrderItemsForApproval()
    {
        DataSet ds_PopulateInventoryOrderItemsForApproval = OrderServices.GetInventoryOrderItemsForApprovalByUserID().DataSet;
        if (ds_PopulateInventoryOrderItemsForApproval.Tables[0].Rows.Count == 0)
        {
            lblNoOrders.Visible = true;
            SaveButton.Visible = false;
            CancelButton.Visible = false;
            chkSelectAllOrderItems.Visible = false;
            btnApproveSeletedOrdersAll.Visible = false;
            lblNoOrders.Text = "There are no orders for approval.";
        }
        else
        {
            lblNoOrders.Visible = false;
            SaveButton.Visible = true;
            CancelButton.Visible = true;
            chkSelectAllOrderItems.Visible = true;
            btnApproveSeletedOrdersAll.Visible = true;
        }
        grdInventoryOrderItemApproval.DataSource = ds_PopulateInventoryOrderItemsForApproval;
        grdInventoryOrderItemApproval.DataBind();
    }

    protected void grdInventoryOrderItemApproval_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblOrderDate = e.Row.FindControl("lblOrderDate") as Label;
            Label lblPendingHours = e.Row.FindControl("lblPendingHours") as Label;
            Label lblOrderNo = e.Row.FindControl("lblOrderNo") as Label;
            Label lblSalMgrName = e.Row.FindControl("lblSalMgrName") as Label;
            Label lblBrandName = e.Row.FindControl("lblBrandName") as Label;
            Label lblItemName = e.Row.FindControl("lblItemName") as Label;
            Label lblDistributor = e.Row.FindControl("lblDistributor") as Label;
            TextBox txtQty = e.Row.FindControl("txtQty") as TextBox;
            Label lblAvailableInventory = e.Row.FindControl("lblAvailableInventory") as Label;
            DropDownList ddlApproved = e.Row.FindControl("ddlApproved") as DropDownList;
            TextBox txtComments = e.Row.FindControl("txtComments") as TextBox;
            HiddenField hdnBrandID = e.Row.FindControl("hdnBrandID") as HiddenField;
            HiddenField hdnItemID = e.Row.FindControl("hdnItemID") as HiddenField;
            HiddenField hdnInventoryOrderDestinationItem = e.Row.FindControl("hdnInventoryOrderDestinationItem") as HiddenField;
            HiddenField hdnOrderNo = e.Row.FindControl("hdnOrderNo") as HiddenField;
            HiddenField hdnSalMgrID = e.Row.FindControl("hdnSalMgrID") as HiddenField;

            lblOrderDate.Text = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "OrderDate").ToString()).ToShortDateString();
            lblPendingHours.Text = DataBinder.Eval(e.Row.DataItem, "PendingHours").ToString();
            hdnOrderNo.Value = DataBinder.Eval(e.Row.DataItem, "InventoryOrderID").ToString();
            lblOrderNo.Text = DataBinder.Eval(e.Row.DataItem, "InventoryOrderID").ToString() + "-" + DataBinder.Eval(e.Row.DataItem, "OrderDestinationID").ToString();
            lblSalMgrName.Text = DataBinder.Eval(e.Row.DataItem, "SMName").ToString();
            hdnBrandID.Value = DataBinder.Eval(e.Row.DataItem, "BrandID").ToString();
            lblBrandName.Text = DataBinder.Eval(e.Row.DataItem, "BrandName").ToString();
            hdnItemID.Value = DataBinder.Eval(e.Row.DataItem, "ItemID").ToString();
            lblItemName.Text = DataBinder.Eval(e.Row.DataItem, "ItemName").ToString();
            lblDistributor.Text = DataBinder.Eval(e.Row.DataItem, "Distributor").ToString();
            txtQty.Text = DataBinder.Eval(e.Row.DataItem, "Qty").ToString();
            txtQty.Attributes.Add("previousQty", txtQty.Text);
            lblAvailableInventory.Text = DataBinder.Eval(e.Row.DataItem, "AvailableInvetory").ToString();
            ddlApproved.SelectedIndex = ddlApproved.Items.IndexOf(ddlApproved.Items.FindByValue(DataBinder.Eval(e.Row.DataItem, "Status").ToString()));
            txtComments.Text = DataBinder.Eval(e.Row.DataItem, "Comments").ToString();
            hdnInventoryOrderDestinationItem.Value = DataBinder.Eval(e.Row.DataItem, "InventoryOrderDestinationItem").ToString();
            hdnSalMgrID.Value = DataBinder.Eval(e.Row.DataItem, "UserID").ToString();
            txtQty.Attributes.Add("onchange", "ValidateQuantity('" + e.Row.RowIndex.ToString() + "')");
            ddlApproved.Attributes.Add("onchange", "ValidateQuantity('" + e.Row.RowIndex.ToString() + "')");

        }
    }


   
    protected void SaveInventoryOrderItemApproval(object sender, EventArgs e)
    {
       
        List<clsInventoryOrderItem> listInventoryOrderItem = new List<clsInventoryOrderItem>();
        clsInventoryOrderItem objInventoryOrderItem;

        //Update Inventory Avaliable
        List<clsInventoryItemAllocation> listInventoryItemAvailableInventory = new List<clsInventoryItemAllocation>();
        clsInventoryItemAllocation objInventoryItemAvailableInventory;

        foreach (GridViewRow row in grdInventoryOrderItemApproval.Rows)
        {
          
            objInventoryOrderItem = new clsInventoryOrderItem();
            HiddenField hdnOrderNo =row.FindControl("hdnOrderNo") as HiddenField;
            HiddenField hdnSalMgrID = row.FindControl("hdnSalMgrID") as HiddenField;
            HiddenField hdnInventoryOrderDestinationItem = row.FindControl("hdnInventoryOrderDestinationItem") as HiddenField;
            DropDownList ddlApproved = row.FindControl("ddlApproved") as DropDownList;
            TextBox txtQty = row.FindControl("txtQty") as TextBox;
            HiddenField hdnItemID = row.FindControl("hdnItemID") as HiddenField;
            TextBox txtComments = row.FindControl("txtComments") as TextBox;
            if (ddlApproved.SelectedValue.ToString() == "0")
                continue;
            objInventoryOrderItem.InventoryOrderID = Convert.ToInt32(hdnOrderNo.Value);
            objInventoryOrderItem.InventoryOrderDestinationItem = Convert.ToInt32(hdnInventoryOrderDestinationItem.Value);
            objInventoryOrderItem.ApprovalStatus = Convert.ToInt32(ddlApproved.SelectedValue.ToString());
            if (ddlApproved.SelectedValue.ToString() == "1")
            {
                objInventoryOrderItem.ApprovedQuantity = 0;
            }
            if (ddlApproved.SelectedValue.ToString() == "2")
            {
                objInventoryOrderItem.ApprovedQuantity = Convert.ToInt32(txtQty.Text);
            }
            objInventoryOrderItem.Approver = new Guid(Membership.GetUser().ProviderUserKey.ToString());
            objInventoryOrderItem.ProviderUserKey = new Guid(hdnSalMgrID.Value.ToString());
            objInventoryOrderItem.Comments = txtComments.Text;
            listInventoryOrderItem.Add(objInventoryOrderItem);

            int iPreviousQty = Convert.ToInt32(txtQty.Attributes["previousQty"].ToString());
            if (Convert.ToInt32(txtQty.Text) > 0 || iPreviousQty > 0)
            {
                objInventoryItemAvailableInventory = new clsInventoryItemAllocation();
                objInventoryItemAvailableInventory.ItemId = Convert.ToInt32(hdnItemID.Value);
                if (ddlApproved.SelectedValue.ToString() == "1")
                {
                    objInventoryItemAvailableInventory.Quantity = 0;
                    objInventoryItemAvailableInventory.PreviousQty = iPreviousQty;
                }
                else
                {
                    objInventoryItemAvailableInventory.Quantity = Convert.ToInt32(txtQty.Text);
                    objInventoryItemAvailableInventory.PreviousQty = iPreviousQty;
                }
                listInventoryItemAvailableInventory.Add(objInventoryItemAvailableInventory);
            }
            
        }
        OrderServices.updateInventoryOrderItemApproval(listInventoryOrderItem);
        OrderServices.UpdateAvailableInventory(listInventoryItemAvailableInventory);
        Response.Redirect("OrderApproval.aspx"); // In effect reloading the page
    }

    protected void Cancel_click(object sender, EventArgs e)
    {
        Response.Redirect("OrderApproval.aspx");
    }
      
    protected void SaveInventoryApproveSelectedOrdersAll(object sender, EventArgs e)
    {
       
        List<clsInventoryOrderItem> listInventoryOrderItem = new List<clsInventoryOrderItem>();
        clsInventoryOrderItem objInventoryOrderItem;

        //Update Inventory Avaliable
        List<clsInventoryItemAllocation> listInventoryItemAvailableInventory = new List<clsInventoryItemAllocation>();
        clsInventoryItemAllocation objInventoryItemAvailableInventory;

        foreach (GridViewRow row in grdInventoryOrderItemApproval.Rows)
        {

            objInventoryOrderItem = new clsInventoryOrderItem();
            CheckBox chkSelect = row.FindControl("chkSelect") as CheckBox;
            HiddenField hdnOrderNo = row.FindControl("hdnOrderNo") as HiddenField;
            HiddenField hdnSalMgrID = row.FindControl("hdnSalMgrID") as HiddenField;
            HiddenField hdnInventoryOrderDestinationItem = row.FindControl("hdnInventoryOrderDestinationItem") as HiddenField;
            DropDownList ddlApproved = row.FindControl("ddlApproved") as DropDownList;
            TextBox txtQty = row.FindControl("txtQty") as TextBox;
            HiddenField hdnItemID = row.FindControl("hdnItemID") as HiddenField;
            TextBox txtComments = row.FindControl("txtComments") as TextBox;
            if (chkSelect.Checked == false)
                continue;
            objInventoryOrderItem.InventoryOrderID = Convert.ToInt32(hdnOrderNo.Value);
            objInventoryOrderItem.InventoryOrderDestinationItem = Convert.ToInt32(hdnInventoryOrderDestinationItem.Value);
            objInventoryOrderItem.ApprovalStatus =  ddlApproved.SelectedValue.ToString()=="0"?2:Convert.ToInt32(ddlApproved.SelectedValue.ToString());
            objInventoryOrderItem.ApprovedQuantity = Convert.ToInt32(txtQty.Text);
            objInventoryOrderItem.Approver = new Guid(Membership.GetUser().ProviderUserKey.ToString());
            objInventoryOrderItem.ProviderUserKey = new Guid(hdnSalMgrID.Value.ToString());
            objInventoryOrderItem.Comments = txtComments.Text;
            listInventoryOrderItem.Add(objInventoryOrderItem);

            int iPreviousQty = Convert.ToInt32(txtQty.Attributes["previousQty"].ToString());
            if (Convert.ToInt32(txtQty.Text) > 0 || iPreviousQty > 0)
            {
                objInventoryItemAvailableInventory = new clsInventoryItemAllocation();
                objInventoryItemAvailableInventory.ItemId = Convert.ToInt32(hdnItemID.Value);
                if (ddlApproved.SelectedValue.ToString() == "1")
                {
                    objInventoryItemAvailableInventory.Quantity = 0;
                    objInventoryItemAvailableInventory.PreviousQty = iPreviousQty;
                }
                else
                {
                    objInventoryItemAvailableInventory.Quantity = Convert.ToInt32(txtQty.Text);
                    objInventoryItemAvailableInventory.PreviousQty = iPreviousQty;
                }
                listInventoryItemAvailableInventory.Add(objInventoryItemAvailableInventory);
            }
            MailServices.SendOrderApprovalStatusToSM(hdnOrderNo.Value);
        }
        OrderServices.updateInventoryOrderItemApproval(listInventoryOrderItem);
        OrderServices.UpdateAvailableInventory(listInventoryItemAvailableInventory);
        Response.Redirect("OrderApproval.aspx"); // In effect reloading the page
    }
}