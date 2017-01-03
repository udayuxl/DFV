using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Admin_AdminLandingPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Manager") || Roles.IsUserInRole("Admin"))
        {
            divadmin.Visible = true;
            divadmintitle.Visible = false;
        }
        else
        {
            divadmin.Visible = false;
            divadmintitle.Visible = true;
            pagetitleID.Visible = false;
        }
    }
}