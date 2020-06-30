<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ListMember.aspx.cs" Inherits="ListMember" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ListMember</title>
    <script language="javascript" type="text/javascript">
			function CloseWindow()
			{
				self.close();
			}
	</script>
</head>
<body>
    <form id="frmGetUser" method="post" runat="server">
    <div>
        <asp:TextBox ID="txtSearch" runat="server"  Visible="False"></asp:TextBox>
                     
        <asp:GridView ID="gvListMember" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" 
            CaptionAlign="Top" CellPadding="2" DataKeyNames="ChtName" Font-Names="Arial Unicode MS"
            Font-Size="10pt" ForeColor="#333333" HorizontalAlign="Justify" Width="90%" 
             OnSelectedIndexChanged="gvListMember_SelectedIndexChanged"  DataSourceID="SqlDSMember">
           <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Select"
                            Text="Select"></asp:LinkButton>
                    </ItemTemplate>                   
                    <HeaderStyle Width="10%" />
                    <ItemStyle Width="10%" />
                </asp:TemplateField>     
                           
               <asp:TemplateField HeaderText="BadgeCode" SortExpression="BadgeCode">
                     <ItemTemplate>
                        <asp:Label ID="lblBadgeCode" runat="server" Font-Size="Small" Text='<%# Bind("BadgeCode") %>'></asp:Label>
                    </ItemTemplate>                    
                   <HeaderStyle Width="20%" />
                   <ItemStyle Width="20%" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Name" SortExpression="ChtName">
                    <ItemTemplate>
                        <asp:Label ID="lblChtName" runat="server" Font-Size="Small" Text='<%# Bind("ChtName") %>'></asp:Label>
                    </ItemTemplate>  
                    <HeaderStyle Width="60%" />
                    <ItemStyle Width="60%" />
                </asp:TemplateField>
            </Columns>
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Left" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#284775" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <EditRowStyle BackColor="#999999" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDSMember" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>"
            SelectCommand="SELECT BadgeCode,ChtName &#13;&#10;FROM v_user &#13;&#10;WHERE IsInUse='True' AND &#13;&#10;(UPPER(ChtName) LIKE '%' + UPPER(@AssignedTo) + '%' &#13;&#10;OR UPPER(BadgeCode) LIKE '%' + UPPER(@AssignedTo) + '%')">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtSearch" Name="AssignedTo" PropertyName="Text" />
            </SelectParameters>
        </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
