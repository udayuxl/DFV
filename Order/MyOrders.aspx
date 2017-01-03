﻿<%@ Page Title="" Language="C#" MasterPageFile="~/LumenNoLeft.master" AutoEventWireup="true" CodeFile="MyOrders.aspx.cs" Inherits="Order_MyOrders" %>

<asp:Content ID="Content4" ContentPlaceHolderID="Title" Runat="Server">
    My Orders
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" Runat="Server">
    <div style="padding:10px">
        <asp:GridView ID="gridMyOrders" runat="server" AllowSorting="true" PageSize="20" AutoGenerateColumns="False" GridLines="None" AllowPaging="true" CssClass="mGrid"  OnRowDataBound="gridMyOrders_RowDataBound"          >
            <Columns>
                <asp:BoundField DataField="OrderNo" HeaderText = "Order No" ItemStyle-HorizontalAlign="Center"/>            
                <asp:TemplateField HeaderText = "Order Details" ItemStyle-Width="100px" Visible="false">
                    <ItemTemplate>
                        <asp:Button runat="server" OnClick="ViewOrderDetails" Text="View Order Details"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="OrderDate" HeaderText="Order Date" ItemStyle-Width="80px" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="OrderTotal" HeaderText="Order Total" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8px" DataFormatString="{0:C}"/>
                <asp:BoundField DataField="ProgramName" HeaderText="Program Name" ItemStyle-Width="300px"/>
                <asp:TemplateField HeaderText="Program Status">
                    <ItemTemplate><asp:Literal ID="litProgramStatus" runat="server"></asp:Literal> </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Status" HeaderText="Order Status" />
                <asp:TemplateField HeaderText = "Edit" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Button ID="btnEditOrder" runat="server" OnClick="EditOrder" Text="Edit Order"/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

