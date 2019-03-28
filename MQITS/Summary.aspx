<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="Summary.aspx.cs" Inherits="Summary" Title="=== MQITS::Summary ===" MaintainScrollPositionOnPostback="true" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
    <table style="text-align:left; border-right: #b6cade 2px solid; border-top: #b6cade 2px solid; border-left: #b6cade 2px solid; border-bottom: #b6cade 2px solid; width: 100%; float: none; clear: both; font-size: 10pt; color: #333333; font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white;" cellpadding="1" cellspacing="1">
        <tr>
            <td style="width: 60px; background-color: #b6cade;">
                Customer</td>
            <td style="width: 15%; background-color: #eff3f8">
                <asp:DropDownList ID="ddlCustomer" runat="server" Width="100%" 
                    DataSourceID="SqlDSCustomer" DataTextField="CustomerName" 
                    DataValueField="CustomerID" Font-Names="Palatino Linotype">
                </asp:DropDownList>
            </td>
            <td style="width: 3px; background-color: #eff3f8;">
                &nbsp;</td>
            <td style="width: 60px; background-color: #b6cade;">
                Site</td>
            <td style="width: 15%; background-color: #eff3f8">
                <asp:DropDownList ID="ddlSite" runat="server" Width="100%" 
                    DataSourceID="SqlDSSite" DataTextField="SiteName" DataValueField="SiteID" 
                    Font-Names="Palatino Linotype">
                </asp:DropDownList></td>            
                <td align="left" style="background-color: #eff3f8; width: 3px;">
                    &nbsp;</td>
                <td align="left" style="text-align: center; background-color: #eff3f8; font-family: 'Arial Unicode MS';">
                <asp:Button ID="btnQry" runat="server" Text="Query" Width="100px" 
                        OnClick="btnQry_Click" Font-Names="Palatino Linotype" />
                    &nbsp;
                <asp:Button ID="btnAddIssue" runat="server" Text="Report Issue" Width="100px" 
                        OnClick="btnAddIssue_Click" Visible="False" 
                        Font-Names="Palatino Linotype" />
                <asp:Button ID="btnExport" runat="server" Text="Export Report" Width="100px" 
                        Visible="False" Font-Names="Palatino Linotype" />&nbsp;
                </td>
        </tr>
        </table>
        <table style="width:100%">
            <tr>
                <td style="border: 2px solid #0000FF; vertical-align:top; padding-bottom: 30px;">
                    <rsweb:ReportViewer ID="rptPCASummary" runat="server" 
                    Font-Names="Verdana" Font-Size="8pt" Width="100%" BorderColor="#9900FF" 
                     BorderStyle="Solid" BorderWidth="1px" Height="400px">
                        <LocalReport ReportPath="rptPCASummary.rdlc" EnableHyperlinks="True">
                            <DataSources>
                                <rsweb:ReportDataSource DataSourceId="SqlDSReport" Name="DSPCA_DataTable1" />
                            </DataSources>
                        </LocalReport>
                    </rsweb:ReportViewer>

                </td>
            </tr>
            <tr style=" height:5px; line-height:1px;"><td>&nbsp;</td></tr>
            <tr>
                <td style="border: 2px solid #800000;vertical-align:top; padding-bottom: 30px;">
                    <rsweb:ReportViewer ID="rptCPUSummary" runat="server" Font-Names="Verdana" 
                        Font-Size="8pt" Width="100%" BorderColor="#CC6600" BorderStyle="Solid" 
                          BorderWidth="1px" Height="400px">
                        <LocalReport ReportPath="rptCPUSummary.rdlc" EnableHyperlinks="True">
                            <DataSources>
                                <rsweb:ReportDataSource DataSourceId="SqlDSCPU" Name="DSCPU_v_ProjectPhase" />
                            </DataSources>
                        </LocalReport>
                    </rsweb:ReportViewer>
                </td>
            </tr>
        </table>
 
<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
    SelectMethod="GetData" TypeName="DSPCATableAdapters.DataTable1TableAdapter">
</asp:ObjectDataSource>

<asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
        SelectMethod="GetData" TypeName="DSCPUTableAdapters.v_ProjectPhaseTableAdapter">
    </asp:ObjectDataSource>
      <asp:SqlDataSource ID="SqlDSCustomer" runat="server" 
        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
        SelectCommand="SELECT  CustomerID, CustomerName FROM [v_Customer]"></asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDSSite" runat="server" 
        ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="/*
select DISTINCT g.GroupID as SiteID,g.Title as SiteName
from m_Group g INNER JOIN m_Group m
on g.GroupID=dbo.GetXml(m.Remark,'SITE')
WHERE g.Parent_GroupID='40' AND m.GroupID=ProjectID
*/

SELECT  SiteID, SiteName FROM [v_Site] ">
            </asp:SqlDataSource>
      <asp:SqlDataSource ID="SqlDSCPU" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
        SelectCommand="">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlCustomer" Name="Customer" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="ddlSite" Name="Site" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDSPCADetail" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>"
                    SelectCommand="select vl.TEXT AS Liability,COUNT(Status) as Amount,&#13;&#10;&#9;&#9;vs.TEXT as StatusName&#13;&#10;from f_common f&#13;&#10;INNER JOIN v_IssueLiability vl on f.Liability =vl.VALUE&#13;&#10;INNER JOIN v_IssueStatus vs on f.Status=vs.VALUE&#13;&#10;WHERE Customer=@Customer AND Site=@Site AND Phase=@Phase AND MaterialType=@Material&#13;&#10;GROUP BY vl.TEXT,Status,vs.TEXT">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlCustomer" Name="Customer" PropertyName="SelectedValue" />
                        <asp:ControlParameter ControlID="ddlSite" Name="Site" PropertyName="SelectedValue" />
                        <asp:Parameter Name="Phase" />
                        <asp:Parameter Name="Material" />
                    </SelectParameters>
                </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDSReport" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
        SelectCommand="">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlCustomer" Name="Customer" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="ddlSite" Name="Site" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    
</asp:Content>