﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MRMAReturnSite.aspx.cs" Inherits="MRMAReturnSite" MaintainScrollPositionOnPostback="true" Theme="MMContainer" %>
<%@ Register src="MMaintain.ascx" tagname="MMaintain" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
    <uc1:MMaintain ID="MMaintain1" runat="server" UserFunction="RMAReturnSite"
        UserViewTable = "v_RMAReturnSite" UserViewTitle = "RMA Return Site" UserObjectType="DDL.RMAReturnSite"/>
</asp:Content>

