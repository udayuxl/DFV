<%@ Page Language="C#" MasterPageFile="~/LumenApp.master" AutoEventWireup="true" Title="Order Recap" CodeFile="BudgetAllocation.aspx.cs" Inherits="Reports_Pre_OrderRecap" %>

<asp:Content ContentPlaceHolderID="Title" runat="server">
    <h2>Budget Allocation and Usage Report</h2>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server">
    <style>
        div.report-section label{color:#93011B}
        input.button{background:#93011B}
        div.header div.headerleft h2 {color:#93011B}
    </style>    
    
     <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Always" ChildrenAsTriggers="true" runat="server" EnableViewState="true">
        <ContentTemplate>
            <asp:Literal ID="litErrMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <div class="report-section">
        <table>
            <tr>
                <td>Program: </td>
                <td><asp:DropDownList ID="ddlPrograms" runat="server" OnSelectedIndexChanged="GenerateReport" AutoPostBack="true"/></td>
                <td style="width:40px;"> </td>
                <td>
                    <div id="divAdmin1" runat="server" visible="false" style="float:left">
                        RVP:
                        <asp:DropDownList ID="ddlDVPs" runat="server" OnSelectedIndexChanged="GenerateReport" AutoPostBack="true">
                            <asp:ListItem Text="All Sales Managers" Value=""></asp:ListItem>
                        </asp:DropDownList>  
                    </div>                                  
                </td>
                <td style="width:40px;"> </td>
                <td>
                    <asp:Button ID="btnGenerateReport" runat="server" Text="Generate Report" OnClick="GenerateReport" CssClass="button"/>
                </td>
            </tr>
        </table>
                
            
            
        </div>
        <div style="margin-top:10px; margin-bottom:45px;">
        
        </div>
    </div>

    <asp:Literal ID="ltMsg" Text="<h3 style=text-align:center>There is no budget allocations made for this program</h3>" Visible="false" runat="server" />    
    <SRS:ReportViewer ID="ReportViewer2" Visible="false" ProcessingMode="Local" CssClass="ReportContainer2" AsyncRendering="false" SizeToReportContent="true" ShowToolBar="true" ShowPrintButton="false" runat="server" />
    <div style="width:790px;overflow:auto">
            
        <SRS:ReportViewer ID="ReportViewer1" Visible="false" ProcessingMode="Local" 
        PageCountMode="Estimate" CssClass="ReportContainer2" 
        AsyncRendering="false"  Width="770px" Height="250px"
        SizeToReportContent="false" ShowToolBar="true" ShowPrintButton="false" runat="server" />

    </div>
</asp:Content>