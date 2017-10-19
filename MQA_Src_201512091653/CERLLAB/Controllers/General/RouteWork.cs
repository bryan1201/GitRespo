using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CERLLAB.Models;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Web.Mvc;
using System.Net.Http;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

namespace CERLLAB.Controllers.General
{
    public class RouteWork
    {
        private sysErrorMessageDBSet syserrdb = new sysErrorMessageDBSet();
        private CERLDBContext db = new CERLDBContext();
        private CERLEntities edb = new CERLEntities();
        private Controller _formcontroller { get; set; }
        private WindowsIdentity _userwi { get; set; }
        private string _routeswebsite { get; set; }
        private string _cerllabwebsite { get; set; }
        private HttpSessionStateBase _httpsession { get; set; }
        private string _userid { get; set; }
        private string UserId { get; set; }

        IEnumerable<vTaskDetail> Ptask = new List<vTaskDetail>();

        public RouteWork(WindowsIdentity wi, Controller fcontroller, HttpSessionStateBase session)
        {
            _routeswebsite = Constant.RoutesWebSite;
            _cerllabwebsite = Constant.S_WebSite;
            _userwi = wi;
            _formcontroller = fcontroller;
            _httpsession = session;
            _userid = fcontroller.User.Identity.Name.ToUpper();
            UserId = Method.GetLogonUserId(_httpsession, _formcontroller, _userid);
        }

        public HttpResponseMessage PostFormDataToRoute(FormCollection fc, f_cerl f_cerl)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            NameValueCollection dyndata = new NameValueCollection();
            string Action = f_cerl.Action;
            if (Action == null)
                Action = "";

            dyndata.Add("FID", f_cerl.fID);
            dyndata.Add("FormCode", f_cerl.FormCode.ToString());
            dyndata.Add("FlowCode", f_cerl.FlowCode.ToString());
            dyndata.Add("Applicant", f_cerl.Applicant);
            dyndata.Add("ListAssignTo", f_cerl.ListAssignTo); // 用分號打包卡號, EX: IEC891652;IEC950848
            dyndata.Add("State", f_cerl.State.ToString());
            dyndata.Add("Action", f_cerl.Action);
            dyndata.Add("Editor", UserId);
            dyndata.Add("Comment", f_cerl.Comment);
            //string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(dyndata);

            using (var wb = new WebClient())
            {
                var wi = _userwi;
                var wic = wi.Impersonate();
                wb.Headers.Set("CONTENT-TYPE", "application/x-www-form-urlencoded");
                wb.UseDefaultCredentials = true;
                //wb.Credentials = new NetworkCredential(Constant.NetworkCredentialUserId, Constant.NetworkCredentialPWD);

                string url = _formcontroller.Url.RouteUrl(@"DefaultApi", new { action = "Task", controller = "api" });
                if (url == null)
                {
                    url = Constant.RoutesWebSite + @"/api/task/Post";
                }
                byte[] rslt = wb.UploadValues(url, dyndata);
            }
            return response;
        }

        public string GetWebResponse(string Url)
        {
            string response = "";
            using (var client = new WebClient())
            {
                //var wi = (System.Security.Principal.WindowsIdentity)HttpContext.User.Identity;
                var wic = _userwi.Impersonate();
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.UseDefaultCredentials = true;
                client.Encoding = System.Text.Encoding.UTF8;
                //client.Credentials = new NetworkCredential(Constant.NetworkCredentialUserId, Constant.NetworkCredentialPWD);
                response = client.DownloadString(Url);
            }
            //var obj = client.DownloadData(ApiUrl);
            return response;
        }

        // Download Route Task FlowPath
        public FlowPath GetFlowPath(string fID, int FlowCode, int State, string strAction)
        {
            //http://localhost:56491/api/Task/getFlowPath?fID=A38AFFA3-4F99-447A-93F3-72FE3207DE1A&FlowCode=1001001&State=10&strAction=A
            string ApiUrl = _routeswebsite + @"/api/task/getFlowPath?fID=" + fID + "&FlowCode=" + FlowCode.ToString() + "&State=" + State.ToString() + "&strAction=" + strAction;
            string response = GetWebResponse(ApiUrl);
            var fp = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
            FlowPath objfp = (FlowPath)Newtonsoft.Json.JsonConvert.DeserializeObject<FlowPath>(fp.ToString());
            return objfp;
        }

        public IEnumerable<FlowState> GetFlowStateList(int FlowCode)
        {
            //http://localhost:56491/Routes/api/Task/GetFlowStateList?FlowCode=1001001
            string ApiUrl = _routeswebsite + @"/api/task/GetFlowStateList?FlowCode=" + FlowCode.ToString();
            string response = GetWebResponse(ApiUrl);
            var fp = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
            IEnumerable<FlowState> objfp = (IEnumerable<FlowState>)Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<FlowState>>(fp.ToString());
            return objfp;
        }

        public void GetListAssignTo(int? outState, string vchSet, out string rslt)
        {
            string ListAssignTo = edb.FnGetAssignTo(outState, vchSet).FirstOrDefault().ListAssitnTo;
            rslt = ListAssignTo.ToString();
        }

        public IEnumerable<vTaskDetail> GetAllTask(string strAction)
        {
            string ApiUrl = _routeswebsite + @"/api/task/GetAllTask?strAction=" + strAction;
            string response = GetWebResponse(ApiUrl);
            var fp = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
            IEnumerable<vTaskDetail> objfp = (IEnumerable<vTaskDetail>)Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<vTaskDetail>>(fp.ToString());

            return objfp;
        }

        public IEnumerable<vTaskDetail> GetAllTask2(IList<string> ListFID)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string jsonFIDString = jsonSerializer.Serialize(ListFID);
            string ApiUrl = _routeswebsite + @"/api/task/GetAllTask2?jsonFIDString=" + jsonFIDString;
            string response = GetWebResponse(ApiUrl);
            var fp = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
            IEnumerable<vTaskDetail> objfp = (IEnumerable<vTaskDetail>)Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<vTaskDetail>>(fp.ToString());

            return objfp;
        }

        // Download FormTaskDetail
        public IEnumerable<vTaskDetail> GetTaskDetail(string fID)
        {
            string ApiUrl = _routeswebsite + @"/api/task/GetTaskDetail?fID=" + fID;
            string response = GetWebResponse(ApiUrl);
            var fp = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
            IEnumerable<vTaskDetail> objfp = (IEnumerable<vTaskDetail>)Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<vTaskDetail>>(fp.ToString()).OrderBy(x => x.S_date);

            return objfp;
        }

        private async Task<IEnumerable<vTaskDetail>> GetUnsignedTask2(string BadgeCode)
        {
            string ApiUrl = _routeswebsite + @"/api/task/GetUnsignedTask?BadgeCode=" + BadgeCode;
            string asyncInfo = @"api/task/GetUnsignedTask?BadgeCode=" + BadgeCode;
            var handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            var _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(Constant.RoutesWebSite);
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _httpClient.GetAsync(asyncInfo);
            response.EnsureSuccessStatusCode();
            IEnumerable<vTaskDetail> task = await response.Content.ReadAsAsync<IEnumerable<vTaskDetail>>();
            return task;
        }

        public async Task RunAsunch(string UserId)
        {
            Ptask = await GetUnsignedTask2(UserId);
        }

        public IEnumerable<vTaskDetail> GetUnsignedTask(string BadgeCode)
        {
            string ApiUrl = _routeswebsite + @"/api/task/GetUnsignedTask?BadgeCode=" + BadgeCode;
            string response = GetWebResponse(ApiUrl);
            var fp = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
            IEnumerable<vTaskDetail> objfp = (IEnumerable<vTaskDetail>)Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<vTaskDetail>>(fp.ToString());

            return objfp;
        }

        public IEnumerable<vTaskDetail> GetSignedTask(string BadgeCode)
        {
            string ApiUrl = _routeswebsite + @"/api/task/GetSignedTask?BadgeCode=" + BadgeCode;
            string response = GetWebResponse(ApiUrl);
            var fp = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
            IEnumerable<vTaskDetail> objfp = (IEnumerable<vTaskDetail>)Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<vTaskDetail>>(fp.ToString());

            return objfp;
        }

        public bool CheckUserUnsignedTask(string fID)
        {
            bool rslt = false;
            List<int> UserRole = db.vUserRole.Where(x => x.BadgeCode == UserId).Select(x => x.RoleId).ToList();
            f_cerl fcerl = db.f_cerl.Where(x => x.fID == fID).FirstOrDefault();

            rslt = (UserRole.Contains(1)) ? true : false;

            if (rslt == true) // Admin?
                return rslt;

            try
            {
                IEnumerable<vTaskDetail> objfp = GetUnsignedTask(UserId);
                List<string> unsinged = objfp.Where(x => x.FID == fID).Select(x => x.FID).ToList();
                // 允許使用者自行移轉
                if (unsinged != null && fcerl != null)
                {
                    if (unsinged.Count > 0 || (fcerl.State == 10 && fcerl.Applicant == UserId))
                        rslt = true;
                }
            }
            catch
            {

            }
            return rslt;
        }

        public f_cerl CheckAndPushTask(FormCollection fc, f_cerl f_cerl)
        {
            string fnCheckAndPushTask = "RouteWork_CheckAndPushTask";
            string UserId = f_cerl.editor;
            string strAction = f_cerl.Action;
            string ListAssignTo = "";
            int errorline = 233;
            try
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                StringBuilder vchSet = new StringBuilder();
                errorline = 238;
                FlowPath fp = GetFlowPath(f_cerl.fID, f_cerl.FlowCode, f_cerl.State, f_cerl.Action); // 取得下一關的State
                errorline = 240;
                if (fp == null)
                    return f_cerl;

                if (f_cerl.State >= 1000)
                {
                    strAction = "";
                    return f_cerl;
                }
                errorline = 249;
                if (fp != null)
                {
                    if (f_cerl.LocalSupervisor == null)
                        f_cerl.LocalSupervisor = db.vUsers.Where(u => u.RoleId == 20).Select(v => v.BadgeCode).FirstOrDefault();//"IEC970209";
                    errorline = 254;
                    if (f_cerl.Manager == null)
                        f_cerl.Manager = f_cerl.Applicant;
                    errorline = 257;
                    vchSet.Append(Method.BuildXML(fp.outState.ToString(), "outState"));
                    vchSet.Append(Method.BuildXML(f_cerl.TestItem.ToString(), "TestItem"));
                    vchSet.Append(Method.BuildXML(f_cerl.Supervisor, "Supervisor"));
                    vchSet.Append(Method.BuildXML(f_cerl.LocalSupervisor, "LocalSupervisor"));
                    vchSet.Append(Method.BuildXML(f_cerl.LabMember, "LabMember"));
                    vchSet.Append(Method.BuildXML(f_cerl.Applicant, "Applicant"));
                    vchSet.Append(Method.BuildXML(f_cerl.Manager, "Manager"));
                    vchSet.Append(Method.BuildXML(f_cerl.Action, ""));
                    errorline = 266;
                    GetListAssignTo(fp.outState, vchSet.ToString(), out ListAssignTo); //取得下一關的收件人
                    f_cerl.ListAssignTo = ListAssignTo;
                    errorline = 269;
                    if (ListAssignTo != "")
                    {
                        errorline = 272;
                        response = PostFormDataToRoute(fc, f_cerl); //Push Task, Send Task mail by AssignTo

                        f_cerl.State = int.Parse(fp.outState.ToString());
                        db.Entry(f_cerl).State = EntityState.Modified;
                        db.SaveChanges();
                        errorline = 278;
                        SendMail(fp, f_cerl.fID);
                        errorline = 280;
                    }
                    errorline = 282;
                }
                f_cerl.Comment = "";
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                string newLine = "\r\n";
                StringBuilder sb = new StringBuilder();
                sb.Append("Issue at line 291: occur error after line " + errorline.ToString() + msg);
                sb.Append(newLine);
                sb.Append(f_cerl.ID + " " + f_cerl.State);
                syserrdb.InitErrorData(Src: fnCheckAndPushTask, content: sb.ToString(), editor: UserId);
            }
            return f_cerl;
        }

        // Download FormAction
        public IEnumerable<FormActions> GetFormAction(int FlowCode, int State)
        {
            string ApiUrl = _routeswebsite + @"/api/task/GetFormAction?FlowCode=" + FlowCode.ToString() + "&State=" + State.ToString();
            string response = GetWebResponse(ApiUrl);
            var fp = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
            IEnumerable<FormActions> objfp = (IEnumerable<FormActions>)Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<FormActions>>(fp.ToString());
            return objfp;
        }

        public string SendMail(FlowPath fp, string fID)
        {
            string fnSendMail = "RouteWork_SendMail(FlowPath fp, string fID)";
            int errorline = 312;
            int inState = int.Parse(fp.inState.ToString());
            //http://iec1-aptest.iec.inventec/CERLLAB/F_CERL/DetailsLAB/
            string FormUrl = _cerllabwebsite + @"/F_CERL/EditLAB/";
            string Form1Url = _cerllabwebsite + @"/F_CERL/EditLAB/";
            string Form2Url = _cerllabwebsite + @"/F_CERL/DetailsLAB/";
            string newLine2 = "<br />\r\n";
            string newLine = "\r\n";
            StringBuilder Body = new StringBuilder();

            FormUrl = (fp.outState >= 1000) ? Form2Url : Form1Url;
            vFCERL fcerl = db.vFCERL.Where(x => x.fID == fID).FirstOrDefault();

            string editor = db.userprofile.Where(x => x.BadgeCode == UserId).Select(x => x.ChtName).FirstOrDefault();
            string ID = fcerl.ID.ToString();
            try
            {
                string tableStyle = "table-layout: fixed;width:100%;border:1px solid;";
                string thStyle = "color:#ffffff;background:#0078ae;font-weight:bold;width:30%;border:1px solid;";
                string tdStyle = "width:70%;border:1px solid;";
                Body.Append(Method.BuildXML2(Method.BuildXML2("簽核者", "th", thStyle) + Method.BuildXML2(editor, "td", tdStyle), "tr"));
                Body.Append(Method.BuildXML2(Method.BuildXML2("UID", "th", thStyle) + Method.BuildXML2(fcerl.UID, "td", tdStyle), "tr"));
                Body.Append(Method.BuildXML2(Method.BuildXML2("Case ID", "th", thStyle) + Method.BuildXML2(fcerl.CaseID, "td", tdStyle), "tr"));
                Body.Append(Method.BuildXML2(Method.BuildXML2("申請人", "th", thStyle) + Method.BuildXML2(fcerl.Applicant, "td", tdStyle), "tr"));
                Body.Append(Method.BuildXML2(Method.BuildXML2("Background Desc.", "th", thStyle) + Method.BuildXML2(fcerl.BackgroundDesc, "td", tdStyle), "tr"));
                Body.Append(Method.BuildXML2(Method.BuildXML2("Tester", "th", thStyle) + Method.BuildXML2(fcerl.LabMember, "td", tdStyle), "tr"));
                Body.Append(Method.BuildXML2(Method.BuildXML2("Analysis Result", "th", thStyle) + Method.BuildXML2(fcerl.AnalysisResult, "td", tdStyle), "tr"));
                Body.Append(Method.BuildXML2(Method.BuildXML2("Comment", "th", thStyle) + Method.BuildXML2(fcerl.Comment, "td", tdStyle), "tr"));
                Body.Append(Method.BuildXML2(Method.BuildXML2("已完成階段", "th", thStyle) + Method.BuildXML2(fp.inStateName, "td", tdStyle), "tr"));
                Body.Append(Method.BuildXML2(Method.BuildXML2("下個階段", "th", thStyle) + Method.BuildXML2(fp.outStateName, "td", tdStyle), "tr"));

                string EmailTitile = "";
                string outStateTitle = fp.outStateName;
                StringBuilder classstyle = new StringBuilder("<style type='text/css'>");
                classstyle.Append("table{table-layout: fixed;width:100%;border:1px solid;}");
                classstyle.Append("th{color:#ffffff;background:#0078ae;font-weight:bold;width:30%;border:1px solid;}");
                classstyle.Append("td{width:70%;border:1px solid;}");
                classstyle.Append("</style>");
                string tmpBody = "<table style='" + tableStyle + "'>" + Body.ToString() + "</table>";
                Body.Clear();
                Body.Append(classstyle.ToString());
                Body.Append("您有一張 ");
                Body.Append(outStateTitle);
                Body.Append(" 的" + fcerl.ParentTestItem + "表單");
                Body.Append(tmpBody);
                Body.Append(newLine2);
                Body.Append(Method.BuildXML2(Method.BuildXML2("請進入此網址查看<a href='" + FormUrl + ID + "'>CERLLAB</a>", "td"), "tr"));

                EmailTitile = Constant.MailTest + "[" + fcerl.UID + ", " + fcerl.CaseID + "] CERLLAB " + outStateTitle + " 通知";

                var maillist = edb.FnGetMailList(fcerl.ListAssignTo).Select(v => v.Email).ToList();
                //var maillist = db.mail_test.Select(v => v.Email).ToList();
                errorline = 364;
                List<string> conItemList = new List<string>();
                foreach (var item in maillist)
                {
                    conItemList.Add(item);
                }
                errorline = 370;
                List<string> CCUserList = new List<string>();
                var sfauserList = db.s_form_authority.Where(x => x.fID == fcerl.fID && x.outState == fp.outState);
                string ccmailcontext = "";
                if (sfauserList != null)
                {
                    if (sfauserList.Count() > 0)
                    {
                        var sfauser = sfauserList.FirstOrDefault();
                        if (sfauser.MemberCodeList != null)
                            ccmailcontext = sfauser.MemberCodeList;

                    }
                }
                errorline = 384;
                if (ccmailcontext != null)
                {
                    if (ccmailcontext.Trim().Length > 0)
                    {
                        maillist = edb.FnGetMailList2(ccmailcontext).Select(x => x.Email).ToList();
                        foreach (var item in maillist)
                        {
                            CCUserList.Add(item);
                        }
                    }
                }
                errorline = 396;
                try
                {
                    errorline = 399;
                    Method.SendMail(conItemList, CCUserList, EmailTitile, Body.ToString());
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Issue at line 406: occur error after line " + errorline.ToString() + msg);
                    sb.Append(newLine);
                    sb.Append(fcerl.ID + " " + fcerl.State);
                    syserrdb.InitErrorData(Src: fnSendMail, content: sb.ToString(), editor: UserId);

                    conItemList.Clear();
                    conItemList.Add(Constant.DefaultMailBcc);
                    CCUserList.Clear();
                    Body.Append(newLine);
                    Body.Append(ex.Message);
                    Method.SendMail(conItemList, CCUserList, EmailTitile, Body.ToString());
                }
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                StringBuilder sb = new StringBuilder();
                sb.Append("Issue at line 423: occur error after line " + errorline.ToString() + msg);
                sb.Append(newLine);
                sb.Append(fcerl.ID + " " + fcerl.State);
                syserrdb.InitErrorData(Src: fnSendMail, content: sb.ToString(), editor: UserId);
            }
            return Body.ToString();
        }

        //
        // Test: /DownloadJson/
        /*
        FlowPath objfp = (FlowPath)Newtonsoft.Json.JsonConvert.DeserializeObject(response);
        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
        FlowPath fp = (FlowPath)jsonSerializer.DeserializeObject(response);

        var objfp = jsonSerializer.DeserializeObject(response);
        jsonSerializer.ConvertToType(objfp, typeof(FlowPath));
        var fp = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<FlowPath>>(response);

        JsonTextReader reader = new JsonTextReader();
        var fp = (FlowPath)jsonSerializer.DeserializeObject(response);
        byte[] data = System.Text.Encoding.Default.GetBytes(JsonString);
        */

    }
}