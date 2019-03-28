# define RELEASE
//# define DEBUG
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Diagnostics;

/// <summary>
/// SendMail 的摘要描述
/// </summary>
public class SendMail
{
    const string TSPROC = "sp_Route";
    private static bool enablesendmail = true;
    [Personalizable]
    public static bool EnableSendMail
    {
        get
        {
            return enablesendmail;
        }
        set
        {
            enablesendmail = value;
        }
    }
    public SendMail()
    {
        //
        // TODO: 在此加入建構函式的程式碼
        //
    }

    public static void smtpMail(string mail_from, string mail_fromName, string mail_to, string mail_cc, string mail_bcc, string mail_subject, string mail_body)
    {
        try
        {
            MailMessage mailMsg = new MailMessage();

            if (mail_from == "")
                mailMsg.From = new MailAddress(Constant.DefaultMailFrom.ToString(), "ITS Web Service");
            else
                mailMsg.From = new MailAddress(mail_from, mail_fromName);

            if (mail_to.ToString().Length == 1)
                if (mail_from == "")
                    mail_to = Constant.DefaultMailFrom.ToString();
                else
                    mail_to = mail_from;

            if (mail_to != null && mail_to != "")
                mailMsg.To.Add(new MailAddress(mail_to));

            if (mail_cc != null && mail_cc != "")
                mailMsg.CC.Add(new MailAddress(mail_cc));

            mailMsg.CC.Add(new MailAddress(mail_from));
            mailMsg.CC.Add(new MailAddress(Constant.DefaultMailBcc));

            if (mail_bcc != null && mail_bcc != "")
                mailMsg.Bcc.Add(new MailAddress(Constant.DefaultMailBcc.ToString()));

            mailMsg.Subject = mail_subject;
            mailMsg.Body = mail_body;
            mailMsg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient(Constant.DefaultMailServer, 25);

            if (mailMsg.To.Count > 0)
            {
#if RELEASE
                client.Send(mailMsg);
#endif
            }
            // --Bryan 20073028
        }
        catch
        {
            // Nothing, will be maillog builded
        }
    }

    public static void smtpMailGroup(string mail_from, string mail_fromName, string mail_to, DataRowCollection mail_cc, DataRowCollection mail_bcc, string mail_subject, string mail_body)
    {
        try
        {
            MailMessage mailMsg = new MailMessage();

            if (mail_from == "")
                mailMsg.From = new MailAddress(Constant.DefaultMailFrom.ToString(), "ITS Web Service");
            else
                mailMsg.From = new MailAddress(mail_from, mail_fromName);

            if (mail_to.ToString().Length == 1)
                if (mail_from == "")
                    mail_to = Constant.DefaultMailFrom.ToString();
                else
                    mail_to = mail_from;

            if (mail_to != null && mail_to != "")
                mailMsg.To.Add(new MailAddress(mail_to));

            if (mail_cc != null)
                if (mail_cc.Count > 0)
                {
                    foreach (DataRow dr in mail_cc)
                    {
                        mailMsg.CC.Add(new MailAddress(dr[1].ToString()));
                    }
                }

            mailMsg.CC.Add(new MailAddress(mail_from));
            mailMsg.CC.Add(new MailAddress(Constant.DefaultMailBcc));

            if (mail_bcc != null)
                if (mail_bcc.Count > 0)
                {
                    foreach (DataRow dr in mail_bcc)
                    {
                        mailMsg.Bcc.Add(new MailAddress(dr[1].ToString()));
                    }
                }

            mailMsg.Subject = mail_subject;
            mailMsg.Body = mail_body;
            mailMsg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient(Constant.DefaultMailServer, 25);

            if (mailMsg.To.Count > 0 || mailMsg.CC.Count > 0 || mailMsg.Bcc.Count > 0)
            {
#if RELEASE
                client.Send(mailMsg);
#endif
            }
            // --Bryan 20073028
        }
        catch
        {
            // Nothing, will be maillog builded
        }
    }

    public static void smtpMail(string mail_from, string mail_to, string mail_cc, string mail_bcc, string mail_subject, string mail_body)
    {
        try
        {
            MailMessage mailMsg = new MailMessage();

            if (mail_from == "")
                mailMsg.From = new MailAddress(Constant.DefaultMailFrom.ToString(), "ITS Web Service");
            else
                mailMsg.From = new MailAddress(mail_from, Method.GetUserNameByeMail(mail_from));

            if (mail_to.ToString().Length == 1)
                if (mail_from == "")
                    mail_to = Constant.DefaultMailFrom.ToString();
                else
                    mail_to = mail_from;

            if (mail_to != null && mail_to != "")
                mailMsg.To.Add(new MailAddress(mail_to));

            if (mail_cc != null && mail_cc != "")
                mailMsg.CC.Add(new MailAddress(mail_cc));

           mailMsg.Bcc.Add(new MailAddress(Constant.DefaultMailBcc.ToString()));

            mailMsg.Subject = mail_subject;
            mailMsg.Body = mail_body;
            mailMsg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient(Constant.DefaultMailServer, 25);

            if (mailMsg.To.Count > 0)
            {
#if RELEASE
                client.Send(mailMsg);
#endif
            }
            // --Bryan 20073028
        }
        catch
        {
            // Nothing, will be maillog builded
        }
    }

    public static void smtpMail(string mail_from, string mail_to, string mail_cc, DataTable mail_bcc, string mail_subject, string mail_body)
    {
        try
        {
            MailMessage mailMsg = new MailMessage();

            if (mail_from == "")
                mailMsg.From = new MailAddress(Constant.DefaultMailFrom.ToString(), "ITS Web Service");
            else
                mailMsg.From = new MailAddress(mail_from, Method.GetUserNameByeMail(mail_from));

            if (mail_to.ToString().Length == 1)
                if (mail_from == "")
                    mail_to = Constant.DefaultMailFrom.ToString();
                else
                    mail_to = mail_from;

            if (mail_to != null && mail_to != "")
                mailMsg.To.Add(new MailAddress(mail_to));

            if (mail_cc != null && mail_cc != "")
                mailMsg.CC.Add(new MailAddress(mail_cc));

            if (mail_bcc.Rows.Count > 0)
            {
                foreach (DataRow dr in mail_bcc.Rows)
                {
                    mailMsg.Bcc.Add(new MailAddress(dr[1].ToString()));
                }
            }
            else
                mailMsg.Bcc.Add(new MailAddress(Constant.DefaultMailBcc));

            mailMsg.Subject = mail_subject;
            mailMsg.Body = mail_body;
            mailMsg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient(Constant.DefaultMailServer, 25);

            if (mailMsg.To.Count > 0)
            {
#if RELEASE
                client.Send(mailMsg);
#endif
            }
            // --Bryan 20073028
        }
        catch
        {
            // Nothing, will be maillog builded
        }
    }



    public static void smtpMailGroup(string mail_from, DataRowCollection mail_to, DataRowCollection mail_cc, DataRowCollection mail_bcc, string mail_subject, string mail_body)
    {
        try
        {
            MailMessage mailMsg = new MailMessage();

            if (mail_from == "")
                mailMsg.From = new MailAddress(Constant.DefaultMailFrom.ToString(), "ITS Web Service");
            else
                mailMsg.From = new MailAddress(mail_from, Method.GetUserNameByeMail(mail_from));

            if (mail_to.Count > 0)
            {
                foreach (DataRow dr in mail_to)
                {
                    mailMsg.To.Add(new MailAddress(dr[1].ToString()));
                }
            }

            if (mail_cc.Count > 0)
            {
                foreach (DataRow dr in mail_cc)
                {
                    mailMsg.CC.Add(new MailAddress(dr[1].ToString()));
                }
            }

            if (mail_bcc.Count > 0)
            {
                foreach (DataRow dr in mail_bcc)
                {
                    mailMsg.Bcc.Add(new MailAddress(dr[1].ToString()));
                }
            }

            mailMsg.CC.Add(new MailAddress(mail_from));
            mailMsg.CC.Add(new MailAddress(Constant.DefaultMailBcc));

            mailMsg.Subject = mail_subject;
            mailMsg.Body = mail_body;
            mailMsg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient(Constant.DefaultMailServer, 25);

            if (mailMsg.To.Count > 0 || mailMsg.CC.Count > 0)
            {
#if RELEASE
                client.Send(mailMsg);
#endif
            }
            // --Bryan 20073028
        }
        catch
        {
            // Nothing, will be maillog builded
        }
    }

    public static void smtpMail(string mail_from, string mail_to, DataTable mail_cc, DataTable mail_bcc, string mail_subject, string mail_body)
    {
        try
        {
            MailMessage mailMsg = new MailMessage();

            if (mail_from == "")
                mailMsg.From = new MailAddress(Constant.DefaultMailFrom.ToString(), "PCBWeb Service");
            else
                mailMsg.From = new MailAddress(mail_from, "PCBWeb Service");


            if (mail_to.Length > 0)
                mailMsg.To.Add(new MailAddress(mail_to));
            else
                mailMsg.To.Add(new MailAddress(Constant.DefaultMailFrom.ToString()));

            if (mail_cc.Rows.Count > 0)
            {
                foreach (DataRow dr in mail_cc.Rows)
                {
                    mailMsg.CC.Add(new MailAddress(dr[1].ToString()));
                }
            }

            mailMsg.CC.Add(new MailAddress(mail_from));

            if (mail_bcc.Rows.Count > 0)
            {
                foreach (DataRow dr in mail_bcc.Rows)
                {
                    mailMsg.Bcc.Add(new MailAddress(dr[1].ToString()));
                }
            }
            else
                mailMsg.Bcc.Add(new MailAddress(Constant.DefaultMailBcc));

            mailMsg.Subject = mail_subject;
            mailMsg.Body = mail_body;
            mailMsg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient(Constant.DefaultMailServer, 25);
            if (mailMsg.To.Count > 0 || mailMsg.CC.Count > 0)
            {
#if RELEASE
                client.Send(mailMsg);
#endif
            }
            // --Bryan 20073028
        }
        catch
        {
            // Nothing, will be maillog builded
        }
    }

    // Add by Dean 20071113 v1.0
    public static void smtpMail(string mail_from, DataTable mail_to, DataTable mail_cc, DataTable mail_bcc, string mail_subject, string mail_body)
    {
        try
        {
            MailMessage mailMsg = new MailMessage();

            if (mail_from == "")
                mailMsg.From = new MailAddress(Constant.DefaultMailFrom.ToString(), "PCBWeb Service");
            else
                mailMsg.From = new MailAddress(mail_from, "PCBWeb Service");


            if (mail_to.Rows.Count > 0)
            {
                foreach (DataRow dr in mail_to.Rows)
                {
                    mailMsg.To.Add(new MailAddress(dr[1].ToString()));
                }
            }

            if (mail_cc.Rows.Count > 0)
            {
                foreach (DataRow dr in mail_cc.Rows)
                {
                    mailMsg.CC.Add(new MailAddress(dr[1].ToString()));
                }
            }

            mailMsg.CC.Add(new MailAddress(mail_from));

            if (mail_bcc.Rows.Count > 0)
            {
                foreach (DataRow dr in mail_bcc.Rows)
                {
                    mailMsg.Bcc.Add(new MailAddress(dr[1].ToString()));
                }
            }
            else
                mailMsg.Bcc.Add(new MailAddress(Constant.DefaultMailBcc));

            mailMsg.Subject = mail_subject;
            mailMsg.Body = mail_body;
            mailMsg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient(Constant.DefaultMailServer, 25);
            if (mailMsg.To.Count > 0 || mailMsg.CC.Count > 0)
            {
#if RELEASE
                client.Send(mailMsg);
#endif
            }
            // --Bryan 20073028
        }
        catch
        {

            //
        }
    }

    public static string ScreenScrapeHtml(string url)
    {
        WebRequest objReq = System.Net.HttpWebRequest.Create(url);

        StreamReader sr = new StreamReader(objReq.GetResponse().GetResponseStream());
        string result = sr.ReadToEnd();
        sr.Close();
        return result;
    }
}