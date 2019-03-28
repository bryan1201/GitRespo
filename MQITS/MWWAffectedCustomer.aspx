<%@ Page Title="" Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MWWAffectedCustomer.aspx.cs" Inherits="MWWAffectedCustomer" MaintainScrollPositionOnPostback="true" Theme="MMContainer" %>
<%@ Register src="MMaintain.ascx" tagname="MMaintain" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
    <uc1:MMaintain ID="MMaintain1" runat="server" UserFunction="WWAffectedCustomer"
        UserViewTable = "v_WWAffectedCustomer" UserViewTitle = "WW Affected Customer" UserObjectType="DDL.WWAffectedCustomer"/>
</asp:Content>

