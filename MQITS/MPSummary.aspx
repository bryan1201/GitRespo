<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MPSummary.aspx.cs" Inherits="MPSummary" Title="=== MQITS::MPSummary ===" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">

    <script language="javascript" type="text/javascript">
	function SPM_OpenCalendar(idname, postBack)
	{
 	popUp = window.open('Calendar.aspx?formname=' + document.forms[0].name + 
		'&id=' + idname + '&selected=' + document.forms[0].elements(idname).value + '&postBack=' + postBack, 
		'popupcal', 'width=235,height=260, left=200,top=180');
	}
	function SPM_SetDate(formName, id, newDate, postBack)
	{
		eval('var theform = document.' + formName + ';');
		popUp.close();
		theform.elements(id).value = newDate;
	}
 </script>
     <table style="text-align:left; border-right: #b6cade 2px solid; border-top: #b6cade 2px solid; border-left: #b6cade 2px solid; border-bottom: #b6cade 2px solid; width: 100%; float: none; clear: both; font-size: 10pt; color: #333333; font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white;" 
             cellpadding="1" cellspacing="1">
        <tr>
            <td style="background-color: #b6cade; width: 15%;">Site</td>
            <td style="background-color: #eff3f8; width: 20%;">
                <asp:DropDownList ID="ddlSite" runat="server" Width="100%" DataSourceID="SqlDSSite" DataTextField="SiteName" DataValueField="SiteID" ></asp:DropDownList>
                <asp:SqlDataSource ID="SqlDSSite" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="/*&#13;&#10;select DISTINCT g.GroupID as SiteID,g.Title as SiteName&#13;&#10;from m_Group g INNER JOIN m_Group m&#13;&#10;on g.GroupID=dbo.GetXml(m.Remark,'SITE')&#13;&#10;WHERE g.Parent_GroupID='40' AND m.GroupID=ProjectID&#13;&#10;*/&#13;&#10;&#13;&#10;SELECT [SiteID], [SiteName] FROM [v_Site] "></asp:SqlDataSource>
                    </td>
            <td style="background-color: #b6cade; width: 15%;">Customer</td>
            <td style="background-color: #eff3f8; width: 20%;">
                 <asp:DropDownList ID="ddlCustomer" runat="server" Width="100%" DataSourceID="SqlDSCustomer" DataTextField="CustomerName" DataValueField="CustomerID" AutoPostBack="True"></asp:DropDownList>
                 <asp:SqlDataSource ID="SqlDSCustomer" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT [CustomerID], [CustomerName] FROM [v_Customer]"></asp:SqlDataSource>
                 </td>            
            <td rowspan="4" style="background-color: #eff3f8; vertical-align: middle; text-align: center; width: 10%;">
                <asp:Button ID="btnQry" runat="server" Text="Query" OnClick="btnQry_Click" Width="64px" Font-Bold="True" Font-Names="Arial" Font-Size="12pt" ForeColor="#C04000"  /></td>              
        </tr>
        <tr>
            <td style=" background-color: #b6cade; width: 15%;">Status</td>
            <td style="background-color: #eff3f8; width: 20%;">
                <asp:DropDownList ID="ddlStatus" runat="server" Width="100%" DataSourceID="SqlDSStatus" DataTextField="TEXT" DataValueField="VALUE" ></asp:DropDownList>
                <asp:SqlDataSource ID="SqlDSStatus" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT ''as VALUE ,'-' as TEXT&#13;&#10;UNION&#13;&#10;SELECT VALUE, TEXT&#13;&#10;FROM v_IssueStatus"></asp:SqlDataSource>
                </td>
            <td style="background-color: #b6cade; width: 15%;"></td>
            <td style="background-color: #eff3f8; width: 20%;"></td>
        </tr>
         <tr>
             <td style="width: 15%; background-color: #b6cade">
                 Production Duration</td>
             <td style="width: 20%; background-color: #eff3f8">
                 From<asp:TextBox ID="txtStart" runat="server"></asp:TextBox>
                <img id="Img1" runat="server" alt="選取日期" onclick="SPM_OpenCalendar(document.all('ctl00$CPH_MQITS$txtStart').name, false)"
                          src="images/calendar.gif" /></td>
             <td colspan="2" style="background-color: #eff3f8">
                 To<asp:TextBox ID="txtEnd" runat="server"></asp:TextBox>
                <img id="Img2" runat="server" alt="選取日期" onclick="SPM_OpenCalendar(document.all('ctl00$CPH_MQITS$txtEnd').name, false)"
                          src="images/calendar.gif" /></td>
         </tr>
         <tr>
             <td style="width: 15%; background-color: #b6cade" valign="top">Project</td>
             <td style="background-color: #eff3f8" colspan="3">
                <table>
                    <tr>
                        <td style="width: 190px;">
                            <asp:ListBox ID="lbtoleft" runat="server" Height="110px" SelectionMode="Multiple"
                                Width="200px" DataSourceID="SqlDSProject" DataTextField="ProjectName" DataValueField="ProjectID"></asp:ListBox><asp:SqlDataSource ID="SqlDSProject" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>"
                                SelectCommand="select GroupID as ProjectID,Title as ProjectName &#13;&#10;FROM m_Group&#13;&#10;WHERE GroupType='5' AND Parent_GroupID='60' &#13;&#10;AND IsMP=1 AND IsEnable=1 AND IsEOSL=0&#13;&#10;AND dbo.GetXml(Remark,'CUSTOMER')=@Customer&#13;&#10;ORDER BY ProjectName">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddlCustomer" Name="Customer" PropertyName="SelectedValue" />
                                    </SelectParameters>
                            </asp:SqlDataSource>
                        </td>
                        <td style="width: 50px;">
                            &nbsp;
                            <asp:Button ID="btnright" runat="server" Text="-->" OnClick="btnright_Click" />
                            <br />
                            <br />
                            &nbsp;
                            <asp:Button ID="btnleft" runat="server" Text="<--" OnClick="btnleft_Click" /></td>
                        <td style="width: 150px;">
                            <asp:ListBox ID="lbtoright" runat="server" Height="110px" SelectionMode="Multiple"
                                Width="200px"></asp:ListBox></td>
                    </tr>
                </table>
             </td>
         </tr>
       
     </table>
    <br />
     <asp:GridView ID="gvPCA" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" Caption="PCA" Width="100%">
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <Columns>
          <asp:BoundField DataField="Project" HeaderText="Project">
                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="8pt" />
          </asp:BoundField>
          <asp:BoundField DataField="Material" HeaderText="PCA PN">
                 <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                 <ItemStyle Font-Size="8pt" />
          </asp:BoundField>
          <asp:TemplateField HeaderText="All">
                <ItemTemplate>
                    <asp:HyperLink ID="lblAll" runat="server" Target="_blank" Text='<%# Bind("All")%>' NavigateUrl='<%# Eval("AllURL") +"&Status=" + ddlStatus.SelectedValue.ToString() %>'></asp:HyperLink>
                </ItemTemplate>
                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="8pt" />
          </asp:TemplateField>
          <asp:TemplateField HeaderText="ICT">
                <ItemTemplate>
                    <asp:HyperLink ID="lblICT" runat="server" Target="_blank" Text='<%# Bind("ICT")%>' NavigateUrl='<%# Eval("ICTURL") +"&Status=" + ddlStatus.SelectedValue.ToString()  %>' ></asp:HyperLink>
                </ItemTemplate>
                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="8pt" />
            </asp:TemplateField>
              <asp:TemplateField HeaderText="SA">
                <ItemTemplate>
                    <asp:HyperLink ID="lblSA" runat="server" Target="_blank" Text='<%# Bind("SA")%>'  NavigateUrl='<%# Eval("SAURL") +"&Status=" + ddlStatus.SelectedValue.ToString()  %>'></asp:HyperLink>
                </ItemTemplate>
                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="8pt" />
            </asp:TemplateField>
              <asp:TemplateField HeaderText="AOI S1">
                <ItemTemplate>
                    <asp:HyperLink ID="lblAOIS1" runat="server" Target="_blank" Text='<%# Bind("AOIS1")%>' NavigateUrl='<%# Eval("AOIS1URL") +"&Status=" + ddlStatus.SelectedValue.ToString()  %>'></asp:HyperLink>
                </ItemTemplate>
                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="8pt" />
            </asp:TemplateField>
              <asp:TemplateField HeaderText="AOI S2">
                <ItemTemplate>
                    <asp:HyperLink ID="lblAOIS2" runat="server" Target="_blank" Text='<%# Bind("AOIS2")%>' NavigateUrl='<%# Eval("AOIS2URL") +"&Status=" + ddlStatus.SelectedValue.ToString()  %>'></asp:HyperLink>
                </ItemTemplate>
                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="8pt" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="EOLI">
                <ItemTemplate>
                    <asp:HyperLink ID="lblEOLI" runat="server" Target="_blank" Text='<%# Bind("EOLI")%>' NavigateUrl='<%# Eval("EOLIURL") +"&Status=" + ddlStatus.SelectedValue.ToString()  %>'></asp:HyperLink>
                </ItemTemplate>
                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="8pt" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="SMT X-ray">
                <ItemTemplate>
                    <asp:HyperLink ID="lblSMTXray" runat="server" Target="_blank" Text='<%# Bind("SMTXray")%>' NavigateUrl='<%# Eval("SMTXrayURL") +"&Status=" + ddlStatus.SelectedValue.ToString()  %>'></asp:HyperLink>
                </ItemTemplate>
                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="8pt" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="W/S X-ray">
                <ItemTemplate>
                    <asp:HyperLink ID="lblWSXray" runat="server" Target="_blank" Text='<%# Bind("WSXray")%>' NavigateUrl='<%# Eval("WSXrayURL") +"&Status=" + ddlStatus.SelectedValue.ToString()  %>'></asp:HyperLink>
                </ItemTemplate>
                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="8pt" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="PQC">
                <ItemTemplate>
                    <asp:HyperLink ID="lblPQC" runat="server" Target="_blank" Text='<%# Bind("PQC")%>' NavigateUrl='<%# Eval("PQCURL") +"&Status=" + ddlStatus.SelectedValue.ToString()  %>'></asp:HyperLink>
                </ItemTemplate>
                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="8pt" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Others">
                <ItemTemplate>
                    <asp:HyperLink ID="lblOthers" runat="server" Target="_blank" Text='<%# Bind("Others")%>' NavigateUrl='<%# Eval("OthersURL") +"&Status=" + ddlStatus.SelectedValue.ToString() %>'></asp:HyperLink>
                </ItemTemplate>
                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="8pt" />
            </asp:TemplateField>
             <asp:BoundField DataField="TAT" HeaderText="TAT(%)">
                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="8pt" />
          </asp:BoundField>
        </Columns>
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>
    <br />
    
    <asp:GridView ID="gvCPU" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" Caption="CPU" Width="100%">
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <Columns>
          <asp:BoundField DataField="Project" HeaderText="Project">
                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="8pt" />
          </asp:BoundField>
          <asp:BoundField DataField="Material" HeaderText="CPU SKU">
                 <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                 <ItemStyle Font-Size="8pt" />
          </asp:BoundField>
          <asp:TemplateField HeaderText="All">
                <ItemTemplate>
                    <asp:HyperLink ID="lblAll" runat="server" Target="_blank" Text='<%# Bind("All")%>' NavigateUrl='<%# Eval("AllURL") +"&Status=" + ddlStatus.SelectedValue.ToString() %>'></asp:HyperLink>
                </ItemTemplate>
                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="8pt" />
          </asp:TemplateField>
          <asp:TemplateField HeaderText="Pre-Test">
                <ItemTemplate>
                    <asp:HyperLink ID="lblPreTest" runat="server" Target="_blank" Text='<%# Bind("PreTest")%>' NavigateUrl='<%# Eval("PreTestURL") +"&Status=" + ddlStatus.SelectedValue.ToString() %>'></asp:HyperLink>
                </ItemTemplate>
                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="8pt" />
            </asp:TemplateField>
              <asp:TemplateField HeaderText="Run-in">
                <ItemTemplate>
                    <asp:HyperLink ID="lblRunin" runat="server" Target="_blank" Text='<%# Bind("Runin")%>'  NavigateUrl='<%# Eval("RuninURL") +"&Status=" + ddlStatus.SelectedValue.ToString() %>'></asp:HyperLink>
                </ItemTemplate>
                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="8pt" />
            </asp:TemplateField>
              <asp:TemplateField HeaderText="Aft-Test">
                <ItemTemplate>
                    <asp:HyperLink ID="lblAftTest" runat="server" Target="_blank" Text='<%# Bind("AftTest")%>'  NavigateUrl='<%# Eval("AftTestURL") +"&Status=" + ddlStatus.SelectedValue.ToString() %>'></asp:HyperLink>
                </ItemTemplate>
                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="8pt" />
            </asp:TemplateField>
              <asp:TemplateField HeaderText="PIA">
                <ItemTemplate>
                    <asp:HyperLink ID="lblPIA" runat="server" Target="_blank" Text='<%# Bind("PIA")%>'  NavigateUrl='<%# Eval("PIAURL") +"&Status=" + ddlStatus.SelectedValue.ToString() %>'></asp:HyperLink>
                </ItemTemplate>
                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="8pt" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="EPIA">
                <ItemTemplate>
                    <asp:HyperLink ID="lblEPIA" runat="server" Target="_blank" Text='<%# Bind("EPIA")%>'  NavigateUrl='<%# Eval("EPIAURL") +"&Status=" + ddlStatus.SelectedValue.ToString() %>'></asp:HyperLink>
                </ItemTemplate>
                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="8pt" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Others">
                <ItemTemplate>
                    <asp:HyperLink ID="lblOthers" runat="server" Target="_blank" Text='<%# Bind("Others")%>'  NavigateUrl='<%# Eval("OthersURL") +"&Status=" + ddlStatus.SelectedValue.ToString() %>'></asp:HyperLink>
                </ItemTemplate>
                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="8pt" />
            </asp:TemplateField>
            <asp:BoundField DataField="TAT" HeaderText="TAT(%)">
                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                <ItemStyle Font-Size="8pt" />
            </asp:BoundField>
        </Columns>
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>
    
    

</asp:Content>

