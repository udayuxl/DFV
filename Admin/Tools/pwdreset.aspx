<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pwdreset.aspx.cs" Inherits="Admin_pwdreset" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txtEmailReset" runat="server"></asp:TextBox>
        <asp:Button ID="btnReset" runat="server" OnClick="resetpwd" />
        
    </div>
    </form>
</body>
</html>
