<%@ Page Title="" Language="C#" MasterPageFile="~/LumenApp.master" AutoEventWireup="true" CodeFile="POS_Items.aspx.cs" Inherits="Admin_POS_Items" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    <h2>POS Items</h2>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HtmlHead" runat="server">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />
  <script src="//code.jquery.com/jquery-1.10.2.js"></script>
  <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
  <link rel="stylesheet" href="/resources/demos/style.css" />
  <script language="javascript" type="text/javascript">
      $(function () {
          $("#<%= txtTranExpectedDate.ClientID %>").datepicker();
      });
      function ValidateTranPopup() {
          var flag = 0;
          var sErrorMsg = '';
          var txterrorMessage = document.getElementById('<%= lblError.ClientID %>');

          //Vendor Name
          var sVendName = document.getElementById('<%= txtTranVendor.ClientID %>');
          if (sVendName.value.length == 0) {
              sErrorMsg += '* Please Enter Vendor Name. <br/>';
              flag++;
          }

          // Expected Qty
          var sExpectedQty = document.getElementById('<%= txtTranExpectedQty.ClientID %>');
          if (sExpectedQty.value.length == 0) {
              sErrorMsg += '* Please Enter Expected Quantity. <br/>';
              flag++;
          }

          //Expected Date
          var sExpectedDate = document.getElementById('<%= txtTranExpectedDate.ClientID %>');
          if (sExpectedDate.value != 'MM/DD/YYYY') {
              if (isValidDate(sExpectedDate.value) == false) {
                  sErrorMsg += '* Please Enter valid date. <br/>';
                  flag++;
              }
          }
          txterrorMessage.innerHTML = sErrorMsg;
          if (flag > 0) {
              return false;
          }
          return true;
          //return confirm('You have setup the item with below attributes.' + SelectedProgram + '' + ItemControlinFlowProgram + '\n\nClick "Ok" to proceed or click "Cancel" to stay on the page and edit the item attributes. ');
      }

      // Validates that the input string is a valid date formatted as "mm/dd/yyyy"
      function isValidDate(dateString) {
          // First check for the pattern
          if (!/^\d{2}\/\d{2}\/\d{4}$/.test(dateString))
              return false;

          // Parse the date parts to integers
          var parts = dateString.split("/");
          var day = parseInt(parts[1], 10);
          var month = parseInt(parts[0], 10);
          var year = parseInt(parts[2], 10);

          // Check the ranges of month and year
          if (year < 1000 || year > 3000 || month == 0 || month > 12)
              return false;

          var monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

          // Adjust for leap years
          if (year % 400 == 0 || (year % 100 != 0 && year % 4 == 0))
              monthLength[1] = 29;

          // Check the range of the day
          return day > 0 && day <= monthLength[month - 1];
      }

      $(document).ready(function () {
          $("input.numericvalidation").keydown(function (event) {

              if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 16 || (event.keyCode >= 37 && event.keyCode <= 40)) {
              }
              else {
                  if (event.keyCode < 95) {
                      if (event.keyCode < 48 || event.keyCode > 57) {
                          event.preventDefault();
                      }
                  }
                  else {
                      if (event.keyCode < 96 || event.keyCode > 105) {
                          event.preventDefault();
                      }
                  }
              }
          });
      });

   </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" Runat="Server">


    <!-- ++++++++ Buttons for grid +++++++++ -->
    <div class="GridButtons" style="width:750px">
        <asp:Button ID="btnAddItem" runat="server" Text="Add" OnClientClick="runwait();" OnClick="Add"/>  
    </div>
    <div style="margin-bottom:10px;clear:both"></div>        
    <!-- +++++++++++++++++++++++++++++++++++ -->
        
    <!-- ########## THE GRID ######### -->
    <asp:GridView runat="server" ID="gridPOSItems" AutoGenerateColumns="false" 
    OnRowDataBound="gridPOSItems_RowDataBound" CssClass="mGrid" EmptyDataText="There are no records.">        
        <Columns>
            <asp:TemplateField HeaderText="Preview"  ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" >
                <ItemTemplate>                                                            
                    <asp:Image runat="server" ID="imgPreview" />
                    <br />
                    <asp:HyperLink Text="Change Image" runat="server" ID="lnkChangeImage" CssClass="lnkbutton" ></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Item_Name" HeaderText="Item Name" ItemStyle-Width="300px" HtmlEncode="false"/>            
            <asp:BoundField DataField="Item_No" HeaderText="Item No" ItemStyle-Width="100px" HtmlEncode="false" Visible="false"/>            
            <asp:TemplateField HeaderText="Item No" ItemStyle-VerticalAlign="middle" ItemStyle-Width="50px">
        		<ItemTemplate>
                    <asp:Literal ID="litItemno" runat="server"></asp:Literal><br /><br />
                    <asp:Literal ID="litItemId" runat="server" Visible="false"></asp:Literal>
		        	<asp:LinkButton ID="lnkItemTrans" Text="View Item<br/>Transactions" runat="server" OnClientClick="return runwaittran();" OnClick="ViewTranHist" Visible="false" ></asp:LinkButton>
        		</ItemTemplate>
        	</asp:TemplateField>
            <asp:BoundField DataField="Brand" HeaderText="Brand" ItemStyle-Width="100px"/>            
            <asp:BoundField DataField="Dimension" HeaderText="Dimension" ItemStyle-Width="100px"/>  
            <asp:BoundField DataField="Price" HeaderText="Price" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>                        
            <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px">
                <ItemTemplate>
                    <asp:ImageButton ID="ImageButton1" ImageUrl="~/Inc/Images/Others/Edit.gif" runat="server" Text="Edit" OnClick="Edit" />
                    <span style="display:none"><asp:TextBox ID="txtID" runat="server"></asp:TextBox></span>
                    <asp:HiddenField ID="hdnEstimatedPrice" runat="server" />
                    <asp:HiddenField ID="hdnActualPrice" runat="server" />
                    <asp:HiddenField ID="hdnBrandID" runat="server" />
                    <asp:HiddenField ID="hdnItemTypeID" runat="server" />
                    <asp:HiddenField ID="hdnImageFileName" runat="server" />
                    <asp:HiddenField ID="hdnDimension" runat="server" />
                    <asp:HiddenField ID="hdnUnitQty" runat="server" />
                    <asp:HiddenField ID="hdnDescription" runat="server" />
                    <asp:HiddenField ID="hdnLowLeveInv" runat="server" />
                    <asp:HiddenField ID="hdnMaxOrderQty" runat="server" />
                    <asp:HiddenField ID="hdnPreOrderItem" runat="server" />
                    <asp:HiddenField ID="hdnInvOrderItem" runat="server" />
                    <asp:HiddenField ID="hdnAppRequired" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>            
            <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                <ItemTemplate>
                <asp:ImageButton ID="ImageButton2" ImageUrl="~/Inc/Images/Others/Delete.gif" runat="server" Text="Delete" OnClick="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');"/>
                </ItemTemplate>
            </asp:TemplateField>            
        </Columns>
    </asp:GridView>
    <!-- ############################# -->

    <!-- ++++++++ Buttons for grid +++++++++ -->
    <div class="GridButtons" style="width:750px">
        <asp:Button ID="Button1" runat="server" Text="Add" OnClientClick="runwait();" OnClick="Add" CssClass="btnaddprograms"/>  
    </div>
    <div style="margin-bottom:10px;clear:both"></div>        
    <!-- +++++++++++++++++++++++++++++++++++ -->

    <!-- ************ POPUP *********************** -->
    <div class="AcePopup" id="editform" style="width:800px">
    <asp:TextBox CssClass="ID" Text="" runat="server" ID="popAddEditID"/>        
        <div style=" margin:0px -10px 0px -10px;">
            <h2 class="popupheader">Add/Edit POS Item</h2>
            <div style="position:absolute;top:5px;right:10px;">
               <b> <asp:LinkButton ID="LinkButton1" Text="X" runat="server" OnClick="Cancel"></asp:LinkButton></b>
            </div>
        </div>
        
        <div style="color:Maroon;">Fields marked with * are mandatory</div>
        <div style="color:Red;font-weight:bold;padding:10px;">            
            <asp:Literal runat="server" ID="litValidationError"></asp:Literal>            
        </div>                  

        <table style="margin-left:20px;" border="0">
            <tr>
                <td style="width:380px;">
                    <div>
                        <div class="FieldLabel">Item Name<b style="color:Red">*</b></div>
                        <div style="float:left; padding-left:10px;"><asp:TextBox ID="popItemName" runat="server" Width="170px" ></asp:TextBox></div>
                    </div> 
                </td>
                <td style="width:10px;">&nbsp;</td>
                <td style="width:380px;">
                    <div>
                        <div class="FieldLabel">Item Number:<b style="color:Red">*</b></div>
                        <div style="float:left; padding-left:10px;"><asp:TextBox ID="popItemNo" runat="server" Width="170px" ></asp:TextBox></div>
                    </div> 
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <div class="FieldLabel">Price ($):<b style="color:Red"></b></div>
                        <div style="float:left; padding-left:10px;"><asp:TextBox CssClass="DecimalField placeholder" placeholder="0.00" placeholdertext="0.00" ID="popEstimatedPrice" runat="server" Width="100px" ></asp:TextBox></div>
                    </div> 
                </td>
                <td></td>
                <td>
                    <div>
                        <div class="FieldLabel">Item Type:<b style="color:Red">*</b></div>
                        <div style="float:left; padding-left:10px;"><asp:DropDownList ID="popItemType" runat="server"  AppendDataBoundItems="true"></asp:DropDownList> </div>
                    </div> 
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <div class="FieldLabel">Dimension:<b style="color:Red"></b></div>
                        <div style="float:left; padding-left:10px;"><asp:TextBox ID="popDimension" runat="server" Width="100px" ></asp:TextBox></div>
                    </div> 
                </td>
                <td></td>
                <td>
                    <div>
                        <div class="FieldLabel">Pack Of:<b style="color:Red"></b></div>
                        <div style="float:left; padding-left:10px;"><asp:TextBox ID="popUnitQuantity" runat="server" Width="100px" ></asp:TextBox></div>
                    </div> 
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <div class="FieldLabel">Pre Order Item<b style="color:Red"></b></div>
                        <div class="FieldControl">
                            <asp:CheckBox id="popPreOrderItem" runat="server"
                                Text="" TextAlign="Right"/>
                        </div>
                    </div><br /><br />
                    <div>
                        <div class="FieldLabel">Inventory Item<b style="color:Red"></b></div>
                        <div class="FieldControl">
                            <asp:CheckBox id="popInvOrderItem" runat="server"
                                Text="" TextAlign="Right"  OnCheckedChanged="Invitem_Check" AutoPostBack="True"/>
                        </div>
                    </div>
                </td>
                <td></td>
                <td  runat="server" ID ="tdInventory" visible="true">
                    <div>
                        <div class="FieldLabel">Maximum Order Quantity:<b style="color:Red"></b></div>
                        <div style="float:left; padding-left:10px;"><asp:TextBox ID="popMaxOrdQty" runat="server" Width="100px" ></asp:TextBox></div>
                    </div><br /><br />
					<div>
                        <div class="FieldLabel">Low Level Inventory:<b style="color:Red"></b></div>
                        <div style="float:left; padding-left:10px;"><asp:TextBox ID="popLowLevelInv" runat="server" Width="100px" ></asp:TextBox></div>
                    </div><br /><br />
                    <div>
                        <div class="FieldLabel">Approval Required<b style="color:Red"></b></div>
                        <div class="FieldControl">
                            <asp:CheckBox id="popAppRequired" runat="server"
                                Text="" TextAlign="Right"/>
                        </div>
                    </div>					
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <div class="FieldLabel">Brand:<b style="color:Red">*</b></div>
                        <div style="float:left; padding-left:10px;"><asp:DropDownList ID="popBrands" runat="server" AppendDataBoundItems="true"></asp:DropDownList></div>
                    </div> 
                </td>
                <td></td>
                <td rowspan="2" valign="top">
                    <div style="margin-top:10px;margin-bottom:1px">Image: </div>                    
                    <asp:Image ID="popPreviewImage" runat="server" /><br />
                    <asp:HiddenField ID="popHdnPreviewImageFileName" runat="server" />                    
                    <div style="background:#CCC;padding:5px;color:darkred">
                        <div style="background:#777;color:white;padding:5px;margin-bottom:5px;font-weight:bold">Upload Image</div>
                        <asp:FileUpload runat="server" ID="popFileUploadPreviewImage" Visible="true"/><br />
                        <asp:Button CssClass="SmallButton" runat="server" ID="popUploadButton" Text="Preview" OnClick="UploadPreviewImage"/>                    
                    </div>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <div style="margin-top:10px;margin-bottom:10px">Description: </div>
                    <asp:TextBox TextMode="MultiLine" runat="server" Height="100px" Width="300px" ID="popDesc"></asp:TextBox>
                </td>
                <td></td>                
            </tr>
        </table>
        <div class="SmallGridButtons" style="width:300px">                           
            <asp:Button ID="Button3" Text="Cancel" OnClick="Cancel" runat="server"/>                    
            <asp:Button ID="Button2" Text="Save" OnClick="Save" runat="server"/>     
        </div>
         <div style="clear:both"></div>
    </div>
    <!-- ****************************************** -->


        <!-- ************ POPUP *********************** -->
    
    <div class="AcePopup" id="viewtran" style="width:800px">
    <asp:TextBox CssClass="TRANID" Text="" runat="server" ID="popTranID" />        

    <div style=" margin:0px -10px 0px -10px; ">
        <h2 class="popupheader">Add / View Item Transactions</h2
    <asp:TextBox ID="txtTranItemID" class="TRANID" ReadOnly="true" runat="server" Visible="false"></asp:TextBox>
    <asp:TextBox ID="txtTranStockNo" class="" ReadOnly="true" runat="server" Visible="false"></asp:TextBox>
    <asp:TextBox ID="txtTranStockName" class="" ReadOnly="true" runat="server" Visible="false"></asp:TextBox>
    
    </div>

    <div style="color:Red;font-weight:bold">
        <asp:Label ID="lblError" runat="server"></asp:Label>       
     </div>
    <table cellspacing="0" border="0" style="border-collapse:collapse;" id="Table1" class="mGrid">
        <tbody>
	        <tr>
		        <td align="center" style="width:100px;"> Vendor Name</td> <td align="left" style="width:300px;" colspan="5"> <asp:TextBox ID="txtTranVendor"  runat="server" Width="450px" style="margin-left:15px;" TabIndex="2" MaxLength="50"></asp:TextBox></td>
            </tr>
            <tr>
			    <td align="center" style="width:100px;"> Qty Expected</td> <td align="center" style="width:100px;"> <asp:TextBox ID="txtTranExpectedQty"  runat="server" Width="100px" TabIndex="2" CssClass="numericvalidation" MaxLength="5" ></asp:TextBox></td>
                <td align="center" style="width:100px;">Expected Date</td> <td align="center" style="width:100px;"> 
                <asp:textbox id="txtTranExpectedDate" width="100px" runat="server"></asp:textbox>(MM/DD/YYYY)</td>
                <td align="center" style="width:100px;">  <asp:Button ID="Button4" class="PopupSaveButton" runat="server" Text="Save" OnClientClick="return ValidateTranPopup();" OnClick="btnSaveVendTran_Click" TabIndex="13" /></td>
            </tr>
        </tbody>
    </table>
    <!-- ########## Item Vendor  Transaction Grid ######### -->
    <br />
    <h2 class="popupheader">Transactions History</h2>
        <asp:GridView runat="server" ID="gridVendTran" AutoGenerateColumns="false" 
        OnRowDataBound="gridVendTran_RowDataBound" CssClass="mGrid" EmptyDataText="There are no records.">
            <Columns>
                <asp:BoundField DataField="Vendor" HeaderText="Vendor" />  
                <asp:BoundField DataField="Qty_Expected" HeaderText="Qty Expected" />
                <asp:BoundField DataField="Expected_Date" HeaderText="Expected Date" />
            </Columns>
        </asp:GridView>
    <!-- ########################### -->

    <!-- ########## Warehoutse Transaction Grid ######### -->
    <br />
    <h2 class="popupheader">JLH Inventory</h2>
    <asp:GridView runat="server" ID="gridTranHist" AutoGenerateColumns="false" 
    OnRowDataBound="gridTranHist_RowDataBound" CssClass="mGrid" EmptyDataText="There are no records.">
        <Columns>
            <asp:BoundField DataField="StockDateTime" HeaderText="Date" />  
            <asp:BoundField DataField="Inventory" HeaderText="Inventory" />
            <asp:BoundField DataField="Variance" HeaderText="Variance" />
        </Columns>
    </asp:GridView>
    <!-- ########################### -->

    <div style ="float:right;margin-right:5px;margin-top:10px;">
        <asp:Button ID="Button5" runat="server" Text="Cancel" OnClick="btnVendTranCancel_Click" TabIndex="14" />
        <asp:HiddenField ID="HiddenField1" runat="server" />
    </div>
        <div style="clear:both"></div>
    </div>
    <!-- ****************************************** -->

    <!-- ########## Jquery Triggers ################## -->
    <div class="jquerytriggers" style="display:none">
        <asp:TextBox ID="failuremsg" CssClass="failuremsg" runat="server"></asp:TextBox>
        <asp:TextBox ID="successmsg" CssClass="successmsg" runat="server"></asp:TextBox>
   </div>
   <!-- ############################################# -->
</asp:Content>

