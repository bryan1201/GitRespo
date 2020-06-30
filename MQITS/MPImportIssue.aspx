<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MPImportIssue.aspx.cs" Inherits="MPImportIssue" Title="=== MQITS::MPImportIssue ===" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
    <table style="font-size: 10pt; width: 100%; font-family: 'Arial Unicode MS'; border-top-style: groove; border-right-style: groove; border-left-style: groove; border-bottom-style: groove;">
        <tr>
            <td style="font-family: 'Arial Unicode MS'; background-color: #ccccff" width="25%" >Customer<asp:DropDownList ID="ddlSite" runat="server" DataSourceID="SqlDSSite" DataTextField="SiteName" DataValueField="SiteID" Visible="False"></asp:DropDownList></td>
            <td width="25%">
                <asp:DropDownList ID="ddlCustomer" runat="server" Width="100%" DataSourceID="SqlDSCustomer" DataTextField="CustomerName" DataValueField="CustomerID" AutoPostBack="True">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDSCustomer" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="--SELECT CustomerID,CustomerName FROM v_Customer WHERE IsEnable=1&#13;&#10;--SELECT * FROM fn_GetProjectCustomer(Login)&#13;&#10;/*&#13;&#10;SELECT * FROM v_IssueCustomerOwner WHERE OwnerCode=UserId AND&#13;&#10;CustomerID IN(&#13;&#10;SELECT [CustomerID] FROM [v_Customer] WHERE [IsEnable] = IsEnable AND [IsVirtual] = IsVirtual&#13;&#10;)&#13;&#10;*/&#13;&#10;SELECT [CustomerID], [CustomerName] FROM [v_Customer]&#13;&#10;&#13;&#10;">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDSSite" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT * FROM v_IssueSiteOwner WHERE OwnerCode = @UserId AND&#13;&#10;SiteID IN (&#13;&#10;SELECT [SiteID] FROM [v_Site] WHERE [IsEnable] = @IsEnable AND [IsVirtual] = @IsVirtual&#13;&#10;)&#13;&#10;">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtUserId" Name="UserId" PropertyName="Text" />
                    <asp:Parameter DefaultValue="True" Name="IsEnable" />
                    <asp:Parameter DefaultValue="False" Name="IsVirtual" />
                </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="font-family: 'Arial Unicode MS'; background-color: #ccccff" width="25%">Project</td>
            <td width="25%">
                <asp:DropDownList ID="ddlProject" runat="server" Width="100%" DataSourceID="SqlDSProject" DataTextField="ProjectName" DataValueField="ProjecdtID" AutoPostBack="True">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDSProject" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="--SELECT ProjectID,ProjectName FROM v_Project&#13;&#10;--SELECT * FROM fn_GetProjectList(Login,Customer,Site)&#13;&#10;/*&#13;&#10;SELECT '' as ProjectID ,'-' as ProjectName &#13;&#10;UNION&#13;&#10;SELECT CONVERT(NVARCHAR, [ProjectID]) as  [ProjectID] , [ProjectName] FROM [v_Project] WHERE ([IsEOSL] = IsEOSL AND IsMP=1) &#13;&#10;AND ProjectID IN (SELECT DISTINCT ProjectID FROM v_IssueProjectOwner WHERE OwnerCode = editor) &#13;&#10;AND Customer = CustomerID */&#13;&#10;select ''as ProjecdtID,'-' as ProjectName&#13;&#10;UNION&#13;&#10;select CONVERT(NVARCHAR,GroupID) as ProjectID,Title as ProjectName &#13;&#10;FROM m_Group&#13;&#10;WHERE GroupType='5' AND Parent_GroupID='60' &#13;&#10;AND IsMP=1 AND IsEnable=1 AND IsEOSL=0&#13;&#10;AND dbo.GetXml(Remark,'CUSTOMER')=@Customer&#13;&#10;ORDER BY ProjectName&#13;&#10;">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlCustomer" Name="Customer" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource></td>
                
        </tr>
    </table>
    <table style="font-size: 10pt; width: 100%; font-family: 'Arial Unicode MS'; border-top-style: groove; border-right-style: groove; border-left-style: groove; border-bottom-style: groove;">
        <tr>
            <td style="width: 10%">
                <asp:HyperLink ID="hlFile" runat="server" Font-Names="Arial Narrow" Font-Underline="True" ForeColor="#3333CC" ImageUrl="~/images/xls.png" 
                    NavigateUrl="~/Template/MPIssue.xls" Target="_self">Issue081002.xls</asp:HyperLink></td>
            <td style="width: 80%">
                <asp:FileUpload ID="fuImport" runat="server" Width="100%" /></td>
            <td style="width: 10%">
                <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" /></td>
        </tr>
    </table>
    <asp:TextBox ID="txtUserId" runat="server" Visible="False"></asp:TextBox><br />
    <asp:Button ID="btnDel" runat="server" Text="Del" Width="20%" OnClick="btnDel_Click" />
    <asp:Button ID="btnOpen" runat="server" Text="OK" Width="20%" OnClick="btnOpen_Click" />
    <asp:Label ID="lblPCAMessage" runat="server" Visible=false></asp:Label><br />
    <asp:Label ID="lblMessage" runat="server" Font-Size="10pt" ForeColor="Red" ></asp:Label><br />
    
    <asp:GridView ID="gvDraft" runat="server" BackColor="White" 
        BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="2" AutoGenerateColumns="False" Width="100%" DataSourceID="SqlDSImportIssue">
        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
        <RowStyle BackColor="White" ForeColor="#003399" />
        <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
        <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
        <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Button ID="btnAll" runat="server" Text="ALL" OnClick="btnAll_Click" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="cbcheck" runat="server" />
                    <asp:HiddenField ID="hfIssueID" runat="server" Value='<%# Bind("IssueID") %>'/>
                </ItemTemplate>
                <HeaderStyle Font-Size="9pt" HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Issue date">
                 <ItemTemplate>
                        <asp:HyperLink ID="lblIssueID" runat="server" Text='<%# Bind("IssueDate")%>' Target="_blank" NavigateUrl='<%# "~/MPDraft.aspx?IssueID="+ Eval("IssueID")%>'></asp:HyperLink>
                </ItemTemplate>
                <HeaderStyle Font-Size="9pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="9pt" />
            </asp:TemplateField>
            <asp:BoundField DataField="MaterialType" HeaderText="MaterialType" >
                <HeaderStyle Font-Size="9pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="9pt" />
            </asp:BoundField>
            <asp:BoundField HeaderText="Station" DataField="StationName">
                <HeaderStyle Font-Size="9pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="9pt" />
            </asp:BoundField>
            <asp:BoundField HeaderText="S/N" DataField="SerialNo">
                <HeaderStyle Font-Size="9pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="9pt" />
            </asp:BoundField>
            <asp:BoundField HeaderText="Failure Symptom" DataField="DefectSymptom">
                <HeaderStyle Font-Size="9pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="9pt" />
            </asp:BoundField>
            <asp:BoundField DataField="LiabilityName" HeaderText="Liability">
                <HeaderStyle Font-Size="9pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="9pt" />
            </asp:BoundField>
            <asp:BoundField DataField="StatusName" HeaderText="Status" >
                <HeaderStyle Font-Size="9pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="9pt" />
            </asp:BoundField>
            <asp:BoundField HeaderText="Editor" DataField="Reporter" >
                <HeaderStyle Font-Size="9pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="9pt" />
            </asp:BoundField>
            <asp:BoundField HeaderText="Update Date" DataField="Cdt">
                <HeaderStyle Font-Size="9pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="9pt" />
            </asp:BoundField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDSImportIssue" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT * FROM v_MPImportIssue WHERE ReportBadge=@editor AND Status ='0'&#13;&#10;ORDER BY Cdt DESC">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtUserId" Name="editor" PropertyName="Text" />
        </SelectParameters>
    </asp:SqlDataSource>
    <br />
    <asp:Button ID="btnDel1" runat="server" Text="Del" Width="20%" OnClick="btnDel1_Click"  />
    <asp:Button ID="btnOpen1" runat="server" Text="OK" Width="20%" OnClick="btnOpen1_Click"  />  

</asp:Content>

