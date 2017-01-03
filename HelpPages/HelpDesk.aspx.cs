using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Order_HelpDesk : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["page"] == "list")
        {
            h3Title.Text = "My Cases";
            iFrameForce.Attributes["src"] = @"http://irg.force.com/helpdesk/my_cases?CurrentAccount=Bogle&LoggedInUser=" + Membership.GetUser().UserName.ToString() + "&Email=" + Membership.GetUser().Email.ToString();
        }
        else
            iFrameForce.Attributes["src"] = @"http://irg.force.com/helpdesk/?CurrentAccount=Bogle&LoggedInUser=" + Membership.GetUser().UserName.ToString() + "&Email=" + Membership.GetUser().Email.ToString();
    }
}