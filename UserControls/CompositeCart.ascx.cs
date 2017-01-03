using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Lumen;
using ACE;


public partial class UserControls_CompositeCart : System.Web.UI.UserControl
{
    protected string _Mode;
    protected bool _ReadOnly;
    protected double _ItemPrice;
    protected int _ApprovalReq = 0;
    protected int _CartIndex = 1;
    protected bool _isInventoryOrder= false;
    protected int _MaxOrderQty = 0;
    protected int _AvailableStock = 0;


    public bool ReadOnly
    {
        get { return _ReadOnly; }
        set { _ReadOnly = value; }
    }
    public string Mode
    {
        get { return _Mode; }
        set { _Mode = value; }
    }
    public string propItemID
    {
        get { return hdnItemId.Value; }
        set { hdnItemId.Value = value; }
    }
    public string propOrderID
    {
        get { return hdnOrderId.Value; }
        set { hdnOrderId.Value = value; }
    }
    public string propProgramID
    {
        get { return hdnProgramId.Value; }
        set { hdnProgramId.Value = value; }
    }

    public string propItemPrice {get; set;}
    public string propCommaSeparatedAddressIDs { get; set; }
    
    public int propApprovalReq
    {
        get { return _ApprovalReq; }
        set { _ApprovalReq = value; }
    }

    public string propItemNo
    {
        get { return hdnItemNo.Value; }
        set { hdnItemNo.Value = value; }
    }

    public int propCartIndex
    {
        get { return _CartIndex; }
        set { _CartIndex = value; }
    }

    public bool IsInventoryOrder
    {
        get { return _isInventoryOrder; }
        set { _isInventoryOrder = value; }
    }
    public int propMaxOrderQty
    {
        get { return _MaxOrderQty; }
        set { _MaxOrderQty = value; }
    }
    public int propAvailableStock
    {
        get { return _AvailableStock; }
        set { _AvailableStock = value; }
    }

    public double SubTotal { get { return hdnSubTotal.Value == "" ? 0 : Convert.ToDouble(hdnSubTotal.Value); } set { hdnSubTotal.Value = value.ToString(); } }

    public static double TotalPrice;
    public static int TotalQuantity;
    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!IsPostBack)
        {
            BindCompositeCart();
        }     
    }

    protected void BindCompositeCart()
    {
        string strProgramID = Convert.ToString(Request.QueryString["ProgramID"]);
        string strOrderID = Convert.ToString(Request.QueryString["OrderID"]);
        lvwCompositeCart.DataSource = OrderServices.GetCompositeCartAddresses(strProgramID, strOrderID, propItemID, propCommaSeparatedAddressIDs);
        lvwCompositeCart.DataBind();

        spanItemPrice.InnerHtml = Convert.ToString(propItemPrice); 
        lblTotalQty.Attributes.Add("cartindex", propCartIndex.ToString());
        lblTotalPrice.Attributes.Add("cartindex", propCartIndex.ToString());
        spanItemPrice.Attributes.Add("cartindex", propCartIndex.ToString());
        lblMsgs1.Attributes.Add("cartindex", propCartIndex.ToString());
        lblMsgs2.Attributes.Add("cartindex", propCartIndex.ToString());
    }

    protected void lvwCompositeCart_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        TextBox txtQuantity = (TextBox)e.Item.FindControl("txtQuantity");
        ListViewDataItem lvsDataItem = (ListViewDataItem)e.Item;

        txtQuantity.Attributes.Add("cartindex", propCartIndex.ToString());
        txtQuantity.Text = DataBinder.Eval(lvsDataItem.DataItem, "Qty", "");
        if (txtQuantity.Text == "")
            txtQuantity.Text = "0";
        txtQuantity.Attributes.Add("previousQty", txtQuantity.Text);
        txtQuantity.Attributes.Add("itemid", propItemID);
        txtQuantity.Attributes.Add("approvReq", propApprovalReq.ToString());
        txtQuantity.Attributes.Add("StateCode", DataBinder.Eval(lvsDataItem.DataItem, "state").ToString());
        txtQuantity.Attributes.Add("price", Convert.ToString(propItemPrice));
        txtQuantity.Attributes.Add("maxOrdQty", Convert.ToString(propMaxOrderQty));
        txtQuantity.Attributes.Add("availbleStock", Convert.ToString(propAvailableStock));

        if(_ReadOnly)
            txtQuantity.Enabled = false;

        if (OrderServices.IsItemValidInState(propItemID, DataBinder.Eval(lvsDataItem.DataItem, "state").ToString()) == false)
        {
            txtQuantity.Enabled = false;
            txtQuantity.Attributes.Add("title", "Not valid in this State - " + DataBinder.Eval(lvsDataItem.DataItem, "state").ToString() + "");
        }

        TotalQuantity = TotalQuantity + Convert.ToInt32(txtQuantity.Text);


    }

}
 

