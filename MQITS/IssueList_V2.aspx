<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="IssueList_V2.aspx.cs" Inherits="IssueList_V2" Title="=== MQITS::Issue List ===" MaintainScrollPositionOnPostback="true" %>
<%@ Register src="QueryIssue_V2.ascx" tagname="QueryIssue_V2" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
    <uc1:QueryIssue_V2 ID="QueryIssue_V21" runat="server" />
</asp:Content>

