using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lumen;
using ACE;

public partial class Order_InventoryOrder : System.Web.UI.Page
{
    string strOrderID, strCommaSeparatedAddressIDs, strCommaSeparatedShippingInstructionIDs;
    string strReadonly;
    string strSearchTxt;
    string strListOfBrandIds;
    protected void Page_Load(object sender, EventArgs e)
    {
        strReadonly = Convert.ToString(Request.QueryString["readonly"]);
        if (strReadonly == "true")
        {            
            btnSave.Visible = false;
            btnConfirm.Visible = false;
            btnBack.Visible = true;
        }
        else
            strReadonly = "false";
        
        strOrderID = Convert.ToString(Request.QueryString["OrderID"]);
        strCommaSeparatedAddressIDs = Convert.ToString(HttpContext.Current.Session["strCommaSeparatedAddressIDs"]);
        strCommaSeparatedShippingInstructionIDs = Convert.ToString(HttpContext.Current.Session["strCommaSeparatedShippingInstructionIDs"]);


        if (!String.IsNullOrEmpty(strOrderID))
        {
            hdnOrderID.Value = strOrderID;
            strCommaSeparatedAddressIDs = OrderServices.GetAddressesInOrder(strOrderID);
        }

        if (String.IsNullOrEmpty(strCommaSeparatedAddressIDs)) 
        {            
            Response.Redirect("AddressBook.aspx?mode=Inventory", false);
        }

        if (!IsPostBack)
        {
            //DataSet lstSeletedBrands;
            DataTable lstSeletedBrands;
            lstSeletedBrands = DataOperations.GetDataTable("SELECT  DISTINCT  B.Brand AS BrandName,B.pk_Brand_Id AS BrandId  FROM Emerge.tblItem item LEFT JOIN Emerge.tblBrand B ON B.pk_Brand_Id = item.fk_BrandId WHERE item.InvOrdItem = 1 ORDER BY B.Brand");
            lvwSelectedBrand.DataSource = lstSeletedBrands.DataSet;
            lvwSelectedBrand.DataBind();

            DataSet Items = null;
            strSearchTxt = "";
            if (Session["SearchText"] != null)
            {
                strSearchTxt = Session["SearchText"].ToString();
                txtSearch.Text = strSearchTxt;
            }
            strListOfBrandIds = "";
            if (Session["SelectedBrandIds"] != null)
            {
                strListOfBrandIds = Session["SelectedBrandIds"].ToString();
            }

            if (strOrderID == null)
                strOrderID = "0";
            if ((strSearchTxt != "") || (strListOfBrandIds != "") || (strOrderID != ""))
                Items = OrderServices.GetInventoryOrderItems(strOrderID, strSearchTxt, strListOfBrandIds).DataSet;
            if (Items != null) 
                LoadData(Items);
            
            Session["SearchText"] = "";
            Session["SelectedBrandIds"] = "";
        }
    }

    protected override void OnLoadComplete(EventArgs e)
    {
        base.OnLoadComplete(e);
        LoadBudgetDetails();
    }

    private void LoadBudgetDetails()
    {
        string strOrdTotal = "0";
        if (String.IsNullOrEmpty(strOrderID)) strOrderID = "0";
        string strCmd = "SELECT ISNULL(SUM(quantity*EstimatedPrice),0) AS  OrderTotal FROM Emerge.tblOrderDestinationItem ODI ";
        strCmd += "LEFT JOIN Emerge.tblOrderDestination OD ON ( OD.pk_OrderDestination_ID = ODI.fk_OrderDestination_ID) ";
        strCmd += "LEFT JOIN Emerge.tblItem I ON (I.pk_Item_ID = ODI.fk_Item_ID) WHERE fk_Order_ID =  " + strOrderID;
        DataRow drOrdertotal = DataOperations.GetSingleRow(strCmd);
        strOrdTotal = (drOrdertotal[0]).ToString();

        Decimal DecOrdTotal = 0;
        Decimal.TryParse(strOrdTotal, out DecOrdTotal);
        litOrderTotal.Text = String.Format("{0:C}", DecOrdTotal);
        hdnOrderValue.Value = strOrdTotal.ToString();
    }
    private void LoadData(DataSet Items)
    {
        //lvwInventoryOrderItems.DataSource = OrderServices.GetInventoryOrderItems(strOrderID);
        //lvwInventoryOrderItems.DataSource = OrderServices.GetInventoryOrderItems(strOrderID, strSearchTxt, strListOfBrandIds);

        //lvwInventoryOrderItems.DataBind();


        lvwInventoryOrderItems.DataSource = null;
        lvwInventoryOrderItems.DataSource = Items;
        lvwInventoryOrderItems.DataBind();
    }

    protected void lvwInventoryOrderItems_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        HiddenField hdnItemID = e.Item.FindControl("hdnItemID") as HiddenField;
        Literal litItemName = e.Item.FindControl("litItemName") as Literal;
        Literal litItemNo = e.Item.FindControl("litItemNo") as Literal;
        Literal litPackSize = e.Item.FindControl("litPackSize") as Literal;
        Literal litBrand = e.Item.FindControl("litBrand") as Literal;
        Literal litItemType = e.Item.FindControl("litItemType") as Literal;
        Literal litPrice = e.Item.FindControl("litPrice") as Literal;
        Literal litDimension = e.Item.FindControl("litDimension") as Literal;
        Literal litDescription = e.Item.FindControl("litDescription") as Literal;
        HyperLink lnkProgramPreviewImage = e.Item.FindControl("lnkProgramPreviewImage") as HyperLink;
        UserControls_CompositeCart compositeCart = e.Item.FindControl("compositeCart") as UserControls_CompositeCart;
        Literal litAvailableInventory = e.Item.FindControl("litAvailableInventory") as Literal;
        Literal litMaxOrdQty = e.Item.FindControl("litMaxOrdQty") as Literal;

        

        hdnItemID.Value = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "pk_Item_ID"));

        litItemName.Text = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "Item_Name"));
        litItemNo.Text = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "Item_No"));        
        litBrand.Text = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "Brand"));
        litDimension.Text = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "Dimension"));
        litItemType.Text = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "Type"));
        litPrice.Text = "$" + Convert.ToString(DataBinder.Eval(e.Item.DataItem, "EstimatedPrice"));
        
        litDescription.Text = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "Description"));
        litAvailableInventory.Text = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "AvblStock"));
        litMaxOrdQty.Text = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "MaxOrderQty"));
        if (litMaxOrdQty.Text == "0")
            litMaxOrdQty.Text = "No Limit";
        lnkProgramPreviewImage.ImageUrl = "~/ImageLibrary/Thumbnails/" + Convert.ToString(DataBinder.Eval(e.Item.DataItem, "PreviewImageFileName"));
        lnkProgramPreviewImage.NavigateUrl = "~/ImageLibrary/Large/" + Convert.ToString(DataBinder.Eval(e.Item.DataItem, "PreviewImageFileName"));

        if (strReadonly != "true") strReadonly = "false";
        compositeCart.ReadOnly = Convert.ToBoolean(strReadonly);
        compositeCart.propItemID = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "pk_Item_ID"));
        compositeCart.propItemPrice = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "EstimatedPrice"));
        compositeCart.propCommaSeparatedAddressIDs = strCommaSeparatedAddressIDs;

        string strUOM = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "UnitOfMeasurement"));
        string strUnitQuantity = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "UnitQuantity"));
        string strPackingInfo = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "Packing"));
        if(String.IsNullOrEmpty(strPackingInfo))
        {
            if (String.IsNullOrEmpty(strUOM))
                if(String.IsNullOrEmpty(strUnitQuantity))                
                    strPackingInfo = "Eaches";
                else
                    strPackingInfo = "Pack of " + strUnitQuantity;
            else
                strPackingInfo = "Pack of " + strUnitQuantity + " " + strUOM;
        }
        litPackSize.Text = strPackingInfo;
        compositeCart.propCartIndex = e.Item.DataItemIndex;

        string strApprovalReq = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "approval_required"));

        if (strApprovalReq.ToUpper() == "TRUE")
            strApprovalReq = "1";
        else
            strApprovalReq = "0";

        if (strApprovalReq != null && Convert.ToDouble(strApprovalReq) == 1)
        {
            ((Label)e.Item.FindControl("ApprovalReqMsg")).Visible = true;
            compositeCart.propApprovalReq = 1;
        }
        compositeCart.propMaxOrderQty = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "MaxOrderQty").ToString());
        compositeCart.propAvailableStock = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "AvblStock").ToString());
    }

    protected void Back(object sender, EventArgs e)
    {
        Response.Redirect("MyInventoryOrders.aspx", false);
    }

    protected void Confirm(object sender, EventArgs e)
    {
        Save(null, null); //First invoke Save button, which ensure that hdnOrderID.Value has the right value
        if (OrderServices.ConfirmOrder(hdnOrderID.Value))
        {
           // MailServices.SendOrderConfirmationEmailToSM(hdnOrderID.Value);
           // MailServices.SendNewOrderPlacedEmailToBMs(hdnOrderID.Value);
            Response.Redirect("MyInventoryOrders.aspx", false);
        }
    }
    protected void Save(object sender, EventArgs e)
    {        
        if (hdnOrderID.Value == "")
            hdnOrderID.Value = OrderServices.CreateEmptyInvOrder(strCommaSeparatedAddressIDs, strCommaSeparatedShippingInstructionIDs);
        foreach (ListViewItem item in lvwInventoryOrderItems.Items)
        {
            HiddenField hdnItemID = item.FindControl("hdnItemID") as HiddenField;
            string strItemID = hdnItemID.Value;
            UserControls_CompositeCart compositeCart = item.FindControl("compositeCart") as UserControls_CompositeCart;
            ListView lvwCompositeCart = compositeCart.FindControl("lvwCompositeCart") as ListView;

            Boolean hasError = false;

            foreach(ListViewItem cartItems in lvwCompositeCart.Items)
            {
                HiddenField hdnAddressId = cartItems.FindControl("hdnAddressId") as HiddenField;
                TextBox txtQuantity = cartItems.FindControl("txtQuantity") as TextBox;
                string strAddressID = hdnAddressId.Value;
                string strQuantity = txtQuantity.Text;

                // Get Current Available Stock and Validate before saving
                string strAvblQty = "0";
                string strOrderID = "0";
                if (String.IsNullOrEmpty(hdnOrderID.Value))
                    strOrderID = "0";
                else
                    strOrderID = hdnOrderID.Value;
                //string strCmd = "SELECT ISNULL([Emerge].[fn_GetAvailableStockByItemID] (" + strItemID + ", " + strOrderID + "), 0) AS AvblQty ";
                string strCmd = "SELECT CASE WHEN (ISNULL(Stock,0)-ISNULL(inProcess_inventory,0)) < 0 THEN 0 ELSE (ISNULL(Stock,0)-ISNULL(inProcess_inventory,0)) END AS AvblQty  FROM Emerge.tblItem WHERE pk_Item_ID = " + strItemID;

                DataRow drAvblQty = DataOperations.GetSingleRow(strCmd);
                strAvblQty = (drAvblQty[0]).ToString();
                Label lblError = compositeCart.FindControl("lblError") as Label;
                if (Convert.ToDouble(strAvblQty) >= Convert.ToDouble(strQuantity)) 
                {
                    OrderServices.SaveOrderQuantity(strItemID, strAddressID, hdnOrderID.Value, strQuantity);
                    if (hasError == false)
                        lblError.Text = "";
                }
                else
                {
                    hasError = true;
                    lblError.Text = "Ordered Qty exceeds Available Stock";
                }
            }
        }
        if (sender != null)
            Response.Redirect("InventoryOrder.aspx?OrderID=" + (hdnOrderID.Value).ToString());
    }

    ///////////////////////////////////////////////////////////////////////////////

    protected void lvwSelectedBrand_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        CheckBox chkRMName = (CheckBox)e.Item.FindControl("chkBrandName");

        chkRMName.Text = DataBinder.Eval(e.Item.DataItem, "BrandName").ToString();
        chkRMName.Attributes.Add("BrandId", DataBinder.Eval(e.Item.DataItem, "BrandId").ToString());

        if (Session["SelectedBrandIds"] == "")
        {
            chkRMName.Checked = false;
        }

        else
        {
            string strListOfBrandIds = Convert.ToString(Session["SelectedBrandIds"]);
            if (strListOfBrandIds != "")
            {
                char[] delimiters = new char[] { ',' };
                string[] arrListofBrandIds = strListOfBrandIds.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                for (int nCount = 0; nCount < arrListofBrandIds.Length; nCount++)
                {
                    if (arrListofBrandIds[nCount].ToString() == DataBinder.Eval(e.Item.DataItem, "BrandId").ToString())
                        chkRMName.Checked = true;
                }
            }
        }
    }
    protected void SearchItems(object sender, EventArgs e)
    {
        List<string> ListOfBrandIds = new List<string>();
        foreach (ListViewDataItem lvi in lvwSelectedBrand.Items)
        {
            CheckBox selectedCheBox = (CheckBox)lvi.FindControl("chkBrandName");
            if (selectedCheBox.Checked)
            {
                string brandIds = selectedCheBox.Attributes["BrandId"];

                ListOfBrandIds.Add(brandIds);
            }
        }
        string strListOfBrandIds = string.Join(",", ListOfBrandIds);
        Session["SelectedBrandIds"] = strListOfBrandIds;
        Session["SearchText"] = txtSearch.Text;
        //Response.Redirect("InventoryOrder.aspx");
        Response.Redirect("InventoryOrder.aspx?"+Request.QueryString);

    }

    protected void btnSelectedBrand_Click(object sender, EventArgs e)
    {
        //DataSet FilteredItems;
        List<string> ListOfBrandIds = new List<string>();
        foreach (ListViewDataItem lvi in lvwSelectedBrand.Items)
        {
            CheckBox selectedCheBox = (CheckBox)lvi.FindControl("chkBrandName");
            if (selectedCheBox.Checked)
            {
                string brandIds = selectedCheBox.Attributes["BrandId"];

                ListOfBrandIds.Add(brandIds);
            }
        }
        string strListOfBrandIds = string.Join(",", ListOfBrandIds);
        Session["SelectedBrandIds"] = strListOfBrandIds;
        Session["SearchText"] = "";
        Response.Redirect("InventoryOrder.aspx?" + Request.QueryString);
    }
}