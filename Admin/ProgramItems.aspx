<%@ Page Title="" Language="C#" MasterPageFile="~/LumenNoLeft.master" AutoEventWireup="true" CodeFile="ProgramItems.aspx.cs" Inherits="Admin_ProgramItems" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server">
<style type="text/css">
    .btnImgChgDone{float: right; margin-right: 26px; margin-top: 10px;}
    div.header {  background: none repeat scroll 0 0 #CCCCCC;   float: left;    font-weight: bold;    margin-top: 5px;    width: 962px;}
    .imgBorder{border: 1px solid #000077;}
    div.header h2 {float:left;}
    div.gridprograms { padding:20px; }

</style>     
    <div class="header">
        <div class="headerleft">
            <h2>Program Items: <asp:Literal ID="litProgramName" runat="server" /></h2>         
        </div>
        <div style="float:right;margin-top:10px;margin-right:40px;">
            <asp:Button ID="btnDone" runat="server" OnClick="GotoProgramSetup" Text="Back to Program Setup" />
         </div>                  
    </div>
    
    &nbsp;
     
   <div class="gridprograms">
       <b> &nbsp;<asp:Literal ID="litItemsInProgram" runat="server" Text="Items In Program" /></b>
       <asp:GridView ID="grdItemInProg" runat="server" 
        AutoGenerateColumns="false" 
        OnRowDataBound="ItemInProgram_RowDataBound" EmptyDataText="There are no Items in the Program" EmptyDataRowStyle-ForeColor="Red"
        CssClass="mGrid" 
        GridLines="None"
        PageSize="150" 
        AllowPaging="true"
        PagerStyle-CssClass="pgr"
        OnPageIndexChanging="grdItemInProgram_PageIndexChanging"
        AllowSorting="true"
        OnSorting="grdItemsInProgram_Sorting">
        <Columns>
            <asp:TemplateField HeaderText="Item" ItemStyle-HorizontalAlign="Left" SortExpression="Item">
                <ItemTemplate>                    
                    <asp:HiddenField ID="hdnItemID" runat="server" />
                   <asp:Literal ID="litItmName" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        
            <asp:TemplateField HeaderText="Brand" ItemStyle-HorizontalAlign="Left" SortExpression="Brand">
                <ItemTemplate>                                        
                    <asp:Literal ID="litItmBrand" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
       
            <asp:TemplateField HeaderText="Dimension" ItemStyle-HorizontalAlign="Left" SortExpression="Dimension">
                <ItemTemplate>                                        
                   <asp:Literal ID="litItmDims" runat="server" />
                </ItemTemplate>
              </asp:TemplateField>
        
            <asp:TemplateField HeaderText="Pack Of" ItemStyle-HorizontalAlign="Left" SortExpression="PackOf">
                <ItemTemplate>                                        
                   <asp:Literal ID="litItmpakof" runat="server" />
                </ItemTemplate>
              </asp:TemplateField>

              <asp:TemplateField HeaderText="Price" ItemStyle-HorizontalAlign="Left" SortExpression="Price" >
                <ItemTemplate>                                        
                   <asp:Literal ID="litItmPri" runat="server" />
                </ItemTemplate>
              </asp:TemplateField>

            <asp:TemplateField HeaderText="Remove" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>                                        
                   <asp:ImageButton ID="imgBtnRemove" ImageUrl="~/Inc/Images/Others/Remove.jpg" OnClick="RemoveItem" alt="Remove" runat="server"  />
                </ItemTemplate>
              </asp:TemplateField>
         </Columns>
   </asp:GridView>
   </div>
   <hr />
   <div class="gridprograms">
       <b> &nbsp;<asp:Literal ID="litItemsAvailable" runat="server" Text="Items Available" /></b>
       <asp:GridView ID="grdItemAvlb" runat="server"
        AutoGenerateColumns="false" 
        OnRowDataBound="ItemAvailable_RowDataBound" 
        CssClass="mGrid" 
        GridLines="None"
        PageSize="150" 
        AllowPaging="true"
        PagerStyle-CssClass="pgr"
        OnPageIndexChanging="grdItemsAvailable_PageIndexChanging"
        AllowSorting="true"
        OnSorting="grdItemsAvailable_Sorting">
        <Columns>
            <asp:TemplateField HeaderText="Item" ItemStyle-HorizontalAlign="Left" SortExpression="Item"  >
                <ItemTemplate>       
                    <asp:HiddenField ID="hdnItemID" runat="server" />             
                    <asp:Literal ID="litItmAvlName" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        
            <asp:TemplateField HeaderText="Brand" ItemStyle-HorizontalAlign="Left" SortExpression="Brand">
                <ItemTemplate>                                        
                    <asp:Literal ID="litItmAvlBrand" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
       
            <asp:TemplateField HeaderText="Inventory on Demand" ItemStyle-HorizontalAlign="Left" SortExpression="Dimension">
                <ItemTemplate>                                        
                   <asp:Literal ID="litItmAvlDims" runat="server" />
                </ItemTemplate>
              </asp:TemplateField>
        
            <asp:TemplateField HeaderText="Pack Of" ItemStyle-HorizontalAlign="Left" SortExpression="PackOf" >
                <ItemTemplate>                                        
                   <asp:Literal ID="litItmAvlPakof" runat="server" />
                </ItemTemplate>
              </asp:TemplateField>

              <asp:TemplateField HeaderText="Price" ItemStyle-HorizontalAlign="Left" SortExpression="Price" >
                <ItemTemplate>                                        
                   <asp:Literal ID="litItmAvlPri" runat="server" />
                </ItemTemplate>
              </asp:TemplateField>

            <asp:TemplateField HeaderText="Add" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>                                        
                   <asp:ImageButton ID="imgBtnAdd" OnClick="AddItem"  ImageUrl="~/Inc/Images/Others/Add.jpg" alt="Add" runat="server"  />
                </ItemTemplate>
              </asp:TemplateField>
        </Columns>
   </asp:GridView>
   </div>

</asp:Content>

