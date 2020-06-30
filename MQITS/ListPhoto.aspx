<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ListPhoto.aspx.cs" Inherits="ListPhoto" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ListPhoto</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Repeater ID="rpPhoto" runat="server" DataSourceID="SqlDSPhoto"
            OnItemDataBound="rpPhoto_ItemDataBound">
            <AlternatingItemTemplate>
                <div style="font-size: 9pt; font-family: 'Century Gothic'; clear: none; display: inline;
                    float: left; visibility: visible; margin-left: 3px; margin-right: 3px;">
                    <asp:Image ID="imgq1" runat="server" Height="100px" ImageUrl='<%# DataBinder.Eval(Container.DataItem,"FilePath") %>'
                        Width="100px" /><br />
                    <asp:Label ID="lblPhotoDesc" runat="server" Text="Photo Descr:"></asp:Label><br />
                    <asp:Label ID="lblPhotoDescr" runat="server" Text='<%# Bind("FileDesc") %>'></asp:Label><br />
                    <asp:HiddenField ID="hfid" runat="server" Value='<%# Eval("ID") %>' />
                    <asp:HiddenField ID="hfFileName" runat="server" Value='<%# Eval("FileName") %>' />
                    
                </div>
            </AlternatingItemTemplate>
            <ItemTemplate>
                <div style="font-size: 9pt; font-family: 'Century Gothic'; clear: none; display: inline;
                    float: left; visibility: visible;">
                    <asp:Image ID="imgq1" runat="server" Height="100px" ImageUrl='<%# DataBinder.Eval(Container.DataItem,"FilePath") %>'
                        Width="100px" /><br />
                    <asp:Label ID="lblPhotoDesc" runat="server" Text="Photo Descr:"></asp:Label><br />
                    <asp:Label ID="lblPhotoDescr" runat="server" Text='<%# Bind("FileDesc") %>'></asp:Label><br />
                    <asp:HiddenField ID="hfid" runat="server" Value='<%# Eval("ID") %>' />
                    <asp:HiddenField ID="hfFileName" runat="server" Value='<%# Eval("FileName") %>' />                   
                </div>
            </ItemTemplate>
        </asp:Repeater>
         <asp:SqlDataSource ID="SqlDSPhoto" runat="server" ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" SelectCommand="SELECT ID,FilePath,dbo.fn_GetChtName(editor) as Uploader,cdt as UploadDate,FileName,&#13;&#10;dbo.GetXml(DESCRPTN,'FILE_DESC') AS FileDesc&#13;&#10;FROM AttachFile&#13;&#10;WHERE IssueID=@IssueID AND FileType='2'&#13;&#10;ORDER BY udt DESC">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="IssueID" QueryStringField="IssueID" />
                    </SelectParameters>
                </asp:SqlDataSource>
    
    </div>
    </form>
</body>
</html>
