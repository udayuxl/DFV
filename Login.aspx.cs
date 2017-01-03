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

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["SalesManagerGuid"] = "";
        Session.Clear();
        UserControl Footer1 = (UserControl)this.FindControl("Footer1");
        string strBrowser = Request.Browser.Browser;
        if (strBrowser.ToUpper().Contains("SAFARI"))
            strBrowser = "Safari.css";
        if (strBrowser.ToUpper().Contains("IE"))
            strBrowser = "IE.css";

        ConditionalCSS.Text = "<link runat=\"server\" href=\"Inc/CSS/" + strBrowser + "\" rel=\"Stylesheet\" type=\"text/css\" />";
        //Footer1.FindControl("divBottomMenu").Visible = false;
        //----- Code to create a new user. Parameters are: (UserName, Password, Email)
        //Membership.CreateUser("Scott", "scott", "scott@insightresourcegroup.com");

        //----- Code to change a user's password -------         
        //string temppwd = Membership.GetUser("matt").ResetPassword();
        //Membership.GetUser("matt").ChangePassword(temppwd, "matt");
    }

    protected void EmergeLogin_Authenticate(object sender, AuthenticateEventArgs e)
    {
        #region Input Validation

        if (string.IsNullOrEmpty(EmergeLogin.UserName))
        {
            EmergeLogin.FailureText = "<div class='error'>" + "Please enter Email." + "</div>";
            return;
        }
        if (string.IsNullOrEmpty(EmergeLogin.Password))
        {
            EmergeLogin.FailureText = "<div class='error'>" + "Please enter Password." + "</div>";
            return;
        }

        #endregion

        try
        {
            string userEmail = EmergeLogin.UserName;
            if (!string.IsNullOrEmpty(userEmail))
            {
                string userName = null;
                userName = Membership.GetUserNameByEmail(userEmail);

                if (userName == null) // User Account doesn't exist
                    EmergeLogin.FailureText = "<div class='error'>" + "Invalid Email" + "</div>";
                else// User Account exists, so continue authenticating password .........
                {
                    if (Membership.ValidateUser(userName, EmergeLogin.Password)) //User entered correct credentials ......
                    {
                        string ReturnURL = "";
                        if (Request.QueryString["returnUrl"] != null)
                            ReturnURL = Request.QueryString["returnUrl"];

                        e.Authenticated = true;
                        FormsAuthentication.SetAuthCookie(userName, false);

                        //check after reset password user login first time if he/she first time login redirect to change password page.
                        string sMembershipUser = Membership.GetUserNameByEmail(userEmail);
                        MembershipUser mu = Membership.GetUser(sMembershipUser);
                        string sComment = mu.Comment;
                        if (sComment == "Need Change Password" && !string.IsNullOrEmpty(sComment))
                        {
                            if (!Request.FilePath.Contains("ChangePassword.aspx"))
                                //clear comment for password reset...
                                Response.Redirect("~/Settings/ChangePassword.aspx?firsttime=1");
                        }
                        else
                        {
                            string flow = "";
                            if (Request.QueryString["flow"] != null)
                                flow = Request.QueryString["flow"];
                            if (flow == "approvalflow")
                                Response.Redirect("~/OrderApproval/InventoryOrderItemApproval.aspx");
                            else
                            {
                                string strRedirectURL = ReturnURL.Replace("/Dev", "~").TrimEnd('/');

                                if (String.IsNullOrEmpty(strRedirectURL))
                                    strRedirectURL = "~/Home.aspx";

                                Response.Redirect(strRedirectURL, true);
                            }
                        }
                    }
                    else //User entered Wrong credentials .....
                    {
                        if (IsUserLocked(userName))
                        {
                            EmergeLogin.FailureText = @"<div class='error'> This user account is locked due to many invalid login attempts<br/><br/>
                            <span style='font-weight:normal'>Please use the <b>Forgot Password</b> link below to unlock your account and reset the password.</span></div>";
                            return;
                        }

                        int MaxInvalidAttemptsAllowed = Membership.MaxInvalidPasswordAttempts;
                        int WrongAttemptsMade = GetInvalidAttempts(userName);
                        int RemainingAttempts = MaxInvalidAttemptsAllowed - WrongAttemptsMade;

                        EmergeLogin.FailureText = @"<div class='error'> Invalid Email or Password.<br/><br/> ";
                        EmergeLogin.FailureText += "Warning: <br/><span style='font-weight:normal'>" + RemainingAttempts.ToString() + " more wrong attempts will lock the user account. <br/>";
                        EmergeLogin.FailureText += "<br/>If you need help in recovering your password, please use the <b>Forgot Password</b> link below.</span></div>";

                        //EmergeLogin.FailureText = "<div class='error'>" + "Invalid Email or Password." + "</div>";
                    }
                }
            }
            else
                EmergeLogin.FailureText = "<div class='error'>" + "Invalid Email or Password." + "</div>";
        }
        catch (Exception ex)
        {
            EmergeLogin.FailureText = "<div class='error'>" + ex.Message + "</div>";
        }

    }

    int GetInvalidAttempts(string strUserName)
    {
        DataRow dr = ACE.DataOperations.GetSingleRow("select m.FailedPasswordAttemptCount from dbo.aspnet_Membership m join aspnet_Users u on u.UserId = m.UserId where UserName = '" + strUserName + "'");
        return Convert.ToInt32(dr[0].ToString());
    }

    bool IsUserLocked(string strUserName)
    {
        {
            DataRow dr = ACE.DataOperations.GetSingleRow("select m.IsLockedOut from dbo.aspnet_Membership m join aspnet_Users u on u.UserId = m.UserId where UserName = '" + strUserName + "'");
            return Convert.ToBoolean(dr[0].ToString());
        }
    }
}