<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ListMPPCADetail.aspx.cs" Inherits="ListMPPCADetail" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ListPCADetail</title>
</head>
<body style="font-family:Arial Unicode MS;">
    <form id="form1" runat="server">
    <div style="font-family:Arial Unicode MS;">
    <rsweb:ReportViewer ID="rptDetail" runat="server" Font-Names="Arial Unicode MS" Font-Size="9pt"
            Height="600px" Width="100%" SizeToReportContent="True">
        <LocalReport ReportPath="rptMPPCADetail.rdlc" EnableHyperlinks="True">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="SqlDSPCAListDetail" 
                    Name="DSMPPCADetail_DataTable1" />
                <rsweb:ReportDataSource DataSourceId="SqlDSLiability" 
                    Name="DSPCALiability_DataTable1" />
            </DataSources>
        </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
            SelectMethod="GetData" 
            TypeName="DSPCALiabilityTableAdapters.DataTable1TableAdapter">
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            SelectMethod="GetData" 
            TypeName="DSMPPCADetailTableAdapters.DataTable1TableAdapter">
        </asp:ObjectDataSource>
        <asp:SqlDataSource ID="SqlDSLiability" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>"
                    SelectCommand=""></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDSPCAListDetail" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>"
        SelectCommand=""></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDSCPUListDetail" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>"
        SelectCommand=""></asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
