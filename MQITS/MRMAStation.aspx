<%@ Page Title="" Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MRMAStation.aspx.cs" Inherits="MRMAStation" MaintainScrollPositionOnPostback="true" Theme="MMContainer" %>
<%@ Register src="MMaintain.ascx" tagname="MMaintain" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
    <uc1:MMaintain ID="MMaintain1" runat="server" UserFunction="RMAStation"
        UserViewTable = "v_RMAStation" UserViewTitle = "RMA Station" UserObjectType="DDL.RMAStation"/>
</asp:Content>
