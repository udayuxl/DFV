using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ACE;
using Lumen;


//using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;


public partial class Admin_POS_Items : System.Web.UI.Page
{
    string strPreviewImageFileName = "";
    private string strCurrentImageFileUploaded = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {            
            BindGrid();
            BindDropDowns();
        }
    }

    private void BindGrid()
    {
        DataTable dtPOSItems = DataOperations.GetDataTable("EXEC [Emerge].[AdminGetPOSItems]");
        gridPOSItems.DataSource = dtPOSItems;
        gridPOSItems.DataBind();
    }

    private void BindDropDowns()
    {
        popBrands.Items.Add(new ListItem("Select", ""));
        DataTable dtBrands = DataOperations.GetDataTable("SELECT * FROM Emerge.tblBrand");
        popBrands.DataSource = dtBrands;        
        popBrands.DataTextField = "Brand";
        popBrands.DataValueField = "pk_Brand_ID";
        popBrands.DataBind();

        popItemType.Items.Add(new ListItem("Select", ""));
        DataTable dtItemTypes = DataOperations.GetDataTable("SELECT * FROM Emerge.tblItemType");
        popItemType.DataSource = dtItemTypes;        
        popItemType.DataTextField = "Type";
        popItemType.DataValueField = "pk_ItemType_ID";
        popItemType.DataBind();

        /*
         DataTable dtImages = DataOperations.GetDataTable("SELECT * FROM Emerge.tblImageLibrary");
        popImages.DataSource = dtImages;
        popImages.Items.Add(new ListItem("Select", ""));
        popImages.DataTextField = "FileName";
        popImages.DataValueField = "pk_Image_ID";
        popImages.DataBind();
         * */
    }

    protected void gridPOSItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        string strPreviewImageFileName = DataBinder.Eval(e.Row.DataItem, "PreviewImageFileName").ToString();
        string strPreviewImageFileURL = "~/ImageLibrary/Thumbnails/" + strPreviewImageFileName;
        if (!File.Exists(Server.MapPath(strPreviewImageFileURL)))
            strPreviewImageFileURL = "~/ImageLibrary/NoImage.jpg";
        
        ((HiddenField)e.Row.FindControl("hdnImageFileName")).Value = strPreviewImageFileName;

        ((Image)e.Row.FindControl("imgPreview")).ImageUrl = strPreviewImageFileURL;
        ((HyperLink)e.Row.FindControl("lnkChangeImage")).NavigateUrl = "SelectImage.aspx?ItemID=" + DataBinder.Eval(e.Row.DataItem, "pk_Item_ID").ToString();
        ((TextBox)e.Row.FindControl("txtID")).Text = DataBinder.Eval(e.Row.DataItem, "pk_Item_ID").ToString();
        ((HiddenField)e.Row.FindControl("hdnEstimatedPrice")).Value = DataBinder.Eval(e.Row.DataItem, "EstimatedPrice").ToString();
        ((HiddenField)e.Row.FindControl("hdnActualPrice")).Value = DataBinder.Eval(e.Row.DataItem, "ActualPrice").ToString();
        ((HiddenField)e.Row.FindControl("hdnBrandID")).Value = DataBinder.Eval(e.Row.DataItem, "fk_BrandId").ToString();
        ((HiddenField)e.Row.FindControl("hdnItemTypeID")).Value = DataBinder.Eval(e.Row.DataItem, "fk_ItemType_ID").ToString();        
        ((HiddenField)e.Row.FindControl("hdnUnitQty")).Value = DataBinder.Eval(e.Row.DataItem, "UnitQuantity").ToString();
        ((HiddenField)e.Row.FindControl("hdnDimension")).Value = DataBinder.Eval(e.Row.DataItem, "Dimension").ToString();
        ((HiddenField)e.Row.FindControl("hdnDescription")).Value = DataBinder.Eval(e.Row.DataItem, "Description").ToString();
        ((HiddenField)e.Row.FindControl("hdnLowLeveInv")).Value = DataBinder.Eval(e.Row.DataItem, "inventory_threshold").ToString();
        ((HiddenField)e.Row.FindControl("hdnMaxOrderQty")).Value = DataBinder.Eval(e.Row.DataItem, "MaxOrderQuantity").ToString();
        ((HiddenField)e.Row.FindControl("hdnPreOrderItem")).Value = DataBinder.Eval(e.Row.DataItem, "PreOrderItem").ToString();
        ((HiddenField)e.Row.FindControl("hdnInvOrderItem")).Value = DataBinder.Eval(e.Row.DataItem, "InvOrdItem").ToString();
        ((HiddenField)e.Row.FindControl("hdnAppRequired")).Value = DataBinder.Eval(e.Row.DataItem, "approval_required").ToString();

        Literal litItemno = e.Row.FindControl("litItemno") as Literal;
        litItemno.Text = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Item_No"));
        Literal litItemId = e.Row.FindControl("litItemId") as Literal;
        litItemId.Text = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "pk_Item_ID"));
 
    }

    // **** [buttons] ************************* 
    protected void Add(object sender, EventArgs e)
    {
        popAddEditID.Text = "0";
        popItemName.Text = "";
        popItemNo.Text = "";
        //popEstimatedPrice.Text = "0.00";
        popItemType.Text = "";
        popDimension.Text = "";
        popUnitQuantity.Text = "";
        popDesc.Text = "";                
        popBrands.Text = "";
        popPreviewImage.ImageUrl = "";   


    }

    // **** [Row buttons] *************************
    protected void Edit(object sender, EventArgs e)
    {
        ImageButton EditButton = sender as ImageButton;
        GridViewRow row = EditButton.Parent.Parent as GridViewRow;
        popAddEditID.Text = ((TextBox)row.FindControl("txtID")).Text;
        popItemName.Text = row.Cells[1].Text;
        Literal litItemno = row.FindControl("litItemno") as Literal;
        popItemNo.Text = litItemno.Text;
        //popItemNo.Text = row.Cells[2].Text;
        popEstimatedPrice.Text = ((HiddenField)row.FindControl("hdnEstimatedPrice")).Value;
        //popActualPrice.Text = ((HiddenField)row.FindControl("hdnActualPrice")).Value;        
        popDimension.Text = ((HiddenField)row.FindControl("hdnDimension")).Value;
        popUnitQuantity.Text = ((HiddenField)row.FindControl("hdnUnitQty")).Value;
        popDesc.Text = ((HiddenField)row.FindControl("hdnDescription")).Value;


        string brandid = ((HiddenField)row.FindControl("hdnBrandID")).Value;
        string itemtypeid = ((HiddenField)row.FindControl("hdnItemTypeID")).Value;
        if(!String.IsNullOrEmpty(brandid))
            popBrands.Text = brandid;
        if (!String.IsNullOrEmpty(itemtypeid))
            popItemType.Text = itemtypeid;

        string strImageFileName = ((HiddenField)row.FindControl("hdnImageFileName")).Value;
        popHdnPreviewImageFileName.Value = strImageFileName;

        string strPreviewImageFileURL = "~/ImageLibrary/Thumbnails/" + strImageFileName;                
        popPreviewImage.ImageUrl = strPreviewImageFileURL;
        popLowLevelInv.Text = ((HiddenField)row.FindControl("hdnLowLeveInv")).Value;
        popMaxOrdQty.Text = ((HiddenField)row.FindControl("hdnMaxOrderQty")).Value;

        if (((HiddenField)row.FindControl("hdnAppRequired")).Value.ToString().ToLower().Equals("true"))
            popAppRequired.Checked = true;
        else
            popAppRequired.Checked = false;
        if (((HiddenField)row.FindControl("hdnPreOrderItem")).Value.ToString().ToLower().Equals("true"))
            popPreOrderItem.Checked = true;
        else
            popPreOrderItem.Checked = false;
        if (((HiddenField)row.FindControl("hdnInvOrderItem")).Value.ToString().ToLower().Equals("true"))
        {
            popInvOrderItem.Checked = true;
            tdInventory.Visible = true;
        }
        else
        {
            popInvOrderItem.Checked = false;
            tdInventory.Visible = false;
        }


        
    }

    protected void Delete(object sender, EventArgs e)
    {
        ImageButton Button = sender as ImageButton;
        GridViewRow row = Button.Parent.Parent as GridViewRow;
        string strID = ((TextBox)row.FindControl("txtID")).Text;
        string strCmd = "DELETE Emerge.tblItem WHERE pk_Item_ID = " + strID;
        string strResult = "";
        DataOperations.ExecuteSQL(strCmd, out strResult);
        if (strResult == "INUSE")
            failuremsg.Text = "This item cannot be deleted as it is in use.";
        else
        {
            successmsg.Text = "Item deleted.";
            BindGrid();
        }
    }

    // **** [Popup buttons] ************************* 
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

                    string strItemName = popItemNo.Text;
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

            }
        }
        catch (Exception ex)
        {
            LogAndTrace.CodeTrace(ex.ToString());
        }        
    }

    protected void Save(object sender, EventArgs e)
    {
        if (Validate() != "")
            return;

        //Upload the Image and get the image file name into - strCurrentImageFileUploded
        UploadPreviewImage(null, null);        

        string strID = popAddEditID.Text;
        string strPreOrderItem = popPreOrderItem.Checked.ToString();
        string strInvOrderItem = popInvOrderItem.Checked.ToString();
        string strAppRequired = popAppRequired.Checked.ToString();

        /*===============================================*/
        if (strPreOrderItem.ToUpper() == "TRUE")
            strPreOrderItem = "1";
        else
            strPreOrderItem = "0";
        /*===============================================*/
        if (strInvOrderItem.ToUpper() == "TRUE")
            strInvOrderItem = "1";
        else
            strInvOrderItem = "0";
        /*===============================================*/
        if (strAppRequired.ToUpper() == "TRUE")
            strAppRequired = "1";
        else
            strAppRequired = "0";
        /*===============================================*/


        AdminServices.SavePOSItem(strID, popItemName.Text, popItemNo.Text, popEstimatedPrice.Text, popEstimatedPrice.Text, popDimension.Text, popUnitQuantity.Text, popHdnPreviewImageFileName.Value, popBrands.SelectedValue, popItemType.Text, popDesc.Text, popLowLevelInv.Text, popMaxOrdQty.Text, strPreOrderItem, strInvOrderItem, strAppRequired);

        Response.Redirect("POS_Items.aspx");
    }
    protected void Cancel(object sender, EventArgs e)
    {
        popAddEditID.Text = "";
        popItemName.Text = "";
        litValidationError.Text = "";
    }

    // ######### VALIDATION #############
    private string Validate()
    {
        string strErrorMessage = "";
        if (popItemName.Text.Trim() == "")
            strErrorMessage += "Item Name is required <br/>";
        if (popItemNo.Text.Trim() == "")
            strErrorMessage += "Item Number is required <br/>";
        if (popBrands.SelectedItem.Value == "")
            strErrorMessage += "Please select a brand <br/>";
        if (popItemType.SelectedItem.Value == "")
            strErrorMessage += "Please select the Item Type <br/>";

        litValidationError.Text = strErrorMessage;
        return strErrorMessage;
    }

    public void Invitem_Check(Object sender, EventArgs e)
    {
        if(popInvOrderItem.Checked.ToString().ToLower().Equals("true"))
            tdInventory.Visible = true;
        else
            tdInventory.Visible = false;
    }
    protected void btnVendTranCancel_Click(object sender, EventArgs e)
    {
        clrvalues_InVendTranPopupServerControls();
    }

    private void clrvalues_InVendTranPopupServerControls()
    {
        txtTranItemID.Text = "";
        txtTranVendor.Text = "";
        txtTranExpectedQty.Text = "";
        txtTranExpectedDate.Text = "";
        popTranID.Text = "";
    }

    // For View Transaxtions functionality
    protected void ViewTranHist(object sender, EventArgs e)
    {


        LinkButton lnkBtn = (LinkButton)sender;
        GridViewRow grdrow = lnkBtn.Parent.Parent as GridViewRow;
        Literal litItemId = grdrow.FindControl("litItemId") as Literal;
        popTranID.Text = litItemId.Text;

        txtTranItemID.Text = Convert.ToString(popTranID.Text);
        string currItemid = popTranID.Text;

        //txtTranStockNo.Text = dsPOSItem.Tables[0].Rows[0]["Item_No"].ToString();
        //txtTranStockName.Text = dsPOSItem.Tables[0].Rows[0]["ItemName"].ToString();


        string strCmd = "";
        /*
        strCmd = "SELECT TOP 5 CURRINV.fk_Item_ID, CURRINV.Item_No, CURRINV.StockDateTime, ISNULL(CURRINV.WarehouseStock,0) AS Inventory, ISNULL((CURRINV.WarehouseStock - VARI.WarehouseStock),0) AS Variance ";
        strCmd += " FROM [Emerge].[tblWarehouseProxyInventoryItem] CURRINV ";
        strCmd += " LEFT JOIN (SELECT TOP 1 fk_Item_ID, StockDateTime, WarehouseStock FROM [Emerge].[tblWarehouseProxyInventoryItem] WHERE fk_Item_ID=" + currItemid + ") VARI ON (VARI.StockDateTime < CURRINV.StockDateTime ) ";
        strCmd += " WHERE CURRINV.fk_Item_ID=" + currItemid;
        strCmd += " ORDER BY CURRINV.StockDateTime DESC";
        */
        strCmd = "SELECT TOP 5 CURRINV.fk_Item_ID, CURRINV.Item_No, CURRINV.StockDateTime, ISNULL(CURRINV.WarehouseStock,0) AS Inventory, ISNULL((CURRINV.WarehouseStock - VARI.WarehouseStock),0) AS Variance ";
        strCmd += " FROM [Emerge].[tblWarehouseProxyInventoryItem] AS CURRINV ";
        strCmd += " LEFT JOIN [Emerge].[tblWarehouseProxyInventoryItem] AS VARI ON VARI.pkWarehouseProxyInventoryItemID = (SELECT MAX(pkWarehouseProxyInventoryItemID) FROM [Emerge].[tblWarehouseProxyInventoryItem] WHERE pkWarehouseProxyInventoryItemID < CURRINV.pkWarehouseProxyInventoryItemID AND  fk_Item_ID=" + currItemid + ") ";
        strCmd += " WHERE CURRINV.fk_Item_ID=" + currItemid;
        strCmd += " ORDER BY CURRINV.pkWarehouseProxyInventoryItemID DESC ";

        DataTable dtTranHist = DataOperations.GetDataTable(strCmd);
        gridTranHist.DataSource = dtTranHist;
        gridTranHist.DataBind();

        strCmd = "SELECT TOP 5 IT.Vendor, IT.Qty_Expected, CONVERT(VARCHAR(10), IT.Expected_Date, 110) AS Expected_Date, IT.fk_Item_ID, II.Item_No, II.Item_Name, IT.AddedOn FROM [Emerge].[tblItemTransactions] IT LEFT JOIN Emerge.tblItem II ON (II.pk_Item_Id = IT.fk_Item_ID) WHERE IT.fk_Item_ID=" + currItemid + " ORDER BY IT.AddedOn DESC";
        DataTable dtVendTran = DataOperations.GetDataTable(strCmd);
        gridVendTran.DataSource = dtVendTran;
        gridVendTran.DataBind();
    }

    protected void gridTranHist_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

    }
    protected void gridVendTran_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
    }

    protected void btnSaveVendTran_Click(object sender, EventArgs e)
    {
        if (txtTranItemID.Text == "")
            return;
        try
        {
            int isValid = 1;//Validate_PopupServerControls();
            if (isValid == 1)
            {
                string strVend = txtTranVendor.Text.Trim();
                string strExpQty = txtTranExpectedQty.Text.Trim();
                string strExpectedDate = txtTranExpectedDate.Text.Trim();
                string strItemId = txtTranItemID.Text.Trim();

                bool bStatusSave = false;// AdminServices.saveVendTransaction(strVend, strExpQty, strExpectedDate, strItemId);
                string strCommand = "";
                try
                {
                    strCommand += "INSERT INTO [Emerge].[tblItemTransactions] (Vendor, Qty_Expected, Expected_Date, fk_Item_ID, AddedOn) ";
                    strCommand += " VALUES ('" + strVend + "'," + strExpQty + ", '" + strExpectedDate + "'," + strItemId + ", GETDATE()) ";
                    DataOperations.ExecuteSQL(strCommand);
                    bStatusSave = true;
                }
                catch (Exception ex)
                {
                    bStatusSave = false;
                }

                if (bStatusSave == true)
                {
                    successmsg.Text = "Item Transaction Item added/updated successfully";
                    string strCmd = "SELECT pk_Item_ID,Item_No, Item_Name FROM Emerge.tblItem WHERE pk_Item_ID = " + strItemId;

                    DataTable dtPOSItem = DataOperations.GetDataTable(strCmd);
                    string strMailBody = "Hello Amanda,<br/> We are expecting " + strExpQty + " units of Items  " + dtPOSItem.Rows[0]["Item_Name"].ToString() + " on  " + strExpectedDate + " From " + strVend;
                    strMailBody += "<br/>Rodney";


 //                   ACE.EmailEngine.GmailSMTP objGmail = new ACE.EmailEngine.GmailSMTP("emerge@ambood.com", "Lumen2014.1");
                    Lumen.GmailSMTP objGmail = new Lumen.GmailSMTP("emerge@uxlconsulting.com", "uxlt1234", false);

                    objGmail.SendMail("uday@ambood.com", "Vendor Transaction :DFV", strMailBody);

                    strCmd = "";
                    strCmd = "SELECT IT.Vendor, IT.Qty_Expected, CONVERT(VARCHAR(10), IT.Expected_Date, 110) AS Expected_Date, IT.fk_Item_ID, II.Item_No, II.Item_Name FROM [Emerge].[tblItemTransactions] IT LEFT JOIN Emerge.tblItem II ON (II.pk_Item_Id = IT.fk_Item_ID) WHERE IT.fk_Item_ID=" + strItemId;

                    DataTable dtVendTran = DataOperations.GetDataTable(strCmd);
                    gridVendTran.DataSource = dtVendTran;
                    gridVendTran.DataBind();
                    clrvalues_InVendTranPopupServerControls();
                }
                else
                {
                    failuremsg.Text = "Item transaction not added successfully";
                    clrvalues_InVendTranPopupServerControls();
                }
            }
        }
        catch (Exception ex)
        {
            failuremsg.Text = "Failed to Add Item transaction";
        }
    }
}