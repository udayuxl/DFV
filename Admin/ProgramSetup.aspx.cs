using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Lumen;

//using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;

public partial class ProgramSetup : System.Web.UI.Page
{
    string strPreviewImageFileName="";

    int Pageindex = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadData();
            // Code for Page x of y
            //PageInfoTopprogram.Text = "Page: " + (gridPrograms.PageIndex + 1).ToString() + " of " + gridPrograms.PageCount.ToString();
        }
    }
    protected void Program_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Image imgPreview = e.Row.FindControl("imgPreview") as Image;
            Literal litProgram = e.Row.FindControl("litProgram") as Literal;
            Literal litDescription = e.Row.FindControl("litDescription") as Literal;
            Literal litStartDate = e.Row.FindControl("litStartDate") as Literal;
            Literal litEndDate = e.Row.FindControl("litEndDate") as Literal;
            Literal litMarketDate = e.Row.FindControl("litMarketDate") as Literal;
            Literal litItemCount = e.Row.FindControl("litItemCount") as Literal;
            HiddenField hdnProgramID = e.Row.FindControl("hdnProgramID") as HiddenField;
            HyperLink lnkChangeItems = e.Row.FindControl("lnkChangeItems") as HyperLink;
            HyperLink lnkChangeImage = e.Row.FindControl("lnkChangeImage") as HyperLink;
            

            imgPreview.ImageUrl = "~/ImageLibrary/Thumbnails/" + DataBinder.Eval(e.Row.DataItem, "PreviewImageFileName").ToString();
            litProgram.Text = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
            litDescription.Text = DataBinder.Eval(e.Row.DataItem, "Description").ToString();
            litStartDate.Text = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "OrderWindowStart")).ToString("MM/dd/yyyy");
            litEndDate.Text = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "OrderWindowEnd")).ToString("MM/dd/yyyy");
            litMarketDate.Text = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "InMarketDate")).ToString("MM/dd/yyyy");
            litItemCount.Text = AdminServices.GetItemCountInProgram(DataBinder.Eval(e.Row.DataItem, "pk_Program_ID").ToString()).ToString();
            hdnProgramID.Value = DataBinder.Eval(e.Row.DataItem, "pk_Program_ID").ToString();
            lnkChangeItems.NavigateUrl = "ProgramItems.aspx?ProgramID=" + DataBinder.Eval(e.Row.DataItem, "pk_Program_ID").ToString();
            lnkChangeImage.NavigateUrl = "SelectImage.aspx?ProgramID=" + DataBinder.Eval(e.Row.DataItem, "pk_Program_ID").ToString();

            if(System.IO.File.Exists(Server.MapPath(imgPreview.ImageUrl)))
                lnkChangeImage.Text = "Change Image";
        }
    }

    private void LoadData()
    {
        DataTable dtPrograms;
        dtPrograms = AdminServices.GetPrograms();

        gridPrograms.DataSource = dtPrograms;
        gridPrograms.DataBind();
    }

    protected void Cancel(object sender, EventArgs e)
    {
        txtpuProgramID.Text = "";
        txtpuprogramName.Text = "";
        txtpuDesc.Text = "";
        txtpuStartDate.Text = "";
        txtpuClsDate.Text = "";
        txtpuMarketDate.Text = "";
        LoadData();
    }

    protected void Edit(object sender, EventArgs e)
    {
        litValidationError.Text = "";
        ImageButton imgBtnEdit = (ImageButton)sender;
        GridViewRow git = imgBtnEdit.Parent.Parent as GridViewRow;
        Literal litProgram = git.FindControl("litProgram") as Literal;
        Literal litDescription = git.FindControl("litDescription") as Literal;
        Literal litStartDate = git.FindControl("litStartDate") as Literal;
        Literal litEndDate = git.FindControl("litEndDate") as Literal;
        Literal litMarketDate = git.FindControl("litMarketDate") as Literal;
        Image imgPreview = git.FindControl("imgPreview") as Image;

        HiddenField hdnProgramID = git.FindControl("hdnProgramID") as HiddenField;
        
        txtpuprogramName.Text = litProgram.Text;
        txtpuDesc.Text = litDescription.Text;
        txtpuStartDate.Text = litStartDate.Text;
        txtpuClsDate.Text = litEndDate.Text;
        txtpuMarketDate.Text = litMarketDate.Text;
        txtpuProgramID.Text = hdnProgramID.Value;
        popPreviewImage.ImageUrl = imgPreview.ImageUrl;
        popHdnPreviewImageFileName.Value = imgPreview.ImageUrl.Substring(imgPreview.ImageUrl.LastIndexOf("/")+1) ;        

    }

    protected void Delete(object sender, EventArgs e)
    {
        ImageButton imgBtnDelete = (ImageButton)sender;
        GridViewRow git = imgBtnDelete.Parent.Parent as GridViewRow;
        HiddenField hdnProgramID = git.FindControl("hdnProgramID") as HiddenField;
        string strResult;
        AdminServices.DeleteProgram(hdnProgramID.Value,out strResult);
        if(strResult == "INUSE")
            failuremsg.Text = "This Program cannot be deleted as it is in use.";
        else
        {
            LoadData();
            successmsg.Text = "Program deleted";
        }        
    }

    protected void UploadPreviewImage(object sender, EventArgs e)
    {
        if (strPreviewImageFileName != "")
            return;

        string strFilePath = "";
        
        try
        {
            if (popFileUploadPreviewImage.HasFile)
            {
                string strFilename = popFileUploadPreviewImage.FileName;

                if (Path.GetExtension(strFilename).ToLower() == ".jpg" || Path.GetExtension(strFilename).ToLower() == ".png" || Path.GetExtension(strFilename).ToLower() == ".bmp" || Path.GetExtension(strFilename).ToLower() == ".jpeg")
                {
                    //save the  uploaded file to the library
                    if (!Directory.Exists(MapPath(@"~\ImageLibrary\Thumbnails")))
                        Directory.CreateDirectory(MapPath(@"~\ImageLibrary\Thumbnails"));

                    if (!Directory.Exists(MapPath(@"~\ImageLibrary\Large")))
                        Directory.CreateDirectory(MapPath(@"~\ImageLibrary\Large"));

                    //Guid guidFileName = Guid.NewGuid();

                    string strItemName = txtpuprogramName.Text.Trim();
                    if (strItemName == "") strItemName = "PreviewProgram";

                    strPreviewImageFileName = strItemName; 
                    string directoryThumbNail = Server.MapPath(@"~\ImageLibrary\Thumbnails\");
                    string directoryLarge = Server.MapPath(@"~\ImageLibrary\Large\");

                    System.Drawing.Bitmap bmpUploaded = new System.Drawing.Bitmap(popFileUploadPreviewImage.FileContent);
                    strFilePath = directoryLarge + "" + strPreviewImageFileName + ".bmp";
                    bmpUploaded.Save(strFilePath);

                    int origWidth = bmpUploaded.Width;
                    int origHeight = bmpUploaded.Height;

                    decimal AspectRatio = (decimal)origWidth / (decimal)origHeight;
                    
                    using (System.Drawing.Image img = System.Drawing.Image.FromFile(directoryLarge + "" + strPreviewImageFileName + ".bmp"))
                    {
                        img.Save(directoryLarge + "" + strPreviewImageFileName + ".jpg", ImageFormat.Jpeg);
                    }
                    //Deleting original BMP file
                    File.Delete(directoryLarge + "" + strPreviewImageFileName + ".bmp");

                    //Thumbnail creation
                    int thumbHeight = 0;
                    int thumbWidth = 0;
                    decimal temp;

                    if (AspectRatio < 0.67M) //tall - make height as 150 and calculate the width
                    {
                        thumbHeight = 150;
                        temp = 150.0M * AspectRatio;
                        thumbWidth = (int)temp;
                    }
                    else
                    {
                        thumbWidth = 100;
                        temp = 100.0M / AspectRatio;
                        thumbHeight = (int)temp;
                    }

                    System.Drawing.Bitmap bmpThumbnail = new System.Drawing.Bitmap(bmpUploaded, thumbWidth, thumbHeight);
                    bmpThumbnail.Save(directoryThumbNail + "" + strPreviewImageFileName + ".bmp");

                    using (System.Drawing.Image img = System.Drawing.Image.FromFile(directoryThumbNail + "" + strPreviewImageFileName + ".bmp"))
                    {
                        img.Save(directoryThumbNail + "" + strPreviewImageFileName + ".jpg", ImageFormat.Jpeg);
                    }
                    //Deleting thumbnail BMP file
                    File.Delete(directoryThumbNail + "" + strPreviewImageFileName + ".bmp");

                    bmpUploaded.Dispose();
                    bmpThumbnail.Dispose();

                    // Show the thumbnail image in the preview
                    popPreviewImage.ImageUrl = "~/ImageLibrary/Thumbnails/" + strPreviewImageFileName + ".jpg";
                    popHdnPreviewImageFileName.Value = strPreviewImageFileName + ".jpg";
                    
                    //end of saving the file to the library
                }
                LogAndTrace.CodeTrace(strFilePath + " uploaded " );
            }
        }
        catch (Exception ex)
        {
            LogAndTrace.CodeTrace(strFilePath + " - " + ex.ToString(), Page);
        } 
    }

    protected void Add(object sender, EventArgs e)
    {
        litValidationError.Text = "";
        txtpuProgramID.Text = "0";
    }

    // Paging Code
    protected void grdProgram_PageIndexChanging(Object sender, GridViewPageEventArgs e)
    {
         
        gridPrograms.PageIndex = e.NewPageIndex;
        LoadData();
    }
    protected void Save(object sender, EventArgs e)
    {
        if (Validate() != "")
            return;

        if(txtpuProgramID.Text == "")
            return;
        try
        {
            UploadPreviewImage(null,null);

            if (AdminServices.SaveProgram(txtpuProgramID.Text, txtpuprogramName.Text, txtpuDesc.Text, popHdnPreviewImageFileName.Value , txtpuStartDate.Text, txtpuClsDate.Text, txtpuMarketDate.Text))
                successmsg.Text = "Program created successfully";

            txtpuProgramID.Text = "";
            txtpuprogramName.Text = "";
            txtpuDesc.Text = "";
            txtpuStartDate.Text = "";
            txtpuClsDate.Text = "";
            txtpuMarketDate.Text = "";
            txtpuProgramID.Text = "";
            
            LoadData();
            
        }
        catch (Exception ex)
        {
            failuremsg.Text = "There was an error in saving. An administrator has been notified";
        }
    }

    // ######### VALIDATION #############
    private string Validate()
    {
        string strErrorMessage = "";
        DateTime dtStartDate = DateTime.Now;
        DateTime dtEndDate = DateTime.Now;
        DateTime dtMarketDate = DateTime.Now;
        
        if (txtpuprogramName.Text.Trim() == "")
            strErrorMessage += "Program Name is required <br/>";

        if (txtpuStartDate.Text.Trim() == "")
            strErrorMessage += "Start Date is required <br/>";
        else
            dtStartDate = Convert.ToDateTime(txtpuStartDate.Text.Trim());
        
        if (txtpuClsDate.Text.Trim() == "")
            strErrorMessage += "Closing Date is required <br/>";
        else
            dtEndDate = Convert.ToDateTime(txtpuClsDate.Text.Trim());
        
        if (txtpuMarketDate.Text.Trim() == "")
            strErrorMessage += "In Market Date is required <br/>";
        else
            dtMarketDate = Convert.ToDateTime(txtpuMarketDate.Text.Trim());
        
        if(dtEndDate < dtStartDate)
            strErrorMessage += "Closing Date cannot be earlier than Start Date <br/>";

        if (dtMarketDate < dtEndDate)
            strErrorMessage += "In Market Date cannot be earlier than Closing Date <br/>";

        litValidationError.Text = strErrorMessage;
        return strErrorMessage;
    }
}