<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Calendar.aspx.cs" Inherits="Calendar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Calendar Page</title>
   
  <script type="text/javascript" language="javascript">    
        function FeedbackCal()
        {	
            opener.document.form1.txtIncomingDate.value = document.form1.txtCal.value;
            window.close();
        } 
	</script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="font-family:Tahoma;font-size:small" id="divControl" runat="server">
                <fieldset>
               <div runat="server" id="divCalendar">
                                               <table>
                                               <tr><td><asp:Calendar ID="cal" runat="server" BackColor="#FFFFCC" BorderColor="#FFCC66" BorderWidth="1px"
                                    DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="#663399"
                                    Height="200px" ShowGridLines="True" Width="220px" OnSelectionChanged="cal_SelectionChanged">
                                    <SelectedDayStyle BackColor="#CCCCFF" Font-Bold="True" />
                                    <TodayDayStyle BackColor="#FFCC66" ForeColor="White" />
                                    <SelectorStyle BackColor="#FFCC66" />
                                    <OtherMonthDayStyle ForeColor="#CC9966" />
                                    <NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC" />
                                    <DayHeaderStyle BackColor="#FFCC66" Font-Bold="True" Height="1px" />
                                    <TitleStyle BackColor="#990000" Font-Bold="True" Font-Size="9pt" ForeColor="#FFFFCC" />
                                                                    </asp:Calendar>
                                                   <asp:HiddenField ID="txtCal" runat="server" />
                                               </td></tr>
                    <tr><td align="center">
                        <input id="btnClose" type="button" value="取消" onclick="window.close();" /></td></tr>
                  </table> 
               </div> 
    </fieldset>
    </div>
    </form>
</body>
</html>
