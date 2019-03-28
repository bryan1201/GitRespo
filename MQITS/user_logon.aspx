<%@ Page Language="C#" AutoEventWireup="true" CodeFile="user_logon.aspx.cs" Inherits="user_logon" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>=== MQITS::User Logon ===</title>
</head>
<body>
    <form id="form1" runat="server">
			<div style="WIDTH: 100%; HEIGHT: 100%; background-attachment: fixed; background-repeat: no-repeat; text-align:center;">
				<table cellspacing="0" cellpadding="0" 
					border="0" 
                    
                    style="text-align:center; background-color:#ffffff; width:486px; margin-top: 100px; background-position:center top; background-image: url(images/5bg_login.jpg); clip: rect(90px auto auto auto); background-repeat: no-repeat; height: 400px; background-attachment: inherit;">
					<tr>
						<td colspan="4" height="60px">
                            <h2 style="text-align:center; margin-left:150px; font-family: 'Arial Unicode MS'; color: #800000; font-weight: bold; font-size: 22px; visibility: collapse; clip: rect(auto, auto, auto, auto); clear: both;">
                                <asp:Label ID="lblWebSiteName" runat="server" Text="MQITS" Font-Bold="True" 
                                    Font-Size="22pt" ForeColor="#8E4E8E"></asp:Label>
                                <asp:Label ID="lblWebSiteVer" runat="server" Font-Size="9pt">v 0.1</asp:Label></h2>
                        </td>
					</tr>
					<tr>
						<td valign="middle" colspan="4" 
                            style="background-position: center top; background-image: url(images/5login_pic.jpg); background-repeat: no-repeat;">
							<div id="divSubmit" style="text-align:center;" runat="server">
								<table cellspacing="0" cellpadding="0" width="85%" border="0">
                                    <tr>
                                        <td colspan="4" style="height:90px;" >
                                        </td>
                                    </tr>
									<tr>
										<td style="WIDTH: 62px" width="62" height="25">
											<div align="right"><font size="2"><img height="10" src="images/ico_login.jpg" width="10" alt="icon" />
												</font>
											</div>
										</td>
										<td style="WIDTH: 53px" width="53" height="40">
											<div align="center"><font size="2">User Id</font></div>
										</td>
										<td style="WIDTH: 26px" width="26" height="40">
											<div align="center"><font size="2">：</font></div>
										</td>
										<td height="25" align="left"><font size="2">&nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                            <asp:textbox id="txtUserId" runat="server" BackColor="White" BorderColor="White" Width="177px"></asp:textbox></font></td>
									</tr>
									<tr>
										<td style="WIDTH: 62px" width="62" height="40">
											<div align="right"><font size="2"><img height="10" src="images/ico_login.jpg" width="10" />
												</font>
											</div>
										</td>
										<td style="WIDTH: 53px" width="53" height="40">
											<div align="center"><font size="2">Password</font></div>
										</td>
										<td style="WIDTH: 26px" width="26" height="24">
											<div align="center"><font size="2">：</font></div>
										</td>
										<td height="24" align="left"><font size="2">&nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                            <asp:textbox id="txtPWD" runat="server" BackColor="White" BorderColor="White" Width="177px"
													TextMode="Password"></asp:textbox></font></td>
									</tr>
								</table>
								<table cellspacing="0" cellpadding="0" width="100%" border="0">
									<tr valign="middle" align="center">
										<td width="48%" height="50">
                                            <asp:button id="ButEnter" runat="server" 
                                                BackColor="#CCFFFF" BorderColor="#CCCCFF" Width="126px"
												Text="Log on" Font-Bold="True" ForeColor="#3333FF" OnClick="ButEnter_Click"></asp:button></td>
										<td width="48%">
                                            <asp:button id="ChangePWD" BackColor="#CCFFFF" 
                                                BorderColor="#CCCFFF" Width="126px" Text="Change Password" Font-Bold="True"
												ForeColor="#996633" Runat="server" OnClick="ChangePWD_Click" Visible="False"></asp:button></td>
									</tr>
                                    <tr align="center" valign="middle">
                                        <td colspan="2" height="50">
                                        </td>
                                    </tr>
								</table>
							</div>
							<div id="divChangePWD" runat="server" style="margin-top: 100px">
								<table cellspacing="0" cellpadding="0" width="80%" style="text-align:center;" border="0">
									<tr>
										<td style="WIDTH: 32px; height: 24px;">
											<div style="text-align:right;"><font size="2"><img height="10" src="images/ico_login.jpg" width="10" alt="icon" />
												</font>
											</div>
										</td>
										<td style="FONT-SIZE: 9pt; WIDTH: 67px; height: 24px;">&nbsp;帳號</td>
										<td style="WIDTH: 27px; height: 24px;" width="27">
											<div align="center"><font size="2">：</font></div>
										</td>
										<td style="height: 24px" align="left">
                                            &nbsp; &nbsp;
                                            <asp:textbox id="txtUserId2" Width="177px" Runat="server"></asp:textbox></td>
									</tr>
									<tr>
										<td style="WIDTH: 32px">
											<div style="text-align:right;"><font size="2"><img height="10" src="images/ico_login.jpg" width="10">
												</font>
											</div>
										</td>
										<td style="FONT-SIZE: 9pt; WIDTH: 67px">&nbsp;舊密碼</td>
										<td style="WIDTH: 27px" width="27">
											<div align="center"><font size="2">：</font></div>
										</td>
										<td align="left">
                                            &nbsp; &nbsp;
                                            <asp:textbox id="txtOldPWD" Width="177px" TextMode="Password" Runat="server"></asp:textbox></td>
									</tr>
									<tr>
										<td style="WIDTH: 32px">
											<div style="text-align:right;"><font size="2"><img height="10" src="images/ico_login.jpg" width="10">
												</font>
											</div>
										</td>
										<td style="FONT-SIZE: 9pt; WIDTH: 67px">&nbsp;新密碼</td>
										<td style="WIDTH: 27px">
											<div style="text-align:center;"><font size="2">：</font></div>
										</td>
										<td align="left">
                                            &nbsp; &nbsp;
                                            <asp:textbox id="txtNewPWD1" Width="177px" TextMode="Password" Runat="server"></asp:textbox>
											</td>
									</tr>
									<tr>
										<td style="WIDTH: 32px">
											<div style="text-align:right;"><font size="2"><img height="10" src="images/ico_login.jpg" width="10">
												</font>
											</div>
										</td>
										<td style="FONT-SIZE: 9pt; WIDTH: 67px">&nbsp;確認新密碼</td>
										<td style="WIDTH: 27px">
											<div align="center"><font size="2">：</font></div>
										</td>
										<td style="text-align:left;">
                                            &nbsp; &nbsp;
                                            <asp:textbox id="txtNewPWD2" Width="177px" TextMode="Password" Runat="server"></asp:textbox>
											</td>
									</tr>
									<tr>
									
										<td align="right" colspan="4" style="height:30px;">
										<asp:button id="btnOK" BackColor="#CCFFFF" BorderColor="#CCCFFF" Text="確認" 
                                                ForeColor="Black" Runat="server" OnClick="btnOK_Click"></asp:button>
                                            <asp:button id="btnCancel" BackColor="#CCFFFF" BorderColor="#CCCFFF" Text="取消" ForeColor="Black"
												Runat="server" OnClick="btnCancel_Click"></asp:button>
										</td>
									</tr>
									<tr><td align="center" colspan="4" style="FONT-SIZE: 10pt; COLOR: purple; FONT-FAMILY: 'Arial Unicode MS'; font-style: italic; height:200px">*密碼原則：至少8碼，含英文數字及符號(!@#$~+=-_)<br />
                                        
												<asp:requiredfieldvalidator id="rfvUserId" runat="server" Font-Size="9pt" Display="Dynamic" ControlToValidate="txtUserId"
													ErrorMessage="不允許空白！" ValidationGroup="PassWord" Visible="False"></asp:requiredfieldvalidator><br />
												<asp:customvalidator id="cuvUserPWD" runat="server" Font-Size="8pt" Display="Dynamic" ControlToValidate="txtPWD"
													ErrorMessage="無此帳號或密碼輸入錯誤，請重新輸入！" ValidateEmptyText="True" ValidationGroup="PassWord"></asp:customvalidator>
											<asp:customvalidator id="cuvPWD2" runat="server" Font-Size="8pt" Display="Dynamic" ControlToValidate="txtOldPWD"
												ErrorMessage="無此帳號或密碼輸入錯誤，請重新輸入！" ValidationGroup="PassWord" Visible="False"></asp:customvalidator>
                                        <asp:customvalidator id="cuvOldPWD" runat="server" Font-Size="8pt" Display="Dynamic" ControlToValidate="txtOldPWD"
												ErrorMessage="舊密碼驗證錯誤，請重新輸入！" ValidationGroup="PassWord"></asp:customvalidator>
											<asp:RequiredFieldValidator id="rfvNewPWD1" runat="server" Font-Size="9pt" ControlToValidate="txtNewPWD1" ErrorMessage="不允許空白！"
												EnableClientScript="False" ValidationGroup="PassWord">不允許空白！</asp:RequiredFieldValidator>
											<asp:RequiredFieldValidator id="rfvNewPWD2" runat="server" Font-Size="9pt" ControlToValidate="txtNewPWD2" ErrorMessage="不允許空白！"
												EnableClientScript="False" ValidationGroup="PassWord">不允許空白！</asp:RequiredFieldValidator><asp:comparevalidator id="cpvNewPWD" runat="server" Font-Size="8pt" Display="Dynamic" ControlToValidate="txtNewPWD2"
												ErrorMessage="新密碼確認不符，請重新輸入！" ControlToCompare="txtNewPWD1" ValidationGroup="PassWord"></asp:comparevalidator></td></tr>
								</table>
							</div>
						</td>
					</tr>
					<tr>
					    <td colspan="4" style="text-align:center;FONT-SIZE: 10pt; COLOR: purple; FONT-FAMILY: 'Arial Unicode MS'; font-style: italic;" valign="top">
					    <asp:ValidationSummary ID="vsMsg" runat="server" Font-Bold="True" Font-Italic="False"
                                            Font-Names="Arial Unicode MS" ValidationGroup="PassWord" Width="100%" />
					    </td>
					</tr>

				</table>
				<map id="Map">
					<area shape="circle" target="_blank" coords="360,19,15" href="#" alt="help" />
				</map>
				<asp:textbox id="TextMsg" runat="server" BackColor="Red" Width="500px" Font-Bold="True" Visible="false"></asp:textbox>
				</div>
    </form>
</body>
</html>
