using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Lumen;
using ACE;

public partial class Admin_ProgramItems : System.Web.UI.Page
{
    
    string strProgramID = "0";
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["ProgramID"] != null)
            strProgramID = Request.QueryString["ProgramID"];

        if (!IsPostBack)
        {            
            LoadData();            
        }
    }

    private void BindGrid_ItemsInProgram(string sortExpression, string direction)
    {
        DataTable dt = AdminServices.GetItemsInProgram(strProgramID);
        DataView dvIIP = new DataView(dt);
        dvIIP.Sort = sortExpression + direction;
        grdItemInProg.DataSource = dvIIP;
        grdItemInProg.DataBind();        
    }

    private void BindGrid_ItemsAvailable(string sortExpression, string direction)
    {
        //  You can cache the DataTable for improving performance 
        DataTable dt = AdminServices.GetItemsAvailableForProgram(strProgramID);
        DataView dvIA = new DataView(dt);
        dvIA.Sort = sortExpression + direction;
        grdItemAvlb.DataSource = dvIA;
        grdItemAvlb.DataBind();

    }

    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }

    protected void grdItemsInProgram_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            BindGrid_ItemsInProgram(sortExpression, DESCENDING);
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            BindGrid_ItemsInProgram(sortExpression, ASCENDING);
        }

    }

    protected void grdItemsAvailable_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            BindGrid_ItemsAvailable(sortExpression, DESCENDING);
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            BindGrid_ItemsAvailable(sortExpression, ASCENDING);
        }

    }

    private void LoadData()
    {
        BindGrid_ItemsInProgram("", "");
        BindGrid_ItemsAvailable("", "");
        litProgramName.Text = Convert.ToString(LookupServices.GetProgramName(strProgramID).ToString());
        litItemsInProgram.Text = " Items in Program - " + Convert.ToString(LookupServices.GetProgramName(strProgramID).ToString());
    }
       
       
    protected void ItemInProgram_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal litItmName = e.Row.FindControl("litItmName") as Literal;
            litItmName.Text = DataBinder.Eval(e.Row.DataItem, "Item").ToString();

            Literal litItmBrand = e.Row.FindControl("litItmBrand") as Literal;
            litItmBrand.Text = DataBinder.Eval(e.Row.DataItem, "Brand").ToString();

            Literal litItmDims = e.Row.FindControl("litItmDims") as Literal;
            litItmDims.Text = DataBinder.Eval(e.Row.DataItem, "Dimension").ToString();

            Literal litItmpakof = e.Row.FindControl("litItmpakof") as Literal;
            litItmpakof.Text = DataBinder.Eval(e.Row.DataItem, "PackOf").ToString();

            Literal litItmPri = e.Row.FindControl("litItmPri") as Literal;
            litItmPri.Text = DataBinder.Eval(e.Row.DataItem, "Price").ToString();

            HiddenField hdnItemID = e.Row.FindControl("hdnItemID") as HiddenField;
            hdnItemID.Value = DataBinder.Eval(e.Row.DataItem, "ItemID").ToString();

        }
    }
    protected void ItemAvailable_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal litItmAvlName = e.Row.FindControl("litItmAvlName") as Literal;
            litItmAvlName.Text = DataBinder.Eval(e.Row.DataItem, "Item").ToString();

            Literal litItmAvlBrand = e.Row.FindControl("litItmAvlBrand") as Literal;
            litItmAvlBrand.Text = DataBinder.Eval(e.Row.DataItem, "Brand").ToString();

            Literal litItmAvlDims = e.Row.FindControl("litItmAvlDims") as Literal;
            litItmAvlDims.Text = DataBinder.Eval(e.Row.DataItem, "Dimension").ToString();

            Literal litItmAvlpakof = e.Row.FindControl("litItmAvlpakof") as Literal;
            litItmAvlpakof.Text = DataBinder.Eval(e.Row.DataItem, "PackOf").ToString();

            Literal litItmAvlPri = e.Row.FindControl("litItmAvlPri") as Literal;
            litItmAvlPri.Text = DataBinder.Eval(e.Row.DataItem, "Price").ToString();

            HiddenField hdnItemID = e.Row.FindControl("hdnItemID") as HiddenField;
            hdnItemID.Value = DataBinder.Eval(e.Row.DataItem, "ItemID").ToString();
        }
    }

    protected void grdItemsAvailable_PageIndexChanging(Object sender, GridViewPageEventArgs e)
    {
        grdItemAvlb.PageIndex = e.NewPageIndex;
        LoadData();
    }

    protected void grdItemInProgram_PageIndexChanging(Object sender, GridViewPageEventArgs e)
    {
        grdItemInProg.PageIndex = e.NewPageIndex;
        LoadData();
    }

    protected void AddItem(object sender, EventArgs e)
    {
        ImageButton imgBtnAdd = sender as ImageButton;
        GridViewRow thisRow = imgBtnAdd.Parent.Parent as GridViewRow;
        HiddenField hdnItemID = thisRow.FindControl("hdnItemID") as HiddenField;
        string strItemID = hdnItemID.Value;

        //string strCmd = "INSERT INTO Emerge.tblProgramItem VALUES(" + strProgramID + "," + strItemID + ")";

        string strCmd = "IF NOT EXISTS ( SELECT * FROM Emerge.tblProgramItem WHERE fk_Program_Id = " + strProgramID + " AND fk_Item_Id = "+strItemID+") ";
        strCmd += "BEGIN ";
        strCmd += "INSERT INTO Emerge.tblProgramItem (fk_Program_Id, fk_Item_Id) VALUES(" + strProgramID + "," + strItemID + ") ";
        strCmd += "END ";
        DataOperations.ExecuteSQL(strCmd);
        
        LoadData();
    }

    protected void RemoveItem(object sender, EventArgs e)
    {
        ImageButton imgBtnRemove = sender as ImageButton;
        GridViewRow thisRow = imgBtnRemove.Parent.Parent as GridViewRow;
        HiddenField hdnItemID = thisRow.FindControl("hdnItemID") as HiddenField;
        string strItemID = hdnItemID.Value;

        string strCmd = "DELETE Emerge.tblProgramItem WHERE fk_Item_Id = " + strItemID + " AND fk_Program_Id = " + strProgramID ;
        DataOperations.ExecuteSQL(strCmd);
        
        LoadData();
    }

    protected void GotoProgramSetup(object sender, EventArgs e)
    {
        Response.Redirect("ProgramSetup.aspx");
    }
}