using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using ACE;
using Lumen;

public partial class Order_OpenPrograms : System.Web.UI.Page
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
        DataTable dtDashboardPrograms = OrderServices.GetDashboardPrograms();
        rptOpenPrograms.DataSource = dtDashboardPrograms;
        rptOpenPrograms.DataBind();

        if(rptOpenPrograms.Items.Count == 0)
            pnlEmptyMessage.Visible = true;
    }

    protected void rptOpenPrograms_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {        
        HyperLink lnkProgramPreviewImage = e.Item.FindControl("lnkProgramPreviewImage") as HyperLink;
        HyperLink lnkProgramName = e.Item.FindControl("lnkProgramName") as HyperLink;
        HyperLink lnkOrder = e.Item.FindControl("lnkOrder") as HyperLink;
        Label lblClosingDate = e.Item.FindControl("lblClosingDate") as Label;
        Label lblMarketDate = e.Item.FindControl("lblMarketDate") as Label;
        Label lblBudget = e.Item.FindControl("lblBudget") as Label;

        HtmlGenericControl divBudgetDetails = e.Item.FindControl("divBudgetDetails") as HtmlGenericControl;
        divBudgetDetails.Visible = OrderServices.CanPlaceOrder();

        lnkProgramPreviewImage.ImageUrl = "~/ImageLibrary/Thumbnails/" + DataBinder.Eval(e.Item.DataItem, "PreviewImageFileName").ToString();
        lnkProgramPreviewImage.NavigateUrl = "~/ImageLibrary/Large/" + DataBinder.Eval(e.Item.DataItem, "PreviewImageFileName").ToString();
        lnkProgramName.Text = DataBinder.Eval(e.Item.DataItem, "Name").ToString();
        if (OrderServices.CanPlaceOrder())
        {
            lnkProgramName.NavigateUrl = "AddressBook.aspx?ProgramID=" + DataBinder.Eval(e.Item.DataItem, "pk_Program_ID").ToString();
            lnkOrder.NavigateUrl = "AddressBook.aspx?ProgramID=" + DataBinder.Eval(e.Item.DataItem, "pk_Program_ID").ToString();
        }
        else
        {
            lnkOrder.Visible = false;
            lnkProgramName.Enabled = false;
        }
        lblClosingDate.Text = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "OrderWindowEnd")).ToShortDateString();
        lblMarketDate.Text = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "InMarketDate")).ToShortDateString();
        Decimal BudgetAvailable = 0;
        string strBudgetAvailable = BudgetServices.GetMyBudgetDetails(DataBinder.Eval(e.Item.DataItem, "pk_Program_ID").ToString(), "0")["BudgetRemaining"].ToString();
        if (strBudgetAvailable == "")
            BudgetAvailable = 0;
        else
            BudgetAvailable = Convert.ToDecimal(strBudgetAvailable);

        if (BudgetAvailable < 0)
        {
            lblBudget.Text = String.Format("- {0:C}", BudgetAvailable);
            lblBudget.ForeColor = System.Drawing.Color.Red;
        }
        else
            lblBudget.Text = String.Format("{0:C}", BudgetAvailable);


        if (lnkProgramPreviewImage.ImageUrl == "~/ImageLibrary/Thumbnails/")
            lnkProgramPreviewImage.ImageUrl = "~/ImageLibrary/NoImage.jpg";
    }
}