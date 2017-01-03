using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Admin_Users : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    
    protected void AddUsers(object sender, EventArgs e)
    {
        string strInput = txtUserLoadInput.Text;
        string[] lines = strInput.Split('\n');
        string username = "", email = "", role = "";
        foreach (string line in lines)
        {
            if (line.Contains(','))
            {
                username = line.Split(',')[0].Trim();
                email = line.Split(',')[1].Trim();
                role = line.Split(',')[2].Trim();
                
                try
                {
                    Membership.CreateUser(username, "password!", email);
                    Roles.AddUserToRole(username, role);

                    Response.Write("User: " + username + " added <br/>");

                }
                catch (Exception ex)
                {
                    Response.Write("User: " + username + " Failed with Exception -" + ex.Message + " <br/>");
                }

            }
        }
        
    }

    protected void AddUser(object sender, EventArgs e)
    {
        try
        {
            string chkUsername = Membership.GetUserNameByEmail(txtEmail.Text);
            if (chkUsername == null)
            {
                chkUsername = txtUserName.Text;
                MembershipUser mu = Membership.CreateUser(chkUsername, "password!", txtEmail.Text);
                Response.Write("<div style='color:gray'>Creating user " + chkUsername + "... </div>");                
            }

            if (!Roles.RoleExists(txtRole.Text))
            {
                Roles.CreateRole(txtRole.Text);
                Response.Write("<div>Creating role " + txtRole.Text + "... </div>");
                Response.Write("<div style='color:gray'>Creating role - " + txtRole.Text + " ...</div>");
            }

            if (!Roles.IsUserInRole(chkUsername, txtRole.Text))
            {
                Roles.AddUserToRole(chkUsername, txtRole.Text);
                Response.Write("<div style='color:gray'>Adding user to the role ... </div>");
            }
            Response.Write("<div style='color:green;font-weight:bold;'>Created " + chkUsername + " and assigned the role - " + txtRole.Text + "</div>");
        }
        catch (Exception ex)
        {
            Response.Write("<div style='color:red'>" + ex.Message +"</div>");
        }
    }
}