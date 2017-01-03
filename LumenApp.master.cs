using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

public partial class LumenApp : System.Web.UI.MasterPage
{
    public HtmlGenericControl Hints;
    public Literal AllocatedBudget;
    public HtmlGenericControl BudgetRemaining;


    protected void Page_Load(object sender, EventArgs e)
    {
        bool AdminTabAllowed = Roles.IsUserInRole("Admin") || Roles.IsUserInRole("MM") || Roles.IsUserInRole("SM") || Roles.IsUserInRole("Level1") || Roles.IsUserInRole("DVP") || Roles.IsUserInRole("SVP") || Roles.IsUserInRole("DM") || Roles.IsUserInRole("SM") || Roles.IsUserInRole("UXL");
        if (Request.Url.ToString().Contains("/Admin/") && !Request.Url.ToString().EndsWith("/Admin/AllocateBudget.aspx"))
        {
            if (!AdminTabAllowed)
                Response.Redirect("~/Home.aspx");
        }

        string strBrowser = Request.Browser.Browser;
        string strTicks = System.DateTime.Now.Ticks.ToString();
        if(strBrowser.ToUpper().Contains("SAFARI"))
            strBrowser = "Safari.css";
        if(strBrowser.ToUpper().Contains("IE"))
            strBrowser = "IE.css";

        ConditionalCSS.Text = "<link runat='server' href='~/Inc/CSS/" + strBrowser + "?ticks=" + strTicks + "' rel='Stylesheet' type='text/css' />";                
        string strDA_JQuery = "<script type='text/javascript' src='" + ResolveUrl("~/Inc/JS/jquery.js?ticks=" + strTicks) + "' ></script>";
        strDA_JQuery += "<script type='text/javascript' src='" + ResolveUrl("~/Inc/JS/DA-jquery.js?ticks=" + strTicks) + "'></script>";

        lit_DA_JQuery.Text = strDA_JQuery;

        if (!Page.IsPostBack)
        {
            BindUserInfo();
        }

        if (Membership.GetUser().GetPassword().ToUpper() == "PASSWORD")
        {
            if(!Request.FilePath.Contains("ChangePassword.aspx"))
                Response.Redirect("~/Settings/ChangePassword.aspx?firsttime=1");
        }
    }

    protected override void OnInit(EventArgs e)
    {        
        AllocatedBudget = litAllocatedBudget;        
        base.OnInit(e);
    }

    private void BindUserInfo()
    {
        if (this.Page.User.Identity.IsAuthenticated)
        {
            string UserName = Membership.GetUser().UserName;
            if (UserName != null)
            {
                Literal litUserName = LoginViewEmerge.FindControl("litUserName") as Literal;
                if (litUserName != null)
                    litUserName.Text = UserName;
            }
        }
    }
    protected void SignOut_Click(object sender, EventArgs e)
    {
        //Clear all session values
        Session.Clear();
        Session.Abandon();
        FormsAuthentication.SignOut();
        FormsAuthentication.RedirectToLoginPage();
    }

    protected void BreadCrumbItemBound(object sender, SiteMapNodeItemEventArgs e)
    {
    }
}
