<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="Admin_Users" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">   
        <asp:Button ID="Button3" Text="Back" runat="server" PostBackUrl="~/Home.aspx" CssClass="button" />
        <h2>Add a new user and assign role</h2>
        
        <div>
            <p><asp:TextBox runat="server" placeholder = "User Name" ID="txtUserName"/></p>
            <p><asp:TextBox runat="server" placeholder = "Email" ID="txtEmail"/></p>
            <p><asp:TextBox runat="server" placeholder = "Role" ID="txtRole" Text="RSM"/></p>            
            <p>
                <asp:Button ID="Button1" runat="server" Text="Add User" OnClick="AddUser"/>
            </p>
        </div>

        <hr /><br />

        <h2>Bulk - Add users</h2>
        <div>
            <asp:TextBox style="width:100%;height:500px;background:#eeFFFF;border:1px solid blue" TextMode="MultiLine" ID="txtUserLoadInput" runat="server"></asp:TextBox>
            <p></p>
            <asp:Button ID="Button2" runat="server" Text="Add Users" OnClick="AddUsers" />
        </div>
    </form>
</body>
</html>
