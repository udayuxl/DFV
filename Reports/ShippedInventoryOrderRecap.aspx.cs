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

public partial class Reports_ShippedInventoryOrderRecap : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindBrands(null, null);
            BindTypes(null, null);
        }
    }

    private void DisplayError(string ErrMsg)
    {
        litErrMsg.Text = "<p class='error'>" + ErrMsg + "</p>";
    }
        
    protected void GenerateReport(object sender, EventArgs e)
    {
        ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"~/Reports/ReportFiles/ShippedInventoryOrderRecap.rdlc");        
        ReportViewer1.LocalReport.DataSources.Clear();
        string strBrandID = ddlBrands.SelectedItem.Value;
        string strItemTypeID = ddlItemTypes.SelectedItem.Value;
        string strUserID = Membership.GetUser().ProviderUserKey.ToString();

        DataTable dtReport = DataOperations.GetDataTable("EXEC Emerge.RepInventoryShippedOrderRecap @UserID = '" + strUserID + "', @Brands = '" + strBrandID + "', @ItemTypes = '" + strItemTypeID + "'");
        //Response.Write("EXEC Emerge.RepInventoryOrderRecap @UserID = '" + strUserID + "', @Brands = '" + strBrandID + "', @ItemTypes = '" + strItemTypeID + "'");
        if(dtReport.Rows.Count > 0)
        {
            ltMsg.Visible = false;
            ReportViewer1.Visible = true;
            ReportParameter[] param = new ReportParameter[1];
            param[0] = new ReportParameter("SelectedBrand", ddlBrands.SelectedItem.Text);
            ReportViewer1.LocalReport.SetParameters(param);
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("RepInventoryOrderRecap", dtReport));
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

    protected void BindTypes(object sender, EventArgs e)
    {
        DataTable dtItemTypes = DataOperations.GetDataTable("SELECT pk_ItemType_ID, Type FROM Emerge.tblItemType");
        ReportViewer1.Visible = false;
        ddlItemTypes.Items.Clear();
        ddlItemTypes.Items.Add(new ListItem("All", ""));
        ddlItemTypes.AppendDataBoundItems = true;

        ddlItemTypes.DataSource = dtItemTypes;
        ddlItemTypes.DataTextField = "Type";
        ddlItemTypes.DataValueField = "pk_ItemType_ID";
        ddlItemTypes.DataBind();

    }
}
