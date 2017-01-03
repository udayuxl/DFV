<%@ Page Title="" Language="C#" MasterPageFile="~/LumenNoLeft.master" AutoEventWireup="true" CodeFile="AssignBudget.aspx.cs" Inherits="Admin_AssignBudget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleTag" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HtmlHead" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Title" Runat="Server">
    Allocate Budget - RVP Southeast
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" Runat="Server">        
    <div style="margin-left:20px;padding:5px; background:silver">Allocate budget to Sales Managers</div>
    <div style="width:500px;margin-left:auto;margin-right:auto;margin-top:20px;margin-bottom:50px">

        
        <div id="divYourBudget" runat="server">            
            <div class="FieldLabel">Available Budget<b style="color:Red"></b></div>
            <div class="FieldControl"> 
                <asp:TextBox runat="server" ID="txtYourBudget" CssClass="DecimalField" enabled="false" Width="80px" Text="$1200.00"> </asp:TextBox>                  
            </div>
            <div style="clear:both"></div>
        </div>
        <br />
        <asp:GridView runat="server" Width="600px" CssClass="SmallGrid" ID="gridDVPUsers" OnRowDataBound="gridDVPUsers_ItemDataBound" AutoGenerateColumns="false" 
        HeaderStyle-BackColor="Gray" HeaderStyle-ForeColor="White" HeaderStyle-Height="25px" RowStyle-BackColor="AntiqueWhite" ShowFooter="true">
            <Columns>
                <asp:BoundField DataField="Username" HeaderText="Sales Manager"/>                    

                <asp:BoundField DataField="Username" HeaderText="Role Description"/>  

                <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Existing Budget" ItemStyle-Width="80px">
                    <ItemTemplate>
                        <asp:Label ID="lblExistingBudget" runat="server" Text="$100.00"></asp:Label>
                    </ItemTemplate>                                        
                </asp:TemplateField>

                <asp:TemplateField HeaderText="New Allocation" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="txtAddBudget" style="margin-top:2px;margin-bottom:2px;width:80px" CssClass="DecimalField placeholder" runat="server"></asp:TextBox>
                        <asp:HiddenField ID="hdnUserID" runat="server" />
                        <div id="divMsg" runat="server"></div>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align:right;font-weight:bold"> Total</div>
                    </FooterTemplate>
                </asp:TemplateField>
                                        
                <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="New Budget" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Label ID="lblBudget" runat="server"></asp:Label>
                    </ItemTemplate>                    
                    <FooterTemplate>
                        <div style="text-align:right;font-weight:bold"> 
                            <asp:Label ID="lblBudgetTotal" runat="server"></asp:Label>
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>

                    
                <asp:TemplateField Visible="false" HeaderText="Budget 2Remove  Budget" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="txtRemoveBudget" style="margin-top:2px;margin-bottom:2px;width:80px" CssClass="DecimalField placeholder" runat="server"></asp:TextBox>                            
                        <div id="divMsg2" runat="server"></div>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphFooter" Runat="Server">
</asp:Content>

