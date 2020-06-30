<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="IssueList.aspx.cs" Inherits="IssueList" Title="=== MQITS::Issue List ===" %>
<%@ Register src="QueryIssue.ascx" tagname="QueryIssue" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
    <uc1:QueryIssue ID="QueryIssue1" runat="server" />
</asp:Content>

