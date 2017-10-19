using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Data.Linq.SqlClient;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Collections.Specialized;
using CERLLAB.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Serialization;
using CERLLAB.Controllers.General;
using System.Web.Script.Serialization;

using System.IO;
using System.Web.Routing;

namespace CERLLAB.Controllers
{
    public class F_CERLController : Controller
    {
        private CERLDBContext db = new CERLDBContext();
        private CERLEntities edb = new CERLEntities();

        public ActionResult SimulateUser(string UserId, int RoleId=2)
        {
            if(UserId != null)
                Method.SetLogonUser(Session, this, UserId.ToString());

            InitDDL("UserRoleList", UserId, RoleId, null);
            Constant.UserRoleList = ((SelectList)ViewData["UserRoleList"]).ToList();
            if (Constant.UserRoleId != RoleId)
                Constant.UserRoleId = RoleId;

            return RedirectToAction("Index", "F_CERL");
        }

        protected void InitDDL2(string ddlName, string root, int iWhere, vFCERL fcerl)
        {
            string[] stringArray = { "siteList", "testList", "requestList" };
            string[] MemberArray = { "SupervisorList", "LocalSupervisorList", "LabMemberList" };
            var initlist = edb.FnGeneralDropDownList(ddlName, root).Select(x => new { Id = x.Id, Name = x.Name }).ToList();
            if (stringArray.Contains(ddlName))
            {
                initlist = edb.FnTestItemMenuDropDownList(root.ToString()).Where(y => y.lvl == iWhere).Select(x => new { Id = x.Id, Name = x.Name }).ToList();
            }

            if (MemberArray.Contains(ddlName))
            {
                initlist = edb.FnMemberDropDownList(ddlName, root).Select(x => new { Id = x.BadgeCode, Name = x.Name }).ToList();
            }

            string selectedvalue = "";
            if (fcerl != null)
            {
                switch (ddlName)
                {
                    case "siteList":
                        selectedvalue = fcerl.Site.ToString();
                        break;
                    case "testList":
                        selectedvalue = fcerl.TestItem.ToString();
                        break;
                    case "requestList":
                        selectedvalue = fcerl.RequestItem.ToString();
                        break;

                    case "LabMemberList":
                        selectedvalue = fcerl.LabMember;
                        break;
                    case "ProcessStepList":
                        selectedvalue = fcerl.ProcessStep.ToString();
                        break;
                    case "TestPurposeList":
                        selectedvalue = fcerl.TestPurpose.ToString();
                        break;
                    case "IssueSourceList":
                        selectedvalue = fcerl.IssueSource.ToString();
                        break;
                    case "FailureSiteList":
                        selectedvalue = fcerl.FailureSite.ToString();
                        break;
                    case "ReturnTypeList":
                        selectedvalue = fcerl.ReturnType.ToString();
                        break;
                    case "CustomerNameList":
                        selectedvalue = fcerl.CustomerID.ToString();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (ddlName)
                {
                    case "UserRoleList":
                        selectedvalue = iWhere.ToString();
                        break;
                    default:
                        break;
                }
            }

            List<SelectListItem> initList = new List<SelectListItem>();
            initList.Add(new SelectListItem()
            {
                Text = "",
                Value = ""
            });

            foreach (var item in initlist)
            {
                initList.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = (item.Id.ToString() == selectedvalue)
                });
            }
            SelectList cList = new SelectList(initList, "Value", "Text");
            ViewData[ddlName] = cList;
            Session[ddlName] = cList;
        }

        protected void InitDDL(string ddlName, string root, int iWhere, f_cerl fcerl)
        {
            string[] stringArray = { "siteList", "testList", "requestList" };
            string[] MemberArray = { "SupervisorList", "LocalSupervisorList", "LabMemberList" };
            var initlist = edb.FnGeneralDropDownList(ddlName, root).Select(x => new { Id = x.Id, Name = x.Name }).ToList();
            if (stringArray.Contains(ddlName))
            {
                initlist = edb.FnTestItemMenuDropDownList(root.ToString()).Where(y => y.lvl == iWhere).Select(x => new { Id = x.Id, Name = x.Name }).ToList();
            }

            if (MemberArray.Contains(ddlName))
            {
                initlist = edb.FnMemberDropDownList(ddlName, root).Select(x => new { Id = x.BadgeCode, Name = x.Name }).ToList();
            }

            string selectedvalue = "";
            if (fcerl != null)
            {
                switch (ddlName)
                {
                    case "siteList":
                        selectedvalue = fcerl.Site.ToString();
                        break;
                    case "testList":
                        selectedvalue = fcerl.TestItem.ToString();
                        break;
                    case "requestList":
                        selectedvalue = fcerl.RequestItem.ToString();
                        break;
                    case "SupervisorList":
                        selectedvalue = fcerl.Supervisor;
                        break;
                    case "LocalSupervisorList":
                        selectedvalue = fcerl.LocalSupervisor;
                        break;
                    case "LabMemberList":
                        selectedvalue = fcerl.LabMember;
                        break;
                    case "ProcessStepList":
                        selectedvalue = fcerl.ProcessStep.ToString();
                        break;
                    case "TestPurposeList":
                        selectedvalue = fcerl.TestPurpose.ToString();
                        break;
                    case "IssueSourceList":
                        selectedvalue = fcerl.IssueSource.ToString();
                        break;
                    case "FailureSiteList":
                        selectedvalue = fcerl.FailureSite.ToString();
                        break;
                    case "ReturnTypeList":
                        selectedvalue = fcerl.ReturnType.ToString();
                        break;
                    case "CustomerNameList":
                        selectedvalue = fcerl.CustomerID.ToString();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (ddlName)
                {
                    case "UserRoleList":
                        selectedvalue = iWhere.ToString();
                        break;
                    default:
                        break;
                }
            }

            List<SelectListItem> initList = new List<SelectListItem>();
            foreach (var item in initlist)
            {
                initList.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = (item.Id.ToString() == selectedvalue)
                });
            }
            SelectList cList = new SelectList(initList, "Value", "Text");
            ViewData[ddlName] = cList;
            Session[ddlName] = cList;
        }

        protected void InitDDLShow(f_cerl f_cerl, string action)
        {
            InitDDL("siteList", "0", 0, f_cerl);
            InitDDL("testList", "1000000", 1, f_cerl);
            InitDDL("requestList", "1001100", 0, f_cerl);
            InitDDL("ReturnTypeList", "0", 0, f_cerl);
            InitDDL("CustomerNameList", "0", 0, f_cerl);
            InitDDL("FailureSiteList", "0", 0, f_cerl);
            InitDDL("IssueSourceList", "0", 0, f_cerl);
            InitDDL("TestPurposeList", "0", 0, f_cerl);
            InitDDL("ProcessStepList", "0", 0, f_cerl);
            InitDDL("SupervisorList", "20", 0, f_cerl);
            InitDDL("UserRoleList", Constant.LogonUserId, 0, f_cerl);
            //InitDDL("LocalSupervisor", f_cerl.TestItem.ToString(), 0, f_cerl);
            if (action.ToUpper() == "EDIT")
            {
                InitDDL("LocalSupervisorList", f_cerl.TestItem.ToString(), 0, f_cerl);
                InitDDL("LabMemberList", f_cerl.TestItem.ToString(), 0, f_cerl);
            }
            else
            {
                InitDDL("LocalSupervisorList", "30", 0, f_cerl);
                InitDDL("LabMemberList", "40", 0, f_cerl);
            }
        }

        // Download FormAction
        public IEnumerable<FormActions> GetFormAction(int FlowCode, int State)
        {
            string ApiUrl = @"http://localhost:56491/api/task/GetFormAction?FlowCode=" + FlowCode.ToString() + "&State=" + State.ToString();
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.UseDefaultCredentials = true;
                client.Encoding = System.Text.Encoding.UTF8;
                client.Credentials = new NetworkCredential(@"IEC\mdcadmin", "manager");

                string response = client.DownloadString(ApiUrl);

                var fp = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                IEnumerable<FormActions> objfp = (IEnumerable<FormActions>)Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<FormActions>>(fp.ToString());

                return objfp;
            };
        }

        // Download FormTaskDetail
        public IEnumerable<vTaskDetail> GetTaskDetail(string fID)
        {
            string ApiUrl = @"http://localhost:56491/api/task/GetTaskDetail?fID=" + fID;
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.UseDefaultCredentials = true;
                client.Encoding = System.Text.Encoding.UTF8;
                client.Credentials = new NetworkCredential(@"IEC\mdcadmin", "manager");

                //var obj = client.DownloadData(ApiUrl);
                string response = client.DownloadString(ApiUrl);

                var fp = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                IEnumerable<vTaskDetail> objfp = (IEnumerable<vTaskDetail>)Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<vTaskDetail>>(fp.ToString());

                return objfp;
            };

        }

        public IEnumerable<vTaskDetail> GetUnsignedTask(string BadgeCode)
        {
            string ApiUrl = @"http://localhost:56491/api/task/GetUnsignedTask?BadgeCode=" + BadgeCode;
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.UseDefaultCredentials = true;
                client.Encoding = System.Text.Encoding.UTF8;
                client.Credentials = new NetworkCredential(@"IEC\mdcadmin", "manager");

                //var obj = client.DownloadData(ApiUrl);
                string response = client.DownloadString(ApiUrl);

                var fp = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                IEnumerable<vTaskDetail> objfp = (IEnumerable<vTaskDetail>)Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<vTaskDetail>>(fp.ToString());

                return objfp;
            };

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

        // Download Route Task FlowPath
        public FlowPath GetFlowPath(string fID, int FlowCode, int State, string strAction)
        {
            //http://localhost:56491/api/Task/getFlowPath?fID=A38AFFA3-4F99-447A-93F3-72FE3207DE1A&FlowCode=1001001&State=10&strAction=A
            string ApiUrl = @"http://localhost:56491/api/task/getFlowPath?fID=" + fID + "&FlowCode=" + FlowCode.ToString() + "&State=" + State.ToString() + "&strAction=" + strAction;
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.UseDefaultCredentials = true;
                client.Encoding = System.Text.Encoding.UTF8;
                client.Credentials = new NetworkCredential(@"IEC\mdcadmin", "manager");
                //var obj = client.DownloadData(ApiUrl);
                string response = client.DownloadString(ApiUrl);

                var fp = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                FlowPath objfp = (FlowPath)Newtonsoft.Json.JsonConvert.DeserializeObject<FlowPath>(fp.ToString());

                return objfp;
            };
        }

        public ActionResult GetFilePath(int fileId, bool record)
        {
            if (fileId == 0)
            {
                return View();
            }
            string result = (from f in db.attachFiles.Where(x => x.fileId == fileId)
                             select f.filePath + f.fileName).FirstOrDefault();

            var attachFile = (from f in db.attachFiles.Where(x => x.fileId == fileId)
                              select f).FirstOrDefault();

            //string fullFilePath =  Server.MapPath(result);
            if (!string.IsNullOrEmpty(result) && System.IO.File.Exists(result))
            {
                return File(System.IO.File.ReadAllBytes(result), "application/unknown", HttpUtility.UrlEncode(Path.GetFileName(result)));
            }
            else
                return View("File not exists: " + result);
        }

        public HttpResponseMessage PostFormDataToRoute(FormCollection fc, f_cerl f_cerl)
        {
            string UserId = Constant.LogonUserId;
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
            //string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(dyndata);

            using (var wb = new WebClient())
            {
                wb.Headers.Set("CONTENT-TYPE", "application/x-www-form-urlencoded");
                wb.UseDefaultCredentials = true;
                wb.Credentials = new NetworkCredential(@"IEC\IEC891652", "bryan8265");

                string url = Url.RouteUrl(@"DefaultApi", new { action = "Task", controller = "api" });
                if (url == null)
                {
                    url = @"http://localhost:56491/api/task/Post";
                }
                byte[] rslt = wb.UploadValues(url, dyndata);
            }
            return response;
        }

        public void GetListAssignTo(int? outState, string vchSet, out string rslt)
        {
            string ListAssignTo = edb.FnGetAssignTo(outState, vchSet).FirstOrDefault().ListAssitnTo;
            rslt = ListAssignTo.ToString();
        }

        public void InitAttachFiles(string fID)
        {
            AttachFileSet attSet = new AttachFileSet();
            ViewData["attFile10"] = attSet.GetFiles(fID, 10);
            ViewData["attFile35"] = attSet.GetFiles(fID, 35);
            ViewData["attFile40"] = attSet.GetFiles(fID, 40);
        }

        //
        // GET: /F_CERL/
        //http://localhost:56614/CERLLAB/F_CERL/Report/1002000?searchString=&Applicant=&FromDate=&EndDate=
        public ActionResult Report(int? id, string searchString, string LabMember, string Applicant, string SerialNumber,  string UID, string CaseID, int? CustomerID, string FromDate, string EndDate)
        {
            InitDDL2("CustomerNameList", "0", 0, null);
            DateTime dtFDate = new DateTime();
            DateTime dtEDate = new DateTime();
            DateTime.TryParse(FromDate, out dtFDate);
            DateTime.TryParse(EndDate, out dtEDate);

            if(FromDate ==null)
            {
                dtFDate = DateTime.Today.AddYears(-3);
            }

            if(EndDate == null)
            {
                dtEDate = DateTime.Today;
            }

            int ItemId = -1;
            if (id != null)
            {
                int.TryParse(id.ToString(), out ItemId);
                ViewBag.id = id = ItemId;
            }
            else
                ViewBag.id = id = ItemId;

            var sitelist = db.f_cerl.Where(x => x.Site == ItemId).Select(v => v.ID).ToList();
            var parenttestlist = db.vFCERL.Where(x => x.ParentTestItemId == ItemId).Select(v => v.ID).ToList();
            var testlist = db.f_cerl.Where(x => x.TestItem == ItemId).Select(v => v.ID).ToList();
            var Requestlist = db.f_cerl.Where(x => x.RequestItem == ItemId).Select(v => v.ID).ToList();
            var Customerlist = db.f_cerl.Where(x => x.CustomerID == ItemId).Select(v => v.ID).ToList();

            IList<int> conItemList = new List<int>();
            foreach(var item in sitelist)
            {
                conItemList.Add(item);
            }

            foreach (var item in parenttestlist)
            {
                if (!conItemList.Contains(item))
                    conItemList.Add(item);
            }

            foreach (var item in testlist)
            {
                if (!conItemList.Contains(item))
                    conItemList.Add(item);
            }

            foreach (var item in Requestlist)
            {
                if (!conItemList.Contains(item))
                    conItemList.Add(item);
            }

            foreach (var item in Customerlist)
            {
                if (!conItemList.Contains(item))
                    conItemList.Add(item);
            }

            var applicantlist = db.vFCERL.Where(x => conItemList.Contains(x.ID) && (x.ApplicantId.Contains(Applicant)) || x.ApplicantId.Contains(Applicant)).Select(v => v.ID).ToList();
            var labmemberlist = db.vFCERL.Where(x => conItemList.Contains(x.ID) && (x.LabMember.Contains(LabMember) || x.LabMemberId.Contains(LabMember))).Select(v => v.ID).ToList();
            
            foreach (var item in labmemberlist)
            {
                if (!conItemList.Contains(item))
                    conItemList.Add(item);
            }

            foreach (var item in applicantlist)
            {
                if (!conItemList.Contains(item))
                    conItemList.Add(item);
            }

            IQueryable<vFCERL> vfcerl = db.vFCERL.Where(x => conItemList.Contains(x.ID) && x.State==1000 && (x.UID.Contains(UID) || x.CaseID.Contains(CaseID)));
            return View(vfcerl);
        }

        //
        // GET: /F_CERL/

        public ActionResult Index(int? id)
        {
            if (id != null)
                ViewBag.id = id;
            else
                ViewBag.id = id = 0;

            string UserId = Method.GetLogonUserId(Session, User.Identity.Name);
            IEnumerable<vTaskDetail> objfp = GetUnsignedTask(UserId);
            var unsinged = objfp.Select(x => x.FID);
            return View(db.vFCERL.Where(x=>x.State<1000 && unsinged.Contains(x.fID)).ToList());
        }

        //
        // GET: /F_CERL/Details/5

        public ActionResult Details(int id = 0)
        {
            vFCERL vfcerl = db.vFCERL.Find(id);
            if (vfcerl == null)
            {
                return HttpNotFound();
            }
            InitAttachFiles(vfcerl.fID);

            IEnumerable<vTaskDetail> vTaskDetail = GetTaskDetail(vfcerl.fID);
            ViewData["vTaskDetail"] = vTaskDetail;
            return View(vfcerl);
        }

        //
        // GET: /F_CERL/Create
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTestItemMenu(string root, string iWhere)
        {
            int iiWhere = int.Parse(iWhere);
            var initlist = edb.FnTestItemMenuDropDownList(root.ToString()).Where(y => y.lvl == iiWhere).Select(x => new { Id = x.Id, Name = x.Name }).ToList();

            return Json(initlist, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDivShow(string root)
        {
            //var initlist = edb.FnGetDivShow(root).Select(x => new { Id = x.DivId }).ToList();
            string DivShow = edb.FnGetDivShow(root).FirstOrDefault().DivId;

            return Json(DivShow, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetReturnSite(string iWhere)
        {
            //InitDDLShow();
            int iiWhere = int.Parse(iWhere);
            var initlist = db.ReturnSite.Where(y => y.ReturnTypeID == iiWhere).Select(x => new { Id = x.SiteID, Name = x.SiteNAME }).ToList();
            return Json(initlist, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetLocalSupervisor(string root)
        {
            var initlist = edb.FnMemberDropDownList("LocalSupervisorList", root.ToString()).Select(x => new { Id = x.BadgeCode, Name = x.Name }).ToList();
            return Json(initlist, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetLabMember(string root)
        {
            var initlist = edb.FnMemberDropDownList("LabMemberList", root.ToString()).Select(x => new { Id = x.BadgeCode, Name = x.Name }).ToList();
            return Json(initlist, JsonRequestBehavior.AllowGet);
        }

        //protected void InitItemMenuDDL(string ddlName, int root, int iWhere)
        //{

        //    var initlist = edb.FnTestItemMenuDropDownList(root.ToString()).Where(y => y.lvl <= iWhere).Select(x => new { Id = x.Id, Name = x.Name }).ToList();
        //    List<SelectListItem> initList = new List<SelectListItem>();
        //    foreach (var item in initlist)
        //    {
        //        initList.Add(new SelectListItem()
        //        {
        //            Text = item.Name,
        //            Value = item.Id.ToString()
        //        });
        //    }

        //    SelectList cList = new SelectList(initList, "Value", "Text", "");
        //    ViewData[ddlName] = cList;
        //}

        //protected void InitGeneralDDL(string ddlName, int root, int iWhere)
        //{
        //    var initlist = edb.FnGeneralDropDownList(ddlName, root).Select(x => new { Id = x.Id, Name = x.Name }).ToList();
        //    List<SelectListItem> initList = new List<SelectListItem>();
        //    foreach (var item in initlist)
        //    {
        //        initList.Add(new SelectListItem()
        //        {
        //            Text = item.Name,
        //            Value = item.Id.ToString()
        //        });
        //    }

        //    SelectList cList = new SelectList(initList, "Value", "Text", "");
        //    ViewData[ddlName] = cList;
        //}

        public ActionResult Create()
        {
            int State = 10;
            int FlowCode = 1001001;
            IEnumerable<FormActions> formactions = GetFormAction(FlowCode, State);
            ViewData["FormAction"] = formactions;
            InitDDLShow(null, "CREATE");
            return View();
        }

        //
        // POST: /F_CERL/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection fc, f_cerl f_cerl)
        {
            StringBuilder vchSet = new StringBuilder();
            string Editor = Method.GetLogonUserId(Session, User.Identity.Name);                    
            string State = "10";
            int FlowCode = 1001001;
            string FormId = System.Guid.NewGuid().ToString().ToUpper();
            vchSet.Append(Method.BuildXML(f_cerl.TestItem.ToString(), "TestItem"));
            vchSet.Append(Method.BuildXML(f_cerl.CustomerID.ToString(), "CustomerID"));
            vchSet.Append(Method.BuildXML(f_cerl.ProjectName, "ProjectName"));
            string CaseId = edb.FnGetCaseID(vchSet.ToString()).FirstOrDefault().CaseID;
            string UID = edb.FnGetCaseID(vchSet.ToString()).FirstOrDefault().UID;

            f_cerl.fID = FormId;
            f_cerl.FlowCode = FlowCode;
            f_cerl.FormCode = int.Parse(f_cerl.TestItem.ToString());
            f_cerl.CaseID = CaseId;
            f_cerl.UID = UID;
            f_cerl.State = int.Parse(State);
            f_cerl.editor = Editor;
            f_cerl.Applicant = Editor;
            f_cerl.cdt = DateTime.Now;
            f_cerl.udt = DateTime.Now;
            if (TryUpdateModel(f_cerl, "", fc.AllKeys, new string[] { "fID", "Manager", "FlowCode", "FormCode", "Applicant", "editor", "State", "udt", "cdt" }))
            {
                if (ModelState.IsValid)
                {
                    if (f_cerl.Manager == null)
                    {
                        f_cerl.Manager = "IEC970209"; // test Manager
                    }

                    var r = new List<attachFile>();
                    int i = 0;


                    foreach (string file in Request.Files)
                    {
                        HttpPostedFileBase hpf = Request.Files[i] as HttpPostedFileBase;
                        if (hpf.ContentLength == 0)
                            continue;

                        string formId = FormId;
                        string state = State;
                        string filePath0 = Constant.UserFileDirectory + formId + @"\";
                        string filePath = Constant.UserFileDirectory + formId + @"\" + state + @"\";
                        FileInfo newinfo = new FileInfo(hpf.FileName);
                        string savedFileName = Path.Combine(filePath, Path.GetFileName(newinfo.Name));
                        if (!Directory.Exists(filePath0))
                        {
                            Directory.CreateDirectory(filePath0);
                        }

                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        hpf.SaveAs(savedFileName);

                        r.Add(new attachFile()
                        {
                            fID = formId,
                            displayname = newinfo.Name,
                            fileName = newinfo.Name,
                            Length = hpf.ContentLength,
                            Type = hpf.ContentType,
                            folderId = state,
                            editor = Editor,
                            filePath = filePath,

                            cdt = DateTime.Now,
                            udt = DateTime.Now
                        });

                        i++;
                    }
                    foreach (attachFile a in r)
                    {
                        db.attachFiles.Add(a);
                    }

                    f_cerl = CheckAndPushTask(fc, f_cerl);

                    db.f_cerl.Add(f_cerl);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(f_cerl);
        }

        //
        // GET: /F_CERL/Edit/5

        public f_cerl CheckAndPushTask(FormCollection fc, f_cerl f_cerl)
        {
            string strAction = f_cerl.Action;
            string ListAssignTo = "";
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            StringBuilder vchSet = new StringBuilder();
            FlowPath fp = GetFlowPath(f_cerl.fID, f_cerl.FlowCode, f_cerl.State, f_cerl.Action); // 取得下一關的State

            if (fp == null)
                return f_cerl;

            if (f_cerl.State >= 1000)
            { 
                strAction = "";
                return f_cerl;
            }

            if (fp != null)
            {
                if (f_cerl.LocalSupervisor == null)
                    f_cerl.LocalSupervisor = "IEC970209";
                vchSet.Append(Method.BuildXML(fp.outState.ToString(), "outState"));
                vchSet.Append(Method.BuildXML(f_cerl.Supervisor, "Supervisor"));
                vchSet.Append(Method.BuildXML(f_cerl.LocalSupervisor, "LocalSupervisor"));
                vchSet.Append(Method.BuildXML(f_cerl.LabMember, "LabMember"));
                vchSet.Append(Method.BuildXML(f_cerl.Applicant, "Applicant"));
                vchSet.Append(Method.BuildXML(f_cerl.Manager, "Manager"));
                vchSet.Append(Method.BuildXML(f_cerl.Action, ""));
                GetListAssignTo(fp.outState, vchSet.ToString(), out ListAssignTo); //取得下一關的收件人
                f_cerl.ListAssignTo = ListAssignTo;

                if(ListAssignTo != "")
                    response = PostFormDataToRoute(fc, f_cerl); //Push Task, Send Task mail by AssignTo
                
                f_cerl.State = int.Parse(fp.outState.ToString());
            }
            return f_cerl;
        }

        public ActionResult Edit(int id = 0)
        {
            f_cerl f_cerl = db.f_cerl.Find(id);
            if (f_cerl == null)
            {
                return HttpNotFound();
            }

            //initDDL();
            //FlowPath flowpath = GetFlowPath(f_cerl.fID, f_cerl.FlowCode, f_cerl.State, "A");
            //if (flowpath != null)
            //{
             //   if (flowpath.inStateAction != null)
              //  {
                    IEnumerable<FormActions> formactions = GetFormAction(f_cerl.FlowCode, f_cerl.State);
                    ViewData["FormAction"] = formactions;
               // }
            //}
            //if (f_cerl.TestItem == 2002100)
            //{
            
            //}

            IEnumerable<vTaskDetail> vTaskDetail = GetTaskDetail(f_cerl.fID);
            ViewData["vTaskDetail"] = vTaskDetail;

            InitDDLShow(f_cerl, "EDIT");
            InitAttachFiles(f_cerl.fID);
            return View(f_cerl);
        }

        //
        // POST: /F_CERL/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(FormCollection fc, f_cerl f_cerl)
        {
            if (f_cerl.State >= 1000)
            {
                InitDDLShow(null, "EDIT");
                return View(f_cerl);
            }
           
            string UserId = Constant.LogonUserId;
            if (ModelState.IsValid)
            {
                if (TryUpdateModel(f_cerl, "", fc.AllKeys, new string[] { "cdt" }))
                {
                    f_cerl = CheckAndPushTask(fc, f_cerl);
                    f_cerl.udt = DateTime.Now;
                    f_cerl.editor = UserId;

                    var r = new List<attachFile>();
                    int i = 0;

                    foreach (string file in Request.Files)
                    {

                        HttpPostedFileBase hpf = Request.Files[i] as HttpPostedFileBase;
                        if (hpf.ContentLength == 0)
                            continue;

                        string formId = f_cerl.fID;
                        string state = f_cerl.State.ToString();
                        string filePath0 = Constant.UserFileDirectory + formId + @"\";
                        string filePath = Constant.UserFileDirectory + formId + @"\" + state + @"\";
                        FileInfo newinfo = new FileInfo(hpf.FileName);
                        string savedFileName = Path.Combine(filePath, Path.GetFileName(newinfo.Name));
                        if (!Directory.Exists(filePath0))
                        {
                            Directory.CreateDirectory(filePath0);
                        }

                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        hpf.SaveAs(savedFileName);
                        int count = db.attachFiles.Where(x => x.fID == formId && x.folderId == state && x.fileName == newinfo.Name).Count();

                        if (count == 0)
                        {
                            r.Add(new attachFile()
                            {
                                fID = formId,
                                displayname = newinfo.Name,
                                fileName = newinfo.Name,
                                Length = hpf.ContentLength,
                                Type = hpf.ContentType,
                                folderId = state,
                                editor = Constant.LogonUserId,
                                filePath = filePath,

                                cdt = DateTime.Now,
                                udt = DateTime.Now
                            });
                        }
                        i++;
                    }
                    foreach (attachFile a in r)
                    {
                        db.attachFiles.Add(a);
                    }
                }

                db.Entry(f_cerl).State = EntityState.Modified;
                db.SaveChanges();
                
                //return RedirectToAction("Index");

                RouteValueDictionary rv = new RouteValueDictionary();
                rv.Add("id", f_cerl.ID);
                return RedirectToAction("Details",rv);
                //InitDDLShow();
                //return RedirectToAction("Index");

            }
            return View(f_cerl);
        }

        //
        // GET: /F_CERL/Delete/5

        public ActionResult Delete(int id = 0)
        {
            f_cerl f_cerl = db.f_cerl.Find(id);
            if (f_cerl == null)
            {
                return HttpNotFound();
            }
            return View(f_cerl);
        }

        //
        // POST: /F_CERL/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            f_cerl f_cerl = db.f_cerl.Find(id);
            db.f_cerl.Remove(f_cerl);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}