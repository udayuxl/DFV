<%@ Page Title="" Language="C#" MasterPageFile="~/LumenNoLeft.master" AutoEventWireup="true" CodeFile="OrderApproval.aspx.cs" Inherits="Admin_OrderApproval" %>
    <asp:Content ID="Content2" ContentPlaceHolderID="Title" runat="server">
        <h2>Order Approval</h2>
    </asp:Content>
    <asp:Content ID="Content3" ContentPlaceHolderID="HtmlHead" runat="server">
        <script type="text/javascript">
            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode
                if (charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;
                return true;
            }

            function ValidateQuantity(rowid) {
                var row = parseInt(rowid);
                var grid = document.getElementById('<%=grdInventoryOrderItemApproval.ClientID%>');
                if (grid.rows[row + 1].cells[8].children[0].value == '') {
                    grid.rows[row + 1].cells[8].children[0].value = 0;
                }
                var iApprovedQuantity = parseInt(grid.rows[row + 1].cells[8].children[0].value);
                var iAvaliableQuantity = parseInt(grid.rows[row + 1].cells[9].children[0].innerHTML);

                if (iApprovedQuantity <= 0) {
                    alert('Approved item quantity is zero, order for this item will be rejected on clicking "Save".');
                }
                if (iApprovedQuantity > iAvaliableQuantity) {
                    //grid.rows[row + 1].cells[7].children[0].value = 0;
                    dropdowns = grid.getElementsByTagName('select');
                    if (dropdowns[rowid].selectedIndex != 1) {
                        dropdowns[rowid].selectedIndex = 0;
                        alert('Approved quantity (' + iApprovedQuantity + ') cannot exceed the Available Inventory (' + iAvaliableQuantity + ') .');
                    }
                }
            }

            function UpdateAvailableInventory(rowid) {
                var row = parseInt(rowid, 10);
                var grid = document.getElementById('<%=grdInventoryOrderItemApproval.ClientID%>');
                dropdowns = grid.getElementsByTagName('select');
                if (dropdowns[row].selectedIndex != 0) {
                    grid.rows[row + 1].cells[7].children[0].disabled = true;
                    grid.rows[row + 1].cells[9].children[0].disabled = true;
                    var itemid = parseInt(grid.rows[row + 1].cells[5].children[1].getAttribute("value"), 10);
                    var previousqty = parseInt(grid.rows[row + 1].cells[7].children[0].getAttribute("previousqty"), 10);
                    var newqty = parseInt(grid.rows[row + 1].cells[7].children[0].value, 10);
                    if (dropdowns[row].selectedIndex == 1) {
                        grid.rows[row + 1].cells[7].children[0].value = 0;
                        newqty = -(newqty - previousqty);
                    }
                    if (dropdowns[row].selectedIndex != 1) {
                        for (var i = 0; i < grid.rows.length - 1; i++) {
                            var currItemid = parseInt(grid.rows[i + 1].cells[5].children[1].getAttribute("value"), 10);
                            var currAvlQty = parseInt(grid.rows[i + 1].cells[8].children[0].innerHTML, 10);
                            if (itemid == currItemid) {
                                grid.rows[i + 1].cells[8].children[0].innerHTML = currAvlQty - (newqty);
                            }
                        }
                    }
                }
            }

            function toggleCheckBoxes(source) {
                var listView = document.getElementById('<%= grdInventoryOrderItemApproval.ClientID %>');
                for (var i = 0; i < listView.rows.length; i++) {
                    var inputs = listView.rows[i].getElementsByTagName('input');
                    for (var j = 0; j < inputs.length; j++) {
                        if (inputs[j].type == "checkbox") {
                            if (inputs[j].disabled != true) {
                                inputs[j].checked = source.checked;
                            }
                        }
                    }
                }
            }

            function ValidateSeletedOrdersForApprove() {
                return confirm('Selected orders will be approved and sent for processing. \n Do you want to proceed?');
            }
        </script>
    </asp:Content>

    <asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server">
    <style type="text/css">
        div.savediv {margin:20px 12px 0px 20px;}
        div.div_grdInventoryOrderItemApproval{margin-top:40px;}
        .mGrid tr th {text-align:center;} 
        .lblNoOrders{color:Red;font-weight:bold; }
        .selectallorderitems{margin-right: 10px;}
    </style>

    <div class="savediv">
     <div style="float:left;">
        <asp:CheckBox runat="server" ID="chkSelectAllOrderItems" Text="Check All" CssClass="selectallorderitems" onclick="toggleCheckBoxes(this);"/>
        <asp:Button runat="server" ID="btnApproveSeletedOrdersAll" Text="Approve Selected Orders" OnClientClick="return ValidateSeletedOrdersForApprove();" OnClick="SaveInventoryApproveSelectedOrdersAll" />
    </div>
    <div style="float:right;">
        <asp:Button ID="SaveButton" runat="server" Text="Save" onclick="SaveInventoryOrderItemApproval"  />
        <asp:Button ID="CancelButton" runat="server" Text="Cancel"  onclick="Cancel_click" />
    </div>
    </div>
    <div style="margin-top:20px;text-align:center;clear:both;">
        <asp:Label ID="lblNoOrders" runat="server" Visible="false" CssClass="lblNoOrders"/>
    </div>

   <div class = "div_grdInventoryOrderItemApproval">
    <asp:GridView ID="grdInventoryOrderItemApproval" runat="server"  
            AutoGenerateColumns="false"
            GridLines="None"
            AllowPaging="false"
            CssClass="mGrid"
            Width="948px"
            OnRowDataBound = "grdInventoryOrderItemApproval_RowDataBound">  
            <HeaderStyle HorizontalAlign ="Center" />
            <Columns>
                 <asp:TemplateField  HeaderText="Select" ItemStyle-HorizontalAlign="Center" >
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelect" Text="" runat="server"/>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Order Date" ItemStyle-Width="100">
                    <ItemTemplate>
                       <asp:Label ID="lblOrderDate" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Pending <br/>Since<br/>(Hours)" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                       <asp:Label ID="lblPendingHours" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Order <br/>Number" ItemStyle-Width="100">
                    <ItemTemplate>
                       <asp:Label ID="lblOrderNo" runat="server" />
                       <asp:HiddenField ID="hdnOrderNo" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Sales <br/>Manager <br/>Name" ItemStyle-Width="150">
                    <ItemTemplate>
                       <asp:Label ID="lblSalMgrName" runat="server" />
                       <asp:HiddenField ID="hdnSalMgrID" runat ="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Brand <br/>Name" ItemStyle-Width="100">
                    <ItemTemplate>
                       <asp:Label ID="lblBrandName" runat="server" />
                       <asp:HiddenField ID="hdnBrandID" runat="server" /> 
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField  HeaderText="Item <br/>Name" ItemStyle-Width="100">
                    <ItemTemplate>
                       <asp:Label ID="lblItemName" runat="server" />
                       <asp:HiddenField ID="hdnItemID" runat="server" /> 
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField  HeaderText="Distributor" ItemStyle-Width="100">
                    <ItemTemplate>
                       <asp:Label ID="lblDistributor" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField  HeaderText="Quantity" ItemStyle-Width="90">
                    <ItemTemplate>
                       <asp:TextBox ID="txtQty" runat="server" Width="45px" Onkeypress="return isNumberKey(event)" />
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Available <br/>Inventory" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                       <asp:Label ID="lblAvailableInventory" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Approved" ItemStyle-Width="80">
                    <ItemTemplate>
                       <asp:DropDownList ID="ddlApproved" runat="server"  Width="70px" >
                            <asp:ListItem Text="" Value="0"></asp:ListItem>
                            <asp:ListItem Text="No" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="2"></asp:ListItem>
                       </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>                
                <asp:TemplateField HeaderText="Comments" ItemStyle-Width="220">
                    <ItemTemplate>
                       <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="1" Width="145px"  style="font-size: 12px;"/>     
                       <asp:HiddenField ID="hdnInventoryOrderDestinationItem" runat="server" />                 
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
    </asp:GridView>
</div>
</asp:Content>

