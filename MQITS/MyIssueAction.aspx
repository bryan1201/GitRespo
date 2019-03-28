<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MyIssueAction.aspx.cs" Inherits="MyIssueAction" Title="=== MQITS::MyDetail ==="  MaintainScrollPositionOnPostback="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
<table style="border-right: #b6cade 2px solid; border-top: #b6cade 2px solid; border-left: #b6cade 2px solid; border-bottom: #b6cade 2px solid; width: 100%; float: none; clear: both; font-size: 10pt; color: #333333; font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white;" 
    align="left" cellpadding="1" cellspacing="1">
        <tr>
            <td style="width: 90px; background-color: #b6cade;">
                Module</td>
            <td style="width: 200px; background-color: #eff3f8">
                <asp:DropDownList ID="ddlModule" runat="server" Width="98%" DataSourceID="SqlDSModule" DataTextField="ModuleName" DataValueField="ModuleID">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDSModule" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT '' as ModuleID, '-' as ModuleName&#13;&#10;UNION&#13;&#10;SELECT CONVERT(NVARCHAR,[ModuleID]) as ModuleID, [ModuleName] &#13;&#10;FROM v_Module WHERE &#13;&#10;[IsEnable] = @IsEnable AND [IsVirtual] = @IsVirtual ">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="True" Name="IsEnable" />
                        <asp:Parameter DefaultValue="False" Name="IsVirtual" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="width: 90px; background-color: #b6cade;">
                Customer</td>
            <td style="width: 200px; background-color: #eff3f8">
                <asp:DropDownList ID="ddlCustomer" runat="server" Width="98%" AutoPostBack="True" DataSourceID="SqlDSCustomer" DataTextField="CustomerName" DataValueField="CustomerID" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged">
                </asp:DropDownList><asp:SqlDataSource ID="SqlDSCustomer" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT '' as CustomerID,'-' as CustomerName&#13;&#10;UNION&#13;&#10;SELECT CONVERT(NVARCHAR,CustomerID) as CustomerID,CustomerName  FROM v_IssueCustomerOwner WHERE OwnerCode=@UserId AND&#13;&#10;CustomerID IN(&#13;&#10;SELECT [CustomerID] FROM [v_Customer] WHERE [IsEnable] = @IsEnable AND [IsVirtual] = @IsVirtual&#13;&#10;)">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtUserId" Name="UserId" PropertyName="Text" />
                        <asp:Parameter DefaultValue="True" Name="IsEnable" />
                        <asp:Parameter DefaultValue="False" Name="IsVirtual" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>            
                <td rowspan="3" align="center" valign="bottom" 
                style="text-align: center; background-color: #eff3f8; vertical-align: middle;">
                <asp:Button ID="btnQry" runat="server" Text="Query" Width="100px" OnClick="btnQry_Click" /><br />
            </td>
        </tr>
    <tr>
        <td style="background-color: #b6cade">
            Site</td>
        <td style="background-color: #eff3f8">
            <asp:DropDownList ID="ddlSite" runat="server" Width="98%" AutoPostBack="True" DataSourceID="SqlDSSite" DataTextField="SiteName" DataValueField="SiteID" OnSelectedIndexChanged="ddlSite_SelectedIndexChanged">
            </asp:DropDownList></td>
            <asp:SqlDataSource ID="SqlDSSite" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT ''as SiteID,'-' as SiteName&#13;&#10;UNION&#13;&#10;SELECT CONVERT(NVARCHAR,SiteID) as SiteID,SiteName  FROM v_IssueSiteOwner WHERE OwnerCode = @UserId AND&#13;&#10;SiteID IN (&#13;&#10;SELECT [SiteID] FROM [v_Site] WHERE [IsEnable] = @IsEnable AND [IsVirtual] = @IsVirtual&#13;&#10;)&#13;&#10;">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtUserId" Name="UserId" PropertyName="Text" />
                    <asp:Parameter DefaultValue="True" Name="IsEnable" />
                    <asp:Parameter DefaultValue="False" Name="IsVirtual" />
                </SelectParameters>
            </asp:SqlDataSource>
        <td style="background-color: #b6cade">
                Project</td>
        <td style="background-color: #eff3f8">
                <asp:DropDownList ID="ddlProject" runat="server" Width="98%" AutoPostBack="True" DataSourceID="SqlDSProject" DataTextField="ProjectName" DataValueField="ProjectID" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                </asp:DropDownList></td>
                 <asp:SqlDataSource ID="SqlDSProject" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT '' as ProjectID,'-' as ProjectName&#13;&#10;UNION&#13;&#10;SELECT CONVERT(NVARCHAR,[ProjectID] ) as [ProjectID], [ProjectName] FROM [v_Project] WHERE ([IsEOSL] = @IsEOSL) &#13;&#10;AND ProjectID IN (SELECT DISTINCT ProjectID FROM v_IssueProjectOwner WHERE OwnerCode = @editor) &#13;&#10;AND Customer = @CustomerID ">
                     <SelectParameters>
                         <asp:Parameter DefaultValue="False" Name="IsEOSL" />
                         <asp:ControlParameter ControlID="txtUserId" DefaultValue="" Name="editor" PropertyName="Text" />
                         <asp:ControlParameter ControlID="ddlCustomer" Name="CustomerID" PropertyName="SelectedValue" />
                     </SelectParameters>
                 </asp:SqlDataSource>
    </tr>
        <tr>
            <td style="background-color: #b6cade">
                ProjectPhase</td>
            <td style="background-color: #eff3f8">
                <asp:DropDownList ID="ddlPhase" runat="server" Width="98%" DataSourceID="SqlDSPhase" DataTextField="PhaseName" DataValueField="PhaseID">
                </asp:DropDownList></td>
                <asp:SqlDataSource ID="SqlDSPhase" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT '' as PhaseID ,'-' as PhaseName &#13;&#10;UNION&#13;&#10;SELECT CONVERT(NVARCHAR,PhaseID) as PhaseID,PhaseName&#13;&#10;FROM fn_GetProjectPhase(@Project,@Site )">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlProject" Name="Project" PropertyName="SelectedValue" />
                        <asp:ControlParameter ControlID="ddlSite" Name="Site" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            <td style="background-color: #b6cade">
                </td>
            <td style="background-color: #eff3f8">
                <asp:Label ID="lblQstring" runat="server" Visible="False"></asp:Label></td>
        </tr>
        </table>
      
        
        <table style="border-left-color: #ccccff; border-bottom-color: #ccccff; width: 100%;
            border-top-style: ridge; border-top-color: #ccccff; font-family: 'Arial Unicode MS';
            border-right-style: ridge; border-left-style: ridge; border-right-color: #ccccff;
            border-bottom-style: ridge">
            <tr>
                <td style="width: 50%" align="left" valign="top">
                 <asp:GridView ID="gvReportMe" runat="server" CellPadding="1" 
                        ForeColor="#333333" AutoGenerateColumns="False" CaptionAlign="Left" Font-Bold="True" Font-Size="12pt" Width="100%" DataSourceID="SqlDSReportMe" EmptyDataText="--nodata list--" AllowPaging="True" OnRowDataBound="gvReportMe_RowDataBound" OnPageIndexChanging="gvReportMe_PageIndexChanging" Caption="Report Me">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" />
                        <PagerSettings Position="TopAndBottom" />
                        <Columns>
                            <asp:TemplateField HeaderText="IssueID" SortExpression="IssueSN">
                            <ItemTemplate>      
                            <asp:HyperLink ID="lblIssueID" runat="server" Text='<%# Bind("IssueSN")%>' Target="_blank"
                                 NavigateUrl='<%# Bind("URL")%>'></asp:HyperLink>
                            </ItemTemplate>
                                <ItemStyle Font-Size="9pt" Width="20%" />
                                <HeaderStyle Font-Size="9pt" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ModuleName" HeaderText="Module" >
                                <HeaderStyle Font-Size="9pt" />
                                <ItemStyle Font-Size="9pt" Width="20%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Defect Symptom" DataField="DefectSymptom">
                                <HeaderStyle Font-Size="9pt" />
                                <ItemStyle Font-Size="9pt" Width="60%" />
                            </asp:BoundField>
                                    
                            <asp:TemplateField HeaderText="Status" SortExpression="Status" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                        <EmptyDataRowStyle ForeColor="#FFC0C0" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDSReportMe" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="txtUserId" Name="Reporter" PropertyName="Text" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
                <td style="width: 50%" align="left" valign="top">
                 <asp:GridView ID="gvMonitor" runat="server" CellPadding="1" ForeColor="#333333" AutoGenerateColumns="False" CaptionAlign="Left" Font-Bold="True" Font-Size="12pt" Width="100%" DataSourceID="SqlDSMonitor" EmptyDataText="--nodata list--" AllowPaging="True" OnRowDataBound="gvMonitor_RowDataBound" OnPageIndexChanging="gvMonitor_PageIndexChanging" Caption="Monitor">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" />
                        <PagerSettings Position="TopAndBottom" />
                        <Columns>
                            <asp:TemplateField HeaderText="IssueID" SortExpression="IssueSN">
                            <ItemTemplate>      
                            <asp:HyperLink ID="lblIssueID" runat="server" Text='<%# Bind("IssueSN")%>' Target="_blank" 
                                    NavigateUrl='<%# Bind("URL")%>'></asp:HyperLink>
                            </ItemTemplate>
                                <ItemStyle Font-Size="9pt" Width="20%" />
                                <HeaderStyle Font-Size="9pt" />
                            </asp:TemplateField>
                             <asp:BoundField DataField="ModuleName" HeaderText="Module" >
                                <HeaderStyle Font-Size="9pt" />
                                <ItemStyle Font-Size="9pt" Width="20%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Defect Symptom" DataField="DefectSymptom">
                                <HeaderStyle Font-Size="9pt" />
                                <ItemStyle Font-Size="9pt" Width="80%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" Visible="False" />
                        </Columns>
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                        <EmptyDataRowStyle ForeColor="#FFC0C0" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDSMonitor" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="txtUserId" Name="Reporter" PropertyName="Text" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">
                <asp:GridView ID="gvOwenMe" runat="server" CellPadding="1" 
                        ForeColor="#333333" AutoGenerateColumns="False" CaptionAlign="Left" Font-Bold="True" Font-Size="12pt" Width="100%" DataSourceID="SqlDSOwnerMe" EmptyDataText="--nodata list--" AllowPaging="True" OnRowDataBound="gvOwenMe_RowDataBound" OnPageIndexChanging="gvOwenMe_PageIndexChanging" Caption="Owner Me">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" />
                        <PagerSettings Position="TopAndBottom" />
                        <Columns>
                            <asp:TemplateField HeaderText="IssueID" SortExpression="IssueSN">
                            <ItemTemplate>      
                            <asp:HyperLink ID="lblIssueID" runat="server" Text='<%# Bind("IssueSN")%>' Target="_blank"
                                     NavigateUrl='<%# Bind("URL")%>'></asp:HyperLink>
                            </ItemTemplate>
                                <ItemStyle Font-Size="9pt" Width="20%" />
                                <HeaderStyle Font-Size="9pt" />
                            </asp:TemplateField>
                             <asp:BoundField DataField="ModuleName" HeaderText="Module" >
                                <HeaderStyle Font-Size="9pt" />
                                <ItemStyle Font-Size="9pt" Width="20%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Defect Symptom" DataField="DefectSymptom">
                                <HeaderStyle Font-Size="9pt" />
                                <ItemStyle Font-Size="9pt" Width="80%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" Visible="False" />
                        </Columns>
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                        <EmptyDataRowStyle ForeColor="#FFC0C0" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDSOwnerMe" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="txtUserId" Name="Reporter" PropertyName="Text" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
                <td align="left" valign="top">
                <asp:GridView ID="gvRM" runat="server" CellPadding="1" 
                        ForeColor="#333333" AutoGenerateColumns="False"  CaptionAlign="Left" Font-Bold="True" Font-Size="12pt" Width="100%" DataSourceID="SqlDSRM" EmptyDataText="--nodata list--" AllowPaging="True" OnRowDataBound="gvRM_RowDataBound" OnPageIndexChanging="gvRM_PageIndexChanging" Caption="Recently Modified Close">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" />
                        <PagerSettings Position="TopAndBottom" />
                        <Columns>
                             <asp:TemplateField HeaderText="IssueID" SortExpression="IssueSN">
                            <ItemTemplate>      
                            <asp:HyperLink ID="lblIssueID" runat="server" Text='<%# Bind("IssueSN")%>' Target="_blank" 
                                    NavigateUrl='<%# Bind("URL")%>'></asp:HyperLink>
                            </ItemTemplate>
                                <ItemStyle Font-Size="9pt" Width="20%" />
                                 <HeaderStyle Font-Size="9pt" />
                            </asp:TemplateField>
                             <asp:BoundField DataField="ModuleName" HeaderText="Module" >
                                <HeaderStyle Font-Size="9pt" />
                                <ItemStyle Font-Size="9pt" Width="20%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Defect Symptom" DataField="DefectSymptom">
                                <HeaderStyle Font-Size="9pt" />
                                <ItemStyle Font-Size="9pt" Width="80%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" Visible="False" />
                        </Columns>
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                        <EmptyDataRowStyle ForeColor="#FFC0C0" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDSRM" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="txtUserId" Name="Reporter" PropertyName="Text" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left" valign="top">
                 <asp:GridView ID="gvMyAction" runat="server" CellPadding="1" 
                    ForeColor="#333333" AutoGenerateColumns="False" CaptionAlign="Left" Font-Bold="True" Font-Size="12pt" Width="100%" DataSourceID="SqlDSMyAction" EmptyDataText="--nodata list--" AllowPaging="True" OnRowDataBound="gvMyAction_RowDataBound" OnPageIndexChanging="gvMyAction_PageIndexChanging" Caption="My Action">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" />
                        <PagerSettings Position="TopAndBottom" />
                        <Columns>
                            <asp:TemplateField HeaderText="IssueID" SortExpression="IssueSN">
                            <ItemTemplate>      
                            <asp:HyperLink ID="lblIssueID" runat="server" Text='<%# Bind("IssueSN")%>' Target="_blank"
                                 NavigateUrl='<%# Bind("URL")%>'></asp:HyperLink>
                            </ItemTemplate>
                                <ItemStyle Font-Size="9pt" Width="10%" />
                                <HeaderStyle Font-Size="9pt" />
                            </asp:TemplateField>
                             <asp:BoundField DataField="ModuleName" HeaderText="Module" >
                            <HeaderStyle Font-Size="9pt" />
                                <ItemStyle Font-Size="9pt" Width="10%" />
                            </asp:BoundField>
                             <asp:BoundField DataField="Project" HeaderText="Project" >
                            <HeaderStyle Font-Size="9pt" />
                                <ItemStyle Font-Size="9pt" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Defect Symptom" DataField="DefectSymptom">
                                <HeaderStyle Font-Size="9pt" />
                                <ItemStyle Font-Size="9pt" Width="30%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ActionDescription" HeaderText="ActionDescription" >
                            <HeaderStyle Font-Size="9pt" />
                                <ItemStyle Font-Size="9pt" Width="30%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Due Date" DataField="DueDate">
                                <HeaderStyle Font-Size="9pt" />
                                <ItemStyle Font-Size="9pt" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" Visible="False" />
                        </Columns>
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                        <EmptyDataRowStyle ForeColor="#FFC0C0" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDSMyAction" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="txtUserId" Name="Reporter" PropertyName="Text" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    
                </td>
            </tr>
        </table>
    <asp:TextBox ID="txtUserId" runat="server" Visible="false"></asp:TextBox>
    <br/>
     <table style="border-right: #b6cade 2px solid; border-top: #b6cade 2px solid; border-left: #b6cade 2px solid; border-bottom: #b6cade 2px solid; width: 100%; float: none; clear: both; font-size: 10pt; color: #333333; font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white; font-weight: bold;" 
                align="left" cellpadding="1" cellspacing="1">
            <tr>
                <td width="25%" bgcolor="#ffccff">OPEN</td>
                <td width="25%" bgcolor="#ccff99">Monitor</td>
                <td width="25%" bgcolor="#cccccc">Close</td>
                <td width="25%" bgcolor="#ffcc66">Cancel</td>
            </tr>
        </table>
    </asp:Content>    

