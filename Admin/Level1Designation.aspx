<%@ Page Title="" Language="C#" MasterPageFile="~/LumenApp.master" AutoEventWireup="true" CodeFile="Level1Designation.aspx.cs" Inherits="Admin_Level1" %>

<asp:Content ContentPlaceHolderID="Title" runat="server">
    <h2>Manage User Designations</h2>
</asp:Content>

<asp:Content ContentPlaceHolderID="HtmlHead" runat="server">
    <script language="javascript" type="text/javascript">
        function confirmDesgChange() {
            var prevDesgName = document.getElementById('<%= txtExistDesgName.ClientID %>').value;
            var currDesgName = document.getElementById('<%= popDesgname.ClientID %>').value;

            if (prevDesgName != currDesgName)
                return confirm('Are you sure you want to update Designation Name?');
            else
                return true;
        }     
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server" >
     <div style="display:nonex">

     <div>
            <div style="border: solid 1px #999999; width:380px; height:23px; float:left;">
                <div class="FieldLabel" style="width:170px;">Select Pre Order Program<b style="color:Red"></b></div>
                <div class="FieldControl"><asp:DropDownList runat="server" ID="ddlProgram" AutoPostBack="true" OnSelectedIndexChanged="ProgramSelected" style="width:200px;"></asp:DropDownList></div>
            </div>
            <div id="divCloneFrom" runat="server" visible="false" style="border: solid 1px #999999; height:60px;width:380px; float:right;">
                <div class="FieldLabel" style="width:170px;">Program to Clone From<b style="color:Red"></b></div>
                <div class="FieldControl"><asp:DropDownList runat="server" ID="ddlProgramToCloneFrom" style="width:200px;"></asp:DropDownList></div>
				<div ><asp:Button ID="btnClone" Text="Clone Hierarchy From Program" OnClick="CloneHierarchy" runat="server" style="width:250px;float:left; height:25px;"/></div>            
            </div>
     </div><br /><br />
      
    <!-- ++++++++ Buttons for grid +++++++++ -->
    <div class="GridButtons" style="width:400px">
        <asp:Button Text="Add" OnClick="Add" runat="server" OnClientClick="ShowAcePopup('EditForm')"/>        
    </div>
    <div style="margin-bottom:10px;clear:both"></div>        
    <!-- +++++++++++++++++++++++++++++++++++ -->

    <!-- ########## THE GRID ######### -->
    <asp:GridView runat="server" ID="gridLevel1Desg" AutoGenerateColumns="false" 
    OnRowDataBound="gridLevel1Desg_RowDataBound" CssClass="mGrid" EmptyDataText="There are no records.">
        <Columns>
            <asp:BoundField DataField="designation_name" HeaderText="Designation Name" />  
            <asp:BoundField DataField="UserName" HeaderText="Manager Name" />
            <asp:TemplateField HeaderText="Subordinates" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="125px" Visible="true">
                <ItemTemplate>                                        
                   <asp:HyperLink ID="lnkManageMgrs" Text="Manage Subordinates" runat="server"></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CheckBoxField DataField="Active" HeaderText="Active" Visible="false" />
            <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="~/Inc/Images/Others/Edit.gif" runat="server" Text="Edit" OnClick="Edit" />
                    <span style="display:none"><asp:TextBox ID="txtID" runat="server"></asp:TextBox></span>
                    <span style="display:none"><asp:TextBox ID="txtSMID" runat="server"></asp:TextBox></span>
                    <span style="display:none"><asp:TextBox ID="txtSubOrdCnt" runat="server"></asp:TextBox></span>
                </ItemTemplate>
            </asp:TemplateField>  
           
            <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px" Visible=false>
                <ItemTemplate>
                <asp:ImageButton ImageUrl="~/Inc/Images/Others/Delete.gif" runat="server" Text="Delete" OnClick="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');"/>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <!-- ########################### -->
    
    <!-- ++++++++ Buttons for grid +++++++++ -->    
    <div class="GridButtons" style="width:400px">
        <asp:Button ID="Button1" Text="Add" OnClick="Add" runat="server" />
    </div>
    <div style="clear:both"></div>
    <!-- +++++++++++++++++++++++++++++++++++ -->

    <!-- ****************************************** -->
    <div class="AcePopup" id="editform" style="width:525px">
        <asp:TextBox CssClass="ID" Text="" runat="server" ID="popAddEditID"/>        
        <div style=" margin:0px -10px 0px -10px;">
            <h2 class="popupheader">Add/Edit Users Designation</h2>
            <div style="position:absolute;top:5px;right:10px;">
               <b> <asp:LinkButton Text="X" runat="server" OnClick="Cancel"></asp:LinkButton></b>
            </div>
        </div>
        
        <div style="color:Maroon;">Fields marked with * are mandatory</div><br />
        <div style="color:Red;font-weight:bold">            
            <asp:Literal runat="server" ID="litValidationError"></asp:Literal>            
        </div>                  

        <table>
            <tr>
                <td style="padding-left:20px;">
                    <div>
                        <div class="FieldLabel">Designation<b style="color:Red">*</b></div>
                        <div class="FieldControl">
                            <asp:TextBox ID="popDesgname"  runat="server" Width="344px" ></asp:TextBox> 
                            <span style="display:none"><asp:TextBox ID="txtExistDesgName" runat="server"></asp:TextBox></span>
                        </div>
                        <div>
                            <div class="FieldLabel">Manager Name<b style="color:Red">*</b></div>
                            <div class="FieldControl">
                                <asp:DropDownList runat="server" ID="ddlUsers" Width="350px" ></asp:DropDownList>
                            </div>
                        </div>
                    </div> 
                </td>
            </tr>
        </table>
        <div>
            <div class="SmallGridButtons" style="width:175px;float:left">                           
                <asp:Button ID="Button3" Text="Cancel" OnClick="Cancel" runat="server"/>                    
                <asp:Button ID="Button2" Text="Save" OnClick="Save" runat="server" OnClientClick="return confirmDesgChange();"/>     
            </div>
            <span style="color:Red;width:350px; float:right;">
                <asp:Literal runat="server" ID="litUpdateDesgMag"></asp:Literal>            
            </span>
        </div>
        <div style="clear:both"></div>
    </div>
    <!-- ****************************************** -->

    <!-- ########## Jquery Triggers ################## -->
    <div class="jquerytriggers" style="display:none">
        <asp:TextBox ID="failuremsg" CssClass="failuremsg" runat="server"></asp:TextBox>
        <asp:TextBox ID="successmsg" CssClass="successmsg" runat="server"></asp:TextBox>
   </div>
   <!-- ############################################# -->
   </div>
</asp:Content>
