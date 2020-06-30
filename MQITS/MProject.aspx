<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MProject.aspx.cs" Inherits="MProject" StylesheetTheme="MMContainer" Title="=== MQITS::Project Maintain ===" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
    <div style="clear: both;">
    <table style="width:100%">
        <tr>  
        <td style="vertical-align:top;"> 
        <div style="background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x; color: #FFFFFF;">
            Project Maintenance: Customer/Site/IsEOSL
        </div>
        <table>
            <tr>
                <td style="vertical-align:middle; width: 125px; font-size: 10pt;">Project Name:</td>
                <td style="vertical-align:middle; width: 70%;">
                    <asp:TextBox ID="txtProject" runat="server" Width="95%"></asp:TextBox></td>
                <td style="vertical-align:middle; ">
                    <asp:Button ID="btnFindProject" runat="server" Font-Bold="True" 
                        Font-Names="Arial Unicode MS" Text="Find" Width="68px" />
                    <br />
                    <asp:Button ID="btnAddProject" runat="server" Font-Bold="True" 
                        Font-Names="Arial Unicode MS" Text="Add" Width="68px" 
                        OnClientClick="return confirm('Are you sure to add new Project?')" 
                        onclick="btnAddProject_Click" /></td>
            </tr>
            <tr>
                <td colspan="3" style="vertical-align:top;">
                    <asp:GridView ID="gvProject" runat="server"
                        CaptionAlign="Left" CellPadding="4" ForeColor="#333333"
                        AutoGenerateColumns="False" ShowFooter="True" DataKeyNames="ProjectID,Customer,Site" 
                        DataSourceID="SqlDSProject" Font-Size="10pt" AllowSorting="True" 
                        onrowupdating="gvProject_RowUpdating"
                        onselectedindexchanged="gvProject_SelectedIndexChanged">
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:CommandField CancelText="Cancel" DeleteText="Del" EditText="Edit" 
                                InsertText="Save" NewText="Add" SelectText="Select" ShowEditButton="True" 
                                UpdateText="Update" />
                            <asp:TemplateField HeaderText="ID" ShowHeader="False" 
                                SortExpression="ProjectID">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnSelProject" runat="server" CausesValidation="False" 
                                        CommandName="Select" Text='<%# Eval("ProjectID", "{0}") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="ProjectName" DataField="ProjectName" 
                                SortExpression="ProjectName" ReadOnly="True" />
                            <asp:CheckBoxField DataField="IsEOSL" HeaderText="IsEOSL" 
                                SortExpression="IsEOSL" />
                            <asp:BoundField HeaderText="Rank" DataField="Rank" SortExpression="Rank" />
                            <asp:TemplateField HeaderText="Customer" SortExpression="Customer">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlCustomer" runat="server" DataSourceID="SqlDSCustomer" 
                                        DataTextField="CustomerName" DataValueField="CustomerID" 
                                        SelectedValue='<%# Bind("Customer", "{0}") %>'>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDSCustomer" runat="server" 
                                        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                                        SelectCommand="SELECT N'' AS CustomerID, N'' AS CustomerName, 0 AS Rank
                                                    UNION
                                                    SELECT CONVERT(NVARCHAR,[CustomerID]) AS CustomerID, [CustomerName], Rank
                                                    FROM [v_Customer] WHERE (([IsEOSL] = @IsEOSL) AND ([CustomerID] &lt;&gt; @CustomerID)) ORDER BY [Rank]">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="False" Name="IsEOSL" Type="Boolean" />
                                            <asp:Parameter DefaultValue="20" Name="CustomerID" Type="Int64" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CheckBoxField DataField="IsMP" HeaderText="Is MP" SortExpression="IsMP" />
                            <asp:TemplateField HeaderText="editor" SortExpression="editor">
                                <EditItemTemplate>
                                    <asp:HyperLink ID="hleditor" runat="server" 
                                        NavigateUrl='<%# Eval("editorEmail", "{0}") %>' 
                                        Text='<%# Eval("editorName", "{0}") %>'></asp:HyperLink>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:HyperLink ID="hleditor" runat="server" 
                                        NavigateUrl='<%# Eval("editorEmail", "{0}") %>' 
                                        Text='<%# Eval("editorName", "{0}") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="udt" HeaderText="udt" ReadOnly="True" 
                                SortExpression="udt" />
                        </Columns>
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>                
                </td>
            </tr>
        </table>

        <asp:SqlDataSource ID="SqlDSProject" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
            
                SelectCommand="SELECT * FROM [v_Project] WHERE (UPPER(ProjectName) LIKE '%' + UPPER(@ProjectName)+ '%')">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtProject" Name="ProjectName" 
                    PropertyName="Text" Type="String" />
            </SelectParameters>
            </asp:SqlDataSource>    
        <div style="background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x; color: #FFFFFF;">
            Project Material Maintenance</div>
        <asp:GridView ID="gvProjectMaterial" runat="server" AutoGenerateColumns="False" 
            BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" 
            CellPadding="2" DataKeyNames="ID" DataSourceID="SqlDSProjectMaterial" 
            Font-Size="10pt" ForeColor="Black" GridLines="Vertical" 
            onrowupdating="gvProjectMaterial_RowUpdating" ShowFooter="True">
            <FooterStyle BackColor="Tan" />
            <Columns>
                <asp:CommandField CancelText="Cancel" DeleteText="Del" EditText="Edit" 
                    InsertText="Save" NewText="Add" SelectText="Sel" ShowEditButton="True" 
                    UpdateText="Save" />
                <asp:TemplateField HeaderText="Material" SortExpression="Material">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Material") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtMaterial" runat="server" Columns="12" 
                            Text='<%# Bind("Material") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:LinkButton ID="lbtnAdd" runat="server" Font-Size="11pt" 
                            ForeColor="#660033" onclick="lbtnAdd_Click">Add</asp:LinkButton>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="MType" SortExpression="MType">
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("MType") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlMType" runat="server" 
                            SelectedValue='<%# Bind("MType", "{0}") %>'>
                            <asp:ListItem Selected="True" Value="PCA"></asp:ListItem>
                            <asp:ListItem Value="CPU"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rank" SortExpression="Rank">
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("Rank") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRank" runat="server" Columns="4" 
                            Text='<%# Bind("Rank", "{0}") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:CheckBoxField DataField="IsInUse" HeaderText="IsInUse" 
                    SortExpression="IsInUse" />
                <asp:BoundField DataField="editorName" HeaderText="editor" ReadOnly="True" 
                    SortExpression="editorName" />
                <asp:BoundField DataField="udt" DataFormatString="{0:d}" HeaderText="udt" 
                    SortExpression="udt" />
            </Columns>
            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                HorizontalAlign="Center" />
            <EmptyDataTemplate>
                <asp:LinkButton ID="lbtnAdd" runat="server" onclick="lbtnAdd_Click">Add Project 
                Material</asp:LinkButton>
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
            <HeaderStyle BackColor="Tan" Font-Bold="True" />
            <AlternatingRowStyle BackColor="PaleGoldenrod" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDSProjectMaterial" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
            
            SelectCommand="SELECT [ID], [Material], [MType], [Rank], [IsInUse], [editorName], [udt]  FROM [v_ProjectMaterial] WHERE ([ProjectID] = @ProjectID) ORDER BY Rank">
            <SelectParameters>
                <asp:ControlParameter ControlID="gvProject" Name="ProjectID" 
                    PropertyName="SelectedValue" Type="Int64" />
            </SelectParameters>
        </asp:SqlDataSource>
    <asp:FormView ID="fvProjectMaterial" runat="server" BorderStyle="Solid" 
       BorderWidth="2px" CellPadding="0" ForeColor="#333333" 
        BorderColor="#FFE0C0" DefaultMode="Insert" DataKeyNames="ID" 
       DataSourceID="SqlDSProjectMaterialOne" oniteminserting="fvProjectMaterial_ItemInserting" 
                        Visible="False" onitemcommand="fvProjectMaterial_ItemCommand">
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
                        <asp:TextBox ID="txtPhaseName" runat="server" Columns="15" 
                            Text='<%# Bind("Material", "{0}") %>'></asp:TextBox>
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
        <asp:SqlDataSource ID="SqlDSProjectMaterialOne" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
            
            SelectCommand="SELECT [ID], [Material], [MType], [Rank], [IsInUse], [editorName], [udt]  FROM [v_ProjectMaterial] WHERE ID =@ID">
            <SelectParameters>
                <asp:ControlParameter ControlID="gvProjectMaterial" Name="ID" 
                    PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
        
        
        
    </td>
    <td style="vertical-align:top; width:50%">
        <table style="width:100%">
            <tr>
                <td style="background-color: #b6cade; background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x; color: #FFFFFF;" 
                    colspan="2">Project Owner Management: by Module/Site</td>
            </tr>
            <tr>
                <td style="width: 150px; background-color: #b6cade;">Module</td>
                <td style="width: 150px; background-color: #b6cade;">Site</td>
            </tr>
            <tr>
                <td style="background-color: #eff3f8"><asp:DropDownList ID="ddlModule" 
                        runat="server" DataSourceID="SqlDSModule" DataTextField="ModuleName" 
                        DataValueField="ModuleID"></asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDSModule" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                        SelectCommand="SELECT [ModuleID], [ModuleName] FROM [v_Module] WHERE (([IsEnable] = @IsEnable) AND ([IsVirtual] = @IsVirtual)) ORDER BY [Rank]">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="True" Name="IsEnable" Type="Boolean" />
                            <asp:Parameter DefaultValue="False" Name="IsVirtual" Type="Boolean" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
                <td style="background-color: #eff3f8"><asp:DropDownList ID="ddlSite" runat="server" 
                        DataSourceID="SqlDSSite" DataTextField="SiteName" DataValueField="SiteID"></asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDSSite" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                        SelectCommand="SELECT [SiteID], [SiteName] FROM [v_Site] WHERE (([IsEnable] = @IsEnable) AND ([IsVirtual] = @IsVirtual)) ORDER BY [Rank]">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="True" Name="IsEnable" Type="Boolean" />
                            <asp:Parameter DefaultValue="False" Name="IsVirtual" Type="Boolean" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="background-color: #b6cade;">
                    <table style="width:60%; background-color: #eff3f8;">
                        <tr style="background-color: #b6cade">
                            <td style="background-color: #b6cade; width: 200px; font-size: 10pt;">Find Role/User:</td>
                            <td style="background-color: #eff3f8">
                                <asp:TextBox ID="txtqRoleUser" runat="server" 
                                    Font-Size="12pt" Width="300px"></asp:TextBox></td>
                            <td style="background-color: #eff3f8"><asp:Button ID="btnFindOwner" Text="Find" 
                                    runat="server" onclick="btnFindOwner_Click" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div style="background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x; color: #FFFFFF;">
            Project Owners
        </div>
        <asp:GridView ID="gvGroupRelationOwner" runat="server" AllowSorting="True" 
            AutoGenerateColumns="False" BackColor="LightGoldenrodYellow" BorderColor="Tan" 
            BorderWidth="1px" CaptionAlign="Left" CellPadding="2" 
            DataKeyNames="OwnerID,ID,ModuleID,CustomerID,SiteID,ProjectID" 
            DataSourceID="SqlDSOwner" Font-Size="10pt" ForeColor="Black" 
            GridLines="Vertical" ShowFooter="True" 
            onrowdeleting="gvGroupRelationOwner_RowDeleting" 
            onsorting="gvGroupRelationOwner_Sorting">
            <FooterStyle BackColor="Tan" />
            <Columns>
                <asp:CommandField DeleteText="Remove" ShowDeleteButton="True" />
                <asp:BoundField DataField="ModuleName" HeaderText="Module" ReadOnly="True" 
                    SortExpression="ModuleName" />
                <asp:BoundField DataField="CustomerName" HeaderText="Customer" 
                    ReadOnly="True" SortExpression="CustomerName" />
                <asp:BoundField DataField="SiteName" HeaderText="Site" ReadOnly="True" 
                    SortExpression="SiteName" />
                <asp:BoundField DataField="OwnerCode" HeaderText="OwnerCode" 
                    SortExpression="OwnerCode" />
                <asp:BoundField DataField="OwnerName" HeaderText="OwnerName" ReadOnly="True" 
                    SortExpression="OwnerName" />
                <asp:CheckBoxField DataField="IsEnable" HeaderText="IsEnable" 
                    SortExpression="IsEnable" Visible="False" />
                <asp:CheckBoxField DataField="IsReadOnly" HeaderText="IsReadOnly" 
                    SortExpression="IsReadOnly" Visible="False" />
                <asp:BoundField DataField="OwnerType" HeaderText="OwnerType" 
                    SortExpression="OwnerType" />
            </Columns>
            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
            <HeaderStyle BackColor="Tan" Font-Bold="True" />
            <AlternatingRowStyle BackColor="PaleGoldenrod" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDSOwner" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
            SelectCommand="">
            <SelectParameters>
                <asp:SessionParameter SessionField="MProject_gvProject_sqlCmd" Type="String" Name="MProject_gvProject_sqlCmd" />
            </SelectParameters>
        </asp:SqlDataSource>
    
        <div style="background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x; color: #FFFFFF;">
            Select to add Project Owner</div>
    
        <asp:GridView ID="gvRoleUser" runat="server" AutoGenerateColumns="False" 
            BackColor="White" BorderColor="#3366CC" BorderStyle="None" 
            BorderWidth="1px" CaptionAlign="Left" CellPadding="4" 
            DataKeyNames="OwnerType,OwnerCode" DataSourceID="SqlDSRoleUser" 
            Font-Size="10pt" onselectedindexchanged="gvRoleUser_SelectedIndexChanged" 
            ShowFooter="True">
            <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
            <RowStyle BackColor="White" ForeColor="#003399" />
            <Columns>
                <asp:CommandField SelectText="Select" ShowSelectButton="True" />
                <asp:BoundField DataField="OwnerType" HeaderText="OwnerType" 
                    SortExpression="OwnerType" />
                <asp:BoundField DataField="OwnerCode" HeaderText="OwnerCode" 
                    SortExpression="OwnerCode" />
                <asp:BoundField DataField="OwnerName" HeaderText="OwnerName" 
                    SortExpression="OwnerName" />
            </Columns>
            <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDSRoleUser" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
            SelectCommand="SELECT 'U' AS OwnerType, BadgeCode AS OwnerCode, ChtName AS OwnerName
FROM v_user WHERE IsInUse='True' AND (UPPER(ChtName) LIKE '%' + UPPER(@RoleName) + '%' OR UPPER(BadgeCode) LIKE '%' + UPPER(@RoleName) + '%')
UNION
SELECT 'R' AS OwnerType, 
CONVERT(NVARCHAR,[RoleCode]) AS OwnerCode,
[RoleName] AS OwnerName FROM [v_role] WHERE (([ModuleCode] = @ModuleCode) AND (UPPER([RoleName]) LIKE '%' + UPPER(@RoleName) + '%'))">
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
<asp:HiddenField ID="hfUserId" runat="server" />
        </div>
</asp:Content>

