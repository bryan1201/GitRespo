﻿<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MPQuery.aspx.cs" Inherits="MPQuery" MaintainScrollPositionOnPostback="true" %>
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
     <table style="font-size: 10pt; width: 980px; font-family: 'Arial Unicode MS'; border-top-style: groove; border-right-style: groove; border-left-style: groove; border-bottom-style: groove;text-align: left;">
            <tr>
                <td style="background-color: #ccccff; width: 125px;" >
                    <asp:Label ID="Label1" runat="server" Text="Module"></asp:Label></td>
                <td style="width: 17%">
                    <asp:DropDownList ID="ddlModel" runat="server" Width="100%" DataSourceID="SqlDSModule" AutoPostBack="True" DataTextField="ModuleName" DataValueField="ModuleID">
                    </asp:DropDownList><asp:SqlDataSource ID="SqlDSModule" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT '' as ModuleID,'-' as ModuleName&#13;&#10;UNION&#13;&#10;SELECT CONVERT(NVARCHAR,ModuleID) as ModuleID,ModuleName &#13;&#10;FROM v_Module"></asp:SqlDataSource>
                </td>
                <td style="background-color: #ccccff; width: 80px;" >
                    <asp:Label ID="Label3" runat="server" Text="Customer"></asp:Label></td>
                <td style="width: 17%">
                    <asp:DropDownList ID="ddlCustomer" runat="server" Width="100%" DataSourceID="SqlDSCustomer" AutoPostBack="True" DataTextField="CustomerName" DataValueField="CustomerID">
                    </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDSCustomer" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT '' as CustomerID,'-' as CustomerName&#13;&#10;UNION&#13;&#10;SELECT  CONVERT(NVARCHAR,CustomerID) as CustomerID,CustomerName &#13;&#10;FROM v_Customer"></asp:SqlDataSource>
                </td>
                <td style="background-color: #ccccff; width: 100px;" >
                    <asp:Label ID="Label8" runat="server" Text="Site"></asp:Label></td>
                <td style="width: 18%">
                    <asp:DropDownList ID="ddlSite" runat="server" Width="100%" AutoPostBack="True" DataSourceID="SqlDSSite" DataTextField="SiteName" DataValueField="SiteID">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDSSite" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT '' as SiteID,'-' as SiteName&#13;&#10;UNION&#13;&#10;Select CONVERT(NVARCHAR,GroupID) as SiteID ,Title&#13;&#10;FROM m_Group&#13;&#10;WHERE Parent_GroupID='40'"></asp:SqlDataSource>
                </td>
            </tr>
             <tr>
                 <td style="background-color: #ccccff; width: 125px;" >
                    <asp:Label ID="Label2" runat="server" Text="Project"></asp:Label></td>
                 <td style="width:17%">
                    <asp:DropDownList ID="ddlProject" runat="server" Width="100%" DataSourceID="SqlDSProject" AutoPostBack="True" DataTextField="ProjectName" DataValueField="ProjectID">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDSProject" runat="server" 
                         ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="IF @Module='301'
BEGIN
	SELECT '' as ProjectID ,'-' as ProjectName
	UNION
	SELECT CONVERT(NVARCHAR,ProjectID) as ProjectID, ProjectName
	FROM v_ProjectPhase 
	WHERE  CustomerID=@CustomerID  AND SiteID=@SiteID AND IsInUse=1
	ORDER BY ProjectName
END
ELSE
BEGIN
	SELECT '' as ProjectID ,'-' as ProjectName 
	UNION
	select CONVERT(NVARCHAR,GroupID) as ProjectID,Title as ProjectName 
	FROM m_Group
	WHERE GroupType='5' AND Parent_GroupID='60' 
	AND IsMP=1 AND IsEnable=1 AND IsEOSL=0
	AND dbo.GetXml(Remark,'CUSTOMER')=@CustomerID
                AND dbo.GetXml(Remark,'Site') IN ( 
	SELECT [SiteID] FROM [v_Site] WHERE [IsEnable] = 1 AND [IsVirtual] = 0 )
	ORDER BY ProjectName
END">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlModel" Name="Module" 
                                PropertyName="SelectedValue" />
                            <asp:ControlParameter ControlID="ddlCustomer" Name="CustomerID" PropertyName="SelectedValue" />
                            <asp:ControlParameter ControlID="ddlSite" Name="SiteID" PropertyName="SelectedValue" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                 </td>
                 <td style="background-color: #ccccff; width: 80px;" >
                     <asp:Label ID="Label9" runat="server" Text="PCA P/N or CPU"></asp:Label></td>
                 <td style="width:17%">
                    <asp:SqlDataSource ID="SqlDSPhase" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="/*SELECT '' as PhaseID,'-' as PhaseName&#13;&#10;UNION&#13;&#10;SELECT CONVERT(NVARCHAR),GroupID) as PhaseID,Title as PhaseName&#13;&#10;FROM m_Group &#13;&#10;WHERE GroupType='4' AND Parent_GroupID=ProjectID*/&#13;&#10;&#13;&#10;SELECT ''as PhaseID ,'-' as PhaseName, '' as Rank&#13;&#10;UNION&#13;&#10;SELECT CONVERT(NVARCHAR,PhaseID) as PhaseID,PhaseName,Rank  FROM fn_GetProjectPhase(@Project,@Site)&#13;&#10;order by Rank">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlProject" Name="Project" PropertyName="SelectedValue" />
                            <asp:ControlParameter ControlID="ddlSite" Name="Site" PropertyName="SelectedValue" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:DropDownList ID="ddlMaterial" runat="server" Width="100%" DataSourceID="SqlDSMaterial" DataTextField="TEXT" DataValueField="VALUE" AutoPostBack="True">
                    </asp:DropDownList></td>
                 <td style="background-color: #ccccff; width: 100px;" >
                    <asp:Label ID="Label13" runat="server" Text="Station"></asp:Label></td>
                 <td style="width: 18%">
                    <asp:SqlDataSource ID="SqlDSMaterial" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="/*SELECT '' as  VALUE ,'-' as TEXT&#13;&#10;UNION&#13;&#10;SELECT * FROM fn_GetMTypeList (ProjectID)*/&#13;&#10;SELECT * FROM fn_GetProjectMaterialList (@ProjectID)">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlProject" Name="ProjectID" PropertyName="SelectedValue" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:DropDownList ID="ddlStation" runat="server" Width="100%" DataSourceID="SqlDSStation" DataTextField="TEXT" DataValueField="VALUE">
                    </asp:DropDownList></td>
             </tr>
            <tr>
                <td style="background-color: #ccccff; width: 125px;">
                    <asp:Label ID="Label5" runat="server" Text="Issue Owner"></asp:Label></td>
                <td style="width:17%; height: 24px;">
                    <asp:SqlDataSource ID="SqlDSStation" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="IF @Module!='303'
BEGIN
        SELECT  '' as VALUE , '-' as TEXT
        UNION
        SELECT * FROM fn_GetStationList(@CustomerID,@ProjectID,@Material)
END
ELSE
BEGIN
       SELECT  '' as VALUE , '-' as TEXT
       UNION
       SELECT CONVERT(NVARCHAR(50),StationID),StationName 
       FROM v_RMAStation
END">
                        <SelectParameters>
<asp:ControlParameter ControlID="ddlModel" PropertyName="SelectedValue" Name="Module"></asp:ControlParameter>
                            <asp:ControlParameter ControlID="ddlCustomer" Name="CustomerID" 
                                PropertyName="SelectedValue" DefaultValue="0" />
                            <asp:ControlParameter ControlID="ddlProject" Name="ProjectID" 
                                PropertyName="SelectedValue" DefaultValue="0" />
                            <asp:ControlParameter ControlID="ddlMaterial" Name="Material" 
                                PropertyName="SelectedValue" DefaultValue="0" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:DropDownList ID="ddlIssueOwner" runat="server" Width="100%" DataSourceID="SqlDSIssueOwner" DataTextField="OwnerName" DataValueField="OwnerCode">
                </asp:DropDownList></td>
                <td style="background-color: #ccccff; width: 80px;">
                    <asp:Label ID="Label7" runat="server" Text="Liability"></asp:Label></td>
                <td style="width: 17%; height: 24px;">
                <asp:SqlDataSource ID="SqlDSIssueOwner" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT ''as OwnerCode,'-' as OwnerName &#13;&#10;UNION&#13;&#10;SELECT DISTINCT IssueOwner,dbo.fn_GetChtName(IssueOwner)&#13;&#10;FROM f_common&#13;&#10;WHERE Module=@ModuleID AND Site=@SiteID  AND ISNULL(IssueOwner,'')!=''&#13;&#10;ORDER BY OwnerName&#13;&#10;">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlModel" Name="ModuleID" PropertyName="SelectedValue" />
                        <asp:ControlParameter ControlID="ddlSite" Name="SiteID" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
                    <asp:DropDownList ID="ddlLiability" runat="server" Width="100%" 
                        DataSourceID="SqlDSLiability" DataTextField="LiabilityName" 
                        DataValueField="LiabilityID">
                    </asp:DropDownList></td>
                <td style="background-color: #ccccff; width: 100px;">
                    <asp:Label ID="Label12" runat="server" Text="Status"></asp:Label></td>
                <td style="width: 18%; height: 24px;">
                    <asp:SqlDataSource ID="SqlDSLiability" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="IF @Module='303'
BEGIN
       SELECT '' as LiabilityID,'-' as LiabilityName
       UNION
       select CONVERT(NVARCHAR(50),LiabilityID),LiabilityName
       from v_RMALiability
END
ELSE
BEGIN
      SELECT '' as LiabilityID,'-' as LiabilityName
      UNION
      select CONVERT(NVARCHAR(50),VALUE),TEXT
      from v_IssueLiability
END">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlModel" Name="Module" 
                                PropertyName="SelectedValue" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:DropDownList ID="ddlStatus" runat="server" Width="100%" DataSourceID="SqlDSStatus" DataTextField="TEXT" DataValueField="VALUE">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="background-color: #ccccff; width: 125px;">
                    <asp:Label ID="Label6" runat="server" Text="Priority"></asp:Label></td>
                <td style="width: 17%;">
                    <asp:SqlDataSource ID="SqlDSStatus" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT '' as [VALUE],'-' as [TEXT] UNION SELECT [VALUE],[TEXT] FROM v_IssueStatus"></asp:SqlDataSource>
                    <asp:DropDownList ID="ddlPriority" runat="server" Width="100%" DataSourceID="SqlDSPriority" DataTextField="TEXT" DataValueField="VALUE">
                    </asp:DropDownList></td>
                <td style="background-color: #ccccff; width: 80px;">Repeat Defect?</td>
                <td style="width: 17%;">
                    <asp:SqlDataSource ID="SqlDSPriority" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT '' as [VALUE],'-' as [TEXT] UNION SELECT [VALUE],[TEXT] FROM v_IssuePriority"></asp:SqlDataSource>
                    <asp:DropDownList ID="ddlRepeat" runat="server" Width="100%">
                        <asp:ListItem Value=" ">-</asp:ListItem>
                        <asp:ListItem Value="Yes">Yes</asp:ListItem>
                        <asp:ListItem Value="No">No</asp:ListItem>
                    </asp:DropDownList></td>
                <td style="background-color: #ccccff; width: 100px;">
                    </td>
               <td style="width: 17%;">
                    </td>
            </tr>
              <tr>
                  <td style="width: 125px; background-color: #ccccff">
                      Production Duration</td>
                  <td colspan="5">From
                      <asp:TextBox ID="txtStart" runat="server"></asp:TextBox> 
                      <img id="Img1" runat="server" alt="選取日期" onclick="SPM_OpenCalendar(document.all('ctl00$CPH_MQITS$txtStart').name, false)"
                          src="images/calendar.gif" />TO
                      <asp:TextBox ID="txtEnd" runat="server"></asp:TextBox>
                      <img id="Img2" runat="server" alt="選取日期" onclick="SPM_OpenCalendar(document.all('ctl00$CPH_MQITS$txtEnd').name, false)"
                          src="images/calendar.gif" /></td>
              </tr>
              <tr>
                  <td style="width: 125px; background-color: #ccccff">
                      Location(For PCA)</td>
                  <td colspan="5">
                    <asp:TextBox ID="txtLocation" runat="server" Width="100%" TextMode="MultiLine"></asp:TextBox></td>
              </tr>
         <tr>
             <td style="width: 125px; background-color: #ccccff">
                 Faulty Commodity<br />
                 (For CPU)</td>
             <td colspan="5">
                 <asp:TextBox ID="txtFaultyCommodity" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox></td>
         </tr>
            <tr>
                <td style="background-color: #ccccff; width: 125px;">
                    <asp:Label ID="Label11" runat="server" Text="Defective Component P/N" Width="153px"></asp:Label><br />
                    (For PCA)</td>
                <td colspan="5">
                    <asp:TextBox ID="txtDefectComponent" runat="server" Width="100%" TextMode="MultiLine"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="background-color: #ccccff; width: 125px;" >
                    Defect Symptom<br />
                    (For PCA/CPU)</td>
                <td colspan="5">
                    <asp:TextBox ID="txtDefectSymptom" runat="server" Width="100%" TextMode="MultiLine"></asp:TextBox></td>
            </tr>
             <tr>
                 <td style="background-color: #ccccff; width: 125px;" >
                     <asp:Label ID="Label10" runat="server" Text="Faulty Commodity P/N" Width="140px"></asp:Label><br />
                     (For CPU)</td>
                 <td colspan="5">
                     <asp:TextBox ID="txtFaultyCommodityPN" runat="server" Width="100%" TextMode="MultiLine"></asp:TextBox></td>
             </tr>
            <tr>
                <td colspan="6" style="text-align:center; width:100%;">
                    <asp:Button ID="btnQuery" runat="server" Text="Query" Font-Bold="True" Font-Names="Century Gothic" Width="100px" OnClick="btnQuery_Click" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnExport" runat="server" Text="Export"  Font-Bold="True" Font-Names="Century Gothic" Width="100px" OnClick="btnExport_Click" /></td>
            </tr>
        </table>
        <asp:Label ID="lblIssueID" runat="server" Visible="False" ></asp:Label>
        <asp:TextBox ID="txtUserID" runat="server" Visible="False"></asp:TextBox><br />
        <asp:GridView ID="gvPCADetail" runat="server" CellPadding="2" ForeColor="#333333" 
            GridLines="Vertical" Width="100%" AutoGenerateColumns="False">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField HeaderText="IssueID">
                    <ItemTemplate>      
                            <asp:HyperLink ID="lblIssueID" runat="server" Text='<%# Bind("IssueSN")%>' Target="_blank"
                                 NavigateUrl='<%# Bind("URL") %>'></asp:HyperLink>
                            <asp:HiddenField ID="hfIssueID" runat="server" Value='<%# Bind("IssueID") %>'></asp:HiddenField>
                     </ItemTemplate>
                     <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="4%" />
                    <ItemStyle Font-Size="8pt" />
                </asp:TemplateField>
                <asp:BoundField DataField="Project" HeaderText="Project" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="66pt" />
                    <ItemStyle Font-Size="8pt" />
                 </asp:BoundField> 
                <asp:BoundField DataField="MaterialType" HeaderText="PCA P/N" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="70pt" />
                    <ItemStyle Font-Size="8pt" />
                 </asp:BoundField> 
                <asp:BoundField HeaderText="Station" DataField="Station">
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="49pt" />
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField> 
                <asp:BoundField DataField="IssueDate" HeaderText="Issue date" >
                 <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="56pt" />
                    <ItemStyle Font-Size="8pt" />
                 </asp:BoundField> 
                 <asp:BoundField DataField="SN" HeaderText="S/N" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="81pt" />
                    <ItemStyle Font-Size="8pt" />
                 </asp:BoundField> 
                <asp:BoundField HeaderText="Defect Symptom" DataField="DefectSymptom" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="98pt" />
                    <ItemStyle Font-Size="8pt"  />
                </asp:BoundField> 
                <asp:BoundField HeaderText="Location" DataField="Location" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="102pt" />
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField> 
                <asp:BoundField HeaderText="Defective Component P/N" DataField="DefectComponentPN" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="98pt"/>
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="PhotoList">
                    <ItemTemplate>      
                            <asp:HyperLink ID="hlPhoto" runat="server" Text='<%# Bind("Photo")%>' Target="_blank"
                                 NavigateUrl='<%# Bind("PhotoURL") %>'></asp:HyperLink>                            
                     </ItemTemplate>
                     <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="2%" />
                    <ItemStyle Font-Size="8pt"  HorizontalAlign="Center"/>
                </asp:TemplateField>
                 <asp:BoundField DataField="RootCause" HeaderText="RootCause" >
                     <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="218pt"/>
                    <ItemStyle Font-Size="8pt" />
                 </asp:BoundField>
                 <asp:BoundField DataField="CorrectAction" HeaderText="CorrectAction" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="314pt"/>
                    <ItemStyle Font-Size="8pt" />
                 </asp:BoundField> 
                 <asp:BoundField HeaderText="PIC" DataField="PIC">
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="58pt"/>
                    <ItemStyle Font-Size="8pt" />
                 </asp:BoundField> 
                <asp:BoundField HeaderText="Liability" DataField="Liability">
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="54pt"/>
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField> 
                <asp:BoundField HeaderText="Status" DataField="Status" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="66pt"/>
                    <ItemStyle Font-Size="8pt" />
                 </asp:BoundField> 
                <asp:BoundField HeaderText="TAT" DataField="PendingDate" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left"  Width="47pt"/>
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField> 
                <asp:BoundField HeaderText="Priority" DataField="Priority" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="54pt"/>
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField> 
                <asp:BoundField HeaderText="Repeat Defect?" DataField="RelatedIssue" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="46pt"/>
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField> 
                 <asp:BoundField HeaderText="Failure Rate" DataField="FailureRate" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="53pt"/>
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField> 
            </Columns>
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        <br />
            <asp:GridView ID="gvCPUDetil" runat="server" CellPadding="2" ForeColor="#333333" 
                    GridLines="Vertical" Width="100%" AutoGenerateColumns="False">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField HeaderText="IssueID">
                    <ItemTemplate>
                        <asp:HyperLink ID="lblIssueID" runat="server" NavigateUrl='<%# Bind("URL") %>'
                            Target="_blank" Text='<%# Bind("IssueSN")%>'></asp:HyperLink>
                        <asp:HiddenField ID="hfIssueID" runat="server" Value='<%# Bind("IssueID") %>' />
                    </ItemTemplate>
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="4%" />
                    <ItemStyle Font-Size="8pt" />
                </asp:TemplateField>
                <asp:BoundField DataField="Project" HeaderText="Project" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="66pt" />
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField>
                <asp:BoundField DataField="MaterialType" HeaderText="CPU" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="70pt" />
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Station" DataField="Station">
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="49pt" />
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField>
                <asp:BoundField DataField="IssueDate" HeaderText="Issue date" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="56pt" />
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField>
                <asp:BoundField DataField="SN" HeaderText="S/N" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="81pt" />
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Defect Symptom" DataField="DefectSymptom" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="98pt" />
                    <ItemStyle Font-Size="8pt"  />
                </asp:BoundField>
                <asp:BoundField HeaderText="Faulty Commodity" DataField="FaultCommodity" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="113pt" />
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Faulty Commodity P/N" DataField="FaultCommodityPN" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="98pt"/>
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Faulty Commodity S/N" DataField="FaultCommoditySN" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="98pt"/>
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="PhotoList">
                    <ItemTemplate>      
                            <asp:HyperLink ID="hlPhoto" runat="server" Text='<%# Bind("Photo")%>' Target="_blank"
                                 NavigateUrl='<%# Bind("PhotoURL") %>'></asp:HyperLink>                            
                     </ItemTemplate>
                     <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="2%" />
                    <ItemStyle Font-Size="8pt"  HorizontalAlign="Center"/>
                </asp:TemplateField>
                <asp:BoundField DataField="RootCause" HeaderText="RootCause" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="218pt"/>
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField>
                <asp:BoundField DataField="CorrectAction" HeaderText="CorrectAction" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="314pt"/>
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField>
                <asp:BoundField HeaderText="PIC" DataField="PIC">
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="58pt"/>
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Liability" DataField="Liability">
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="54pt"/>
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Status" DataField="Status" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="66pt"/>
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField>
                <asp:BoundField HeaderText="TAT" DataField="PendingDate" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left"  Width="47pt"/>
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Priority" DataField="Priority" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="54pt"/>
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Repeat Defect?" DataField="RelatedIssue" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="46pt"/>
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Failure Rate" DataField="FailureRate" >
                    <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" Width="53pt"/>
                    <ItemStyle Font-Size="8pt" />
                </asp:BoundField>
            </Columns>
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>

</asp:Content>

