using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ACE;

public partial class ForgotPassword : System.Web.UI.Page
{
    protected void SendPassword(object sender, EventArgs e)
    {
        if (String.IsNullOrWhiteSpace(txtEmail.Text))
        {
            FailureText.Text = "Please enter Email";
            return;
        }
        string strUserName = Membership.GetUserNameByEmail(txtEmail.Text.Trim());
        if (strUserName == null)
        {
            FailureText.Text = "User not found / Invalid Email";
            return;
        }

        if (Lumen.MailServices.SendPasswordEmail(strUserName))
            Response.Redirect("login.aspx", false);

    }

    
}