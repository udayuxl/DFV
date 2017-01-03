<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="ForgotPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forgot Password</title>
    <link id="Link1" runat="server" href="~/Inc/CSS/Style.css?v=mar25" rel="Stylesheet" type="text/css" />
    <link id="Link2" runat="server" href="~/Inc/CSS/ReportsPrint.css" rel="Stylesheet" media="print" type="text/css" />
    <!--[if lte IE 6]>
    <link runat="server" href="~/Inc/CSS/IE.css" rel="Stylesheet" type="text/css" />
    <![endif]-->
    <asp:Literal ID="ConditionalCSS" runat="server"></asp:Literal> 
    <style type="text/css">        
        div.Container  { background-color: white;  border: 1px solid #E7E7E7;  float: left; width: 958px;min-height:450px; }        
        div.header {background: none repeat scroll 0 0 #736C66 !important;float: left;font-weight: bold; width: 958px; margin-top: -2px; }            
        h2 {font-size: 36px;color: #444444;}            
        p  { font-family: "ff-tisa-web-pro-1","ff-tisa-web-pro-2",Georgia,serif; line-height: 140%;color: #444444;font-size:16px;}
        .btnforgotpassword{float:right;margin-right: 34px; height: 30px !important;}
        span.lblerrormsg{color:Red;margin-bottom:5px;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release" LoadScriptsBeforeUI="false"></asp:ScriptManager>
       
        <div class="Header"> 
            <Lumen:Header ID="Header" runat="server" />           
        </div>
        <div id="divContainer" class="Container" runat="server">                        
            <div class="header">
                <div class="headerleft">
                    <h2>Forgot Password</h2>
                </div>
            </div>
            <br />
            <%--<div style="margin-left:20px;font-weight:bold;margin-top: 45px;">
                <asp:Label ID="lblServerErrOrSucc" runat="server" Visible="false" Text = "test mail" /> 
            </div>--%>
            <div style="margin-left:15px; width: 540px;">
                <asp:PlaceHolder ID="plchldEmailReset" runat="server" Visible="true">
                    <h2>Forgot your password?</h2>
                    <p> Please enter your “Registered Email Address” and click "Submit" button.  <br />We will send your Username and Password to you.</p>
                    <div>
                        <asp:Label ID="lblErrormsg" runat="server" Visible="false" CssClass="lblerrormsg" />
                        <br />
                        <asp:TextBox ID="txtEmail" runat="server" placeholder="Your E-mail Address" Width="500px" />
                        <br /> <br />
                        <asp:Button ID="btnForgotPassword" runat="server" Text="Submit" CssClass="btnforgotpassword" OnClick="BtnForgotPassword_Click" />
                    </div>
                </asp:PlaceHolder>
            </div>
            <div style="margin-left:15px; width: 700px;">
                <asp:PlaceHolder ID="plchldEmailsuccessful" runat="server" Visible="false">
                    <h2>Check your e-mail.</h2>
                    <p>Your Username and Password has been sent to the registered email address.</p>
                    <p>If you do not see the email in your inbox, check your junk mail folder. If you have not received the email , make sure you enter your correct email address and  <a href="ForgotPassword.aspx"><b> try again</b></a>.</p>
                    <p>If you continue to face issues in logging into your account. Please send an email to <a href="mailto:jfwpos@gmail.com">support@jfwpos.com</a>  </p>
                </asp:PlaceHolder>
            </div>
        </div>
        <div class="Footer">
        </div>         
    </form>
</body>
</html>
