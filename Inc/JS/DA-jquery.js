$(document).ready(function () {
    doneloading();

    $('div.wait').hide();
    $('div.messagebox').hide();

    AddSuggestivePlaceholders();      

    $("input.IntegerField").keypress(function (evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    });

    $("input.DecimalField").keypress(function (evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        if (charCode == 46 || charCode == 45) return true; //Period(.) and Hyphen allowed is allowed
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    });


    ProcessEditForm();
    ProcessViewTranForm();
    try {
        CustomPageLoad();
    }
    catch (err) {
    }
    ProcessScreenMessages();
    AdjustReportViewer();

    if ($.browser.msie && $.browser.version < 9)
        $("input.triggerautosaveAllocatedQty").blur(function (sender) { SaveInventoryAllocationQtyValue(sender.target, false); }); // Save Inventory Allocation qty
    else
        $("input.triggerautosaveAllocatedQty").change(function (sender) { SaveInventoryAllocationQtyValue(sender.target, false); }); // Save Inventory Allocation qty

    //  $("input.small").change(function (sender) { CheckInventoryAllocationQtyValue(sender.target); }); // Check stock Inventory Allocation qty avaliable for perticual item






    $("input.numericvalidation").keydown(function (event) {

        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 16 || (event.keyCode >= 37 && event.keyCode <= 40)) {
        }
        else {
            if (event.keyCode < 95) {
                if (event.keyCode < 48 || event.keyCode > 57) {
                    event.preventDefault();
                }
            }
            else {
                if (event.keyCode < 96 || event.keyCode > 105) {
                    event.preventDefault();
                }
            }
        }
    });
    /* Added by Deepak for JQuery based Calculation 
    for Sub Total and Order Total */
    $("input.TriggerCalculations").focus(function (obj) {
        if ($(this).val() == "0")
            $(this).val("");
    });
    $("input.TriggerCalculations").blur(function (obj) {
        if ($(this).val() == "")
            $(this).val("0");
    });

    $("input.TriggerCalculations").change(function () {
        DoMath(this, true);
        CalculateTotal(this);
    });

    $("input.TriggerCalculations").each(function () {
        DoMath(this, false);
    });
    /*
    // Start Uday
    $(".ob_gSH").bind('mouseup', function (eventObj) { displayGrid(eventObj); });

    $("input.setscroll").live('keydown', function (eventObj) {
    var keyCode = eventObj.keyCode || eventObj.which;
    if (keyCode == 9) {
    setScroll(eventObj);
    }
    });

    function setScroll(eventObj) {
    strrowid = $(eventObj.target).attr("rowid");
    ColumnIndex = $(eventObj.target).attr('ColumnIndex');
    moveScroll(ColumnIndex, strrowid);
    }

    function displayGrid(eventObj) {
    strrowid = $(eventObj.target).attr("rowid");
    ColumnIndex = $(eventObj.target).attr('ColumnIndex');
    showGrid();
    scrollLeft(strrowid, ColumnIndex);
    }
    // End Uday
    */

});


function doneloading() {
    $('div#LoadingMask').hide();    
}

function runwait() {

    $('div.editform').hide();
    $('div.wait').show();
    $('div.messagebox').hide();
    return true;
}

function runwaittran() {

    $('div.viewtran').hide();
    $('div.wait').show();
    $('div.messagebox').hide();
    return true;
}

function ProcessScreenMessages() {
    var winH = $(window).height();
    var winW = $(window).width();

    successmsg = $('input.successmsg').attr("value");
    failuremsg = $('input.failuremsg').attr("value");
    $('input.failuremsg').attr("value", "");
    $('input.successmsg').attr("value", "");

    if (successmsg != null && successmsg != "") {
        currentstyle = $('div.messagebox').attr("style");
        newstyle = currentstyle.replace(/maroon/g, "green");
        $('div.messagebox').attr("style",newstyle);
        $('div.messagebox div').attr("style", "color:green;");        
        $('div.messagebox div').html(successmsg);
        $('div.messagebox').css('left', winW / 2 - $('div.messagebox').width() / 2);
        $('div.messagebox').show();
        $('div.messagebox').fadeOut(5000);
    }
    if (failuremsg != null && failuremsg != "") {
        $('div.messagebox div').attr("style", "color:red");
        $('div.messagebox div').html(failuremsg);
        $('div.messagebox').css('left', winW / 2 - $('div.messagebox').width() / 2);
        $('div.messagebox').show();
        $('div.messagebox').fadeOut(10000);
    }
}

function ProcessAcePopup(strPopupID) {
    strJQueryID = "div.AcePopup#" + strPopupID;
    acePopup = $(strJQueryPopupID);
    alert(acePopup);

    ID = $('div.editform input.ID').attr("value");
}

function AddSuggestivePlaceholders() {

    $('input.placeholder').each(function () {
        strPlaceHolder = $(this).attr("placeholdertext");        
        if ($(this).val() == "") {
            $(this).val(strPlaceHolder);
            $(this).css("color", "#777");
        }
    });    

    $('input.placeholder').focus(function () {
        strPlaceHolder = $(this).attr("placeholdertext");
        $(this).css("color", "#000");
        if ($(this).val() == strPlaceHolder)
            $(this).val("");
    });

    $('input.placeholder').blur(function () {
        strPlaceHolder = $(this).attr("placeholdertext");
        if ($(this).val() == "") {
            $(this).val(strPlaceHolder);
            $(this).css("color", "#777");
        }
    });


}

function ProcessEditForm() {
    editform = $('div#editform');
    mask = $('div#ModalPopupMask');
    ID = $('div#editform input.ID').attr("value");
    //alert("!"+ID+"!"); //ID is the Primary Key of the master table being edited
    
    if (ID != null && ID != "") {
        $(editform).css("z-index", "10");
        $(editform).show();
        $(mask).show();

        AddSuggestivePlaceholders();

        try 
        {
            PopupPageLoad();            
        }
        catch (err) 
        {
        }

        //        $(editform).draggable();
        //        mask = $('div.ModalBackground');
        //        var maskHeight = $(document).height();
        //        var maskWidth = $(window).width();
        //        var winH = $(window).height();
        //        var winW = $(window).width();

        mask.height(maskHeight);
        mask.width(maskWidth);

        $(editform).css('top', winH / 2 - $(editform).height() / 2);
        $(editform).css('left', (960 - $(editform).width()) / 2);

    }
    else {
        $(editform).hide();
        $(mask).hide();
    }
}

function ProcessViewTranForm() {
    //ViewTransPopup
    viewtran = $('div#viewtran');
    mask = $('div#ModalPopupMask');
    TRANID = $('div#viewtran input.TRANID').attr("value");
    // alert("!" + TRANID + "!"); //TRANID is the Primary Key of the master table being edited

    if (TRANID != null && TRANID != "") {
        $(viewtran).css("z-index", "10");
        $(viewtran).show();
        $(mask).show();

        AddSuggestivePlaceholders();

        try {
            PopupPageLoad();
        }
        catch (err) {
        }
        try {
            mask.height(maskHeight);
            mask.width(maskWidth);
        }
        catch (err) {
        }
        var winH = $(window).height();
        if ($(viewtran).height() > 1)
            $(viewtran).css('top', winH / 2 - $(viewtran).height() / 2);
        else
            $(viewtran).css('top', winH / 2);
        if($(viewtran).width() >0)
            $(viewtran).css('left', (960 - $(viewtran).width()) / 2);
        else
            $(viewtran).css('left', (960) / 2);
    }
    else {
        $(viewtran).hide();
        $(mask).hide();
    }
}
/************************ Code for Row Selected Colors ************************* */

var previousRow;
var previousRowColor;
function ChangeRowColor(row) {
    if (previousRow == row)
        return;
    else if (previousRow != null)
        document.getElementById(previousRow).style.backgroundColor = previousRowColor;
    previousRowColor = document.getElementById(row).style.backgroundColor;
    document.getElementById(row).style.backgroundColor = "#FFDDAA";
    previousRow = row;

}

var arrAllQty = new Array();
function SaveInventoryAllocationQtyValue(txtBox, bRecursive) {
   // debugger;
    $(txtBox).css("background", "yellow");
   // strBrandID = $(txtBox).attr("brandid");
    strItemID = $(txtBox).attr("itemid");
    strUserID = $(txtBox).attr("userid");
    strrowid = $(txtBox).attr("rowid");
    strQtyToSave = txtBox.value;
    strServerURL = "../InventoryAllocationAJAXServer.aspx";
    strSenderid = "InventoryAllocation";
    var IAQtyExceedsError = CalculateTotalUnallocatedStock(strrowid)
    if (!IAQtyExceedsError) {
        $.post(strServerURL, { SenderID: strSenderid, ItemId: strItemID, UserID: strUserID, Quantity: strQtyToSave },
           function (data) {
               if (data == "True")
                   $(txtBox).css("background", "white");
               else {
                   $(txtBox).css("background", "pink");
               }
           });
           SaveArrAllocatedQtyValue();
       }
       else {          
           if (!bRecursive)
               arrAllQty[arrAllQty.length] = txtBox;
           $(txtBox).css("background", "pink");

       }
   }

   function removeByElement(arrayName, arrayElement) {
       for (var i = 0; i < arrayName.length; i++) {
           if (arrayName[i] == arrayElement)
               arrayName.splice(i, 1);
       }
   }
   
      

   function SaveArrAllocatedQtyValue() {
       if (arrAllQty.length > 0) {
           var objTxtBox = arrAllQty[arrAllQty.length - 1];
           removeByElement(arrAllQty, objTxtBox);
           SaveInventoryAllocationQtyValue(objTxtBox, true);
       }
   }

   function PrintOnDemandLoadTemplate(ItemID, nAttribute1, nAttribute2, objPreviewImageControl) {
       // debugger;
       strItemID = ItemID;
       strServerURL = "../PrintOnDemandAJAXServer.aspx";
       strSenderid = "PrintOnDemand_LoadTemplate";
       $.post(strServerURL, { SenderID: strSenderid, ItemId: strItemID, Attribute1: nAttribute1, Attribute2: nAttribute2 },
        function (data) {
            //alert(data.toString());
            objPreviewImageControl.src = data.toString().replace("~","..");
        });
    }

    function PrintOnDemandLoadComboId(ItemID, nAttribute1, nAttribute2, objHiddenComboIDControl, nOrderID, objSpanExistTempletError, lstINPUTsQtyTxtBox) {
       // debugger;   
        strItemID = ItemID;
        strServerURL = "../PrintOnDemandAJAXServer.aspx";
        strSenderid = "PrintOnDemand_LoadComboID";
        $.post(strServerURL, { SenderID: strSenderid, ItemId: strItemID, Attribute1: nAttribute1, Attribute2: nAttribute2, OrderID: nOrderID },
        function (data) {
            // debugger
            objHiddenComboIDControl.value = data.toString();
            // old code || data.toString() == "0"
            if (data.toString() == "-1" ) { 
                if (data.toString() == "-1")
                    objSpanExistTempletError.innerHTML = 'You have already chosen this combination.';
                //                else if (data.toString() == "0")
                //                    objSpanExistTempletError.innerHTML = 'Selected combination does not exist. Please select a different combination.';
                for (var i = 0; i < lstINPUTsQtyTxtBox.length; i++) {
                    if (lstINPUTsQtyTxtBox[i].type == "text" && $(lstINPUTsQtyTxtBox[i]).attr('class') == 'small') {
                        $(lstINPUTsQtyTxtBox[i]).val(0);
                        $(lstINPUTsQtyTxtBox[i]).attr("disabled", "disabled");
                    }
                }
            }
            else {
                objSpanExistTempletError.innerHTML = '';
                for (var i = 0; i < lstINPUTsQtyTxtBox.length; i++) {
                    if (lstINPUTsQtyTxtBox[i].type == "text" && $(lstINPUTsQtyTxtBox[i]).attr('class') == 'small') {
                        $(lstINPUTsQtyTxtBox[i]).removeAttr("disabled");
                    }
                }
            }
        });
    }

    function validateAvailableInventory(txtBox, nTotalQty, objErrorLabel) {
        // debugger;
        var iStockAvailable = 0;
        strItemID = $(txtBox).attr("itemid");
        strServerURL = "../InventoryAllocationAJAXServer.aspx";
        strSenderid = "AvailableInventory";
        strQty = txtBox.value;
        //Checks only when order qty grater than zero or Already error occure and revert back that user control only.
        if (strQty > 0 || objErrorLabel.innerHTML.length > 0) {
            $.post(strServerURL, { SenderID: strSenderid, ItemId: strItemID },
               function (data) {
                   iStockAvailable = parseInt(data);
                   if (nTotalQty > iStockAvailable) {
                       //objErrorLabel.innerHTML = 'Total Order Qty (' + nTotalQty + ') cannot exceed current stock (' + iStockAvailable + ') inventory available.'
                       objErrorLabel.innerHTML = 'Ordered Qty (' + nTotalQty + ') is more than stock available (' + iStockAvailable + '). Please enter a lower qty.'
                   }
                   else {
                       objErrorLabel.innerHTML = '';
                   }
               });
            //return iStockAvailable;  
        }
    }
        function validateAvailableInventory_grdView(txtBox, nTotalQty, objErrorLabel, arrTDCurrentItem) {
           // debugger;
            var iStockAvailable = 0;
            strItemID = $(txtBox).attr("itemid");
            strServerURL = "../InventoryAllocationAJAXServer.aspx";
            strSenderid = "AvailableInventory";
            strQty = txtBox.value;
            if (strQty > 0) {
                $.post(strServerURL, { SenderID: strSenderid, ItemId: strItemID },
               function (data) {
                   iStockAvailable = parseInt(data);
                   if (nTotalQty > iStockAvailable) {
                       objErrorLabel.innerHTML = '*  Some of the order quantities exceeds Allocated Qty / Max Order Qty / Available Inventory.';

                       arrInvalidQtyItems[arrInvalidQtyItems.length] = strItemID;
                       for (var nArrElement = 0; nArrElement < arrTDCurrentItem.length; nArrElement++) {
                           //Change the TD back color.
                           arrTDCurrentItem[nArrElement].bgColor = '#FFAAAA';
                           var nStockAvailable = 0;
                           if (iStockAvailable > 0)
                               nStockAvailable = iStockAvailable;
                           $(arrTDCurrentItem[nArrElement]).parent().attr("title", 'Available Inventory : ' + nStockAvailable);
                       }


                   }
                   else {
                       if (arrInvalidQtyItems.length = 0)
						objErrorLabel.innerHTML = '';

                       removeArrayElement(arrInvalidQtyItems, strItemID);
                       for (var nArrElement = 0; nArrElement < arrTDCurrentItem.length; nArrElement++) {
                           //Change the TD back color back to white.
                           arrTDCurrentItem[nArrElement].bgColor = '#FFFFFF';
                           $(arrTDCurrentItem[nArrElement]).parent().attr("title", "");
                       }

                   }
               });
            }
       }


       function ProcessThemeImage(Action) {
           ThemeImage = $('div.ThemeImage');
           ThemeImage.draggable();
           ID = $('div.ThemeImage input.ID').attr("value");

           if (Action == "Show") {
               $(ThemeImage).show();

               mask = $('div.ModalBackground');
               var maskHeight = $(document).height();
               var maskWidth = $(window).width();
               var winH = $(window).height();
               var winW = $(window).width();
               var winS = $(window).scrollTop() + Math.floor($(window).height() / 2)

               mask.height(maskHeight);
               mask.width(maskWidth);
                             
//                $(ThemeImage).css("position", "absolute");
//                $(ThemeImage).css('top', winH - ($(ThemeImage).height() / 2) + winS);
//                               $(ThemeImage).css('left', (960 - $(ThemeImage).width()) / 2);

               $(ThemeImage).css("position", "fixed");
               $(ThemeImage).css('top', '2%');
               $(ThemeImage).css('left', '40%');
              
           }
           else
               $(ThemeImage).hide();

       }

       function ValidateLoginUserBrandManager() {
           // debugger;          
          
           strServerURL = "../InventoryAllocationAJAXServer.aspx";
           strSenderid = "ValidateUser";
           var funcReturned = null;
           $.ajaxSetup({ async: false });

           $.post(strServerURL, { SenderID: strSenderid },
              function (data) {
                  if (data == "True") {
                      funcReturned = true;
                  }
                  else {
                      funcReturned = false;
                  }
              });

              $.ajaxSetup({ async: true });
              return funcReturned;
       }
       
	   //Wrapper function for parseInt Base 10 function - LBK
       function parseInt10(sValue) {
           return parseInt(sValue, 10);
       }

       /*Adjust the ReportViewer width and height  */
       function AdjustReportViewer() {
           screenwidth = $(window).width();
           screenheight = $(window).height();
           ReportViewerwidth = screenwidth - 270;
           ReportViewerheight = screenheight - 300;
           //$("div.ReportViewer").css("width", ReportViewerwidth + "px");
           //$("div.ReportViewer").css("height", ReportViewerheight + "px");
           $("div.divReportViewer").css("height", ReportViewerheight + "px");

       }
       
       function SameCombinationColors(nStatus, objSpanExistTempletError, lstINPUTsQtyTxtBox) {
           var numericStatus = parseInt(nStatus);
           if (numericStatus == 0) {
               objSpanExistTempletError.innerHTML = 'Background Color and Trim Color cannot be the same.';
               for (var i = 0; i < lstINPUTsQtyTxtBox.length; i++) {
                   if (lstINPUTsQtyTxtBox[i].type == "text" && $(lstINPUTsQtyTxtBox[i]).attr('class') == 'small') {
                       $(lstINPUTsQtyTxtBox[i]).val(0);
                       $(lstINPUTsQtyTxtBox[i]).attr("disabled", "disabled");
                   }
               }
           }
           else {
               objSpanExistTempletError.innerHTML = '';
               for (var i = 0; i < lstINPUTsQtyTxtBox.length; i++) {
                   if (lstINPUTsQtyTxtBox[i].type == "text" && $(lstINPUTsQtyTxtBox[i]).attr('class') == 'small') {
                       $(lstINPUTsQtyTxtBox[i]).removeAttr("disabled");
                   }
               }
           }
       }
        
       