<%@ Page Title="Program Playbook" Language="C#" MasterPageFile="~/LumenApp.master" AutoEventWireup="true" CodeFile="ProgramPlaybook.aspx.cs" Inherits="Reports_ProgramPlaybook" %>

<asp:Content runat="server" ContentPlaceHolderID="Title">
    <h2>Program Playbook</h2>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server">    
    
    <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Always" ChildrenAsTriggers="true" runat="server" EnableViewState="true" Visible="false">
        <ContentTemplate>
            <asp:Literal ID="litErrMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <div class="report-section">
        <label>Select Program</label> <asp:DropDownList ID="ddlPrograms" runat="server" />
        <asp:Button ID="btnGenerateReport" runat="server" Text="Generate Report" OnClick="GenerateReport" CssClass="button" />		
    </div>
    
    <div class="report">

    </div>

    <asp:Literal ID="ltMsg" Text="<h3>No data found.</h3>" Visible="false" runat="server" />    
    <SRS:ReportViewer ID="ReportViewer2" Visible="false" ProcessingMode="Local" CssClass="ReportContainer2" AsyncRendering="false" SizeToReportContent="true" ShowToolBar="true" ShowPrintButton="false" runat="server" />

    <div style="width:790px;overflow:auto">
            
        <SRS:ReportViewer ID="ReportViewer1" Visible="false" ProcessingMode="Local" 
        PageCountMode="Estimate" CssClass="ReportContainer2" 
        AsyncRendering="false"  Width="770px" Height="250px"
        SizeToReportContent="false" ShowToolBar="true" ShowPrintButton="false" runat="server" />

    </div>

</asp:Content>