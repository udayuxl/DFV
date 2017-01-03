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
using System.Web.Configuration;
using System.Xml.Linq;
using Microsoft.Reporting.WebForms;
using Lumen;
using ACE;

public partial class Reports_ProgramPlaybook : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            BindPrograms(null,null);
    }

    protected void GenerateReport(object sender, EventArgs e)
    {
        string strProgramID = ddlPrograms.SelectedItem.Value;
        string strImageLibraryBaseURL = "http://fetzer.ambood.com/ImageLibrary/Thumbnails/";

        ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"~/Reports/ReportFiles/ProgramPlaybook.rdlc");
        ReportViewer1.LocalReport.DataSources.Clear();
        DataTable dtPlaybook = DataOperations.GetDataTable("EXEC [Emerge].[RepProgramPlayBook] @ProgramID=" + strProgramID + ", @ImageLibraryBaseURL = '" + strImageLibraryBaseURL +"'" );

        ReportViewer1.Visible = true;
        ReportViewer1.LocalReport.EnableExternalImages = true;
        ReportParameter[] param = new ReportParameter[1];
        param[0] = new ReportParameter("SelectedProgram", ddlPrograms.SelectedItem.Text);
        ReportViewer1.LocalReport.SetParameters(param);
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("RepProgramBookResult",dtPlaybook));
        ReportViewer1.LocalReport.EnableExternalImages = true;
        ReportViewer1.LocalReport.Refresh();
    }

    protected void BindPrograms(object sender, EventArgs e)
    {
        DataTable dtPrograms = AdminServices.GetPrograms();
        ReportViewer1.Visible = false;
        ddlPrograms.Items.Clear();
        ddlPrograms.Items.Add(new ListItem("All", "0"));
        ddlPrograms.AppendDataBoundItems = true;

        ddlPrograms.DataSource = dtPrograms;
        ddlPrograms.DataTextField = "Name";
        ddlPrograms.DataValueField = "pk_Program_Id";
        ddlPrograms.DataBind();

    }
}
