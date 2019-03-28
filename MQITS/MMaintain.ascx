<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MMaintain.ascx.cs" Inherits="MMaintain" %>
<div style=" width:100%; font-family:Arial Unicode MS; font-size:12pt; color: #FFFFFF; background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x;" id="divMaintainTitle" runat="server">Maintain:&nbsp; <span id="spanTitle" runat="server"></span></div>
<div id="divPriority" runat="server" visible="false">
<asp:GridView ID="gvPriority" runat="server" AutoGenerateColumns="False" 
        CellPadding="2" BackColor="LightGoldenrodYellow" 
        BorderColor="Tan" BorderWidth="1px" DataKeyNames="ID" 
        DataSourceID="SqlDSIssuePriority" Font-Size="11pt" ShowFooter="True" 
        ForeColor="Black">
        <FooterStyle BackColor="Tan" />
        <Columns>
            <asp:CommandField ShowEditButton="True" CancelText="Cancel" DeleteText="Del" 
                EditText="Edit" SelectText="Sel" UpdateText="Save" />
            <asp:BoundField HeaderText="Priority" DataField="VALUE" 
                SortExpression="VALUE" />
            <asp:BoundField HeaderText="Name" DataField="TEXT" />
            <asp:CheckBoxField DataField="IsInUse" HeaderText="IsInUse" 
                SortExpression="IsInUse" />
            <asp:BoundField DataField="Rank" HeaderText="Rank" SortExpression="Rank" />
            <asp:BoundField HeaderText="editor" DataField="editorName" ReadOnly="True" />
            <asp:BoundField HeaderText="Update Date" DataField="Udt" 
                DataFormatString="{0:d}" />
            <asp:BoundField HeaderText="Create Date" DataField="Cdt" 
                DataFormatString="{0:d}" />
        </Columns>
        <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
            HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
        <HeaderStyle BackColor="Tan" Font-Bold="True" />
        <AlternatingRowStyle BackColor="PaleGoldenrod" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDSIssuePriority" runat="server" 
        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
        SelectCommand="SELECT * FROM v_IssuePriority ORDER BY Rank">
    </asp:SqlDataSource>
</div>
<div id="divLiability" runat="server" visible="false">
<asp:GridView ID="gvLiability" runat="server" AutoGenerateColumns="False" 
        CellPadding="4" ForeColor="Black" GridLines="Vertical" BackColor="White" 
        BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" DataKeyNames="ID" 
        DataSourceID="SqlDSIssueLiability" Font-Size="11pt" ShowFooter="True">
        <FooterStyle BackColor="#CCCC99" />
        <RowStyle BackColor="#F7F7DE" />
        <Columns>
            <asp:CommandField ShowEditButton="True" CancelText="Cancel" DeleteText="Del" 
                EditText="Edit" SelectText="Sel" UpdateText="Save" />
            <asp:BoundField HeaderText="Liability" DataField="VALUE" />
            <asp:BoundField HeaderText="Liability(Category) Name" DataField="TEXT" />
            <asp:CheckBoxField DataField="IsInUse" HeaderText="IsInUse" 
                SortExpression="IsInUse" />
<asp:BoundField HeaderText="Rank" DataField="Rank" SortExpression="Rank"></asp:BoundField>
            <asp:BoundField HeaderText="editor" DataField="editorName" />
            <asp:BoundField HeaderText="Update Date" DataField="Udt" 
                DataFormatString="{0:d}" />
            <asp:BoundField HeaderText="Create Date" DataField="Cdt" 
                DataFormatString="{0:d}" />
        </Columns>
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDSIssueLiability" runat="server" 
        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
        SelectCommand="SELECT * FROM [v_IssueLiability] ORDER BY [Rank]">
    </asp:SqlDataSource>
</div>
<div id="divIssueStatus" runat="server" visible="false">
<asp:GridView ID="gvIssueStatus" runat="server" AutoGenerateColumns="False" 
        CellPadding="4" ForeColor="Black" GridLines="Vertical" BackColor="White" 
        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" DataKeyNames="ID" 
        DataSourceID="SqlDSIssueStatus" Font-Size="11pt" ShowFooter="True">
        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
        <Columns>
            <asp:CommandField ShowEditButton="True" CancelText="Cancel" DeleteText="Del" 
                EditText="Edit" InsertText="Save" SelectText="Sel" 
                UpdateText="Save" />
            <asp:BoundField HeaderText="Status" DataField="VALUE" />
            <asp:BoundField HeaderText="IssueStatus Name" DataField="TEXT" />
            <asp:CheckBoxField DataField="IsInUse" HeaderText="IsInUse" 
                SortExpression="IsInUse" />
<asp:BoundField HeaderText="Rank" DataField="Rank" SortExpression="Rank"></asp:BoundField>
            <asp:BoundField HeaderText="editor" DataField="editorName" ReadOnly="True" />
            <asp:BoundField HeaderText="Update Date" DataField="Udt" 
                DataFormatString="{0:d}" />
            <asp:BoundField HeaderText="Create Date" DataField="Cdt" 
                DataFormatString="{0:d}" />
        </Columns>
        <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
        <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDSIssueStatus" runat="server" 
        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
        SelectCommand="SELECT * FROM [v_IssueStatus] ORDER BY [Rank]">
    </asp:SqlDataSource>
    &nbsp;</div>
<div id="divRMAMaintain" runat="server" visible="false">
<table>
<tr>
    <td>
    <asp:GridView ID="gvRMAMaintain" runat="server" AutoGenerateColumns="False" 
            CellPadding="4" ForeColor="Black" GridLines="Vertical" BackColor="White" 
            BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" DataKeyNames="ID" 
            DataSourceID="SqlDSRMAMaintain" Font-Size="11pt" ShowFooter="True" 
            onrowupdating="gvRMAMaintain_RowUpdating">
            <FooterStyle BackColor="#CCCC99" />
            <RowStyle BackColor="#F7F7DE" />
            <Columns>
                <asp:CommandField ShowEditButton="True" CancelText="Cancel" DeleteText="Del" 
                    EditText="Edit" SelectText="Sel" UpdateText="Save" />
                <asp:BoundField HeaderText="ID" DataField="vID" Visible="False" />
                <asp:TemplateField HeaderText="Title">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("viewTitle") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtvTitle" runat="server" Text='<%# Bind("viewTitle") %>' 
                            Columns="16"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:LinkButton ID="lbtnAdd" runat="server" onclick="lbtnAdd_Click">Add</asp:LinkButton>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:CheckBoxField DataField="IsInUse" HeaderText="IsInUse" 
                    SortExpression="IsInUse" />
                <asp:BoundField HeaderText="Rank" DataField="Rank" SortExpression="Rank" />
                <asp:BoundField HeaderText="editor" DataField="editorName" ReadOnly="True" />
                <asp:BoundField HeaderText="Update Date" DataField="Udt" 
                    DataFormatString="{0:d}" ReadOnly="True" />
                <asp:BoundField HeaderText="Create Date" DataField="Cdt" 
                    DataFormatString="{0:d}" ReadOnly="True" />
            </Columns>
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
            <EmptyDataTemplate>
                <asp:LinkButton ID="lbtnAdd" runat="server" onclick="lbtnAdd_Click">Add</asp:LinkButton>
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDSRMAMaintain" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
            SelectCommand="sp_RMAMaintain" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter DefaultValue="SelectRMADLL" Name="vchCmd" Type="String" />
                <asp:SessionParameter DefaultValue="" Name="vchObjectName" 
                    SessionField="RMAMaintainObjectName" Type="String" />
                <asp:SessionParameter Name="vchSet" 
                    DefaultValue="&lt;editor&gt;IEC891652&lt;/editor&gt;"
                     SessionField="RMAMaintainvchSet" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
    </td>
    <td style="vertical-align:top;">
        <asp:FormView ID="fvRMAMaintain" runat="server" BorderStyle="Solid" 
           BorderWidth="2px" CellPadding="0" ForeColor="#333333" 
            BorderColor="#FFE0C0" DefaultMode="Insert" DataKeyNames="ID" 
           DataSourceID="SqlDSRMAMaintainOne" Visible="False" 
            oniteminserting="fvRMAMaintain_ItemInserting" 
            onmodechanging="fvRMAMaintain_ModeChanging">
            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
            <InsertItemTemplate>
                <asp:LinkButton ID="lbtnInsertRMAMaintain" runat="server" 
                    CausesValidation="True" CommandName="Insert"
                    Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Save"></asp:LinkButton>
                <asp:LinkButton ID="lbtnInsertRMAMaintainCancel" runat="server" 
                    CausesValidation="False" CommandName="Cancel"
                    Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Cancel"></asp:LinkButton>
               <table border="0" cellpadding="2" cellspacing="1" style="padding-right: 0px; padding-left: 0px;
                    font-size: 10pt; padding-bottom: 0px; margin: 0px; border-top-style: none; padding-top: 0px;
                    font-family: 'Arial Unicode MS'; border-right-style: none; border-left-style: none;
                    border-bottom-style: none" width="100%">
                    <tr>
                        <td class="FVTDINSCol1">Title</td>
                        <td class="FVTDINSCol2">
                            <asp:TextBox ID="txtvTitle" runat="server" Columns="15" 
                                Text='<%# Bind("vTitle", "{0}") %>' ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="FVTDINSCol1">Rank</td>
                        <td class="FVTDINSCol2">
                            <asp:TextBox ID="TextBox1" runat="server" Columns="15" 
                                Text='<%# Bind("Rank", "{0}") %>' ></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </InsertItemTemplate>
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        </asp:FormView>
        <asp:SqlDataSource ID="SqlDSRMAMaintainOne" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" >
            <SelectParameters>
                <asp:ControlParameter ControlID="gvRMAMaintain" Name="ID" 
                    PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
    </td>
</tr>
</table>
</div>
<asp:HiddenField ID="hfUserId" runat="server" />