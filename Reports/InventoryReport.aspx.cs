using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.Reporting.WebForms;
using Lumen;
using ACE;

public partial class Reports_InventoryReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindBrands(null, null);
        }
    }

    private void DisplayError(string ErrMsg)
    {
        litErrMsg.Text = "<p class='error'>" + ErrMsg + "</p>";
    }
        
    protected void GenerateReport(object sender, EventArgs e)
    {
        ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"~/Reports/ReportFiles/InventoryReport.rdlc");        
        ReportViewer1.LocalReport.DataSources.Clear();
        string strBrandID = ddlBrands.SelectedItem.Value;
        DataTable dtReport = DataOperations.GetDataTable("EXEC Emerge.RepInventory @Brands = '" + strBrandID + "'");
        if(dtReport.Rows.Count > 0)
        {
            ltMsg.Visible = false;
            ReportViewer1.Visible = true;
            ReportParameter[] param = new ReportParameter[1];
            param[0] = new ReportParameter("SelectedBrand", ddlBrands.SelectedItem.Text);
            ReportViewer1.LocalReport.SetParameters(param);
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("RepInventory", dtReport));
            ReportViewer1.LocalReport.Refresh();
        }
        else
        {
            ReportViewer1.Visible = false;
            ltMsg.Visible = true;
        }
    }

    protected void BindBrands(object sender, EventArgs e)
    {
        DataTable dtBrands = DataOperations.GetDataTable("SELECT pk_Brand_id, Brand FROM Emerge.tblBrand");
        ReportViewer1.Visible = false;
        ddlBrands.Items.Clear();        
        ddlBrands.Items.Add(new ListItem("All", ""));
        ddlBrands.AppendDataBoundItems = true;

        ddlBrands.DataSource = dtBrands;
        ddlBrands.DataTextField = "Brand";
        ddlBrands.DataValueField = "pk_Brand_id";
        ddlBrands.DataBind();

    }
}
