<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MMLiability.aspx.cs" Inherits="MMLiability" Title="=== MQITS::Maintain Liability ===" %>

<%@ Register src="MMaintain.ascx" tagname="MMaintain" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
<div style="width:100%">
    <uc1:MMaintain ID="MMaintain1" runat="server" UserFunction="Liability" />
</div>
</asp:Content>

