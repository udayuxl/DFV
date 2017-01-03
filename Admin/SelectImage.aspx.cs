using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using Lumen;
using ACE;

public partial class Admin_SelectImage : System.Web.UI.Page
{
    string strProgramID = "0"; 
    string strItemID= "0";       
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["ProgramID"] != null)     
            strProgramID = Request.QueryString["ProgramID"];
        if (Request.QueryString["ItemID"] != null)     
            strItemID = Request.QueryString["ItemID"];
        
        if(!IsPostBack)
            Load();
    }

    protected void GoBack(object sender, EventArgs e)
    {
        if (Request.QueryString["ProgramID"] != null)
        {
            Response.Redirect("ProgramSetUp.aspx");
        }
        else if (Request.QueryString["ItemID"] != null)
        {
           // Response.Redirect("ItemMaster.aspx");
            Response.Redirect("POS_Items.aspx");
        }
        else if (Request.QueryString["InventoryItemID"] != null)
        {
           // Response.Redirect("InventoryItemMaster.aspx");
            Response.Redirect("POS_Items.aspx");
        }
    }
       
    protected void ImagesItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ImageButton btnImage = e.Item.FindControl("btnImage") as ImageButton;
            Literal litName = e.Item.FindControl("litName") as Literal;
            HiddenField hdnImageID = e.Item.FindControl("hdnImageID") as HiddenField;

            btnImage.ImageUrl = "~/ImageLibrary/Thumbnails/" + Convert.ToString(e.Item.DataItem);
            hdnImageID.Value = Convert.ToString(e.Item.DataItem);
            e.Item.Visible = File.Exists(Server.MapPath(btnImage.ImageUrl));

            litName.Text = hdnImageID.Value;
        }
    }

    protected void ChangeImage(object sender, EventArgs e)
    {
        ImageButton btnImage = sender as ImageButton;
        HiddenField hdnImageID = btnImage.Parent.FindControl("hdnImageID") as HiddenField;
        
        string strFileName = btnImage.ImageUrl.Substring(btnImage.ImageUrl.LastIndexOf("/")+1);
        
        string strCmd = "";

        if (strProgramID != "0")
        {
            strCmd = "UPDATE Emerge.tblProgram SET PreviewImageFileName = '" + hdnImageID.Value + "' WHERE pk_Program_Id = " + strProgramID;
        }

        if (strItemID != "0")
        {
            strCmd = "UPDATE Emerge.tblItem SET PreviewImageFileName = '" + hdnImageID.Value + "' WHERE pk_Item_Id = " + strItemID;
        }
        DataOperations.ExecuteSQL(strCmd);
        Load();
    }

    private void Load()
    {
        if(strProgramID != "0")
        {
            string strImageFileName = LookupServices.GetProgramPreviewImageFileName(strProgramID);
            litprgitm.Text = "Program: ";
            litItemOrProgramHeader.Text = "Program ";
            litItemOrProgramName.Text = LookupServices.GetProgramName(strProgramID);
            imgItemOrProgram.ImageUrl = "~/ImageLibrary/Thumbnails/" + strImageFileName;
            litimageName.Text = strImageFileName;
         }

         if (strItemID != "0")
         {
             string strImageFileName = LookupServices.GetItemPreviewImageFileName(strItemID);
             litprgitm.Text = "Item: ";
             litItemOrProgramHeader.Text = "Item ";
             litItemOrProgramName.Text = LookupServices.GetItemName(strItemID);
             imgItemOrProgram.ImageUrl = "~/ImageLibrary/Thumbnails/" + strImageFileName;
             litimageName.Text = strImageFileName;                          
         }

         rptImages.DataSource = GetImageFileInfoList();
         rptImages.DataBind();
     }

    private List<string> GetImageFileInfoList()
    {
        List<string> imagefiles = new List<string>();
        string strPath = Server.MapPath("~/ImageLibrary/Thumbnails");
        DirectoryInfo dirinfo = new DirectoryInfo(strPath);
        foreach (FileInfo finfo in dirinfo.GetFiles("*.jpg"))
        {
            imagefiles.Add(finfo.Name);
        }
        return imagefiles;
    }
}
