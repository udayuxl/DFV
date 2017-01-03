using System;
using System.Collections;
using System.Collections.Generic;
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

////using Emerge.Data;
////using Emerge.Service;


public partial class UserControls_ListMenu : System.Web.UI.UserControl
{
    public string CssClass { get; set; }
    public string StartingNodeURL { get; set; }
    public bool ShowStartingNode { get; set; }


    protected void Page_Load(object sender, EventArgs e)
    {
        SiteMapNode RootNode = SiteMap.RootNode;
        if(StartingNodeURL != null && StartingNodeURL != "")
            RootNode = SiteMap.Provider.FindSiteMapNodeFromKey(StartingNodeURL);

        string menuTDs = "";
        foreach (SiteMapNode topmenunode in RootNode.ChildNodes)
        {
            if (topmenunode.Title == "Coupon Management")
            {
                /*
                bool status = IsVisiablebyCurrentLoginUser(topmenunode);
                bool nodeVisible = Convert.ToBoolean(topmenunode["visible"]);
                if (status == true && nodeVisible)
                {
                    menuTDs += "<td><a href='" + topmenunode.Url + "'>" + topmenunode.Title + "</a></td>";
                }
                 * */
            }
            else if (topmenunode.Title == "Order Approval")
            {
                /*
                bool status = IsVisiablebyMarketingAndAdmin(topmenunode);
                if (status == true)
                {
                    menuTDs += "<td><a href='" + topmenunode.Url + "'>" + topmenunode.Title + "</a></td>";
                }
                 */
            }
            else if (IsAccessible(topmenunode))
            {
                menuTDs += "<td><a href='" + topmenunode.Url + "'>" + topmenunode.Title + "</a></td>";
            }
            

            /*else if (topmenunode.Title == "Admin")
            {
                bool status = IsVisiablebyAllDSMs(topmenunode);
                if (status == true)
                {
                    menuTDs += "<td><a href='" + topmenunode.Url + "'>" + topmenunode.Title + "</a></td>";
                }

            }*/
        }
        litTopmenu.Text = menuTDs;
    }

    private bool IsAccessible(SiteMapNode node)
    {
        if (node["visible"] == "false")
            return false;
        string strRoles = node["allowedroles"];        
        if(strRoles == null)
            return true;
        if(strRoles == "")
            return true;
        string [] roles = strRoles.Split(',');
        foreach(string role in roles)
        {
            if(Roles.IsUserInRole(role))
                return true;
        }
        return false;
    }

    

    private string GetDynamicChildNodesHTML(string datasourcename)
    {
        string childNodesHTML = "";
        childNodesHTML += "<ul>";
        childNodesHTML += "<li><a href='#'>This is dynamic child 1 of " + datasourcename + " </a></li>";
        childNodesHTML += "<li><a href='#'>This is dynamic child 2 of " + datasourcename + " </a></li>";
        childNodesHTML += "<li><a href='#'>This is dynamic child 3 of " + datasourcename + " </a></li>";
        childNodesHTML += "<li><a href='#'>This is dynamic child 4 of " + datasourcename + " </a></li>";
        childNodesHTML += "</ul>";
        return childNodesHTML;
    }

}
