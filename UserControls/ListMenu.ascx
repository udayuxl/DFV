<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ListMenu.ascx.cs" Inherits="UserControls_ListMenu" %>

<script language="javascript">

function mouseoverLI(obj)
{
    obj.getElementsByTagName("ul")[0].className = "visible"; 
    obj.className = "expanded";   
}

function mouseoutLI(obj)
{       
   obj.getElementsByTagName("ul")[0].className = "";    
   obj.className = "";
}

function clickLI(obj)
{
    parentUL = obj.parentNode;        
    allLIs = parentUL.getElementsByTagName("li")
    
    for(i=0;i<allLIs.length;i++)
    {        
        allLIs[i].className = "";
        subULs = allLIs[i].getElementsByTagName("ul");
        if(subULs.length > 0)
            subULs[0].className = "";
    }

    //if(obj.getElementsByTagName("ul")[0].className == "")
    {
        obj.getElementsByTagName("ul")[0].className = "visible";    
        obj.className = "expanded";
    }
    
    
    
    /*else
    {
        obj.getElementsByTagName("ul")[0].className = "";    
        obj.className = "";
    }*/
}

</script>

<style>
div#divleftmenu 
{
    x-background: #CCCCCC;
    position: relative;
    x-top: 40px;
}
</style>

<div class="hints" style="display:none">
    <asp:Literal ID="litTotalSpend" runat="server" Visible="false"></asp:Literal>
</div>
<h2 id="h2MenuTitle" runat="server">Search Items</h2>
<div id="divleftmenu">
    <ul ID="rootUL" runat="server">
    </ul>
</div>
