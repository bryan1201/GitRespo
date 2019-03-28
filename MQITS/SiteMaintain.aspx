<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="SiteMaintain.aspx.cs" Inherits="SiteMaintain" Title="=== MQITS::Site Maintain ===" Theme="MMContainer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
    <div style="color: #FFFFFF; background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x;">Site Maintenance:</div>
    <asp:GridView ID="gvSite" runat="server" CaptionAlign="Left" CellPadding="4" ForeColor="#333333" 
        AutoGenerateColumns="False" ShowFooter="True">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <Columns>
            <asp:CommandField ShowEditButton="True" CancelText="Cancel" DeleteText="Delete" 
                EditText="Edit" UpdateText="Save" />
            <asp:CommandField ShowDeleteButton="True" DeleteText="Delete" />
            <asp:BoundField HeaderText="ID" />
            <asp:TemplateField HeaderText="Site Name">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:LinkButton ID="lbtnAddSite" runat="server">Add Site</asp:LinkButton>
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Current Phase" />
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
    <table>
        <tr>
            <td>
                <div style="color: #FFFFFF; background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x;">Site Owner Maintenance:</div>
                <asp:GridView ID="gvSiteOwner" runat="server" CaptionAlign="Left" CellPadding="4" ForeColor="#333333" 
                AutoGenerateColumns="False" ShowFooter="True">
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <Columns>
                    <asp:CommandField ShowEditButton="True" CancelText="Cancel" DeleteText="Delete" 
                        EditText="Edit" UpdateText="Save" />
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
                <div style="color: #FFFFFF; background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x;">Site Members Maintenance:</div>
                <asp:GridView ID="gvSiteMember" runat="server" CaptionAlign="Left" CellPadding="4" ForeColor="#333333" 
                    AutoGenerateColumns="False" ShowFooter="True">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <Columns>
                        <asp:CommandField ShowEditButton="True" CancelText="Cancel" DeleteText="Delete" 
                            EditText="Edit" UpdateText="Save" />
                        <asp:CommandField ShowDeleteButton="True" DeleteText="Delete" />
                        <asp:BoundField HeaderText="ID" />
                        <asp:TemplateField HeaderText="Member Name">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lbtnAddMember" runat="server">Add Member</asp:LinkButton>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server"></asp:Label>
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
        </tr>
    </table>
</asp:Content>

