<%@ Control Language="C#" AutoEventWireup="true" CodeFile="wucIssueList.ascx.cs" Inherits="wucIssueList" %>
<asp:GridView ID="gvIssueList" runat="server" AutoGenerateColumns="False" 
    BorderColor="#9900FF" BorderStyle="Solid" BorderWidth="2px" CellPadding="4" 
    DataSourceID="SqlDSIssueList" Font-Size="9pt" ForeColor="#333333">
    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
    <Columns>
        <asp:BoundField DataField="InformationSummary" HeaderText="Summary" 
            ReadOnly="True" SortExpression="InformationSummary" />
        <asp:BoundField DataField="IssueSN" HeaderText="IssueSN" 
            SortExpression="IssueSN" />
        <asp:BoundField DataField="Project" HeaderText="Project" ReadOnly="True" 
            SortExpression="Project" />
        <asp:BoundField DataField="Phase" HeaderText="Phase" ReadOnly="True" 
            SortExpression="Phase" />
        <asp:BoundField DataField="PCA PN" HeaderText="PCA PN" 
            SortExpression="PCA PN" />
        <asp:BoundField DataField="Station" HeaderText="Station" ReadOnly="True" 
            SortExpression="Station" />
        <asp:BoundField DataField="IssueDate" HeaderText="IssueDate" ReadOnly="True" 
            SortExpression="IssueDate" />
        <asp:BoundField DataField="SerialNo" HeaderText="SerialNo" ReadOnly="True" 
            SortExpression="SerialNo" />
        <asp:BoundField DataField="DefectSymptom" HeaderText="DefectSymptom" 
            ReadOnly="True" SortExpression="DefectSymptom" />
        <asp:BoundField DataField="Location" HeaderText="Location" ReadOnly="True" 
            SortExpression="Location" />
        <asp:BoundField DataField="Priority" HeaderText="Priority" ReadOnly="True" 
            SortExpression="Priority" />
        <asp:BoundField DataField="Liability" HeaderText="Liability" ReadOnly="True" 
            SortExpression="Liability" />
    </Columns>
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <EditRowStyle BackColor="#999999" />
    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
</asp:GridView>
<asp:SqlDataSource ID="SqlDSIssueList" runat="server" 
    ConnectionString="<%$ ConnectionStrings:MQITSConnectionString %>" 
    SelectCommand="SELECT * FROM [v_InfoGroupIssueList] WHERE ([GroupID] = @GroupID)">
    <SelectParameters>
        <asp:ControlParameter ControlID="hfIssueGroupID" Name="GroupID" 
            PropertyName="Value" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:HiddenField ID="hfIssueGroupID" runat="server" />
