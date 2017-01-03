<%@ Page Language="C#" MasterPageFile="~/LumenApp.master" AutoEventWireup="true" Title="Inventory Report" CodeFile="InventoryReport.aspx.cs" Inherits="Reports_InventoryReport" %>

<asp:Content ContentPlaceHolderID="Title" runat="server">
    <h2>Inventory Report</h2>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server">
    <style>
        div.report-section label{color:#2060a0}
        input.button{background:#2060a0}
        div.header div.headerleft h2 {color:#2060a0}
    </style>    
    
     <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Always" ChildrenAsTriggers="true" runat="server" EnableViewState="true">
        <ContentTemplate>
            <asp:Literal ID="litErrMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <div class="report-section">
        <div>
            <label>Select Brand</label> 
            <asp:DropDownList ID="ddlBrands" runat="server" OnSelectedIndexChanged="GenerateReport"/>
            <asp:Button ID="btnGenerateReport" runat="server" Text="Generate Report" OnClick="GenerateReport" CssClass="button"  style="float:right;margin-right:10px;"/>
        </div>
        <div style="margin-top:10px; margin-bottom:45px;">
        
        </div>
    </div>

    <asp:Literal ID="ltMsg" Text="<h3 style=text-align:center>There is no data for the selected criteria, please make another selection.</h3>" Visible="false" runat="server" />    


    <div style="width:790px;overflow:auto">
            
        <SRS:ReportViewer ID="ReportViewer1" Visible="false" ProcessingMode="Local" 
        PageCountMode="Estimate" CssClass="ReportContainer1" 
        AsyncRendering="false"  Width="770px" Height="450px"
        SizeToReportContent="false" ShowToolBar="true" ShowPrintButton="false" runat="server" />

    </div>


    <SRS:ReportViewer ID="ReportViewer2" Visible="false" ProcessingMode="Local" CssClass="ReportContainer2" AsyncRendering="false" SizeToReportContent="true" ShowToolBar="true" ShowPrintButton="false" runat="server" />
</asp:Content>