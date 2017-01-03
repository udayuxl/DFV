using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lumen;

public partial class Home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        rptCurrentPrograms.DataSource = OrderServices.GetDashboardPrograms();
        rptCurrentPrograms.DataBind(); 
    }

    protected void rptCurrentPrograms_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        object Program = ((System.Web.UI.WebControls.RepeaterItem)(e.Item)).DataItem;

        HyperLink imgProgramPreviewImage = e.Item.FindControl("imgProgramPreviewImage") as HyperLink;
        HyperLink lnkProgramName = e.Item.FindControl("lnkProgramName") as HyperLink;
        Literal litClosingDate = e.Item.FindControl("litClosingDate") as Literal;
        Literal litMarketDate = e.Item.FindControl("litMarketDate") as Literal;

        imgProgramPreviewImage.ImageUrl = DataBinder.Eval(Program, "PreviewImageFileName").ToString();
        imgProgramPreviewImage.NavigateUrl = "/Order/Addressbook.aspx?Source=SeasonalPrograms&ProgramID=" + DataBinder.Eval(Program, "pk_Program_Id").ToString();

        lnkProgramName.Text = DataBinder.Eval(Program, "Name").ToString();
        lnkProgramName.NavigateUrl = "/Order/Addressbook.aspx?Source=SeasonalPrograms&ProgramID=" + DataBinder.Eval(Program, "pk_Program_Id").ToString();

        litClosingDate.Text = Convert.ToDateTime(DataBinder.Eval(Program, "OrderWindowEnd").ToString()).ToShortDateString();
        litMarketDate.Text = Convert.ToDateTime(DataBinder.Eval(Program, "InMarketDate").ToString()).ToShortDateString();
    }
    
}