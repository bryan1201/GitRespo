<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MProjectOwner.aspx.cs" Inherits="MProjectOwner" Title="=== MQITS:Project Owner Maintenance ===" MaintainScrollPositionOnPostback="true" StylesheetTheme="MMContainer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
        <table style="width:100%;" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width: 150px; background-color: #b6cade;">Module</td> 
                <td style="width: 150px; background-color: #b6cade;">Customer</td>
                <td style="width: 150px; background-color: #b6cade;">Project</td>
                <td style="width: 150px; background-color: #b6cade;">Site</td>
                <td rowspan="3" style="background-color: #b6cade; text-align: center;">
                    <asp:Button ID="btnQuery" runat="server" Font-Names="Arial Rounded MT Bold" 
                        Font-Size="10pt" Height="32px" Text="Query" Width="66px" onclick="btnQuery_Click" />
                </td> 
            </tr>
            <tr>
                <td style="background-color: #eff3f8"><asp:DropDownList ID="ddlModule"
                        runat="server" DataSourceID="SqlDSModule" DataTextField="ModuleName" 
                        DataValueField="ModuleID"></asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDSModule" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                        SelectCommand="SELECT [ModuleID], [ModuleName] FROM v_Module WHERE ModuleID IN(
SELECT DISTINCT ModuleID FROM v_GroupRelationOwner_V2
WHERE OwnerCode=@editor) AND (([IsEnable] = @IsEnable) AND ([IsVirtual] = @IsVirtual)) ORDER BY Rank
">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="hfUserId" DefaultValue="" Name="editor" 
                                PropertyName="Value" />
                            <asp:Parameter DefaultValue="True" Name="IsEnable" Type="Boolean" />
                            <asp:Parameter DefaultValue="False" Name="IsVirtual" Type="Boolean" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td> 
                <td style="background-color: #eff3f8">
                    <asp:DropDownList ID="ddlCustomer" 
                        runat="server" DataSourceID="SqlDSqCustomer" DataTextField="CustomerName" 
                        DataValueField="CustomerID" AutoPostBack="True"></asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDSqCustomer" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                        SelectCommand="SELECT * FROM v_IssueCustomerOwner WHERE OwnerCode=@UserId AND
CustomerID IN(
SELECT [CustomerID] FROM [v_Customer] WHERE [IsEnable] = @IsEnable AND [IsVirtual] = @IsVirtual
)">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="hfUserId" Name="UserId" PropertyName="Value" />
                            <asp:Parameter DefaultValue="True" Name="IsEnable" Type="Boolean" />
                            <asp:Parameter DefaultValue="False" Name="IsVirtual" Type="Boolean" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
                <td style="background-color: #eff3f8">
                    <asp:DropDownList ID="ddlProject" 
                        runat="server" DataSourceID="SqlDSqProject" DataTextField="ProjectName" 
                        DataValueField="ProjectID" AutoPostBack="True" 
                        onselectedindexchanged="ddlProject_SelectedIndexChanged"></asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDSqProject" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                        SelectCommand="SELECT -1 AS ProjectID, '---Select One---'  AS ProjectName, 0 AS Rank
UNION
SELECT [ProjectID], [ProjectName], Rank FROM [v_Project] WHERE ([IsEOSL] = @IsEOSL) AND IsMP = @IsMP
AND ProjectID IN (SELECT DISTINCT ProjectID FROM v_IssueProjectOwner WHERE OwnerCode = @editor) 
AND Customer = @CustomerID ORDER BY [Rank]">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="False" Name="IsEOSL" />
                            <asp:Parameter DefaultValue="False" Name="IsMP" />
                            <asp:ControlParameter ControlID="hfUserId" DefaultValue="IEC970101" 
                                Name="editor" PropertyName="Value" />
                            <asp:ControlParameter ControlID="ddlCustomer" DefaultValue="" Name="CustomerID" 
                                PropertyName="SelectedValue" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
                <td style="background-color: #eff3f8">
                    <asp:DropDownList ID="ddlSite" runat="server" 
                        DataSourceID="SqlDSSite" DataTextField="SiteName" DataValueField="SiteID"></asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDSSite" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                        SelectCommand="SELECT [SiteID],  SiteName FROM [v_Site] WHERE [IsEnable] = @IsEnable AND [IsVirtual] = @IsVirtual
ORDER BY Rank">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="True" Name="IsEnable" Type="Boolean" />
                            <asp:Parameter DefaultValue="False" Name="IsVirtual" Type="Boolean" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
        <table cellpadding="3" cellspacing="3" style="width:100%">
            <tr style="width: 100%">
                <td style="vertical-align:top; background-color: #FFFFCC;">
                <div style="color: #FFFFFF; background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x;">
                    Project Phase Maintenance:</div>
                    <asp:GridView ID="gvProjectPhase" runat="server" AutoGenerateColumns="False" 
                        BackColor="White" BorderColor="#999999" BorderStyle="None" 
                        BorderWidth="1px" CaptionAlign="Left" CellPadding="3" DataKeyNames="ID,ProjectID,PhaseID,SiteID" 
                        DataSourceID="SqlDSProjectPhase" Font-Size="9pt" GridLines="Vertical" 
                        ShowFooter="True" 
                        onselectedindexchanged="gvProjectPhase_SelectedIndexChanged" 
                        AllowPaging="True" onrowupdating="gvProjectPhase_RowUpdating" 
                        EmptyDataText="-- No any phase defined by this project --" Width="100%" 
                                                            onrowdeleting="gvProjectPhase_RowDeleting">
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                                        CommandName="Edit" Text="Edit"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                                        CommandName="Select" Text="Sel"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" 
                                        CommandName="Delete" onclientclick="return confirm('Are you sure?')" Text="Del"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                                        CommandName="Update" Text="Save"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                                        CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PhaseID" HeaderText="PhaseID" SortExpression="PhaseID" 
                                FooterText="PhaseID" ReadOnly="True" Visible="False" />
                            <asp:TemplateField HeaderText="Phase Name" SortExpression="PhaseName">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("PhaseName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPhaseName" runat="server" Text='<%# Bind("PhaseName") %>' 
                                        Columns="10"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton ID="lbtnAdd" runat="server" Font-Size="10pt" 
                                        onclick="lbtnAdd_Click">Add</asp:LinkButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Site Name" SortExpression="SiteName">
                                <ItemTemplate>
                                    <asp:Label ID="Label7" runat="server" Text='<%# Bind("SiteName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlSite" runat="server" DataSourceID="SqlDSSite" 
                                        DataTextField="SiteName" DataValueField="SiteID" 
                                        SelectedValue='<%# Bind("SiteID", "{0}") %>'>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rank" SortExpression="Rank">
                                <ItemTemplate>
                                    <asp:Label ID="Label8" runat="server" Text='<%# Bind("Rank") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Columns="2" 
                                        Text='<%# Bind("Rank", "{0}") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Start Date" SortExpression="StartDate">
                                <ItemTemplate>
                                    <asp:Label ID="Label9" runat="server" Text='<%# Bind("StartDate", "{0:d}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtSDate" runat="server" Columns="8" 
                                        Text='<%# Bind("StartDate", "{0:d}") %>'></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revSDate" runat="server" 
                                        ControlToValidate="txtSDate" Display="Dynamic" ErrorMessage="&lt;br /&gt;yyyy/MM/dd" 
                                        ValidationExpression="(19|20)\d\d[- /.](0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])"></asp:RegularExpressionValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:CheckBoxField DataField="IsInUse" HeaderText="IsInUse" 
                                SortExpression="IsInUse" />
                            <asp:TemplateField HeaderText="Input EndDate" 
                                SortExpression="InputQtyEndDate">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtInputQtyEndDate" runat="server" Columns="8" 
                                        Text='<%# Bind("InputQtyEndDate", "{0:d}") %>'></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revInputEDate" runat="server" 
                                        ControlToValidate="txtInputQtyEndDate" Display="Dynamic" 
                                        ErrorMessage="&lt;br /&gt;yyyy/MM/dd" 
                                        ValidationExpression="(19|20)\d\d[- /.](0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])"></asp:RegularExpressionValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Button ID="btnTransferToMP" runat="server" CausesValidation="False" 
                                        Font-Bold="True" Font-Names="Century Gothic" ForeColor="#9966FF" 
                                        onclick="btnTransferToMP_Click" onclientclick="return confirm('Are you sure?')" 
                                        Text="Transfer To MP" Width="100px" />
                                </FooterTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label11" runat="server" 
                                        Text='<%# Bind("InputQtyEndDate", "{0:d}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="End Date" SortExpression="EndDate">
                                <ItemTemplate>
                                    <asp:Label ID="Label10" runat="server" Text='<%# Bind("EndDate", "{0:d}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEDate" runat="server" Columns="8" 
                                        Text='<%# Bind("EndDate", "{0:d}") %>'></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revEDate" runat="server" 
                                        ControlToValidate="txtEDate" Display="Dynamic" 
                                        ErrorMessage="&lt;br /&gt;yyyy/MM/dd" 
                                        ValidationExpression="(19|20)\d\d[- /.](0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])"></asp:RegularExpressionValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Phase Input Qty" SortExpression="PhaseInputQty">
                                <ItemTemplate>
                                    <asp:Label ID="Label6" runat="server" Text='<%# Bind("PhaseInputQty") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox6" runat="server" Columns="6" 
                                        Text='<%# Bind("PhaseInputQty", "{0}") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="#DCDCDC" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDSProjectPhase" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                        
                        
                                                            
                                                            SelectCommand="SELECT ID, ProjectID, SiteID, SiteName, [PhaseID], [PhaseName], [Rank], [StartDate], [IsInUse],  InputQtyEndDate, [EndDate], [PhaseInputQty] FROM [v_ProjectPhase] WHERE ([CustomerID] = @CustomerID) AND ([ProjectID] = @ProjectID) ORDER BY [Rank]">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlCustomer" Name="CustomerID" 
                                PropertyName="SelectedValue" Type="String" />
                            <asp:ControlParameter ControlID="ddlProject" Name="ProjectID" 
                                PropertyName="SelectedValue" Type="Int64" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:HiddenField ID="hfUserId" runat="server" />
                    <asp:HiddenField ID="hfProjectID" runat="server" />
                    <asp:HiddenField ID="hfModuleID" runat="server" />
                    <asp:HiddenField ID="hfCustomerID" runat="server" />
                    <asp:HiddenField ID="hfSiteID" runat="server" />
                    <asp:HiddenField ID="hfPhaseID" runat="server" />
                    <asp:HiddenField ID="hfCurrentPhaseID" runat="server" />
    <asp:FormView ID="fvProjectPhase" runat="server" BorderStyle="Solid" 
       BorderWidth="2px" CellPadding="0" ForeColor="#333333" 
        BorderColor="#FFE0C0" DefaultMode="Insert" DataKeyNames="ID" 
       DataSourceID="SqlDSProjectPhase" oniteminserting="fvProjectPhase_ItemInserting" 
                        Visible="False" onmodechanging="fvProjectPhase_ModeChanging">
        <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
        <InsertItemTemplate>
            <asp:LinkButton ID="lbtnInsertProjectPhase" runat="server" CausesValidation="True" CommandName="Insert"
                Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Save"></asp:LinkButton>
            <asp:LinkButton ID="lbtnInsertProjectPhaseCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Cancel"></asp:LinkButton>
           <table border="0" cellpadding="2" cellspacing="1" style="padding-right: 0px; padding-left: 0px;
                font-size: 10pt; padding-bottom: 0px; margin: 0px; border-top-style: none; padding-top: 0px;
                font-family: 'Arial Unicode MS'; border-right-style: none; border-left-style: none;
                border-bottom-style: none" width="100%">
                <tr>
                    <td class="FVTDINSCol1">Phase Name</td>
                    <td class="FVTDINSCol2">
                        <asp:TextBox ID="txtPhaseName" runat="server" Columns="15" 
                            Text='<%# Bind("PhaseName", "{0}") %>'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="FVTDINSCol1">Rank</td>
                    <td class="FVTDINSCol2">
                        <asp:TextBox ID="txtRank" runat="server" Columns="6" 
                            Text='<%# Bind("Rank", "{0}") %>'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="FVTDINSCol1">Site</td>
                    <td class="FVTDINSCol2">
                        <asp:DropDownList ID="ddlSite" runat="server" DataSourceID="SqlDSSite" 
                            DataValueField="SiteID" DataTextField="SiteName" 
                            SelectedValue='<%# Bind("SiteID", "{0}") %>'>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </InsertItemTemplate>
        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
    </asp:FormView><br />
    <table style="border: 1px solid #6666FF; padding: 1px; width: 100%; background-color: #FFCCFF;" frame="below" 
                        cellpadding="0" 
                        cellspacing="0" title="Project Material Maintenance">
        <thead>
            <tr><td colspan="2" 
                    style="color: #FFFFFF; background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x;">Project Material Maintenence:</td></tr>
        </thead>
        <tbody>
        <tr>
            <td style="vertical-align:top;">
                <asp:GridView ID="gvProjectMaterial" runat="server" AutoGenerateColumns="False" 
                    BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" 
                    CellPadding="2" DataKeyNames="ID" DataSourceID="SqlDSProjectMaterial" 
                    Font-Size="10pt" ForeColor="Black" GridLines="Vertical" 
                    onrowupdating="gvProjectMaterial_RowUpdating" ShowFooter="True" 
                    CaptionAlign="Left" Width="100%" 
                    onrowdeleting="gvProjectMaterial_RowDeleting">
                    <FooterStyle BackColor="Tan" />
                    <Columns>
                        <asp:CommandField CancelText="Cancel" DeleteText="Del" EditText="Edit" 
                            InsertText="Save" NewText="Add" SelectText="Sel" ShowEditButton="True" 
                            UpdateText="Save" />
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                                    CommandName="Delete" Text="Del" OnClientClick="return confirm('Are you sure?')"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Material" SortExpression="Material">
                            <ItemTemplate>
                                <asp:Label ID="Label11" runat="server" Text='<%# Bind("Material") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtMaterial" runat="server" Columns="12" 
                                    Text='<%# Bind("Material") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lbtnAddProjectMaterial" runat="server" Font-Size="11pt" 
                                    ForeColor="#660033" onclick="lbtnAddProjectMaterial_Click">Add</asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="MType" SortExpression="MType">
                            <ItemTemplate>
                                <asp:Label ID="Label12" runat="server" Text='<%# Bind("MType") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlMType" runat="server" 
                                    SelectedValue='<%# Bind("MType", "{0}") %>'>
                                    <asp:ListItem Selected="True" Value="PCA"></asp:ListItem>
                                    <asp:ListItem Value="CPU"></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CheckBoxField DataField="IsInUse" HeaderText="IsInUse" 
                            SortExpression="IsInUse" />
                        <asp:BoundField DataField="editorName" HeaderText="editor" ReadOnly="True" 
                            SortExpression="editorName" />
                        <asp:BoundField DataField="udt" DataFormatString="{0:d}" HeaderText="udt" 
                            SortExpression="udt" ReadOnly="True" />
                    </Columns>
                    <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                        HorizontalAlign="Center" />
                    <EmptyDataTemplate>
                        <asp:LinkButton ID="lbtnAddProjectMaterial" runat="server" 
                            onclick="lbtnAddProjectMaterial_Click">Add Project Material</asp:LinkButton>
                    </EmptyDataTemplate>
                    <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                    <HeaderStyle BackColor="Tan" Font-Bold="True" />
                    <AlternatingRowStyle BackColor="PaleGoldenrod" />
                </asp:GridView>
            </td>
            <td style="vertical-align:top;">
                <asp:FormView ID="fvProjectMaterial" runat="server" BorderStyle="Solid" 
                   BorderWidth="2px" CellPadding="0" ForeColor="#333333" 
                    BorderColor="#FFE0C0" DefaultMode="Insert" DataKeyNames="ID" 
                   DataSourceID="SqlDSProjectMaterialOne" oniteminserting="fvProjectMaterial_ItemInserting" 
                                    Visible="False" 
                    onmodechanging="fvProjectMaterial_ModeChanging">
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                    <InsertItemTemplate>
                        <asp:LinkButton ID="lbtnInsertProjectMaterial" runat="server" 
                            CausesValidation="True" CommandName="Insert"
                            Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Save"></asp:LinkButton>
                        <asp:LinkButton ID="lbtnInsertProjectMaterialCancel" runat="server" 
                            CausesValidation="False" CommandName="Cancel"
                            Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Cancel"></asp:LinkButton>
                       <table border="0" cellpadding="2" cellspacing="1" style="padding-right: 0px; padding-left: 0px;
                            font-size: 10pt; padding-bottom: 0px; margin: 0px; border-top-style: none; padding-top: 0px;
                            font-family: 'Arial Unicode MS'; border-right-style: none; border-left-style: none;
                            border-bottom-style: none" width="100%">
                            <tr>
                                <td class="FVTDINSCol1">Material</td>
                                <td class="FVTDINSCol2">
                                    <asp:TextBox ID="txtMaterial" runat="server" Columns="15" 
                                        Text='<%# Bind("Material", "{0}") %>' ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="FVTDINSCol1">MType</td>
                                <td class="FVTDINSCol2">
                                    <asp:DropDownList ID="ddlSite0" runat="server" DataSourceID="SqlDSMType" 
                                        DataValueField="VALUE" DataTextField="TEXT" 
                                        SelectedValue='<%# Bind("MType", "{0}") %>'>
                                        <asp:ListItem Value="PCA"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDSMType" runat="server" 
                                        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                                        SelectCommand="SELECT * FROM [v_ProjectMType]"></asp:SqlDataSource>
                                </td>
                            </tr>
                        </table>
                    </InsertItemTemplate>
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                </asp:FormView>
            </td>
        </tr>
        </tbody>
    </table>
        <asp:SqlDataSource ID="SqlDSProjectMaterial" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
            
                        SelectCommand="SELECT [ID], [Material], [MType], [Rank], [IsInUse], [editorName], [udt]  FROM [v_ProjectMaterial] WHERE ([ProjectID] = @ProjectID) ORDER BY Rank">
            <SelectParameters>
                <asp:ControlParameter ControlID="hfProjectID" Name="ProjectID" 
                    PropertyName="Value" Type="Int64" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDSProjectMaterialOne" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
            
            SelectCommand="SELECT [ID], [Material], [MType], [Rank], [IsInUse], [editorName], [udt]  FROM [v_ProjectMaterial] WHERE ID =@ID">
            <SelectParameters>
                <asp:ControlParameter ControlID="gvProjectMaterial" Name="ID" 
                    PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
                                                        <br />
        <table style="width:100%;">
            <thead><tr><td style="color: #FFFFFF; background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x; width:100%">
            PCA/CPU Yield Maintenance by Project-Phase
            </td></tr></thead>
        <tbody style="width: 100%"><tr><td style="width: 100%">
                    <asp:GridView ID="gvProjectMaterialYield" runat="server" AutoGenerateColumns="False" 
                        BackColor="White" BorderColor="#999999" BorderStyle="None" 
                        BorderWidth="1px" CaptionAlign="Left" CellPadding="3" DataKeyNames="ID" 
                        DataSourceID="SqlDSProjectMaterialYield" Font-Size="9pt" GridLines="Vertical" 
                        ShowFooter="True" 
                        onrowupdating="gvProjectMaterialYield_RowUpdating" 
                        EmptyDataText="-- No any phase defined by this project --" Width="100%">
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <Columns>
                            <asp:CommandField CancelText="Cancel" EditText="Edit" SelectText="Sel" 
                                ShowEditButton="True" UpdateText="Save" />
                            <asp:BoundField DataField="Material" HeaderText="Material" ReadOnly="True" 
                                SortExpression="Material" >
                                <HeaderStyle BackColor="#333399" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="ICT Yield (%)" SortExpression="ICTYield">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtICTYield" runat="server" Columns="4" 
                                        Text='<%# Bind("ICTYield", "{0:N}") %>' Visible='<%# Eval("IsPCA") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("ICTYield", "{0:N}%") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle BackColor="#009933" BorderStyle="Solid" BorderWidth="2px" 
                                    ForeColor="#FFFFCC" />
                                <ItemStyle BorderColor="#009933" BorderStyle="Solid" BorderWidth="2px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SA Yield (%)" SortExpression="SAYield">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtSAYield" runat="server" Columns="4" 
                                        Text='<%# Bind("SAYield") %>' Visible='<%# Eval("IsPCA") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("SAYield", "{0:N}%") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle BackColor="#009933" BorderStyle="Solid" BorderWidth="2px" />
                                <ItemStyle BorderColor="#009933" BorderStyle="Solid" BorderWidth="2px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PreTest Yield (%)" SortExpression="PreTestYield">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPreTestYield" runat="server" Columns="4" 
                                        Text='<%# Bind("PreTestYield", "{0:N}") %>' Visible='<%# Eval("IsCPU") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" 
                                        Text='<%# Bind("PreTestYield", "{0:N}%") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RunIn Yield (%)" SortExpression="RunInYield">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtRunInYield" runat="server" Columns="4" 
                                        Text='<%# Bind("RunInYield", "{0}") %>' Visible='<%# Eval("IsCPU") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("RunInYield", "{0:N}%") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FPY (%)" SortExpression="FPY">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtFPY" runat="server" Columns="4" 
                                        Text='<%# Bind("FPY", "{0}") %>' ReadOnly="True" 
                                        Visible='<%# Eval("IsCPU") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("FPY", "{0:N}%") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="editorName" HeaderText="editor" ReadOnly="True" 
                                SortExpression="editorName" />
                            <asp:BoundField DataField="udt" DataFormatString="{0:d}" HeaderText="udt" 
                                ReadOnly="True" SortExpression="udt" />
                        </Columns>
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="#DCDCDC" />
                    </asp:GridView>
                <asp:SqlDataSource ID="SqlDSProjectMaterialYield" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                    
                        SelectCommand="SELECT [ID], [Material], [ICTYield], [SAYield], [PreTestYield], [RunInYield], [FPY], [editorName], [udt], IsCPU, IsPCA FROM [v_ProjectMaterialYield] WHERE ([ProjectPhaseID] = @ProjectPhaseID) ORDER BY [Material]">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hfCurrentPhaseID" Name="ProjectPhaseID" 
                            PropertyName="Value" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
                </td>
                </tr>
                </tbody>
                </table>
                </td>
                <td style="vertical-align:top; background-color: #CCFFCC;">
                    <table style="width:100%; background-color: #CCFFCC;">
                        <thead style="background-color: #99CCFF; color: #990099;"><tr>
                            <td style="background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x; color: #FFFFFF">Project Owners</td></tr></thead>
                        <tbody>
                        <tr>
                            <td>
                                <asp:GridView ID="gvGroupRelationOwner" runat="server" AllowSorting="True" 
                                    AutoGenerateColumns="False" BackColor="LightGoldenrodYellow" BorderColor="Tan" 
                                    BorderWidth="1px" CaptionAlign="Left" CellPadding="2" 
                                    DataKeyNames="ID" 
                                    DataSourceID="SqlDSOwner" Font-Size="10pt" ForeColor="Black" 
                                    GridLines="Vertical" ShowFooter="True" 
                                    onrowdeleting="gvGroupRelationOwner_RowDeleting">
                                    <FooterStyle BackColor="Tan" />
                                    <Columns>
                                        <asp:CommandField DeleteText="Remove" ShowDeleteButton="True" />
                                        <asp:BoundField DataField="OwnerCode" HeaderText="Owner Code" 
                                            SortExpression="OwnerCode" />
                                        <asp:BoundField DataField="OwnerName" HeaderText="Owner Name" ReadOnly="True" 
                                            SortExpression="OwnerName" />
                                        <asp:CheckBoxField DataField="IsEnable" HeaderText="Enable" 
                                            SortExpression="IsEnable" Visible="False" />
                                        <asp:CheckBoxField DataField="IsReadOnly" HeaderText="ReadOnly" 
                                            SortExpression="IsReadOnly" Visible="False" />
                                        <asp:BoundField DataField="SiteName" HeaderText="Site" 
                                            SortExpression="SiteName" />
                                        <asp:BoundField DataField="OwnerType" HeaderText="Owner Type" 
                                            SortExpression="OwnerType" Visible="False" />
                                    </Columns>
                                    <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                                        HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                    <HeaderStyle BackColor="Tan" Font-Bold="True" />
                                    <AlternatingRowStyle BackColor="PaleGoldenrod" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDSOwner" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                                    
                                    
                                    SelectCommand="SELECT ID,  [OwnerType], [OwnerCode], [OwnerName], [IsEnable], [IssueType], [IsReadOnly], SiteID,[SiteName] FROM [v_GroupRelationOwner] WHERE (([ProjectID] = @ProjectID) AND ([ModuleID] = @ModuleID) AND SiteID = @SiteID) AND IssueType=1">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddlProject" Name="ProjectID" 
                                            PropertyName="SelectedValue" Type="Int64" />
                                        <asp:ControlParameter ControlID="ddlModule" Name="ModuleID" 
                                            PropertyName="SelectedValue" Type="Int64" />
                                        <asp:ControlParameter ControlID="ddlSite" Name="SiteID" 
                                            PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
        <div style="background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x; color: #FFFFFF;">
            Project Members</div>
                                <asp:GridView ID="gvGroupRelationMember" runat="server" AllowSorting="True" 
                                    AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" 
                                    BorderWidth="1px" CaptionAlign="Left" CellPadding="4" 
                                    DataKeyNames="ID" 
                                    DataSourceID="SqlDSMember" Font-Size="10pt" ForeColor="Black" 
                                    GridLines="Horizontal" ShowFooter="True" BorderStyle="None" 
                                    onrowdeleting="gvGroupRelationMember_RowDeleting">
                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                    <Columns>
                                        <asp:CommandField DeleteText="Remove" ShowDeleteButton="True" />
                                        <asp:BoundField DataField="OwnerCode" HeaderText="Member Code" 
                                            SortExpression="OwnerCode" />
                                        <asp:BoundField DataField="OwnerName" HeaderText="Member Name" ReadOnly="True" 
                                            SortExpression="OwnerName" />
                                        <asp:CheckBoxField DataField="IsEnable" HeaderText="Enable" 
                                            SortExpression="IsEnable" Visible="False" />
                                        <asp:CheckBoxField DataField="IsReadOnly" HeaderText="ReadOnly" 
                                            SortExpression="IsReadOnly" Visible="False" />
                                        <asp:BoundField DataField="SiteName" HeaderText="Site" 
                                            SortExpression="SiteName" />
                                        <asp:BoundField DataField="OwnerType" HeaderText="Owner Type" 
                                            SortExpression="OwnerType" Visible="False" />
                                    </Columns>
                                    <PagerStyle BackColor="White" ForeColor="Black" 
                                        HorizontalAlign="Right" />
                                    <SelectedRowStyle BackColor="#CC3333" ForeColor="White" Font-Bold="True" />
                                    <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDSMember" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                                    
                                    
                                                                            
                                                                            SelectCommand="SELECT ID,  [OwnerType], [OwnerCode], [OwnerName], [IsEnable], [IssueType], [IsReadOnly], SiteID,[SiteName] FROM [v_GroupRelationOwner] WHERE (([ProjectID] = @ProjectID) AND ([ModuleID] = @ModuleID)  AND SiteID = @SiteID) AND IssueType=2">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddlProject" Name="ProjectID" 
                                            PropertyName="SelectedValue" Type="Int64" />
                                        <asp:ControlParameter ControlID="ddlModule" Name="ModuleID" 
                                            PropertyName="SelectedValue" Type="Int64" />
                                        <asp:ControlParameter ControlID="ddlSite" Name="SiteID" 
                                            PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:SqlDataSource>                            
                            </td>
                        </tr>
                        </tbody>
                    </table>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 200px; font-size: 10pt; background-image: url('images/5bg_Nav.jpg'); color: #FFFFFF;">
                                Find Role/User:</td>
                        </tr>
                        <tr>
                            <td style="background-color: #eff3f8"><asp:TextBox ID="txtqRoleUser" runat="server" 
                                    Font-Size="12pt"></asp:TextBox><asp:Button ID="btnFindOwner" Text="Find" 
                                    runat="server" onclick="btnFindOwner_Click" /></td>
                        </tr>
                        <tr>
                            <td style="background-color: #b6cade; width: 200px; font-size: 10pt;">
                                <table>
                                    <tr>
                                        <td style="width: 200px; font-size: 10pt; background-image: url('images/5bg_Nav.jpg'); color: #FFFFFF;">Add Owner/Member:</td>
                                        <td style="background-image: url('images/5bg_Nav.jpg'); color: #FFFFFF;">
                                            <asp:DropDownList ID="ddlIssueType" runat="server">
                                                <asp:ListItem Value="1" Text="Owner" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Member"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    </table>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>

                                <asp:GridView ID="gvRoleUser" runat="server" AutoGenerateColumns="False" 
                                    BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" 
                                    Caption="Please select Role or User;" CaptionAlign="Left" CellPadding="4" 
                                    DataKeyNames="OwnerType,OwnerCode" DataSourceID="SqlDSRoleUser" 
                                    Font-Size="10pt" 
                                    ShowFooter="True" onselectedindexchanged="gvRoleUser_SelectedIndexChanged">
                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                    <RowStyle BackColor="White" ForeColor="#003399" />
                                    <Columns>
                                        <asp:CommandField SelectText="Select" ShowSelectButton="True" />
                                        <asp:BoundField DataField="OwnerType" HeaderText="Role User" 
                                            SortExpression="OwnerType" />
                                        <asp:BoundField DataField="OwnerCode" HeaderText="Owner Code" 
                                            SortExpression="OwnerCode" />
                                        <asp:BoundField DataField="OwnerName" HeaderText="Owner Name" 
                                            SortExpression="OwnerName" />
                                    </Columns>
                                    <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                                    <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                    <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDSRoleUser" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                                    SelectCommand="SELECT 'U' AS OwnerType, BadgeCode AS OwnerCode, ChtName AS OwnerName
                        FROM v_user WHERE IsInUse='True' AND (UPPER(ChtName) LIKE '%' + UPPER(@RoleName) + '%'  OR UPPER(BadgeCode) LIKE '%' + UPPER(@RoleName) + '%')
                        UNION
                        SELECT 'R' AS OwnerType, 
                        CONVERT(NVARCHAR,[RoleCode]) AS OwnerCode,
                        [RoleName] AS OwnerName FROM [v_role] WHERE [ModuleCode] = @ModuleCode AND UPPER([RoleName]) LIKE '%' + UPPER(@RoleName) + '%'">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="txtqRoleUser" Name="RoleName" 
                                            PropertyName="Text" Type="String" />
                                        <asp:ControlParameter ControlID="ddlModule" Name="ModuleCode" 
                                            PropertyName="SelectedValue" Type="Int32" />
                                    </SelectParameters>
                                </asp:SqlDataSource>                            
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
</asp:Content>

