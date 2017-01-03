<%@ Page Title="" Language="C#" MasterPageFile="~/LumenApp.master" AutoEventWireup="true" CodeFile="ProgramSetup.aspx.cs" Inherits="ProgramSetup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    <h2>Ad Hoc Programs</h2>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" Runat="Server">
 <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"/>
   <style type="text/css">     
     
     .mGrid a {color:Blue; text-decoration:underline; }
     table tr td{margin:-5px;}
     span.textdesign{font-weight:bold;color:#620937;}
     
     
     .btnaddprograms{float: right; margin-right: 26px; margin-top: 10px;}
     input#txtpuprogramName{padding-left:10px;}
     .calextender{width:150px;padding-left:40px;}
     .MyCalendar .ajax__calendar_container { border:2px solid #ED017F; background-color:#B7A521; color:#620937; margin-left:160px;}
     .pgr span{color:#ED017F;}
     .startdateerror{color:Red;}
     
     /* CSS For Date Error messages
     .closedateerror{margin-top:-67px;float:left;}*/
     
 </style>
 <!-- Script for disable previous date-->
 <script language="javascript">
     function checkDate(sender, args) {
         var toDate = new Date();
         toDate.setMinutes(0);
         toDate.setSeconds(0);
         toDate.setHours(0);
         toDate.setMilliseconds(0);
         document.getElementById('ctl00_cphMain_Errormsg').innerHTML = "";
         if (sender._selectedDate < toDate) {
             //alert("Please select a valid Start Date.");
             //document.getElementById('ctl00_cphMain_Errormsg').innerHTML = "Please select a valid Start Date.";
             //sender._selectedDate = toDate;
             //set the date back to the current date
             //sender._textbox.set_Value(sender._selectedDate.format(sender._format))
         }
     } 
 </script>

 
    
    <!-- ++++++++ Buttons for grid +++++++++ -->
    <div class="GridButtons" style="width:750px">
        <asp:Button ID="btnAddProgram" runat="server" Text="Add" OnClientClick="runwait();" OnClick="Add" CssClass="btnaddprograms"/>  
    </div>
    <div style="margin-bottom:10px;clear:both"></div>        
    <!-- +++++++++++++++++++++++++++++++++++ -->

          

    <!-- ########## THE GRID ######### -->
    <asp:GridView ID="gridPrograms" runat="server" AutoGenerateColumns="false" EmptyDataText = "There are no programs."
            OnRowDataBound="Program_RowDataBound"  
            CssClass="mGrid"            AlternatingRowStyle-CssClass="alt"            PagerStyle-CssClass="pgr"        GridLines="None"
            PageSize="20"         OnPageIndexChanging="grdProgram_PageIndexChanging"        AllowPaging="true" >
        <Columns>
            <asp:TemplateField HeaderText="Preview"  ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" >
                <ItemTemplate>                    
                    <asp:HiddenField ID="hdnProgramID" runat="server" />                    
                    <asp:Image runat="server" ID="imgPreview" />
                    <br />
                    <asp:HyperLink Text="Select Image" runat="server" ID="lnkChangeImage" CssClass="lnkbutton" ></asp:HyperLink>                    
                </ItemTemplate>
            </asp:TemplateField>
        
            <asp:TemplateField HeaderText="Program" ItemStyle-VerticalAlign="Top" ItemStyle-Width="215px">
                <ItemTemplate>                                        
                    <b><asp:Literal ID="litProgram" runat="server"></asp:Literal></b>
                    <br /><br />
                    <asp:Literal ID="litDescription" runat="server"></asp:Literal>
                    <br />
                </ItemTemplate>
            </asp:TemplateField>
       
            <asp:TemplateField  HeaderText="Ordering Window" ItemStyle-VerticalAlign="Top" ItemStyle-Width="180px">
                <ItemTemplate >                                        
                    Start Date:<asp:Literal ID="litStartDate" runat="server"></asp:Literal>
                    <br /><br />
                    End Date:<asp:Literal ID="litEndDate" runat="server"></asp:Literal>
                    <br /><br />
                    In-Market Date:<asp:Literal ID="litMarketDate" runat="server"></asp:Literal>
                </ItemTemplate>
              </asp:TemplateField>
        
            <asp:TemplateField HeaderText="Items" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="125px">
                <ItemTemplate>                                        
                   <b><asp:Literal ID="litItemCount" runat="server"></asp:Literal> Items</b>
                   <br />
                   <asp:HyperLink ID="lnkChangeItems" Text="Add/Remove Items" runat="server"></asp:HyperLink>
                </ItemTemplate>
              </asp:TemplateField>
       
            <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                <ItemTemplate>                                        
                  <asp:ImageButton ID="imgBtnEdit" ImageUrl="~/Inc/Images/Others/Edit.gif" runat="server" OnClientClick="runwait();" OnClick="Edit"  />
                </ItemTemplate>
              </asp:TemplateField>
        
            <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" >
                <ItemTemplate>                                        
                   <asp:ImageButton ID="imgBtnDelete" ImageUrl="~/Inc/Images/Others/Delete.gif"  runat="server" OnClientClick="return confirm('Are you sure you want to delete this record?');" OnClick="Delete" />
                </ItemTemplate>
              </asp:TemplateField>
        </Columns>		
   </asp:GridView>

    <!-- ************ POPUP *********************** -->
  <div id="editform" style="display:none"> 
     <div style=" margin:0px -10px 0px -10px; ">
       <h2 class="popupheader">Add/Edit Program</h2>
       <div style="position:absolute;top:5px;right:10px;">
            <b> <asp:LinkButton ID="LinkButton1" Text="X" runat="server" OnClick="Cancel"></asp:LinkButton></b>
        </div>
     </div>
     
     <div style="color:Maroon;">Fields marked with * are mandatory</div><br />
     <div style="color:Red;font-weight:bold">            
        <asp:Literal runat="server" ID="litValidationError"></asp:Literal>            
     </div>
          
     <table border="0">
      <tr>
          <td style="padding-left:20px;">
              <div>
                 <div style="float:left;"> <span class="textdesign">Program Name</span></div>
                 <div style="float:left; padding-left:10px;" id="IEtxtname"><asp:TextBox ID="txtpuprogramName"  runat="server" Width="196px" ></asp:TextBox></div>
              </div> 
              <div>
                 <div style="float:left;padding-top: 5px;"> <span class="textdesign">Description</span></div>
                 <div style=" float: left; padding-left: 34px; padding-top: 5px;"> <asp:TextBox ID="txtpuDesc" TextMode="MultiLine" Rows="3" runat="server"></asp:TextBox></div>
              </div>              
          </td>
          <td style="float:left;padding-top:7px;padding-left:20px;">
              <div>
                 <div style="float:left;"><span class="textdesign">Start Date</span></div>
                 <div style="float:left;margin-left: 35px;"><asp:TextBox ID="txtpuStartDate" runat="server" ></asp:TextBox>
                 <Ajax:CalendarExtender ID="calExtStartDate" runat="server" TargetControlID="txtpuStartDate" Format="MM/dd/yyyy" CssClass="MyCalendar" OnClientDateSelectionChanged="checkDate" /><br />
                 <asp:Label ID="Errormsg" runat="server" CssClass="startdateerror"></asp:Label>
                 </div>
              </div>
              <div style="margin-top:5px;">
                 <div style="float:left;margin-top: 5px;" id="IElblclosedate"><span class="textdesign">Closing Date</span></div>
                 <div style="float:left; margin-left: 20px;margin-top: 5px;" id="IEtxtclosedate"><asp:TextBox ID="txtpuClsDate" runat="server"></asp:TextBox>
                 <Ajax:CalendarExtender ID="calExtClosingDate" runat="server" TargetControlID="txtpuClsDate" Format="MM/dd/yyyy" CssClass="MyCalendar" /><br />                 
                 </div>
              </div> 
              <div style="margin-top:5px;">
                 <div style="float:left;margin-top: 5px;"><span class="textdesign">In Market Date</span></div>
                 <div style="float:left;margin-left: 4px;margin-top: 5px;"><asp:TextBox ID="txtpuMarketDate" runat="server"></asp:TextBox>
                 <Ajax:CalendarExtender ID="calExtInMarketDate" runat="server" TargetControlID="txtpuMarketDate" Format="MM/dd/yyyy" CssClass="MyCalendar" /><br />                 
                 </div>
              </div>
         </td>
     </tr>
     <tr style="display:nonex">
        <td>
            <div style="margin-top:10px;margin-bottom:1px">Image: </div>                    
            <asp:Image ID="popPreviewImage" runat="server" /><br />
            <asp:HiddenField ID="popHdnPreviewImageFileName" runat="server" />                    
            <div style="background:#CCC;padding:5px;color:darkred">
                <div style="background:#777;color:white;padding:5px;margin-bottom:5px;font-weight:bold">Upload Image</div>
                <asp:FileUpload runat="server" ID="popFileUploadPreviewImage" Visible="true"/><br />
                <asp:Button CssClass="SmallButton" runat="server" ID="popUploadButton" Text="Preview" OnClick="UploadPreviewImage"/>                    
            </div>
        </td>
     </tr>
    </table>
     <div class="field" style="display:none">
        <asp:TextBox ID="txtpuProgramID" class="ID" ReadOnly="true" runat="server"></asp:TextBox>
    </div>
    <div style =" margin-top:5px; margin-left:550px;">
        <asp:Button ID="txtSave" runat="server" Text="Save" OnClick="Save"/>
        <asp:Button ID="txtCancel" runat="server" Text="Cancel" OnClick="Cancel"/>
    </div>
   </div>

   <div class="jquerytriggers" style="display:none">
        <asp:TextBox ID="failuremsg" CssClass="failuremsg" runat="server"></asp:TextBox>
        <asp:TextBox ID="successmsg" CssClass="successmsg" runat="server"></asp:TextBox>
   </div> 

   

</asp:Content>

