<%@ Page Title="" Language="C#" MasterPageFile="~/MQITSPage.master" AutoEventWireup="true" CodeFile="ErrorPage.aspx.cs" Inherits="ErrorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_MQITS" Runat="Server">
    <div style=" text-align:center; height:400px; padding-top:50px;">
    <p style="font-size: 18pt;">Error for MQITS!</p>
    <a runat="server" id="hlErrorPageMsg" href="MyIssueAction.aspx" target="_self" style=" color:Red;">You have no authority to use 
        CurrentPage of MQITS!!!</a>
    </div>
</asp:Content>

