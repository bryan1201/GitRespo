<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MGroupRelation.aspx.cs" Inherits="MGroupRelation" Title="=== MQITS::Group Relation Maintenance ===" MaintainScrollPositionOnPostback="true" StylesheetTheme="MMContainer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
        <table style="width:100%">
            <tr>
                <td style="width: 120px; background-color: #b6cade;">Module</td>
                <td style="width: 120px; background-color: #b6cade;">Customer</td>
                <td style="width: 120px; background-color: #b6cade;">Project</td>
                <td style="width: 120px; background-color: #b6cade;">Site</td>
                <td style="width: 120px; background-color: #b6cade;">Project&#39;s Phase</td>
                <td rowspan="2" style="background-color: #eff3f8;"><asp:Button ID="btnQuery" runat="server" Text="Query" /></td>
            </tr>
            <tr>
                <td style="background-color: #eff3f8"><asp:DropDownList ID="ddlModule" 
                        runat="server" DataSourceID="SqlDSModule" DataTextField="ModuleName" 
                        DataValueField="ModuleID"></asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDSModule" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                        SelectCommand="
                        SELECT 0 ModuleID, '' ModuleName, 0 AS Rank UNION
                        SELECT [ModuleID], [ModuleName], Rank FROM [v_Module] WHERE (([IsEnable] = @IsEnable) AND ([IsVirtual] = @IsVirtual)) ORDER BY [Rank]">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="True" Name="IsEnable" Type="Boolean" />
                            <asp:Parameter DefaultValue="False" Name="IsVirtual" Type="Boolean" />
                        </SelectParameters>
                    </asp:SqlDataSource> 
                </td>
                <td style="background-color: #eff3f8"><asp:DropDownList ID="ddlCustomer" 
                        runat="server" DataSourceID="SqlDSqCustomer" DataTextField="CustomerName" 
                        DataValueField="CustomerID" AutoPostBack="true"></asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDSqCustomer" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                        SelectCommand="
                        SELECT 0 CustomerID, '' CustomerName, 0 AS Rank UNION
                        SELECT [CustomerID], [CustomerName], Rank FROM [v_Customer] WHERE (([IsEnable] = @IsEnable) AND ([IsVirtual] = @IsVirtual)) ORDER BY [Rank]">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="True" Name="IsEnable" Type="Boolean" />
                            <asp:Parameter DefaultValue="False" Name="IsVirtual" Type="Boolean" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
                <td style="background-color: #eff3f8">
                    <asp:DropDownList ID="ddlProject" 
                        runat="server" DataSourceID="SqlDSqPorject" DataTextField="ProjectName" 
                        DataValueField="ProjectID"></asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDSqPorject" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                        SelectCommand="SELECT 0 ProjectID, '' ProjectName, 0 AS Rank UNION
                        SELECT [ProjectID], [ProjectName], Rank FROM [v_Project] WHERE (([IsEOSL] = @IsEOSL) AND ([Customer] = @Customer)) ORDER BY [Rank]">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="False" Name="IsEOSL" Type="Boolean" />
                            <asp:ControlParameter ControlID="ddlCustomer" Name="Customer" 
                                PropertyName="SelectedValue" Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
                <td style="background-color: #eff3f8"><asp:DropDownList ID="ddlSite" runat="server" 
                        DataSourceID="SqlDSSite" DataTextField="SiteName" DataValueField="SiteID"></asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDSSite" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                        SelectCommand="SELECT 0 SiteID, '' SiteName, 0 AS Rank UNION
                        SELECT [SiteID], [SiteName], Rank FROM [v_Site] WHERE (([IsEnable] = @IsEnable) AND ([IsVirtual] = @IsVirtual)) ORDER BY [Rank]">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="True" Name="IsEnable" Type="Boolean" />
                            <asp:Parameter DefaultValue="False" Name="IsVirtual" Type="Boolean" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
                <td style="background-color: #eff3f8">
                    <asp:DropDownList ID="ddlProjectPhase" 
                        runat="server" DataSourceID="SqlDSProjectPhase" DataTextField="PhaseName" 
                        DataValueField="PhaseID"></asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDSProjectPhase" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                        
                        SelectCommand="SELECT 0 PhaseID, '' PhaseName, 0 AS Rank UNION
                        SELECT [PhaseID], [PhaseName], Rank FROM [m_ProjectPhase] ORDER BY [Rank]">
                    </asp:SqlDataSource>
                </td>
            </tr>
            <tr>
                <td colspan="5" style="background-color: #b6cade;">
                    <table style="width:60%; background-color: #eff3f8;">
                        <tr style="background-color: #b6cade">
                            <td style="background-color: #b6cade; width: 200px; font-size: 10pt;">Create 
                                Relation:</td>
                            <td style="background-color: #eff3f8; width: 10px;">
                                &nbsp;</td>
                            <td style="background-color: #eff3f8">
                                <asp:Button ID="btnGenRelation" Text="Create" 
                                    runat="server" onclick="btnGenRelation_Click" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        
        <asp:GridView ID="gvGroupRelation" runat="server" AllowSorting="True" 
            AutoGenerateColumns="False" BackColor="LightGoldenrodYellow" BorderColor="Tan" 
            BorderWidth="1px" Caption="Group Relation" CaptionAlign="Left" CellPadding="2" 
            DataKeyNames="ID" DataSourceID="SqlDSGroupRelation" Font-Size="10pt" ForeColor="Black" 
            GridLines="Vertical" ShowFooter="True" 
            onrowupdating="gvGroupRelation_RowUpdating">
            <FooterStyle BackColor="Tan" />
            <Columns>
                <asp:CommandField DeleteText="Delete" CancelText="Cancel" EditText="Edit" 
                    InsertText="Save" NewText="Add" SelectText="Select" ShowEditButton="True" 
                    UpdateText="Save" />
                <asp:TemplateField HeaderText="ModuleName" SortExpression="ModuleName">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlModule" runat="server" DataSourceID="SqlDSModule" 
                            DataTextField="ModuleName" DataValueField="ModuleID" 
                            SelectedValue='<%# Bind("ModuleID", "{0}") %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("ModuleName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CustomerName" SortExpression="CustomerName">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlCustomer" runat="server" DataSourceID="SqlDSqCustomer" 
                            DataTextField="CustomerName" DataValueField="CustomerID" 
                            SelectedValue='<%# Bind("CustomerID", "{0}") %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ProjectName" SortExpression="ProjectName">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlProject" runat="server" DataSourceID="SqlDSqPorject" 
                            DataTextField="ProjectName" DataValueField="ProjectID" 
                            SelectedValue='<%# Bind("ProjectID", "{0}") %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("ProjectName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SiteName" SortExpression="SiteName">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlSite" runat="server" DataSourceID="SqlDSSite" 
                            DataTextField="SiteName" DataValueField="SiteID" 
                            SelectedValue='<%# Bind("SiteID", "{0}") %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("SiteName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="PhaseName" SortExpression="PhaseName">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlPhase" runat="server" DataSourceID="SqlDSProjectPhase" 
                            DataTextField="PhaseName" DataValueField="PhaseID">
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("PhaseName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
            <HeaderStyle BackColor="Tan" Font-Bold="True" />
            <AlternatingRowStyle BackColor="PaleGoldenrod" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDSGroupRelation" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
            SelectCommand="SELECT * FROM [v_GroupRelation] ORDER BY [ModuleName], [ProjectName], [SiteName]">
        </asp:SqlDataSource>
        
<asp:HiddenField ID="hfUserId" runat="server" />
</asp:Content>

