<%@ Page Title="" Language="C#" MasterPageFile="~/LumenApp.master" AutoEventWireup="true" CodeFile="AdminLandingPage.aspx.cs" Inherits="Admin_AdminLandingPage" %>

<asp:Content ContentPlaceHolderID="Title" runat="server">
    <h2 id="pagetitleID" runat=server>Administrator</h2>
</asp:Content>   

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server">
    <style type="text/css">
  
    .admindiv
    {
    	margin-left:20px;
    	color:#620937;
    }
  
    .headertext
    {
    	font-size:14px; 
    	font-weight:bold;
    	margin-left:15px;    	
    }
       
    p
    {
    	margin-left:10px;
    }
    div.pardiv
    {
    	margin-top:-20px;
    	margin-left:8px;
    }
    
</style>
    
    <div>&nbsp;</div>
    <div  id="divadmintitle" runat="server" style="font-size: 36px; text-align:center;  margin-top:110px;">  Administration Section </div>
    <div  id="divadmin" runat="server" class="admindiv">          
        <h2 class="headertext"><a href="POS_Items.aspx">POS Item Setup</a></h2><br />
        <div class="pardiv">
            <p>Create new promotional items and add to Programs </p>
            <p>Use Items already created – Add to programs, Edit Items, Enable/Disable Items</p>
        </div>  
       <h2 class="headertext"><a href="ProgramSetup.aspx">Program Setup</a></h2><br />
        <div class="pardiv">
            <p>Create and publish new Programs</p>
            <p>Define Program Order Windows and edit existing Programs</p>
        </div> 
        <!--
        <h2 class="headertext"><a href="../OrderApproval/InventoryOrderItemApproval.aspx">Order Approval</a></h2><br />
        <div class="pardiv">
            <p> Approve Orders.</p>
        </div>     
        -->
   </div>
    <div id="divInvAll" runat="server" class="admindiv" style="display:none">
        <h2 class="headertext"><a href="InventoryAllocation.aspx">Inventory Allocation</a></h2><br />
        <div class="pardiv">
            <p> Allocate inventoried items to Sales Managers based on the inventory available.</p>
            
        </div>
    </div>
    <div id="div1" runat="server" class="admindiv">
       
    </div>
</asp:Content>

 