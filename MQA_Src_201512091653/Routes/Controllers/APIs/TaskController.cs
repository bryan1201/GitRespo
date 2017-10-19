using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using Routes.Models;
using System.Text;
using Routes.Controllers.General;
using System.Collections.Specialized;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Data;
using System.Data.Entity;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Net.Mail;

namespace Routes.Controllers.APIs
{
    public class TaskController : ApiController
    {
        RouteDBEntities edb = new RouteDBEntities();
        RouteDBContext db = new RouteDBContext();
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string GetFlowStateList(int FlowCode)
        {
            //var task = edb.FnGetTask(fID, State, Applicant);
            IQueryable<FnGetFlowStateList_Result> flowstatelist = edb.FnGetFlowStateList(FlowCode);

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            var json = jsonSerializer.Serialize(flowstatelist);

            return json;
        }

        public string GetFlowPath(string fID, int FlowCode, int State, string strAction)
        {
            //var task = edb.FnGetTask(fID, State, Applicant);
            FnFlowPathTreeList_Result flowpath = edb.FnFlowPathTreeList(FlowCode, State, strAction).FirstOrDefault();
            
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            var json = jsonSerializer.Serialize(flowpath);
            
            //var json = JsonConvert.SerializeObject(flowpath);
            return json;
        }

        public string GetFormAction(int FlowCode, int State)
        {
            //var task = edb.FnGetTask(fID, State, Applicant);
            IQueryable<FnGetFormAction_Result> formaction = edb.FnGetFormAction(FlowCode, State);
            
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            var json = jsonSerializer.Serialize(formaction);

            //var json = JsonConvert.SerializeObject(flowpath);
            return json;
        }

        public string GetAllTask2(string jsonFIDString)
        {
            var fp = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonFIDString);
            IList<string> ListFID = (IList<string>)Newtonsoft.Json.JsonConvert.DeserializeObject<IList<string>>(fp.ToString());
            var vTask = db.vTask.Where(x => ListFID.Contains(x.FID));

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(vTask);

            return json;
        }

        public string GetAllTask(string strAction)
        {
            if (strAction == null)
                strAction = "";
            strAction = strAction.Trim().ToUpper();
            var vTask = db.vTask.Where(x => x.Action != "");
            if (strAction == "")
                vTask = db.vTask.Where(x => x.Action == "");

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(vTask);

            return json;
        }

        public string GetUnsignedTask(string BadgeCode)
        {
            var vTask = db.vTask.Where(x => x.AssignedTo == BadgeCode && x.Action == ""); 
            if(BadgeCode == "")
                vTask = db.vTask.Where(x => x.Action == "");

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(vTask);

            return json;
        }

        public string GetSignedTask(string BadgeCode)
        {
            var vTask = db.vTask.Where(x => x.Editor == BadgeCode && x.Action != "");

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(vTask);

            return json;
        }

        public string GetTaskDetail(string fID)
        {
            var vTask = db.vTask.Where(x => x.FID == fID);
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(vTask);

            return json;
        }

        /*
        public String GetFlowPath(string fID)
        {
            int inState = db.vTask.Where(x => x.FID == fID).Select(t => t.CurrentState).FirstOrDefault();
            if (inState <= 0)
                inState = 10;
            var flowpath = new FlowPath();
            flowpath.Paths = db.r_flow_path.ToList()
                .Select(t => new r_flow_path { outState = t.outState, inState = t.inState })
                .ToDictionary(t => t.PathID);
            flowpath.RootPath = new r_flow_path {inState=inState};
            flowpath.BuildPath();
            return JsonConvert.SerializeObject(flowpath);
        }
        */
        // POST api/<controller>
        public void Post(FormDataCollection values)
        {
            string FormCode = values.Get("FormCode");
            string FlowCode = values.Get("FlowCode");
            string Applicant = values.Get("Applicant");
            string Editor = values.Get("Editor");
            string Comment = values.Get("Comment");
            string State = values.Get("State");
            string ListAssignTo = values.Get("ListAssignTo");
            string Action = values.Get("Action");
            var FID = values.Get("FID");
            string sp_Route = "sp_Route";
            string vchCmd = "PUSH";
            string vchObject = "TASK";
            string TaskID = System.Guid.NewGuid().ToString();
            StringBuilder vchSet = new StringBuilder();
            vchSet.Append(Method.BuildXML(FID.ToString(), "FID"));
            vchSet.Append(Method.BuildXML(FormCode, "FormCode"));
            vchSet.Append(Method.BuildXML(FlowCode, "FlowCode"));
            vchSet.Append(Method.BuildXML(Applicant, "Applicant"));
            vchSet.Append(Method.BuildXML(Editor, "Editor"));
            vchSet.Append(Method.BuildXML(Comment, "Comment"));
            vchSet.Append(Method.BuildXML(State, "InState"));
            vchSet.Append(Method.BuildXML(ListAssignTo, "ListAssignTo"));
            vchSet.Append(Method.BuildXML(Action, "Action"));
            string sqlCmd = Method.GetSqlCmd(sp_Route, vchCmd, vchObject, vchSet.ToString());
            DAO.sqlCmd(Constant.ConnRouteDBContext, sqlCmd);

            try {
                if (Constant.IsTracking == true)
                {
                    MailMessage mail = new MailMessage();
                    MailAddress mailadd = new MailAddress(Constant.DefaultMailFrom, "MQA Routes Job Tracking");
                    MailAddress mailbcc = new MailAddress(Constant.DefaultMailBcc, "MQA Routes Developers");
                    mail.IsBodyHtml = true;
                    mail.Subject = "[Test] Task Notify mail";
                    mail.Body = "sqlCmd:" + sqlCmd;
                    mail.From = mailadd;
                    mail.To.Add(mailadd);
                    SmtpClient smtp = new SmtpClient();
                    smtp.Credentials = new NetworkCredential(Constant.NetworkCredentialUserId, Constant.NetworkCredentialPWD);
                    smtp.Host = Constant.DefaultMailServer.ToString();
                    smtp.Send(mail);
                }
            }
            catch { 
            //
            }
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
            

        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}