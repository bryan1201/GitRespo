<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="RoleMaintain.aspx.cs" Inherits="RoleMaintain" StylesheetTheme="MMContainer"  Title="=== MQITS::人員角色權限管理 ===" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
    <div style="width:100%; font-family: Arial Unicode MS, Arial;">
    <div class="menu_top" style="width:98%">
        <h3 style="color: #ffffff;">Role/Module Management</h3>
    </div>
    <div class="menu_mid">
    <div class="menu_container"><span style="clear:none;float:left;">
        <asp:Menu ID="menuRolemaintain" runat="server"
            BackColor="PaleTurquoise" DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="0.8em"
            ForeColor="#990000" Orientation="Horizontal" StaticSubMenuIndent="10px" Height="30px" OnMenuItemClick="menuRolemaintain_MenuItemClick">
            <Items>
                <asp:MenuItem Text="Role Authority" Value="0" Selected="True"></asp:MenuItem>
                <asp:MenuItem Text="User Management" Value="1"></asp:MenuItem>
            </Items>
            <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" CssClass="menu_btn_inact" />
            <DynamicHoverStyle ForeColor="White" />
            <DynamicMenuStyle BackColor="#FFE0C0" /> 
            <StaticSelectedStyle CssClass="menu_btn_act"  Font-Bold="False"
                ForeColor="Snow" />
            <DynamicSelectedStyle BackColor="#FFE0C0" />
            <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
            <StaticHoverStyle ForeColor="White" />
        </asp:Menu>
        </span>
        <span style="clear:right; float:none; width:400px;">
        <asp:ValidationSummary ID="vsAllError" runat="server" Font-Bold="True" Font-Names="Arial Unicode MS"
            Font-Overline="False" Font-Size="10pt" ValidationGroup="Component" ShowMessageBox="True" ForeColor="DarkRed"  />
        </span></div>
        </div>
<div style="width:100%">
    <asp:MultiView ID="mvRolemaintain" runat="server" Visible="True">
        <asp:View ID="vFunctionmaintain" runat="server">
<table width="100%" border="0" style="padding-right: 1px; padding-left: 1px; padding-bottom: 1px; margin: 1px; clip: rect(0px 0px 0px 0px); padding-top: 1px; border-collapse: collapse; border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid; border-bottom: gray 1px solid;" cellpadding="2" cellspacing="1">
    <tr>
    <td style="width:580px" valign="top">
        <asp:GridView ID="gvTreeModuleRole" runat="server" AutoGenerateColumns="False"
            CellPadding="4" ForeColor="#333333" GridLines="Vertical" 
            Font-Names="Arial Narrow" Font-Size="11pt" DataSourceID="SqlDSModule" 
            DataKeyNames="ModuleID" ShowFooter="True">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:CommandField CancelText="Cancel" DeleteText="Delete" EditText="Edit" 
                    InsertText="Save" NewText="Add" SelectText="Select" ShowSelectButton="True" 
                    UpdateText="Save" />
                <asp:BoundField HeaderText="Module Name" DataField="ModuleName" />
                <asp:TemplateField HeaderText="IsInUse">
                    <EditItemTemplate>
                        <asp:CheckBox ID="chkIsInUse" runat="server" Checked="True" Enabled="False" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkIsInUse" runat="server" Checked="True" Enabled="False" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="IsVirtual" SortExpression="IsVirtual">
                    <EditItemTemplate>
                    <asp:CheckBox ID="chkIsVirtual" runat="server" Checked='<%# Bind("IsVirtual") %>' 
                            Enabled="False" />
                    </EditItemTemplate>
                    <ItemTemplate>
                    <asp:CheckBox ID="chkIsVirtual" runat="server" Checked='<%# Bind("IsVirtual") %>' Enabled="False" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="editor">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("editorName") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("editorName") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Font-Size="8pt" />
                </asp:TemplateField>
                <asp:BoundField HeaderText="Rank" DataField="Rank" SortExpression="Rank" />
                <asp:BoundField DataField="udt" HeaderText="Update Date">
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField>
            </Columns>
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDSModule" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
            SelectCommand="SELECT * FROM [v_Module_all]"></asp:SqlDataSource>
        <hr style="font-size: 10pt; color: blue" /><div style="color: #FFFFFF; background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x;">Role List:</div>
        <asp:Label ID="lblFindRole" runat="server" Text="Find Role:"></asp:Label>
        <asp:TextBox ID="txtFindRole" runat="server"></asp:TextBox>
        <asp:Button ID="btnFindRole" runat="server" OnClick="btnFindRole_Click" 
            Text="Search" />
        
        <asp:GridView ID="gvRole" runat="server" AllowPaging="True" AllowSorting="True"
        AutoGenerateColumns="False"
        BorderStyle="Solid" BorderWidth="1px" CellPadding="2"
        DataKeyNames="RoleCode" DataSourceID="SqlDSRole"
        Font-Names="Arial Unicode MS" Font-Size="10pt" ForeColor="#333333" GridLines="Vertical"
        RowHeaderColumn="RoleName" ShowFooter="True" Width="100%" 
            OnSelectedIndexChanged="gvRole_SelectedIndexChanged" 
            OnRowEditing="gvRole_RowEditing">
        <PagerSettings Position="TopAndBottom" />
        <FooterStyle BackColor="#CCCC99" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:TemplateField>
                <EditItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                        Text="Save"></asp:LinkButton>
                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                        Text="Cancel"></asp:LinkButton>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                        Text="Edit" Visible="False"></asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandName="Select"
                        Text="Select"></asp:LinkButton>
                    <asp:LinkButton ID="LinkButton4" runat="server" CausesValidation="False" CommandName="Delete"
                        Text="Delete" OnClientClick="return confirm('Are you sure?')" Visible="False"></asp:LinkButton>
                </ItemTemplate>
                <HeaderStyle Width="50px" />
            </asp:TemplateField>
            <asp:BoundField DataField="RoleCode" HeaderText="RoleCode" ReadOnly="True" 
                SortExpression="RoleCode" />
            <asp:TemplateField HeaderText="Role Name" SortExpression="RoleName">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Columns="15" Text='<%# Bind("RoleName") %>'></asp:TextBox>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:LinkButton ID="lbtnAddRole" runat="server" OnClick="lbtnAddRole_Click">Add</asp:LinkButton>
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("RoleName") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Font-Bold="True" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="IsUserRole">
                <EditItemTemplate>
                    <asp:CheckBox ID="chkIsUserRole" runat="server" Checked="True" Enabled="True" />
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkIsUserRole" runat="server" Checked='<%# Bind("IsUserRole") %>' Enabled="False" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Rank" SortExpression="sortorder">
                <EditItemTemplate>
                    <asp:TextBox ID="txtEGSortOrder" runat="server" Columns="1" MaxLength="2" Text='<%# Bind("sortorder") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("sortorder") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="SiteName" SortExpression="SiteName">
                <ItemTemplate>
                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("SiteName") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlSite" runat="server" DataSourceID="SqlDSSite" 
                        DataTextField="SiteName" DataValueField="SiteID" 
                        SelectedValue='<%# Bind("Site", "{0:N}") %>'>
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="editor" HeaderText="editor" 
                SortExpression="editor" >
                <ItemStyle Font-Size="8pt" />
                <ItemStyle Font-Size="8pt" />
            </asp:BoundField>
            <asp:BoundField DataField="udt" HeaderText="Update Date" SortExpression="udt" 
                ReadOnly="True" >
            </asp:BoundField>
            <asp:BoundField HeaderText="Create Date" DataField="cdt" Visible="False" >
            </asp:BoundField>
        </Columns>
        <RowStyle BackColor="#F7F7DE" ForeColor="#333333" HorizontalAlign="Left" />
        <SelectedRowStyle BackColor="#80FFFF" Font-Bold="False" ForeColor="#CE5D5A" />
        <PagerStyle BackColor="#F7F7DE" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
            ForeColor="#333333" HorizontalAlign="Right" />
        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDSRole" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>"
        DeleteCommand="DELETE FROM m_role WHERE (ID = @RoleCode)" InsertCommand="INSERT INTO m_role(RoleName, sortorder) VALUES (@RoleName, @sortorder)"
        SelectCommand="SELECT * FROM dbo.v_role&#13;&#10;WHERE ModuleCode = @ModuleCode AND UPPER(RoleName) LIKE &#13;&#10;CASE&#13;&#10; WHEN ISNULL(@RoleName,'') = '' THEN '%'&#13;&#10; ELSE '%' + @RoleName + '%'&#13;&#10;END" UpdateCommand="UPDATE m_role SET RoleName = @RoleName, sortorder = @sortorder, udt = GETDATE() WHERE (ID = @RoleCode)">
        <DeleteParameters>
            <asp:Parameter Name="RoleCode" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="RoleName" />
            <asp:Parameter Name="sortorder" />
            <asp:Parameter Name="RoleCode" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="RoleName" />
            <asp:Parameter Name="sortorder" />
        </InsertParameters>
        <SelectParameters>
            
            <asp:ControlParameter ControlID="gvTreeModuleRole" Name="ModuleCode" 
                PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="txtFindRole" Name="RoleName" 
                PropertyName="Text" />
        </SelectParameters>
    </asp:SqlDataSource>
    
        <asp:FormView ID="fvRole" runat="server" BorderStyle="Solid" BorderWidth="2px" CellPadding="0"
            DataKeyNames="RoleCode" DataSourceID="SqlDSAddRole" ForeColor="#333333" OnItemCommand="fvRole_ItemCommand" OnItemInserting="fvRole_ItemInserting" OnItemUpdating="fvRole_ItemUpdating" BorderColor="#FFE0C0" >
            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditItemTemplate>
                <asp:LinkButton ID="lbtnRoleUpdate" runat="server" CausesValidation="True" CommandName="Update"
                    Text="Save" Font-Names="Arial Unicode MS" Font-Size="10pt"></asp:LinkButton>
                <asp:LinkButton ID="lbtnUpdateRoleCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                    Text="Cancel" Font-Names="Arial Unicode MS" Font-Size="10pt"></asp:LinkButton>
                <table style="vertical-align:top;">
                    <tr>
                        <td style="vertical-align:top;">
                            <table border="0" cellpadding="2" cellspacing="1" style="font-size: 10pt; line-height: 10pt;
                                font-family: 'Arial Unicode MS'; width:100%; vertical-align:top;">
                                <tr>
                                    <td class="FVTDEDITCol1">
                                        ID</td>
                                    <td class="FVTDEDITCol2">
                            <asp:Label ID="RoleCodeLabel1" runat="server" Text='<%# Eval("RoleCode") %>'></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="FVTDEDITCol1">
                                        Role Name</td>
                                    <td class="FVTDEDITCol2">
                                        <asp:TextBox ID="txtRoleName" runat="server" Text='<%# Bind("RoleName") %>' Columns="15"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="FVTDEDITCol1">
                                        editor</td>
                                    <td class="FVTDEDITCol2">
                                        <asp:Label ID="lblEEditor" runat="server" Text='<%# Eval("editor", "{0}") %>'></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="FVTDEDITCol1">
                                        Create Date</td>
                                    <td class="FVTDEDITCol2">
                                        <asp:Label ID="lblECdt" runat="server" Text='<%# Eval("cdt") %>'></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="FVTDEDITCol1">
                                        Update Date</td>
                                    <td class="FVTDEDITCol2">
                                        <asp:Label ID="lblEUdt" runat="server" Text='<%# Eval("udt") %>'></asp:Label></td>
                                </tr>
                            </table>                    
                        </td>
                        <td style="vertical-align:top;">
                            <table cellpadding="2" cellspacing="1" style="padding-right: 0px; padding-left: 0px;
                                font-size: 10pt; padding-bottom: 0px; margin: 0px; border-top-style: none; padding-top: 0px;
                                font-family: 'Arial Unicode MS'; border-right-style: none; border-left-style: none;
                                border-bottom-style: none" width="100%" border="0">
                                <tr>
                                    <td class="FVTDITCol1">
                                        Site</td>
                                    <td class="FVTDITCol2">
                                        <asp:DropDownList ID="ddlSite" runat="server" DataSourceID="SqlDSSite" 
                                            DataTextField="SiteName" DataValueField="SiteID" 
                                            SelectedValue='<%# Bind("Site", "{0}") %>'>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="FVTDITCol1">
                                        Rank</td>
                                    <td class="FVTDITCol2">
                                        <asp:TextBox ID="txtRank" runat="server" Columns="9" 
                                            Text='<%# Bind("sortorder") %>'></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="FVTDITCol1">IsProjectRole</td>
                                    <td class="FVTDITCol2"><asp:CheckBox ID="chkIsProjectRole" runat="server" 
                                            Checked='<%# Bind("IsPrjRole") %>' /></td>
                                </tr>
                                <tr>
                                    <td class="FVTDITCol1">IsSigner</td>
                                    <td class="FVTDITCol2"><asp:CheckBox ID="chkIsSigner" runat="server" 
                                            Checked='<%# Bind("IsSigner") %>' /></td>
                                </tr>
                                <tr>
                                    <td class="FVTDITCol1">IsUserRole</td>
                                    <td class="FVTDITCol2"><asp:CheckBox ID="chkIsUserRole" runat="server" 
                                            Checked='<%# Bind("IsUserRole") %>' /></td>
                                </tr>
                             </table>
                        </td>
                    </tr>
                </table>
            </EditItemTemplate>
            <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
            <InsertItemTemplate>
                <asp:LinkButton ID="lbtnInsertRole" runat="server" CausesValidation="True" CommandName="Insert"
                    Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Save"></asp:LinkButton>
                <asp:LinkButton ID="lbtnInsertRoleCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                    Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Cancel"></asp:LinkButton>
                <table style="vertical-align:top;">
                    <tr>
                        <td  style="vertical-align:top">
                           <table border="0" cellpadding="2" cellspacing="1" style="padding-right: 0px; padding-left: 0px;
                                font-size: 10pt; padding-bottom: 0px; margin: 0px; border-top-style: none; padding-top: 0px;
                                font-family: 'Arial Unicode MS'; border-right-style: none; border-left-style: none;
                                border-bottom-style: none" width="100%">
                                <tr>
                                    <td class="FVTDINSCol1">
                                        Role Name</td>
                                    <td class="FVTDINSCol2">
                                        <asp:TextBox ID="txtRoleName" runat="server" Columns="15" Text='<%# Bind("RoleName") %>'></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="FVTDINSCol1">
                                        Site</td>
                                    <td class="FVTDINSCol2">
                                        <asp:DropDownList ID="ddlSite" runat="server" DataSourceID="SqlDSSite" 
                                            DataTextField="SiteName" DataValueField="SiteID" 
                                            SelectedValue='<%# Bind("Site", "{0:N}") %>'>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="FVTDINSCol1">
                                        Rank</td>
                                    <td class="FVTDINSCol2">
                                        <asp:TextBox ID="txtRank" runat="server" Columns="9" 
                                            Text='<%# Bind("sortorder") %>'></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="vertical-align:top">
                           <table border="0" cellpadding="2" cellspacing="1" style="padding-right: 0px; padding-left: 0px;
                                font-size: 10pt; padding-bottom: 0px; margin: 0px; border-top-style: none; padding-top: 0px;
                                font-family: 'Arial Unicode MS'; border-right-style: none; border-left-style: none;
                                border-bottom-style: none" width="100%">
                                <tr>
                                    <td class="FVTDITCol1">IsProjectRole</td>
                                    <td class="FVTDITCol2"><asp:CheckBox ID="chkIsProjectRole" runat="server" 
                                            Checked='<%# Bind("IsPrjRole") %>' /></td>
                                </tr>
                                <tr>
                                    <td class="FVTDITCol1">IsSigner</td>
                                    <td class="FVTDITCol2"><asp:CheckBox ID="chkIsSigner" runat="server" 
                                            Checked='<%# Bind("IsSigner") %>' /></td>
                                </tr>
                                <tr>
                                    <td class="FVTDITCol1">IsUserRole</td>
                                    <td class="FVTDITCol2"><asp:CheckBox ID="chkIsUserRole" runat="server" 
                                            Checked='<%# Bind("IsUserRole") %>' /></td>
                                </tr>
                            </table>                      
                        </td>
                    </tr>
                </table>

            </InsertItemTemplate>
            <ItemTemplate>
                <asp:LinkButton ID="lbtnRoleNew" runat="server" CommandName="New" Font-Names="Arial Unicode MS"
                    Font-Size="10pt" OnClick="lbtnRoleNew_Click">Add</asp:LinkButton>
                <asp:LinkButton ID="lbtnRoleEdit" runat="server" CommandName="Edit" Font-Names="Arial Unicode MS"
                    Font-Size="10pt">Edit</asp:LinkButton>
                <asp:LinkButton ID="lblRoleCancel" runat="server" CommandName="Cancel" Font-Names="Arial Unicode MS"
                    Font-Size="10pt" OnClick="lbtnRoleCancel_Click">Cancel</asp:LinkButton>
                    <table style="vertical-align:top;">
                    <tr>
                    <td  style="vertical-align:top">
                        <table cellpadding="2" cellspacing="1" style="padding-right: 0px; padding-left: 0px;
                            font-size: 10pt; padding-bottom: 0px; margin: 0px; border-top-style: none; padding-top: 0px;
                            font-family: 'Arial Unicode MS'; border-right-style: none; border-left-style: none;
                            border-bottom-style: none" width="100%" border="0">
                            <tr>
                                <td class="FVTDITCol1">
                                    ID</td>
                                <td class="FVTDITCol2">
                                    <asp:Label ID="lblRoleCode" runat="server" Text='<%# Eval("RoleCode") %>'></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="FVTDITCol1" style="height: 21px">
                                    Role Name</td>
                                <td class="FVTDITCol2" style="height: 21px">
                                    <asp:Label ID="lblRoleName" runat="server" Text='<%# Bind("RoleName") %>'></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="FVTDITCol1">
                                    editor</td>
                                <td class="FVTDITCol2">
                                    <asp:Label ID="editorLabel" runat="server" Text='<%# Bind("editor") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="FVTDITCol1">
                                    Create Date</td>
                                <td class="FVTDITCol2">
                                    <asp:Label ID="cdtLabel" runat="server" Text='<%# Bind("cdt") %>'></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="FVTDITCol1">
                                    Update Date</td>
                                <td class="FVTDITCol2">
                                    <asp:Label ID="udtLabel" runat="server" Text='<%# Bind("udt") %>'></asp:Label></td>
                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align:top">
                        <table cellpadding="2" cellspacing="1" style="padding-right: 0px; padding-left: 0px;
                            font-size: 10pt; padding-bottom: 0px; margin: 0px; border-top-style: none; padding-top: 0px;
                            font-family: 'Arial Unicode MS'; border-right-style: none; border-left-style: none;
                            border-bottom-style: none" width="100%" border="0">
                            <tr>
                                <td class="FVTDITCol1">
                                    Site</td>
                                <td class="FVTDITCol2">
                                    <asp:Label ID="lblSite" runat="server" Text='<%# Eval("SiteName", "{0}") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="FVTDITCol1">
                                    Rank</td>
                                <td class="FVTDITCol2">
                                    <asp:Label ID="lblRank" runat="server" Text='<%# Bind("sortorder") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="FVTDITCol1">IsProjectRole</td>
                                <td class="FVTDITCol2"><asp:CheckBox ID="chkIsProjectRole" runat="server" 
                                        Checked='<%# Bind("IsPrjRole") %>' Enabled="false" /></td>
                            </tr>
                            <tr>
                                <td class="FVTDITCol1">IsSigner</td>
                                <td class="FVTDITCol2"><asp:CheckBox ID="chkIsSigner" runat="server" 
                                        Checked='<%# Bind("IsSigner") %>' Enabled="false" /></td>
                            </tr>
                            <tr>
                                <td class="FVTDITCol1">IsUserRole</td>
                                <td class="FVTDITCol2"><asp:CheckBox ID="chkIsUserRole" runat="server" 
                                        Checked='<%# Bind("IsUserRole") %>' Enabled="false" /></td>
                            </tr>
                         </table>
                    </td>
                    </tr>
                    </table>
                    
            </ItemTemplate>
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        </asp:FormView>
        <asp:SqlDataSource ID="SqlDSAddRole" runat="server" ConflictDetection="CompareAllValues"
            ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT * FROM v_role
WHERE RoleCode = @RoleCode">
            <SelectParameters>
                <asp:ControlParameter ControlID="gvRole" Name="RoleCode" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
        <div style="color: #FFFFFF; background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x;">Role Members List:</div>
    <asp:Label ID="lblFindMember" runat="server" Text="Find Member:"></asp:Label><asp:TextBox
            ID="txtFindMember" runat="server"></asp:TextBox>
        <asp:Button ID="btnFindMember" runat="server" OnClick="btnFindMember_Click" 
            Text="Search" />
     <asp:GridView ID="gvUserRole" runat="server" AllowPaging="True" AllowSorting="True"
        AutoGenerateColumns="False" BorderStyle="Solid" BorderWidth="0px" Caption="Role Members"
        CaptionAlign="Left" CellPadding="4" CellSpacing="1" DataKeyNames="BadgeCode"
        DataSourceID="SqlDSUserRole" Font-Names="Arial Unicode MS"
        Font-Size="10pt" ForeColor="#333333" GridLines="Vertical" HorizontalAlign="Justify"
        ShowFooter="True" OnRowUpdating="gvUserRole_RowUpdating" 
            OnRowDeleting="gvUserRole_RowDeleting" Width="100%">
        <PagerSettings Position="TopAndBottom" />
        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Select"
                        Text="Select"></asp:LinkButton><asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False"
                            CommandName="Delete" OnClientClick="return confirm('Are you sure?')" Text="Delete"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="InUse" SortExpression="Status">
                <EditItemTemplate>
                    &nbsp;<asp:CheckBox ID="chkEUserIsInUse" runat="server" Checked='<%# bool.Parse(((string)DataBinder.Eval(Container, "DataItem.Status")).ToLower()) %>' />
                </EditItemTemplate>
                <ItemTemplate>
                    &nbsp;<asp:CheckBox ID="chkTUserIsInUse" runat="server" Checked='<%# bool.Parse(((string)DataBinder.Eval(Container, "DataItem.Status")).ToLower()) %>'
                        Enabled="False" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="BadgeCode" SortExpression="BadgeCode">
                <EditItemTemplate>
                    <asp:TextBox ID="txtEBadgeCode" runat="server" Columns="9" Rows="1" Text='<%# Bind("BadgeCode") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("BadgeCode") %>'></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:LinkButton ID="lbtnUserRoleAdd" runat="server" ForeColor="#FFFF80"
                        OnClick="lbtnUserRoleAdd_Click">Add</asp:LinkButton>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ChtName" HeaderText="User Name" SortExpression="ChtName" />
            <asp:BoundField DataField="email" HeaderText="eMailAdress" SortExpression="email" />
        </Columns>
        <RowStyle BackColor="#E3EAEB" />
        <EditRowStyle BackColor="#7C6F57" />
        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#C0FFC0" ForeColor="DarkBlue" HorizontalAlign="Right" />
        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" />
        <EmptyDataTemplate>
            <div class="GVDIVEmptyData">
                <asp:LinkButton ID="lblUserRoleAddUser" runat="server" Font-Names="Arial Unicode MS" Font-Size="10pt" OnClick="lblUserRoleAddUser_Click">Add Member</asp:LinkButton>
            </div>
        </EmptyDataTemplate>
    </asp:GridView>
   <asp:SqlDataSource ID="SqlDSUserRole" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>"
        SelectCommand="SELECT Status, BadgeCode, ChtName, email FROM v_userrole WHERE (RoleCode = @RoleCode) AND (UPPER(ChtName) LIKE &#13;&#10;CASE&#13;&#10; WHEN ISNULL(@ChtName,'') = '' THEN '%'&#13;&#10; ELSE '%' + @ChtName + '%'&#13;&#10;END&#13;&#10;&#13;&#10;OR&#13;&#10; UPPER([BadgeCode]) LIKE &#13;&#10;CASE WHEN @BadgeCode='' THEN '%'&#13;&#10;ELSE @BadgeCode&#13;&#10;END)">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvRole" Name="RoleCode" PropertyName="SelectedValue"
                Type="Int32" />
            <asp:ControlParameter ControlID="txtFindMember" Name="ChtName" PropertyName="Text" />
            <asp:ControlParameter ControlID="txtFindMember" Name="BadgeCode" PropertyName="Text" />
        </SelectParameters>
    </asp:SqlDataSource>
        <asp:FormView ID="fvUserRole" runat="server" DataSourceID="SqlDSUserRoleAdd" OnItemInserting="fvUserRole_ItemInserting" OnItemDeleting="fvUserRole_ItemDeleting" OnItemUpdating="fvUserRole_ItemUpdating" Width="50%" BorderColor="#FFE0C0" BorderStyle="Solid" BorderWidth="2px">
            <EditItemTemplate>
                <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
                    Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Save"></asp:LinkButton>
                <asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                    Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Cancel"></asp:LinkButton><table border="0" cellpadding="2" cellspacing="1" style="font-size: 10pt; line-height: 10pt;
                    font-family: 'Arial Unicode MS'" width="100%">
                    <tr>
                        <td class="FVTDEDITCol1">
                            InUse</td>
                        <td class="FVTDEDITCol2">
                            <asp:CheckBox ID="chkUserIsInUse" runat="server" Checked='<%# bool.Parse(((string)DataBinder.Eval(Container, "DataItem.Status")).ToLower()) %>' /></td>
                    </tr>
                    <tr>
                        <td class="FVTDEDITCol1">
                            BadgeCode</td>
                        <td class="FVTDEDITCol2">
                            <asp:TextBox ID="BadgeCodeTextBox" runat="server" Columns="9" Text='<%# Bind("BadgeCode") %>'></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="FVTDEDITCol1">
                            User Name</td>
                        <td class="FVTDEDITCol2">
                            <asp:TextBox ID="ChtNameTextBox" runat="server" Text='<%# Bind("ChtName") %>'></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="FVTDEDITCol1">
                            Role Name</td>
                        <td class="FVTDEDITCol2">
                            <asp:Label ID="lblRoleName" runat="server" Text='<%# Eval("RoleName") %>'></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="FVTDEDITCol1">
                            eMail</td>
                        <td class="FVTDEDITCol2">
                            <asp:TextBox ID="txtemail" runat="server" Text='<%# Bind("email", "{0}") %>'></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="FVTDEDITCol1">
                            editor</td>
                        <td class="FVTDEDITCol2">
                            <asp:Label ID="lbleditor" runat="server" Text='<%# Eval("editor_name") %>'></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="FVTDEDITCol1">
                            Create Date</td>
                        <td class="FVTDEDITCol2">
                            <asp:Label ID="lblcdt" runat="server" Text='<%# Eval("cdt") %>'></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="FVTDEDITCol1">
                            Update Date</td>
                        <td class="FVTDEDITCol2">
                            <asp:Label ID="lbludt" runat="server" Text='<%# Eval("udt", "{0:g}") %>'></asp:Label></td>
                    </tr>
                </table>
            </EditItemTemplate>
            <InsertItemTemplate>
                <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert"
                    Text="Save" ></asp:LinkButton>
                <asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                    Text="Cancel"></asp:LinkButton><table border="0" cellpadding="2" cellspacing="1" style="padding-right: 0px; padding-left: 0px;
                    font-size: 10pt; padding-bottom: 0px; margin: 0px; border-top-style: none; padding-top: 0px;
                    font-family: 'Arial Unicode MS'; border-right-style: none; border-left-style: none;
                    border-bottom-style: none" width="100%">
                    <tr>
                        <td class="FVTDINSCol1">
                            InUse</td>
                        <td class="FVTDINSCol2">
                            <asp:CheckBox ID="chkUserIsInUse" runat="server" Checked="true" /></td>
                    </tr>
                    <tr>
                        <td class="FVTDINSCol1">
                            BadgeCode</td>
                        <td class="FVTDINSCol2">
                <asp:TextBox ID="txtBadgeCode" runat="server" Text='<%# Bind("BadgeCode", "{0}") %>' Columns="9"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="FVTDINSCol1">
                            User Name</td>
                        <td class="FVTDINSCol2">
                <asp:TextBox ID="txtChtName" runat="server" Text='<%# Bind("ChtName", "{0}") %>'></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="FVTDINSCol1">
                            eMail</td>
                        <td class="FVTDINSCol2">
                            <asp:TextBox ID="txtemail" runat="server" Text='<%# Bind("email", "{0}") %>'></asp:TextBox></td>
                    </tr>
                </table>
            </InsertItemTemplate>
            <ItemTemplate>
                <asp:LinkButton ID="lbtnUserRoleNew" runat="server" CommandName="New" Font-Names="Arial Unicode MS"
                    Font-Size="10pt" >Add</asp:LinkButton>
                <asp:LinkButton ID="lbtnUserRoleEdit" runat="server" CommandName="Edit" Font-Names="Arial Unicode MS"
                    Font-Size="10pt" >Edit</asp:LinkButton><table cellpadding="2" cellspacing="1" style="padding-right: 0px; padding-left: 0px;
                    font-size: 10pt; padding-bottom: 0px; margin: 0px; border-top-style: none; padding-top: 0px;
                    font-family: 'Arial Unicode MS'; border-right-style: none; border-left-style: none;
                    border-bottom-style: none" width="100%">
                    <tr>
                        <td class="FVTDITCol1">
                            InUse</td>
                        <td class="FVTDITCol2">
                            <asp:CheckBox ID="chkUserIsInUse" runat="server" Checked='<%# bool.Parse(((string)DataBinder.Eval(Container, "DataItem.Status")).ToLower()) %>'
                                Enabled="False" /></td>
                    </tr>
                    <tr>
                        <td class="FVTDITCol1">
                            BadgeCode</td>
                        <td class="FVTDITCol2">
                            <asp:Label ID="lblBadgeCode" runat="server" Text='<%# Bind("BadgeCode") %>'></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="FVTDITCol1">
                            User Name</td>
                        <td class="FVTDITCol2">
                            <asp:Label ID="lblChtName" runat="server" Text='<%# Bind("ChtName") %>'></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="FVTDITCol1">
                            Role Name</td>
                        <td class="FVTDITCol2">
                            <asp:Label ID="lblRoleName" runat="server" Text='<%# Bind("RoleName") %>'></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="FVTDITCol1" style="height: 18px">
                            eMail</td>
                        <td class="FVTDITCol2" style="height: 18px">
                            <asp:Label ID="lblemail" runat="server" Text='<%# Bind("email", "{0}") %>'></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="FVTDITCol1">
                            Create Date</td>
                        <td class="FVTDITCol2">
                            <asp:Label ID="lblcdt" runat="server" Text='<%# Bind("cdt") %>'></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="FVTDITCol1">
                            Update Date</td>
                        <td class="FVTDITCol2">
                            <asp:Label ID="lbludt" runat="server" Text='<%# Eval("udt", "{0:g}") %>'></asp:Label></td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:FormView>
        &nbsp;
        <asp:SqlDataSource ID="SqlDSUserRoleAdd" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>"
            SelectCommand="SELECT * FROM [v_userrole] WHERE (([BadgeCode] = @BadgeCode) AND ([RoleCode] = @RoleCode))">
            <SelectParameters>
                <asp:ControlParameter ControlID="gvUserRole" Name="BadgeCode" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="gvRole" Name="RoleCode" PropertyName="SelectedValue"
                    Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>       
    </td>
    <td style="width:35%" valign="top" rowspan="1" title="Role Authority">
        <div style="color: #FFFFFF; background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x;">Role Function:</div>
        <asp:Button ID="btnRenewRFunction" runat="server" Text="Save Role Function" /><br />
    <asp:TreeView ID="tvRFunction" runat="server" BorderWidth="0px" Font-Names="Arial Unicode MS" Font-Size="10pt" OnTreeNodeCheckChanged="tvRFunction_TreeNodeCheckChanged" ShowCheckBoxes="All" ShowLines="True" NodeIndent="25">
    </asp:TreeView>
        <asp:TextBox ID="txtUserId" runat="server" Visible="False"></asp:TextBox>
    </td>
    </tr>
    </table>
            </asp:View>
            <asp:View ID="vUsermaintain" runat="server"><asp:SqlDataSource ID="SqlDSUserAdd" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>"
        SelectCommand="SELECT * FROM [v_user] WHERE ([BadgeCode] = @BadgeCode)">
                <SelectParameters>
                    <asp:ControlParameter ControlID="gvUser" Name="BadgeCode" PropertyName="SelectedValue"
                        Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
                <asp:Label ID="lblUserFind" runat="server" Text="Search Member："></asp:Label><asp:TextBox
                    ID="txtUserFind" runat="server">*</asp:TextBox><asp:Button ID="btnUserFind" runat="server"  Text="Search" OnClick="btnUserFind_Click" />
                <asp:SqlDataSource ID="SqlDSUser" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>"
        SelectCommand="SELECT [IsInUse], [BadgeCode], [ChtName], [email], [CorpTel], [Description], [EngName], [Corp], [Bplace], [Dept], [PrivateTel], [Agent], [EnableAgent] FROM [v_user] WHERE ((UPPER([ChtName]) LIKE&#13;&#10;CASE&#13;&#10;WHEN @BadgeCode='' THEN '%'&#13;&#10; ELSE '%' + @BadgeCode + '%'&#13;&#10;END) OR (&#13;&#10;UPPER([BadgeCode]) LIKE &#13;&#10;CASE WHEN @BadgeCode='' THEN '%'&#13;&#10;ELSE '%' + @BadgeCode + '%'&#13;&#10;END) OR (&#13;&#10;email LIKE&#13;&#10;CASE WHEN @BadgeCode='' THEN '%'&#13;&#10;ELSE '%' + @BadgeCode + '%'&#13;&#10;END&#13;&#10;))">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtUserFind" DefaultValue="%" Name="BadgeCode" PropertyName="Text"
                            Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <div style="color: #FFFFFF; background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x;">Members List:</div>
                <asp:GridView ID="gvUser" runat="server" AllowPaging="True" AllowSorting="True"
        AutoGenerateColumns="False" BorderStyle="Solid" BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="BadgeCode"
        DataSourceID="SqlDSUser" Font-Names="Arial Unicode MS"
        Font-Size="10pt" ForeColor="#333333" GridLines="Vertical" HorizontalAlign="Justify"
        ShowFooter="True" Width="99%" OnRowDeleting="gvUser_RowDeleting">
                    <PagerSettings Position="TopAndBottom" />
                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Select"
                                    Text="Select"></asp:LinkButton>
                                <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False"
                                        CommandName="Delete" OnClientClick="return confirm('Are you sure to Delete?')" Text="Delete"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="InUse" SortExpression="IsInUse">
                            <EditItemTemplate>
                                &nbsp;<asp:CheckBox ID="chkEUserIsInUse" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.IsInUse") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                &nbsp;<asp:CheckBox ID="chkTUserIsInUse" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.IsInUse") %>'
                        Enabled="False" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BadgeCode" SortExpression="BadgeCode">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEBadgeCode" runat="server" Columns="9" Rows="1" Text='<%# Bind("BadgeCode") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lbtnUserAdd" runat="server" ForeColor="#FFFF80" OnClick="lbtnUserAdd_Click"
                                    >Add</asp:LinkButton>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBadgeCode" runat="server" Text='<%# Bind("BadgeCode", "{0}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ChtName" HeaderText="User Name" SortExpression="ChtName" />
                        <asp:BoundField DataField="email" HeaderText="eMailAdress" SortExpression="email" />
                        <asp:BoundField DataField="Bplace" HeaderText="Place" SortExpression="Bplace" />
                        <asp:BoundField DataField="Dept" HeaderText="Dept." SortExpression="Dept" />
                        <asp:BoundField DataField="CorpTel" HeaderText="Corp. Tel" SortExpression="CorpTel" />
                        <asp:BoundField DataField="PrivateTel" HeaderText="Private Tel" SortExpression="PrivateTel" />
                    </Columns>
                    <RowStyle BackColor="#E3EAEB" />
                    <EmptyDataTemplate>
                        <div class="GVDIVEmptyData">
                            <asp:LinkButton ID="lblUserRoleAddUser" runat="server" Font-Names="Arial Unicode MS"
                                Font-Size="10pt" OnClick="lblUserAdd_Click">Add Member</asp:LinkButton>
                        </div>
                    </EmptyDataTemplate>
                    <EditRowStyle BackColor="#7C6F57" />
                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#C0FFC0" ForeColor="DarkBlue" HorizontalAlign="Right" />
                    <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView><asp:FormView ID="fvUser" runat="server" DataSourceID="SqlDSUserAdd"  Width="35%" OnItemInserting="fvUser_ItemInserting" OnItemUpdating="fvUser_ItemUpdating" BorderColor="#FFE0C0" BorderStyle="Solid" BorderWidth="2px">
                    <EditItemTemplate>
                        <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
                            Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Save"></asp:LinkButton>
                        <asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                            Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Cancel"></asp:LinkButton><table border="0" cellpadding="2" cellspacing="1" style="font-size: 10pt; line-height: 10pt;
                    font-family: 'Arial Unicode MS'" width="100%">
                            <tr>
                                <td class="FVTDEDITCol1">
                                    InUse</td>
                                <td class="FVTDEDITCol2">
                                    <asp:CheckBox ID="chkUserIsInUse" runat="server" Checked='<%# Bind("IsInUse") %>' /></td>
                            </tr>
                            <tr>
                                <td class="FVTDEDITCol1">
                                    BadgeCode</td>
                                <td class="FVTDEDITCol2">
                                    <asp:Label ID="lblBadgeCode" runat="server" Text='<%# Bind("BadgeCode", "{0}") %>'></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="FVTDEDITCol1">
                                    User Name</td>
                                <td class="FVTDEDITCol2">
                                    <asp:TextBox ID="txtChtName" runat="server" Text='<%# Bind("ChtName") %>'></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="FVTDEDITCol1">
                                    eMail</td>
                                <td class="FVTDEDITCol2">
                                    <asp:TextBox ID="txtemail" runat="server" Text='<%# Bind("email", "{0}") %>'></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="FVTDEDITCol1">
                                    Remark</td>
                                <td class="FVTDEDITCol2">
                                    <asp:TextBox ID="txtDescr" runat="server" Rows="5" Text='<%# Bind("Description", "{0}") %>'
                                        TextMode="MultiLine"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="FVTDEDITCol1">
                                    editor</td>
                                <td class="FVTDEDITCol2">
                                    <asp:Label ID="lbleditor" runat="server" Text='<%# Eval("editorName", "{0}") %>'></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="FVTDEDITCol1">
                                    Create Date</td>
                                <td class="FVTDEDITCol2">
                                    <asp:Label ID="lblcdt" runat="server" Text='<%# Eval("Cdt", "{0:g}") %>'></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="FVTDEDITCol1">
                                    Update Date</td>
                                <td class="FVTDEDITCol2">
                                    <asp:Label ID="lbludt" runat="server" Text='<%# Eval("udt", "{0:g}") %>'></asp:Label></td>
                            </tr>
                        </table>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert"
                             Text="Save"></asp:LinkButton>
                        <asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                             Text="Cancel"></asp:LinkButton><table border="0" cellpadding="2" cellspacing="1" style="padding-right: 0px; padding-left: 0px;
                    font-size: 10pt; padding-bottom: 0px; margin: 0px; border-top-style: none; padding-top: 0px;
                    font-family: 'Arial Unicode MS'; border-right-style: none; border-left-style: none;
                    border-bottom-style: none" width="100%">
                            <tr>
                                <td class="FVTDINSCol1">
                                    InUse</td>
                                <td class="FVTDINSCol2">
                                    <asp:CheckBox ID="chkUserIsInUse" runat="server" Checked='<%# Bind("IsInUse") %>' /></td>
                            </tr>
                            <tr>
                                <td class="FVTDINSCol1">
                                    BadgeCode</td>
                                <td class="FVTDINSCol2">
                                    <asp:TextBox ID="txtBadgeCode" runat="server" Columns="9" Text='<%# Bind("BadgeCode", "{0}") %>'></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="FVTDINSCol1">
                                    User Name</td>
                                <td class="FVTDINSCol2">
                                    <asp:TextBox ID="txtChtName" runat="server" Text='<%# Bind("ChtName", "{0}") %>'></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="FVTDINSCol1">
                                    eMail</td>
                                <td class="FVTDINSCol2">
                                    <asp:TextBox ID="txtemail" runat="server" Text='<%# Bind("email", "{0}") %>'></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="FVTDINSCol1">
                                    Remark</td>
                                <td class="FVTDINSCol2">
                                    <asp:TextBox ID="txtDesc" runat="server" Rows="5" Text='<%# Bind("Description", "{0}") %>'
                                        TextMode="MultiLine"></asp:TextBox></td>
                            </tr>
                        </table>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnUserRoleNew" runat="server" CommandName="New" Font-Names="Arial Unicode MS"
                            Font-Size="10pt">Add</asp:LinkButton>
                        &nbsp;<asp:LinkButton ID="lbtnUserRoleEdit" runat="server" CommandName="Edit" Font-Names="Arial Unicode MS"
                            Font-Size="10pt">Edit</asp:LinkButton><table cellpadding="2" cellspacing="1" style="padding-right: 0px; padding-left: 0px;
                    font-size: 10pt; padding-bottom: 0px; margin: 0px; border-top-style: none; padding-top: 0px;
                    font-family: 'Arial Unicode MS'; border-right-style: none; border-left-style: none;
                    border-bottom-style: none" width="100%">
                            <tr>
                                <td class="FVTDITCol1">
                                    InUse</td>
                                <td class="FVTDITCol2">
                                    <asp:CheckBox ID="chkUserIsInUse" runat="server" Checked='<%# Bind("IsInUse") %>'
                                Enabled="False" /></td>
                            </tr>
                            <tr>
                                <td class="FVTDITCol1">
                                    BadgeCode</td>
                                <td class="FVTDITCol2">
                                    <asp:Label ID="lblBadgeCode" runat="server" Text='<%# Bind("BadgeCode") %>'></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="FVTDITCol1">
                                    User Name</td>
                                <td class="FVTDITCol2">
                                    <asp:Label ID="lblChtName" runat="server" Text='<%# Bind("ChtName") %>'></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="FVTDITCol1" style="height: 18px">
                                    eMail</td>
                                <td class="FVTDITCol2" style="height: 18px">
                                    <asp:Label ID="lblemail" runat="server" Text='<%# Bind("email", "{0}") %>'></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="FVTDITCol1" style="height: 18px">
                                    Remark</td>
                                <td class="FVTDITCol2" style="height: 18px">
                                    <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("Description", "{0}") %>'></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="FVTDITCol1">
                                    Create Date</td>
                                <td class="FVTDITCol2">
                                    <asp:Label ID="lblcdt" runat="server" Text='<%# Bind("cdt") %>'></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="FVTDITCol1">
                                    Update Date</td>
                                <td class="FVTDITCol2">
                                    <asp:Label ID="lbludt" runat="server" Text='<%# Eval("udt", "{0:g}") %>'></asp:Label></td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:FormView>
                <div style="color: #FFFFFF; background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x;">Member's Role List:</div>
                <asp:GridView ID="gvRoleUser" runat="server" AllowPaging="True" AllowSorting="True"
        AutoGenerateColumns="False"
        BorderStyle="Solid" BorderWidth="1px" CellPadding="2"
        DataKeyNames="RoleCode" DataSourceID="SqlDSRoleUser"
        Font-Names="Arial Unicode MS" Font-Size="10pt" ForeColor="#333333" GridLines="Vertical"
        RowHeaderColumn="RoleName" ShowFooter="True" Width="99%">
                    <PagerSettings Position="TopAndBottom" />
                    <FooterStyle BackColor="#CCCC99" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                                    Text="Save"></asp:LinkButton>
                                <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                                    Text="Cancel"></asp:LinkButton>
                            </EditItemTemplate>
                            <HeaderStyle Width="50px" />
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                                    Text="Edit" Visible="False"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandName="Select"
                                    Text="Select"></asp:LinkButton>
                                <asp:LinkButton ID="LinkButton4" runat="server" CausesValidation="False" CommandName="Delete"
                                    OnClientClick="return confirm('確定Delete?')" Text="Delete" Visible="False"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RoleCode" HeaderText="Role Code" ReadOnly="True" SortExpression="RoleCode" />
                        <asp:BoundField DataField="RoleName" HeaderText="Role Name" SortExpression="RoleName" />
                        <asp:BoundField DataField="ModuleName" HeaderText="Used by Role" SortExpression="ModuleName" />
                        <asp:BoundField DataField="SiteName" HeaderText="Site" 
                            SortExpression="SiteName" />
                        <asp:BoundField DataField="editor" HeaderText="editor" SortExpression="editor" Visible="False" >
                            <ItemStyle Font-Size="8pt" />
                        </asp:BoundField>
                        <asp:BoundField DataField="udt" HeaderText="Update Date" ReadOnly="True" SortExpression="udt" Visible="False" >
                            <ItemStyle Font-Size="8pt" />
                        </asp:BoundField>
                        <asp:BoundField DataField="editor_name" HeaderText="editor" SortExpression="editor_name" />
                        <asp:BoundField DataField="udt" HeaderText="Update Date" SortExpression="udt" />
                    </Columns>
                    <RowStyle BackColor="#F7F7DE" ForeColor="#333333" HorizontalAlign="Left" />
                    <SelectedRowStyle BackColor="#80FFFF" Font-Bold="False" ForeColor="#CE5D5A" />
                    <PagerStyle BackColor="#F7F7DE" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
            ForeColor="#333333" HorizontalAlign="Right" />
                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDSRoleUser" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>"
        SelectCommand="SELECT * FROM [v_userrole] WHERE ([BadgeCode] = @BadgeCode)">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="gvUser" Name="BadgeCode" PropertyName="SelectedValue"
                            Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
            
            </asp:View>
        </asp:MultiView>
    </div>
    </div>
<asp:SqlDataSource ID="SqlDSSite" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT NULL AS SiteID, '-' AS SiteName, 0 AS Rank
UNION
SELECT [SiteID], [SiteName], Rank FROM [v_Site] ORDER BY [Rank]"></asp:SqlDataSource>
</asp:Content>