<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MMIssueStatus.aspx.cs" Inherits="MMIssueStatus" Title="=== MQITS::Maintain Issue Status ===" %>

<%@ Register src="MMaintain.ascx" tagname="MMaintain" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
    <uc1:MMaintain ID="MMaintain1" runat="server" UserFunction="IssueStatus" />
</asp:Content>

