<%@ Page Title="" Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MRMAReturnType.aspx.cs" Inherits="MRMAReturnType" MaintainScrollPositionOnPostback="true" Theme="MMContainer" %>
<%@ Register src="MMaintain.ascx" tagname="MMaintain" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
    <uc1:MMaintain ID="MMaintain1" runat="server" UserFunction="RMAReturnType"
        UserViewTable = "v_RMAReturnType" UserViewTitle = "RMA Return Type" UserObjectType="DDL.RMAReturnType"/>
</asp:Content>


