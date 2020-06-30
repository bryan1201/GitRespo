<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MGroup.aspx.cs" Inherits="MGroup" MaintainScrollPositionOnPostback="true" StylesheetTheme="MMContainer" Title="=== MQITS::Group Maintenance ===" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
    <div style="width:100%; font-family: Arial Unicode MS, Arial;">
    <div class="menu_top" style="width:98%">
        <h3 style="color: #ffffff;">Group/Customer/Module/Site/Project/Phase Management</h3>
    </div>
    <div class="menu_mid">
    <div class="menu_container"><span style="clear:none;float:left;">
        <asp:Menu ID="menuGroupMaintain" runat="server"
            BackColor="PaleTurquoise" DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="0.8em"
            ForeColor="#990000" Orientation="Horizontal" StaticSubMenuIndent="10px" 
            Height="30px" onmenuitemclick="menuGroupMaintain_MenuItemClick">
            <Items>
                <asp:MenuItem Text="Group-Owner Authority" Value="0" Selected="True"></asp:MenuItem>
                <asp:MenuItem Text="Owner-Group Authority" Value="1"></asp:MenuItem>
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
    <asp:MultiView ID="mvGroupMaintain" runat="server" Visible="True">
        <asp:View ID="vGroupOwner" runat="server">
<table style="width:100%">
<tr>
    <td style="vertical-align: top">
        <asp:Button ID="btnRenewGroup" runat="server" onclick="btnRenewGroup_Click" 
            Text="Renew Group" Width="200px" />
        <asp:TreeView ID="tvGroup" runat="server" ImageSet="Contacts" NodeIndent="10" 
            ShowCheckBoxes="Leaf" ShowLines="True" 
            onselectednodechanged="tvGroup_SelectedNodeChanged" 
            ontreenodecheckchanged="tvGroup_TreeNodeCheckChanged">
            <ParentNodeStyle Font-Bold="True" ForeColor="#5555DD" />
            <HoverNodeStyle Font-Underline="False" />
            <SelectedNodeStyle Font-Underline="True" HorizontalPadding="0px" 
                VerticalPadding="0px" />
            <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" 
                HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
        </asp:TreeView>
    </td>
    <td style="vertical-align:top;">
        <asp:GridView ID="gvGroupOwner" runat="server" AutoGenerateColumns="False" 
            CellPadding="4" DataSourceID="SqlDSGroupOwner" Font-Size="9pt" 
            ForeColor="#333333" GridLines="Vertical" ShowFooter="True" 
            CaptionAlign="Left" DataKeyNames="ID" 
            EmptyDataText="--- No any owners and members ---">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:CommandField EditText="Edit" InsertText="Save" NewText="Add" 
                    SelectText="Select" ShowSelectButton="True" UpdateText="Update" />
                <asp:BoundField DataField="OwnerCode" HeaderText="OwnerCode" 
                    SortExpression="OwnerCode" />
                <asp:TemplateField HeaderText="OwnerName" SortExpression="OwnerName">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("OwnerName") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:LinkButton ID="lbtnAdd" runat="server" Font-Bold="False" 
                            Font-Names="Arial Black" Font-Size="10pt" ForeColor="#FFFF66" 
                            oncommand="lbtnAdd_Command">Add</asp:LinkButton>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("OwnerName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Rank" HeaderText="Rank" SortExpression="Rank" />
                <asp:BoundField DataField="IssueType" HeaderText="IssueType" 
                    SortExpression="IssueType" />
                <asp:CheckBoxField DataField="IsEnable" HeaderText="IsEnable" 
                    SortExpression="IsEnable" />
                <asp:CheckBoxField DataField="IsReadOnly" HeaderText="IsReadOnly" 
                    SortExpression="IsReadOnly" />
            </Columns>
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDSGroupOwner" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
            SelectCommand="SELECT DISTINCT * FROM [v_GroupOwner] WHERE ([GroupID] = @GroupID) ORDER BY [Rank]">
            <SelectParameters>
                <asp:ControlParameter ControlID="tvGroup" Name="GroupID" 
                    PropertyName="SelectedValue" Type="Int64" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:Label ID="lblFindRole0" runat="server" Font-Size="9pt" 
            Text="Find Member/Role："></asp:Label>
        <asp:TextBox ID="txtFindRole0" runat="server"></asp:TextBox>
        <asp:Button ID="btnFindRole0" runat="server" OnClick="btnFindRole_Click" 
            Text="尋找" />
        <asp:GridView ID="gvRole0" runat="server" AutoGenerateColumns="False" 
            CellPadding="4" DataKeyNames="RoleCode" DataSourceID="SqlDSRole" 
            Font-Size="9pt" ForeColor="#333333" GridLines="Vertical" 
            onrowcommand="gvRole_RowCommand" 
            onselectedindexchanged="gvRole_SelectedIndexChanged">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:CommandField SelectText="Select" ShowCancelButton="False" 
                    ShowSelectButton="True" />
                <asp:BoundField DataField="ModuleName" HeaderText="ModuleName" 
                    SortExpression="ModuleName" />
                <asp:BoundField DataField="RoleCode" HeaderText="RoleCode" ReadOnly="True" 
                    SortExpression="RoleCode" />
                <asp:BoundField DataField="RoleName" HeaderText="RoleName" 
                    SortExpression="RoleName" />
                <asp:CheckBoxField DataField="IsPrjRole" HeaderText="IsPrjRole" ReadOnly="True" 
                    SortExpression="IsPrjRole" />
                <asp:CheckBoxField DataField="IsSigner" HeaderText="IsSigner" ReadOnly="True" 
                    SortExpression="IsSigner" />
                <asp:CheckBoxField DataField="IsUserRole" HeaderText="IsUserRole" 
                    ReadOnly="True" SortExpression="IsUserRole" />
            </Columns>
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
    </td>
    </tr>
    </table>
    </asp:View>
    <asp:View ID="vOwnerGroupMaintain" runat="server">
        <table style="width:100%;">
        <tr>
        <td style="vertical-align:top;">
            <asp:Label ID="lblFindRole" runat="server" Text="Find Member/Role：" 
                Font-Size="10pt"></asp:Label>
                    <asp:TextBox ID="txtFindRole" runat="server"></asp:TextBox>
                    <asp:Button ID="btnFindRole" runat="server" OnClick="btnFindRole_Click" 
                        Text="尋找" />
                    <asp:GridView ID="gvRole" runat="server" CellPadding="4" Font-Size="9pt" 
                        ForeColor="#333333" GridLines="Vertical" AutoGenerateColumns="False"
                        DataKeyNames="RoleCode" DataSourceID="SqlDSRole" 
                onrowcommand="gvRole_RowCommand" 
                onselectedindexchanged="gvRole_SelectedIndexChanged">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" />
                        <Columns>
                            <asp:CommandField SelectText="Select" ShowSelectButton="True" 
                                ShowCancelButton="False" />
                            <asp:BoundField DataField="ModuleName" HeaderText="ModuleName" 
                                SortExpression="ModuleName" />
                            <asp:BoundField DataField="RoleCode" HeaderText="RoleCode" ReadOnly="True" 
                                SortExpression="RoleCode" />
                            <asp:BoundField DataField="RoleName" HeaderText="RoleName" 
                                SortExpression="RoleName" />
                            <asp:CheckBoxField DataField="IsPrjRole" HeaderText="IsPrjRole" ReadOnly="True" 
                                SortExpression="IsPrjRole" />
                            <asp:CheckBoxField DataField="IsSigner" HeaderText="IsSigner" ReadOnly="True" 
                                SortExpression="IsSigner" />
                            <asp:CheckBoxField DataField="IsUserRole" HeaderText="IsUserRole" 
                                ReadOnly="True" SortExpression="IsUserRole" />
                        </Columns>
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDSRole" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                        InsertCommand="INSERT INTO m_role(RoleName, sortorder) VALUES (@RoleName, @sortorder)" SelectCommand="SELECT * FROM dbo.v_role
            WHERE RoleName LIKE 
            CASE
             WHEN ISNULL(@RoleName,'') = '' THEN '%'
             ELSE '%' + @RoleName + '%'
            END ORDER BY ModuleCode, sortorder">
                        <InsertParameters>
                            <asp:Parameter Name="RoleName" />
                            <asp:Parameter Name="sortorder" />
                        </InsertParameters>
                        <SelectParameters>
                            <asp:ControlParameter ControlID="txtFindRole" Name="RoleName" 
                                PropertyName="Text" />
                        </SelectParameters>
                    </asp:SqlDataSource>
            </td>
        <td style="vertical-align:top;">
           <asp:TreeView ID="tvOwnerGroup" runat="server" ImageSet="Contacts" NodeIndent="10" 
            ShowCheckBoxes="Leaf" ShowLines="True" 
                onselectednodechanged="tvOwnerGroup_SelectedNodeChanged">
            <ParentNodeStyle Font-Bold="True" ForeColor="#5555DD" />
            <HoverNodeStyle Font-Underline="False" />
            <SelectedNodeStyle Font-Underline="True" HorizontalPadding="0px" 
                VerticalPadding="0px" />
            <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" 
                HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
            </asp:TreeView>
        </td>
        <td style="vertical-align:top; width: 250px;">
            <asp:FormView ID="fvOwnerGroup" runat="server" DataSourceID="SqlDSOwnerGroup" 
                DefaultMode="Edit" Font-Size="9pt" CellPadding="4" 
                DataKeyNames="GroupID,OwnerCode" ForeColor="#333333" 
                onitemupdating="fvOwnerGroup_ItemUpdating" Width="150px">
                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                <EditItemTemplate>
                    <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
                        CommandName="Update" Text="Save" />
                    &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" 
                        CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                    <asp:HiddenField ID="hfGroupID" runat="server" 
                        Value='<%# Bind("GroupID", "{0}") %>' Visible="False" />
                    <asp:HiddenField ID="hfOwnerCode" runat="server" 
                        Value='<%# Bind("OwnerCode", "{0}") %>' Visible="False" />
                    <asp:HiddenField ID="hfOwnerType" runat="server" 
                        Value='<%# Bind("OwnerType", "{0}") %>' Visible="False" />
                    <br />
                    IssueType:<asp:DropDownList ID="ddlOwnerType" runat="server" 
                        DataSourceID="SqlDSOwnerType" DataTextField="TEXT" DataValueField="VALUE" 
                        SelectedValue='<%# Bind("IssueType", "{0}") %>'>
                    </asp:DropDownList>
&nbsp;<asp:SqlDataSource ID="SqlDSOwnerType" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                        SelectCommand="SELECT * FROM [v_OwnerType]"></asp:SqlDataSource>
                    IsEnable:
                    <asp:CheckBox ID="IsEnableCheckBox" runat="server" 
                        Checked='<%# Bind("IsEnable") %>' />
                    <br />
                    IsReadOnly:
                    <asp:CheckBox ID="IsReadOnlyCheckBox" runat="server" 
                        Checked='<%# Bind("IsReadOnly") %>' />
                    <asp:HiddenField ID="hfOwnerID" runat="server" 
                        Value='<%# Bind("OwnerID", "{0}") %>' Visible="False" />
                </EditItemTemplate>
                <InsertItemTemplate>
                    GroupID:
                    <asp:TextBox ID="GroupIDTextBox" runat="server" Text='<%# Bind("GroupID") %>' />
                    <br />
                    Parent_GroupID:
                    <asp:TextBox ID="Parent_GroupIDTextBox" runat="server" 
                        Text='<%# Bind("Parent_GroupID") %>' />
                    <br />
                    GroupName:
                    <asp:TextBox ID="GroupNameTextBox" runat="server" 
                        Text='<%# Bind("GroupName") %>' />
                    <br />
                    OwnerCode:
                    <asp:TextBox ID="OwnerCodeTextBox" runat="server" 
                        Text='<%# Bind("OwnerCode") %>' />
                    <br />
                    OwnerName:
                    <asp:TextBox ID="OwnerNameTextBox" runat="server" 
                        Text='<%# Bind("OwnerName") %>' />
                    <br />
                    Rank:
                    <asp:TextBox ID="RankTextBox" runat="server" Text='<%# Bind("Rank") %>' />
                    <br />
                    IssueType:
                    <asp:CheckBox ID="IssueTypeCheckBox" runat="server" 
                        Checked='<%# Bind("IssueType") %>' />
                    <br />
                    IsEnable:
                    <asp:CheckBox ID="IsEnableCheckBox" runat="server" 
                        Checked='<%# Bind("IsEnable") %>' />
                    <br />
                    IsReadOnly:
                    <asp:CheckBox ID="IsReadOnlyCheckBox" runat="server" 
                        Checked='<%# Bind("IsReadOnly") %>' />
                    <br />
                    IsOwner:
                    <asp:CheckBox ID="IsOwnerCheckBox" runat="server" 
                        Checked='<%# Bind("IsOwner") %>' />
                    <br />
                    OwnerID:
                    <asp:TextBox ID="OwnerIDTextBox" runat="server" Text='<%# Bind("OwnerID") %>' />
                    <br />
                    <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" 
                        CommandName="Insert" Text="插入" />
                    &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" 
                        CausesValidation="False" CommandName="Cancel" Text="取消" />
                </InsertItemTemplate>
                <ItemTemplate>
                    GroupID:
                    <asp:Label ID="GroupIDLabel" runat="server" Text='<%# Bind("GroupID") %>' />
                    <br />
                    Parent_GroupID:
                    <asp:Label ID="Parent_GroupIDLabel" runat="server" 
                        Text='<%# Bind("Parent_GroupID") %>' />
                    <br />
                    GroupName:
                    <asp:Label ID="GroupNameLabel" runat="server" Text='<%# Bind("GroupName") %>' />
                    <br />
                    OwnerCode:
                    <asp:Label ID="OwnerCodeLabel" runat="server" Text='<%# Bind("OwnerCode") %>' />
                    <br />
                    OwnerName:
                    <asp:Label ID="OwnerNameLabel" runat="server" Text='<%# Bind("OwnerName") %>' />
                    <br />
                    Rank:
                    <asp:Label ID="RankLabel" runat="server" Text='<%# Bind("Rank") %>' />
                    <br />
                    IssueType:
                    <asp:CheckBox ID="IssueTypeCheckBox" runat="server" 
                        Checked='<%# Bind("IssueType") %>' Enabled="false" />
                    <br />
                    IsEnable:
                    <asp:CheckBox ID="IsEnableCheckBox" runat="server" 
                        Checked='<%# Bind("IsEnable") %>' Enabled="false" />
                    <br />
                    IsReadOnly:
                    <asp:CheckBox ID="IsReadOnlyCheckBox" runat="server" 
                        Checked='<%# Bind("IsReadOnly") %>' Enabled="false" />
                    <br />
                    IsOwner:
                    <asp:CheckBox ID="IsOwnerCheckBox" runat="server" 
                        Checked='<%# Bind("IsOwner") %>' Enabled="false" />
                    <br />
                    OwnerID:
                    <asp:Label ID="OwnerIDLabel" runat="server" Text='<%# Bind("OwnerID") %>' />
                    <br />
                </ItemTemplate>
                <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            </asp:FormView>
            <asp:SqlDataSource ID="SqlDSOwnerGroup" runat="server" 
                ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                SelectCommand="SELECT * FROM fn_GetOwnerGroupOne(@GroupID,@OwnerCode)">
                <SelectParameters>
                    <asp:ControlParameter ControlID="tvOwnerGroup" Name="GroupID" 
                        PropertyName="SelectedValue" Type="Int64" />
                    <asp:ControlParameter ControlID="gvRole" Name="OwnerCode" 
                        PropertyName="SelectedValue" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </td>
        </tr>
        </table>
    </asp:View>
    </asp:MultiView><asp:HiddenField ID="hfUserId" runat="server" />
    </div>
    </div>

</asp:Content>

