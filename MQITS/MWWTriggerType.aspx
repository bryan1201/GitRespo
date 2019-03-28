<%@ Page Title="" Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MWWTriggerType.aspx.cs" Inherits="MWWTriggerType" MaintainScrollPositionOnPostback="true" Theme="MMContainer" %>
<%@ Register src="MMaintain.ascx" tagname="MMaintain" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
    <uc1:MMaintain ID="MMaintain1" runat="server" UserFunction="WWTriggerType"
        UserViewTable = "v_WWTriggerType" UserViewTitle = "WW Trigger Type" UserObjectType="DDL.WWTriggerType"/>
</asp:Content>

