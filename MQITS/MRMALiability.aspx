<%@ Page Title="" Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MRMALiability.aspx.cs" Inherits="MRMALiability" MaintainScrollPositionOnPostback="true" Theme="MMContainer" %>
<%@ Register src="MMaintain.ascx" tagname="MMaintain" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
    <uc1:MMaintain ID="MMaintain1" runat="server" UserFunction="RMALiability"
        UserViewTable = "v_RMALiability" UserViewTitle = "RMA Liability" UserObjectType="DDL.RMALiability"/>
</asp:Content>

