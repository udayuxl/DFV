using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text.RegularExpressions;

public partial class ForgotPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strBrowser = Request.Browser.Browser;
        if (strBrowser.ToUpper().Contains("SAFARI"))
            strBrowser = "Safari.css";
        if (strBrowser.ToUpper().Contains("IE"))
            strBrowser = "IE.css";
        ConditionalCSS.Text = "<link runat=\"server\" href=\"../Inc/CSS/" + strBrowser + "\" rel=\"Stylesheet\" type=\"text/css\" />";
    }
    protected void BtnForgotPassword_Click(object sender, EventArgs e)
    {
        bool bStatus = ValidateEmail(txtEmail.Text);
        if (bStatus == false)
        {
            lblErrormsg.Visible = true;
            lblErrormsg.Text = "* This email address is invalid. <br/>  Please enter your email address registered with www.jfwpos.com.<br/>";
        }
        else if(bStatus == true)
        {
            lblErrormsg.Visible = false;
            lblErrormsg.Text = "";
            try
            {
                string temppwd;
                string sMembershipUser = Membership.GetUserNameByEmail(txtEmail.Text);
                MembershipUser mu = Membership.GetUser(sMembershipUser);
                mu.UnlockUser();

                string newPassword = Membership.GeneratePassword(15, 0);
                newPassword = Regex.Replace(newPassword, @"[^a-zA-Z0-9]", m => "9");
                string UserID = mu.ProviderUserKey.ToString();
                temppwd = mu.ResetPassword();
                mu.ChangePassword(temppwd, newPassword);
                                
                //Send email notification when password reset.
                string EmailBody = "";
                EmailBody = GetEmailFormat(mu.UserName.ToString(), mu.Email.ToString(), newPassword);                
                plchldEmailReset.Visible = false;
                plchldEmailsuccessful.Visible = true;
            }
            catch (Exception ex)
            {
                // when error occure reseting password or send email.
                
            }
        }
    }

    protected bool ValidateEmail(string email)
    {
        if(string.IsNullOrEmpty(email))
            return false;
        else
        {
            if (Membership.FindUsersByEmail(email) != null)
            {
                if (Membership.FindUsersByEmail(email).Count <= 0)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    public static string GetEmailFormat(string username,string email, string password)
    {
        string strEmailBody = "";
        strEmailBody += "Dear " + username;
        strEmailBody += "<br/><br/>";
        strEmailBody += "Your password has been reset as per your request.";
        strEmailBody += "<br/><br/>";
        strEmailBody += "Please find the login information below.";
        strEmailBody += "<br/><br/>";
        strEmailBody += "<table border='0px solid #C1C1C1' style='border-collapse: collapse;'>";
        strEmailBody += "<tr><td>User Name :</td><td>" + email + "</td></tr>";
        strEmailBody += "<tr><td>Password  :</td><td>" + password + "</td></tr>";        
        strEmailBody += "</table>";
        strEmailBody += "<br/><br/>";  
        strEmailBody += "Please use ''Change Password'' screen to change your password during the first login.";
        strEmailBody += "<br/><br/>";        
        strEmailBody += "Jackson Family Wines - Emerge";
        strEmailBody += "<br/>";
        return strEmailBody;
    }
}