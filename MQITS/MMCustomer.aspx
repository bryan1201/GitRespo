<%@ Page Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MMCustomer.aspx.cs" Inherits="MMCustomer" Title="=== MQITS::Maintain Customer ===" %>
<%@ Register src="MMaintain.ascx" tagname="MMaintain" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
    
    <uc1:MMaintain ID="MMaintain1" runat="server" UserFunction="Customer"  />
    
</asp:Content>

