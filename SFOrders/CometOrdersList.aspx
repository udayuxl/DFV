<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CometOrdersList.aspx.cs" Inherits="CometOrdersList" Title="Comet Orders"   EnableEventValidation="false"%>
<%@ Register TagPrefix="obout" Namespace="Obout.Grid" Assembly="obout_Grid_NET" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Interface" Assembly="obout_Interface" %>
<%@ Register TagPrefix="obout" Namespace="Obout.SuperForm" Assembly="obout_SuperForm" %>
<%@ Register TagPrefix="owd" Namespace="OboutInc.Window" Assembly="obout_Window_NET" %>

<%@ import Namespace="System.Globalization" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script type="text/javascript">
        var maximizewidth, maximizeheight = 0;
        var searchTimeout = null;
        function FilterTextBox_KeyUp() {
            //if (LiveSearchCheckBox.checked()) {
                if (searchTimeout != null) {
                    window.clearTimeout(searchTimeout);
                }
                searchTimeout = window.setTimeout(performSearch, 500);
           // }
        }
        function performSearch() {
            var searchValue = FilterTextBox.value();
            if (searchValue == FilterTextBox.WatermarkText) {
                searchValue = '';
            }
            orderDestGrid.addFilterCriteria('OrderName', OboutGridFilterCriteria.Contains, searchValue);
            orderDestGrid.addFilterCriteria('Address1', OboutGridFilterCriteria.Contains, searchValue);
            orderDestGrid.addFilterCriteria('Address2', OboutGridFilterCriteria.Contains, searchValue);
            orderDestGrid.addFilterCriteria('City', OboutGridFilterCriteria.Contains, searchValue);
            orderDestGrid.addFilterCriteria('State', OboutGridFilterCriteria.Contains, searchValue);
            orderDestGrid.addFilterCriteria('Zip', OboutGridFilterCriteria.Contains, searchValue);

            orderDestGrid.executeFilter();
            searchTimeout = null;
            return false;
        }
    </script>
    <style type="text/css">		
        .search-container {position: relative; margin-bottom: 10px;}
        .search-text {position: absolute; top:0px; left:5px;line-height: 21px;}
        .search-field {position: absolute; top: 0px; left: 58px;}
        .search-check {position: absolute; top: 0px; left: 530px;line-height: 21px;}
        .search-button {position: absolute; left: 385px;}
        .reset-button {position: absolute; left: 0px; top:27px;}
        .lockusers-check {position: absolute; left: 540px;}
        .UserMsg {position: absolute; margin-top:9px; left: 425px;}
    </style>
</head>
<body>
 <form id="form1" runat="server">

<div runat="server" ID="SummaryContent">
    <div style='font-weight:bold;font-size:14px;color:black; text-align:center;'> Comet Orders</div>
</div>
<div runat="server" ID="MainContent">
    <style id="Style1" type="text/css">
        .styAdminlink{color: white;font-weight: bolder;text-decoration: none; padding: 4px 15px 6px 10px; border-right:1px solid white;background-color:#052986;}   
        .ob_gHContWG .ob_gH .ob_gC, .ob_gHContWG .ob_gH .ob_gCW 
        {
            background-repeat: repeat;
            /*height:40px;*/
        }
        div.ob_gCc2, div.ob_gCc2C, div.ob_gCc2R
        {
            padding-left: 2px !important;
            padding-right: 2px !important;
            margin-right: 0px !important;
        }
        .ob_gDGEB img { margin-top: -3px; }
        .super-form
        {
            margin: 12px;
        }
        .ob_fC table td
        {
            white-space: normal !important;
        }
        .command-row .ob_fRwF
        {
            padding-left: 50px !important;
        }
        .hide
        {
            display:none;
        }
        .ob_gFPT { display: none; }
        a.ob_gALF { font-size: 15px; }
        a.ob_gALF:hover { font-size: 17px; }
        
        div[id*='Segment_id'] .ob_iDdlICBC { height:10px !important; min-height:35px !important; }
        div[id*='pk_territory_BU_id'] .ob_iDdlICBC { height:10px !important; min-height:120px !important; }
    </style>

    <div>
        <asp:Label ID="lblServerError" Visible="false" runat="server"></asp:Label>        
        <asp:Panel ID="pnlNoAccount" runat="server" Visible="false">
        </asp:Panel>
    </div>
    <br />
    <div>
        <asp:Literal ID="litgridPlanEmpty" runat="server" Text="<br/><br/><span style='font-weight:bold;font-size:14px;color:maroon;'> No items found.</span>" Visible="false" />
    </div>
    <div class="search-container tdText">
        <div class="search-text"></div>
        <div class="search-field">
             <obout:OboutTextBox runat="server" ID="FilterTextBox" WatermarkText="Enter partial / full OrderName / Address / City …" Width="335">
                <ClientSideEvents OnKeyUp="FilterTextBox_KeyUp" />
            </obout:OboutTextBox> 
        </div>  
        <div class="search-button">&#160;&#160; <obout:OboutButton ID="SearchButton" runat="server" Text="Search" OnClientClick="return performSearch();" /></div>
        <br />
   </div>  
   <div>
       <span id="WindowPositionHelper"></span>
       <obout:Grid runat="server" ID="orderDestGrid" AutoGenerateColumns="false" PageSize="-1" CallbackMode="true"
            DataSourceID="sds1" FolderStyle="~/Inc/CSS/black_glass" AllowAddingRecords="false"
            AllowPageSizeSelection="false" AllowPaging ="false" AllowSorting ="false"
            AllowFiltering="true" FilterType="ProgrammaticOnly"
            OnRowDataBound="OrderDestGrid_RowDataBound" OnColumnsCreated="OrderDestGrid_ColumnsCreated" 
            OnUpdateCommand="UpdateOrderDestination"  OnInsertCommand="InsertOrderDestination"
            OnDeleteCommand="DeleteOrderDestination"  AllowMultiRecordDeleting="true"  Height="500" Width="1400"  >
            <ClientSideEvents OnBeforeClientEdit="orderDestGrid_ClientEdit" OnBeforeClientAdd="orderDestGrid_ClientAdd" ExposeSender="true" />
            <ScrollingSettings ScrollWidth="100%" ScrollHeight="100%" />
            <Columns>
            
				<obout:Column DataField="pk_OrderDestination_ID" HeaderText="ID"  Width="200" Visible="true" />
				<obout:Column DataField="OrderName" HeaderText="OrderName"  Width="100"/>
				<obout:Column DataField="OrderedByFirstName" HeaderText="OrderedByFirstName"  Width="75"/>
				<obout:Column DataField="OrderedByLastName" HeaderText="OrderedByLastName"  Width="75"/>
				<obout:Column DataField="ShipToFirstName" HeaderText="ShipToFirstName"  Width="75"/>
				<obout:Column DataField="ShipToLastName" HeaderText="ShipToLastName"  Width="75"/>
				<obout:Column DataField="Address1" HeaderText="Address1"  Width="100"/>
				<obout:Column DataField="Address2" HeaderText="Address2"  Width="100"/>
				<obout:Column DataField="City" HeaderText="City"  Width="75"/>
				<obout:Column DataField="State" HeaderText="State"  Width="50"/>
				<obout:Column DataField="Zip" HeaderText="Zip"  Width="50"/>
				<obout:Column DataField="Phone" HeaderText="Phone"  Width="75" Visible="false" />
				<obout:Column DataField="Email" HeaderText="Email"  Width="75" Visible="false" />
				<obout:Column DataField="TransactionNo" HeaderText="TransactionNo"  Width="50"/>
				<obout:Column DataField="SequenceNo" HeaderText="SequenceNo"  Width="50"/>
				<obout:Column DataField="Status" HeaderText="Status"  Width="75"/>
				<obout:Column DataField="SubmittedOrderNo" HeaderText="SubmittedOrderNo"  Width="50" Visible="false" />
				<obout:Column DataField="ProcessStatus" HeaderText="ProcessStatus"  Width="75"/>
				<obout:Column DataField="ShippingStatus" HeaderText="ShippingStatus"  Width="100"/>
				<obout:Column DataField="ShippingInfo" HeaderText="ShippingInfo"  Width="75" Visible="false" />
				<obout:Column DataField="CustName" HeaderText="CustName"  Width="75"/>
				<obout:Column DataField="shipping_instruction" HeaderText="shipping_instruction"  Width="75"/>
				<obout:Column DataField="freight_billing_code" HeaderText="freight_billing_code"  Width="50" Visible="false" />
                <obout:Column ID="ColumnEdit" HeaderText="Action" Width="110" AllowEdit="true" AllowDelete="true" runat="server" />
                <obout:Column ID="Column1" DataField="pk_OrderDestination_ID" Visible="false" ReadOnly="true" HeaderText="pk_OrderDestination_ID" runat="server" />
                <obout:Column ID="Column2" DataField="CustName" Visible="false" ReadOnly="true" HeaderText="CustName" runat="server"/>
            </Columns>
            <MasterDetailSettings LoadingMode="OnCallback" ShowEmptyDetails = "true" />
            <DetailGrids>
                <obout:DetailGrid runat="server" ID="orderItemsGrid" AutoGenerateColumns="false"  CallbackMode="true"
                    AllowAddingRecords="false" ShowFooter="true" PageSize="-1"
                    AllowPageSizeSelection="false" AllowPaging ="false" AllowSorting ="false"
                    DataSourceID="sds2" FolderStyle="~/Inc/CSS/black_glass" ForeignKeys="pk_OrderDestination_ID" 
                    OnUpdateCommand="UpdateOrderItems"  OnInsertCommand="InsertOrderItems" OnDeleteCommand="DeleteOrderItems" 
                    AllowMultiRecordDeleting="true" >
                    <ClientSideEvents ExposeSender="true"
                        OnBeforeClientEdit="orderItemsGrid_ClientEdit" OnBeforeClientAdd="orderItemsGrid_ClientAdd" />
                    <Columns>
				        <obout:Column DataField="pk_OrderItem_ID" HeaderText="pk_OrderItem_ID"  Width="200"/>
        				<obout:Column DataField="pk_OrderDestination_ID" HeaderText="pk_OrderDestination_ID"  Width="200"/>
        				<obout:Column DataField="ItemNo" HeaderText="ItemNo"  Width="200"/>
        				<obout:Column DataField="Quantity" HeaderText="Quantity"  Width="200"/>
                        <obout:Column ID="Column3" HeaderText="Action" Width="110" AllowEdit="true" AllowDelete="true" runat="server" />
                    </Columns>
                    <MasterDetailSettings LoadingMode="OnCallback" />
                    <ScrollingSettings ScrollHeight="200" />
                </obout:DetailGrid>
            </DetailGrids>
            <FilteringSettings MatchingType="AnyFilter" />
            <ScrollingSettings ScrollWidth="1025" ScrollHeight="100%" />
            <Templates>
                <obout:GridTemplate runat="server" ID="EmptyTemplate">
		    	    <Template>
                    </Template>
                </obout:GridTemplate>
            </Templates>
        </obout:Grid>

        <asp:SqlDataSource runat="server" ID="sds1"  
		     ConnectionString="<%$ ConnectionStrings:ComplemarBridge %>" 
             SelectCommand="SP_getCometOrderDetails" SelectCommandType="StoredProcedure" >
                <SelectParameters>
                    <asp:QueryStringParameter Name="Customer" QueryStringField="Customer"  DefaultValue="All" />
                </SelectParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource runat="server" ID="sds2" 
            ConnectionString="<%$ ConnectionStrings:ComplemarBridge %>"
            SelectCommand="SP_getCometOrderItemDetails" SelectCommandType="StoredProcedure" >
		        <SelectParameters>
                    <asp:Parameter Name="pk_OrderDestination_ID" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
        <owd:Window ID="OrderDestWindow" runat="server" IsModal="true" ShowCloseButton="true" Status=""
            RelativeElementID="WindowPositionHelper" Top="-125" Left="500" Height="470" Width="551" VisibleOnLoad="false" StyleFolder="~/Inc/CSS/styles/wdstyles/blue"
            Title="Add / Edit Record" Overflow="AUTO">
            <input type="hidden" id="pk_OrderDestination_ID" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Group1" />
            <div class="super-form">
                <obout:SuperForm ID="OrderDestForm" runat="server"
                    AutoGenerateRows="false"
                    AutoGenerateInsertButton ="false"
                    AutoGenerateEditButton="false"
                    AutoGenerateDeleteButton="false"  
                    DataKeyNames="pk_OrderDestination_ID" DefaultMode="Insert" Width="525"
                    OnDataBound="OrderDestForm_DataBound"
                    ValidationGroup="Group1"  
                    >
                    <Fields>
                    <%-- 
                        <obout:BoundField DataField="pk_OrderDestination_ID" HeaderText="pk_OrderDestination_ID" FieldSetID="FieldSet1" MaxLength="50"/>
				        <obout:BoundField DataField="OrderName" HeaderText="OrderName" FieldSetID="FieldSet1" MaxLength="50" />
				        <obout:BoundField DataField="OrderedByFirstName" HeaderText="OrderedByFirstName" FieldSetID="FieldSet1" MaxLength="50"/>
				        <obout:BoundField DataField="OrderedByLastName" HeaderText="OrderedByLastName" FieldSetID="FieldSet1" MaxLength="50" />
				        <obout:BoundField DataField="ShipToFirstName" HeaderText="ShipToFirstName" FieldSetID="FieldSet1" MaxLength="50" />
                        <obout:BoundField DataField="ShipToLastName" HeaderText="ShipToLastName" FieldSetID="FieldSet1" MaxLength="50" />--%>
				        <obout:BoundField DataField="Address1" HeaderText="Address1" FieldSetID="FieldSet1" MaxLength="200" />
                        <%--
				        <obout:BoundField DataField="Address2" HeaderText="Address2" FieldSetID="FieldSet1" MaxLength="200" />--%>
				        <obout:BoundField DataField="City" HeaderText="City" FieldSetID="FieldSet1" MaxLength="50"  />
				        <obout:BoundField DataField="State" HeaderText="State" FieldSetID="FieldSet1" MaxLength="50" />
				       <obout:BoundField DataField="Zip" HeaderText="Zip" FieldSetID="FieldSet1" MaxLength="50" />
				         <%--<obout:BoundField DataField="Phone" HeaderText="Phone" FieldSetID="FieldSet1" MaxLength="50" />
                        
				        <obout:BoundField DataField="Email" HeaderText="Email" FieldSetID="FieldSet1" MaxLength="50" /> --%>
				        <obout:BoundField DataField="TransactionNo" HeaderText="TransactionNo" FieldSetID="FieldSet1" MaxLength="50"/>
                        <%--
				        <obout:BoundField DataField="SequenceNo" HeaderText="SequenceNo" FieldSetID="FieldSet1" MaxLength="50" />--%>
				        <obout:BoundField DataField="Status" HeaderText="Status" FieldSetID="FieldSet1" MaxLength="50" />
                        <%--
				        <obout:BoundField DataField="SubmittedOrderNo" HeaderText="SubmittedOrderNo" FieldSetID="FieldSet1" MaxLength="50" />--%>
				        <obout:BoundField DataField="ProcessStatus" HeaderText="ProcessStatus" FieldSetID="FieldSet1" MaxLength="50"  />
                        <%--
				        <obout:BoundField DataField="ShippingStatus" HeaderText="ShippingStatus" FieldSetID="FieldSet1" MaxLength="50"  />
				        <obout:BoundField DataField="ShippingInfo" HeaderText="ShippingInfo" FieldSetID="FieldSet1" MaxLength="50"  />--%>
				        <obout:BoundField DataField="CustName" HeaderText="CustName" FieldSetID="FieldSet1" MaxLength="50" /> 
				        <obout:BoundField DataField="shipping_instruction" HeaderText="shipping_instruction" FieldSetID="FieldSet1" MaxLength="50"/>
				        <%--
                        <obout:BoundField DataField="freight_billing_code" HeaderText="freight_billing_code" FieldSetID="FieldSet1" MaxLength="50"/>--%>
                        <obout:TemplateField FieldSetID="FieldSet3">
                            <EditItemTemplate>
                                <obout:OboutButton ID="OboutButton1" runat="server" Text="Save" OnClientClick="saveOrderDestinationChanges();" Width="75" />
                                <obout:OboutButton ID="OboutButton2" runat="server" Text="Cancel" OnClientClick="CanceOrderDestinationChanges(); return false;" Width="75" />
                            </EditItemTemplate>
                        </obout:TemplateField>
                    </Fields>
                    <FieldSets>
                        <obout:FieldSetRow>
                            <obout:FieldSet ID="FieldSet1" Title="OrderDestination Information" />
                        </obout:FieldSetRow>
                        <obout:FieldSetRow>
                            <obout:FieldSet ID="FieldSet2" Title="Hide"  CssClass="hide"/>
                        </obout:FieldSetRow>
                        <obout:FieldSetRow>
                            <obout:FieldSet ID="FieldSet3" ColumnSpan="2" CssClass="command-row" />
                        </obout:FieldSetRow>
                    </FieldSets>
                </obout:SuperForm>
            </div>
        </owd:Window>
        <owd:Window ID="OrderItemsWindow" runat="server" IsModal="true" ShowCloseButton="true" Status=""
            RelativeElementID="WindowPositionHelper" Top="-125" Left="500" Height="425" Width="551" VisibleOnLoad="false" StyleFolder="~/Inc/CSS/styles/wdstyles/blue"
            Title="Add / Edit Record" Overflow="AUTO">
            <input type="hidden" id="pk_OrderItem_ID" />  
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Group2" />
            <div class="super-form">
                <obout:SuperForm ID="OrderItemsSuperForm" runat="server"
                    AutoGenerateRows="false"
                    AutoGenerateInsertButton ="false"
                    AutoGenerateEditButton="false"
                    AutoGenerateDeleteButton="false"  
                    DataKeyNames="pk_OrderItem_ID" DefaultMode="Insert" Width="525"
                    ValidationGroup="Group2"  
                    >
                    <Fields>
                        <obout:BoundField DataField="ItemNo" HeaderText="ItemNo" FieldSetID="FieldSet4" MaxLength="50">
                            <Validators>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Display="Dynamic" ErrorMessage="Address is mandatory" Text="*" ValidationGroup="Group2" />
                            </Validators>
                        </obout:BoundField>
                        <obout:BoundField DataField="Quantity" HeaderText="Quantity" FieldSetID="FieldSet4"  MaxLength="50">
                            <Validators>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" Display="Dynamic" ErrorMessage="City is mandatory" Text="*" ValidationGroup="Group2"/>
                            </Validators>
                        </obout:BoundField>
                        <obout:BoundField DataField="pk_OrderDestination_ID" HeaderText="pk_OrderDestination_ID" FieldSetID="FieldSet5" />
                        <obout:TemplateField FieldSetID="FieldSet6">
                            <EditItemTemplate>
                                <obout:OboutButton ID="OboutButton3" runat="server" Text="Save" OnClientClick="if(Page_ClientValidate('Group2') == true){saveOrderItemsChanges();} return false;" Width="75" />
                                <obout:OboutButton ID="OboutButton4" runat="server" Text="Cancel" OnClientClick="CancelOrderItemsChanges(); return false;" Width="75" />
                            </EditItemTemplate>
                        </obout:TemplateField>
                    </Fields>
                    <FieldSets>
                        <obout:FieldSetRow>
                            <obout:FieldSet ID="FieldSet4" Title="Order Item Details" />
                        </obout:FieldSetRow>
                        <obout:FieldSetRow>
                            <obout:FieldSet ID="FieldSet5" Title="Hide"  CssClass="hide"/>
                        </obout:FieldSetRow>
                        <obout:FieldSetRow>
                            <obout:FieldSet ID="FieldSet6" ColumnSpan="2" CssClass="command-row" />
                        </obout:FieldSetRow>
                    </FieldSets>
                </obout:SuperForm>
            </div>
        </owd:Window>
    </div>
    <script type="text/javascript">
        var detailInAddEditMode = false;
        function orderDestGrid_ClientEdit(sender, record) {
            OrderDestWindow.Open();
            OrderDestWindow.screenCenter();
            document.getElementById('pk_OrderDestination_ID').value = record.pk_OrderDestination_ID;
            OrderDestForm_Address1.value(record.Address1);
            OrderDestForm_City.value(record.City);
            OrderDestForm_State.value(record.State);
            OrderDestForm_Zip.value(record.Zip);
            OrderDestForm_TransactionNo.value(record.TransactionNo);
            OrderDestForm_Status.value(record.Status);
            OrderDestForm_ProcessStatus.value(record.ProcessStatus);
            OrderDestForm_CustName.value(record.CustName);
            OrderDestForm_shipping_instruction.value(record.shipping_instruction);
            return false;
        }

        function orderDestGrid_ClientAdd(sender, record) {
            OrderDestWindow.Open();
            OrderDestWindow.screenCenter();


            document.getElementById('pk_OrderDestination_ID').value = 0;
            OrderDestForm_Address1.value('');
            OrderDestForm_City.value('');
            OrderDestForm_State.value('');
            OrderDestForm_Zip.value('');
            OrderDestForm_TransactionNo.value('');
            OrderDestForm_Status.value('');
            OrderDestForm_ProcessStatus.value('');
            OrderDestForm_CustName.value('');
            OrderDestForm_shipping_instruction.value('');
            return false;
        }

        function updateValidations() {
            var validatorIds = "";
            
            if (OrderDestForm_Segment_id.value() == "7")
                for (var i = 0; i < validatorIds.length; i++)
                    ValidatorEnable(document.getElementById(validatorIds[i]), false);
            else
                for (var i = 0; i < validatorIds.length; i++)
                    ValidatorEnable(document.getElementById(validatorIds[i]), true);
                    
        }

        function saveOrderDestinationChanges() {
            OrderDestWindow.Close();
            var pk_OrderDestination_ID = document.getElementById('pk_OrderDestination_ID').value;
            var data = new Object();
            data.Address1 = OrderDestForm_Address1.value();
            data.City = OrderDestForm_City.value();
            data.State = OrderDestForm_State.value();
            data.Zip = OrderDestForm_Zip.value();
            data.TransactionNo = OrderDestForm_TransactionNo.value();
            data.Status = OrderDestForm_Status.value();
            data.ProcessStatus = OrderDestForm_ProcessStatus.value();
            data.CustName = OrderDestForm_CustName.value();
            data.shipping_instruction = OrderDestForm_shipping_instruction.value();
            if (parseInt(pk_OrderDestination_ID, 10) != 0) {
                data.pk_OrderDestination_ID = pk_OrderDestination_ID;
                orderDestGrid.executeUpdate(data);
            } else {
                orderDestGrid.executeInsert(data);
            }
            orderDestGrid.refresh();
        }

        function CanceOrderDestinationChanges() {
            OrderDestWindow.Close();
        }

        function validate() {
            return Page_ClientValidate();
        }

        function Grid1_BeforeUpdate(record) {
            return Page_ClientValidate();
        }

        function Grid1_BeforeInsert(record) {
            return Page_ClientValidate();
        }


        function orderItemsGrid_ClientEdit(sender, record) {
            detailInAddEditMode = sender;
            OrderItemsWindow.Open();
            OrderItemsWindow.screenCenter();
            document.getElementById('<%=RequiredFieldValidator6.ClientID%>').style.display = "none";
            document.getElementById('<%=RequiredFieldValidator7.ClientID%>').style.display = "none";
            document.getElementById('<%=ValidationSummary2.ClientID%>').style.display = "none";

            document.getElementById('pk_OrderItem_ID').value = record.pk_OrderItem_ID;
            OrderItemsSuperForm_ItemNo.value(record.ItemNo);
            OrderItemsSuperForm_Quantity.value(record.Quantity);
            OrderItemsSuperForm_pk_OrderDestination_ID.value(record.pk_OrderDestination_ID);
            return false;
        }
        function orderItemsGrid_ClientAdd(sender, record) {
            detailInAddEditMode = sender;
            OrderItemsWindow.Open();
            OrderItemsWindow.screenCenter();
            document.getElementById('<%=RequiredFieldValidator6.ClientID%>').style.display = "none";
            document.getElementById('<%=RequiredFieldValidator7.ClientID%>').style.display = "none";
            document.getElementById('<%=ValidationSummary2.ClientID%>').style.display = "none";

            document.getElementById('pk_OrderItem_ID').value = 0;

            OrderItemsSuperForm_ItemNo.value('');
            OrderItemsSuperForm_Quantity.value('');
            OrderItemsSuperForm_pk_OrderDestination_ID.value(detailInAddEditMode.ForeignKeys.pk_OrderDestination_ID.Value);
            return false;
        }

        function saveOrderItemsChanges() {
            OrderItemsWindow.Close();
            var pk_OrderItem_ID = document.getElementById('pk_OrderItem_ID').value;
            var data = new Object();
            data.ItemNo = OrderItemsSuperForm_ItemNo.value();
            data.Quantity = OrderItemsSuperForm_Quantity.value();
            if (parseInt(pk_OrderItem_ID, 10) != 0) {
                data.pk_OrderItem_ID = pk_OrderItem_ID;
                data.pk_OrderDestination_ID = detailInAddEditMode.ForeignKeys.pk_OrderDestination_ID.Value;
                detailInAddEditMode.executeUpdate(data);
            } else {
                data.pk_OrderItem_ID = 0;
                data.pk_OrderDestination_ID = detailInAddEditMode.ForeignKeys.pk_OrderDestination_ID.Value;
                detailInAddEditMode.executeInsert(data);
            }
        }
        function CancelOrderItemsChanges() {
            OrderItemsWindow.Close();
        }
        function AdjustgridWidth() {
            if (typeof (orderDestGrid) != "undefined") {
                screenwidth = $(window).width();
                screenheight = $(window).height();
                gridwidth = screenwidth - 259;
                gridwidth = screenwidth - 10;
                if (gridwidth > 1440)
                    gridwidth = 1440;
                gridheight = screenheight - 540;

                orderDestGrid.GridMainContainer.style.width = gridwidth + 'px';

                gridheight = Math.min(gridheight, orderDestGrid.GridMainContainer.clientHeight)
                if (gridheight < 400)
                    gridheight = 400;
                orderDestGrid.GridMainContainer.style.height = gridheight + 'px';
            }
        }
        
    </script>
</div>
    </form>    
</body>
</html>
