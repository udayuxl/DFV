<%@ Page Title="" Language="C#" MasterPageFile="~/LumenNoLeft.master" AutoEventWireup="true" CodeFile="InventoryOrder.aspx.cs" Inherits="Order_InventoryOrder" %>


<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HtmlHead" Runat="Server">
    <script language="javascript" type="text/javascript">
        function CustomPageLoad() {
            $("input.IntegerField").focus(function () {
                if ($(this).val() == "0")
                    $(this).val("");
            });
            $("input.IntegerField").blur(function () {
                if ($(this).val() == "")
                    $(this).val("0");
            });
        }
    </script>

    <style type="text/css">
        div.header h2 input[type="button"], input[type="submit"] 
        {
            float:right;
            margin-top:-4px;
            margin-right:20px;
        }
    </style>

    <script language="javascript">
        function DoMath(theQtyTextBox, qtyChanged) {
            blnQtyChanged = qtyChanged;
            cartindex = $(theQtyTextBox).attr("cartindex");
            // Available Stock
            $('span.cartmsgs1[cartindex="' + cartindex + '"]').text("");
            var availStock = parseInt($(theQtyTextBox).attr("availbleStock"), 10);
            var currOrdQty = parseInt($(theQtyTextBox).val(), 10);
            var prevQty = parseInt($(theQtyTextBox).attr("previousqty"), 10); 
            if (currOrdQty > availStock) {
                $('span.cartmsgs1[cartindex="' + cartindex + '"]').html("<div style='color:Red;font-weight:bold;padding:10px;'>Ordered Quantity should be less than Available Stock.</div>");
                $(theQtyTextBox).val(prevQty);
                return false;
            }
            // MaxOrder Qty
            $('span.cartmsgs2[cartindex="' + cartindex + '"]').text("");
            var maxOrdQty = parseInt($(theQtyTextBox).attr("maxordqty"),10);
            if ((maxOrdQty > 0) && (currOrdQty > maxOrdQty))
                $('span.cartmsgs2[cartindex="' + cartindex + '"]').html("<div style='color:Red;font-weight:bold;padding:10px;'>Item requires approval as it exceeds Maximum Order Quanity.</div>");
             
            cartQuantity = 0;
            itemPrice = parseFloat($('span.itemprice[cartindex="' + cartindex + '"]').text());

            $('input[cartindex="' + cartindex + '"]').each(function () {
                qty = parseInt($(this).val()) || 0;
                cartQuantity += qty;
            });

            $('span.qtysubtotal[cartindex="' + cartindex + '"]').text(cartQuantity);
            subtotal = cartQuantity * itemPrice;

            $('span.subtotal[cartindex="' + cartindex + '"]').text("$ " + subtotal.toFixed(2));
            $('span.subtotalhidden[cartindex="' + cartindex + '"]').text(subtotal.toFixed(2));

            ////////////// 
            ProgramSubtotal = 0;
            $('span.subtotalhidden').each(function () {
                //Total all the subtotals and set in the span.budget
                ProgramSubtotal += parseFloat($(this).text());

            });

            // This is for Calculating Remaing Budget
            /*
            AvailableBudget = parseFloat($("span.budget").text()) - ProgramSubtotal;
            $("div.hints div").text("$ " + AvailableBudget.toFixed(2));

            if (AvailableBudget <= 0) {
            $("input#ctl00_cphMain_btnSave").css("background", "#aaaaaa");
            $("input#ctl00_cphMain_btnSave").attr("disabled", "true");
            }
            else {
            $("input#ctl00_cphMain_btnSave").css("background", "#93011B");
            $("input#ctl00_cphMain_btnSave").removeAttr("disabled");
            }
            */
        }
        var blnQtyChanged = false;
        function confirmAction() {
            if (blnQtyChanged) {
                var confirmed = confirm("You will lose unsaved changes. Do you wish to switch the view? ");
                if (!confirmed)
                    return false;
            }
        }

        function IsNumeric(strString)
        //  check for valid numeric strings
        {
            if (strString == null)
                return false;
            var strValidChars = "0123456789.";
            var strChar;
            var blnResult = true;

            if (strString.length == 0) return false;

            //  test strString consists of valid characters listed above
            for (i = 0; i < strString.length && blnResult == true; i++) {
                strChar = strString.charAt(i);
                if (strValidChars.indexOf(strChar) == -1) {
                    blnResult = false;
                }
            }
            return blnResult;
        }

        function CalculateTotal(objCurrentTextBox) {
            var nGrandTotal = 0;
            var nTDTotal = 0;
            var nQtyTotal = 0;
            var TDClass = "";
            var INPUTs = null;
            var INPUTPrice = "0";

            //get all the Carts
            blnQtyChanged = true;
            var TDs = document.getElementsByTagName("div");
            for (var i = 0; i < TDs.length; i++) {
                TDClass = TDs[i].getAttribute("class");

                if (TDClass == "CompositeCart") {
                    nTDTotal = 0;
                    nQtyTotal = 0;
                    INPUTs = TDs[i].getElementsByTagName("input");

                    for (var j = 0; j < INPUTs.length; j++) {
                        INPUTPrice = INPUTs[j].getAttribute("price");
                        if (IsNumeric(INPUTPrice)) {
                            if (IsNumeric(INPUTs[j].value)) {

                                nTDTotal += parseFloat(INPUTPrice) * parseFloat(INPUTs[j].value);
                                nQtyTotal += parseInt10(INPUTs[j].value);
                            }
                        }
                    }

                    SPANs = TDs[i].getElementsByTagName("span");
                    for (var j = 0; j < SPANs.length; j++) {
                        SPANClass = SPANs[j].getAttribute("class");
                        if (SPANClass == "subtotal") {
                            SPANs[j].innerHTML = "$" + nTDTotal.toFixed(2);
                        }
                        if (SPANClass == "small qtysubtotal") {
                            SPANs[j].innerHTML = nQtyTotal;
                        }
                    }
                    nGrandTotal += nTDTotal;
                }
            }
            document.getElementById('<%=litOrderTotal.ClientID %>').innerHTML = "$" + nGrandTotal.toFixed(2);

            // To Caluclate Remaining Budget
            var prevOrdVal = document.getElementById("<%=hdnOrderValue.ClientID %>").value;
            var currRemainingBudget = 0;
            currRemainingBudget = (parseFloat(prevRemainingBudget) + parseFloat(prevOrdVal)) - parseFloat(nGrandTotal);
        }

        function toggleCheckBoxes(source) {
            var listView = document.getElementById('<%= lvwSelectedBrand.FindControl("tbl_SelectedBrand").ClientID %>');
            for (var i = 0; i < listView.rows.length; i++) {
                var inputs = listView.rows[i].getElementsByTagName('input');
                for (var j = 0; j < inputs.length; j++) {
                    if (inputs[j].type == "checkbox")
                        inputs[j].checked = source.checked;
                }
            }
        }


        function ConfirmOrderClick() {
            //if (isLoginUserBrandManager == null) {
            //    isLoginUserBrandManager = ValidateLoginUserBrandManager();
            //}
            isLoginUserBrandManager = false;
            if (isLoginUserBrandManager == false) {
                var QtyBoxesTD = document.getElementsByTagName("TD");
                var ordreqapp = false;
                for (var i = 0; i < QtyBoxesTD.length; i++) {
                    TDClass = QtyBoxesTD[i].getAttribute("class");
                    if (TDClass == "TDQuantity") {
                        INPUTs = QtyBoxesTD[i].getElementsByTagName("input");
                        for (var j = 0; j < INPUTs.length; j++) {
                            if ((INPUTs[j].value > 0) && (INPUTs[j].getAttribute("approvReq") == "1")) {
                                ordreqapp = true;
                            }

                            if ((parseInt(INPUTs[j].value, 10) > 0) && (parseInt(INPUTs[j].getAttribute("maxordqty"), 10) > 0) && (parseInt(INPUTs[j].value, 10) > parseInt(INPUTs[j].getAttribute("maxordqty"), 10))) {
                                ordreqapp = true;
                            }
                        }
                    }
                }
                if (ordreqapp == true) {
                    var confirmed = confirm("Please note one or more items in your order requires approval.\n\n The entire order will be submitted to warehouse for shipping only after approval.\n\nIf you would like other items in the order to be shipped without delay, please create a separate order for items requiring approval.\n\nClick 'OK' to proceed or click 'Cancel' to make changes to your order.");
                    if (!confirmed) {
                        return false;
                    }
                }
                document.getElementById('<%= hiddenIsBrandManager.ClientID %>').value = "0";
            }
            else
                document.getElementById('<%= hiddenIsBrandManager.ClientID %>').value = "1";
           // SaveClick();
        }
    </script>
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Title" Runat="Server">
<asp:HiddenField ID="hiddenIsBrandManager" runat="server" Value="1" />
    <h2 style="width:875px">
        Inventory Order
        <div id="divOrderTotal" runat="server" class="budget">Order Total : 
			<b style=" color:#080">      
				<asp:Label ID="litOrderTotal" runat="server"></asp:Label>
			</b>
		</div>
		<asp:HiddenField ID="hdnOrderValue" runat="server" />
        <asp:Button ID="btnConfirm" Text="Confirm Order" OnClick="Confirm" runat="server" OnClientClick="return ConfirmOrderClick();" />
        <asp:Button ID="btnSave" Text="Save" OnClick="Save" runat="server" />        
        <asp:Button ID="btnBack" Text="Back" OnClick="Back" runat="server" Visible="false"/>        
    </h2>




    <div class="GridButtons" style="width:800px">
    	<p id="BrandHeader"  style="color:#620937;font-size:14px;font-weight:bold;margin-right:10px;margin-top:25px;">Select Brand</p> 
        <div>
            <div style="padding:5px;overflow-y:scroll;overflow-x:none;width:260px;height: 120px;border: 1px solid #C0C0C0;float:left; "> 
			    <div id="Div3">
                    <div style="margin-left:1px;height:110px;width:160px;">
                        <asp:ListView ID="lvwSelectedBrand"  ItemPlaceholderID="PlaceHolderbrands" runat="server" OnItemDataBound="lvwSelectedBrand_ItemDataBound" > 
                            <LayoutTemplate>
                                <table runat="server" id="tbl_SelectedBrand">
                                    <tr>
                                        <td style="margin-left:10px;">
                                            <asp:CheckBox ID="cbSelectAll" runat="server" onclick="toggleCheckBoxes(this);" Text="ALL" />
                                        </td>                                            
                                    </tr>
                                        <tr runat="server" id="PlaceHolderbrands">
                                    </tr>
                                </table>
                            </LayoutTemplate>                                 
                            <ItemTemplate>
                                <tr id="Tr1" runat="server" >                                                       
                                    <td align="left">                                                                
                                        <asp:CheckBox ID="chkBrandName" runat="server" />
                                    </td>       
                                </tr>                 
                            </ItemTemplate>                  
                        </asp:ListView>                       
                    </div> 
                </div>
             </div>
        </div>
        <br />
        <br />
        <br />
        <br />
        <br />
        <span style="float:right;margin-right:50px;">
            <asp:TextBox ID="txtSearch" runat="server" placeholder="Enter Stock Number / Item Name" Width=350px/>
            <asp:Button ID="btnSearch" runat="server" OnClick="SearchItems" Text="Search" />
        </span>
    </div>
    <br />
    <br />
    <br />
            <div class="selectedBrandSubmit" style=" margin-left: 0px; margin-top:0px; float:left;">             
        	    <asp:Button ID ="submitSelectedBrand" runat="server" Text= "Show All items in selected brands" OnClick= "btnSelectedBrand_Click" OnClientClick="return BrandChange_confirmAction()" style="border:1px solid gray !important;" />
            </div>  
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" Runat="Server">
    <asp:HiddenField ID="hdnOrderID" runat="server" />
    <div class="OrderItems">
        <asp:ListView ID="lvwInventoryOrderItems" runat="server" ItemPlaceholderID="PlaceHolder1" onitemdatabound="lvwInventoryOrderItems_ItemDataBound">        
            <ItemTemplate>
                <div class="Item">
                    <asp:HiddenField ID="hdnItemID" runat="server" />
                    <table border="0">
                        <tr>
                            <td class="column1" valign="top">
                                <asp:HyperLink ID="lnkProgramPreviewImage" CssClass="ProgramPreviewImage" Target="_blank" runat="server"/>
                            </td>
                            <td class="column2" valign="top">                        
                                <div class="ItemName">                                
                                    <asp:Literal ID="litItemName" Text="" runat="server" />
                                </div >                            
                                <div class="ItemInfo">Item Number : <b><asp:Literal ID="litItemNo" runat="server"/></b></div>
                                <div class="ItemInfo">Brand : <b><asp:Literal ID="litBrand" Text="" runat="server"/></b></div>
                                <div style="clear:both"></div>
                                <div style="display:none" class="ItemInfo">Item Type: <b><asp:Literal ID="litItemType" Text="" runat="server"/></b> </div>
                                <div class="ItemInfo">Price : <b><asp:Literal ID="litPrice" Text="" runat="server"/></b></div>
                                <div style="clear:both"></div>
                                <div class="ItemInfo">Pack Size: <b><asp:Literal ID="litPackSize" Text="" runat="server"/></b> </div>
                                <div class="ItemInfo">Dimension: <b><asp:Literal ID="litDimension" Text="" runat="server"/></b> </div>
                                <div class="ItemInfo">Inventory : <asp:Literal ID="litAvailableInventory" Text="" runat="server"/></div>
                                 <div class="ItemInfo">Max Order Quantity : <asp:Literal ID="litMaxOrdQty" Text="" runat="server"/></div>
                                <div style="clear:both"></div>
                                
                                <div class="ItemDescription">
                                    <asp:Literal ID="litDescription" Text="" runat="server"/>                                    
                                </div>                                
                                <div class="Notes" style="display:none">
                                    <asp:Literal ID="LitNotes" runat="server" Text="Notes:"></asp:Literal>
                                    <asp:TextBox runat="server" ID="txtNotes" TextMode="MultiLine" MaxLength="500" Width="350px" Height="50px" />
                                </div>
                                <asp:Label ID="ApprovalReqMsg" runat ="server" Visible="false" CssClass ="style_lblErrorMsg" style="color:Red;font-weight:bold;padding:10px;" Text="Orders for this item require approval." />
                            </td>
                            <td class="column3" valign="top">                                
                                <Lumen:CompositeCart ID="compositeCart" runat="server" />
                            </td>
                        </tr>                        
                    </table>                    
                </div>
            </ItemTemplate>
            <EmptyDataTemplate>
                <div>No Item found!</div>
            </EmptyDataTemplate>
        </asp:ListView>
    </div>
</asp:Content>

