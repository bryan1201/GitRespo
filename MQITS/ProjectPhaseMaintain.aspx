<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="ProjectPhaseMaintain.aspx.cs" Inherits="ProjectPhaseMaintain" Title="=== MQITS:Project-Phase Maintain ===" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
<div>
<table>
    <tr>
        <td>Project:</td>
        <td><asp:DropDownList ID="ddlProject" runat="server"></asp:DropDownList></td>
        <td>&nbsp;</td>
        <td>Customer:</td>
        <td>
            <asp:Label ID="lblCustomer" runat="server" Text="HP"></asp:Label></td>
    </tr></table>
</div>
    <asp:GridView ID="gvProjectPhase" runat="server" AutoGenerateColumns="False" 
        Caption="Project-Phase Maintain" CaptionAlign="Left" CellPadding="4" 
        ForeColor="#333333" ShowFooter="True">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <Columns>
            <asp:CommandField ShowEditButton="True" CancelText="Cancel" DeleteText="Delete" 
                EditText="Edit" />
            <asp:CommandField ShowDeleteButton="True" DeleteText="Delete" />
            <asp:BoundField HeaderText="ID" />
            <asp:TemplateField HeaderText="Phase Name">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:LinkButton ID="lbtnAddPhase" runat="server">AddPhase</asp:LinkButton>
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Rank" />
            <asp:BoundField HeaderText="Start Date" />
            <asp:TemplateField HeaderText="IsEnd">
                <EditItemTemplate>
                    <asp:CheckBox ID="chkIsEnd" runat="server" />
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkIsEnd" runat="server" Enabled="False" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="End Date" />
            <asp:BoundField HeaderText="editor" />
            <asp:BoundField HeaderText="Update Date" />
        </Columns>
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>
    <asp:GridView ID="gvProjectPCA" runat="server" AutoGenerateColumns="False" 
        Caption="PCA P/N" CaptionAlign="Left" CellPadding="4" 
        ForeColor="#333333" ShowFooter="True">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <Columns>
            <asp:CommandField ShowEditButton="True" CancelText="Cancel" DeleteText="Delete" 
                EditText="Edit" />
            <asp:CommandField ShowDeleteButton="True" DeleteText="Delete" />
            <asp:BoundField HeaderText="ID" />
            <asp:TemplateField HeaderText="PCA P/N">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:LinkButton ID="lbtnAddPCA" runat="server">Add PCA P/N</asp:LinkButton>
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server"></asp:Label>
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
    &nbsp;
</asp:Content>

