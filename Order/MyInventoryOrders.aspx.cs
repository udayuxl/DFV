using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Lumen;
using ACE;

public partial class Order_MyInventoryOrders : System.Web.UI.Page
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
        DataTable dtMyOrders = OrderServices.GetMyInventoryOrders();
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

       /*
        if (!OrderServices.IsOrderWindowOpen(strOrderID))
        {
            litProgramStatus.Text = "Window Closed";
            btnEditOrder.Text = "View Order";            
        }
        else
            litProgramStatus.Text = "Window Open";
        */
        string orderStatus = ((DataRowView)e.Row.DataItem)["status"].ToString(); 
        if (orderStatus.ToUpper() != "OPEN")
            btnEditOrder.Text = "View Order";
        else
            btnEditOrder.Text = "Edit Order";
    }

    protected void ViewOrderDetails(object sender, EventArgs e)
    {
    }

    protected void EditOrder(object sender, EventArgs e)
    {
        GridViewRow row = ((Button)sender).Parent.Parent as GridViewRow;
        string strOrderID = row.Cells[0].Text;
        strOrderID = strOrderID.Split('-')[0];
        /*
        if (!OrderServices.IsOrderWindowOpen(strOrderID))
            Response.Redirect("InventoryOrder.aspx?OrderID=" + strOrderID + "&readonly=true", false);
        else
            Response.Redirect("InventoryOrder.aspx?OrderID=" + strOrderID, false);
        */
        string orderStatus = row.Cells[5].Text;
        if (orderStatus.ToUpper() != "OPEN")
            Response.Redirect("InventoryOrder.aspx?OrderID=" + strOrderID + "&readonly=true", false);
        else
            Response.Redirect("InventoryOrder.aspx?OrderID=" + strOrderID, false);
    }
}