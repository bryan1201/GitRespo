<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MModule.aspx.cs" Inherits="MModule" StylesheetTheme="MMContainer" Title="=== MQITS::Module Maintenance ===" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
   <asp:GridView ID="gvModule" runat="server" AutoGenerateColumns="False"
        CellPadding="4" ForeColor="#333333" GridLines="Vertical" 
        Font-Names="Arial Narrow" Font-Size="11pt" DataSourceID="SqlDSModule" 
        DataKeyNames="ModuleID" ShowFooter="True" 
       onrowupdating="gvModule_RowUpdating">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <Columns>
            <asp:CommandField CancelText="Cancel" DeleteText="Del" EditText="Edit" 
                InsertText="Add" NewText="New" SelectText="Select" ShowEditButton="True" 
                UpdateText="Save" />
            <asp:CommandField CancelText="Cancel" DeleteText="Delete" EditText="Edit" 
                InsertText="Save" NewText="Add" SelectText="Select" ShowSelectButton="True" 
                UpdateText="Save" />
            <asp:BoundField DataField="ModuleID" HeaderText="ModuleID" 
                InsertVisible="False" ReadOnly="True" SortExpression="ModuleID" />
            <asp:TemplateField HeaderText="Module Name">
                <EditItemTemplate>
                    <asp:TextBox ID="txtModuleName" runat="server" 
                        Text='<%# Bind("ModuleName", "{0}") %>'></asp:TextBox>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:LinkButton ID="lbtnAdd" runat="server" ForeColor="#FFFFCC" 
                        onclick="lbtnAdd_Click">Add</asp:LinkButton>
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("ModuleName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="IsEOSL">
                <EditItemTemplate>
                    <asp:CheckBox ID="chkIsEOSL" runat="server" Checked='<%# Bind("IsEOSL") %>' />
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkIsInUse" runat="server" Checked="True" Enabled="False" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="IsVirtual" SortExpression="IsVirtual">
                <EditItemTemplate>
                <asp:CheckBox ID="chkIsVirtual" runat="server" Checked='<%# Bind("IsVirtual") %>' />
                </EditItemTemplate>
                <ItemTemplate>
                <asp:CheckBox ID="chkIsVirtual" runat="server" Checked='<%# Bind("IsVirtual") %>' Enabled="False" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="editor">
                <EditItemTemplate>
                    <asp:HyperLink ID="hleditor" runat="server" 
                        NavigateUrl='<%# Eval("editorEmail", "{0}") %>' 
                        Text='<%# Eval("editorName") %>'></asp:HyperLink>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("editorName") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Font-Size="8pt" />
            </asp:TemplateField>
            <asp:BoundField HeaderText="Rank" DataField="Rank" SortExpression="Rank" />
            <asp:BoundField DataField="udt" HeaderText="Update Date" ReadOnly="True">
                <ItemStyle Font-Size="8pt" />
            </asp:BoundField>
            <asp:BoundField DataField="cdt" HeaderText="Create Date" SortExpression="cdt">
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
        SelectCommand="SELECT * FROM [v_Module] ORDER BY Rank"></asp:SqlDataSource>
    <asp:HiddenField ID="hfUserId" runat="server" />
    
    <asp:FormView ID="fvModule" runat="server" BorderStyle="Solid" 
       BorderWidth="2px" CellPadding="0" ForeColor="#333333" 
        BorderColor="#FFE0C0" DefaultMode="Insert" DataKeyNames="ModuleID" 
       DataSourceID="SqlDSModule" oniteminserting="fvModule_ItemInserting" 
       onmodechanging="fvModule_ModeChanging" >
        <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
        <ItemTemplate>
            ModuleID:
            <asp:Label ID="ModuleIDLabel" runat="server" Text='<%# Eval("ModuleID") %>' />
            <br />
            ModuleName:
            <asp:Label ID="ModuleNameLabel" runat="server" 
                Text='<%# Bind("ModuleName") %>' />
            <br />
            IsEOSL:
            <asp:CheckBox ID="IsEOSLCheckBox" runat="server" 
                Checked='<%# Bind("IsEOSL") %>' Enabled="false" />
            <br />
            IsVirtual:
            <asp:CheckBox ID="IsVirtualCheckBox" runat="server" 
                Checked='<%# Bind("IsVirtual") %>' Enabled="false" />
            <br />
            Rank:
            <asp:Label ID="RankLabel" runat="server" Text='<%# Bind("Rank") %>' />
            <br />
            editor:
            <asp:Label ID="editorLabel" runat="server" Text='<%# Bind("editor") %>' />
            <br />
            editorName:
            <asp:Label ID="editorNameLabel" runat="server" 
                Text='<%# Bind("editorName") %>' />
            <br />
            editorEmail:
            <asp:Label ID="editorEmailLabel" runat="server" 
                Text='<%# Bind("editorEmail") %>' />
            <br />
            cdt:
            <asp:Label ID="cdtLabel" runat="server" Text='<%# Bind("cdt") %>' />
            <br />
            udt:
            <asp:Label ID="udtLabel" runat="server" Text='<%# Bind("udt") %>' />
            <br />
        </ItemTemplate>
        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
        <EditItemTemplate>
            ModuleID:
            <asp:Label ID="ModuleIDLabel1" runat="server" Text='<%# Eval("ModuleID") %>' />
            <br />
            ModuleName:
            <asp:TextBox ID="ModuleNameTextBox" runat="server" 
                Text='<%# Bind("ModuleName") %>' />
            <br />
            IsEOSL:
            <asp:CheckBox ID="IsEOSLCheckBox" runat="server" 
                Checked='<%# Bind("IsEOSL") %>' />
            <br />
            IsVirtual:
            <asp:CheckBox ID="IsVirtualCheckBox" runat="server" 
                Checked='<%# Bind("IsVirtual") %>' />
            <br />
            Rank:
            <asp:TextBox ID="RankTextBox" runat="server" Text='<%# Bind("Rank") %>' />
            <br />
            editor:
            <asp:TextBox ID="editorTextBox" runat="server" Text='<%# Bind("editor") %>' />
            <br />
            editorName:
            <asp:TextBox ID="editorNameTextBox" runat="server" 
                Text='<%# Bind("editorName") %>' />
            <br />
            editorEmail:
            <asp:TextBox ID="editorEmailTextBox" runat="server" 
                Text='<%# Bind("editorEmail") %>' />
            <br />
            cdt:
            <asp:TextBox ID="cdtTextBox" runat="server" Text='<%# Bind("cdt") %>' />
            <br />
            udt:
            <asp:TextBox ID="udtTextBox" runat="server" Text='<%# Bind("udt") %>' />
            <br />
            <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
                CommandName="Update" Text="更新" />
            &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" 
                CausesValidation="False" CommandName="Cancel" Text="取消" />
        </EditItemTemplate>
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
                                    Module Name</td>
                                <td class="FVTDINSCol2">
                                    <asp:TextBox ID="txtModuleName" runat="server" Columns="15" 
                                        Text='<%# Bind("ModuleName") %>'></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="FVTDINSCol1">
                                    Rank</td>
                                <td class="FVTDINSCol2">
                                    <asp:TextBox ID="txtRank" runat="server" Columns="9" 
                                        Text='<%# Bind("Rank") %>'></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align:top">
                       <table border="0" cellpadding="2" cellspacing="1" style="padding-right: 0px; padding-left: 0px;
                            font-size: 10pt; padding-bottom: 0px; margin: 0px; border-top-style: none; padding-top: 0px;
                            font-family: 'Arial Unicode MS'; border-right-style: none; border-left-style: none;
                            border-bottom-style: none" width="100%">
                            <tr>
                                <td class="FVTDITCol1">IsEOSL</td>
                                <td class="FVTDITCol2">
                                    <asp:CheckBox ID="chkIsEOSL" runat="server" 
                                        Checked='<%# Bind("IsEOSL") %>' /></td>
                            </tr>
                            <tr>
                                <td class="FVTDITCol1">IsVirtual</td>
                                <td class="FVTDITCol2">
                                    <asp:CheckBox ID="chkIsVirtual" runat="server" 
                                        Checked='<%# Bind("IsVirtual") %>' /></td>
                            </tr>
                        </table>                      
                    </td>
                </tr>
            </table>
        </InsertItemTemplate>
        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
    </asp:FormView>
</asp:Content>

