<%@ Page Title="" Language="C#" MasterPageFile="~/LumenNoLeft.master" AutoEventWireup="true" CodeFile="MyInventoryOrders.aspx.cs" Inherits="Order_MyInventoryOrders" %>

<asp:Content ID="Content4" ContentPlaceHolderID="Title" Runat="Server">
    <h2>My Orders</h2>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" Runat="Server">
    <div style="padding:10px">
        <asp:GridView ID="gridMyOrders" runat="server" AllowSorting="true" PageSize="20" AutoGenerateColumns="False" GridLines="None" AllowPaging="false" CssClass="mGrid"  OnRowDataBound="gridMyOrders_RowDataBound" >
            <Columns>
                <asp:BoundField DataField="OrderNo" HeaderText = "Order No" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="75px"/>            
                <asp:TemplateField HeaderText = "Order Details" ItemStyle-Width="100px" Visible="false">
                    <ItemTemplate>
                        <asp:Button runat="server" OnClick="ViewOrderDetails" Text="View Order Details"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="OrderDate" HeaderText="Order Date" ItemStyle-Width="150px" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="OrderTotal" HeaderText="Order Total" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="150px" DataFormatString="{0:C}" />                
                <asp:TemplateField HeaderText="Program Status" Visible=false>
                    <ItemTemplate><asp:Literal ID="litProgramStatus" runat="server"></asp:Literal> </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Status" HeaderText="Order Status" />

                <asp:TemplateField HeaderText="Tracking No"  ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" >
                    <ItemTemplate>
                        <asp:HyperLink ID="hypTrackingNo" runat="server" NavigateUrl='<%# Eval("TrackingNos", "https://www.fedex.com/fedextrack/html/index.html?tracknumbers={0}") %>'
                        Text='<%# Eval("TrackingNos") %>' Target="_blank" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText = "Edit" ItemStyle-Width="150px">
                    <ItemTemplate>
                        <asp:Button ID="btnEditOrder" runat="server" OnClick="EditOrder" Text="Edit Order"/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

