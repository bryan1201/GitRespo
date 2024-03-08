<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            width: 224px;
        }
        .auto-style3 {
            width: 166px;
        }
    </style>
</head>
<body>
    <div>
        <asp:HyperLink NavigateUrl="~/CreateNewDoc.asmx" runat="server" Text="CreateNewDoc ASMX" />&nbsp;|&nbsp;
		<asp:HyperLink NavigateUrl="~/DeleteDOC.asmx" runat="server" Text="DeleteDOC ASMX" />
    </div>
    <form id="form1" runat="server">
    <div>
    
        <br />
        <table class="auto-style1">
            <tr>
                <td class="auto-style3">
                    <asp:Label ID="Label2" runat="server" Text="KM資料夾ID"></asp:Label>
                </td>
                <td class="auto-style2">
                    <asp:TextBox ID="txtFolderID" runat="server"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3">
                    <asp:Label ID="Label1" runat="server" Text="來源檔案路徑"></asp:Label>
                </td>
                <td class="auto-style2">
                    <asp:TextBox ID="txtSourceFile" runat="server"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3">
                    <asp:Label ID="Label3" runat="server" Text="KM文件名稱"></asp:Label>
                </td>
                <td class="auto-style2">
                    <asp:TextBox ID="txtKMDocName" runat="server"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
        <br />
    
        <asp:Button ID="btnCreateNewDoc" runat="server" OnClick="btnCreateNewDoc_Click" Text="Create New Doc" />
    
        <asp:Label ID="lblResult" runat="server" Text="Result"></asp:Label>
    
    </div>
    </form>
</body>
</html>
