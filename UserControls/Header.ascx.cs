using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_Header : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Literal litUserName = loginView.FindControl("litUserName") as Literal;
        if (Membership.GetUser() == null)
            return;
        string strUserName = Membership.GetUser().UserName;
        if (!String.IsNullOrEmpty(strUserName))
        {              
            litUserName.Text = strUserName;
        }

        HyperLink lnkAdminPages = loginView.FindControl("lnkAdminPages") as HyperLink;
        bool AdminTabAllowed = Roles.IsUserInRole("Admin") || Roles.IsUserInRole("MM") || Roles.IsUserInRole("UXL") || Roles.IsUserInRole("SM") || Roles.IsUserInRole("Level1");
        lnkAdminPages.Visible = AdminTabAllowed;        
    }
}
