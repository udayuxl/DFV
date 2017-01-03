<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Footer.ascx.cs" Inherits="UserControls_Footer" %>
 
    <div id="divBottomMenu" runat="server" class="BottomNav" >
        <%--<asp:HyperLink NavigateUrl="~/TermsAndConditions.aspx" runat="server" ID="lnkbtnTermsAndConditions" Text="Terms and Conditions" />
        | <asp:HyperLink NavigateUrl="http://www.drinkiq.com" Target="_blank"   runat="server" ID="lnkbtnDrinkIQ" Text="DRINKiQ" />--%>
        <%--asp:Literal ID="separator1" Text="|" runat="server"></asp:Literal> 
        <asp:HyperLink NavigateUrl="~/Admin/pwdreset.aspx" Target="_blank" runat="server" ID="AdminLink1" Text="System Users" />
        <asp:Literal ID="separator2" Text="|" runat="server"></asp:Literal> --%>
<%--        <asp:HyperLink NavigateUrl="~/Order/MyInventoryProgramOrders.aspx"  runat="server" ID="AdminLink2" Text="Order Editor" />
        <asp:Literal ID="separator4" Text="|" runat="server"></asp:Literal> 
        <asp:HyperLink NavigateUrl="~/Admin/POS_Items.aspx"  runat="server" ID="AdminLink4" Text="Item Editor" />
        <asp:Literal ID="separator3" Text="|" runat="server"></asp:Literal> 
        <asp:HyperLink NavigateUrl="~/Order/AddressBook.aspx"  runat="server" ID="AdminLink3" Text="All Addresses" />--%>
        <%-- <asp:Literal ID="separator5" Text="|" runat="server"></asp:Literal> --%>
        <%--<asp:HyperLink NavigateUrl="~/Admin/DropShipList.aspx"  runat="server" ID="AdminLink5" Text="Drop Ship List"  />--%> 
         <%--<asp:Literal ID="separator6" Text="|" runat="server"></asp:Literal>--%>
        <%--<asp:HyperLink NavigateUrl="~/Settings/ChangePassword.aspx"  runat="server" ID="ChangePassword" Text="Change Password"  />--%>
         <%--<asp:Literal ID="separator7" Text="|" runat="server"></asp:Literal> 
        <asp:HyperLink NavigateUrl="~/Order/Help.aspx"  runat="server" ID="Help" Text="Help"  />--%>
       
       <%--<asp:Literal ID="Literal1" Text="|" runat="server"></asp:Literal> 
        <asp:HyperLink NavigateUrl="~/HelpPages/HelpDesk.aspx"  runat="server" ID="HyperLink1" Text="Help Desk"  /> --%>
       <%--<asp:Literal ID="Literal2" Text="|" runat="server"></asp:Literal> 
        <asp:HyperLink NavigateUrl="~/HelpPages/HelpDesk.aspx?page=list"  runat="server" ID="HyperLink2" Text="My Cases"  />       
        --%>
    </div>

    <div style="float:right;padding-top:5px;padding-right:20px;color" runat="server" id="divSuperAdminTools" visible="false">        
        <asp:HyperLink NavigateUrl="~/Admin/Tools/Users.aspx"  runat="server" ID="HyperLink3" Text="Create Users" />
        |
        <asp:HyperLink NavigateUrl="~/Admin/Tools/pwdreset.aspx"  runat="server" ID="HyperLink5" Text="PWD RESET" />
        |
        <asp:HyperLink NavigateUrl="~/Admin/Tools/Query.aspx"  runat="server" ID="HyperLink4" Text="SQL Query" />
    </div>

