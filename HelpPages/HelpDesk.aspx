<%@ Page Language="C#" MasterPageFile="~/LumenNoLeft.master" AutoEventWireup="true" CodeFile="HelpDesk.aspx.cs" Inherits="Order_HelpDesk" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server">
    <div style="padding:20px 20px 20px 20px;">
    <h3><asp:Label ID="h3Title" runat="server">Log a Case</asp:Label></h3>
    <iframe src="ttp://irg.force.com/helpdesk"  height="500" width="930" style="border-style:solid;border-color:Silver;border-width:thin" runat="server" id="iFrameForce">
      <p>Your browser does not support this feature.</p>
    </iframe>
    </div>
    </asp:Content>