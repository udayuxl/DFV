<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="CreateOrders.aspx.cs" Inherits="CreateOrders" EnableEventValidation="false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
        <title>Create Orders</title>
        <style type="text/css">
            .style1
            {
                color: #FF0066;
                text-decoration: underline;
                font-weight: bold;
            }
        </style>
    </head>
    <body>
        <form id="form1" runat="server" method="post" action="SaveOrder.aspx" >
            <table align="center" bgcolor="#B7FFC9" cellpadding="4" cellspacing="2" style="border: 1px solid #999999; font-family: Century">
                <tr><td class="style1" colspan="3" style="text-align: center; background-color: #FEEEDE">Order Form</td></tr>
		        <tr>
			        <td width="100px">OrderName</td>
			        <td ></td>
			        <td width="200px"><asp:TextBox ID="OrderName" runat="server"></asp:TextBox></td>
		        </tr>

		        <tr>
			        <td width="100px">OrderedByFirstName</td>
			        <td ></td>
			        <td width="200px"><asp:TextBox ID="OrderedByFirstName" runat="server"></asp:TextBox></td>
		        </tr>

		        <tr>
			        <td width="100px">OrderedByLastName</td>
			        <td ></td>
			        <td width="200px"><asp:TextBox ID="OrderedByLastName" runat="server"></asp:TextBox></td>
		        </tr>

		        <tr>
			        <td width="100px">ShipToFirstName</td>
			        <td ></td>
			        <td width="200px"><asp:TextBox ID="ShipToFirstName" runat="server"></asp:TextBox></td>
		        </tr>

		        <tr>
			        <td width="100px">ShipToLastName</td>
			        <td ></td>
			        <td width="200px"><asp:TextBox ID="ShipToLastName" runat="server"></asp:TextBox></td>
		        </tr>

		        <tr>
			        <td width="100px">Address1</td>
			        <td ></td>
			        <td width="200px"><asp:TextBox ID="Address1" runat="server"></asp:TextBox></td>
		        </tr>

		        <tr>
			        <td width="100px">Address2</td>
			        <td ></td>
			        <td width="200px"><asp:TextBox ID="Address2" runat="server"></asp:TextBox></td>
		        </tr>

		        <tr>
			        <td width="100px">City</td>
			        <td ></td>
			        <td width="200px"><asp:TextBox ID="City" runat="server"></asp:TextBox></td>
		        </tr>

		        <tr>
			        <td width="100px">State</td>
			        <td ></td>
			        <td width="200px"><asp:TextBox ID="State" runat="server"></asp:TextBox></td>
		        </tr>

		        <tr>
			        <td width="100px">Zip</td>
			        <td ></td>
			        <td width="200px"><asp:TextBox ID="Zip" runat="server"></asp:TextBox></td>
		        </tr>

		        <tr>
			        <td width="100px">Phone</td>
			        <td ></td>
			        <td width="200px"><asp:TextBox ID="Phone" runat="server"></asp:TextBox></td>
		        </tr>

		        <tr>
			        <td width="100px">Email</td>
			        <td ></td>
			        <td width="200px"><asp:TextBox ID="Email" runat="server"></asp:TextBox></td>
		        </tr>

		        <tr>
			        <td width="100px">TransactionNo</td>
			        <td ></td>
			        <td width="200px"><asp:TextBox ID="TransactionNo" runat="server"></asp:TextBox></td>
		        </tr>

		        <tr>
			        <td width="100px">SequenceNo</td>
			        <td ></td>
			        <td width="200px"><asp:TextBox ID="SequenceNo" runat="server"></asp:TextBox></td>
		        </tr>

		        <tr>
			        <td width="100px">Status</td>
			        <td ></td>
			        <td width="200px"><asp:TextBox ID="Status" runat="server"></asp:TextBox></td>
		        </tr>

		        <tr>
			        <td width="100px">SubmittedOrderNo</td>
			        <td ></td>
			        <td width="200px"><asp:TextBox ID="SubmittedOrderNo" runat="server"></asp:TextBox></td>
		        </tr>

		        <tr>
			        <td width="100px">ProcessStatus</td>
			        <td ></td>
			        <td width="200px"><asp:TextBox ID="ProcessStatus" runat="server"></asp:TextBox></td>
		        </tr>

		        <tr>
			        <td width="100px">ShippingStatus</td>
			        <td ></td>
			        <td width="200px"><asp:TextBox ID="ShippingStatus" runat="server"></asp:TextBox></td>
		        </tr>

		        <tr>
			        <td width="100px">ShippingInfo</td>
			        <td ></td>
			        <td width="200px"><asp:TextBox ID="ShippingInfo" runat="server"></asp:TextBox></td>
		        </tr>

		        <tr>
			        <td width="100px">CustName</td>
			        <td ></td>
			        <td width="200px"><asp:TextBox ID="CustName" runat="server"></asp:TextBox></td>
		        </tr>

		        <tr>
			        <td width="100px">shipping_instruction</td>
			        <td ></td>
			        <td width="200px"><asp:TextBox ID="shipping_instruction" runat="server"></asp:TextBox></td>
		        </tr>

		        <tr>
			        <td width="100px">freight_billing_code</td>
			        <td ></td>
			        <td width="200px"><asp:TextBox ID="freight_billing_code" runat="server"></asp:TextBox></td>
		        </tr>

                <tr>
			        <td width="400px" Colspan="3">Item Details</td>
        		</tr>
		        <tr>
			        <td width="400px" Colspan="3">
				        <table> <tr>
					        <td width="100px">ItemNo</td>
					        <td width="200px"><input name="ItemNo" value="Itm1" /></td>
					        <td width="100px">Quantity</td>
					        <td width="200px"><input name="Quantity" value="1" /></td>
				        </tr></table>
			        </td>
		        </tr>
                <tr>
			        <td width="400px" Colspan="3">
				        <table> <tr>
					        <td width="100px">ItemNo</td>
					        <td width="200px"><input name="ItemNo" value="Itm2" /></td>
					        <td width="100px">Quantity</td>
					        <td width="200px"><input name="Quantity" value="2" /></td>
				        </tr></table>
			        </td>
		        </tr>

               <tr>
			        <td width="400px" Colspan="3">
				        <table> <tr>
					        <td width="100px">ItemNo</td>
					        <td width="200px"><input name="ItemNo" value="Itm3" /></td>
					        <td width="100px">Quantity</td>
					        <td width="200px"><input name="Quantity" value="3" /></td>
				        </tr></table>
			        </td>
		        </tr>

                <tr>
			        <td width="400px" Colspan="3">
				        <table> <tr>
					        <td width="100px">ItemNo</td>
					        <td width="200px"><input name="ItemNo" value="Itm4" /></td>
					        <td width="100px">Quantity</td>
					        <td width="200px"><input name="Quantity" value="4" /></td>
				        </tr></table>
			        </td>
		        </tr>
                <tr>
			        <td width="400px" Colspan="3">
				        <table> <tr>
					        <td width="100px">ItemNo</td>
					        <td width="200px"><input name="ItemNo" value="Itm5" /></td>
					        <td width="100px">Quantity</td>
					        <td width="200px"><input name="Quantity" value="5" /></td>
				        </tr></table>
			        </td>
		        </tr>

                <tr><td></td>
                    <td>

                    <asp:Button ID="Reset_Button" runat="server" Text="Reset" OnClientClick="this.form.reset();return false;" />
                    <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" Text="Save" />
                    </td>
                </tr>
            </table>
        </form>
    </body>
</html>