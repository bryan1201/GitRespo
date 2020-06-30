<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MPersonalProfile.aspx.cs" Inherits="MPersonalProfile" Title="=== MQITS:Personal Profile ===" MaintainScrollPositionOnPostback="true" Theme="MMContainer" StylesheetTheme="MMContainer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">

    <div style=" width:100%">
    <div style=" width:100%; font-family:Arial Unicode MS; font-size:12pt; color: #FFFFFF; background-image: url('images/5bg_Nav.jpg'); background-repeat: repeat-x;" id="divMaintainTitle" runat="server">
        User Profile and Agent:&nbsp; <span id="spanTitle" runat="server"></span></div>

        <asp:FormView ID="fvUserProfile" runat="server" BackColor="White" 
            BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
            DataKeyNames="BadgeCode" DataSourceID="SqlDSUserProfile" GridLines="Both" 
            Font-Size="11pt" Width="500px" onitemupdating="fvUserProfile_ItemUpdating">
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <RowStyle ForeColor="#000066" />
            <EditItemTemplate>
            <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
                            Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Save"></asp:LinkButton>&nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                            Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Cancel"></asp:LinkButton>
                <table  style="border-right: #b6cade 2px solid; width:550px; border-top: #b6cade 2px solid; border-left: #b6cade 2px solid; color: #333333; border-bottom: #b6cade 2px solid; font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white; font-weight: normal;" 
                    cellpadding="1" cellspacing="1">
                    <tr>
                        <td style="width: 25%; color: #333333;  background-color: #b6cade;">BadgeCode:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                            <asp:Label ID="lblBadgeCode" runat="server" 
                                Text='<%# Eval("BadgeCode", "{0}") %>' /></td>
                        <td>&nbsp;</td>
                        <td style="width: 25%; color: #333333;  background-color: #b6cade;">Name:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                            <asp:Label ID="ChtNameLabel" runat="server" 
                                Text='<%# Eval("ChtName", "{0}") %>' /></td>
                    </tr>
                    <tr>
                        <td style="width: 25%; color: #333333;  background-color: #b6cade;">Corp:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                            <asp:Label ID="CorpLabel" runat="server" Text='<%# Eval("Corp", "{0}") %>' /></td>
                        <td>&nbsp;</td>
                        <td style="width: 25%; color: #333333;  background-color: #b6cade;">Place:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                            <asp:Label ID="BplaceLabel" runat="server" 
                                Text='<%# Eval("Bplace", "{0}") %>' /></td>
                    </tr>
                    <tr>
                        <td style="width: 25%; color: #333333;  background-color: #b6cade;">Dept:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                            <asp:Label ID="DeptLabel" runat="server" Text='<%# Eval("Dept", "{0}") %>' /></td>
                        <td>&nbsp;</td>
                        <td style="width: 25%; color: #333333;  background-color: #b6cade;">email:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                            <asp:Label ID="emailLabel" runat="server" Text='<%# Eval("email", "{0}") %>' /></td>
                    </tr>
                    <tr>
                        <td style="width: 25%; color: #333333;  background-color: #b6cade;">Corp Tel:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("CorpTel", "{0}") %>' /></td>
                        <td>&nbsp;</td>
                        <td style="width: 25%; color: #333333;  background-color: #b6cade;">PrivateTel:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("PrivateTel", "{0}") %>' /></td>
                    </tr>
                    <tr>
                        <td style="width: 25%; color: #333333;  background-color: #b6cade;">Description:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                            <asp:Label ID="Label3" runat="server" 
                                Text='<%# Eval("Description", "{0}") %>' /></td>
                        <td>&nbsp;</td>
                        <td style="width: 25%; color: #333333;  background-color: #b6cade;">Enable Agent:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8; text-align: center;">
                            <asp:CheckBox ID="chkAgent" runat="server" 
                                Checked='<%# Bind("EnableAgent2") %>' />
                        </td>
                    </tr>
                </table>
            </EditItemTemplate>
            <InsertItemTemplate>
                <table  style="border-right: #b6cade 2px solid; width:500px; border-top: #b6cade 2px solid; border-left: #b6cade 2px solid; color: #333333; border-bottom: #b6cade 2px solid; font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white" cellpadding="1" cellspacing="1">
                    <tr>
                        <td style="width: 20%; color: #333333;  background-color: #b6cade;">BadgeCode:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;"><asp:Label ID="BadgeCodeLabel" runat="server" Text='<%# Eval("BadgeCode") %>' /></td>
                        <td>&nbsp;</td><td style="width: 20%; color: #333333;  background-color: #b6cade;">Name:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;"><asp:Label ID="ChtNameLabel" runat="server" Text='<%# Bind("ChtName") %>' /></td>
                    </tr>
                    <tr>
                        <td style="width: 20%; color: #333333;  background-color: #b6cade;">Corp:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;"><asp:Label ID="CorpLabel" runat="server" Text='<%# Bind("Corp") %>' /></td>
                        <td>&nbsp;</td><td style="width: 20%; color: #333333;  background-color: #b6cade;">Place:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;"><asp:Label ID="BplaceLabel" runat="server" Text='<%# Bind("Bplace") %>' /></td>
                    </tr>
                    <tr>
                        <td style="width: 20%; color: #333333;  background-color: #b6cade;">Dept:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;"><asp:Label ID="DeptLabel" runat="server" Text='<%# Bind("Dept") %>' /></td>
                        <td>&nbsp;</td><td style="width: 20%; color: #333333;  background-color: #b6cade;">email:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;"><asp:Label ID="emailLabel" runat="server" Text='<%# Bind("email") %>' /></td>
                    </tr>
                    <tr>
                        <td style="width: 20%; color: #333333;  background-color: #b6cade;">Corp Tel:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;"><asp:Label ID="Label1" runat="server" Text='<%# Bind("CorpTel") %>' /></td>
                        <td>&nbsp;</td>
                        <td style="width: 20%; color: #333333;  background-color: #b6cade;">PrivateTel:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;"><asp:Label ID="Label2" runat="server" Text='<%# Bind("PrivateTel") %>' /></td>
                    </tr>
                    <tr>
                        <td style="width: 20%; color: #333333;  background-color: #b6cade;">Description:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;"><asp:Label ID="Label3" runat="server" Text='<%# Bind("Description") %>' /></td>
                        <td>&nbsp;</td><td style="width: 20%; color: #333333;  background-color: #b6cade;">Enable Agent:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                            <asp:CheckBox ID="chkAgent" runat="server" Checked='<%# bool.Parse(Eval("EnableAgent").ToString()) %>' 
                                Enabled="False" />
                        </td>
                    </tr>
                </table>
            </InsertItemTemplate>
            <ItemTemplate>
            <asp:LinkButton ID="lbtnUserRoleEdit" runat="server" CommandName="Edit" Font-Names="Arial Unicode MS"
                            Font-Size="10pt">Edit</asp:LinkButton>
                <table  style="border-right: #b6cade 2px solid; width:550px; border-top: #b6cade 2px solid; border-left: #b6cade 2px solid; color: #333333; border-bottom: #b6cade 2px solid; font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white" 
                    cellpadding="1" cellspacing="1">
                    <tr>
                        <td style="width: 20%; color: #333333;  background-color: #b6cade;">BadgeCode:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;"><asp:Label ID="BadgeCodeLabel" runat="server" Text='<%# Eval("BadgeCode") %>' /></td>
                        <td>&nbsp;</td><td style="width: 20%; color: #333333;  background-color: #b6cade;">Name:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;"><asp:Label ID="ChtNameLabel" runat="server" Text='<%# Bind("ChtName") %>' /></td>
                    </tr>
                    <tr>
                        <td style="width: 20%; color: #333333;  background-color: #b6cade;">Corp:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;"><asp:Label ID="CorpLabel" runat="server" Text='<%# Bind("Corp") %>' /></td>
                        <td>&nbsp;</td><td style="width: 20%; color: #333333;  background-color: #b6cade;">Place:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;"><asp:Label ID="BplaceLabel" runat="server" Text='<%# Bind("Bplace") %>' /></td>
                    </tr>
                    <tr>
                        <td style="width: 20%; color: #333333;  background-color: #b6cade;">Dept:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;"><asp:Label ID="DeptLabel" runat="server" Text='<%# Bind("Dept") %>' /></td>
                        <td>&nbsp;</td><td style="width: 20%; color: #333333;  background-color: #b6cade;">email:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;"><asp:Label ID="emailLabel" runat="server" Text='<%# Bind("email") %>' /></td>
                    </tr>
                    <tr>
                        <td style="width: 20%; color: #333333;  background-color: #b6cade;">Corp Tel:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;"><asp:Label ID="Label1" runat="server" Text='<%# Bind("CorpTel") %>' /></td>
                        <td>&nbsp;</td>
                        <td style="width: 20%; color: #333333;  background-color: #b6cade;">PrivateTel:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;"><asp:Label ID="Label2" runat="server" Text='<%# Bind("PrivateTel") %>' /></td>
                    </tr>
                    <tr>
                        <td style="width: 20%; color: #333333;  background-color: #b6cade;">Description:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;"><asp:Label ID="Label3" runat="server" Text='<%# Bind("Description") %>' /></td>
                        <td>&nbsp;</td><td style="width: 20%; color: #333333;  background-color: #b6cade;">Enable Agent:</td>
                        <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8; text-align: center;">
                            <asp:CheckBox ID="chkAgent" runat="server" Checked='<%# Bind("EnableAgent2") %>' 
                                Enabled="False" />
                        </td>
                    </tr>
                </table>
                
            </ItemTemplate>
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCFF66" Font-Bold="True" ForeColor="White" />
        </asp:FormView>
    
        <asp:SqlDataSource ID="SqlDSUserProfile" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
            
            SelectCommand="SELECT [BadgeCode], [ChtName], [Corp], [Bplace], [Dept], [email], [CorpTel], [PrivateTel], [Description], [EnableAgent2] FROM [m_userprofile] WHERE ([BadgeCode] = @BadgeCode)">
            <SelectParameters>
                <asp:ControlParameter ControlID="hfUserId" Name="BadgeCode" 
                    PropertyName="Value" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
    
        <asp:GridView ID="gvAgent" runat="server" AutoGenerateColumns="False" 
            BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" 
            CellPadding="2" DataKeyNames="ID" DataSourceID="SqlDSAgent" Font-Size="10pt" 
            ForeColor="Black" ShowFooter="True" onrowupdating="gvAgent_RowUpdating" 
            onrowdeleting="gvAgent_RowDeleting">
            <FooterStyle BackColor="Tan" />
            <Columns>
                <asp:CommandField CancelText="Cancel" EditText="Edit" ShowEditButton="True" 
                    UpdateText="Save" />
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                            CommandName="Delete" Text="Del"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="AgentCode" SortExpression="agent">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("agent") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("agent") %>'></asp:Label>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:LinkButton ID="lbtnAdd" runat="server" onclick="lbtnAdd_Click">Add</asp:LinkButton>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="AgentName" HeaderText="Name" ReadOnly="True" 
                    SortExpression="AgentName" />
                <asp:BoundField DataField="AgentEmail" HeaderText="email" ReadOnly="True" 
                    SortExpression="AgentEmail" />
                <asp:CheckBoxField DataField="IsInUse" HeaderText="IsInUse" 
                    SortExpression="IsInUse" >
                <ItemStyle HorizontalAlign="Center" />
                </asp:CheckBoxField>
                <asp:BoundField DataField="editorName" HeaderText="editor" ReadOnly="True" 
                    SortExpression="editorName" />
                <asp:BoundField DataField="cdt" HeaderText="Create Date" ReadOnly="True" 
                    SortExpression="cdt" />
                <asp:BoundField DataField="udt" HeaderText="Update Date" ReadOnly="True" 
                    SortExpression="udt" />
            </Columns>
            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                HorizontalAlign="Center" />
            <EmptyDataTemplate>
                <asp:LinkButton ID="lbtnAdd" runat="server" onclick="lbtnAdd_Click">Add Agent</asp:LinkButton>
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
            <HeaderStyle BackColor="Tan" Font-Bold="True" />
            <AlternatingRowStyle BackColor="PaleGoldenrod" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDSAgentAdd" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
            SelectCommand="SELECT [agent], [AgentName], [AgentEmail], [IsInUse] FROM [v_UserAgent] WHERE ([agent] = @agent)">
            <SelectParameters>
                <asp:ControlParameter ControlID="gvAgent" Name="agent" 
                    PropertyName="SelectedValue" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:FormView ID="fvAgent" runat="server" 
                    DataSourceID="SqlDSAgentAdd"  Width="35%" OnItemInserting="fvAgent_ItemInserting" BorderColor="#FFE0C0" BorderStyle="Solid" 
                    BorderWidth="2px">
                    <EditItemTemplate>
                        <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
                            Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Save"></asp:LinkButton>
                        <asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                            Font-Names="Arial Unicode MS" Font-Size="10pt" Text="Cancel"></asp:LinkButton>
                        <table border="0" cellpadding="2" 
                            cellspacing="1" style="font-size: 10pt; line-height: 10pt;
                    font-family: 'Arial Unicode MS'" width="100%">
                            <tr>
                                <td class="FVTDEDITCol1" style="width:80px;">
                                    InUse</td>
                                <td class="FVTDEDITCol2">
                                    <asp:CheckBox ID="chkUserIsInUse" runat="server" 
                                        Checked='<%# Bind("IsInUse") %>' /></td>
                            </tr>
                            <tr>
                                <td class="FVTDEDITCol1">
                                   Agent Code</td>
                                <td class="FVTDEDITCol2">
                                    <asp:Label ID="lblAgentCode" runat="server" 
                                        Text='<%# Bind("agent", "{0}") %>'></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="FVTDEDITCol1">
                                    Agent Name</td>
                                <td class="FVTDEDITCol2">
                                    <asp:TextBox ID="txtChtName" runat="server" Text='<%# Bind("ChtName") %>'></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="FVTDEDITCol1">
                                    eMail</td>
                                <td class="FVTDEDITCol2">
                                    <asp:TextBox ID="txtemail" runat="server" Text='<%# Bind("email", "{0}") %>'></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="FVTDEDITCol1">
                                    editor</td>
                                <td class="FVTDEDITCol2">
                                    <asp:Label ID="lbleditor" runat="server" 
                                        Text='<%# Eval("editorName", "{0}") %>'></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="FVTDEDITCol1">
                                    Create Date</td>
                                <td class="FVTDEDITCol2">
                                    <asp:Label ID="lblcdt" runat="server" Text='<%# Eval("cdt", "{0:g}") %>'></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="FVTDEDITCol1">
                                    Update Date</td>
                                <td class="FVTDEDITCol2">
                                    <asp:Label ID="lbludt" runat="server" Text='<%# Eval("udt", "{0:g}") %>'></asp:Label></td>
                            </tr>
                        </table>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert"
                             Text="Save"></asp:LinkButton>
                        <asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                             Text="Cancel"></asp:LinkButton>
                        <table border="0" 
                            cellpadding="2" cellspacing="1" style="padding-right: 0px; padding-left: 0px;
                    font-size: 10pt; padding-bottom: 0px; margin: 0px; border-top-style: none; padding-top: 0px;
                    font-family: 'Arial Unicode MS'; border-right-style: none; border-left-style: none;
                    border-bottom-style: none" width="100%">
                            <tr>
                                <td class="FVTDINSCol1" style="width:80px;">
                                    InUse</td>
                                <td class="FVTDINSCol2">
                                    <asp:CheckBox ID="chkUserIsInUse0" runat="server" 
                                        Checked='<%# Bind("IsInUse") %>' /></td>
                            </tr>
                            <tr>
                                <td class="FVTDINSCol1">
                                    Agent Code</td>
                                <td class="FVTDINSCol2">
                                    <asp:TextBox ID="txtBadgeCode" runat="server" Columns="9" 
                                        Text='<%# Bind("agent", "{0}") %>'></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="FVTDINSCol1">
                                    Agent Name</td>
                                <td class="FVTDINSCol2">
                                    <asp:TextBox ID="txtChtName0" runat="server" 
                                        Text='<%# Bind("ChtName", "{0}") %>'></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="FVTDINSCol1">
                                    eMail</td>
                                <td class="FVTDINSCol2">
                                    <asp:TextBox ID="txtemail0" runat="server" Text='<%# Bind("email", "{0}") %>'></asp:TextBox></td>
                            </tr>
                        </table>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnUserRoleNew" runat="server" CommandName="New" Font-Names="Arial Unicode MS"
                            Font-Size="10pt">Add</asp:LinkButton>
                        <asp:LinkButton ID="lbtnUserRoleEdit" runat="server" CommandName="Edit" Font-Names="Arial Unicode MS"
                            Font-Size="10pt">Edit</asp:LinkButton>
                        <table cellpadding="2" cellspacing="1" style="padding-right: 0px; padding-left: 0px;
                    font-size: 10pt; padding-bottom: 0px; margin: 0px; border-top-style: none; padding-top: 0px;
                    font-family: 'Arial Unicode MS'; border-right-style: none; border-left-style: none;
                    border-bottom-style: none" width="100%">
                            <tr>
                                <td class="FVTDITCol1" style="width:80px;">
                                    InUse</td>
                                <td class="FVTDITCol2">
                                    <asp:CheckBox ID="chkUserIsInUse1" runat="server" Checked='<%# Bind("IsInUse") %>'
                                Enabled="False" /></td>
                            </tr>
                            <tr>
                                <td class="FVTDITCol1">
                                    Agent Code</td>
                                <td class="FVTDITCol2">
                                    <asp:Label ID="lblBadgeCode0" runat="server" Text='<%# Bind("agent") %>'></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="FVTDITCol1">
                                    User Name</td>
                                <td class="FVTDITCol2">
                                    <asp:Label ID="lblChtName" runat="server" Text='<%# Bind("ChtName") %>'></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="FVTDITCol1" style="height: 18px">
                                    eMail</td>
                                <td class="FVTDITCol2" style="height: 18px">
                                    <asp:Label ID="lblemail" runat="server" Text='<%# Bind("email", "{0}") %>'></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="FVTDITCol1">
                                    Create Date</td>
                                <td class="FVTDITCol2">
                                    <asp:Label ID="lblcdt0" runat="server" Text='<%# Bind("cdt") %>'></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="FVTDITCol1">
                                    Update Date</td>
                                <td class="FVTDITCol2">
                                    <asp:Label ID="lbludt0" runat="server" Text='<%# Eval("udt", "{0:g}") %>'></asp:Label></td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:FormView>
        <asp:SqlDataSource ID="SqlDSAgent" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
            SelectCommand="SELECT * FROM [v_UserAgent] WHERE ([BadgeCode] = @BadgeCode)">
            <SelectParameters>
                <asp:ControlParameter ControlID="hfUserId" Name="BadgeCode" 
                    PropertyName="Value" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
    
        <asp:HiddenField ID="hfUserId" runat="server" />
    
    </div>
</asp:Content>