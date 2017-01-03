<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="ForgotPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
        <title>Emerge - Online POS - Forgot Password</title>
        <link id="Link1" runat="server" href="~/Inc/CSS/Style.css" rel="Stylesheet" type="text/css" />
		<!--[if IE]>
			<link runat="server" href="~/Inc/CSS/IE.css" rel="Stylesheet" type="text/css" />
		<![endif]-->
		<asp:Literal ID="ConditionalCSS" runat="server"></asp:Literal>
        <script language="javascript"">
            function pleasewaitmsg() {
                document.getElementById("divMsg").innerHTML = "Processing...";
                document.getElementById("btnLogin").setAttribute("style","display:none");
                return true;
            }
        </script>
    </head>
    <body>        
        <form id="form1" runat="server">
            <div id="Div1" class="LoginContent" runat="server">
                <div style="">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Inc/Images/MasterPage/LogoDFV.jpg" />                
                </div>
                <div class="Login">
                    <div style="position:absolute;z-index:1">
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/Inc/Images/MasterPage/Wines500.png" />
                    </div>
                    <div style="float:left;margin-left:610px;">
                        <table class="LoginLayout">
                            <tr>
                                <td>
                                    <h2>FORGOT PASSWORD</h2>
                                    <div style="color:red" id="divMsg"><asp:Literal ID="FailureText" runat="server"/></div>
                                    <div style="font-weight:bold">
                                        EMAIL:<em>*</em>
                                        <asp:TextBox ID="txtEmail" CssClass="LoginLayoutTextbox" runat="server"/>
                                    </div>
                                    <div class="bottompadding10" >							            
                                        <asp:Button ID="btnLogin" Text="Send Password" ClientIDMode="Static" OnClientClick="return pleasewaitmsg();" CssClass="LoginLayoutButton" OnClick="SendPassword" runat="server" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>                    
                </div>
            </div>
        </form>
    </body>
</html>
