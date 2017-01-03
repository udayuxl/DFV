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

public partial class Reports_ProgramOrderDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindPrograms(null, null);
        }
    }

    private void DisplayError(string ErrMsg)
    {
        litErrMsg.Text = "<p class='error'>" + ErrMsg + "</p>";
    }
        
    protected void GenerateReport(object sender, EventArgs e)
    {             
        string strProgramID = ddlPrograms.SelectedItem.Value;
        if (strProgramID == "-1")
            return;

        ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"~/Reports/ReportFiles/ProgramOrderDetails.rdlc");        
        ReportViewer1.LocalReport.DataSources.Clear();                

        DataTable dtReport = DataOperations.GetDataTable("EXEC Emerge.RepProgramOrderDetails @ProgramID = " + strProgramID + ", @UserID = '" + Membership.GetUser().ProviderUserKey.ToString() + "'");

        if(dtReport.Rows.Count > 0)
        {
            ltMsg.Visible = false;
            ReportViewer1.Visible = true;
            ReportParameter[] param = new ReportParameter[1];
            param[0] = new ReportParameter("SelectedProgram", ddlPrograms.SelectedItem.Text);
            ReportViewer1.LocalReport.SetParameters(param);
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("RepOrderRecapDataSet", dtReport));
            ReportViewer1.LocalReport.Refresh();
        }
        else
        {
            ReportViewer1.Visible = false;
            ltMsg.Visible = true;
        }
    }

    protected void BindPrograms(object sender, EventArgs e)
    {
        DataTable dtPrograms = AdminServices.GetPrograms();
        ReportViewer1.Visible = false;
        ddlPrograms.Items.Clear();
        ddlPrograms.Items.Add(new ListItem("Select", "-1"));        
        ddlPrograms.AppendDataBoundItems = true;
        
        ddlPrograms.DataSource = dtPrograms;
        ddlPrograms.DataTextField = "Name";
        ddlPrograms.DataValueField = "pk_Program_Id";
        ddlPrograms.DataBind();
        ddlPrograms.Items.Add(new ListItem("Current Market Period", "0"));
    }
}
