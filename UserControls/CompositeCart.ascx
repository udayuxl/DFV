<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CompositeCart.ascx.cs" Inherits="UserControls_CompositeCart" %>

<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" ChildrenAsTriggers="true" runat="server" EnableViewState="true">

<ContentTemplate>
<div class="CompositeCart">    
    <span style="display:none" class="itemprice" ID="spanItemPrice" runat="server"></span>
    <asp:HiddenField runat="server" ID="hdnItemId" />
    <asp:HiddenField runat="server" ID="hdnSeasonalOrderId" />
    <asp:HiddenField runat="server" ID="hdnOrderId" />
    <asp:HiddenField runat="server" ID="hdnProgramId" />
    <asp:HiddenField runat="server" ID="hdnItemNo" />
    <span style="color:Red"><asp:Label ID="lblError" CssClass="error" runat="server" Text=""/></span>     
    <asp:Label runat="server" ID="lblMsgs1" class="cartmsgs1" Text="" />
    <asp:Label runat="server" ID="lblMsgs2" class="cartmsgs2" Text="" />

    <table cellpadding="0" cellspacing="0" border="0">
        <tr>
            <th class="space">&nbsp;</th>              
            <th class="TDAddress" align=left>Distributor</th>
            <th class="TDQuantity">Quantity in Packs</th>            
        </tr>            
        <asp:ListView ID="lvwCompositeCart" OnItemDataBound="lvwCompositeCart_ItemDataBound" runat="server" Visible="true">
            <ItemTemplate>
                <tr runat="server" id="TRAddresses">
                    <td class="space">&nbsp;</td>                    
                    <td align=left class="TDAddress">
                        <asp:Literal ID="litDestinationCode" Text='<%# Eval("destination_name")%>' runat="server"></asp:Literal>
                        <asp:HiddenField ID="hdnAddressId" runat="server" Value='<%# Eval("pk_Address_id")%>' />
                    </td>
                    <td align=center class="TDQuantity">
                        <asp:TextBox CssClass="small TriggerCalculations IntegerField" ID="txtQuantity" runat="server" MaxLength="5" />
                        <asp:HiddenField runat="server" ID="hdnItemPrice" />
                    </td>               
                </tr>                 
            </ItemTemplate>
            <EmptyDataTemplate>
                <p align="center">Your address book does not have any active addresses. Click <a href="../Order/AddressBook.aspx">here</a> to add or activate addresses</p>
            </EmptyDataTemplate>
        </asp:ListView>  
    </table>

    <div class="summary" style="display:nonex">
        <table>
            <tr>
                <td nowrap>Quantity: <asp:Label class="small qtysubtotal" runat="server" ID="lblTotalQty" Text="<b>0</b>"/></td>
                <td align="right" nowrap=nowrap>
                    Sub Total: <asp:Label runat="server" ID="lblTotalPrice" class="subtotal" Text="<b>$ 0.00</b>" />
                    <span runat="server" ID="spanHiddenSubTotal" class="subtotalhidden" style="display:none;color:cyan">
                        0.00                
                    </span>
                    <asp:HiddenField ID="hdnSubTotal" runat="server" />
                </td>            
            </tr>
        </table>
       
    </div>
</div>
<script type = "text/javascript">
    var defaultText = "0";
    function WaterMark(txt, evt) 
    {

        if(txt.value.length == 0 && evt.type == "blur")
        {
            txt.value = defaultText;
        }
        if(txt.value == defaultText && evt.type == "focus") 
        {
            txt.value=""; 
        }
    }
    function trim(str)
    {
        return str.replace(/^\s+|\s+$/g, '');
    }

    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;

        return true;
    }
</script>
</ContentTemplate>
</asp:UpdatePanel>