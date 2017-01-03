using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Settings_ChangePassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        changepwd.ChangingPassword += new LoginCancelEventHandler(changepwd_ChangingPassword);

        if (Request.QueryString["firsttime"]!=null)
            ((HtmlControl)changepwd.ChangePasswordTemplateContainer.FindControl("FirstTime")).Visible = true;

    }

    void changepwd_ChangingPassword(object sender, LoginCancelEventArgs e)
    {
        if (changepwd.NewPassword.Length < 8)
        {
            changepwd.ChangePasswordFailureText += "<br>The password must be minimum 8 characters";
        
            ((Literal)changepwd.ChangePasswordTemplateContainer.FindControl("FailureText")).Text= "The password must be minimum 8 characters";
            e.Cancel = true;
        }
    }
    
    protected void GoHome(object sender, EventArgs e)
    {
        Response.Redirect("~/Home.aspx");
    }
}
