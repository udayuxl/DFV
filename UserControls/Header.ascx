<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="UserControls_Header" %>

<div class="Banner" style="float:left;">        

    <div style="float:left;width:285px;margin-top:5px">
        <asp:HyperLink NavigateUrl="~/Home.aspx" runat="server" ID="HyperLink3" >
            <asp:Image ID="Image3" ImageUrl="~/Inc/Images/MasterPage/DFVWines_Header.JPG" AlternateText="Home" runat="server"/> 
        </asp:HyperLink>          
    </div>

    <div style="float:left;width:400px;margin-top:5px;margin-left:25px; " >
        <asp:HyperLink NavigateUrl="~/Home.aspx" runat="server" ID="HyperLink1" >
            <asp:Image ID="Image6" ImageUrl="~/Inc/Images/MasterPage/DFVSeasonal_Header.png" AlternateText="Home" runat="server" Width="370" Height="104"/> 
        </asp:HyperLink>
         
    </div>
    
    <asp:LoginView runat="server" ID="loginView">
        <AnonymousTemplate>
            <asp:Literal ID="litUserName" runat="server" />
        </AnonymousTemplate>
        <LoggedInTemplate>        
            <div class="LoggedInTemplate" style="margin-top:10px;width:220px;float:right;">
            Welcome
            <b><asp:Literal ID="litUserName" runat="server" /></b>                                        |
            <asp:LoginStatus ID="LoginStatus1" runat="server" />

            <div class="GlobalNav" style="margin-top:15px;">                
                <table class="tools" style="float:right">
                    <tr>                        
                        <td align="center">
                            <asp:HyperLink NavigateUrl="~/Settings/MyProfile.aspx"  runat="server" ID="HyperLink2" >
                                <asp:Image ID="Image4" ImageUrl="~/Inc/Images/Others/profile.png" AlternateText="My Profile" runat="server"/>
                                <br />
                                My Profile
                            </asp:HyperLink>                    
                        </td>                        
                        <td align="center" style="display:none;">
                            <asp:HyperLink NavigateUrl="~/HelpPages/HelpDesk.aspx"  runat="server" ID="HyperLink6" >
                                <asp:Image ID="Image5" ImageUrl="~/Inc/Images/Others/helpdesk2.png" runat="server"/> 
                                <br />
                                Helpdesk
                            </asp:HyperLink>                                        
                        </td>
                        <td align="center" style="display:none;">                    
                            <asp:HyperLink NavigateUrl="~/HelpPages/HelpDesk.aspx?page=list" runat="server" ID="HyperLink5">
                                <asp:Image ID="Image2" ImageUrl="~/Inc/Images/Others/helpdesk2.png" runat="server"/>
                                <br />
                                My Cases
                            </asp:HyperLink>
                        </td>
                        <td align="center">
                            <asp:HyperLink NavigateUrl="~/Admin/AdminLandingPage.aspx" runat="server" ID="lnkAdminPages">
                                <asp:Image ID="Image1" ImageUrl="~/Inc/Images/Others/admin.png" runat="server"/>
                                <br />
                                Admin
                            </asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </div>    
        </div>    
    </LoggedInTemplate>
    </asp:LoginView>
</div>


<div style="clear:both"></div>
