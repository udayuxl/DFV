<%@ Page Title="" Language="C#" MasterPageFile="~/LumenNoLeft.master" AutoEventWireup="true" CodeFile="UserHierarchy.aspx.cs" Inherits="Admin_Tools_UserHierarchy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleTag" Runat="Server">
    UserHierarchy - AdminTool
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Title" Runat="Server">
    Admin Tool - User Hierarchy
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" Runat="Server">
    <asp:GridView runat="server" ID="gridUserHierarchy" AutoGenerateColumns="false" OnRowDataBound="gridUserHierarchy_RowDataBound">
        <Columns>
            <asp:BoundField HeaderText = "User" DataField="username" ItemStyle-Width="150px"/>
            <asp:BoundField HeaderText = "Email" DataField="LoginEmail" ItemStyle-Width="150px" />
            <asp:BoundField HeaderText = "Role" DataField="Role" ItemStyle-Width="50px" />
            <asp:TemplateField HeaderText="Reports To">
                <ItemTemplate>
                    <asp:HiddenField ID="hdnUserID" runat="server" />                    
                    <asp:DropDownList ID="ddlAllUsers" Width="150px" runat="server" AppendDataBoundItems="true" OnSelectedIndexChanged="ReportToSelected" AutoPostBack="true">
                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button OnClick="Save" ID="btnSave" Visible="false" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    
</asp:Content>
