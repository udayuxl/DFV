<%@ Page Title="" Language="C#" MasterPageFile="~/LumenNoLeft.master" AutoEventWireup="true" CodeFile="AllocateBudget.aspx.cs" Inherits="Admin_AllocateBudget" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HtmlHead" Runat="Server">
    <style>
        div.FieldLabel {margin-bottom:20px}
        div.FieldControl {width:100px}
        
        div.Grid {background:red;float:left}
        
        div.GridHeaderCell {background:gray;color:White;margin:1px;}
        div.GridCell {background:yellow;color:White;margin:1px;}
        div.FieldLabel { float: left; width: 175px; }
        
    </style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Title" Runat="Server">
    Allocate Budget
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" Runat="Server">
    <div style="width:600px;margin-left:auto;margin-right:auto;margin-top:20px;margin-bottom:50px">
        <div>
            <div class="FieldLabel">Select Pre Order Program<b style="color:Red"></b></div>
            <div class="FieldControl"><asp:DropDownList runat="server" ID="ddlProgram" AutoPostBack="true" OnSelectedIndexChanged="ProgramSelected"></asp:DropDownList></div>
        </div> 
        <asp:HiddenField ID="hdnProgramStarted" runat="server" Value="No" />

        <div style="clear:both;width:700px" ID="divAssignPrgBudget" visible="false" runat="server" >
            <div class="FieldLabel">Program Budget</div>
            <div class="FieldControl"> 
                <asp:TextBox runat="server" ID="txtProgramBudget" CssClass="DecimalField" enabled="false" Width="80px"> </asp:TextBox>                  
            </div>
            <asp:Button ID="btnProgramBudget" runat="server" Text= "Update Budget" OnClick="UpdatePrgBudget" Visible="false" />
            <div style="clear:both"></div>
        </div>

        <div style="clear:both;width:700px" ID="divAssignBudget" visible="false" runat="server" >
            <h3>
                Allocate Budget
            </h3>
            <div id="divYourBudget" runat="server">
                <div class="FieldLabel">Marketing Allocated Budget<b style="color:Red"></b></div>
                <div class="FieldControl"> 
                    <asp:TextBox runat="server" ID="txtYourBudget" CssClass="DecimalField" enabled="false" Width="80px"> </asp:TextBox>                  
                </div>
                <div style="clear:both"></div>
            </div>

            <asp:GridView runat="server" CssClass="mGrid" ID="gridRVPUsers" OnRowDataBound="gridRVPUsers_ItemDataBound" AutoGenerateColumns="false" 
                HeaderStyle-BackColor="Gray" HeaderStyle-ForeColor="White" HeaderStyle-Height="25px" RowStyle-BackColor="AntiqueWhite" ShowFooter="true">
                <Columns>
                    <asp:BoundField DataField="designation_name" HeaderText="Position"/>
                    <asp:TemplateField HeaderText="Sales Manager" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                           <asp:Label ID="lblSMID"  Text='<%# Eval("Username") %>' runat="server"></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div style="text-align:right;font-weight:bold"> Total</div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Existing Budget" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lblBudget" runat="server"></asp:Label>
                            <asp:HiddenField ID="hdnMktBud" runat="server" Value="0" />
                        </ItemTemplate>
                        <FooterTemplate>
                            <div style="text-align:right;font-weight:bold"> 
                                <asp:Label ID="lblBudgetTotal" runat="server"></asp:Label>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Add Budget" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:TextBox ID="txtAddBudget" style="margin-top:2px;margin-bottom:2px;width:80px" CssClass="DecimalField placeholder" runat="server"></asp:TextBox>
                            <asp:HiddenField ID="hdnBudRecID" runat="server" />
                            <asp:HiddenField ID="hdnLevel" runat="server" />
                            <asp:HiddenField ID="hdnLevel1" runat="server" />
                            <asp:HiddenField ID="hdnLevel2" runat="server" />
                            <asp:HiddenField ID="hdnLevel3" runat="server" />
                            <div id="divMsg" runat="server"></div>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div style="text-align:right;font-weight:bold"></div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Updated Total" ItemStyle-Width="100px"  Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblBudgetUpd" runat="server"></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div style="text-align:right;font-weight:bold"> 
                                <asp:Label ID="lblBudgetTotalUpd" runat="server"></asp:Label>
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
            
            <br />         

            <div class="SmallGrid" style="width:400px">
                <asp:Button ID="Button1" runat="server" Text= "Cancel" OnClick="Cancel" />
                <asp:Button ID="Button2" runat="server" Text= "Save" OnClick="Save" />
            </div>
        </div>
        <!-- ########## Jquery Triggers ################## -->
        <div class="jquerytriggers" style="display:none">
            <asp:TextBox ID="failuremsg" CssClass="failuremsg" runat="server"></asp:TextBox>
            <asp:TextBox ID="successmsg" CssClass="successmsg" runat="server"></asp:TextBox>
       </div>
       <!-- ############################################# -->
    </div>
</asp:Content>

