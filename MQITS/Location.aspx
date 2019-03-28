<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Location.aspx.cs" Inherits="Location" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>AddLocation</title>
</head>
<body>
    <form id="form1" runat="server">
       <div style="display:none">
       <asp:TextBox ID="txtUserID" runat="server" Width="60px"></asp:TextBox>
       <asp:TextBox ID="txtFailureDataID" runat="server" Width="60px"></asp:TextBox>
        <asp:TextBox ID="txtIssueID" runat="server" Width="60px"></asp:TextBox></div>
        <table align="left" cellpadding="1" cellspacing="1" style="clear: both; border-right: #b6cade 2px solid;
            border-top: #b6cade 2px solid; font-size: 10pt; float: none; border-left: #b6cade 2px solid;
            width: 50%; color: #333333; border-bottom: #b6cade 2px solid; font-family: 'Arial Unicode MS';
            border-collapse: collapse; background-color: white">
            <tr>
                <td style="width: 15%; background-color: #b6cade">Location</td>
                <td style="width: 15%; background-color: #eff3f8">
                    <asp:TextBox ID="txtLocation" runat="server"></asp:TextBox></td>
                <td style="width: 10%; background-color: #b6cade">Qty</td>
                <td style="width: 15%; background-color: #eff3f8">
                    <asp:TextBox ID="txtQty" runat="server" Width="75px"></asp:TextBox></td>
                <td align="left" rowspan="1" style="font-family: 'Arial Unicode MS'; background-color: #eff3f8;
                    text-align: left;">
                    <asp:Button ID="btnSave" runat="server"  Text="Save" Width="100px" OnClick="btnSave_Click" /></td>
            </tr>
            <tr>
                <td colspan="5" >
                    <asp:GridView ID="gvLocation" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical" DataSourceID="SqlDSLocation" AutoGenerateColumns="False" DataKeyNames="LocationID" OnRowDeleting="gvLocation_RowDeleting" Width="100%" OnRowUpdating="gvLocation_RowUpdating">
                    <FooterStyle BackColor="#CCCCCC" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="#CCCCCC" />
                    <Columns>
                     <asp:TemplateField ShowHeader="False">
                            <EditItemTemplate>
                                <asp:LinkButton ID="lbtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                    Text="Update"></asp:LinkButton>
                                <asp:LinkButton ID="lbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                    Text="Cancel"></asp:LinkButton>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                    Text="Edit"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="5%" />
                        </asp:TemplateField>
                         <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                                <asp:LinkButton ID="lbtnDel" runat="server" CausesValidation="False" CommandName="Delete" OnClientClick="return confirm('are you sure?')" 
                                    Text="Del"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="5%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="LocationID" SortExpression="LocationID" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblLocationID" runat="server" Text='<%# Bind("LocationID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Location" SortExpression="Location">
                            <ItemTemplate>
                                <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
                            </ItemTemplate>
                             <EditItemTemplate>
                                <asp:TextBox ID="txtLocation" runat="server" Text='<%# Bind("Location") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qty" SortExpression="Qty">
                            <ItemTemplate>
                                <asp:Label ID="lblQty" runat="server" Text='<%# Bind("Qty") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtQty" runat="server" Text='<%# Bind("Qty") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDSLocation" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT LocationID,Location,Qty,cdt &#13;&#10;FROM f_RMALocation&#13;&#10;WHERE FailureDataID=@FailureDataID AND IssueID=@IssueID">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtFailureDataID" Name="FailureDataID" PropertyName="Text" />
                        <asp:ControlParameter ControlID="txtIssueID" Name="IssueID" PropertyName="Text" />
                    </SelectParameters>
                </asp:SqlDataSource>
                    <br />
            
                <asp:Button ID="btnConfirm" runat="server" Text="Confirm and close window" OnClick="btnConfirm_Click" Font-Bold="True" Font-Names="Arial Unicode MS" ForeColor="Red" Width="185px" /></td>
            </tr>
        </table>
       
    </form>
</body>
</html>
