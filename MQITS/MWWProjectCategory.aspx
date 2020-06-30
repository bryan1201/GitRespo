<%@ Page Title="" Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="MWWProjectCategory.aspx.cs" Inherits="MWWProjectCategory" MaintainScrollPositionOnPostback="true" Theme="MMContainer" %>

<%@ Register src="MMaintain.ascx" tagname="MMaintain" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">

        <uc1:MMaintain ID="MMaintain1" runat="server" UserFunction="WWProjectCategory"
        UserViewTable = "v_WWProjectCategory" UserViewTitle = "WW Project Category" UserObjectType="CHECKBOXLIST.WWProjectCategory"/>

</asp:Content>

