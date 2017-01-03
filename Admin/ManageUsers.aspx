<%@ Page Title="" Language="C#" MasterPageFile="~/LumenApp.master" AutoEventWireup="true" CodeFile="ManageUsers.aspx.cs" Inherits="Admin_ManageUsers" %>

<asp:Content ContentPlaceHolderID="Title" runat="server">
    <h2>Manage Users</h2>
</asp:Content>

<asp:Content ContentPlaceHolderID="HtmlHead" runat="server">
    <script language="javascript" type="text/javascript">
        function uncheckStates() {
            if (confirm('Are you sure you want to Uncheck all States ?')) {
                var cbs = document.getElementsByTagName('input');
                for (var i = 0; i < cbs.length; i++) {
                    if (cbs[i].type == 'checkbox') {
                        /*
                        if (cbs[i].id.contains('chkTerritory')) {
                        cbs[i].checked = false;
                        }
                        */
                        if (cbs[i].id != null) {
                            var strIndex = cbs[i].id.indexOf('chkRegion');
                            if (strIndex == -1) {
                                //string not found
                            } else {
                                cbs[i].checked = false;
                            }
                        }
                    }
                }
            }
            else {
                return false;
            }
        }
    </script>
    <style type="text/css"> 
    .mGrid td { background:#FFF; padding: 5px; padding-left:3px; padding-right:2px; border: solid 1px #999999;}
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server" >
     <div style="display:nonex"> 
    <!-- ++++++++ Buttons for grid +++++++++ -->
    <div class="GridButtons" style="width:800px">
        <asp:Button Text="Add" OnClick="Add" runat="server" OnClientClick="ShowAcePopup('EditForm')"/>
        <span style="float:right;margin-right:50px;">
            <asp:TextBox ID="txtSearch" runat="server" placeholder="Please enter complete or partial Name/Email..."  Width=350px/>
            <asp:Button ID="btnSearch" runat="server" OnClick="SearchUsers" Text="Search" />
        </span>          
    </div>
    <div style="margin-bottom:10px;clear:both"></div>        
    <!-- +++++++++++++++++++++++++++++++++++ -->

    <!-- ########## THE GRID ######### -->
    <asp:GridView runat="server" ID="gridUsers" AutoGenerateColumns="false" 
    OnRowDataBound="gridUsers_RowDataBound" CssClass="mGrid" EmptyDataText="There are no records.">
        <Columns>
            <asp:BoundField DataField="UserName" HeaderText="User Name" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:CheckBoxField DataField="Admin" HeaderText="Admin" ItemStyle-HorizontalAlign="Center"/>
            <asp:CheckBoxField DataField="MarketingManager" HeaderText="Marketing Manager" ItemStyle-HorizontalAlign="Center"/>
            <asp:CheckBoxField DataField="SalesManager" HeaderText="Sales Manager" ItemStyle-HorizontalAlign="Center"/>
            <asp:BoundField DataField="RegionNames" HeaderText="States"  Visible="false" ItemStyle-Width="200px" />
            <asp:TemplateField HeaderText="Reset Password" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="125px">
                <ItemTemplate>                                        
                   <asp:LinkButton OnClick="Reset" OnClientClick="return confirm('Are you sure you want to reset password?');" ID="lnkReset" Text="Reset password" runat="server" />
                   <span style="display:none"><asp:TextBox ID="txtUserName" runat="server"></asp:TextBox></span>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px">
                <ItemTemplate>
                    <asp:ImageButton ID="ImageButton1" ImageUrl="~/Inc/Images/Others/Edit.gif" runat="server" Text="Edit" OnClick="Edit" />
                    <span style="display:none"><asp:TextBox ID="txtUserId" runat="server"></asp:TextBox></span>
                    <span style="display:none"><asp:TextBox ID="txtActive" runat="server"></asp:TextBox></span>
                    <span style="display:none"><asp:TextBox ID="txtSalesRole" runat="server"></asp:TextBox></span>
                    <span style="display:none"><asp:TextBox ID="txtMarMgrRole" runat="server"></asp:TextBox></span>
                    <span style="display:none"><asp:TextBox ID="txtAdminRole" runat="server"></asp:TextBox></span>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CheckBoxField DataField="Active" HeaderText="Active"/>
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
        <asp:TextBox CssClass="ID" Text="" runat="server" ID="popTxtActive"/>
        <asp:TextBox CssClass="ID" Text="" runat="server" ID="popTxtSalesRole"/>
        <asp:TextBox CssClass="ID" Text="" runat="server" ID="popTxtMarMgrRole"/>
        <asp:TextBox CssClass="ID" Text="" runat="server" ID="popTxtAdminRole"/>
               
        <div style=" margin:0px -10px 0px -10px;">
            <h2 class="popupheader">Add/Edit Users</h2>
            <div style="position:absolute;top:5px;right:10px;">
               <b> <asp:LinkButton Text="X" runat="server" OnClick="Cancel"></asp:LinkButton></b>
            </div>
        </div>
        
        <div style="color:Maroon;">Fields marked with * are mandatory</div><br />
        <div style="color:Red;font-weight:bold">            
            <asp:Literal runat="server" ID="litValidationError"></asp:Literal>            
        </div>                  

        <table><tbody>
            <tr>
                <td style="padding-left:20px;">
                    <div>
                        <div class="FieldLabel">First Name<b style="color:Red">*</b></div>
                        <div class="FieldControl">
                            <asp:TextBox ID="popFName"  runat="server" Width="344px" ></asp:TextBox> 
                        </div>
                        <div class="FieldLabel">Last Name<b style="color:Red">*</b></div>
                        <div class="FieldControl">
                            <asp:TextBox ID="popLName"  runat="server" Width="344px" ></asp:TextBox> 
                        </div>
                        <div class="FieldLabel">Email<b style="color:Red">*</b></div>
                        <div class="FieldControl">
                            <asp:TextBox ID="popEmail"  runat="server" Width="344px" ></asp:TextBox> 
                        </div>

                        <div class="FieldLabel">Active<b style="color:Red"></b></div>
                        <div class="FieldControl">
                            <asp:CheckBox id="popActive" runat="server"
                                Text="" TextAlign="Right"/>
                        </div>
                        </div>
                    </td>
            </tr>
            <tr>
                    <td>
                        <table>
                            <tr>
                        	    <td>
                                    <div class="FieldLabel">Roles<b style="color:Red"></b></div>
                                </td>
                        	    <td>
                                    <div class="FieldControl">
                                        <asp:CheckBox id="popAdminRole" runat="server"
                                            Text="Admin" TextAlign="Right"/>
                                    </div>
                                </td>
                        	    <td>
                                    <div class="FieldControl">
                                        <asp:CheckBox id="popMarMgrRole" runat="server"
                                            Text="Marketing Manager" TextAlign="Right"/>
                                    </div>
                                </td>
                        	    <td>
                                    <div class="FieldControl">
                                        <asp:CheckBox id="popSalesRole" runat="server"
                                        Text="Sales Manager" TextAlign="Right"/>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td colspan=2>
                        <p id="BrandHeader"  style="color:#620937;font-size:14px;font-weight:bold;margin-right:10px;margin-top:25px;"> Select States 
                        <span style="padding-left: 80px; display:none;">
                            <a onClick="uncheckStates();">Uncheck All States</a>
                        </span></p>
                        <div>
                            <div style="padding:5px;overflow-y:scroll;overflow-x:none;width:330px;height: 120px;border: 1px solid #C0C0C0;float:left; "> 
			                    <div id="Div3">
                                    <div style="margin-left:1px;height:110px;width:280px;">
                                        <asp:ListView ID="lvwSelectedRegion"  ItemPlaceholderID="PlaceHolderRegions" runat="server" OnItemDataBound="lvwSelectedRegion_ItemDataBound" > 
                                            <LayoutTemplate>
                                                <div id="PlaceHolderRegions" runat="server"></div>
                                            </LayoutTemplate>                                 
                                            <ItemTemplate>
                                                <table>
                                                    <tr><td style="width:150px;"><asp:CheckBox ID="chkRegion" runat="server" /> </td>
                                        	</ItemTemplate>
                                            <AlternatingItemTemplate>
                                                    <td style="width:150px;"><asp:CheckBox ID="chkRegion" runat="server" /> </td></tr>
                                                </table>
                                            </AlternatingItemTemplate>                
                                        </asp:ListView>                       
                                    </div> 
                                </div>
                             </div>
                        </div>
                        
                        <!-- END -->
                    </td>
                </tr>
            </tbody>
        </table>
        <div style="clear:both"></div><br />
        <div class="SmallGridButtons" style="width:300px">                           
            <asp:Button ID="Button3" Text="Cancel" OnClick="Cancel" runat="server"/>                    
            <asp:Button ID="Button2" Text="Save" OnClick="Save" runat="server"/>     
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
