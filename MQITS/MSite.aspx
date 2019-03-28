<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MSite.aspx.cs" Inherits="MSite" StylesheetTheme="MMContainer" Title="=== MQITS::Site Maintenance ===" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
    <table style="width:100%;">
    <tr>
        <td style="vertical-align:top; width:100%" colspan="2">
            <div style="color: #FFFFFF; background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x;">Site Maintenance:</div>
            <asp:GridView ID="gvSite" runat="server" AutoGenerateColumns="False"
                CellPadding="4" ForeColor="#333333" GridLines="Vertical" 
                Font-Names="Arial Narrow" Font-Size="11pt" DataSourceID="SqlDSSite" 
                DataKeyNames="SiteID" ShowFooter="True" 
                onrowupdating="gvSite_RowUpdating" 
                onselectedindexchanged="gvSite_SelectedIndexChanged" Width="99%">
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <Columns>
                    <asp:CommandField CancelText="Cancel" DeleteText="Del" EditText="Edit" 
                        InsertText="Add" NewText="New" SelectText="Select" ShowEditButton="True" 
                        UpdateText="Save" />
                    <asp:CommandField CancelText="Cancel" DeleteText="Delete" EditText="Edit" 
                        InsertText="Save" NewText="Add" SelectText="Select" ShowSelectButton="True" 
                        UpdateText="Save" />
                    <asp:BoundField DataField="SiteID" HeaderText="SiteID" InsertVisible="False" 
                        ReadOnly="True" SortExpression="SiteID" />
                    <asp:TemplateField HeaderText="Site Name">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtSiteName" runat="server" 
                                Text='<%# Bind("SiteName", "{0}") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:LinkButton ID="lbtnAdd" runat="server" ForeColor="#FFFFCC" 
                                onclick="lbtnAddSite_Click">Add Site</asp:LinkButton>
                        </FooterTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("SiteName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="IsInUse">
                        <EditItemTemplate>
                            <asp:CheckBox ID="chkIsInUse" runat="server" Checked='<%# Bind("IsInUse") %>' />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIsInUse" runat="server" Checked='<%# Bind("IsInUse") %>' 
                                Enabled="False" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Rank" DataField="Rank" SortExpression="Rank" />
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
                    <asp:BoundField DataField="udt" HeaderText="Update Date" ReadOnly="True">
                        <ItemStyle Font-Size="8pt" />
                    </asp:BoundField>
                    <asp:BoundField DataField="cdt" HeaderText="Create Date" SortExpression="cdt">
                        <ItemStyle Font-Size="8pt" />
                    </asp:BoundField>
                </Columns>
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <EmptyDataTemplate>
                    <asp:LinkButton ID="lbtnAddSite" runat="server" onclick="lbtnAddSite_Click">Create Site</asp:LinkButton>
                </EmptyDataTemplate>
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDSSite" runat="server" 
                ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                SelectCommand="SELECT * FROM [v_Site] ORDER BY Rank" 
                CancelSelectOnNullParameter="False"></asp:SqlDataSource>
           <asp:HiddenField ID="hfUserId" runat="server" />

            <asp:FormView ID="fvSite" runat="server" BorderStyle="Solid" 
               BorderWidth="2px" CellPadding="0" ForeColor="#333333" 
                BorderColor="#FFE0C0" DefaultMode="Insert" DataKeyNames="SiteID" 
               DataSourceID="SqlDSSite" oniteminserting="fvSite_ItemInserting" 
                onmodechanging="fvSite_ModeChanging">
                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                <InsertItemTemplate>
                    <asp:LinkButton ID="lbtnInsertRole" runat="server" CausesValidation="True" CommandName="Insert"
                        Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Save"></asp:LinkButton>
                    <asp:LinkButton ID="lbtnInsertRoleCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                        Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Cancel"></asp:LinkButton>
                   <table border="0" cellpadding="2" cellspacing="1" style="padding-right: 0px; padding-left: 0px;
                        font-size: 10pt; padding-bottom: 0px; margin: 0px; border-top-style: none; padding-top: 0px;
                        font-family: 'Arial Unicode MS'; border-right-style: none; border-left-style: none;
                        border-bottom-style: none" width="100%">
                        <tr>
                            <td class="FVTDINSCol1">Site Name</td>
                            <td class="FVTDINSCol2">
                                <asp:TextBox ID="txtSiteName" runat="server" Columns="15" 
                                    Text='<%# Bind("SiteName") %>'></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="FVTDINSCol1">Rank</td>
                            <td class="FVTDINSCol2">
                                <asp:TextBox ID="txtRank" runat="server" Columns="9" 
                                    Text='<%# Bind("Rank") %>'></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </InsertItemTemplate>
                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            </asp:FormView>           
        </td>
    </tr>
    <tr>
        <td style="vertical-align:top; width:50%">
            <div style="color: #FFFFFF; background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x;">Site Line Maintenance:</div>
            <asp:GridView ID="gvSiteLine" runat="server" AutoGenerateColumns="False"
                CellPadding="2" ForeColor="Black" 
                Font-Names="Arial Narrow" Font-Size="10pt" DataSourceID="SqlDSSiteLine" 
                DataKeyNames="LineID" ShowFooter="True" 
                BackColor="LightGoldenrodYellow" 
                BorderColor="Tan" BorderWidth="1px" 
                onrowupdating="gvSiteLine_RowUpdating" Width="100%">
                <FooterStyle BackColor="Tan" />
                <Columns>
                    <asp:CommandField CancelText="Cancel" DeleteText="Del" EditText="Edit" 
                        InsertText="Add" NewText="New" SelectText="Select" ShowEditButton="True" 
                        UpdateText="Save" />
                    <asp:CommandField CancelText="Cancel" DeleteText="Delete" EditText="Edit" 
                        InsertText="Save" NewText="Add" SelectText="Select" ShowSelectButton="True" 
                        UpdateText="Save" />
                    <asp:TemplateField HeaderText="LineName" InsertVisible="False" 
                        SortExpression="LineName">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("LineName") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("LineName") %>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:LinkButton ID="lbtnAddLine" runat="server" onclick="lbtnAddSiteLine_Click">Add Line</asp:LinkButton>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:CheckBoxField DataField="IsInUse" HeaderText="IsInUse" 
                        SortExpression="IsInUse" />
                    <asp:BoundField DataField="Rank" HeaderText="Rank" SortExpression="Rank" />
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
                    <asp:BoundField DataField="udt" HeaderText="Update Date" ReadOnly="True">
                        <ItemStyle Font-Size="8pt" />
                    </asp:BoundField>
                </Columns>
                <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                    HorizontalAlign="Center" />
                <EmptyDataTemplate>
                    <asp:LinkButton ID="lbtnAddSiteLine" runat="server" onclick="lbtnAddSiteLine_Click">Create Line</asp:LinkButton>
                </EmptyDataTemplate>
                <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                <HeaderStyle BackColor="Tan" Font-Bold="True" />
                <AlternatingRowStyle BackColor="PaleGoldenrod" />
            </asp:GridView>
            <asp:FormView ID="fvSiteLine" runat="server" BorderStyle="Solid" 
               BorderWidth="2px" CellPadding="0" ForeColor="#333333" 
                BorderColor="#FFE0C0" DefaultMode="Insert" DataKeyNames="LineID" 
               DataSourceID="SqlDSSiteLine" oniteminserting="fvSiteLine_ItemInserting" 
                onmodechanging="fvSiteLine_ModeChanging">
                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                <InsertItemTemplate>
                    <asp:LinkButton ID="lbtnInsertRole" runat="server" CausesValidation="True" CommandName="Insert"
                        Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Save"></asp:LinkButton>
                    <asp:LinkButton ID="lbtnInsertRoleCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                        Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Cancel"></asp:LinkButton>
                   <table border="0" cellpadding="2" cellspacing="1" style="padding-right: 0px; padding-left: 0px;
                        font-size: 10pt; padding-bottom: 0px; margin: 0px; border-top-style: none; padding-top: 0px;
                        font-family: 'Arial Unicode MS'; border-right-style: none; border-left-style: none;
                        border-bottom-style: none" width="100%">
                        <tr>
                            <td class="FVTDINSCol1">Line Name</td>
                            <td class="FVTDINSCol2">
                                <asp:TextBox ID="txtLineName" runat="server" Columns="15" 
                                    Text='<%# Bind("LineName") %>'></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="FVTDINSCol1">Rank</td>
                            <td class="FVTDINSCol2">
                                <asp:TextBox ID="txtRank" runat="server" Columns="9" 
                                    Text='<%# Bind("Rank") %>'></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </InsertItemTemplate>
                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            </asp:FormView>
            <asp:SqlDataSource ID="SqlDSSiteLine" runat="server" 
                ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                SelectCommand="SELECT * FROM [v_SiteLine]
WHERE SiteID=@SiteID
ORDER BY [Rank]">
                <SelectParameters>
                    <asp:ControlParameter ControlID="gvSite" Name="SiteID" 
                        PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:SqlDataSource>
        </td>
        <td style="vertical-align:top; width:50%">
        <div style="color: #FFFFFF; background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x;">Site Shift Maintenance:</div>
            <asp:GridView ID="gvSiteShift" runat="server" AutoGenerateColumns="False"
                CellPadding="2" ForeColor="Black" 
                Font-Names="Arial Narrow" Font-Size="10pt" DataSourceID="SqlDSSiteShift" 
                DataKeyNames="ShiftID" ShowFooter="True" 
                BackColor="LightGoldenrodYellow" 
                BorderColor="Tan" BorderWidth="1px" 
                onrowupdating="gvSiteShift_RowUpdating" Width="100%">
                <FooterStyle BackColor="Tan" />
                <Columns>
                    <asp:CommandField CancelText="Cancel" DeleteText="Del" EditText="Edit" 
                        InsertText="Add" NewText="New" SelectText="Select" ShowEditButton="True" 
                        UpdateText="Save" />
                    <asp:CommandField CancelText="Cancel" DeleteText="Delete" EditText="Edit" 
                        InsertText="Save" NewText="Add" SelectText="Select" ShowSelectButton="True" 
                        UpdateText="Save" />
                    <asp:TemplateField HeaderText="ShiftName" InsertVisible="False" 
                        SortExpression="ShiftName">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("ShiftName") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("ShiftName") %>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:LinkButton ID="lbtnAddShift" runat="server" 
                                onclick="lbtnAddSiteShift_Click">Add Shift</asp:LinkButton>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:CheckBoxField DataField="IsInUse" HeaderText="IsInUse" 
                        SortExpression="IsInUse" />
                    <asp:BoundField DataField="Rank" HeaderText="Rank" SortExpression="Rank" />
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
                    <asp:BoundField DataField="udt" HeaderText="Update Date" ReadOnly="True">
                        <ItemStyle Font-Size="8pt" />
                    </asp:BoundField>
                </Columns>
                <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                    HorizontalAlign="Center" />
                <EmptyDataTemplate>
                    <asp:LinkButton ID="lbtnAddSiteShift" runat="server" onclick="lbtnAddSiteShift_Click">Create Shift</asp:LinkButton>
                </EmptyDataTemplate>
                <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                <HeaderStyle BackColor="Tan" Font-Bold="True" />
                <AlternatingRowStyle BackColor="PaleGoldenrod" />
            </asp:GridView>
            <asp:FormView ID="fvSiteShift" runat="server" BorderStyle="Solid" 
               BorderWidth="2px" CellPadding="0" ForeColor="#333333" 
                BorderColor="#FFE0C0" DefaultMode="Insert" DataKeyNames="ShiftID" 
               DataSourceID="SqlDSSiteLine" oniteminserting="fvSiteShift_ItemInserting" 
                onmodechanging="fvSiteShift_ModeChanging">
                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                <InsertItemTemplate>
                    <asp:LinkButton ID="lbtnInsertRole" runat="server" CausesValidation="True" CommandName="Insert"
                        Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Save"></asp:LinkButton>
                    <asp:LinkButton ID="lbtnInsertRoleCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                        Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Cancel"></asp:LinkButton>
                   <table border="0" cellpadding="2" cellspacing="1" style="padding-right: 0px; padding-left: 0px;
                        font-size: 10pt; padding-bottom: 0px; margin: 0px; border-top-style: none; padding-top: 0px;
                        font-family: 'Arial Unicode MS'; border-right-style: none; border-left-style: none;
                        border-bottom-style: none" width="100%">
                        <tr>
                            <td class="FVTDINSCol1">Shift Name</td>
                            <td class="FVTDINSCol2">
                                <asp:TextBox ID="txtShiftName" runat="server" Columns="15" 
                                    Text='<%# Bind("ShiftName") %>'></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="FVTDINSCol1">Rank</td>
                            <td class="FVTDINSCol2">
                                <asp:TextBox ID="txtRank" runat="server" Columns="9" 
                                    Text='<%# Bind("Rank") %>'></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </InsertItemTemplate>
                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            </asp:FormView>
            <asp:SqlDataSource ID="SqlDSSiteShift" runat="server" 
                ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                SelectCommand="SELECT * FROM [v_SiteShift]
WHERE SiteID=@SiteID
ORDER BY [Rank]">
                <SelectParameters>
                    <asp:ControlParameter ControlID="gvSite" Name="SiteID" 
                        PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:SqlDataSource>
        </td>
    </tr>
</table>

</asp:Content>

