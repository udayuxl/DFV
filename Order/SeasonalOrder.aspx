<%@ Page Title="" Language="C#" MasterPageFile="~/LumenNoLeft.master" AutoEventWireup="true" CodeFile="SeasonalOrder.aspx.cs" Inherits="Order_SeasonalOrder" %>

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
            var totBudget = document.getElementById("<%=hdnAllocatedBudget.ClientID %>").value;
            var prevRemainingBudget = document.getElementById("<%=hdnRemainingBudget.ClientID %>").value;
            var prevOrdVal = document.getElementById("<%=hdnOrderValue.ClientID %>").value;
            var currRemainingBudget = 0;
            currRemainingBudget = (parseFloat(prevRemainingBudget) + parseFloat(prevOrdVal)) - parseFloat(nGrandTotal);
            document.getElementById('<%=litRemainingBudget.ClientID %>').innerHTML = "$" + currRemainingBudget.toFixed(2);
        }
    </script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Title" Runat="Server">    
    Seasonal Order              
    <asp:Button ID="btnBack" Text="Back" OnClick="Back" runat="server" Visible="false"/>            
    <div id="divBudget" runat="server" class="budget" style="display:nonex;">Budget Remaining : 
        <b style=" color:#080">            
            <asp:Label ID="litRemainingBudget" runat="server"></asp:Label>
        </b> of 
        <b style="color:#555">
            <asp:Label ID="litAllocatedBudget" runat="server"></asp:Label>
        </b>
    </div>

    <div id="divOrderTotal" runat="server" class="budget">Order Total : 
        <b style=" color:#080">      
             <asp:Label ID="litOrderTotal" runat="server"></asp:Label>
        </b>
    </div>
    <asp:HiddenField ID="hdnAllocatedBudget" runat="server" />
	<asp:HiddenField ID="hdnRemainingBudget" runat="server" />
    <asp:HiddenField ID="hdnOrderValue" runat="server" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" Runat="Server">
    <asp:HiddenField ID="hdnOrderID" runat="server" />

    <table style="background:#DDD;width:960px;margin-bottom:10px;">
        <tr>    
            <td style="width:200px;">
            <div >
                <p id="BrandHeader" style="color:#cb7c17; font-family: Arial; font-size:16px; font-weight:bold; margin-right:10px; margin-top:5px; margin-left:5px;">Brands</p> 
                <div>
                    <div style="padding:5px;overflow-y:scroll;overflow-x:none;width:220;height: 120px;border: 1px solid #C0C0C0;float:left; "> 
			            <div id="Div3">
                            <div style="margin-left:1px;height:110px;width:180px;">
                                <asp:ListView ID="lvwSelectedBrands"  ItemPlaceholderID="PlaceHolderBrands" runat="server" OnItemDataBound="lvwSelectedBrands_ItemDataBound" > 
                                    <LayoutTemplate>
                                        <div id="PlaceHolderBrands" runat="server"></div>
                                    </LayoutTemplate>                                 
                                    <ItemTemplate>
                                        <table>
                                            <tr><td style="width:150px;"><asp:CheckBox ID="chkBrand" runat="server" /> </td></tr></table>
                                    </ItemTemplate>
                                        
                                </asp:ListView>                       
                            </div> 
                        </div>
                        </div>
                </div>

                </div>
            </td>        
            <td style="width:475px;">
                <span style="float:right;margin-right:5px; margin-top:75px;">
                    <asp:TextBox ID="txtSearch" runat="server" placeholder="Enter complete or partial Item Name or Item No or Brand..."  Width=450px/>
                </span><br />
                <span style="float:right;margin-top:15px;">
                    <asp:Button ID="btnSearch" runat="server" OnClick="SearchItems" Text="Search"/>
                </span>
            </td>
            <td style="padding:5px;">
                <asp:Button ID="btnConfirmUp" Text="Confirm Order" OnClick="Confirm" runat="server" style="margin-top:40px;" />
                <asp:Button ID="btnSaveUp" Text="Save" OnClick="Save" runat="server" style="margin-top:40px;"/>                  
            </td>
        </tr>
    </table>

    <div class="OrderItems">
        <asp:ListView ID="lvwSeasonalOrderItems" runat="server" ItemPlaceholderID="PlaceHolder1" onitemdatabound="lvwSeasonalOrderItems_ItemDataBound">        
            <ItemTemplate>
                <div class="Item">
                    <asp:HiddenField ID="hdnItemID" runat="server" />
                    <asp:HiddenField ID="hdnPrice" runat="server" />
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
                                <div style="display:nonex" class="ItemInfo">Item Type: <b><asp:Literal ID="litItemType" Text="" runat="server"/></b> </div>
                                <div class="ItemInfo">Price : <b><asp:Literal ID="litPrice" Text="" runat="server"/></b></div>
                                <div style="clear:both"></div>
                                <div class="ItemInfo">Pack Size: <b><asp:Literal ID="litPackSize" Text="" runat="server"/></b> </div>
                                <div class="ItemInfo">Dimension: <b><asp:Literal ID="litDimension" Text="" runat="server"/></b> </div>
                                <div style="clear:both"></div>
                                
                                <div class="ItemDescription">
                                    <asp:Literal ID="litDescription" Text="" runat="server"/>                                    
                                </div>                                
                                <div class="Notes" style="display:none">
                                    <asp:Literal ID="LitNotes" runat="server" Text="Notes:"></asp:Literal>
                                    <asp:TextBox runat="server" ID="txtNotes" TextMode="MultiLine" MaxLength="500" Width="350px" Height="50px" />
                                </div>
                            </td>
                            <td class="column3" valign="top">                                
                                <Lumen:CompositeCart ID="compositeCart" runat="server" />
                            </td>
                        </tr>                        
                    </table>                    
                </div>
            </ItemTemplate>
            <EmptyDataTemplate>
                <div style="text-align:center; font-size:larger; color:Maroon; font-size:30px;">No Items found!</div>
            </EmptyDataTemplate>
        </asp:ListView>
    </div>

    <table style="background:#DDD;width:960px;margin-bottom:10px;">
        <tr>
            
            <td style="padding:5px; display:none;">
                <b>Please specify Region, Acct #, Dept, and Brand for total being charged to your budget:</b>
                <br />
                <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Width=650px style="margin-top:5px"></asp:TextBox>
            </td>
            <td style="padding:5px;">
                <asp:Button ID="btnConfirm" Text="Confirm Order" OnClick="Confirm" runat="server" />
                <asp:Button ID="btnSave" Text="Save" OnClick="Save" runat="server" />                  
            </td>
        </tr>
    </table>

    <!-- ########## Jquery Triggers ################## -->
    <div class="jquerytriggers" style="display:none">
        <asp:TextBox ID="failuremsg" CssClass="failuremsg" runat="server"></asp:TextBox>
        <asp:TextBox ID="successmsg" CssClass="successmsg" runat="server"></asp:TextBox>
    </div>
    <!-- ############################################# -->
    
</asp:Content>

