<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Emerge - Online POS - Login</title>
        <link id="Link1" runat="server" href="~/Inc/CSS/Style.css" rel="Stylesheet" type="text/css" />
		<!--[if IE]>
			<link runat="server" href="~/Inc/CSS/IE.css" rel="Stylesheet" type="text/css" />
		<![endif]-->
		<asp:Literal ID="ConditionalCSS" runat="server"></asp:Literal>
    </head>
    <body>        
        <form id="form1" runat="server">
            <div class="LoginContent" runat="server">
                <div class="Login">
                    <asp:Login ID="EmergeLogin" runat="server" CssClass="LoginLayout" 
                        onauthenticate="EmergeLogin_Authenticate" >
                        <LayoutTemplate>                            
                                <h2>SIGN IN</h2>
                                <asp:Literal ID="FailureText" runat="server"/>
                                <div style="font-weight:bold">
                                    EMAIL:<em>*</em>
                                    <asp:TextBox ID="UserName" CssClass="LoginLayoutTextbox" runat="server"/>
                                </div>
                                <div style="font-weight:bold">
                                    PASSWORD:<em>*</em>
                                    <asp:TextBox ID="Password" CssClass="LoginLayoutTextbox" TextMode="Password" runat="server"/>
                                </div>                                
                                <div style="text-align:right">
                                    <em>*</em><span style="margin-right:50px;">REQUIRED</span>
                                    <asp:CheckBox ID="RememberMe" Text="REMEMBER ME" Visible="true" runat="server" />
                                </div>
                                <div class="bottompadding10" >
									<a class="topmargin10" href="ForgotPassword.aspx"><b>FORGOT PASSWORD</b></a>
                                    <asp:Button ID="Login" Text="Login" CssClass="LoginLayoutButton" CommandName="Login" runat="server" />
                                </div>
                        </LayoutTemplate>
                    </asp:Login>
                    <%--<div class="logintermsandconditions">
                    <asp:HyperLink NavigateUrl="~/TermsAndConditions.aspx" runat="server" ID="lnkbtnTermsAndConditions" Text="TERMS AND CONDITIONS" /></a>
                    </div>
                    <div class="logindrinkiq">
                    <asp:HyperLink NavigateUrl="http://www.drinkiq.com" Target="_blank" runat="server" ID="lnkbtnDrinkIQ" Text="DRINKiQ" /></a>
                    </div>--%>
                </div>
            </div>
        </form>
    </body>
</html>
