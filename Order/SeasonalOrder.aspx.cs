using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lumen;
using ACE;

public partial class Order_SeasonalOrder : System.Web.UI.Page
{
    string strProgramID, strOrderID, strCommaSeparatedAddressIDs;
    string strReadonly;
    Decimal dBudgetAllocated;
    Decimal dBudgetRemaining;
    Decimal dThisOrderAmount;
    string strBrandIds = "";
    string searchText = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        strReadonly = Convert.ToString(Request.QueryString["readonly"]);
        if (strReadonly == "true")
        {            
            btnSave.Visible = false;
            btnConfirm.Visible = false;
            btnSaveUp.Visible = false;
            btnConfirmUp.Visible = false;
            btnBack.Visible = true;
        }
        else
            strReadonly = "false";
        strProgramID = Convert.ToString(Request.QueryString["ProgramID"]);
        strOrderID = Convert.ToString(Request.QueryString["OrderID"]);
        strCommaSeparatedAddressIDs = Convert.ToString(HttpContext.Current.Session["strCommaSeparatedAddressIDs"]);
        
        if(String.IsNullOrEmpty(strProgramID) && String.IsNullOrEmpty(strOrderID))
            Response.Redirect("OpenPrograms.aspx",true);

        if (!String.IsNullOrEmpty(strOrderID))
            hdnOrderID.Value = strOrderID;

        if (String.IsNullOrEmpty(strCommaSeparatedAddressIDs) && !String.IsNullOrEmpty(strProgramID)) 
        {            
            Response.Redirect("AddressBook.aspx?ProgramID=" + strProgramID, false);
        }        

        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["Search"]) != "1")
            {
                Session["SelectedBrandIds"] = null;
                Session["searchText"] = null;
            }

            if (String.IsNullOrEmpty(strProgramID) && (!String.IsNullOrEmpty(strOrderID)))
                strProgramID = OrderServices.GetProgramIdByOrderId(strOrderID);
 
            DataTable lstBrands;
            lstBrands = DataOperations.GetDataTable(" SELECT pk_Brand_id, Brand, 1 AS displayOrder FROM Emerge.tblBrand UNION SELECT 9999 AS pk_Brand_id, 'ALL' AS Brand, 0 AS displayOrder ORDER BY displayOrder, Brand ");

            //lstBrands = DataOperations.GetDataTable(" SELECT pk_ItemType_ID AS pk_Brand_id, [Type] AS Brand, 1 AS displayOrder FROM Emerge.tblItemType UNION SELECT 9999 AS pk_Brand_id, 'ALL' AS Brand, 0 AS displayOrder ORDER BY displayOrder, Brand ");

            lvwSelectedBrands.DataSource = lstBrands.DataSet;
            lvwSelectedBrands.DataBind();
            LoadData();
        }

    }

    protected override void OnLoadComplete(EventArgs e)
    {
        base.OnLoadComplete(e);
        LoadBudgetDetails();  
    }

    private void LoadBudgetDetails()
    {
        GetBudgetDetails();

        litOrderTotal.Text = String.Format("{0:C}", dThisOrderAmount);

        if (dBudgetRemaining < 0)
            litRemainingBudget.Text = "<font color='red'> -" + String.Format("{0:C}", dBudgetRemaining * -1) + "</font>";
        else
            litRemainingBudget.Text = "<font color='green'>" + String.Format("{0:C}", dBudgetRemaining) + "</font>";
        litAllocatedBudget.Text = "<font color='black'>" + String.Format("{0:C}", dBudgetAllocated) + "</font>";
       
        hdnAllocatedBudget.Value = dBudgetAllocated.ToString();
        hdnRemainingBudget.Value = dBudgetRemaining.ToString();
        hdnOrderValue.Value = dThisOrderAmount.ToString();
    }

    private void LoadData()
    {
        if (String.IsNullOrEmpty(strCommaSeparatedAddressIDs))
            strCommaSeparatedAddressIDs = Convert.ToString(HttpContext.Current.Session["strCommaSeparatedAddressIDs"]);
        if (String.IsNullOrEmpty(strCommaSeparatedAddressIDs))
        {
            if (!String.IsNullOrEmpty(strOrderID))
            {
                DataTable dtSelAddresses = OrderServices.GetAddressesForOrderID(strOrderID);
                foreach (DataRow row in dtSelAddresses.Rows)
                {
                    strCommaSeparatedAddressIDs += row["fk_address_id"].ToString() + ",";
                }
                strCommaSeparatedAddressIDs = strCommaSeparatedAddressIDs.TrimEnd(',');
                Session["strCommaSeparatedAddressIDs"] = strCommaSeparatedAddressIDs;
            }
        }
        if (Session["searchText"] != null)
        {
            searchText = Session["searchText"].ToString();
        }
        if (Session["SelectedBrandIds"] != null)
        {
            strBrandIds = Session["SelectedBrandIds"].ToString();
        }

        lvwSeasonalOrderItems.DataSource = OrderServices.GetSeasonalOrderItems(strProgramID, strOrderID, strBrandIds, searchText); // Filter By Brand
        //lvwSeasonalOrderItems.DataSource = OrderServices.GetSeasonalOrderItemsFilterByItemType(strProgramID, strOrderID, strBrandIds, searchText); // Filter By ITem type
        
        lvwSeasonalOrderItems.DataBind();

        if (!String.IsNullOrEmpty(strOrderID))
            txtNotes.Text = OrderServices.GetOrderMetaDetails(strOrderID)["Notes"].ToString();
    }

    private void GetBudgetDetails()
    {
        if(String.IsNullOrEmpty(strProgramID))
            strProgramID = OrderServices.GetProgramIdByOrderId(strOrderID);
        
        DataRow drBudgetDetails = BudgetServices.GetMyBudgetDetails(strProgramID, strOrderID);

        dBudgetAllocated = Convert.ToDecimal(drBudgetDetails["BudgetAllocatedToMe"].ToString());
        Decimal dSpentOnOtherOrders = Convert.ToDecimal(drBudgetDetails["BudgetSpent"].ToString());
        dThisOrderAmount = GetThisOrderAmount();
        dBudgetRemaining = dBudgetAllocated - (dSpentOnOtherOrders + dThisOrderAmount);
    }

    protected void lvwSeasonalOrderItems_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        HiddenField hdnItemID = e.Item.FindControl("hdnItemID") as HiddenField;
        HiddenField hdnPrice = e.Item.FindControl("hdnPrice") as HiddenField;
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

        hdnItemID.Value = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "pk_Item_ID"));
        hdnPrice.Value = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "EstimatedPrice"));

        litItemName.Text = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "Item_Name"));
        litItemNo.Text = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "Item_No"));        
        litBrand.Text = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "Brand"));
        litDimension.Text = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "Dimension"));
        litItemType.Text = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "Type"));
        litPrice.Text = "$" + Convert.ToString(DataBinder.Eval(e.Item.DataItem, "EstimatedPrice"));
        
        litDescription.Text = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "Description"));


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
    }

    protected void Back(object sender, EventArgs e)
    {
        Response.Redirect("MyOrders.aspx", false);
    }

    protected void Confirm(object sender, EventArgs e)
    {
        Save("Confirm Method", null); //First invoke Save button, which ensure that hdnOrderID.Value has the right value
        if (failuremsg.Text != "")
            return;
        OrderServices.UpdateOrder(hdnOrderID.Value, txtNotes.Text.Trim(), "CONFIRMED") ;
        //OrderServices.UpdateOrder(hdnOrderID.Value, "CONFIRMED");

        MailServices.SendSeasonalOrderConfirmationEmailToSM(hdnOrderID.Value);
        MailServices.SendNewSeasonalOrderPlacedEmailToBMs(hdnOrderID.Value);

        Response.Redirect("MyOrders.aspx", false);
    }

    private bool EnteringNegativeBudget()
    {
        GetBudgetDetails();
        if (dBudgetRemaining < 0)
            return true;
        else
            return false;
    }

    protected void Save(object sender, EventArgs e)
    {   
        /**/
        if(EnteringNegativeBudget())
        {
            if (txtNotes.Text.Trim() == "")
            {
                failuremsg.Text = @"<b><u>The order value is more than the allocated budget.<u></b> <br/><br/> 
                                Please reduce the order value to proceed.";
                return;
            }
        }
       /* */
        //Proceed with saving
        if (hdnOrderID.Value == "")
            hdnOrderID.Value = OrderServices.CreateEmptyOrder(strProgramID, strCommaSeparatedAddressIDs);

        OrderServices.UpdateOrder(hdnOrderID.Value, txtNotes.Text.Trim(), "OPEN");


        foreach (ListViewItem item in lvwSeasonalOrderItems.Items)
        {
            HiddenField hdnItemID = item.FindControl("hdnItemID") as HiddenField;
            string strItemID = hdnItemID.Value;
            UserControls_CompositeCart compositeCart = item.FindControl("compositeCart") as UserControls_CompositeCart;
            ListView lvwCompositeCart = compositeCart.FindControl("lvwCompositeCart") as ListView;

            foreach(ListViewItem cartItems in lvwCompositeCart.Items)
            {
                HiddenField hdnAddressId = cartItems.FindControl("hdnAddressId") as HiddenField;
                TextBox txtQuantity = cartItems.FindControl("txtQuantity") as TextBox;
                string strAddressID = hdnAddressId.Value;
                string strQuantity = txtQuantity.Text;
                OrderServices.SaveOrderQuantity(strItemID, strAddressID, hdnOrderID.Value, strQuantity);
            }
        }
        if (sender.ToString() != "Confirm Method")
            Response.Redirect("SeasonalOrder.aspx?OrderID=" + hdnOrderID.Value);
        //LoadBudgetDetails();
    }

    private Decimal GetThisOrderAmount()
    {        
        Decimal dTotalSpendBeforeSave = 0;
        foreach (ListViewItem item in lvwSeasonalOrderItems.Items)
        {            
            HiddenField hdnPrice = item.FindControl("hdnPrice") as HiddenField;
            Decimal dPrice = Convert.ToDecimal(hdnPrice.Value);
            UserControls_CompositeCart compositeCart = item.FindControl("compositeCart") as UserControls_CompositeCart;
            ListView lvwCompositeCart = compositeCart.FindControl("lvwCompositeCart") as ListView;

            foreach (ListViewItem cartItems in lvwCompositeCart.Items)
            {
                Decimal dQty;
                TextBox txtQuantity = cartItems.FindControl("txtQuantity") as TextBox;
                txtQuantity.Text = txtQuantity.Text == "" ? "0": txtQuantity.Text;
                dQty = Convert.ToDecimal(txtQuantity.Text);
                dTotalSpendBeforeSave += (dQty * dPrice);
            }
        }
        return dTotalSpendBeforeSave;
    }

    protected void lvwSelectedBrands_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        CheckBox chkBrand = (CheckBox)e.Item.FindControl("chkBrand");
        chkBrand.Text = DataBinder.Eval(e.Item.DataItem, "Brand").ToString();
        chkBrand.Attributes.Add("pk_Brand_id", DataBinder.Eval(e.Item.DataItem, "pk_Brand_id").ToString());
        if (Session["SelectedBrandIds"] == "")
        {
            chkBrand.Checked = false;
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
                    if (arrListofBrandIds[nCount].ToString() == DataBinder.Eval(e.Item.DataItem, "pk_Brand_id").ToString())
                        chkBrand.Checked = true;
                }
            }
        }
    }

    protected void SearchItems(object sender, EventArgs e)
    {
        //DataSet FilteredItems;
        searchText = txtSearch.Text.ToString();
        Session["searchText"] = searchText;

        List<string> ListOfBrandIds = new List<string>();
        foreach (ListViewDataItem lvi in lvwSelectedBrands.Items)
        {
            CheckBox selectedCheBox = (CheckBox)lvi.FindControl("chkBrand");
            if (selectedCheBox.Checked)
            {
                string brandId = selectedCheBox.Attributes["pk_Brand_id"];
                ListOfBrandIds.Add(brandId);
            }
        }
        strBrandIds = string.Join(",", ListOfBrandIds);
        Session["SelectedBrandIds"] = strBrandIds;
       
        if (Request.QueryString["search"] == null)
            Response.Redirect("SeasonalOrder.aspx?search=1&" + Request.QueryString);
        else
            Response.Redirect("SeasonalOrder.aspx?" + Request.QueryString);
    }
}