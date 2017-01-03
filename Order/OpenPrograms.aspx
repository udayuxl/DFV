<%@ Page Title="" Language="C#" MasterPageFile="~/LumenNoLeft.master" AutoEventWireup="true" CodeFile="OpenPrograms.aspx.cs" Inherits="Order_OpenPrograms" %>
<asp:Content ContentPlaceHolderID="Title" runat="server">
    Pre Order Programs
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" Runat="Server">
    
    <asp:Panel ID="pnlEmptyMessage" runat="server" Visible="false">
        <div style="color:#770000; padding:30px">
            <b>Currently, there are no Pre Order programs.</b>
        </div>
    </asp:Panel>

    <asp:Repeater runat="server" ID="rptOpenPrograms" OnItemDataBound="rptOpenPrograms_ItemDataBound">                        
        <ItemTemplate>
            <div class="Program">
                <table>
                    <tbody><tr>
                        <td valign="top" class="column1">		
                            <asp:HyperLink ID="lnkProgramPreviewImage" Target="_blank" runat="server"/>
                        </td>
                        <td valign="top" class="column2">
                            <div class="ProgramDetails">
                                <div class="ProgramName">
                                    <asp:HyperLink runat="server" ID="lnkProgramName" CssClass="PreviewImageLink"></asp:HyperLink>
                                </div>
                                <div class="ProgramDescription">
                                    <asp:Literal runat="server" ID="litProgramDescription"></asp:Literal>
                                </div>
                                <div style="padding:5px;display:none;">
                                    <div style="float:left;">Program Type: &nbsp;</div>
                                    <div style="float:left;background:#FFFFAA;color:#00AA00; font-weight:bold;">Seasonal</div>
                                </div>
                                <div style="clear:both">&nbsp;</div>
                                <asp:HyperLink runat="server" id="lnkOrder" Text="Order" CssClass="button"></asp:HyperLink>
                            </div>                            
                        </td>
                        <td valign="top" class="column3">
                            <div class="ImportantDates">
                                <div class="ImportantDateLabel">Closing Date: </div>
                                <div class="ImportantDate">
                                    <asp:Label runat="server" ID="lblClosingDate"></asp:Label>
                                </div>
                                <br>
                                <div class="ImportantDateLabel">In Market Date: </div>
                                <div class="ImportantDate">
                                    <asp:Label runat="server" ID="lblMarketDate"></asp:Label>
                                </div>
                                <br>
                                <div id="divBudgetDetails" runat="server" style="display:none;">
                                    <div style="display:nonex" class="ImportantDateLabel">Budget Remaining: </div>
                                    <div style="display:nonex" class="ImportantDate">
                                        <asp:Label runat="server" ID="lblBudget"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </td>                                
                            
                    </tr>
                </tbody></table>                        
            </div>
        </ItemTemplate>        
    </asp:Repeater>
</asp:Content>

