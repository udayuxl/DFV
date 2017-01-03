<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/LumenNoLeft.master" AutoEventWireup="true" CodeFile="SelectImage.aspx.cs" Inherits="Admin_SelectImage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server">
    <style type="text/css">
        .btnImgChgDone{float: right; margin-right: 26px; margin-top: 10px;}
        div.header {  background: none repeat scroll 0 0 #CCCCCC;   float: left;    font-weight: bold;    margin-top: 5px;    width: 950px;}
        div.header h2 {float:left;}
        .imgBorder{border: 1px solid #620937;}    
    </style>
    
    <div class="header">        
            <h2>Change <asp:Literal ID="litItemOrProgramHeader" runat="server" />Image</h2>        
        <asp:Button ID="btnImgChgDone" runat="server" Text="Done" CssClass="btnImgChgDone" OnClick="GoBack"/>  
    </div>
    <div style="clear:both"></div>
    <br />

    <!-- **** CURRENT IMAGE ************************* -->
    <div style="Border:1px solid #620937;">
        <div style="background:#CCCCCC;padding:5px 0pc 10px 0px;font-weight:bold;color:#2060A0;">
            <span style="padding-left:10px;">Current Image</span>
        </div>
        <table>
            <tr>
                <td style="padding-left:20px;">
                    <span style="color:#620937;">
                        <asp:Literal ID="litprgitm" runat="server" />                        
                        <b><asp:Literal ID="litItemOrProgramName" runat="server" /></b>
                    </span>
                </td>
                <td style="padding-left:90px; text-align:center;">
                    <asp:Image ID="imgItemOrProgram" runat="server" BorderColor="#620937" BorderStyle="Solid" BorderWidth="2px" />
                    <br /><br />
                    <span style="padding:5px 0  10px 0;font-weight:bold;color:#620937;">
                        <asp:Literal ID="litimageName" runat="server" />
                    </span>
                    
                </td>
            </tr>
        </table>
    </div> 
    <br /><br />
    <!-- ******************************************** -->


    <!-- **** GALLERY (IMAGE LIBRARY) ************************* -->
    <div style="Border:1px solid #620937;" >
        <div style="background:#CCC;padding:5px 0pc 10px 0px;font-weight:bold;color:#2060A0;">
            <span style="padding-left:10px;">Select new Image: Click on an Image in the gallery below</span>
        </div>
        <div style="Border:1px solid #620937;">                                        
            <asp:Repeater ID="rptImages" runat="server" OnItemDataBound="ImagesItemDataBound" >
                <ItemTemplate>
                    <div style="width:150px;height:150px;background:white;float:left;margin:10px;padding:25px 30px 10px 25px;">
                        <asp:ImageButton ID="btnImage" OnClick="ChangeImage" runat="server" BorderColor="#620937" BorderStyle="Solid" BorderWidth="2px" />
                        <asp:HiddenField ID="hdnImageID" runat="server" />
                        <br />
                        <span style="text-align:left;font-weight:bold;color:#620937;"><asp:Literal ID="litName" runat="server"></asp:Literal></span>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
           
    </div>
    <!-- **** GALLERY (IMAGE LIBRARY) ************************* -->
</asp:Content>

