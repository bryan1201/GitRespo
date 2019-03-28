<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MPDraft.aspx.cs" Inherits="MPDraft" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>MPDraft</title>
</head>
<body>
    <form id="form1" runat="server">
     <asp:SqlDataSource ID="SqlDSDetail" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT * FROM [v_MPImportIssue] WHERE IssueID=@IssueID">
            <SelectParameters>
                <asp:QueryStringParameter Name="IssueID" QueryStringField="IssueID" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:FormView ID="fvIssue" runat="server" CellPadding="4" DataSourceID="SqlDSDetail" Font-Names="Arial Unicode MS" Font-Size="10pt" ForeColor="#333333" OnDataBound="fvIssue_DataBound" >
           <ItemTemplate>
                <div style="width: 902px; background-color: #E3EAEB; text-align: left;">
                </div>
                    <div style="width: 100%; border-right: firebrick 2px solid; border-top: firebrick 2px solid;
                        border-left: firebrick 2px solid; border-bottom: firebrick 2px solid; text-align: left;">
                         <table width="100%" style="font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white; color: #333333;" cellpadding="1" cellspacing="1">
                        <tr>
                            <td style="width: 20%; color: #333333;  background-color: #b6cade; height: 18px;">
                                <asp:Label ID="lblCustomer" runat="server">Customer</asp:Label></td>
                             <td style="WIDTH: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8; height: 18px;">
                                 <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName", "{0}") %>'></asp:Label></td>
                             <td style="width: 20%;  background-color: #b6cade; height: 18px;">
                                <asp:Label ID="lblReporter" runat="server">Reporter</asp:Label></td>
                            <td style="width: 30%; color: #333333; background-color: #eff3f8; height: 18px;">
                                <asp:Label ID="lblReportName" runat="server" Text='<%# Bind("Reporter", "{0}") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 20%; color: #333333;  background-color: #b6cade;">
                                 <asp:Label ID="lblProject" runat="server">Project</asp:Label></td>
                            <td style="WIDTH: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                                <asp:Label ID="lblProjectName" runat="server" Text='<%# Bind("ProjectName", "{0}") %>'></asp:Label></td>
                            <td style="width: 20%; background-color: #b6cade;">
                                <asp:Label ID="lblOwner" runat="server" >Reporter</asp:Label></td>
                            <td style="width: 30%; color: #333333;  background-color: #eff3f8;">
                                <asp:Label ID="lblIssueOwnerName" runat="server" Text='<%# Bind("IssueOwnerName", "{0}") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 20%; color: #333333; background-color: #b6cade;">
                                <asp:Label ID="lblPCACPU" runat="server">PCA P/N or CPU</asp:Label></td>
                            <td style="WIDTH: 30%; color: #333333; font-family: 'Arial Unicode MS';  background-color: #eff3f8;">
                                <asp:Label ID="lblMaterialTypeName" runat="server" Text='<%# Bind("MaterialType", "{0}") %>'></asp:Label></td>
                             <td style="width: 20%;  background-color: #b6cade;">
                                <asp:Label ID="lblStatus" runat="server">Status</asp:Label></td>
                            <td style="width: 30%; color: #333333;  background-color: #eff3f8;">
                                <asp:Label ID="lblStatusName" runat="server" Text='<%# Bind("StatusName", "{0}") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 20%; color: #333333;  background-color: #b6cade">
                               <asp:Label ID="lblStation" runat="server">Station</asp:Label></td>
                            <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8">
                                <asp:Label ID="lblStationName" runat="server" Text='<%# Bind("StationName", "{0}") %>'></asp:Label></td>
                           <td style="width: 20%; color: #333333; background-color: #b6cade">
                                <asp:Label ID="lblIssueDate" runat="server">Issue Date</asp:Label></td>
                            <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8">
                                <asp:Label ID="lblIssueDateName" runat="server" Text='<%# Bind("IssueDate", "{0}") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 20%; color: #333333; background-color: #b6cade">
                                <asp:Label ID="lblLiability" runat="server">Liability</asp:Label></td>
                            <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8">
                                <asp:Label ID="lblLiabilityName" runat="server" Text='<%# Bind("LiabilityName", "{0}") %>'></asp:Label></td>
                            <td style="width: 20%; background-color: #b6cade">
                                <asp:Label ID="lblPriority" runat="server" >Priority</asp:Label></td>
                            <td style="width: 30%; color: #333333; background-color: #eff3f8">
                                <asp:Label ID="lblPriorityName" runat="server" Text='<%# Bind("PriorityName", "{0}") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 20%; color: #333333;  background-color: #b6cade;">
                                <asp:Label ID="lblSerialNo" runat="server" >S/N</asp:Label><br />
                                (PCA or CPU S/N)</td>
                            <td colspan="4" style="color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8; width: 80%;">
                                <asp:Label ID="lblSNName" runat="server" Text='<%# Bind("SerialNo", "{0}") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 20%; color: #333333; background-color: #b6cade; border-top-width: 1px; border-left-width: 1px; border-left-color: white; border-bottom-width: 1px; border-bottom-color: white; border-top-color: white; border-right-width: 1px; border-right-color: white;">
                                <asp:Label ID="lblDefectSympton" runat="server"></asp:Label></td>
                            <td colspan="4" style="color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8; width: 80%;">
                                <asp:Label ID="lblDefectName" runat="server" Text='<%# Bind("DefectSymptom", "{0}") %>'></asp:Label></td>
                        </tr>
                        </table>
                        
                        <asp:Panel ID="panelPCA" runat="server" Font-Size="10pt" Width="100%" Visible="false" BorderColor="White">
                        <table style="border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; width: 100%; color: #333333; border-bottom: white 1px solid; font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white" cellpadding="0" cellspacing="0">  
                        <tr>
                            <td style="width: 20%; color: #333333; background-color: #b6cade">
                                <asp:Label ID="lblLocation" runat="server">Location</asp:Label>(For PCA Only)</td>
                            <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8; border-bottom: white 2px solid;">
                                <asp:Label ID="lblLocationName" runat="server" Text='<%# Bind("Location", "{0}") %>'></asp:Label></td>                           
                        </tr>
                        <tr>
                            <td style="width: 20%; color: #333333;background-color: #b6cade; border-top: white 1px solid;">
                                <asp:Label ID="lblDefectPartNo" runat="server">Defective Component P/N</asp:Label><br />
                                (For PCA Only)</td>
                            <td colspan="4" style="color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8; width: 80%;">
                                <asp:Label ID="lblDefectivePNName" runat="server" Text='<%# Bind("DefectComponentPN", "{0}") %>'></asp:Label></td>
                        </tr>
                        </table>
                        </asp:Panel>
                        
                        <asp:Panel ID="panelCPU" runat="server" Font-Size="10pt" Width="100%" Visible="false">
                        <table style="border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; width: 100%; color: #333333; border-bottom: white 1px solid; font-family: 'Arial Unicode MS'; border-collapse: collapse; background-color: white" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 20%; color: #333333; background-color: #b6cade; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid;">
                                <asp:Label ID="lblCommdity" runat="server">Faulty Commodity</asp:Label><br />
                                For CPU Only)</td>
                            <td style="width: 30%; color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8; border-bottom: white 2px solid;">
                                <asp:Label ID="lblFaultyCommodityName" runat="server" Text='<%# Bind("FaultCommodity", "{0}") %>'></asp:Label></td>
                            <td style="width: 20%;  background-color: #b6cade; border-bottom: white 2px solid;">
                                <asp:Label ID="Label4" runat="server">Faulty Commodity S/N</asp:Label><br />
                                (For CPU Only)</td>
                            <td style="width: 30%; color: #333333;  background-color: #eff3f8; border-bottom: white 2px solid;">
                                <asp:Label ID="lblFaultyCommoditySNName" runat="server" Text='<%# Bind("FaultCommoditySN", "{0}") %>'></asp:Label></td>
                        </tr>      
                        <tr>
                            <td style="width: 20%; color: #333333; background-color: #b6cade; border-top-width: 1px; border-left-width: 1px; border-left-color: white; border-bottom-width: 1px; border-bottom-color: white; border-top-color: white; border-right-width: 1px; border-right-color: white;">
                                <asp:Label ID="lblCommodityPN" runat="server">Faulty Commodity P/N</asp:Label>
                                (For CPU Only)</td>
                            <td colspan="4" style="width: 80%; color: #333333; font-family: 'Arial Unicode MS'; background-color: #eff3f8">
                                <asp:Label ID="lblFaultyCommodityPNName" runat="server" Text='<%# Bind("FaultCommodityPN", "{0}") %>'></asp:Label></td>
                        </tr>
                        </table>
                        </asp:Panel>                      
                    </div>
            </ItemTemplate>
            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        </asp:FormView>
    </form>
</body>
</html>
