<%@ Page Title="--- MQITS::ProjectMaterial ---" Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MProjectMaterial.aspx.cs" Inherits="MProjectMaterial" MaintainScrollPositionOnPostback="true" StylesheetTheme="MMContainer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
    <div style="text-align:center; margin-left:1px;">
    <table style="width:100%; text-align:left;" cellpadding="0" cellspacing="0">
        <tr>
            <td style="width: 30%; background-color: #b6cade; margin-left: 50px;">
                Customer</td>
            <td style="width: 30%; background-color: #b6cade; margin-left: 50px;">
                Project</td>
            <td rowspan="2" style="background-color: #b6cade; text-align: center;">
                <asp:Button ID="btnQuery" runat="server" Font-Names="Arial Rounded MT Bold" 
                        Font-Size="10pt" Height="32px" Text="Query" Width="66px"  />
            </td>
        </tr>
        <tr>
            <td style="background-color: #eff3f8; width: 471px;">
                    <asp:DropDownList ID="ddlCustomer" 
                        runat="server" DataSourceID="SqlDSqCustomer" DataTextField="CustomerName" 
                        DataValueField="CustomerID" AutoPostBack="True"></asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDSqCustomer" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                        SelectCommand="SELECT * FROM dbo.v_Customer WHERE [IsEnable] = @IsEnable AND [IsVirtual] = @IsVirtual ORDER BY Rank">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="hfUserId" Name="UserId" PropertyName="Value" />
                            <asp:Parameter DefaultValue="True" Name="IsEnable" Type="Boolean" />
                            <asp:Parameter DefaultValue="False" Name="IsVirtual" Type="Boolean" />
                        </SelectParameters>
                    </asp:SqlDataSource>
            </td>
            <td style="background-color: #eff3f8; width: 471px;">
                <asp:DropDownList ID="ddlProject" 
                        runat="server" DataSourceID="SqlDSqProject" DataTextField="ProjectName" 
                        DataValueField="ProjectID" AutoPostBack="true" 
                    onselectedindexchanged="ddlProject_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDSqProject" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                        SelectCommand="SELECT -1 AS ProjectID, '---Select One---'  AS ProjectName, 0 AS Rank
UNION
SELECT [ProjectID], [ProjectName], Rank FROM [v_Project] WHERE ([IsEOSL] = @IsEOSL) AND IsMP = @IsMP
AND Customer = @CustomerID ORDER BY [Rank]">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="False" Name="IsEOSL" />
                        <asp:Parameter DefaultValue="True" Name="IsMP" />
                        <asp:ControlParameter ControlID="hfUserId" DefaultValue="IEC970101" 
                                Name="editor" PropertyName="Value" />
                        <asp:ControlParameter ControlID="ddlCustomer" DefaultValue="" Name="CustomerID" 
                                PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
        </tr>
    </table>
       <table style="border: 1px solid #6666FF; padding: 1px; width: 100%; background-color: #FFCCFF; text-align:left;" frame="below" 
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
                    CaptionAlign="Left" Width="100%" AllowSorting="True">
                    <FooterStyle BackColor="Tan" />
                    <Columns>
                        <asp:CommandField CancelText="Cancel" DeleteText="Del" EditText="Edit" 
                            InsertText="Save" NewText="Add" SelectText="Sel" ShowEditButton="True" 
                            UpdateText="Save" />
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
</div>
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
<asp:HiddenField ID="hfUserId" runat="server" />
<asp:HiddenField ID="hfProjectID" runat="server" />
</asp:Content>