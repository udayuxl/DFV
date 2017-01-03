<%@ Page Language="C#" MasterPageFile="~/LumenNoLeft.master" AutoEventWireup="true" CodeFile="MyProfile.aspx.cs" Inherits="Settings_MyProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server">
    <div>
        <div class="header" style="width: 960px !important;">
            <div class="headerleft">
                <h2><asp:Literal ID="litPageTitle" Text="My Profile" runat="server" /></h2>
            </div>
        </div>

        <div style="padding:30px;clear:both">
            <ul style="list-style:none">
                <li style="margin-bottom:10px">
                    <asp:HyperLink ID="HyperLink2" NavigateUrl="~/Order/AddressBook.aspx" runat="server" Text="Manage My Address Book"></asp:HyperLink>
                </li>            
                <li style="margin-bottom:10px">
                    <asp:HyperLink ID="HyperLink1" NavigateUrl="~/Settings/ChangePassword.aspx" runat="server" Text="Change Password"></asp:HyperLink>
                </li>            
            </ul>
            
            
            
        </div>
        
    </div>
</asp:Content>
