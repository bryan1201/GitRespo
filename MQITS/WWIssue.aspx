<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WWIssue.aspx.cs" Inherits="WWIssue"  MaintainScrollPositionOnPostback="true"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>=== MQITS::Issue ===</title>
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
    <link rel="stylesheet" type="text/css" href="PCB.css" />
    <link href="wesley.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 176px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align:center;width:100%">
        <div style="width:900px;text-align:center;" >
            <div class="simple_navi" style="width: 100%">
            <div class="bar_top">
                <h4><asp:label ID="lblIssueFunction" runat="server" Text="WW Report/Edit Issue"></asp:label></h4>
            </div>
            <div style="width: 100%">
                <asp:CheckBox ID="chkShowIssue" Text="Show Issue Detail" runat="server" Checked="true" AutoPostBack="True" OnCheckedChanged="chkShowIssue_CheckedChanged" />
                <asp:FormView ID="fvIssue" runat="server" DefaultMode="Insert" CellPadding="4" Font-Names="Arial Unicode MS" Font-Size="10pt" ForeColor="#333333"   OnItemCommand="fvIssue_ItemCommand" OnModeChanging="fvIssue_ModeChanging"  OnItemInserting="fvIssue_ItemInserting"   DataSourceID="SqlDSMPDetailIssues" OnDataBound="fvIssue_DataBound" >
                <InsertItemTemplate>
                    <div style="width: 902px; background-color: #E3EAEB; text-align: left;">
                        <table style="border-right: #b6cade 2px solid; border-top: #b6cade 2px solid; border-left: #b6cade 2px solid; width: 893px; color: #333333; border-bottom: #b6cade 2px solid; font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white" cellpadding="0" cellspacing="1">
                            <tr>
                                <td style="width: 30%; background-color: #b6cade; height: 18px;">
                                    <asp:LinkButton ID="UpdateSubmit" runat="server" CommandArgument="Submit"
                                        CommandName="Insert" Text="Submit Issue" Font-Bold="True" ForeColor="Red" Visible="False"></asp:LinkButton>
                                    <asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                        Text="Cancel" Font-Bold="True" ForeColor="#C000C0" Visible="False" ></asp:LinkButton>
                                    </td>
                                <td style="background-color: #b6cade; height: 18px;"></td>
                                <td style="width:50%; height: 18px;"></td>
                                <td style="height: 18px">
                                <asp:Label ID="lblIssueNoLabel" runat="server" Text="Issue No:" Visible="False"></asp:Label></td>
                                <td style="height: 18px">
                                 <asp:Label ID="lblIssueNo" runat="server" Visible="False"></asp:Label></td>
                             </tr>
                        </table>
                        <div style="width:99%; border-right: firebrick 2px solid; border-top: firebrick 2px solid; border-left: firebrick 2px solid; border-bottom: firebrick 2px solid; text-align: left;">
                            <table width="100%" style="font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white; color: #333333;" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td style="width: 20%; color: #333333;  background-color: #b6cade;">
                                        <span style="color: #ff0000">*</span><asp:Label ID="lblTitle" runat="server">Title</asp:Label>
                                    </td>
                                    <td style="color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;" 
                                        colspan="3">
                                        <asp:TextBox ID="txtTitle" runat="server" Width="99%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; color: #333333;  background-color: #b6cade; ">
                                        <span style="color: #ff0000">*</span><asp:Label ID="lblCustomer" runat="server">Customer</asp:Label>
                                        <asp:DropDownList ID="ddlSite" runat="server" DataSourceID="SqlDSSite" 
                                            DataTextField="SiteName" DataValueField="SiteID" Visible="False">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDSSite" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                                            SelectCommand="select DISTINCT Site as SiteID ,SiteName from v_userrole where BadgeCode=@editor and Site is not null">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="txtUserId" Name="editor" PropertyName="Text" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </td>
                                    <td style="WIDTH: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8; ">
                                        <asp:DropDownList ID="ddlCustomer" runat="server" AutoPostBack="True" 
                                            DataSourceID="SqlDSCustomer" DataTextField="CustomerName" 
                                            DataValueField="CustomerID" Width="100%">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDSCustomer" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                                            SelectCommand="select CustomerID,CustomerName from v_Customer">
                                        </asp:SqlDataSource>
                                    </td>
                                    <td style="width: 20%;  background-color: #b6cade; ">
                                        <span style="color: #ff0000">*</span><asp:Label ID="lblReporter" runat="server">Reporter</asp:Label>
                                    </td>
                                    <td style="width: 30%; color: #333333; background-color: #eff3f8; ">
                                        <asp:Label ID="lblReporterName" runat="server" Width="100%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; color: #333333;  background-color: #b6cade;">
                                        <asp:Label ID="lblProjectCategory" runat="server">Project Category</asp:Label>
                                        &nbsp;</td>
                                    <td style="WIDTH: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                                        <asp:CheckBoxList ID="chkListPrjCategory" runat="server" 
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem>BL</asp:ListItem>
                                            <asp:ListItem>DL</asp:ListItem>
                                            <asp:ListItem>ML</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>                           
                                    <td style="width: 20%; background-color: #b6cade;">
                                        <asp:Label ID="lblReportSite" runat="server">Report Site</asp:Label>
                                    </td>
                                    <td style="width: 30%; color: #333333;  background-color: #eff3f8;">
                                        <asp:DropDownList ID="ddlReportSite" runat="server" Width="99%">
                                            <asp:ListItem>TAO</asp:ListItem>
                                            <asp:ListItem>IPT</asp:ListItem>
                                            <asp:ListItem>IMX</asp:ListItem>
                                            <asp:ListItem>ICZ</asp:ListItem>
                                            <asp:ListItem>IHS</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; color: #333333;  background-color: #b6cade;">
                                        <span style="color: #ff0000">*</span><asp:Label ID="lblProject" runat="server">Project</asp:Label>
                                    </td>
                                    <td style="WIDTH: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                                        <asp:DropDownList ID="ddlProject" runat="server" AutoPostBack="True" 
                                            DataSourceID="SqlDSProject" DataTextField="ProjectName" 
                                            DataValueField="ProjectID" 
                                            OnSelectedIndexChanged="ddlProject_SelectedIndexChanged" Width="100%">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDSProject" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT '' as ProjectID ,'-' as ProjectName 
        UNION
        select CONVERT(NVARCHAR,GroupID) as ProjectID,Title as ProjectName 
        FROM m_Group
        WHERE GroupType='5' AND Parent_GroupID='60' 
        AND IsMP=1 AND IsEnable=1 AND IsEOSL=0
        AND dbo.GetXml(Remark,'CUSTOMER')=@Customer
        ORDER BY ProjectName
        ">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddlCustomer" Name="Customer" 
                                                    PropertyName="SelectedValue" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </td>
                                    <td style="width: 20%; background-color: #b6cade;">
                                        <span style="color: #ff0000">*</span><asp:Label ID="lblOwner" runat="server">Issue Owner</asp:Label>
                                    </td>
                                    <td style="width: 30%; color: #333333;  background-color: #eff3f8;">
                                        <asp:DropDownList ID="ddlOwner" runat="server" DataSourceID="SqlDSIssueOwner" 
                                            DataTextField="OwnerName" DataValueField="OwnerCode" Width="99%">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDSIssueOwner" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="--SELECT * FROM fn_GetIssueProjectOwner(Project,1)
        select distinct  BadgeCode as OwnerCode,dbo.fn_GetChtName(BadgeCode) as OwnerName
        FROM v_userrole 
        WHERE  BadgeCode=@editor   --and  RoleCode in (7,10,16,17,18)">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="txtUserId" Name="editor" PropertyName="Text" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; color: #333333;  background-color: #b6cade;">
                                        <span style="color: #ff0000">*</span><asp:Label ID="lblTriggerType" runat="server" >Trigger Type</asp:Label></td>
                                    <td style="WIDTH: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                                        <asp:DropDownList ID="ddlTriggerType" runat="server" Width="100%">
                                            <asp:ListItem Value="Internal Issue">Internal Issue</asp:ListItem>
                                            <asp:ListItem>Customer Complain</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 20%;  background-color: #b6cade; ">
                                        <span style="color: #ff0000">*</span><asp:Label ID="lblOccurenceDate" runat="server">Occurence Date</asp:Label>
                                    </td>
                                    <td style="width: 30%; color: #333333;  background-color: #eff3f8">
                                        <asp:TextBox ID="txtOccruenceDate" runat="server" Width="90%"></asp:TextBox>
                                        <img ID="Img6" runat="server" alt="選取日期" 
                                            onclick="SPM_OpenCalendar(document.all('fvIssue$txtDueDate').name, false)" 
                                            src="images/calendar.gif" /></td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; background-color: #b6cade">
                                        <span style="color: #ff0000">*</span><asp:Label ID="lblPriority" runat="server">Priority</asp:Label>
                                    </td>
                                    <td style="width: 30%; color: #333333; background-color: #eff3f8">
                                        <asp:DropDownList ID="ddlPriority" runat="server" DataSourceID="SqlDSPriority" 
                                            DataTextField="TEXT" DataValueField="VALUE" Width="100%">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDSPriority" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                                            SelectCommand="SELECT * FROM v_IssuePriority"></asp:SqlDataSource>
                                    </td>
                                    <td style="width: 20%;  background-color: #b6cade; color: #333333;">
                                        <span style="color: #ff0000">*</span><asp:Label ID="lblIssueDate" 
                                            runat="server">Issue Date</asp:Label>
                                    </td>
                                    <td style="width: 30%; color: #333333;  background-color: #eff3f8; font-family: 'Arial Unicode MS';">
                                        <asp:TextBox ID="txtIssueDate" runat="server" Width="90%"></asp:TextBox>
                                        <img ID="Img1" runat="server" alt="選取日期" 
                                            onclick="SPM_OpenCalendar(document.all('fvIssue$txtIssueDate').name, false)" 
                                            src="images/calendar.gif" /></td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; color: #333333; background-color: #b6cade">
                                        &nbsp;</td>
                                    <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8">
                                        &nbsp;</td>
                                    <td style="width: 20%; background-color: #b6cade">
                                        <asp:Label ID="lblDueDate" runat="server">Due Date</asp:Label></td>
                                    <td style="width: 30%; color: #333333; background-color: #eff3f8">
                                        <asp:TextBox ID="txtDueDate" runat="server" Width="90%"></asp:TextBox>
                                        <img id="Img2" alt="選取日期" runat="server" onclick="SPM_OpenCalendar(document.all('fvIssue$txtDueDate').name, false)" src="images/calendar.gif"/></td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; color: #333333; background-color: #b6cade;">
                                        <asp:Label ID="lblPDPA" runat="server">Problem description and Preliminary Analysis <br />(by Reporter)</asp:Label>
                                    </td>
                                    <td colspan="3" 
                                        style="color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                                        <asp:TextBox ID="txtPDPA" runat="server" Rows="8" TextMode="MultiLine" 
                                            Width="100%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; color: #333333; background-color: #b6cade;">
                                        <span style="color: #ff0000">*</span><asp:Label ID="lblPCACPU" runat="server">P/N, SKU</asp:Label>
                                    </td>
                                    <td style="WIDTH: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                                        <asp:TextBox ID="txtProjecMaterial" runat="server" MaxLength="12" 
                                            Visible="false" Width="100px"></asp:TextBox>
                                    </td>
                                    <td style="width: 20%;  background-color: #b6cade;">
                                        <span style="color: #ff0000">*</span><asp:Label ID="lblStatus" runat="server">Status</asp:Label>
                                    </td>
                                    <td style="width: 30%; color: #333333;  background-color: #eff3f8;">
                                        <asp:DropDownList ID="ddlStatus" runat="server" DataSourceID="SqlDSStatus" 
                                            DataTextField="TEXT" DataValueField="VALUE" Width="100%">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDSStatus" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                                            SelectCommand="SELECT * FROM [v_IssueStatus]"></asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; color: #333333; background-color: #b6cade;">
                                         <asp:Label ID="lblDPPM" runat="server">Failure Rate (% or DPPM)</asp:Label>
                                    </td>
                                    <td style="WIDTH: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                                        <asp:Label ID="lblFailureRate" runat="server" Font-Bold="True" Font-Size="10pt" 
                                            ForeColor="Red" Width="100%"></asp:Label>
                                    </td>
                                    
                                    <td style="width: 20%;  background-color: #b6cade;">                              
                                        <asp:Label ID="lblDTQty" runat="server" Text="Defect Qty / Total Qty"></asp:Label>
                                    </td>
                                    <td style="width: 30%; color: #333333;  background-color: #eff3f8;">
                                        <asp:TextBox ID="txtDefectQty" runat="server" MaxLength="12" Visible="false" 
                                            Width="100px"></asp:TextBox>
                                        &nbsp;/ <asp:TextBox ID="txtTotalQty" runat="server" MaxLength="12" Visible="false" 
                                            Width="100px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; color: #333333; background-color: #b6cade">
                                        <span style="color: #ff0000">*<asp:Label ID="lblFRComponent" runat="server">Failure Component information ( SN# / CT# / PO# etc)</asp:Label>
                                        </span></td>
                                    <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8">
                                        <asp:Button ID="btnUploadFRComponent" runat="server" Text="Upload" />
                                        <asp:FileUpload ID="fuFRComponent" runat="server" />
                                    </td>
                                    <td style="width: 20%;  background-color: #b6cade">
                                        <asp:Label ID="lblRepeatDefect" runat="server">Repeat Defect?</asp:Label>
                                    </td>
                                    <td style="width: 30%; color: #333333; background-color: #eff3f8">
                                        <asp:Label ID="lblRepeatDefectCheck" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <p>&nbsp;</p>
                            <table style="border-right: #b6cade 2px solid; border-top: #b6cade 2px solid; border-left: #b6cade 2px solid; width: 893px; color: #333333; border-bottom: #b6cade 2px solid; font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white" cellpadding="0" cellspacing="1">
                                <tr>
                                    <td style="width: 20%; color: #333333; background-color: #b6cade" colspan="4">
                                        Impact Scope</td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; color: #333333; background-color: #b6cade;">
                                        <asp:Label ID="lblAffectedSites" runat="server">Affected Sites</asp:Label>
                                    </td>
                                    <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8;">
                                        <asp:CheckBoxList ID="chkListAffectedSites" runat="server" 
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem>TAO</asp:ListItem>
                                            <asp:ListItem>IPT</asp:ListItem>
                                            <asp:ListItem>IMX</asp:ListItem>
                                            <asp:ListItem>ICZ</asp:ListItem>
                                            <asp:ListItem>IHS</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                    <td style="width: 20%; background-color: #b6cade;">
                                        <asp:Label ID="lblAffectedSKUPN" runat="server">SKU or P/N</asp:Label></td>
                                    <td style="width: 30%; color: #333333; background-color: #eff3f8;">
                                        <asp:TextBox ID="txtAffectedSKUPN" runat="server" MaxLength="12" 
                                            Visible="false" Width="100px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; color: #333333;  background-color: #b6cade;">
                                        <asp:Label 
                                            ID="lblAffectedScope" runat="server" >Affected Scope</asp:Label>
                                        </td>
                                    <td 
                                        
                                        style="color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8; width: 30%;">
                                        <asp:TextBox ID="txtAffectedScope" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                    </td>
                                    <td style="width: 20%; background-color: #b6cade;">
                                        <span style="color: #ff0000">*<asp:Label ID="lblAffectedCustomer" 
                                            runat="server">Customer</asp:Label>
                                        </span>
                                    </td>
                                    <td style="width: 30%; color: #333333; background-color: #eff3f8;">
                                        <asp:CheckBoxList ID="chkListAffectedCustomer" runat="server" 
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem>ISS</asp:ListItem>
                                            <asp:ListItem>BCS</asp:ListItem>
                                            <asp:ListItem>SWD</asp:ListItem>
                                            <asp:ListItem>WGBU</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        <p>&nbsp;</p>
                        <table style="border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; width: 893px; color: #333333; border-bottom: white 1px solid; font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white" 
                        cellpadding="0" cellspacing="1">
                            <tr>
                                <td style="width: 20%; color: #333333; background-color: #b6cade; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid;">
                                    <asp:Label ID="lblRootCause" runat="server">Root Cause</asp:Label>
                                </td>
                                <td colspan="3" 
                                    style="color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8; width: 80%; ">
                                    <asp:TextBox ID="txtRootCause" runat="server" Rows="8" TextMode="MultiLine" 
                                        Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                        <tr>
                            <td style="width: 20%; color: #333333;background-color: #b6cade; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid;">
                               <asp:Label ID="lblContainmentPlan" runat="server">Containment Plan</asp:Label></td>
                            <td colspan="3" 
                                style="color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8; width: 80%;">
                                <asp:TextBox ID="txtContainmentPlan" runat="server" Rows="8" 
                                    TextMode="MultiLine" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                            <tr>
                                <td style="width: 20%; color: #333333;background-color: #b6cade; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid;">
                                    <asp:Label ID="lblPermanentCorrectiveAction" runat="server">Permanent Corrective Action</asp:Label>
                                </td>
                                <td colspan="3" 
                                    style="color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8; width: 80%;">
                                    <asp:TextBox ID="txtCorrectiveAction" runat="server" Rows="8" 
                                        TextMode="MultiLine" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%; background-color: #b6cade;">
                                    <span style="color: #ff0000">*<asp:Label ID="lblLiability" runat="server">Liability</asp:Label>
                                    </span>
                                </td>
                                <td style="width: 30%; color: #333333; background-color: #eff3f8;">
                                    <asp:DropDownList ID="ddlLiability" runat="server" 
                                        DataSourceID="SqlDSLiability" DataTextField="TEXT" DataValueField="VALUE" 
                                        Width="99%">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDSLiability" runat="server" 
                                        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
                                        SelectCommand="SELECT * FROM v_IssueLiability"></asp:SqlDataSource>
                                </td>
                                <td style="width: 20%; background-color: #b6cade; color: #333333;">
                                    Quix No</td>
                                <td style="width: 30%; color: #333333; background-color: #eff3f8">
                                    <asp:TextBox ID="txtQuixNo" runat="server" MaxLength="12" Visible="false" 
                                        Width="100px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%; background-color: #b6cade;">
                                    QEN No</td>
                                <td style="width: 30%; color: #333333; background-color: #eff3f8;">
                                    <asp:TextBox ID="txtQENNO" runat="server" MaxLength="12" Visible="false" 
                                        Width="100px"></asp:TextBox>
                                </td>
                                <td style="width: 20%; background-color: #b6cade; color: #333333;">
                                    PEN No</td>
                                <td style="width: 30%; color: #333333; background-color: #eff3f8">
                                    <asp:TextBox ID="txtPENNO" runat="server" MaxLength="12" Visible="false" 
                                        Width="100px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%; background-color: #b6cade;">
                                    8D Report</td>
                                <td style="width: 30%; color: #333333; background-color: #eff3f8;">
                                    <asp:Button ID="btnGen8D" runat="server" Text="Generate 8D Report" />
                                </td>
                                <td style="width: 20%; background-color: #b6cade; color: #333333;">
                                    Orginal Mail</td>
                                <td style="width: 30%; color: #333333; background-color: #eff3f8">
                                    <asp:TextBox ID="txtOrginalMail" runat="server" MaxLength="12" Visible="false" 
                                        Width="100px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>                          
                    <asp:LinkButton ID="IssueTitle" runat="server" CommandArgument="Submit" CommandName="Insert"
                        Font-Bold="True" ForeColor="Red" Text="Save Issue"></asp:LinkButton>
                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                        Font-Bold="True" ForeColor="#C000C0" Text="Cancel"></asp:LinkButton>
                    </div>
                </InsertItemTemplate>
               
                <ItemTemplate>
                   <div style="width: 902px; background-color: #E3EAEB; text-align: left;">
                        <table style="border-right: #b6cade 2px solid; border-top: #b6cade 2px solid; border-left: #b6cade 2px solid; width: 893px; color: #333333; border-bottom: #b6cade 2px solid; font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white" cellpadding="0" cellspacing="1">
                            <tr>
                                <td style="width: 30%; background-color: #b6cade; height: 18px;">
                                    <asp:LinkButton ID="UpdateSubmit" runat="server" CommandArgument="Submit"
                                        CommandName="Insert" Text="Submit Issue" Font-Bold="True" ForeColor="Red" Visible="False"></asp:LinkButton>
                                    <asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                        Text="Cancel" Font-Bold="True" ForeColor="#C000C0" Visible="False" ></asp:LinkButton>
                                    </td>
                                <td style="background-color: #b6cade; height: 18px;"></td>
                                <td style="width:50%; height: 18px;"></td>
                                <td style="height: 18px">
                                <asp:Label ID="lblIssueNoLabel" runat="server" Text="Issue No:" Visible="False"></asp:Label></td>
                                <td style="height: 18px">
                                 <asp:Label ID="lblIssueNo" runat="server" Visible="False"></asp:Label></td>
                             </tr>
                        </table>
                        <div style="width:99%; border-right: firebrick 2px solid; border-top: firebrick 2px solid; border-left: firebrick 2px solid; border-bottom: firebrick 2px solid; text-align: left;">
                            <table width="100%" style="font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white; color: #333333;" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td style="width: 20%; color: #333333;  background-color: #b6cade;">
                                        <span style="color: #ff0000">*</span><asp:Label ID="lblTitle" runat="server">Title</asp:Label>
                                    </td>
                                    <td style="color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;" 
                                        colspan="3">
                                        <asp:Label ID="lblTitleValue" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; color: #333333;  background-color: #b6cade; ">
                                        <span style="color: #ff0000">*</span><asp:Label ID="lblCustomer" runat="server">Customer</asp:Label>
                                    </td>
                                    <td style="WIDTH: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8; ">
                                        <asp:Label ID="lblCustomerValue" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 20%;  background-color: #b6cade; ">
                                        <span style="color: #ff0000">*</span><asp:Label ID="lblReporter" runat="server">Reporter</asp:Label>
                                    </td>
                                    <td style="width: 30%; color: #333333; background-color: #eff3f8; ">
                                        <asp:Label ID="lblReporterName" runat="server" Width="100%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; color: #333333;  background-color: #b6cade;">
                                        <asp:Label ID="lblProjectCategory" runat="server">Project Category</asp:Label>
                                        &nbsp;</td>
                                    <td style="WIDTH: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                                        <asp:CheckBoxList ID="chkListPrjCategory" runat="server" 
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem>BL</asp:ListItem>
                                            <asp:ListItem>DL</asp:ListItem>
                                            <asp:ListItem>ML</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>                           
                                    <td style="width: 20%; background-color: #b6cade;">
                                        <asp:Label ID="lblReportSite" runat="server">Report Site</asp:Label>
                                    </td>
                                    <td style="width: 30%; color: #333333;  background-color: #eff3f8;">
                                        <asp:Label ID="lblReportSiteValue" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; color: #333333;  background-color: #b6cade;">
                                        <span style="color: #ff0000">*</span><asp:Label ID="lblProject" runat="server">Project</asp:Label>
                                    </td>
                                    <td style="WIDTH: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                                        <asp:Label ID="lblProjectValue" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 20%; background-color: #b6cade;">
                                        <span style="color: #ff0000">*</span><asp:Label ID="lblOwner" runat="server">Issue Owner</asp:Label>
                                    </td>
                                    <td style="width: 30%; color: #333333;  background-color: #eff3f8;">
                                        <asp:Label ID="lblIssueOwnerValue" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; color: #333333;  background-color: #b6cade;">
                                        <span style="color: #ff0000">*</span><asp:Label ID="lblTriggerType" runat="server" >Trigger Type</asp:Label></td>
                                    <td style="WIDTH: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                                        <asp:Label ID="lblTriggerTypeValue" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 20%;  background-color: #b6cade; ">
                                        <span style="color: #ff0000">*</span><asp:Label ID="lblOccurenceDate" runat="server">Occurence Date</asp:Label>
                                    </td>
                                    <td style="width: 30%; color: #333333;  background-color: #eff3f8">
                                        <asp:Label ID="lblOccurenceDateValue" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; background-color: #b6cade">
                                        <span style="color: #ff0000">*</span><asp:Label ID="lblPriority" runat="server">Priority</asp:Label>
                                    </td>
                                    <td style="width: 30%; color: #333333; background-color: #eff3f8">
                                        <asp:Label ID="lblPriorityValue" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 20%;  background-color: #b6cade; color: #333333;">
                                        <span style="color: #ff0000">*</span><asp:Label ID="lblIssueDate" 
                                            runat="server">Issue Date</asp:Label>
                                    </td>
                                    <td style="width: 30%; color: #333333;  background-color: #eff3f8; font-family: 'Arial Unicode MS';">
                                        <asp:Label ID="lblIssueDateValue" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; color: #333333; background-color: #b6cade">
                                        &nbsp;</td>
                                    <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8">
                                        &nbsp;</td>
                                    <td style="width: 20%; background-color: #b6cade">
                                        <asp:Label ID="lblDueDate" runat="server">Due Date</asp:Label></td>
                                    <td style="width: 30%; color: #333333; background-color: #eff3f8">
                                        <asp:Label ID="lblDueDateValue" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; color: #333333; background-color: #b6cade;">
                                        <asp:Label ID="lblPDPA" runat="server">Problem description and Preliminary Analysis <br />(by Reporter)</asp:Label>
                                    </td>
                                    <td colspan="3" 
                                        style="color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                                        <asp:Label ID="lblPDPAValue" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="color: #333333; background-color: #b6cade;">
                                        <span style="color: #ff0000">*</span><asp:Label ID="lblPNSKU" runat="server">P/N, SKU</asp:Label>
                                    </td>
                                    <td style="color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                                        <asp:Label ID="lblPNSKUValue" runat="server"></asp:Label>
                                    </td>
                                    <td style="background-color: #b6cade;">
                                        <span style="color: #ff0000">*</span><asp:Label ID="lblStatus" runat="server">Status</asp:Label>
                                    </td>
                                    <td style="color: #333333;  background-color: #eff3f8;">
                                        <asp:Label ID="lblStatusValue" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; color: #333333; background-color: #b6cade;">
                                         <asp:Label ID="lblDPPM" runat="server">Failure Rate (% or DPPM)</asp:Label>
                                    </td>
                                    <td style="WIDTH: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                                        <asp:Label ID="lblFailureRate" runat="server" Font-Bold="True" Font-Size="10pt" 
                                            ForeColor="Red" Width="100%"></asp:Label>
                                    </td>
                                    
                                    <td style="width: 20%;  background-color: #b6cade;">                              
                                        <asp:Label ID="lblDTQty" runat="server" Text="Defect Qty / Total Qty"></asp:Label>
                                    </td>
                                    <td style="width: 30%; color: #333333;  background-color: #eff3f8;">
                                        <asp:Label ID="lblDefectQtyValue" runat="server"></asp:Label>
                                        &nbsp;/
                                        <asp:Label ID="lblTotalQtyValue" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; color: #333333; background-color: #b6cade">
                                        <span style="color: #ff0000">*<asp:Label ID="lblFRComponent" runat="server">Failure Component information ( SN# / CT# / PO# etc)</asp:Label>
                                        </span></td>
                                    <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8">
                                        <asp:Label ID="lblFRComponentValue" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 20%;  background-color: #b6cade">
                                        <asp:Label ID="lblRepeatDefect" runat="server">Repeat Defect?</asp:Label>
                                    </td>
                                    <td style="width: 30%; color: #333333; background-color: #eff3f8">
                                        <asp:Label ID="lblRepeatDefectCheck" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <p>&nbsp;</p>
                            <table style="border-right: #b6cade 2px solid; border-top: #b6cade 2px solid; border-left: #b6cade 2px solid; width: 893px; color: #333333; border-bottom: #b6cade 2px solid; font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white" cellpadding="0" cellspacing="1">
                                <tr>
                                    <td style="width: 20%; color: #333333; background-color: #b6cade" colspan="4">
                                        Impact Scope</td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; color: #333333; background-color: #b6cade;">
                                        <asp:Label ID="lblAffectedSites" runat="server">Affected Sites</asp:Label>
                                    </td>
                                    <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8;">
                                        <asp:CheckBoxList ID="chkListAffectedSites" runat="server" 
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem>TAO</asp:ListItem>
                                            <asp:ListItem>IPT</asp:ListItem>
                                            <asp:ListItem>IMX</asp:ListItem>
                                            <asp:ListItem>ICZ</asp:ListItem>
                                            <asp:ListItem>IHS</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                    <td style="width: 20%; background-color: #b6cade;">
                                        <asp:Label ID="lblAffectedSKUPN" runat="server">SKU or P/N</asp:Label></td>
                                    <td style="width: 30%; color: #333333; background-color: #eff3f8;">
                                        <asp:Label ID="lblAffectedSKUPNValue" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; color: #333333;  background-color: #b6cade;">
                                        <asp:Label 
                                            ID="lblAffectedScope" runat="server" >Affected Scope</asp:Label>
                                        </td>
                                    <td 
                                        
                                        style="color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8; width: 30%;">
                                        <asp:Label ID="lblAffectedScopeValue" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 20%; background-color: #b6cade;">
                                        <span style="color: #ff0000">*<asp:Label ID="lblAffectedCustomer" 
                                            runat="server">Customer</asp:Label>
                                        </span>
                                    </td>
                                    <td style="width: 30%; color: #333333; background-color: #eff3f8;">
                                        <asp:CheckBoxList ID="chkListAffectedCustomer" runat="server" 
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem>ISS</asp:ListItem>
                                            <asp:ListItem>BCS</asp:ListItem>
                                            <asp:ListItem>SWD</asp:ListItem>
                                            <asp:ListItem>WGBU</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; color: #333333;  background-color: #b6cade;">
                                        &nbsp;</td>
                                    <td colspan="3" 
                                        style="color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8; ">
                                        <table style="width:100%;">
                                            <tr>
                                                <td class="style1">
                                                    <b><span lang="EN-US">Affected Scope(</span><span>影響範圍</span><span lang="EN-US">)</span></b></td>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="style1">
                                                    &nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="style1">
                                                    &nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        <p>&nbsp;</p>
                        <table style="border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; width: 893px; color: #333333; border-bottom: white 1px solid; font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white" 
                        cellpadding="0" cellspacing="1">
                            <tr>
                                <td style="width: 20%; color: #333333; background-color: #b6cade; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid;">
                                    <asp:Label ID="lblRootCause" runat="server">Root Cause</asp:Label>
                                </td>
                                <td colspan="3" 
                                    style="color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8; width: 80%; ">
                                    <asp:Label ID="lblRootCauseValue" runat="server"></asp:Label>
                                </td>
                            </tr>
                        <tr>
                            <td style="width: 20%; color: #333333;background-color: #b6cade; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid;">
                               <asp:Label ID="lblContainmentPlan" runat="server">Containment Plan</asp:Label></td>
                            <td colspan="3" 
                                style="color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8; width: 80%;">
                                <asp:Label ID="lblContainmentPlanValue" runat="server"></asp:Label>
                            </td>
                        </tr>
                            <tr>
                                <td style="width: 20%; color: #333333;background-color: #b6cade; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid;">
                                    <asp:Label ID="lblPermanentCorrectiveAction" runat="server">Permanent Corrective Action</asp:Label>
                                </td>
                                <td colspan="3" 
                                    style="color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8; width: 80%;">
                                    <asp:Label ID="lblPermanentCorrectiveActionValue" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%; background-color: #b6cade;">
                                    <span style="color: #ff0000">*</span><asp:Label ID="lblLiability0" 
                                        runat="server">Liability</asp:Label>
                                </td>
                                <td style="width: 30%; color: #333333; background-color: #eff3f8;">
                                    <asp:Label ID="lblLiabilityValue" runat="server"></asp:Label>
                                </td>
                                <td style="width: 20%; background-color: #b6cade; color: #333333;">
                                    Quix No</td>
                                <td style="width: 30%; color: #333333; background-color: #eff3f8">
                                    <asp:Label ID="lblQuixNoValue" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%; background-color: #b6cade;">
                                    QEN No</td>
                                <td style="width: 30%; color: #333333; background-color: #eff3f8;">
                                    <asp:Label ID="lblQENNoValue" runat="server"></asp:Label>
                                </td>
                                <td style="width: 20%; background-color: #b6cade; color: #333333;">
                                    PEN No</td>
                                <td style="width: 30%; color: #333333; background-color: #eff3f8">
                                    <asp:Label ID="lblPENNoValue" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%; background-color: #b6cade;">
                                    8D Report</td>
                                <td style="width: 30%; color: #333333; background-color: #eff3f8;">
                                    <asp:HyperLink ID="hl8DReport" runat="server" NavigateUrl="#">Link to 8D Report</asp:HyperLink>
                                </td>
                                <td style="width: 20%; background-color: #b6cade; color: #333333;">
                                    Orginal Mail</td>
                                <td style="width: 30%; color: #333333; background-color: #eff3f8">
                                    <asp:HyperLink ID="hlOrginalMail" runat="server" NavigateUrl="#">Orginal Mail of Issue</asp:HyperLink>
                                </td>
                            </tr>
                        </table>
                    </div>                          
                    <asp:LinkButton ID="IssueTitle" runat="server" CommandArgument="Submit" CommandName="Insert"
                        Font-Bold="True" ForeColor="Red" Text="Save Issue"></asp:LinkButton>
                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                        Font-Bold="True" ForeColor="#C000C0" Text="Cancel"></asp:LinkButton>
                    </div>                     
                </ItemTemplate>
                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            </asp:FormView>

    <div style="text-align:center;">
    <div style="width:900px; text-align: left;">
        <div class="simple_navi">
              <div class="bar_mid">
                <div class="btn_container3">
                    <asp:Menu ID="menuAction" runat="server" BackColor="Transparent" DynamicHorizontalOffset="2"
                        Font-Names="Verdana" Font-Size="0.8em" ForeColor="#990000" Orientation="Horizontal"
                        StaticSubMenuIndent="10px" OnMenuItemClick="menuAction_MenuItemClick" Height="30px"
                        Width="444px">
                        <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" BackColor="Transparent"
                            CssClass="simple_btn_inact" />
                        <DynamicHoverStyle BackColor="#990000" ForeColor="White" />
                        <DynamicMenuStyle BackColor="#FFFBD6" />
                        <StaticSelectedStyle CssClass="simple_btn_act" BackColor="Transparent" Font-Bold="False"
                            ForeColor="White" />
                        <DynamicSelectedStyle BackColor="#FFCC66" />
                        <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                        <Items>
                            <asp:MenuItem Text="Action Requests" Value="0" Selected="True"></asp:MenuItem>
                            <asp:MenuItem Text="Attachment Files" Value="1"></asp:MenuItem>
                            <asp:MenuItem Text="Issue Photo" Value="2"></asp:MenuItem>
                            <asp:MenuItem Text="Handling  History" Value="3"></asp:MenuItem>
                            <asp:MenuItem Text="Repeat Defect Check" Value="4"></asp:MenuItem>
                            <asp:MenuItem Text="Note" Value="5"></asp:MenuItem>
                        </Items>
                        <StaticHoverStyle BackColor="Transparent" CssClass="simple_btn_hover" />
                        <StaticMenuStyle BackColor="Transparent" />
                    </asp:Menu>
                </div>
            </div>
         </div>
         <asp:MultiView ID="mvIssueForm" runat="server">
            <asp:View ID="vHR" runat="server" >
            <table width="100%" style="font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white; color: #333333; font-size: 10pt; border-top-style: groove; border-right-style: groove; border-left-style: groove; border-bottom-style: groove;" cellpadding="1" cellspacing="1" id="TABLE1">
                <tr>
                    <td style="width: 20%; color: #333333; background-color: #b6cade;" align="left">
                        <span style="color: #ff0000">*</span><asp:Label ID="lblAssignedTo" runat="server">AssignedTo</asp:Label>
                        <div style="display:none"><asp:TextBox ID="txtAssignedToBadgeCode" runat="server" Width="74px" ></asp:TextBox></div></td>
                    <td style="WIDTH: 30%; color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8;" align="left">
                        <asp:TextBox ID="txtAssignedTo" runat="server"></asp:TextBox>
                        <asp:Button ID="btnAssignedTo" runat="server" Text="Find User" 
                            OnClick="btnAssignedTo_Click" Font-Bold="True" Font-Names="Arial Unicode MS" 
                            ForeColor="Red"/>
                        </td>
                    <td style="width: 20%; background-color: #b6cade;" align="left">
                        <span style="color: #ff0000">*</span><asp:Label ID="lblStatus" runat="server">Status</asp:Label></td>
                    <td style="width: 30%; color: #333333;background-color: #eff3f8;">
                        <asp:DropDownList ID="ddlStatus" runat="server" Width="100%" DataSourceID="SqlDSARStatus" DataTextField="TEXT" DataValueField="VALUE" >
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDSARStatus" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT * FROM v_IssueStatus WHERE VALUE NOT IN ('2','4')"></asp:SqlDataSource>
                        </td>
                    
                </tr>
                <tr>
                    <td style="width: 20%; color: #333333; background-color: #b6cade;" align="left">
                        <span style="color: #ff0000">*</span><asp:Label ID="lblDueDay" runat="server">Due Date</asp:Label></td>
                    <td style="WIDTH: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;" align="left">
                        <asp:TextBox ID="txtDueDate" runat="server" Width="90%"></asp:TextBox>
                        <img id="Img3" alt="選取日期" runat="server" onclick="SPM_OpenCalendar(document.all('txtDueDate').name, false)" src="images/calendar.gif"/></td>
                    <td style="width: 20%; background-color: #b6cade;" align="left">
                       <asp:Label ID="lblPendingDay" runat="server">Pending Day</asp:Label></td>
                    <td style="width: 30%; color: #333333; background-color: #eff3f8;">
                        <asp:TextBox ID="txtPendingDay" runat="server" Width="100%" ReadOnly="True"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 20%; color: #333333;background-color: #b6cade">
                        <span style="color: #ff0000">*</span><asp:Label ID="lblActionDescription" runat="server" Text="Action Description"></asp:Label></td>
                    <td align="left" colspan="3" style="color: #333333; font-family: 'Arial Unicode MS';
                        background-color: #eff3f8; width: 80%;">
                        <asp:TextBox ID="txtActionDescription" runat="server" Width="99%"  TextMode="MultiLine" Rows="4"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="left" style="width: 20%; color: #333333; background-color: #b6cade">
                        <asp:Label ID="lblCompletionComment" runat="server" Width="125px">Completion Comment</asp:Label></td>
                    <td align="left" colspan="3" style="color: #333333; font-family: 'Arial Unicode MS';
                        background-color: #eff3f8; width: 79%;">
                        <asp:TextBox ID="txtCompletionComment" runat="server" Width="99%" TextMode="MultiLine" Rows="3"></asp:TextBox></td>
                </tr>
                 <tr>
                     <td colspan="4" style="text-align:center; width:80%; height: 24px;">
                        <table style="width:90%; font-family:Arial Unicode MS;">
                            <tr>
                                <td style="text-align: left; text-indent: 10px;">
                                    <asp:Button ID="btnAdd" runat="server" Text="Add Action" 
                                        OnClientClick="return confirm('are you sure?')" OnClick="btnAdd_Click" 
                                        Width="120px" Font-Names="Century Gothic" Font-Bold="True" />
                                </td>
                                <td>&nbsp;</td>                     
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" 
                                        OnClientClick="return confirm('are you sure?')" Width="60px" 
                                        Font-Names="Century Gothic" Font-Bold="True" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
                                        OnClick="btnCancel_Click" Width="60px" Font-Names="Century Gothic" 
                                        Font-Bold="True" />
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    &nbsp;<asp:HyperLink ID="hlSQMP" runat="server" NavigateUrl="#" Target="_blank" Text="Link to SQMP"></asp:HyperLink></td>
                            </tr>
                        </table>
                     </td>
                     </tr>                  
                <tr>
                    <td  colspan="4" align="center">
                       <asp:GridView ID="gvAR" runat="server" CellPadding="2" ForeColor="#333333" Width="100%" AutoGenerateColumns="False" DataSourceID="SqlDSAR" DataKeyNames="ActionID" OnSelectedIndexChanged="gvAR_SelectedIndexChanged">
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#EFF3FB" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbcheck" runat="server" Enabled='<%# ChangeBool(Eval("StatusVisible").ToString()) %>' />
                                    </ItemTemplate>
                                     <HeaderTemplate>
                                        <asp:Button ID="btnAll" runat="server" Text="Select All" Width="55px" Font-Size="X-Small" ForeColor="Navy" Height="25px" OnClick="btnAll_Click" />
                                    </HeaderTemplate>
                                    <ItemStyle Font-Size="8pt" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Select" Visible='<%# bool.Parse(Eval("EditVisible").ToString()) %>'
                                            Text="Edit"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="AssignTo">
                                     <ItemTemplate>
                                        <asp:Label ID="lblAssignedTo" runat="server" Text='<%# Bind("AssignedTo") %>'></asp:Label>
                                        <asp:Label ID="lblAssignedToCode" runat="server" Visible="false" Text='<%# Bind("AssignedToCode") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                     <ItemTemplate>
                                        <asp:Label ID="lblStatusName" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                        <asp:Label ID="lblStatusCode" runat="server" Text='<%# Bind("StatusCode") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Due Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDueDate" runat="server" Text='<%# Bind("DueDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pending Day">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPD" runat="server" Text='<%# Bind("PendingDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action Descr.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAD" runat="server" Text='<%# Bind("ActionDescription") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Completion Comment">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCC" runat="server" Text='<%# Bind("CompletionComment") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Finish Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFinishDate" runat="server" Text='<%# Bind("FinishDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActionID" runat="server" Text='<%# Bind("ActionID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                        <asp:Button ID="btnSendMail" runat="server" Font-Bold="True" Font-Names="Century Gothic"
                            OnClick="btnSendMail_Click" Text="SendMail" /><asp:SqlDataSource ID="SqlDSAR" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT fa.ActionID,dbo.fn_GetChtName(AssignedTo) as AssignedTo,dbo.GetXml(Remark,'TEXT') as Status,fa.AssignedTo as AssignedToCode,&#13;&#10;CONVERT(NVARCHAR(10),DueDate,111) as DueDate,&#13;&#10;[dbo].[fn_GetARPendingDate](ActionID) as PendingDate,&#13;&#10;CASE WHEN DATEPART(YEAR,FinishDate)>='1950'&#13;&#10;&#9;THEN  CONVERT(NVARCHAR(10),FinishDate,111) &#13;&#10;&#9;ELSE '' END AS FinishDate,&#13;&#10;CASE WHEN (CONVERT(NVARCHAR,fa.Status))='1' THEN 'False'&#13;&#10;ELSE 'True' END AS StatusVisible,&#13;&#10;ActionDescription,CompletionComment,fa.Status as StatusCode,&#13;&#10;CASE  WHEN AssignedTo =@editor THEN 'True'&#13;&#10;&#9; WHEN @lblreporter =@editor THEN 'True'&#13;&#10;&#9; WHEN @ProjectOwner='Y' THEN 'True'&#13;&#10;ELSE 'False' END as EditVisible&#13;&#10;FROM f_AR fa INNER JOIN &#13;&#10;m_maintain mm &#13;&#10;ON fa.Status=dbo.GetXml(Remark,'VALUE') AND mm.Type='DDL.Status'&#13;&#10;WHERE IssueID=@IssueID&#13;&#10;">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="txtUserId" Name="editor" PropertyName="Text" />
                                <asp:ControlParameter ControlID="lblReporterBadgeCode" Name="lblreporter" PropertyName="Text" />
                                <asp:ControlParameter ControlID="lblIssueOwnerBadgeCode" Name="ProjectOwner" PropertyName="Text" />
                                <asp:ControlParameter ControlID="lblIssueID" Name="IssueID" PropertyName="Text" />
                            </SelectParameters>
                        </asp:SqlDataSource></td>
            </tr>
            </table>
            </asp:View>
            
            <asp:View ID="vATF" runat="server">
                 <table width="100%" style="font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white; color: #333333; font-size: 10pt; border-top-style: groove; border-right-style: groove; border-left-style: groove; border-bottom-style: groove;" cellpadding="1" cellspacing="1">
                     <tr>
                          <td style="width: 125px; color: #333333; background-color: #b6cade">
                              <span style="color: #ff0000">*</span>File Descr</td>
                          <td align="left" style="color: #333333; font-family: 'Arial Unicode MS';
                                      background-color: #eff3f8">
                                  <asp:TextBox ID="txtFileDescr" runat="server" Width="100%"></asp:TextBox></td>
                      </tr>
                      <tr>
                          <td style="width: 125px; color: #333333; background-color: #b6cade;">
                                <span style="color: #ff0000">*</span>File Path</td>
                          <td style="color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8;" align="left">
                                <asp:FileUpload ID="fufile" runat="server" Width="90%"  />
                                <asp:Button ID="btnUpload" Text="Upload" runat="server" OnClick="btnUpload_Click"  Font-Bold="True" 
                                    Font-Names="Century Gothic" Width="60px"/></td>
                       </tr>
                  </table>
                     <asp:GridView ID="gvATF" runat="server" AutoGenerateColumns="False" 
                         BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" 
                         CellPadding="2" ForeColor="Black" 
                         Width="100%" DataSourceID="SqlDSAF" OnRowDeleting="gvATF_RowDeleting">
                         <FooterStyle BackColor="Tan" />
                         <Columns>
                             <asp:TemplateField ShowHeader="False">
                                 <ItemTemplate>
                                     <asp:LinkButton ID="lbtDel" runat="server" CausesValidation="False" CommandName="Delete" OnClientClick="return confirm('are you sure?')" Visible='<%# bool.Parse(Eval("EditVisible").ToString()) %>'
                                         Text="Del"></asp:LinkButton>
                                 </ItemTemplate>
                                 <ItemStyle Width="10%" />
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="File Descr.">
                                 <ItemTemplate>
                                     <asp:HyperLink ID="hyFileDesc" runat="server" Text='<%# Bind("FileDesc")%>' Target="_blank" NavigateUrl='<%# Bind("FilePath")%>'></asp:HyperLink>
                                     <asp:HiddenField ID="hfID" runat="server" Value='<%# Eval("ID") %>' />
                                     <asp:HiddenField ID="hfFileName" runat="server" Value='<%# Eval("FileName") %>' />
                                 </ItemTemplate>
                                 <ItemStyle Width="40%" />
                             </asp:TemplateField>
                             <asp:BoundField HeaderText="Editor" DataField="Uploader" >
                                 <ItemStyle Width="25%" />
                             </asp:BoundField>
                             <asp:BoundField HeaderText="Upload Date" DataField="UploadDate" >
                                 <ItemStyle Width="25%" />
                             </asp:BoundField>
                             <asp:TemplateField HeaderText="editor" SortExpression="editor" Visible="False">
                                 <ItemTemplate>
                                     <asp:Label ID="lblEditor" runat="server" Text='<%# Bind("editor") %>'></asp:Label>
                                 </ItemTemplate>
                             </asp:TemplateField>
                         </Columns>
                         <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                         <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                         <HeaderStyle BackColor="Tan" Font-Bold="True" />
                         <AlternatingRowStyle BackColor="PaleGoldenrod" />
                     </asp:GridView>
                <asp:SqlDataSource ID="SqlDSAF" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT ID,FilePath,dbo.fn_GetChtName(editor) as Uploader,cdt as UploadDate,FileName,&#13;&#10;dbo.GetXml(DESCRPTN,'FILE_DESC') AS FileDesc,udt,editor,&#13;&#10;CASE WHEN editor =@editor THEN 'True'&#13;&#10;&#9; WHEN @lblreporter =@editor THEN 'True'&#13;&#10;&#9; WHEN @ProjectOwner='Y' THEN 'True'&#13;&#10;ELSE 'False' END as EditVisible&#13;&#10;FROM AttachFile&#13;&#10;WHERE IssueID=@IssueID AND FileType='1'&#13;&#10;ORDER BY udt DESC">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtUserId" Name="editor" PropertyName="Text" />
                        <asp:ControlParameter ControlID="lblReporterBadgeCode" Name="lblreporter" PropertyName="Text" />
                        <asp:ControlParameter ControlID="lblIssueOwnerBadgeCode" Name="ProjectOwner" PropertyName="Text" />
                        <asp:ControlParameter ControlID="lblIssueID" Name="IssueID" PropertyName="Text" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:Label ID="lblExtType" runat="server" Visible="False" ></asp:Label></asp:View>
            
            <asp:View ID="vIssuePhoto" runat="server">
                 <table width="100%" style="font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white; color: #333333; font-size: 10pt; border-top-style: groove; border-right-style: groove; border-left-style: groove; border-bottom-style: groove;" cellpadding="1" cellspacing="1">
                     <tr>
                         <td style="width: 125px; color: #333333;  background-color: #b6cade">
                             <span style="color: #ff0000">*</span>Photo Descr</td>
                         <td align="left" style="color: #333333; font-family: 'Arial Unicode MS'; 
                             background-color: #eff3f8">
                             <asp:TextBox ID="txtPhotoDesc" runat="server" Width="100%"></asp:TextBox></td>
                     </tr>
                    <tr>
                        <td style="width: 125px; color: #333333;  background-color: #b6cade; height: 23px;">
                            <span style="color: #ff0000">*</span><asp:Label ID="lblIssuePhoto" runat="server" Text="Issue Photo"></asp:Label></td>
                        <td style="color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8; height: 23px;" 
                            align="left"><asp:FileUpload ID="fuphoto" runat="server" Width="90%"  />
                            <asp:Button ID="btnUploadPhoto" Text="Upload" runat="server" OnClick="btnUploadPhoto_Click"  Font-Bold="True" Font-Names="Century Gothic" 
                             Width="60px"/></td>
                    </tr>
                    </table>
                     <asp:Repeater ID="rpPhoto" runat="server" DataSourceID="SqlDSPhoto" OnItemDataBound="rpPhoto_ItemDataBound" OnItemCommand="rpPhoto_ItemCommand">
                        <AlternatingItemTemplate>
                            <div style="font-size: 9pt; font-family: 'Century Gothic'; clear: none; display: inline; float: left; visibility: visible; margin-left: 3px; margin-right: 3px;">
                                 <asp:Image ID="imgq1" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem,"FilePath") %>' Width="100px" Height="100px"></asp:Image><br />
                                 <asp:Label ID="lblPhotoDesc" runat="server" Text="Photo Descr:"></asp:Label><br />
                                 <asp:Label ID="lblPhotoDescr" runat="server" Text='<%# Bind("FileDesc") %>'></asp:Label><br />
                                 <asp:LinkButton  ID="hlForumDelete" CommandName="DelChart"  runat="server" Text="Del" Font-Size="10pt" ForeColor="SlateBlue" OnClientClick="return confirm('are you sure?')" Visible='<%# bool.Parse(Eval("EditVisible").ToString()) %>'></asp:LinkButton>
                                 <asp:HiddenField ID="hfid" runat="server" Value='<%# Eval("ID") %>' />
                                 <asp:HiddenField ID="hfFileName" runat="server" Value='<%# Eval("FileName") %>' />
                                 <asp:Label ID="lblEditor" runat="server" Text='<%# Eval("editor") %>' Visible="false"/>
                            </div>
                        </AlternatingItemTemplate>
                        <ItemTemplate>
                             <div style="font-size: 9pt; font-family: 'Century Gothic'; clear: none; display: inline; float: left; visibility: visible;" >
                                  <asp:Image ID="imgq1" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem,"FilePath") %>'  Width="100px" Height="100px"></asp:Image><br />
                                  <asp:Label ID="lblPhotoDesc" runat="server" Text="Photo Descr:"></asp:Label><br />
                                  <asp:Label ID="lblPhotoDescr" runat="server" Text='<%# Bind("FileDesc") %>'></asp:Label><br />
                                  <asp:LinkButton ID="hlForumDelete"  CommandName="DelChart" runat="server" Text="Del" Font-Size="10pt" ForeColor="SlateBlue" OnClientClick="return confirm('are you sure?')" Visible='<%# bool.Parse(Eval("EditVisible").ToString()) %>'></asp:LinkButton>
                                  <asp:HiddenField ID="hfid" runat="server" Value='<%# Eval("ID") %>' />
                                  <asp:HiddenField ID="hfFileName" runat="server" Value='<%# Eval("FileName") %>' />
                                  <asp:Label ID="lblEditor" runat="server" Text='<%# Eval("editor") %>' Visible="false"/>
                             </div> 
                        </ItemTemplate>
                    </asp:Repeater>
                <asp:SqlDataSource ID="SqlDSPhoto" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT ID,FilePath,dbo.fn_GetChtName(editor) as Uploader,cdt as UploadDate,FileName,&#13;&#10;dbo.GetXml(DESCRPTN,'FILE_DESC') AS FileDesc,udt,editor,&#13;&#10;CASE &#13;&#10;--WHEN Status='3' THEN 'False'&#13;&#10;WHEN editor =@editor THEN 'True'&#13;&#10;WHEN @lblreporter =@editor THEN 'True'&#13;&#10;WHEN @ProjectOwner='Y' THEN 'True'     &#13;&#10;ELSE 'False' END as EditVisible&#13;&#10;FROM AttachFile&#13;&#10;WHERE IssueID=@IssueID AND FileType='2'&#13;&#10;ORDER BY udt DESC">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtUserId" Name="editor" PropertyName="Text" />
                        <asp:ControlParameter ControlID="lblReporterBadgeCode" Name="lblreporter" PropertyName="Text" />
                        <asp:ControlParameter ControlID="lblIssueOwnerBadgeCode" Name="ProjectOwner" PropertyName="Text" />
                        <asp:ControlParameter ControlID="lblIssueID" Name="IssueID" PropertyName="Text" />
                    </SelectParameters>
                </asp:SqlDataSource>
           </asp:View>
            
            <asp:View ID="vHH" runat="server">
              <table width="100%" style="font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white; color: #333333; font-size: 10pt;" cellpadding="1" cellspacing="1">
                  <tr>
                      <td colspan="4" style="text-align:center; height: 139px;">
                          <table width="100%" style="font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white; color: #333333; border-top-style: groove; border-right-style: groove; border-left-style: groove; border-bottom-style: groove;" cellpadding="1" cellspacing="1">
                              <tr>
                                  <td style="width: 16%; color: #333333;  background-color: #b6cade;" align="left">
                                      <span style="color: #ff0000">*</span><asp:Label ID="lblDate" runat="server" Text="Date"></asp:Label></td>
                                  <td style="width: 18%; color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8;" align="left">
                                      <asp:TextBox ID="txtDate" runat="server" Width="80%" ></asp:TextBox>
                                      <img id="Img5" alt="選取日期" runat="server" onclick="SPM_OpenCalendar(document.all('txtDate').name, false)" src="images/calendar.gif"/></td>
                                  <td style="width: 16%;  background-color: #b6cade;" align="left">
                                      Line</td>
                                  <td style="width: 17%"><asp:DropDownList ID="ddlLine" runat="server" Width="100%" DataSourceID="SqlDSLine" DataTextField="LineName" DataValueField="LineID">
                                      </asp:DropDownList>
                                      <asp:SqlDataSource ID="SqlDSLine" runat="server" 
                                          ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="select ''as LineID,'-' as LineName,'' as [Rank]
UNION
select CONVERT(NVARCHAR,LineID),LineName,CONVERT(NVARCHAR,[Rank])
from m_SiteLine
where SiteID=@SiteID AND IsInUse=1 ORDER BY [Rank]">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="lblSiteID" Name="SiteID" PropertyName="Text" />
                                        </SelectParameters>
                                      </asp:SqlDataSource></td>
                                  <td style="width: 16%;  background-color: #b6cade;" align="left">
                                      Shift</td>
                                  <td style="width: 17%"><asp:DropDownList ID="ddlShift" runat="server" Width="100%" DataSourceID="SqlDSShift" DataTextField="ShiftName" DataValueField="ShiftID">
                                       </asp:DropDownList>
                                       <asp:SqlDataSource ID="SqlDSShift" runat="server" 
                                          ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="select ''as ShiftID,'-' as ShiftName,'' as [Rank]
UNION
select CONVERT(NVARCHAR,ShiftID),ShiftName,CONVERT(NVARCHAR,[Rank])
from m_SiteShift
where SiteID=@SiteID AND IsInUse=1 ORDER BY [Rank]">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="lblSiteID" Name="SiteID" PropertyName="Text" />
                                        </SelectParameters>
                                      </asp:SqlDataSource></td>
                              </tr>
                              <tr>
                                  <td align="left" style="width: 16%; color: #333333; background-color: #b6cade">
                                      <span style="color: #ff0000">*</span>Input Qty</td>
                                  <td align="left" style="width: 18%; color: #333333; font-family: 'Arial Unicode MS';
                                      background-color: #eff3f8">
                                        <asp:TextBox ID="txtInputQty" runat="server" Width="100%"></asp:TextBox></td>
                                  <td align="left" style="width: 16%; background-color: #b6cade">
                                      <span style="color: #ff0000">*</span><asp:Label ID="lblFailQty" runat="server" ForeColor="Black">Failure Qty</asp:Label></td>
                                  <td style="width: 17%">
                                          <asp:TextBox ID="txtFailQty" runat="server" Width="99%"></asp:TextBox></td>
                                  <td align="left" style="width: 16%; background-color: #b6cade">
                                  </td>
                                  <td style="width: 17%">
                                  </td>
                              </tr>
                              <tr>
                                  <td style="width: 16%; color: #333333; background-color: #b6cade;" align="left">
                                      <span style="color: #ff0000">*</span><asp:Label ID="lblHandlingNote" runat="server">Handling Note</asp:Label></td>
                                  <td colspan="5">
                                        <asp:TextBox ID="txtHandlingNote" runat="server" Width="100%" TextMode="MultiLine"></asp:TextBox></td>
                              </tr>
                              <tr>
                                  <td align="center" colspan="4">
                                    <asp:Button ID="btnAddHH" runat="server" Text="Save" OnClick="btnAddHH_Click"  Font-Bold="True" 
                                        Font-Names="Century Gothic" Width="60px"/>
                                    <asp:Button ID="btnCancelHH" runat="server" OnClick="btnCancelHH_Click" Text="Cancel" Font-Bold="True" Font-Names="Century Gothic" 
                                    Width="60px" /></td>
                              </tr>
                          </table>
                       </td>
                  </tr>
                </table>
                 <asp:GridView ID="gvHH" runat="server" CellPadding="4" ForeColor="Black" Width="100%"
                            GridLines="Vertical" BackColor="White" BorderColor="#DEDFDE" 
                            BorderStyle="None" BorderWidth="1px" AutoGenerateColumns="False" DataSourceID="SqlDSHH" OnRowDeleting="gvHH_RowDeleting" OnSelectedIndexChanged="gvHH_SelectedIndexChanged" DataKeyNames="HandingID" >
                            <FooterStyle BackColor="#CCCC99" />
                            <RowStyle BackColor="#F7F7DE" />
                            <Columns>
                                <asp:TemplateField ShowHeader="False" >
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnSel" runat="server" CausesValidation="False" CommandName="Select"  Visible='<%# bool.Parse(Eval("EditVisible").ToString()) %>'
                                            Text="Edit"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" Font-Size="10pt"/>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnDel" runat="server" CausesValidation="False" CommandName="Delete" OnClientClick="return confirm('are you sure?')" Visible='<%# bool.Parse(Eval("EditVisible").ToString()) %>'
                                            Text="Del"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" Font-Size="10pt" /> 
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date">
                                   <ItemTemplate>
                                        <asp:Label ID="lblHandingDate" runat="server" Text='<%# Bind("HandingDate") %>'></asp:Label>
                                        <asp:HiddenField ID="hfHandingID" runat="server" Value='<%# Bind("HandingID") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" Font-Size="10pt" />
                                    <HeaderStyle Font-Size="10pt" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Line">
                                  <ItemTemplate>
                                        <asp:Label ID="lblSiteLine" runat="server" Text='<%# Bind("SiteLine") %>'></asp:Label>
                                        <asp:HiddenField ID="hfSiteLine" runat="server" Value='<%# Bind("SiteLineCode") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%"  Font-Size="10pt"/>
                                    <HeaderStyle Font-Size="10pt" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Shift">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSiteShift" runat="server" Text='<%# Bind("SiteShift") %>'></asp:Label>
                                        <asp:HiddenField ID="hfSiteShift" runat="server" Value='<%# Bind("SiteShiftCode") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" Font-Size="10pt" />
                                    <HeaderStyle Font-Size="10pt" />
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Input Qty">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInputQty" runat="server" Text='<%# Bind("InputQty") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%"  Font-Size="10pt"/>
                                    <HeaderStyle Font-Size="10pt" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Failure Qty">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFailureQty" runat="server" Text='<%# Bind("FailureQty") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="13%"  Font-Size="10pt"/>
                                    <HeaderStyle Font-Size="10pt" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Failure Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFailureRate" runat="server" Text='<%# Bind("FailureRate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="13%"  Font-Size="10pt"/>
                                    <HeaderStyle Font-Size="10pt" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Handling Note">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHandlingNote" runat="server" Text='<%# Bind("HandingNote") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="30%"  Font-Size="10pt"/>
                                    <HeaderStyle Font-Size="10pt" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="editor" SortExpression="editor" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEditor" runat="server" Text='<%# Bind("editor") %>'></asp:Label>
                                    </ItemTemplate>
                                 </asp:TemplateField>
                            </Columns>
                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                            <SelectedRowStyle BackColor="#C0FFFF" Font-Bold="True" ForeColor="Sienna" />
                            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>                    
                <asp:SqlDataSource ID="SqlDSHH" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="&#13;&#10;SELECT HandingID,IssueID,CONVERT(NVARCHAR(10),HandingDate,111) as HandingDate,&#13;&#10;InputQty,FailureQty,HandingNote,editor,SiteLine as SiteLineCode,SiteShift as SiteShiftCode,&#13;&#10;[dbo].[fn_GetSiteLineName](SiteLine) as SiteLine,&#13;&#10;[dbo].[fn_GetSiteShiftName](SiteShift) as SiteShift,&#13;&#10;[dbo].[fn_GetTop1HandingValue](HandingID,'EachRate')+'%'  as FailureRate,&#13;&#10;CASE WHEN editor =@editor THEN 'True'&#13;&#10;&#9; WHEN @lblreporter =@editor THEN 'True'&#13;&#10;&#9; WHEN @ProjectOwner='Y' THEN 'True'&#13;&#10;ELSE 'False' END as EditVisible&#13;&#10;FROM f_HandingHistory&#13;&#10;WHERE IssueID=@IssueID&#13;&#10;ORDER BY HandingDate">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtUserId" Name="editor" PropertyName="Text" />
                        <asp:ControlParameter ControlID="lblReporterBadgeCode" Name="lblreporter" PropertyName="Text" />
                        <asp:ControlParameter ControlID="lblIssueOwnerBadgeCode" Name="ProjectOwner" PropertyName="Text" />
                        <asp:ControlParameter ControlID="lblIssueID" Name="IssueID" PropertyName="Text" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:Label ID="lblHandingID" runat="server" Visible="False"></asp:Label></asp:View>
            
            <asp:View ID="vrdc" runat="server">
                <div style="WIDTH: 100%; TEXT-ALIGN: left">
                <asp:Button ID="btnRepeatDefect" runat="server" Text="Repeat Defect Check" 
                        CausesValidation="False" onclick="btnRepeatDefect_Click"  Font-Bold="True" Font-Names="Century Gothic" 
                             Width="145px" /><br />
                <asp:Button id="btnConfirm" runat="server" Text="Confirm" Visible="False"></asp:Button><br />
                    <asp:GridView ID="gvRepeatDefectCheck" runat="server" AutoGenerateColumns="False" CellPadding="2"
                        ForeColor="#333333" GridLines="Vertical" Width="100%"  OnRowDeleting="gvRepeatDefectCheck_RowDeleting">
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnDel" runat="server" CausesValidation="False" CommandName="Delete" OnClientClick="return confirm('are you sure?')" 
                                        Text="Del"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IssueSN">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lblIssueID" runat="server" NavigateUrl='<%# Bind("URL") %>'
                                        Target="_blank" Text='<%# Bind("IssueSN")%>'></asp:HyperLink>
                                    <asp:HiddenField ID="hfIssueID" runat="server" Value='<%# Bind("IssueID") %>' />
                                </ItemTemplate>
                                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                                <ItemStyle Font-Size="8pt" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Station" HeaderText="Station">
                                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                                <ItemStyle Font-Size="8pt" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SN" HeaderText="S/N">
                                <HeaderStyle Font-Size="8pt" />
                                <ItemStyle Font-Size="8pt" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DefectSymptom" HeaderText="Defect Symptom">
                                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                                <ItemStyle Font-Size="8pt" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FailureQty" HeaderText="Failed Qty">
                                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                                <ItemStyle Font-Size="8pt" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FailureRate" HeaderText="Failure Rate">
                                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                                <ItemStyle Font-Size="8pt" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Location" HeaderText="Location">
                                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                                <ItemStyle Font-Size="8pt" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DefectComponentPN" HeaderText="P/N">
                                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                                <ItemStyle Font-Size="8pt" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PackageType" HeaderText="Package Type">
                                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                                <ItemStyle Font-Size="8pt" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Liability" HeaderText="Liability">
                                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                                <ItemStyle Font-Size="8pt" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PendingDate" HeaderText="PendingDate">
                                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                                <ItemStyle Font-Size="8pt" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="Status">
                                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                                <ItemStyle Font-Size="8pt" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Priority" HeaderText="Priority">
                                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                                <ItemStyle Font-Size="8pt" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RelatedIssue" HeaderText="Repeat Defect?">
                                <HeaderStyle Font-Size="8pt" HorizontalAlign="Left" />
                                <ItemStyle Font-Size="8pt" />
                            </asp:BoundField>
                        </Columns>
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDSRepeatDefect" runat="server"></asp:SqlDataSource>
              </div>
            </asp:View>
            <asp:View ID="vNote" runat="server">
                <table width="100%" style="font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white; color: #333333; font-size: 10pt; border-top-style: groove; border-right-style: groove; border-left-style: groove; border-bottom-style: groove;" cellpadding="1" cellspacing="1">
                <tr>
                    <td style="width: 10%; background-color: #b6cade">
                        <span style="color: #ff0000">*</span>Note</td>
                     <td style="width: 80%">
                         <asp:TextBox ID="txtNote" runat="server" Width="100%" TextMode="MultiLine"></asp:TextBox></td>
                     <td style="width: 10%; text-align: center">
                         <asp:Button ID="btnAddNote" runat="server" Text="Save" OnClick="btnAddNote_Click"  Font-Bold="True" Font-Names="Century Gothic" 
                             Width="60px"/></td>
                </tr>
                </table>
                <asp:GridView ID="gvNote" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%" DataSourceID="SqlDSNote" OnRowDeleting="gvNote_RowDeleting" OnRowUpdating="gvNote_RowUpdating">
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <EditItemTemplate>
                                <asp:LinkButton ID="lbtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                    Text="Update"></asp:LinkButton>
                                <asp:LinkButton ID="lbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                    Text="Cancel"></asp:LinkButton>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnEdit" runat="server" CausesValidation="False" CommandName="Edit" Visible='<%# bool.Parse(Eval("EditVisible").ToString()) %>'
                                    Text="Edit"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="5%" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnDel" runat="server" CausesValidation="False" CommandName="Delete" OnClientClick="return confirm('are you sure?')" Visible='<%# bool.Parse(Eval("EditVisible").ToString()) %>'
                                    Text="Del"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="5%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Note">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtNote" runat="server" TextMode="MultiLine" Width="100%" Text='<%# Bind("Note") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblNote" runat="server" Text='<%# Bind("Note") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="50%" Font-Size="10pt" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Editor">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Editor") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="20%" Font-Size="10pt" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Update Date">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("udt") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="20%" Font-Size="10pt" />
                        </asp:TemplateField>
                         <asp:TemplateField  Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblNoteID" runat="server" Text='<%# Bind("NoteID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="editorBadgeCode" SortExpression="editorBadgeCode"
                            Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblEditor" runat="server" Text='<%# Bind("editorBadgeCode") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="Gainsboro" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDSNote" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT NoteID,IssueID,Note,dbo.fn_GetChtName(editor) as Editor,udt,editor as editorBadgeCode,&#13;&#10;CASE WHEN editor =@editor THEN 'True'&#13;&#10; WHEN @lblreporter =@editor THEN 'True'&#13;&#10; WHEN @ProjectOwner='Y' THEN 'True'&#13;&#10;ELSE 'False' END as EditVisible&#13;&#10;FROM dbo.f_Note&#13;&#10;WHERE IssueID=@IssueID&#13;&#10;ORDER BY udt DESC&#13;&#10;">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtUserId" Name="editor" PropertyName="Text" />
                        <asp:ControlParameter ControlID="lblReporterBadgeCode" Name="lblreporter" PropertyName="Text" />
                        <asp:ControlParameter ControlID="lblIssueOwnerBadgeCode" Name="ProjectOwner" PropertyName="Text" />
                        <asp:ControlParameter ControlID="lblIssueID" Name="IssueID" PropertyName="Text" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </asp:View>
         </asp:MultiView>
            <asp:SqlDataSource ID="SqlDSMPIssue" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="sp_NPIIssue" SelectCommandType="StoredProcedure">
            <SelectParameters>
            <asp:ControlParameter ControlID="lblvchCmd" Name="vchCmd" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="lblObjectName" Name="vchObjectName" PropertyName="Text"
            Type="String" />
            <asp:SessionParameter Name="vchSet" SessionField="vchSet" Type="String" />
            </SelectParameters>
            </asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlDSMPDetailIssues" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT * FROM [v_MPIssue_ItemMode] WHERE IssueID=@IssueID">
            <SelectParameters>
            <asp:ControlParameter ControlID="lblIssueID" Name="IssueID" PropertyName="Text" />
            </SelectParameters>
            </asp:SqlDataSource>
            <div id="divParameters" runat="server" visible="false">
            <asp:Label ID="lblARBadgeCode" runat="server"></asp:Label><br />
            <asp:Label ID="lblActionID" runat="server" ></asp:Label><br />
            <asp:Label ID="lblvchCmd" runat="server" Text="SELECT"></asp:Label><br />
            <asp:Label ID="lblObjectName" runat="server" Text="LISTISSUE"></asp:Label><br />
            <asp:Label ID="lblIssueID" runat="server"></asp:Label><asp:TextBox ID="txtUserId" runat="server"></asp:TextBox><br />
            <asp:Label ID="lblStatusCode" runat="server"></asp:Label><br />
            <asp:Label ID="lblProjectID" runat="server"></asp:Label><br />
            <asp:Label ID="lblReporterBadgeCode" runat="server"></asp:Label><br />
            <asp:Label ID="lblIssueOwnerBadgeCode" runat="server"></asp:Label><br />
            <asp:HiddenField ID="hfProjectMaterial" runat="server" />
            <asp:HiddenField ID="hfMType" runat="server" />
            <asp:Label ID="lblSiteID" runat="server"></asp:Label>
            </div>      
        </div>
        </div>
        </div>
        </div>
        </div>
    </div>
    </form>
</body>
</html>
