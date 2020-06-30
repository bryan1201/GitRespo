<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MCustomer.aspx.cs" StylesheetTheme="MMContainer" Inherits="MCustomer" Title="=== MQITS::Customer Maintenance ===" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
<table style="width:100%; background-color: #eff3f8;" cellpadding="2" 
        cellspacing="2">
    <tr>
        <td style="vertical-align:top; width: 70%;">
           <asp:GridView ID="gvCustomer" runat="server" AutoGenerateColumns="False"
                CellPadding="2" ForeColor="Black" 
                Font-Names="Arial Narrow" Font-Size="11pt" DataSourceID="SqlDSCustomer" 
                DataKeyNames="CustomerID" ShowFooter="True"  
                onrowupdating="gvCustomer_RowUpdating" BackColor="LightGoldenrodYellow" 
                BorderColor="Tan" BorderWidth="1px" Width="100%" AllowSorting="True" > 
                <FooterStyle BackColor="Tan" />
                <Columns>
                    <asp:CommandField CancelText="Cancel" DeleteText="Del" EditText="Edit" 
                        InsertText="Add" NewText="New" SelectText="Select" ShowEditButton="True" 
                        UpdateText="Save" Visible="False" />
                    <asp:CommandField CancelText="Cancel" DeleteText="Delete" EditText="Edit" 
                        InsertText="Save" NewText="Add" SelectText="Select" ShowSelectButton="True" 
                        UpdateText="Save" />
                    <asp:BoundField DataField="CustomerID" HeaderText="ID" InsertVisible="False" 
                        ReadOnly="True" SortExpression="CustomerID" />
                    <asp:TemplateField HeaderText="Customer Name" SortExpression="CustomerName">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtCustomerName" runat="server" 
                                Text='<%# Bind("CustomerName", "{0}") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:LinkButton ID="lbtnAdd" runat="server" ForeColor="#FFFFCC" 
                                onclick="lbtnAdd_Click">Add</asp:LinkButton>
                        </FooterTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="IsEnable" SortExpression="IsEnable">
                        <EditItemTemplate>
                            <asp:CheckBox ID="chkIsEnable" runat="server" 
                                Checked='<%# Bind("IsEnable") %>' />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIsEnable" runat="server" Checked='<%# Bind("IsEnable") %>' 
                                Enabled="False" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Rank" DataField="Rank" SortExpression="Rank" />
                    <asp:TemplateField HeaderText="editor" SortExpression="editorName">
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
                    <asp:BoundField DataField="udt" HeaderText="Update Date" ReadOnly="True" 
                        SortExpression="udt">
                        <ItemStyle Font-Size="8pt" />
                    </asp:BoundField>
                    <asp:BoundField DataField="cdt" HeaderText="Create Date" SortExpression="cdt">
                        <ItemStyle Font-Size="8pt" />
                    </asp:BoundField>
                </Columns>
                <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                    HorizontalAlign="Center" />
                <EmptyDataTemplate>
                    <asp:LinkButton ID="lbtnAdd" runat="server" onclick="lbtnAdd_Click">Create Customer</asp:LinkButton>
                </EmptyDataTemplate>
                <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                <HeaderStyle BackColor="Tan" Font-Bold="True" />
                <AlternatingRowStyle BackColor="PaleGoldenrod" />
            </asp:GridView>        
        </td>
        <td style="vertical-align:top; width: 30%;">
            <asp:FormView ID="fvCustomer" runat="server" BorderStyle="Solid" 
               BorderWidth="2px" CellPadding="0" ForeColor="#333333" 
                BorderColor="#FFE0C0" DefaultMode="Insert" DataKeyNames="CustomerID" 
               DataSourceID="SqlDSCustomer" oniteminserting="fvCustomer_ItemInserting" 
                onmodechanging="fvCustomer_ModeChanging" 
                onitemcommand="FormView_ItemCommand">
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
                            <td class="FVTDINSCol1">Customer Name</td>
                            <td class="FVTDINSCol2">
                                <asp:TextBox ID="txtCustomerName" runat="server" Columns="15" 
                                    Text='<%# Bind("CustomerName") %>'></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="FVTDINSCol1">Rank</td>
                            <td class="FVTDINSCol2">
                                <asp:TextBox ID="txtRank" runat="server" Columns="9" 
                                    Text='<%# Bind("Rank") %>'></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="FVTDITCol1">IsEnable</td>
                            <td class="FVTDITCol2">
                                <asp:CheckBox ID="chkIsEnable" runat="server" 
                                    Checked='<%# Bind("IsEnable") %>' />
                            </td>
                        </tr>
                    </table>
                </InsertItemTemplate>
                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            </asp:FormView>        
        </td>
    </tr>
</table>

    <asp:SqlDataSource ID="SqlDSCustomer" runat="server" 
        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
        SelectCommand="SELECT * FROM [v_Customer] ORDER BY Rank" 
        CancelSelectOnNullParameter="False"></asp:SqlDataSource>
   <asp:HiddenField ID="hfUserId" runat="server" />

    <table style="width:100%; " cellpadding="3" cellspacing="3">
        <tr>
            <td style="width: 33%; color: #FFFFFF; height: 20px; background-image: url('images/5bg_Nav.jpg');">Customer Define Phase</td>
            <td style="color: #FFFFFF; background-image: url('images/5bg_Nav.jpg');" 
                colspan="2">
                Customer Define Station (by PCA/CPU)</td>
        </tr>
        <tr>
            <td style="vertical-align: top; background-color: #eff3f8;" rowspan="2">
                <asp:GridView ID="gvPhaseTemplate" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" ForeColor="Black" ShowFooter="True" 
                    DataSourceID="SqlDSCustomerPhase" Font-Size="10pt" 
                    onrowupdating="gvPhaseTemplate_RowUpdating" BackColor="White" 
                    BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
                    AllowSorting="True" DataKeyNames="PhaseID">
                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                    <Columns>
                        <asp:CommandField ShowEditButton="True" CancelText="Cancel" EditText="Edit" 
                            SelectText="Select" UpdateText="Save" />
                        <asp:BoundField HeaderText="ID" DataField="PhaseID" SortExpression="PhaseID" 
                            ReadOnly="True" Visible="False" />
                        <asp:TemplateField HeaderText="Phase Name" SortExpression="PhaseName">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" 
                                    Text='<%# Bind("PhaseName", "{0}") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lbtnAddPhase" runat="server" ForeColor="#333399" 
                                    onclick="lbtnAddPhase_Click">Add Phase</asp:LinkButton>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("PhaseName", "{0}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IsInUse" SortExpression="IsEnable">
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkIsInUse" runat="server" 
                                    Checked='<%# Bind("IsEnable") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsInUse" runat="server" Checked='<%# Bind("IsEnable") %>' 
                                    Enabled="False" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Rank" DataField="Rank" SortExpression="Rank" />
                        <asp:BoundField HeaderText="editor" DataField="editorName" 
                            SortExpression="editorName" Visible="False" />
                        <asp:BoundField HeaderText="Update Date" DataField="udt" SortExpression="udt" 
                            Visible="False" />
                    </Columns>
                    <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                    <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDSCustomerPhase" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                    SelectCommand="SELECT * FROM [v_CustomerPhase] WHERE CustomerID = @CustomerID ORDER BY Rank" 
                    CancelSelectOnNullParameter="False">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="gvCustomer" Name="CustomerID" 
                            PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:FormView ID="fvPhase" runat="server" BorderStyle="Solid" 
                   BorderWidth="2px" CellPadding="0" ForeColor="#333333" 
                    BorderColor="#FFE0C0" DefaultMode="Insert" DataKeyNames="PhaseID" 
                   DataSourceID="SqlDSCustomerPhase" oniteminserting="fvPhase_ItemInserting" Visible="False" 
                    onitemcommand="FormView_ItemCommand">
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                    <InsertItemTemplate>
                        <asp:LinkButton ID="lbtnInsertPhase" runat="server" CausesValidation="True" CommandName="Insert"
                            Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Save"></asp:LinkButton>
                        <asp:LinkButton ID="lbtnInsertPhaseCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                            Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Cancel"></asp:LinkButton>
                       <table border="0" cellpadding="2" cellspacing="1" style="padding-right: 0px; padding-left: 0px;
                            font-size: 10pt; padding-bottom: 0px; margin: 0px; border-top-style: none; padding-top: 0px;
                            font-family: 'Arial Unicode MS'; border-right-style: none; border-left-style: none;
                            border-bottom-style: none" width="100%">
                            <tr>
                                <td class="FVTDINSCol1">Phase Name</td>
                                <td class="FVTDINSCol2">
                                    <asp:TextBox ID="txtPhaseName" runat="server" Columns="15" 
                                        Text='<%# Bind("PhaseName") %>'></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="FVTDINSCol1">Rank</td>
                                <td class="FVTDINSCol2">
                                    <asp:TextBox ID="txtRank" runat="server" Columns="9" 
                                        Text='<%# Bind("Rank") %>'></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="FVTDITCol1">IsEnable</td>
                                <td class="FVTDITCol2">
                                    <asp:CheckBox ID="chkIsEnable" runat="server" 
                                        Checked='<%# Bind("IsEnable") %>' />
                                </td>
                            </tr>
                        </table>
                    </InsertItemTemplate>
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                </asp:FormView>
            </td>
            <td style="vertical-align: top; background-color: #eff3f8;">
                <asp:GridView ID="gvStation" runat="server" AutoGenerateColumns="False" 
                    CellPadding="3" ShowFooter="True" 
                    DataSourceID="SqlDSCustomerStation" Font-Size="10pt" 
                    onrowupdating="gvStation_RowUpdating" 
                    DataKeyNames="ID,CustomerID,StationID,MType,GroupType" 
                    BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" 
                    BorderWidth="1px" EmptyDataText="-- No Station defined --" 
                    AllowSorting="True">
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                    <Columns>
                        <asp:CommandField ShowEditButton="True" CancelText="Cancel" EditText="Edit" 
                            SelectText="Select" UpdateText="Save" />
                        <asp:BoundField DataField="StationID" HeaderText="ID" 
                            SortExpression="StationID" ReadOnly="True" Visible="False" />
                        <asp:TemplateField HeaderText="StationName" SortExpression="StationName">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtStation" runat="server" Text='<%# Bind("StationName") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStation" runat="server" Text='<%# Bind("StationName") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lbtnAddStation" runat="server" ForeColor="#000099" 
                                    onclick="lbtnAddStation_Click">Add Station</asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="MType" SortExpression="MType">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlMType" runat="server" 
                                    DataSourceID="SqlDSv_ProjectMType" DataTextField="TEXT" DataValueField="VALUE" 
                                        SelectedValue='<%# Bind("MType", "{0}") %>'></asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("MType") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IsInUse" SortExpression="IsInUse">
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkIsInUse" runat="server" Checked='<%# Bind("IsInUse") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsInUse" runat="server" Checked='<%# Bind("IsInUse") %>' 
                                    Enabled="False" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rank" SortExpression="Rank">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Columns="6" 
                                    Text='<%# Bind("Rank", "{0}") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Rank") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="editor" DataField="editorName" Visible="False" />
                        <asp:BoundField HeaderText="Update Date" DataField="Udt" Visible="False" />
                    </Columns>
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                    <EmptyDataTemplate>
                        <asp:LinkButton ID="lbtnAddStation" runat="server" ForeColor="#3333CC" 
                            onclick="lbtnAddStation_Click">Add Station</asp:LinkButton>
                    </EmptyDataTemplate>
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" />
                </asp:GridView>
            </td>
            <td style="vertical-align: top; background-color: #eff3f8;">
                <asp:GridView ID="gvStationCPU" runat="server" AutoGenerateColumns="False" 
                    CellPadding="3" ShowFooter="True" 
                    DataSourceID="SqlDSCustomerStatopmCPU" Font-Size="10pt" 
                    onrowupdating="gvStation_RowUpdating" 
                    DataKeyNames="ID,CustomerID,StationID,MType,GroupType" 
                    BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" 
                    BorderWidth="1px" EmptyDataText="-- No Station defined --" 
                    AllowSorting="True">
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                    <Columns>
                        <asp:CommandField ShowEditButton="True" CancelText="Cancel" EditText="Edit" 
                            SelectText="Select" UpdateText="Save" />
                        <asp:BoundField DataField="StationID" HeaderText="ID" 
                            SortExpression="StationID" ReadOnly="True" Visible="False" />
                        <asp:TemplateField HeaderText="StationName" SortExpression="StationName">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtStation0" runat="server" Text='<%# Bind("StationName") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStation0" runat="server" Text='<%# Bind("StationName") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lbtnAddStation0" runat="server" ForeColor="#000099" 
                                    onclick="lbtnAddStation_Click">Add Station</asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="MType" SortExpression="MType">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlMType0" runat="server" 
                                    DataSourceID="SqlDSv_ProjectMType" DataTextField="TEXT" DataValueField="VALUE" 
                                        SelectedValue='<%# Bind("MType", "{0}") %>'></asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("MType") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IsInUse" SortExpression="IsInUse">
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkIsInUse0" runat="server" 
                                    Checked='<%# Bind("IsInUse") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsInUse1" runat="server" Checked='<%# Bind("IsInUse") %>' 
                                    Enabled="False" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rank" SortExpression="Rank">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Columns="6" 
                                    Text='<%# Bind("Rank", "{0}") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("Rank") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="editor" DataField="editorName" Visible="False" />
                        <asp:BoundField HeaderText="Update Date" DataField="Udt" Visible="False" />
                    </Columns>
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                    <EmptyDataTemplate>
                        <asp:LinkButton ID="lbtnAddStationCPU" runat="server" ForeColor="#3333CC" 
                            onclick="lbtnAddStation_Click">Add Station</asp:LinkButton>
                    </EmptyDataTemplate>
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top; background-color: #eff3f8;" colspan="2">
                <asp:FormView ID="fvStation" runat="server" BorderStyle="Solid" 
                   BorderWidth="2px" CellPadding="0" ForeColor="#333333" 
                    BorderColor="#FFE0C0" DefaultMode="Insert" DataKeyNames="StationID" 
                   DataSourceID="SqlDSCustomerStation" 
                    oniteminserting="fvStation_ItemInserting" Visible="False" 
                    onitemcommand="FormView_ItemCommand">
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                    <InsertItemTemplate>
                        <asp:LinkButton ID="lbtnInsertStation" runat="server" CausesValidation="True" CommandName="Insert"
                            Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Save"></asp:LinkButton>
                        <asp:LinkButton ID="lbtnInsertStationCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                            Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Cancel"></asp:LinkButton>
                       <table border="0" cellpadding="2" cellspacing="1" style="padding-right: 0px; padding-left: 0px;
                            font-size: 10pt; padding-bottom: 0px; margin: 0px; border-top-style: none; padding-top: 0px;
                            font-family: 'Arial Unicode MS'; border-right-style: none; border-left-style: none;
                            border-bottom-style: none" width="100%">
                            <tr>
                                <td class="FVTDINSCol1">Station Name</td>
                                <td class="FVTDINSCol2">
                                    <asp:TextBox ID="txtStationName" runat="server" Columns="15" 
                                        Text='<%# Bind("StationName", "{0}") %>'></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="FVTDINSCol1">MType</td>
                                <td class="FVTDINSCol2">
                                    <asp:DropDownList ID="ddlMType" runat="server" 
                                        DataSourceID="SqlDSv_ProjectMType" DataTextField="TEXT" DataValueField="VALUE" 
                                        SelectedValue='<%# Bind("MType", "{0}") %>'></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="FVTDINSCol1">Rank</td>
                                <td class="FVTDINSCol2">
                                    <asp:TextBox ID="txtRank" runat="server" Columns="9" 
                                        Text='<%# Bind("Rank", "{0}") %>'></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="FVTDITCol1">IsInUse</td>
                                <td class="FVTDITCol2">
                                    <asp:CheckBox ID="chkIsEnable" runat="server" 
                                        Checked='<%# Bind("IsInUse") %>' />
                                </td>
                            </tr>
                        </table>
                    </InsertItemTemplate>
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                </asp:FormView>
                <asp:SqlDataSource ID="SqlDSCustomerStation" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                    SelectCommand="SELECT * FROM [v_Station] WHERE CustomerID = @CustomerID AND MType='PCA' ORDER BY Rank" 
                    CancelSelectOnNullParameter="False">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="gvCustomer" Name="CustomerID" 
                            PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDSv_ProjectMType" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                    SelectCommand="SELECT * FROM [v_ProjectMType]"></asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDSCustomerStatopmCPU" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                    SelectCommand="SELECT * FROM [v_Station] WHERE CustomerID = @CustomerID AND MType='CPU' ORDER BY Rank" 
                    CancelSelectOnNullParameter="False">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="gvCustomer" Name="CustomerID" 
                            PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDSModule" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                        SelectCommand="
                        SELECT 0 ModuleID, '' ModuleName, 0 AS Rank UNION
                        SELECT [ModuleID], [ModuleName], Rank FROM [v_Module] WHERE (([IsEnable] = @IsEnable) AND ([IsVirtual] = @IsVirtual)) ORDER BY [Rank]">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="True" Name="IsEnable" Type="Boolean" />
                            <asp:Parameter DefaultValue="False" Name="IsVirtual" Type="Boolean" />
                        </SelectParameters>
                    </asp:SqlDataSource> 
                </td>
        </tr>
    </table>
</asp:Content>

