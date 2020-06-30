<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="ProjectMemberMaintain.aspx.cs" Inherits="ProjectMemberMaintain" Title="=== MQITS:ProjectMember Maintain ===" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
    <table>
        <tr>
            <td colspan="2">
            <table><tr><td>Project:</td><td><asp:DropDownList ID="ddlProject" runat="server"></asp:DropDownList></td></tr></table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="gvProjectOwner" runat="server" Caption="Project Owners" 
                CaptionAlign="Left" CellPadding="4" ForeColor="#333333" 
                AutoGenerateColumns="False" ShowFooter="True">
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" DeleteText="Delete" />
                    <asp:BoundField HeaderText="ID" />
                    <asp:TemplateField HeaderText="Owner Name">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:LinkButton ID="lbtnAddOwner" runat="server">Add Owner</asp:LinkButton>
                        </FooterTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="IsInUse">
                        <EditItemTemplate>
                            <asp:CheckBox ID="chkIsInUse" runat="server" Checked="True" Enabled="True" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIsInUse" runat="server" Checked="True" Enabled="False" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="editor" />
                    <asp:BoundField HeaderText="Update Date" />
                    <asp:BoundField HeaderText="Create Date" />
                </Columns>
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                </asp:GridView>
            </td>
            <td>
                <asp:GridView ID="gvProjectMember" runat="server" Caption="Project Members" 
                    CaptionAlign="Left" CellPadding="4" ForeColor="#333333" 
                    AutoGenerateColumns="False" ShowFooter="True">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" DeleteText="Delete" />
                        <asp:BoundField HeaderText="ID" />
                        <asp:TemplateField HeaderText="Member Name">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtMember" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lbtnAddMember" runat="server">Add Member</asp:LinkButton>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblMember" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IsInUse">
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkIsInUse" runat="server" Checked="True" Enabled="True" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsInUse" runat="server" Checked="True" Enabled="False" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="editor" />
                        <asp:BoundField HeaderText="Update Date" />
                        <asp:BoundField HeaderText="Create Date" />
                        <asp:BoundField HeaderText="RoleName" />
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
</asp:Content>

