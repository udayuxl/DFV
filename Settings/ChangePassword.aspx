<%@ Page Language="C#" MasterPageFile="~/LumenNoLeft.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="Settings_ChangePassword" Title="Change Password" %>

<asp:Content ContentPlaceHolderID="Title" runat="server">
    <h2>Change Password</h2>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server">

    <asp:ChangePassword ID="changepwd" runat="server" CssClass="ChangePwd" >

    <ChangePasswordTemplate>    
        <div style="width:750px;padding-left:20px;font-weight:normal"  class="header" runat="server" id="FirstTime" visible="false">
            <b>Please take a moment to change your password.</b>
            <br /><br />
            Your password must be a minimum of 8 characters and must contain atleast 1 Special character.
        </div>
        
        <table border="0" cellpadding="1" cellspacing="0" 
            style="border-collapse:collapse; margin:20px;">
            <tr>               
                <td>
                    <table border="0" cellpadding="0">                      
                          <tr>
                            <td colspan="2">
                                <div style="margin-bottom:15px;width:300px;color:Red;">
                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                </div>
                            </td>
                        </tr> 
                    
                        <tr>
                            <td align="left">
                                <asp:Label ID="CurrentPasswordLabel" runat="server" 
                                    AssociatedControlID="CurrentPassword">Current Password:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" 
                                    ControlToValidate="CurrentPassword" ErrorMessage="Password is required." 
                                    ToolTip="Password is required." ValidationGroup="changepwd">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="NewPasswordLabel" runat="server" 
                                    AssociatedControlID="NewPassword">New Password:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="NewPassword" runat="server" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" 
                                    ControlToValidate="NewPassword" ErrorMessage="New Password is required." 
                                    ToolTip="New Password is required." ValidationGroup="changepwd">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="ConfirmNewPasswordLabel" runat="server" 
                                    AssociatedControlID="ConfirmNewPassword">Confirm New Password:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" 
                                    ControlToValidate="ConfirmNewPassword" 
                                    ErrorMessage="Confirm New Password is required." 
                                    ToolTip="Confirm New Password is required." ValidationGroup="changepwd">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:CompareValidator ID="NewPasswordCompare" runat="server" 
                                    ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword" 
                                    Display="Dynamic" 
                                    ErrorMessage="The Confirm New Password must match the New Password entry." 
                                    ValidationGroup="changepwd"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2" style="color:Red;">
                                
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td align="right" style="padding-right:10px;">
                                <asp:Button ID="ChangePasswordPushButton" runat="server" 
                                    CommandName="ChangePassword" Text="Change Password" 
                                    ValidationGroup="changepwd" class="button"/>
                            </td>                            
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </ChangePasswordTemplate>
    <SuccessTemplate>                
         <table border="0" cellpadding="1" cellspacing="0" 
            style="border-collapse:collapse; margin:40px;">
            <tr>               
                <td style="color:Green;padding-bottom:20px">
                    Your password has been changed!
                </td>
            </tr>                
            
            <tr>               
                <td align="right">
                    <asp:Button runat="server" Text="Continue" OnClick="GoHome" />
                </td>
            </tr>                
        </table>
    </SuccessTemplate>
    </asp:ChangePassword>

</asp:Content>

