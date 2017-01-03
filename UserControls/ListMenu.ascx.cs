﻿using System;
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
using Lumen;

public partial class UserControls_ListMenu : System.Web.UI.UserControl
{
    public string CssClass { get; set; }
    public string StartingNodeURL { get; set; }
    public bool ShowStartingNode { get; set; }


    protected void Page_Load(object sender, EventArgs e)
    {                    
        litTotalSpend.Text = "Remaining Budget: <b>$" + "WIP" + "</b>";


        SiteMapNode StartingNode = SiteMap.CurrentNode;
        if (StartingNode != null)
        {
            if (StartingNode.IsDescendantOf(SiteMap.Provider.FindSiteMapNode("~/Reports/ReportsHome.aspx")))
                StartingNode = SiteMap.CurrentNode.ParentNode;
            if (StartingNode.IsDescendantOf(SiteMap.Provider.FindSiteMapNode("~/Admin/AdminLandingPage.aspx")))
                StartingNode = SiteMap.CurrentNode.ParentNode;
            if (StartingNode.IsDescendantOf(SiteMap.Provider.FindSiteMapNode("~/Coupons/CouponList.aspx")))
                StartingNode = SiteMap.CurrentNode.ParentNode;
        }
        else
        {
            h2MenuTitle.InnerText = "";
            if (!Page.IsPostBack)
            {                    
            }
            return;
        }
        if (StartingNodeURL != "")
            StartingNode = SiteMap.Provider.FindSiteMapNode(StartingNodeURL);

        if (!Page.IsPostBack)
        {
            if (rootUL.Attributes["class"] != null)
                rootUL.Attributes["class"] = CssClass;
            else
                rootUL.Attributes.Add("class", CssClass);
            {
                h2MenuTitle.Visible = true;                    
                    
                if (Request.Url.AbsoluteUri.IndexOf("Reports/MasterPage.aspx", StringComparison.OrdinalIgnoreCase) > 0 || Request.Url.AbsoluteUri.IndexOf("Reports/Pre-OrderRecap.aspx", StringComparison.OrdinalIgnoreCase) > 0 || Request.Url.AbsoluteUri.IndexOf("Reports/ProgramPlaybook.aspx", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    if (StartingNodeURL != "")
                        StartingNode = SiteMap.Provider.FindSiteMapNode("~/URLLeftMenuReports");
                }
                if (Request.Url.AbsoluteUri.IndexOf("Reports/SalesManagerWisePre-Order.aspx", StringComparison.OrdinalIgnoreCase) > 0 || Request.Url.AbsoluteUri.IndexOf("Reports/ItemWisePre-Order.aspx", StringComparison.OrdinalIgnoreCase) > 0 || Request.Url.AbsoluteUri.IndexOf("Reports/StateWisePre-OrderRecap.aspx", StringComparison.OrdinalIgnoreCase) > 0 || Request.Url.AbsoluteUri.IndexOf("Reports/StateWiseBudgetAllocation.aspx", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    if (StartingNodeURL != "")
                        StartingNode = SiteMap.Provider.FindSiteMapNode("~/URLLeftMenuReports");
                }
                       
                if (ShowStartingNode)
                    rootUL.InnerHtml = GetNodeHTML(StartingNode);
                else
                {
                    h2MenuTitle.Visible = false;                            
                    foreach (SiteMapNode childnode in StartingNode.ChildNodes)
                    {
                        if (childnode.Title.ToUpper() == "ORDER APPROVAL")
                        {
                            if (!AdminServices.IsUserAnApprover())
                                continue;
                        }

                        if (childnode.Title.ToUpper() == "ALLOCATE BUDGET")
                        {
                            if (AdminServices.IsUserInLevel("Level1") || AdminServices.IsUserInLevel("Level2") || AdminServices.IsUserInLevel("Level3") || Roles.IsUserInRole("MM") || Roles.IsUserInRole("Admin") )
                                rootUL.InnerHtml += GetNodeHTML(childnode);
                            continue;
                        }

                        if (IsAccessible(childnode))
                            rootUL.InnerHtml += GetNodeHTML(childnode);

                        //if (Roles.IsUserInRole("Admin") || Roles.IsUserInRole("UXL"))
                        //{
                        //    rootUL.InnerHtml += GetNodeHTML(childnode);
                        //}
                        //else
                        //{
                        //    if(childnode.Title.ToUpper() == "ALLOCATE BUDGET")
                        //        if(AdminServices.IsUserInLevel("Level1") || AdminServices.IsUserInLevel("Level2") || AdminServices.IsUserInLevel("Level3"))
                        //            rootUL.InnerHtml += GetNodeHTML(childnode);
                        //}
                    }                            
                }
                    
            }
            if (SiteMap.CurrentNode != null)
            {
                if (SiteMap.CurrentNode["ShowTitle"] != null && SiteMap.CurrentNode["ShowTitle"].ToUpper() == "FALSE")
                {
                    h2MenuTitle.Visible = false;
                }
            }                              
        }        
    }

    private bool IsAccessible(SiteMapNode node)
    {
        if (node["visible"] == "false")
            return false;
        string strRoles = node["allowedroles"];
        if (strRoles == null)
            return true;
        if (strRoles == "")
            return true;
        string[] roles = strRoles.Split(',');
        foreach (string role in roles)
        {
            if (Roles.IsUserInRole(role))
                return true;
        }
        return false;
    }
    private string GetNodeHTML(SiteMapNode node)
    {
        bool clickmode = Request.Browser.IsBrowser("ie");
        clickmode = true;

        if (node["Visible"] == "false")
            return "";

        if (node["manageronly"] == "true")
        {
            if (!Roles.IsUserInRole("Manager") && !Roles.IsUserInRole("Admin"))
                return "";
        }

        string liHTML = "";

        if (node.HasChildNodes)
            liHTML = "<li " + ((clickmode) ? "onClick='clickLI(this)'" : "onmouseover='mouseoverLI(this);' onmouseout='mouseoutLI(this);'") + ">" + "<a class='expandable' href='" + ((node["NotLink"] == "true") ? "#" : node.Url) + "'>" + node.Title + "</a>";
        else
            liHTML = "<li>" + "<a class='' href='" + ((node["NotLink"] == "true") ? "#" : node.Url) + "'>" + node.Title + "</a>";
        string SubUlHTML = "";
        if (node.HasChildNodes)
        {
            SubUlHTML = "<ul>";
            foreach (SiteMapNode childnode in node.ChildNodes)
            {
                SubUlHTML += GetNodeHTML(childnode);
            }
            SubUlHTML += "</ul>";
        }
        if (node["ChildrenDataSource"] != null && node["ChildrenDataSource"] != "")
        {
            SubUlHTML = GetDynamicChildNodesHTML(node["ChildrenDataSource"]);
        }
        liHTML += SubUlHTML;
        liHTML += "</li>";

        return liHTML;
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
