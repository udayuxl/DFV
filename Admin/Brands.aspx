<%@ Page Title="" Language="C#" MasterPageFile="~/LumenApp.master" AutoEventWireup="true" CodeFile="Brands.aspx.cs" Inherits="Admin_Brands" %>

<asp:Content ContentPlaceHolderID="Title" runat="server">
    <h2>Brands</h2>
</asp:Content>

<asp:Content ContentPlaceHolderID="HtmlHead" runat="server">
    <script language="javascript" type="text/javascript">        
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server">
    
    <!-- ++++++++ Buttons for grid +++++++++ -->
    <div class="GridButtons" style="width:400px">
        <asp:Button Text="Add" OnClick="Add" runat="server" OnClientClick="ShowAcePopup('EditForm')"/>        
    </div>
    <div style="margin-bottom:10px;clear:both"></div>        
    <!-- +++++++++++++++++++++++++++++++++++ -->

    <!-- ########## THE GRID ######### -->
    <asp:GridView runat="server" ID="gridBrands" AutoGenerateColumns="false" 
    OnRowDataBound="gridBrands_RowDataBound" CssClass="SmallGrid" EmptyDataText="There are no records.">
        <Columns>
            <asp:BoundField DataField="Brand" HeaderText="Brand" />            
            <asp:BoundField DataField="BrandManager" HeaderText="Marketing Manager" />
            <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="~/Inc/Images/Others/Edit.gif" runat="server" Text="Edit" OnClick="Edit" />
                    <span style="display:none"><asp:TextBox ID="txtID" runat="server"></asp:TextBox></span>
                </ItemTemplate>
            </asp:TemplateField>            
            <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px">
                <ItemTemplate>
                <asp:ImageButton ImageUrl="~/Inc/Images/Others/Delete.gif" runat="server" Text="Delete" OnClick="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');"/>
                </ItemTemplate>
            </asp:TemplateField>            
        </Columns>
    </asp:GridView>
    <!-- ########################### -->
    
    <!-- ++++++++ Buttons for grid +++++++++ -->    
    <div class="GridButtons" style="width:400px">
        <asp:Button ID="Button1" Text="Add" OnClick="Add" runat="server" />
    </div>
    <div style="clear:both"></div>
    <!-- +++++++++++++++++++++++++++++++++++ -->

    <!-- ****************************************** -->
    <div class="AcePopup" id="editform" style="width:400px">
        <asp:TextBox CssClass="ID" Text="" runat="server" ID="popAddEditID"/>        
        <div style=" margin:0px -10px 0px -10px;">
            <h2 class="popupheader">Add/Edit Brand</h2>
            <div style="position:absolute;top:5px;right:10px;">
               <b> <asp:LinkButton Text="X" runat="server" OnClick="Cancel"></asp:LinkButton></b>
            </div>
        </div>
        
        <div style="color:Maroon;">Fields marked with * are mandatory</div>
        <div style="color:Maroon;">Only marketing managers will appear in drop down</div><br />

        <div style="color:Red;font-weight:bold">            
            <asp:Literal runat="server" ID="litValidationError"></asp:Literal>            
        </div>                  

        <table>
            <tr>
                <td style="padding-left:20px;">
                    <div>
                        <div class="FieldLabel">Brand Name<b style="color:Red">*</b></div>
                        <div class="FieldControl"><asp:TextBox ID="popBrandname"  runat="server" Width="196px" ></asp:TextBox></div>
                    </div> 
                </td>
            </tr>
            <tr>
                <td style="padding-left:20px;">
                    <div>
                            <div class="FieldLabel">Marketing Manager<b style="color:Red">*</b></div>
                            <div class="FieldControl">
                                <asp:DropDownList runat="server" ID="popMMUsers" Width="202px" AppendDataBoundItems="true"></asp:DropDownList>
                            </div>
                    </div> 
                </td>
            </tr>
        </table>
        <br />
        <div class="SmallGridButtons" style="width:300px">                           
            <asp:Button ID="Button3" Text="Cancel" OnClick="Cancel" runat="server"/>                    
            <asp:Button ID="Button2" Text="Save" OnClick="Save" runat="server"/>     
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