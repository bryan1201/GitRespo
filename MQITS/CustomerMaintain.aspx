<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="CustomerMaintain.aspx.cs" Inherits="CustomerMaintain"  Title="=== MQITS::Customer Maintain ===" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
    <asp:GridView ID="gvCustomer" runat="server" AutoGenerateColumns="False" 
        CellPadding="4" ForeColor="#333333" GridLines="Vertical" ShowFooter="True" Width="100%">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <Columns>
            <asp:CommandField ShowEditButton="True" CancelText="Cancel" EditText="Edit" 
                SelectText="Select" UpdateText="Save" DeleteText="Delete" 
                ShowDeleteButton="True" ShowSelectButton="True" >
            </asp:CommandField>
            <asp:BoundField HeaderText="ID" />
            <asp:TemplateField HeaderText="Customer">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:LinkButton ID="lbtnAddCustomer" runat="server">Add Customer</asp:LinkButton>
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
    <table>
        <tr>
            <td>
                <asp:GridView ID="gvPhaseTemplate" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" ForeColor="#333333" GridLines="Vertical" ShowFooter="True">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <Columns>
                        <asp:CommandField ShowEditButton="True" CancelText="Cancel" EditText="Edit" 
                            SelectText="Select" UpdateText="Save" />
                        <asp:CommandField ShowDeleteButton="True" DeleteText="Delete" />
                        <asp:BoundField HeaderText="ID" />
                        <asp:TemplateField HeaderText="Phase Name">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lbtnAddPhase" runat="server">Add Phase</asp:LinkButton>
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
                        <asp:BoundField HeaderText="Rank" />
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
                <asp:GridView ID="gvStation" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" ForeColor="#333333" GridLines="Vertical" ShowFooter="True">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <Columns>
                        <asp:CommandField ShowEditButton="True" CancelText="Cancel" EditText="Edit" 
                            SelectText="Select" UpdateText="Save" />
                        <asp:CommandField ShowDeleteButton="True" DeleteText="Delete" />
                        <asp:BoundField HeaderText="ID" />
                        <asp:TemplateField HeaderText="Station Name">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lbtnAddStation" runat="server">Add Station</asp:LinkButton>
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
                        <asp:BoundField HeaderText="Rank" />
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

