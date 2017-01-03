<%@ Page Title="" Language="C#" MasterPageFile="~/LumenNoLeft.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>

<asp:Content ContentPlaceHolderID="Title" runat="server">
Welcome to Emerge POS Items Ordering and Management System. 
</asp:Content>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <style>
        div.AreaLeft  {display:none; float:left;width:515px;background:#e7e7ee; padding:5px; border-radius:10px; margin-top:10px; margin-bottom:10px; margin-right:10px }
        div.AreaRight {display:none; float:left;width:415px;background:#eee0e0; padding:5px; border-radius:10px; margin-top:10px; margin-bottom:10px;}
        h2 {padding:5px;margin:5px;xbackground:white}
        
        table.Programs tr td{padding-bottom:10px; }

        table.Programs div.ItemTitle 
            {margin-top:20px;margin-left:10px;font-size:15px;font-weight:bold;font-family:Calibri; margin-bottom:15px;}
        table.Programs div.ItemTitle a {text-decoration:none;}            
        table.Programs div.ItemTitle a:hover {text-decoration:underline;}            
        
        table.Programs div.FieldName 
            {margin-left:30px; font-size:13px;font-weight:bold;font-family:Arial;color:#555577;}
        table.Programs div.FieldValue 
            {margin-left:30px; font-size:14px;font-weight:bold;font-family:Arial;color:Maroon;margin-bottom:15px;}
        div.header{display:none;}
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server">
    <div class="AreaLeft">                
        <h2>Recently Added POS Items</h2>    
        <div style="margin:10px;">
            <img src="Inc/Images/FeaturedItems.png" />
        </div>
    </div>

    <div class="AreaRight">        
        <h2>Current Programs</h2>                    
        
        <!-- <img src="Inc/Images/CurrentPrograms.png" style="margin-left:10px"/>        -->
        <div style="margin-left:10px;width:390px;background:white;">
        
        <asp:Repeater ID="rptCurrentPrograms" runat="server" OnItemDataBound="rptCurrentPrograms_ItemDataBound">
            <ItemTemplate>
                <table class="Programs" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:HyperLink ID="imgProgramPreviewImage" runat="server"/>
                        </td>
                        <td>
                            <div class="ItemTitle" style="">
                                <asp:HyperLink ID="lnkProgramName" runat="server"></asp:HyperLink>
                                
                            </div>
                            <div class="FieldName">
                                Closing Date:
                            </div>
                            <div class="FieldValue">
                                <asp:Literal ID="litClosingDate" runat="server"></asp:Literal>                                
                            </div>
                            <div class="FieldName">
                                In Market Date:
                            </div>
                            <div class="FieldValue">
                                <asp:Literal ID="litMarketDate" runat="server"></asp:Literal>                                                                
                            </div>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:Repeater>
        
        
        
        </div>
        
    </div>
    
    <div class="DescriptionTitlex" style="margin-top: 10px;">
        
    </div>

    <div style="margin-top:20px;margin-left:20px;width:500px;display:none">
        <asp:Image ImageUrl="~/Inc/Images/Others/WelcomePageBanner.png" runat="server"/>
    </div>
    
            

    <div class="DescriptionText" style="width:800px;margin-top:100px;margin-left:70px;padding:20px;padding-bottom:25px;text-align:center">
        <h1>Emerge</h1>
        
        <h2>Your POS Items Ordering Portal</h2>
    </div>
   
</asp:Content>

