using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Admin_pwdreset : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {             
        //if (!Roles.IsUserInRole("Admin"))
        //{
        //    Response.Write("Sorry! You do not have permissions to perform this action.");
        //    btnReset.Visible = false;
        //    return;
        //}

        MembershipUserCollection allusers = Membership.GetAllUsers();
        Response.Write("<h2>List of Users and their Passwords</h2> <div>Usernames in Red indicate disabled users.");
        Response.Write("<table border=1>");
        Response.Write("<tr><th>Email</th><th>User</th><th>Password</th></tr>");
        foreach (MembershipUser mu in allusers)
        {
            if (mu.IsLockedOut)
            {
                Response.Write("<tr style='color:red'><td>" + mu.Email + "</td><td>" + mu.UserName + "</td><td>???</td></tr>");
            }
            else
                Response.Write("<tr style='color:blue'><td>" + mu.Email + "</td><td>" + mu.UserName + "</td><td>" + mu.GetPassword() + "</td></tr>");
            Response.Flush();
        }
        Response.Write("</table>");
    }

    protected void resetpwd(object sender, EventArgs e)
    {
        string temppwd;
        MembershipUser mu = Membership.GetUser(txtEmailReset.Text);
        mu.UnlockUser();
        temppwd = mu.ResetPassword();
        mu.ChangePassword(temppwd, "password");
        Response.Redirect("pwdreset.aspx");
    }
}
