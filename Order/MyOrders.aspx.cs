using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Lumen;
using ACE;

public partial class Order_MyOrders : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadData();
        }
    }

    private void LoadData()
    {
        DataTable dtMyOrders = OrderServices.GetMyOrders();
        gridMyOrders.DataSource = dtMyOrders;
        gridMyOrders.DataBind();
        gridMyOrders.RowDataBound += new GridViewRowEventHandler(gridMyOrders_RowDataBound);
    }

    protected void gridMyOrders_RowDataBound(object sender, GridViewRowEventArgs e)
    {        
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        Literal litProgramStatus = e.Row.FindControl("litProgramStatus") as Literal;
        Button btnEditOrder = e.Row.FindControl("btnEditOrder") as Button;
        string strOrderID = e.Row.Cells[0].Text;
        string strOrderStatus = e.Row.Cells[6].Text;

        // If Order Status is confirmed OR If the Program is closed...
        if (strOrderStatus == "CONFIRMED" || !OrderServices.IsOrderWindowOpen(strOrderID)) // If Order Status is confirmed
            btnEditOrder.Text = "View Order";

        // If the Program is closed...
        if (!OrderServices.IsOrderWindowOpen(strOrderID))
            litProgramStatus.Text = "Window Closed";
        else
            litProgramStatus.Text = "Window Open";
    }

    protected void ViewOrderDetails(object sender, EventArgs e)
    {
    }

    protected void EditOrder(object sender, EventArgs e)
    {
        GridViewRow row = ((Button)sender).Parent.Parent as GridViewRow;
        string strOrderID = row.Cells[0].Text;
        string strOrderStatus = row.Cells[6].Text;

        // If Order Status is confirmed OR If the Program is closed...
        if (strOrderStatus == "CONFIRMED" || !OrderServices.IsOrderWindowOpen(strOrderID)) // If Order Status is confirmed
            Response.Redirect("SeasonalOrder.aspx?OrderID=" + strOrderID + "&readonly=true", false);
        else
            Response.Redirect("SeasonalOrder.aspx?OrderID=" + strOrderID,false);
    }
}