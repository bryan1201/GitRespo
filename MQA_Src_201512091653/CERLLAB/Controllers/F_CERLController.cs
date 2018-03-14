using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CERLLAB.Models;
using CERLLAB.Controllers.General;

using System.IO;
using System.Web.Routing;
using System.Security.Principal;
using PagedList.Mvc;
using PagedList;

using LinqKit;
using System.Data.Entity;


namespace CERLLAB.Controllers
{

    public static class clsCompiledQuery
    {
        private static CERLDBContext db = new CERLDBContext();

        public static Func<DataContext, List<string>, IQueryable<vFCERL>>
            getFCERL = CompiledQuery.Compile((DataContext db, List<string> unsigned)
                => from objFCERL in db.GetTable<vFCERL>()
                   where unsigned.Contains(objFCERL.fID)
                   select objFCERL);

        public static List<string> ConditionItemList2(List<string> conItemList, queryCondition query)
        {
            StringBuilder sbwhereclause = new StringBuilder();
            string UserId = query.UserId;
            DateTime dtFDate = new DateTime();
            DateTime dtEDate = new DateTime();
            DateTime.TryParse(query.FromDate, out dtFDate);
            DateTime.TryParse(query.EndDate, out dtEDate);

            DateTime dtNextTestFDate = new DateTime();
            DateTime dtNextTestEDate = new DateTime();
            DateTime.TryParse(query.NextTestFromDate, out dtNextTestFDate);
            DateTime.TryParse(query.NextTestEndDate, out dtNextTestEDate);

            if (query.FromDate == null || query.FromDate.Trim().Length == 0)
            {
                dtFDate = DateTime.Today.AddYears(-10);
                query.FromDate = dtFDate.ToShortDateString();
            }

            if (query.EndDate == null || query.EndDate.Trim().Length == 0)
            {
                dtEDate = DateTime.Today.AddDays(1.0);
                query.EndDate = dtEDate.ToShortDateString();
            }
            else
            {
                dtEDate = dtEDate.AddDays(1.0);
            }

            if (query.NextTestFromDate == null || query.NextTestFromDate.Trim().Length == 0)
            {
                dtNextTestFDate = DateTime.Today.AddYears(-10);
                query.NextTestFromDate = dtNextTestFDate.ToShortDateString();
            }

            if (query.NextTestEndDate == null || query.NextTestEndDate.Trim().Length == 0)
            {
                dtNextTestEDate = DateTime.Today.AddDays(1.0);
                query.NextTestEndDate = dtNextTestEDate.ToShortDateString();
            }
            else
            {
                dtNextTestEDate = dtNextTestEDate.AddDays(1.0);
            }

            int ItemId = -1;
            if (query.id < 0)
            {
                int.TryParse(query.id.ToString(), out ItemId);
                query.id = ItemId;
            }
            else
                ItemId = query.id;

            bool blhascond = false;
            int ihascond = 0;
            if (!(query.FromDate == null || query.FromDate.Trim().Length == 0))
            {
                sbwhereclause.Append(" cdt >= ");
                sbwhereclause.Append(dtFDate.ToShortDateString());
                blhascond = true;
                ihascond++;
            }

            if (!(query.EndDate == null || query.EndDate.Trim().Length == 0))
            {
                if (blhascond == true)
                    sbwhereclause.Append(" AND ");
                sbwhereclause.Append(" udt <= ");
                sbwhereclause.Append(dtEDate.ToShortDateString());
                blhascond = true;
            }

            if (query.Applicant != "" && query.Applicant != null)
            {
                if (blhascond == true)
                    sbwhereclause.Append(" AND ");
                sbwhereclause.Append(" (ApplicantId LIKE '%");
                sbwhereclause.Append(query.Applicant);
                sbwhereclause.Append("%' OR Applicant LIKE '%");
                sbwhereclause.Append(query.Applicant);
                sbwhereclause.Append("%')");
                blhascond = true;
            }

            if (query.LabMember != "" && query.LabMember != null)
            {
                if (blhascond == true)
                    sbwhereclause.Append(" AND ");
                sbwhereclause.Append(" (LabMemberId LIKE '%");
                sbwhereclause.Append(query.LabMember);
                sbwhereclause.Append("%' OR LabMember LIKE '%");
                sbwhereclause.Append(query.LabMember);
                sbwhereclause.Append("%')");
                blhascond = true;
            }

            if (query.FlowState > 0)
            {
                if (blhascond == true)
                    sbwhereclause.Append(" AND ");
                sbwhereclause.Append(" State = ");
                sbwhereclause.Append(query.FlowState.ToString());
                blhascond = true;
            }

            if (!(query.SerialNumber == null || query.SerialNumber.Trim().Length == 0))
            {
                if (blhascond == true)
                    sbwhereclause.Append(" AND ");
                sbwhereclause.Append(" (SerialNumber LIKE '%");
                sbwhereclause.Append(query.SerialNumber);
                sbwhereclause.Append("%' OR PartNumber LIKE '%");
                sbwhereclause.Append(query.SerialNumber);
                sbwhereclause.Append("%')");
                blhascond = true;
            }

            if (query.CustomerID > 0)
            {
                if (blhascond == true)
                    sbwhereclause.Append(" AND ");
                sbwhereclause.Append(" CustomerID = ");
                sbwhereclause.Append(query.CustomerID.ToString());
            }

            if (query.UID != null)
            {
                if (blhascond == true)
                    sbwhereclause.Append(" AND ");
                sbwhereclause.Append(" TOUPPER(UID) LIKE '%");
                sbwhereclause.Append(query.UID.ToUpper());
                sbwhereclause.Append("%'");
            }

            if (!(query.NextTestFromDate == null || query.NextTestFromDate.Trim().Length == 0))
            {
                if (blhascond == true)
                    sbwhereclause.Append(" AND ");
                sbwhereclause.Append(" NextTestDate >= ");
                sbwhereclause.Append(dtNextTestFDate.ToShortDateString());
                blhascond = true;
            }

            if (!(query.NextTestEndDate == null || query.NextTestEndDate.Trim().Length == 0))
            {
                if (blhascond == true)
                    sbwhereclause.Append(" AND ");
                sbwhereclause.Append(" NextTestDate <= ");
                sbwhereclause.Append(dtNextTestEDate.ToShortDateString());
                blhascond = true;
            }

            if (!(query.CaseID == null || query.CaseID.Trim().Length == 0))
            {
                if (blhascond == true)
                    sbwhereclause.Append(" AND ");
                sbwhereclause.Append(" TOUPPER(CaseID) LIKE '%");
                sbwhereclause.Append(query.CaseID.ToUpper());
                sbwhereclause.Append("%'");
            }
            return conItemList;
        }

        public static List<string> ConditionItemList3(List<string> conItemList, queryCondition query)
        {
            string UserId = query.UserId;
            DateTime dtFDate = new DateTime();
            DateTime dtEDate = new DateTime();
            DateTime.TryParse(query.FromDate, out dtFDate);
            DateTime.TryParse(query.EndDate, out dtEDate);

            DateTime dtNextTestFDate = new DateTime();
            DateTime dtNextTestEDate = new DateTime();
            DateTime.TryParse(query.NextTestFromDate, out dtNextTestFDate);
            DateTime.TryParse(query.NextTestEndDate, out dtNextTestEDate);

            if (query.FromDate == null || query.FromDate.Trim().Length == 0)
            {
                dtFDate = DateTime.Today.AddYears(-10);
                query.FromDate = dtFDate.ToShortDateString();
            }

            if (query.EndDate == null || query.EndDate.Trim().Length == 0)
            {
                dtEDate = DateTime.Today.AddDays(1.0);
                query.EndDate = dtEDate.ToShortDateString();
            }
            else
            {
                dtEDate = dtEDate.AddDays(1.0);
            }

            if (query.NextTestFromDate == null || query.NextTestFromDate.Trim().Length == 0)
            {
                dtNextTestFDate = DateTime.Today.AddYears(-10);
                query.NextTestFromDate = dtNextTestFDate.ToShortDateString();
            }

            if (query.NextTestEndDate == null || query.NextTestEndDate.Trim().Length == 0)
            {
                dtNextTestEDate = DateTime.Today.AddDays(1.0);
                query.NextTestEndDate = dtNextTestEDate.ToShortDateString();
            }
            else
            {
                dtNextTestEDate = dtNextTestEDate.AddDays(1.0);
            }

            int ItemId = -1;
            if (query.id < 0)
            {
                int.TryParse(query.id.ToString(), out ItemId);
                query.id = ItemId;
            }
            else
                ItemId = query.id;

            IQueryable<vFCERL> oDataQuery = db.vFCERL;

            try
            {
                if (!(query.FromDate == null || query.FromDate.Trim().Length == 0))
                {
                    oDataQuery = oDataQuery.Where(x => conItemList.Contains(x.fID) && x.cdt >= dtFDate);
                }

                if (!(query.EndDate == null || query.EndDate.Trim().Length == 0))
                {
                    oDataQuery = oDataQuery.Where(x => x.udt <= dtEDate);
                }

                if (query.Applicant != "" && query.Applicant != null)
                {
                    oDataQuery = oDataQuery.Where(x => x.ApplicantId.Contains(query.Applicant) || x.Applicant.Contains(query.Applicant));
                }

                if (query.LabMember != "" && query.LabMember != null)
                {
                    oDataQuery = oDataQuery.Where(x => x.LabMember.Contains(query.LabMember) || x.LabMemberId.Contains(query.LabMember));
                }

                if (query.FlowState > 0)
                {
                    int iState = int.Parse(query.FlowState.ToString());
                    oDataQuery = oDataQuery.Where(x => x.State == iState);
                }

                if (!(query.SerialNumber == null || query.SerialNumber.Trim().Length == 0))
                {
                    oDataQuery = oDataQuery.Where(x => x.SerialNumber.Contains(query.SerialNumber) || x.PartNumber.Contains(query.SerialNumber));
                }

                if (query.CustomerID > 0)
                {
                    oDataQuery = oDataQuery.Where(x => x.CustomerID == query.CustomerID);
                }

                if (query.UID != null)
                {
                    oDataQuery = oDataQuery.Where(x => x.UID.Contains(query.UID));
                }

                if (!(query.NextTestFromDate == null || query.NextTestFromDate.Trim().Length == 0))
                {
                    oDataQuery = oDataQuery.Where(x => x.cdt >= dtNextTestFDate);
                }

                if (!(query.NextTestEndDate == null || query.NextTestEndDate.Trim().Length == 0))
                {
                    oDataQuery = oDataQuery.Where(x => x.cdt <= dtNextTestEDate);
                }

                if (!(query.CaseID == null || query.CaseID.Trim().Length == 0))
                {
                    oDataQuery = oDataQuery.Where(x => x.CaseID.Contains(query.CaseID));
                }

                conItemList = oDataQuery.Select(v => v.fID).ToList();
                return conItemList;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
        // bad performance, do not to use it anymore.
        public static List<string> ConditionItemList(List<string> conItemList, queryCondition query)
        {
            string UserId = query.UserId;
            DateTime dtFDate = new DateTime();
            DateTime dtEDate = new DateTime();
            DateTime.TryParse(query.FromDate, out dtFDate);
            DateTime.TryParse(query.EndDate, out dtEDate);

            DateTime dtNextTestFDate = new DateTime();
            DateTime dtNextTestEDate = new DateTime();
            DateTime.TryParse(query.NextTestFromDate, out dtNextTestFDate);
            DateTime.TryParse(query.NextTestEndDate, out dtNextTestEDate);

            if (query.FromDate == null || query.FromDate.Trim().Length == 0)
            {
                dtFDate = DateTime.Today.AddYears(-10);
                query.FromDate = dtFDate.ToShortDateString();
            }

            if (query.EndDate == null || query.EndDate.Trim().Length == 0)
            {
                dtEDate = DateTime.Today.AddDays(1.0);
                query.EndDate = dtEDate.ToShortDateString();
            }
            else
            {
                dtEDate = dtEDate.AddDays(1.0);
            }

            if (query.NextTestFromDate == null || query.NextTestFromDate.Trim().Length == 0)
            {
                dtNextTestFDate = DateTime.Today.AddYears(-10);
                query.NextTestFromDate = dtNextTestFDate.ToShortDateString();
            }

            if (query.NextTestEndDate == null || query.NextTestEndDate.Trim().Length == 0)
            {
                dtNextTestEDate = DateTime.Today.AddDays(1.0);
                query.NextTestEndDate = dtNextTestEDate.ToShortDateString();
            }
            else
            {
                dtNextTestEDate = dtNextTestEDate.AddDays(1.0);
            }

            int ItemId = -1;
            if (query.id < 0)
            {
                int.TryParse(query.id.ToString(), out ItemId);
                query.id = ItemId;
            }
            else
                ItemId = query.id;

            List<string> userlist;
            List<string> removelist;

            try
            {
                if (!(query.FromDate == null || query.FromDate.Trim().Length == 0))
                {
                    userlist = db.vFCERL.Where(x => conItemList.Contains(x.fID) && x.cdt >= dtFDate).Select(v => v.fID).ToList();
                    removelist = db.vFCERL.Where(x => !userlist.Contains(x.fID)).Select(v => v.fID).ToList();
                    foreach (var item in removelist)
                    {
                        if (conItemList.Contains(item))
                            conItemList.Remove(item);
                    }
                }

                if (!(query.EndDate == null || query.EndDate.Trim().Length == 0))
                {
                    userlist = db.vFCERL.Where(x => conItemList.Contains(x.fID) && x.udt <= dtEDate).Select(v => v.fID).ToList();
                    removelist = db.vFCERL.Where(x => !userlist.Contains(x.fID)).Select(v => v.fID).ToList();
                    foreach (var item in removelist)
                    {
                        if (conItemList.Contains(item))
                            conItemList.Remove(item);
                    }
                }

                if (query.Applicant != "" && query.Applicant != null)
                {
                    userlist = db.vFCERL.Where(x => conItemList.Contains(x.fID) && (x.ApplicantId.Contains(query.Applicant) || x.Applicant.Contains(query.Applicant))).Select(v => v.fID).ToList();
                    removelist = db.vFCERL.Where(x => !userlist.Contains(x.fID)).Select(v => v.fID).ToList();
                    foreach (var item in removelist)
                    {
                        if (conItemList.Contains(item))
                            conItemList.Remove(item);
                    }
                }

                if (query.LabMember != "" && query.LabMember != null)
                {
                    userlist = db.vFCERL.Where(x => conItemList.Contains(x.fID) && (x.LabMember.Contains(query.LabMember) || x.LabMemberId.Contains(query.LabMember))).Select(v => v.fID).ToList();
                    removelist = db.vFCERL.Where(x => !userlist.Contains(x.fID)).Select(v => v.fID).ToList();
                    foreach (var item in removelist)
                    {
                        if (conItemList.Contains(item))
                            conItemList.Remove(item);
                    }
                }

                if (query.FlowState > 0)
                {
                    int iState = int.Parse(query.FlowState.ToString());
                    userlist = db.vFCERL.Where(x => x.State == iState).Select(v => v.fID).ToList();
                    removelist = db.vFCERL.Where(x => !userlist.Contains(x.fID)).Select(v => v.fID).ToList();
                    foreach (var item in removelist)
                    {
                        if (conItemList.Contains(item))
                            conItemList.Remove(item);
                    }
                }

                if (!(query.SerialNumber == null || query.SerialNumber.Trim().Length == 0))
                {
                    userlist = db.vFCERL.Where(x => conItemList.Contains(x.fID) && (x.SerialNumber.Contains(query.SerialNumber) || x.PartNumber.Contains(query.SerialNumber))).Select(v => v.fID).ToList();
                    removelist = db.vFCERL.Where(x => !userlist.Contains(x.fID)).Select(v => v.fID).ToList();
                    foreach (var item in removelist)
                    {
                        if (conItemList.Contains(item))
                            conItemList.Remove(item);
                    }
                }

                if (query.CustomerID > 0)
                {
                    removelist = db.vFCERL.Where(x => conItemList.Contains(x.fID) && x.CustomerID != query.CustomerID).Select(v => v.fID).ToList();
                    foreach (var item in removelist)
                    {
                        if (conItemList.Contains(item))
                            conItemList.Remove(item);
                    }
                }

                if (query.UID != null)
                {
                    userlist = db.vFCERL.Where(x => conItemList.Contains(x.fID) && x.UID.Contains(query.UID)).Select(v => v.fID).ToList();
                    removelist = db.vFCERL.Where(x => !userlist.Contains(x.fID)).Select(v => v.fID).ToList();
                    foreach (var item in removelist)
                    {
                        if (conItemList.Contains(item))
                            conItemList.Remove(item);
                    }
                }

                if (!(query.NextTestFromDate == null || query.NextTestFromDate.Trim().Length == 0))
                {
                    userlist = db.vFCERL.Where(x => conItemList.Contains(x.fID) && x.NextTestDate >= dtNextTestFDate).Select(v => v.fID).ToList();
                    removelist = db.vFCERL.Where(x => !userlist.Contains(x.fID)).Select(v => v.fID).ToList();
                    foreach (var item in removelist)
                    {
                        if (conItemList.Contains(item))
                            conItemList.Remove(item);
                    }
                }

                if (!(query.NextTestEndDate == null || query.NextTestEndDate.Trim().Length == 0))
                {
                    userlist = db.vFCERL.Where(x => conItemList.Contains(x.fID) && x.NextTestDate <= dtNextTestEDate).Select(v => v.fID).ToList();
                    removelist = db.vFCERL.Where(x => !userlist.Contains(x.fID)).Select(v => v.fID).ToList();
                    foreach (var item in removelist)
                    {
                        if (conItemList.Contains(item))
                            conItemList.Remove(item);
                    }
                }

                if (!(query.CaseID == null || query.CaseID.Trim().Length == 0))
                {
                    userlist = db.vFCERL.Where(x => x.CaseID.Contains(query.CaseID)).Select(v => v.fID).ToList();
                    removelist = db.vFCERL.Where(x => !userlist.Contains(x.fID)).Select(v => v.fID).ToList();
                    foreach (var item in removelist)
                    {
                        if (conItemList.Contains(item))
                            conItemList.Remove(item);
                    }
                }

                return conItemList;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
    }

    public class F_CERLController : Controller
    {
        private CERLDBContext db = new CERLDBContext();
        private CERLEntities edb = new CERLEntities();
        private sysErrorMessageDBSet syserrdb = new sysErrorMessageDBSet();
        //private RouteWork routewk = new RouteWork((WindowsIdentity)HttpContext.User.Identity, fcontroller: this, session: Session);

        public ActionResult SimulateUser(string UserId, int RoleId=2)
        {
            UserId = (!(UserId == null || UserId.Trim().Length == 0)) ? UserId.ToUpper().Trim() : this.User.Identity.Name.ToUpper();
            Method.SetLogonUser(Session, this, UserId);
            UserId = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());// Constant.LogonUserId;

            InitDDL("UserRoleList", UserId, RoleId, null, null);
            Constant.UserRoleList = ((SelectList)ViewData["UserRoleList"]).ToList();
            Constant.UserRoleId = (Constant.UserRoleId != RoleId) ? RoleId : Constant.UserRoleId;

            return RedirectToAction("Index", "F_CERL");
        }

        private void InitDDL(string ddlName, string root, int iWhere, f_cerl fcerl, string Type)
        {
            string[] stringArray = { "siteList", "testList", "requestList" };
            string[] MemberArray = { "SupervisorList", "LocalSupervisorList", "LabMemberList" };
            string[] FlowArray = { "flowstateList"};
            string[] IDArray = { "ReturnTypeList" };
            string[] WhereArray = { "ProductStage" };
            var initlist = edb.FnGeneralDropDownList(ddlName, root).Select(x => new { Id = x.Id, Name = x.Name }).OrderBy(x => x.Name).ToList();
            if(IDArray.Contains(ddlName))
                initlist = edb.FnGeneralDropDownList(ddlName, root).Select(x => new { Id = x.Id, Name = x.Name }).OrderBy(x => x.Id).ToList();

            if (WhereArray.Contains(ddlName))
                initlist = edb.FnGeneralDropDownList(ddlName, iWhere.ToString()).Select(x => new { Id = x.Id, Name = x.Name }).OrderBy(x => x.Name).ToList();

            if (stringArray.Contains(ddlName))
            {
                initlist = edb.FnTestItemMenuDropDownList(root.ToString()).Where(y => y.lvl == iWhere).Select(x => new { Id = x.Id, Name = x.Name }).OrderBy(x=>x.Id).ToList();
            }

            if (MemberArray.Contains(ddlName))
            {
                initlist = edb.FnMemberDropDownList(ddlName, root).Select(x => new { Id = x.BadgeCode, Name = x.Name }).OrderBy(x=>x.Name).ToList();
            }

            if (FlowArray.Contains(ddlName))
            {
                int[] notstate = { 15, 18, 45 };
                RouteWork routewk = new RouteWork((WindowsIdentity)HttpContext.User.Identity, fcontroller: this, session: Session);
                var olist = routewk.GetFlowStateList(int.Parse(Constant.FcerlFlowCode)).Where(x=>x.State<1000 && x.State>10 && !notstate.Contains(x.State)).Select(x => new { Id = x.State, Name = x.Title }).ToList();
                initlist = Enumerable.Empty<object>().Select(r => new { Id = "", Name = "" }).ToList();
                foreach(var item in olist)
                {
                    initlist.Add(new 
                    {
                        Id = item.Id.ToString(),
                        Name = item.Name
                    });
                }
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
                    case "ProductStage":
                        selectedvalue = fcerl.ProductStageID.ToString();
                        break;
                    case "PCBVendor":
                        selectedvalue = fcerl.PCBVendorID.ToString();
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
            string[] stringSpecicalArray = { "ProductStage", "ReturnTypeList", "FailureSiteList", "TestPurposeList", "IssueSourceList", "ProcessStepList" };
            if (!(Type == null || Type.Trim().Length == 0) || stringSpecicalArray.Contains(ddlName))
            {
                initList.Add(new SelectListItem()
                {
                    Text = "",
                    Value = "-1"
                });
            }
            //var orderlist = initlist.OrderBy(x => x.Name).ToList();
            var orderlist = initlist.ToList();
            foreach (var item in orderlist)
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
            string UserId = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            InitDDL("siteList", "0", 0, f_cerl, null);

            if(f_cerl == null)
            {
                SelectList cList = (SelectList)ViewData["siteList"];
                string Site = cList.First().Value;
                InitDDL("requestList", Site, 1, f_cerl, null);
                cList = (SelectList)ViewData["requestList"];
                string TestItem = cList.First().Value;
                //string Site = "1000000";
                //string TestItem = "1001100";

                InitDDL("testList", Site, 1, f_cerl, null);
                InitDDL("requestList", TestItem, 0, f_cerl, null);
                InitDDL("LocalSupervisorList", TestItem, 0, f_cerl, null);
                InitDDL("LabMemberList", TestItem, 0, f_cerl, "LABMember");
                InitDDL("ProductStage", "0", 1, f_cerl, null);
            }
            else
            {
                InitDDL("testList", f_cerl.Site.Value.ToString(), 1, f_cerl, null);
                InitDDL("requestList", f_cerl.TestItem.Value.ToString(), 0, f_cerl, null);
                InitDDL("ProductStage", "0", f_cerl.CustomerID.Value, f_cerl, null);
            }

            InitDDL("PCBVendor", "0", 0, f_cerl, null);
            
            InitDDL("ReturnTypeList", "0", 0, f_cerl, null);
            InitDDL("CustomerNameList", "0", 0, f_cerl, null);
            InitDDL("FailureSiteList", "0", 0, f_cerl, null);
            InitDDL("IssueSourceList", "0", 0, f_cerl, null);
            InitDDL("TestPurposeList", "0", 0, f_cerl, null);
            InitDDL("ProcessStepList", "0", 0, f_cerl, null);
            InitDDL("SupervisorList", "20", 0, f_cerl, null);
            InitDDL("UserRoleList", UserId, 0, f_cerl, null);
            //InitDDL("LocalSupervisor", f_cerl.TestItem.ToString(), 0, f_cerl);
            string[] actions = { "EDIT"};
            if (actions.Contains(action.ToUpper()))
            {
                InitDDL("LocalSupervisorList", f_cerl.TestItem.ToString(), 0, f_cerl, null);
                InitDDL("LabMemberList", f_cerl.TestItem.ToString(), 0, f_cerl, "LABMember");
            }
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

        public ActionResult DeleteFile(int fileId, bool record)
        {
            if (fileId == 0)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            string result = (from f in db.attachFiles.Where(x => x.fileId == fileId)
                             select f.filePath + f.fileName).FirstOrDefault();

            var attachFile = (from f in db.attachFiles.Where(x => x.fileId == fileId)
                              select f).FirstOrDefault();

            //string fullFilePath =  Server.MapPath(result);
            if (!string.IsNullOrEmpty(result) && System.IO.File.Exists(result))
            {
                FileInfo newinfo = new FileInfo(result);
                newinfo.Delete();
                db.attachFiles.Remove(attachFile);
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
                return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult GetFileVerPath(int fileId, bool record)
        {
            if (fileId == 0)
            {
                return View();
            }
            string result = (from f in db.attachFileVer.Where(x => x.fileId == fileId)
                             select f.filePath + f.fileName).FirstOrDefault();

            var attachFile = (from f in db.attachFileVer.Where(x => x.fileId == fileId)
                              select f).FirstOrDefault();

            //string fullFilePath =  Server.MapPath(result);
            if (!string.IsNullOrEmpty(result) && System.IO.File.Exists(result))
            {
                return File(System.IO.File.ReadAllBytes(result), "application/unknown", HttpUtility.UrlEncode(Path.GetFileName(result)));
            }
            else
                return View("File not exists: " + result);
        }

        public ActionResult DeleteFileVer(int fileId, bool record)
        {
            if (fileId == 0)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            string result = (from f in db.attachFileVer.Where(x => x.fileId == fileId)
                             select f.filePath + f.fileName).FirstOrDefault();

            var attachFile = (from f in db.attachFileVer.Where(x => x.fileId == fileId)
                              select f).FirstOrDefault();

            //string fullFilePath =  Server.MapPath(result);
            if (!string.IsNullOrEmpty(result) && System.IO.File.Exists(result))
            {
                FileInfo newinfo = new FileInfo(result);
                newinfo.Delete();
                db.attachFileVer.Remove(attachFile);
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
                return Redirect(Request.UrlReferrer.ToString());
        }

        public void InitCCUsers(string fID)
        {
            string Editor = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            CCUserSet ccset = new CCUserSet();
            ViewData["CCUser"] = ccset.GetFormAuthority(fID, Editor);
        }

        public void InitAttachFiles(string fID)
        {
            //尚未結案不可顯示作業中的附件給申請者，但可給所有的LAB人員查看
            AttachFileVerMaxSet attSet = new AttachFileVerMaxSet();
            ViewData["attFile10"] = attSet.GetFiles(fID, "10");
            ViewData["attFile35"] = attSet.GetFiles(fID, "35");
            ViewData["attFile40"] = attSet.GetFiles(fID, "40");
            ViewData["attFile1000"] = attSet.GetFiles(fID, "1000");

            AttachFileSet attVerSet = new AttachFileSet();
            ViewData["attFileVer35"] = attVerSet.GetFiles(fID, "35");
            //ViewData["attFileLabInformation"] = attSet.GetFiles(fID, "LabInformation");
        }

        public ActionResult ReportOnHandWork()
        {
            //IEnumerable<vTaskDetail> objtask = GetAllTask(strAction);
            //IList<string> objfcerl = db.vFCERL.Where(x => x.State < 1000 && x.State > 10).Select(x => x.fID).ToList();
            //IEnumerable<vTaskDetail> objtask = GetAllTask2(objfcerl);
            //IEnumerable<vTaskDetail> onhandtask = objtask.Where(x => objfcerl.Contains(x.FID) && x.Action == "");

            IEnumerable<LABMemberOnHandWork> onhandwork = from t in db.vCountOnHandWork
                                                       select new LABMemberOnHandWork
                                                       {
                                                           LABMember = t.LABMember,
                                                           Site = "",
                                                           TestItem = "",
                                                           RequestItem = "",
                                                           JobNumber = t.JobNumber
                                                       };

            IEnumerable<LABMemberOnHandWork> LabMemberonhandwork = from t in db.vLABMemberOnHandWork
                                                                   select new LABMemberOnHandWork
                                                                {
                                                                    LABMember = t.LABMember,
                                                                    Site = "",
                                                                    TestItem = "",
                                                                    RequestItem = "",
                                                                    JobNumber = t.JobNumber
                                                                };

            ViewData["OnHandWork"] = onhandwork.OrderByDescending(x=>x.JobNumber);
            ViewData["LABMemberOnHandWork"] = LabMemberonhandwork.OrderByDescending(x => x.JobNumber);
            return View(onhandwork);
        }

        //
        // GET: /F_CERL/
        public ActionResult Report(int? id, string searchString, string LabMember, string Applicant, string SerialNumber,  string UID, string CaseID, int? CustomerID, string FromDate, string EndDate)
        {
            InitDDL("CustomerNameList", "0", 0, null, "Report");
            queryCondition query = new queryCondition();
            int outint = -1;
            query.id = int.TryParse(id.ToString(), out outint) ? outint : outint;
            query.searchString = searchString;
            query.LabMember = LabMember;
            query.Applicant = Applicant;
            query.UID = UID;
            query.CaseID = CaseID;
            query.SerialNumber = SerialNumber;
            outint = -1;
            query.CustomerID = int.TryParse(CustomerID.ToString(), out outint) ? outint : outint;
            query.FromDate = FromDate;
            query.EndDate = EndDate;
            query.FlowState = 1000;
            
            IList<int> conItemList = new List<int>();
            //conItemList = ConditionItemList(conItemList, query);

            var sitelist = db.f_cerl.Where(x => x.Site == query.id && x.State == query.FlowState).Select(v => v.ID).ToList();
            var parenttestlist = db.vFCERL.Where(x => x.ParentTestItemId == query.id && x.State == query.FlowState).Select(v => v.ID).ToList();
            var testlist = db.f_cerl.Where(x => x.TestItem == query.id && x.State == query.FlowState).Select(v => v.ID).ToList();
            var Requestlist = db.f_cerl.Where(x => x.RequestItem == query.id && x.State == query.FlowState).Select(v => v.ID).ToList();

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

            conItemList = ConditionItemList(conItemList, query);

            IEnumerable<vFCERL> vfcerl = new List<vFCERL>();

            string UserId = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            // string Readers = FnGetReader(x.Applicant+';'+x.AssignTo+';'+x.FormAuthorityMemberList+';')
            // Readers.Contains(UserId)
            
            var user = db.vUsers.Where(x => x.BadgeCode.Equals(UserId) && x.RoleId.Equals(2)).FirstOrDefault(); //只是一般使用者
            if (user != null)
            {
                List<int> ReaderList = GetFormAuthroity2(UserId);
                //一般使用者只能看自己的及部門的報告
                vfcerl = db.vFCERL.Where(x => conItemList.Contains(x.ID) && x.State == 1000 && ReaderList.Contains(x.ID)).OrderByDescending(x => x.UID).Take(Constant.iOutResultLimit);
            }
            else
            {
                //管理員、Supervisor、Lab Members、PQA稽核人員可以看所有的報告
                vfcerl = db.vFCERL.Where(x => conItemList.Contains(x.ID) && x.State == 1000).OrderByDescending(x => x.UID).Take(Constant.iOutResultLimit);
            }
            return View(vfcerl);
        }

        public List<int> GetFormAuthroity2(string UserId)
        {
            System.Data.SqlClient.SqlParameter p1 = new System.Data.SqlClient.SqlParameter{
                ParameterName = "UserId",
                DbType = DbType.String,
                Size=30,
                Direction = ParameterDirection.Input,
                Value=UserId
            };

            IEnumerable<int> IsReadAllow = db.Database.SqlQuery<int>("SELECT ID FROM FnGetFormAuthority2(@UserId)", p1);
            return IsReadAllow.ToList();
        }

        //
        // GET: /F_CERL/
        public ActionResult Index(int? id, string SerialNumber, string CaseID, string FromDate, string EndDate, int? page, string sortOrder)
        {
            int outint = -1;
            queryCondition query = new queryCondition();
            query.id = int.TryParse(id.ToString(), out outint) ? outint : outint;
            query.CaseID = CaseID;
            query.SerialNumber = SerialNumber;
            query.FromDate = FromDate;
            query.EndDate = EndDate;
            query.PageSize = Constant.iPageSize;
            string UserId = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            query.UserId = UserId;
            
            try
            {
                RouteWork routewk = new RouteWork((WindowsIdentity)HttpContext.User.Identity, fcontroller: this, session: Session);
                IEnumerable<vTaskDetail> objfp = routewk.GetUnsignedTask(UserId);
                List<string> unsinged = objfp.Select(x => x.FID).ToList();
                List<string> conItemList = new List<string>();
                conItemList = clsCompiledQuery.ConditionItemList3(unsinged, query);
                //RunAsunch(UserId).Wait();
                //GetUnsignedTask2(UserId);
                //IEnumerable<vTaskDetail> objfp = Ptask;
                query.FID = unsinged;

                var pageNumber = page ?? 1;
                query.PageIndex = pageNumber;
                DataContext objContext = new DataContext(db.Database.Connection.ConnectionString);
                //var vfcerl = clsCompiledQuery.getFCERL(objContext, conItemList);
                //var vfcerl = db.vFCERL.Where(x => x.State < 1000 && conItemList.Contains(x.fID)).OrderByDescending(x => x.UID);
                CERLDataContent dc = new CERLDataContent();

                var vfcerl = dc.sp_GetVFCERL(conItemList).Where(x=>x.State<1000);
                //vfcerl = db.vFCERL.Where(x => x.State < 1000 && unsinged.Contains(x.fID)).OrderByDescending(x => x.UID).Take(Constant.iOutResultLimit).ToList();
                //vfcerl = db.vFCERL.Where(x => x.State < 1000 && unsinged.Contains(x.fID)).OrderByDescending(x => x.UID);
                //IPagedList<vFCERLHeader> pfcerl = vfcerl.ToPagedList(query.PageIndex, query.PageSize);
                StaticPagedList<vFCERLHeader> pfcerlaspagelist = new StaticPagedList<vFCERLHeader>(vfcerl, query.PageIndex + 1, query.PageSize, vfcerl.Count());
                ViewBag.OnePageOfList = pfcerlaspagelist;
                ViewBag.queryContidion = query;
                return View(pfcerlaspagelist);
            }
            catch(Exception ex)
            {
                return View(ex.Message);
            }
        }

        public ActionResult Index2(int? id, string SerialNumber, string CaseID, string FromDate, string EndDate, int? page)
        {
            int outint = -1;
            queryCondition query = new queryCondition();
            query.id = int.TryParse(id.ToString(), out outint) ? outint : outint;
            query.CaseID = CaseID;
            query.SerialNumber = SerialNumber;
            query.FromDate = FromDate;
            query.EndDate = EndDate;
            string UserId = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            try
            {
                RouteWork routewk = new RouteWork((WindowsIdentity)HttpContext.User.Identity, fcontroller: this, session: Session);
                IEnumerable<vTaskDetail> objfp = routewk.GetUnsignedTask(UserId);
                IList<int> conItemList = new List<int>();
                conItemList = ConditionItemList(conItemList, query);
                //RunAsunch(UserId).Wait();
                //GetUnsignedTask2(UserId);
                //IEnumerable<vTaskDetail> objfp = Ptask;
                List<string> unsinged = (List<string>)objfp.Select(x => x.FID);
                query.FID = unsinged;
                IEnumerable<vFCERL> vfcerl = new List<vFCERL>();
                var pageNumber = page ?? 1;
                //vfcerl = db.vFCERL.Where(x => x.State < 1000 && unsinged.Contains(x.fID)).OrderByDescending(x => x.UID).Take(Constant.iOutResultLimit).ToList();
                vfcerl = db.vFCERL.Where(x => x.State < 1000 && unsinged.Contains(x.fID)).OrderByDescending(x => x.UID);
                IPagedList<vFCERL> pfcerl = vfcerl.ToPagedList(pageNumber, Constant.iPageSize);
                ViewBag.OnePageOfList = pfcerl;
                return View(pfcerl);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public ActionResult Draft()
        {
            string UserId = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            const string Action = "D";
            IQueryable<vFCERL> fcerl = db.vFCERL.Where(x => x.ApplicantId == UserId && x.State == 10 && x.Action == Action).OrderByDescending(x => x.UID);
            return View(fcerl);
        }

        public ActionResult Signed(int? id, string CaseID, string SerialNumber, string FromDate, string EndDate)
        {
            InitDDL("CustomerNameList", "0", 0, null, "Signed");
            queryCondition query = new queryCondition();
            int outint = -1;
            query.id = int.TryParse(id.ToString(), out outint) ? outint : outint;
            query.CaseID = CaseID;
            query.SerialNumber = SerialNumber;
            query.FromDate = FromDate;
            query.EndDate = EndDate;

            IList<int> conItemList = new List<int>();
            //conItemList = ConditionItemList(conItemList, query);

            string UserId = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            try
            {
                RouteWork routewk = new RouteWork((WindowsIdentity)HttpContext.User.Identity, fcontroller: this, session: Session);
                IEnumerable<vTaskDetail> objfp = routewk.GetSignedTask(UserId);
                var singdtasklist = objfp.Select(x => x.FID).ToList();
                var singedlist = db.f_cerl.Where(x => singdtasklist.Contains(x.fID)).Select(v => v.ID).ToList();

                foreach (var item in singedlist)
                {
                    conItemList.Add(item);
                }

                if (conItemList.Count() > 0)
                    conItemList = ConditionItemList(conItemList, query);
                
                IEnumerable<vFCERL> vfcerl = new List<vFCERL>();

                vfcerl = db.vFCERL.Where(x => conItemList.Contains(x.ID)).OrderByDescending(x => x.UID).Take(Constant.iOutResultLimit);
                return View(vfcerl);
            }
            catch(Exception ex)
            {
                return View(ex.Message);
            }
        }

        IList<int> ConditionItemList(IList<int> conItemList, queryCondition query)
        {
            DateTime dtFDate = new DateTime();
            DateTime dtEDate = new DateTime();
            DateTime.TryParse(query.FromDate, out dtFDate);
            DateTime.TryParse(query.EndDate, out dtEDate);

            DateTime dtNextTestFDate = new DateTime();
            DateTime dtNextTestEDate = new DateTime();
            DateTime.TryParse(query.NextTestFromDate, out dtNextTestFDate);
            DateTime.TryParse(query.NextTestEndDate, out dtNextTestEDate);

            if (query.FromDate == null || query.FromDate.Trim().Length == 0)
            {
                dtFDate = DateTime.Today.AddYears(-10);
                query.FromDate = dtFDate.ToShortDateString();
            }

            if (query.EndDate == null || query.EndDate.Trim().Length == 0)
            {
                dtEDate = DateTime.Today.AddDays(1.0);
                query.EndDate = dtEDate.ToShortDateString();
            }
            else
            {
                dtEDate = dtEDate.AddDays(1.0);
            }

            if (query.NextTestFromDate == null || query.NextTestFromDate.Trim().Length == 0)
            {
                dtNextTestFDate = DateTime.Today.AddYears(-10);
                query.NextTestFromDate = dtNextTestFDate.ToShortDateString();
            }

            if (query.NextTestEndDate == null || query.NextTestEndDate.Trim().Length == 0)
            {
                dtNextTestEDate = DateTime.Today.AddDays(1.0);
                query.NextTestEndDate = dtNextTestEDate.ToShortDateString();
            }
            else
            {
                dtNextTestEDate = dtNextTestEDate.AddDays(1.0);
            }

            int ItemId = -1;
            if (query.id < 0)
            {
                int.TryParse(query.id.ToString(), out ItemId);
                ViewBag.id = query.id = ItemId;
            }
            else
                ViewBag.id = ItemId = query.id;

            string UserId = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            try
            {
                IQueryable<vFCERL> oDataQuery = db.vFCERL;
                if (conItemList.Count > 0)
                    oDataQuery = oDataQuery.Where(x => conItemList.Contains(x.ID));

                if (query.Applicant != "" && query.Applicant != null)
                {
                    oDataQuery = oDataQuery.Where(x => (x.ApplicantId.Contains(query.Applicant)) || x.Applicant.Contains(query.Applicant));
                }

                if (query.LabMember != "" && query.LabMember != null)
                {
                    oDataQuery = oDataQuery.Where(x => x.LabMember.Contains(query.LabMember) || x.LabMemberId.Contains(query.LabMember));
                }

                if (query.FlowState>0)
                {
                    int iState = int.Parse(query.FlowState.ToString());
                    oDataQuery = oDataQuery.Where(x => x.State == iState);
                }

                if (!(query.SerialNumber == null || query.SerialNumber.Trim().Length == 0))
                {
                    oDataQuery = oDataQuery.Where(x => x.SerialNumber.Contains(query.SerialNumber) || x.PartNumber.Contains(query.SerialNumber));
                }

                if (query.CustomerID > 0)
                {
                    oDataQuery = oDataQuery.Where(x => x.CustomerID == query.CustomerID);
                }

                if (query.UID != null)
                {
                    oDataQuery = oDataQuery.Where(x => x.UID.Contains(query.UID));
                }

                if (!(query.FromDate == null || query.FromDate.Trim().Length == 0))
                {
                    oDataQuery = oDataQuery.Where(x => x.cdt >= dtFDate);
                }

                if (!(query.EndDate == null || query.EndDate.Trim().Length == 0))
                {
                    oDataQuery = oDataQuery.Where(x => x.udt <= dtEDate);
                }

                if (!(query.NextTestFromDate == null || query.NextTestFromDate.Trim().Length == 0))
                {
                    oDataQuery = oDataQuery.Where(x => x.cdt >= dtNextTestFDate);
                }

                if (!(query.NextTestEndDate == null || query.NextTestEndDate.Trim().Length == 0))
                {
                    oDataQuery = oDataQuery.Where(x => x.cdt <= dtNextTestEDate);
                }

                if (!(query.CaseID == null || query.CaseID.Trim().Length == 0))
                {
                    oDataQuery = oDataQuery.Where(x => x.CaseID.Contains(query.CaseID));
                }

                conItemList = oDataQuery.Select(v => v.ID).ToList();
                return conItemList;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

        public ActionResult LABMemberReport(int? id, string CaseID, string SerialNumber, string FromDate, string EndDate)
        {
            InitDDL("CustomerNameList", "0", 0, null, "Signed");
            queryCondition query = new queryCondition();
            query.id = int.Parse(id.ToString());
            query.CaseID = CaseID;
            query.SerialNumber = SerialNumber;
            query.FromDate = FromDate;
            query.EndDate = EndDate;

            IList<int> conItemList = new List<int>();
            conItemList = ConditionItemList(conItemList, query);
            string UserId = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            try
            {
                RouteWork routewk = new RouteWork((WindowsIdentity)HttpContext.User.Identity, fcontroller: this, session: Session);
                IEnumerable<vTaskDetail> objfp = routewk.GetSignedTask(UserId);
                var singdtasklist = objfp.Select(x => x.FID).ToList();
                var singedlist = db.f_cerl.Where(x => singdtasklist.Contains(x.fID)).Select(v => v.ID).ToList();

                foreach (var item in singedlist)
                {
                    conItemList.Add(item);
                }

                conItemList = ConditionItemList(conItemList, query);
                
                IEnumerable<vFCERL> vfcerl = new List<vFCERL>();
                vfcerl = db.vFCERL.Where(x => conItemList.Contains(x.ID) && x.State == 1000 && x.LabMemberId == UserId).OrderByDescending(x => x.UID).Take(Constant.iOutResultLimit);
                return View(vfcerl);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //http://localhost:56614/CERLLAB/F_CERL/ReportAllUnsignWork?CaseID=&FlowState=20&LabMember=&Applicant=
        public ActionResult ReportAllUnsignWork(int? id, string CaseID, int? FlowState, string SerialNumber, string LabMember, string Applicant)
        {
            if (id != null)
                ViewBag.id = id;
            else
                ViewBag.id = id = 0;

            return Unsigned(id, CaseID, FlowState, SerialNumber, LabMember, Applicant);
        }

        public ActionResult Unsigned(int? id, string CaseID, int? FlowState, string SerialNumber, string LabMember, string Applicant)
        {
            if (id.HasValue)
                ViewBag.id = id;
            else
                ViewBag.id = id = 0;

            var itemlist = edb.FnTestItemMenuDropDownList(id.ToString()).Select(x=>x.Id).ToList();
            IList<int> itemList = new List<int>();
            foreach (var item in itemlist)
            {
                int i = int.Parse(item);
                itemList.Add(i);
            }

            var sitelist = db.vFCERL.Where(x => itemList.Contains(x.SiteId)).Select(v => v.fID).ToList();
            var parenttestlist = db.vFCERL.Where(x => itemList.Contains(x.ParentTestItemId)).Select(v => v.fID).ToList();
            var testlist = db.vFCERL.Where(x => itemList.Contains(x.TestItemId)).Select(v => v.fID).ToList();
            var Requestlist = db.vFCERL.Where(x => itemList.Contains(x.RequestItemId)).Select(v => v.fID).ToList();

            List<string> conItemList = new List<string>();
            foreach (var item in sitelist)
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

            queryCondition query = new queryCondition();
            int outint = -1;
            query.id = int.TryParse(id.ToString(), out outint) ? outint : outint;
            query.CaseID = CaseID;
            outint = -1;
            query.FlowState = int.TryParse(FlowState.ToString(), out outint) ? outint : outint;
            query.SerialNumber = SerialNumber;
            query.LabMember = LabMember;
            query.Applicant = Applicant;

            conItemList = clsCompiledQuery.ConditionItemList3(conItemList, query);

            InitDDL("flowstateList", "0", 0, null, "Unsigned");
            ReportOnHandWork();

            CERLDataContent dc = new CERLDataContent();
            var vfcerl = dc.sp_GetVFCERL(conItemList).Where(x => x.State < 1000 && x.State >= 20 && conItemList.Contains(x.fID)).OrderByDescending(x => x.UID);
            //var vfcerl = db.vFCERL.Where(x => x.State < 1000 && x.State >= 20 && conItemList.Contains(x.ID)).OrderByDescending(x => x.UID).Take(Constant.iOutResultLimit);
            
            string UserId = Method.GetLogonUserId(Session, User.Identity.Name.ToUpper());
            string webroot = Constant.WebRoot;
            string TreeViewRoot = webroot + "/F_CERL/Unsigned";
            /*
            ItemTreeDbSet tr = new ItemTreeDbSet(TreeViewRoot);
             Mark by Bryan for no TreeView
            ItemTree tree = tr.GetData(TreeViewRoot, UserId, 0, mIsShowReport: 0, mIsShowUnsignForm: 1);
            ViewData["ItemTree"] = tree;
            

            IList<FnGetItemParentListById_Result> parentlist = tr.GetParentList(ViewBag.id);
            ViewData["parentlist"] = parentlist;
            */
            return View(vfcerl);
        }

        //
        // GET: /F_CERL/Details/5

        public ActionResult DetailsLAB(int id = 0)
        {   int? RoleId = null;
            string UserId = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            var user = db.vUsers.Where(x => x.BadgeCode == UserId && x.RoleId == 20).FirstOrDefault();
            if (user != null)
                RoleId = user.RoleId;

            f_cerl f_cerl = db.f_cerl.Find(id);
            FnGetFCERL_Result vfcerl = edb.FnGetFCERL(f_cerl.fID, UserId).FirstOrDefault();

            if (vfcerl == null)
            {
                return HttpNotFound();
            }
            InitAttachFiles(vfcerl.fID);

            RouteWork routewk = new RouteWork((WindowsIdentity)HttpContext.User.Identity, fcontroller: this, session: Session);
            IEnumerable<vTaskDetail> vTaskDetail = routewk.GetTaskDetail(vfcerl.fID);
            ViewData["vTaskDetail"] = vTaskDetail;

            int[] instate = { 40, 1000, 1100 };
            
            if (instate.Contains(f_cerl.State))
            {
                if (RoleId.HasValue)
                    Session["AnalysisSummary_Visible_LABSupervisor"] = true;
                else
                    Session["AnalysisSummary_Visible_LABSupervisor"] = false;
            }
            else
                Session["AnalysisSummary_Visible_LABSupervisor"] = false;

            InitCCUsers(vfcerl.fID);
            return View(vfcerl);
        }

        public ActionResult Details(int id = 0)
        {
            vFCERL vfcerl = db.vFCERL.Find(id);
            if (vfcerl == null)
            {
                return HttpNotFound();
            }
            InitAttachFiles(vfcerl.fID);

            RouteWork routewk = new RouteWork((WindowsIdentity)HttpContext.User.Identity, fcontroller: this, session: Session);
            IEnumerable<vTaskDetail> vTaskDetail = routewk.GetTaskDetail(vfcerl.fID);
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
            var initList = Enumerable.Empty<object>().Select(r => new { Id = "", Name = "" }).ToList();
            initList.Add(new 
            {
                Id = "",
                Name = ""
            });
            foreach (var item in initlist)
            {
                initList.Add(new
                {
                    Id = item.Id.ToString(),
                    Name = item.Name
                });
            }

            return Json(initList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDivShow(string root)
        {
            string DivShow = edb.FnGetDivShow(root).FirstOrDefault().DivId;
            return Json(DivShow, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetProductStage(string iWhere)
        {
            int iiWhere = int.Parse(iWhere);
            var initlist = db.ProductStage.Where(y => y.CustomerID == iiWhere).Select(x => new { Id = x.ID, Name = x.TextValue }).ToList();
            return Json(initlist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateLABSupplementReport(int? id, int FlowCode = 0, int State = 0, string UID = "")
        {
            State = (State <= 0) ? 35 : State;
            FlowCode = (FlowCode == 0) ? 1001002 : FlowCode;
            RouteWork routewk = new RouteWork((WindowsIdentity)HttpContext.User.Identity, fcontroller: this, session: Session);
            IEnumerable<FormActions> formactions = routewk.GetFormAction(FlowCode, State);
            ViewData["FormAction"] = formactions;
            f_cerl fcerl = db.f_cerl.Where(x => x.UID == UID && x.State==1000).OrderByDescending(x=>x.cdt).FirstOrDefault();
            
            if(fcerl!=null)
            {
                f_cerl newfcerl = new f_cerl();
                string newfID = newfcerl.CopyFCERL(fcerl, FlowCode, State);
                newfcerl.Applicant = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
                newfcerl.editor = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
                db.f_cerl.Add(newfcerl);
                db.SaveChanges();
                newfcerl = db.f_cerl.Where(y=>y.fID == newfID).FirstOrDefault();
                RouteValueDictionary rv = new RouteValueDictionary();
                rv.Add("id", newfcerl.ID);
                return RedirectToAction("EditLAB", rv);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Create(int? id, int FlowCode=0, int State=0)
        {
            RouteWork routewk = new RouteWork((WindowsIdentity)HttpContext.User.Identity, fcontroller: this, session: Session);
            InitDDLShow(null, "CREATE");
            State = (State <= 0) ? 10 : State;
            FlowCode = (FlowCode == 0) ? 1001001 : FlowCode;
            IEnumerable<FormActions> formactions = routewk.GetFormAction(FlowCode, State);//GetFormAction(FlowCode, State);
            formactions = formactions.Where(x => x.FormAction != "T");
            ViewData["FormAction"] = formactions;

            return View();
        }

        public FnGenInitInfo_Result GenFCERLInitInfo(f_cerl fcerl)
        {
            StringBuilder vchSet = new StringBuilder();
            vchSet.Append(Method.BuildXML(fcerl.Site.ToString(), "Site"));
            vchSet.Append(Method.BuildXML(fcerl.TestItem.ToString(), "TestItem"));
            vchSet.Append(Method.BuildXML(fcerl.CustomerID.ToString(), "CustomerID"));
            vchSet.Append(Method.BuildXML(fcerl.ProjectName, "ProjectName"));
            vchSet.Append(Method.BuildXML(fcerl.fID, "fID"));
            FnGenInitInfo_Result FCERLInitInfo = edb.FnGenInitInfo(vchSet.ToString()).FirstOrDefault();
            return FCERLInitInfo;
        }

        //
        // POST: /F_CERL/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection fc, f_cerl f_cerl)
        {
            StringBuilder vchSet = new StringBuilder();
            string Editor = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            string State = "10";
            int FlowCode = 1001001;
            string FormId = System.Guid.NewGuid().ToString().ToUpper();

            FnGenInitInfo_Result CaseIdList = GenFCERLInitInfo(f_cerl);

            f_cerl.fID = FormId;
            f_cerl.FlowCode = FlowCode;
            f_cerl.FormCode = int.Parse(CaseIdList.FormCode.ToString());
            f_cerl.CaseID = CaseIdList.CaseID;
            f_cerl.UID = (f_cerl.Action == "A") ? CaseIdList.UID : "";
            f_cerl.State = int.Parse(State);
            f_cerl.editor = Editor;
            f_cerl.Applicant = Editor;
            f_cerl.cdt = DateTime.Now;
            f_cerl.udt = DateTime.Now;
            if (TryUpdateModel(f_cerl, "", fc.AllKeys, new string[] { "fID", "Manager", "FlowCode", "FormCode", "Applicant", "editor", "State", "udt", "cdt" }))
            {
                if (ModelState.IsValid)
                {
                    // 找出申請者主管
                    if (f_cerl.Manager == null || f_cerl.Manager.Trim() == "")
                        f_cerl.Manager = db.vUsers.Where(x => x.BadgeCode == Editor).FirstOrDefault().Manager;//"IEC970209"; // test Manager
                    
                    f_cerl.Manager = (f_cerl.Manager == null) ? "" : f_cerl.Manager;

                    // 初始化設定副本收件人，特別是第一關要送到申請人主管
                    InitCCUsers(f_cerl.fID);

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

                    if (f_cerl.Action == "Create")
                        f_cerl.Action = "D";

                    if (Validation(null, f_cerl) == false)
                        return Redirect(Request.UrlReferrer.ToString());

                    db.f_cerl.Add(f_cerl);
                    db.SaveChanges();

                    RouteWork routewk = new RouteWork((WindowsIdentity)HttpContext.User.Identity, fcontroller: this, session: Session);
                    f_cerl = routewk.CheckAndPushTask(fc, f_cerl);

                    db.Entry(f_cerl).State = EntityState.Modified;
                    db.SaveChanges();

                    if(f_cerl.Action=="D")
                    {
                        int id = db.f_cerl.Where(x => x.fID == FormId).Select(x=>x.ID).FirstOrDefault();
                        RouteValueDictionary rv = new RouteValueDictionary();
                        rv.Add("id", id);
                        return RedirectToAction("EditLAB", rv);
                    }
                }
                return RedirectToAction("Index");
            }

            return View(f_cerl);
        }

        public ActionResult NextStrainGague(string fID, string View)
        {
            string TSPROC = "sp_CopyFcerl";
            System.Text.StringBuilder vchSet = new System.Text.StringBuilder();
            vchSet.Append(Method.BuildXML(fID, "fID"));
            string sqlCmd = Method.GetSqlCmd(TSPROC, "NEW_FID", "F_CERL", vchSet.ToString());
            DAO.sqlCmd(Constant.ConnCERLDBContext, sqlCmd);
            string[] result = DAO.sqlCmdArr(Constant.ConnCERLDBContext, sqlCmd);

            int ID = int.Parse(result[0]);
            RouteValueDictionary rv = new RouteValueDictionary();
            rv.Add("id", ID);
            return RedirectToAction(View, rv);
        }

        public ActionResult IndexNextStrainGague(string CaseID, string SerialNumber, string FromDate, string EndDate)
        {
            string UserId = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            var user = db.vUsers.Where(x => x.BadgeCode == UserId && x.RoleId == 2).FirstOrDefault();
            string[] chkActions = { "D", "T" };
            IEnumerable<vFCERLHeader> vfcerl = new List<vFCERLHeader>();

            queryCondition query = new queryCondition();
            query.id = -1;
            query.CaseID = CaseID;
            query.SerialNumber = SerialNumber;
            query.FromDate = FromDate;
            query.EndDate = EndDate;
            query.FlowState = 10;

            List<string> conItemList = new List<string>();
            var NextStrainGague = db.f_cerl.Where(x => (x.CopyfID != null || x.CopyfID != "" ) && chkActions.Contains(x.Action)).Select(v => v.fID).ToList();
            foreach (var item in NextStrainGague)
            {
                conItemList.Add(item);
            }
            conItemList = clsCompiledQuery.ConditionItemList3(conItemList, query);
            CERLDataContent dc = new CERLDataContent();
            if (user != null)
            {
                vfcerl = dc.sp_GetVFCERL(conItemList).Where(x => x.ApplicantId == UserId).OrderByDescending(x => x.UID).Take(Constant.iOutResultLimit);
                //vfcerl = db.vFCERL.Where(x => x.ApplicantId == UserId && conItemList.Contains(x.fID)).OrderByDescending(x => x.UID).Take(Constant.iOutResultLimit);
            }
            else
            {
                vfcerl = dc.sp_GetVFCERL(conItemList).OrderByDescending(x => x.UID).Take(Constant.iOutResultLimit);
            }

            return View(vfcerl);
        }

        public ActionResult IndexScheduleJob(string CaseID, string SerialNumber, string FromDate, string EndDate)
        {
            int [] StrainGagueArray = {1004000,2003000};
            string UserId = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            var user = db.vUsers.Where(x => x.BadgeCode == UserId && x.RoleId == 2).FirstOrDefault();
            string[] chkActions = { "D", "T" };
            IEnumerable<vFCERL> vfcerl = new List<vFCERL>();

            queryCondition query = new queryCondition();
            query.id = -1;
            query.CaseID = CaseID;
            query.SerialNumber = SerialNumber;
            query.FromDate = FromDate;
            query.EndDate = EndDate;
            query.FlowState = 1000;

            IList<int> conItemList = new List<int>();
            var NextStrainGague = db.f_cerl.Where(x => StrainGagueArray.Contains(x.FormCode)).Select(v => v.ID).ToList();
            foreach (var item in NextStrainGague)
            {
                conItemList.Add(item);
            }
            conItemList = ConditionItemList(conItemList, query);

            if (user != null)
            {
                vfcerl = db.vFCERL.Where(x => x.ApplicantId == UserId && conItemList.Contains(x.ID)).OrderByDescending(x => x.UID).Take(Constant.iOutResultLimit);
            }
            else
            {
                vfcerl = db.vFCERL.Where(x => conItemList.Contains(x.ID)).OrderByDescending(x => x.UID).Take(Constant.iOutResultLimit);
            }

            return View(vfcerl);
        }

        
        //
        // GET: /F_CERL/Edit/5
        public ActionResult EditLAB(int id = 0)
        {
            f_cerl fcerl = db.f_cerl.Find(id);
            string UserId = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            if (fcerl == null)
            {
                return HttpNotFound();
            }
            string fID = fcerl.fID;
            RouteWork routewk = new RouteWork(wi: (WindowsIdentity)HttpContext.User.Identity, fcontroller: this, session: Session);
            bool blChk = routewk.CheckUserUnsignedTask(fID);

            if (blChk == false) { return RedirectToAction(actionName: "Index"); }

            FnGetFCERL_Result FCERL = edb.FnGetFCERL(fID: fID, badgeCode: UserId).FirstOrDefault();
            IEnumerable<FormActions> formactions = routewk.GetFormAction(fcerl.FlowCode, fcerl.State);
            if(fcerl.State==10)
            {
                string [] chkTestItem = {"1004000","2003000"}; // TAO & IPT Strain Gague
                if (!chkTestItem.Contains(FCERL.ParentTestItem.ToString()) || fcerl.CopyfID == null)
                    formactions = formactions.Where(x => x.FormAction != "T");

                FCERL.Applicant_ReadOnly = (chkTestItem.Contains(FCERL.ParentTestItem.ToString()) && fcerl.CopyfID != null) ? false : true;
            }

            if (fcerl.State == 1000)
            {
                List<int> UserRole = db.vUserRole.Where(x => x.BadgeCode == UserId).Select(x => x.RoleId).ToList();
                if (UserRole != null)
                    FCERL.AnalysisSummary_Visible = FCERL.AnalysisSummary_Enable = (UserRole.Contains(20)) ? true : false; // LAB Supervisor Only
            }

            ViewData["FormAction"] = formactions;

            IEnumerable<vTaskDetail> vTaskDetail = routewk.GetTaskDetail(fcerl.fID);
            ViewData["vTaskDetail"] = vTaskDetail;

            InitDDLShow(fcerl, "EDIT");
            InitAttachFiles(fcerl.fID);
            InitCCUsers(fcerl.fID);
            return View(FCERL);
        }

        public f_cerl CopyFCERL(FnGetFCERL_Result fcerl, f_cerl f_cerl)
        {
            f_cerl.VirtualAnalysisSummary = fcerl.VirtualAnalysisSummary;
            f_cerl.Action = fcerl.Action;
            f_cerl.AnalysisResult = fcerl.AnalysisResult;
            f_cerl.AnalysisSummary = fcerl.AnalysisSummary;
            f_cerl.Applicant = fcerl.Applicant;
            f_cerl.BackgroundDesc = fcerl.BackgroundDesc;
            f_cerl.CaseID = fcerl.CaseID;
            f_cerl.Comment = fcerl.Comment;
            f_cerl.CustomerID = fcerl.CustomerID;
            f_cerl.editor = fcerl.editor;
            f_cerl.FailureSite = fcerl.FailureSite;
            f_cerl.fID = fcerl.fID;
            f_cerl.FinishDate = fcerl.FinishDate;
            f_cerl.FixtureNo = fcerl.FixtureNo;
            f_cerl.FixtureSupplier = fcerl.FixtureSupplier;
            f_cerl.FixtureVersionNo = fcerl.FixtureVersionNo;
            f_cerl.FlowCode = int.Parse(fcerl.FlowCode.ToString());
            f_cerl.FormCode = int.Parse(fcerl.FormCode.ToString());
            f_cerl.IssueSource = fcerl.IssueSource;
            f_cerl.LabMember = fcerl.LabMember;
            f_cerl.ListAssignTo = fcerl.ListAssignTo;
            f_cerl.LocalSupervisor = fcerl.LocalSupervisor;
            f_cerl.Manager = fcerl.Manager;
            f_cerl.NextTestDate = fcerl.NextTestDate;
            f_cerl.PartNumber = fcerl.PartNumber;
            f_cerl.ProcessStep = fcerl.ProcessStep;
            f_cerl.ProductStage = fcerl.ProductStage;
            f_cerl.ProjectName = fcerl.ProjectName;
            f_cerl.ReceiptDate = fcerl.ReceiptDate;
            f_cerl.ReceiptQty = fcerl.ReceiptQty;
            f_cerl.LabWorkHour = fcerl.LabWorkHour;
            f_cerl.RequestItem = fcerl.RequestItem;
            f_cerl.ReturnType = fcerl.ReturnType;
            f_cerl.SampleQty = fcerl.SampleQty;
            f_cerl.SerialNumber = fcerl.SerialNumber;
            f_cerl.Site = fcerl.Site;
            f_cerl.State = int.Parse(fcerl.State.ToString());
            f_cerl.Supervisor = fcerl.Supervisor;
            f_cerl.TestItem = fcerl.TestItem;
            f_cerl.TestPurpose = fcerl.TestPurpose;
            f_cerl.UID = fcerl.UID;
            return f_cerl;
        }

        public void UpdateCCUser(int SFAID, string MemberCode, int outState)
        {
            string UpdateCCUser = "UpdateCCUser";
            if (SFAID == 0)
            {
                return;
            }
            MemberCode = (MemberCode == null) ? "" : MemberCode;
            string Editor = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            var sfa = db.s_form_authority.Find(SFAID);
            if(sfa !=null)
            {
                sfa.MemberCodeList = MemberCode;
                sfa.udt = DateTime.Now;
                sfa.editor = Editor;
                db.Entry(sfa).State = EntityState.Modified;
            }
            else
            {
                sfa = new s_form_authority();
                sfa.fID = System.Guid.NewGuid().ToString().ToUpper();
                sfa.MemberCodeList = MemberCode;
                sfa.outState = SFAID;
                sfa.editor = Editor;
                sfa.udt = DateTime.Now;
                sfa.cdt = DateTime.Now;
                db.s_form_authority.Add(sfa);
            }
            db.SaveChanges();

            syserrdb.InitErrorData(Src: UpdateCCUser, content: "Test UpdateCCUser", editor: "IEC891652");
        }

        public string SendMail(string fID, string UserId, string outStateTitle, string Comment)
        {
            UserId = (UserId == null) ? "" : UserId;
            string _cerllabwebsite = Constant.S_WebSite;
            try
            {
                string Form2Url = _cerllabwebsite + @"/F_CERL/DetailsLAB/";
                vFCERL fcerl = db.vFCERL.Where(x => x.fID == fID).FirstOrDefault();
                string editor = db.userprofile.Where(x => x.BadgeCode == UserId).Select(x => x.ChtName).FirstOrDefault();
                string ID = fcerl.ID.ToString();
                outStateTitle = (outStateTitle == null) ? "更新測試報告" : outStateTitle;
                int outState = fcerl.State;

                StringBuilder Body = new StringBuilder();
                string newLine2 = "<br />\r\n";
                string newLine = "\r\n";
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
                Body.Append(Method.BuildXML2(Method.BuildXML2("Comment", "th", thStyle) + Method.BuildXML2(Comment, "td", tdStyle), "tr"));
                Body.Append(Method.BuildXML2(Method.BuildXML2("完成事項", "th", thStyle) + Method.BuildXML2(outStateTitle, "td", tdStyle), "tr"));

                string EmailTitile = "";
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
                Body.Append(Method.BuildXML2(Method.BuildXML2("請進入此網址查看<a href='" + Form2Url + ID + "'>CERLLAB</a>", "td"), "tr"));

                EmailTitile = Constant.MailTest + "[" + fcerl.UID + ", " + fcerl.CaseID + "] CERLLAB " + outStateTitle + " 通知";
                string UserManager = db.vUsers.Where(x=>x.BadgeCode==UserId).FirstOrDefault().Manager;
                UserManager = (UserManager == null) ? "" : UserManager;
                string AssignCC = UserId + ";" + UserManager + ";";
                var maillist = edb.FnGetMailList2(AssignCC).Select(v => v.Email).ToList();
                //var maillist = db.mail_test.Select(v => v.Email).ToList();
                // 副本給申請者及其主管，加上原有簽核表單的人員。
                List<string> conItemList = new List<string>();
                foreach (var item in maillist)
                {
                    conItemList.Add(item);
                }
                RouteWork routewk = new RouteWork(wi: (WindowsIdentity)HttpContext.User.Identity, fcontroller: this, session: Session);
                IEnumerable<vTaskDetail> vTaskDetail = routewk.GetTaskDetail(fcerl.fID);
                if(vTaskDetail != null)
                    if(vTaskDetail.Count()>0)
                    {
                        foreach(var item in vTaskDetail)
                        {
                            string useremail = db.vUsers.Where(x => x.BadgeCode == item.AssignedTo).FirstOrDefault().Email;
                            conItemList.Add(useremail);
                        }
                    }

                List<string> CCUserList = new List<string>();
                string ccmailcontext = db.s_form_authority.Where(x => x.fID.Equals(fcerl.fID) && x.outState.Equals(outState)).FirstOrDefault().MemberCodeList;
                if (ccmailcontext != null)
                {
                    maillist = edb.FnGetMailList2(ccmailcontext).Select(x => x.Email).ToList();
                    foreach (var item in maillist)
                        CCUserList.Add(item);
                }

                try
                {
                    if (CCUserList != null)
                        Method.SendMail(CCUserList, conItemList, EmailTitile, Body.ToString());
                }
                catch (Exception ex)
                {
                    Body.Append(newLine);
                    Body.Append(ex.Message);
                    syserrdb.InitErrorData(Src: "F_CERLController_SendMail: i1", content: Body.ToString(), editor: UserId);
                }
                return Body.ToString();
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                syserrdb.InitErrorData(Src: "F_CERLController_SendMail: o1", content: msg, editor: UserId);
                return "";
            }
        }

        // Runs after workflow closed, state=1000
        public ActionResult UpdateFileVersion(FormCollection fc, string FolderId)
        {
            string UpdateFileVersion = "UpdateFileVersion";
            string UserId = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            try
            {
                int SFAID = int.Parse(fc["SFAID"]);
                string MemberCode = fc["MemberCode"];
                string Comment = fc["Comment"];
                string Subject = fc["Subject"];
                UpdateCCUser(SFAID, MemberCode, 1000);
                string formId = fc["fID"];
                string state = FolderId;
                
                IQueryable<attachFileVer> attachfilever = db.attachFileVer.Where(x => x.fID == formId && x.folderId == state);
                IQueryable<attachFile> attachfile = db.attachFiles.Where(x => x.fID == formId && x.folderId == state);
                int filemaxver =1;

                if (attachfile != null)
                    filemaxver = (attachfile.Count() == 0) ? filemaxver : attachfile.Max(x => x.Version) + 1;
                var att = new List<attachFile>();

                int i = 0;
   
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase hpf = Request.Files[i] as HttpPostedFileBase;
                    if (hpf.ContentLength == 0)
                        continue;
                    FileInfo newinfo = new FileInfo(hpf.FileName);
                    string filePath0 = Constant.UserFileDirectory + formId + @"\";
                    string filePath = Constant.UserFileDirectory + formId + @"\" + state + @"\";
                    string filePathver = Constant.UserFileDirectory + formId + @"\" + state + @"\" + filemaxver.ToString() + @"\";
                    string savedFileName = Path.Combine(filePathver, Path.GetFileName(newinfo.Name));

                    if (!Directory.Exists(filePath0))
                        Directory.CreateDirectory(filePath0);

                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);

                    if (!Directory.Exists(filePathver))
                        Directory.CreateDirectory(filePathver);

                    hpf.SaveAs(savedFileName);

                    att.Add(new attachFile()
                    {
                        fID = formId,
                        displayname = newinfo.Name,
                        fileName = newinfo.Name,
                        Length = hpf.ContentLength,
                        Type = hpf.ContentType,
                        folderId = state,
                        editor = UserId,
                        filePath = filePathver,
                        Version = filemaxver,
                        cdt = DateTime.Now,
                        udt = DateTime.Now
                    });
                    i++;
                }

                foreach (attachFile a in att)
                    db.attachFiles.Add(a);
                db.SaveChanges();

                SendMail(formId, UserId, Subject, Comment);
            }
            catch(Exception ex) { // Do nothing!
                string msg = ex.Message;
                syserrdb.InitErrorData(Src: UpdateFileVersion, content: msg, editor: UserId);
                return Redirect(Request.UrlReferrer.ToString());
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        public void UpdateFCERL(FormCollection fc, f_cerl f_cerl)
        {
            string fnUpdateFCERL = "F_CERLController_UpdateFCERL";
            string UserId = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            try
            {
                string orgState = f_cerl.State.ToString(); //Mapping to attach file FolderId
                
                if (ModelState.IsValid)
                {
                    if (TryUpdateModel(f_cerl, "", fc.AllKeys, new string[] { "cdt", "Manager" }))
                    {
                        f_cerl.Manager = db.vUsers.Where(x => x.BadgeCode == f_cerl.Applicant).FirstOrDefault().Manager;//"IEC970209"; // test Manager

                        f_cerl.Manager = (f_cerl.Manager == null) ? "" : f_cerl.Manager;

                        int[] stateArray = { 10, 15, 18 };
                        if (f_cerl.State >= 10 && f_cerl.State < 1000)
                        {
                            FnGenInitInfo_Result CaseIdList = GenFCERLInitInfo(f_cerl);
                            f_cerl.CaseID = CaseIdList.CaseID;
                            f_cerl.FormCode = int.Parse(CaseIdList.FormCode.ToString());
                            if (f_cerl.UID == "" || f_cerl.UID == null)
                                f_cerl.UID = (f_cerl.Action == "A") ? CaseIdList.UID : "";
                        }

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
                            string state = orgState;
                            string filePath0 = Constant.UserFileDirectory + formId + @"\";
                            string filePath = Constant.UserFileDirectory + formId + @"\" + orgState + @"\";
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
                            int count = db.attachFiles.Where(x => x.fID == formId && x.folderId == orgState && x.fileName == newinfo.Name).Count();

                            if (count == 0)
                            {
                                r.Add(new attachFile()
                                {
                                    fID = formId,
                                    displayname = newinfo.Name,
                                    fileName = newinfo.Name,
                                    Length = hpf.ContentLength,
                                    Type = hpf.ContentType,
                                    folderId = orgState,
                                    editor = UserId,
                                    filePath = filePath,
                                    Version = 1,
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
                    RouteWork routewk = new RouteWork((WindowsIdentity)HttpContext.User.Identity, fcontroller: this, session: Session);
                    f_cerl = routewk.CheckAndPushTask(fc, f_cerl);

                    db.Entry(f_cerl).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                msg = "line 2097: " + msg;
                syserrdb.InitErrorData(Src: fnUpdateFCERL, content: msg, editor: UserId);
                throw new Exception(msg, ex.InnerException);
            }
        }

        public bool Validation(FnGetFCERL_Result fcerl, f_cerl f_cerl)
        {
            bool rsltValid = true;
            string[] chkActions = { "A", "T" };
            if (chkActions.Contains(f_cerl.Action))
            {
                if (f_cerl.State >= 20 && f_cerl.Action=="A")
                    if (f_cerl.LabMember == null || f_cerl.LabMember.Trim().Length == 0 || f_cerl.LabMember == "-1")
                    {
                        Session["ValidLabMember"] = "Please assign Lab Member!";
                        rsltValid = false;
                    }
                    else
                    {
                        Session["ValidLabMember"] = null;
                    }

                if (f_cerl.State >= 30 && f_cerl.State <= 32)
                {
                    if (fcerl != null)
                    {
                        CERLLAB.Models.f_cerl f_cerl_org = db.f_cerl.Find(fcerl.ID);
                        if (f_cerl_org.LabMember != f_cerl.LabMember && f_cerl.Action=="A")
                        {
                            Session["ValidLabMember"] = "You are assigning other member, please click Action '委派其他成員'[T] to other member!";
                            rsltValid = false;
                        }
                        else
                        {
                            Session["ValidLabMember"] = null;
                        }
                    }
                }

                string chkApplication = db.userprofile.Where(x => x.BadgeCode == f_cerl.Applicant).FirstOrDefault().BadgeCode;
                if (chkApplication != null)
                {
                    if (chkApplication.Trim().Length > 0)
                        Session["ValidApplicant"] = null;
                }
                else
                {
                    Session["ValidApplicant"] = "Please input inventec user!";
                    rsltValid = false;
                }

                if (f_cerl.Site == null)
                {
                    Session["ValidSite"] = "Please assign site!";
                    rsltValid = false;
                }
                else
                    Session["ValidSite"] = null;

                if (f_cerl.TestItem == null || (f_cerl.Site.Value > f_cerl.TestItem.Value))
                {
                    Session["ValidTestItem"] = "Please assign right TestItem!";
                    InitDDL("testList", f_cerl.Site.Value.ToString(), 1, f_cerl, null);
                    ViewData["requestList"] = null;
                    ViewData["LocalSupervisorList"] = null;
                    ViewData["LabMemberList"] = null;
                    rsltValid = false;
                }
                else
                    Session["ValidTestItem"] = null;

                if (f_cerl.RequestItem == null || f_cerl.TestItem.Value > f_cerl.RequestItem.Value)
                {
                    Session["ValidRequestItem"] = "Please assign RequestItem!";
                    rsltValid = false;
                }
                else
                    Session["ValidRequestItem"] = null;
            }

            return rsltValid;
        }

        //
        // POST: /F_CERL/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult EditLAB(FormCollection fc, FnGetFCERL_Result fcerl)
        {
            string fnEditLAB = "F_CERLController_EditLAB";
            string UserId = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            try
            {
                if (fc["CCUserAction"] == "UpdateCCUser")
                {
                    int SFAID = int.Parse(fc["SFAID"]);
                    string MemberCode = fc["MemberCode"];
                    UpdateCCUser(SFAID, MemberCode, 1000);
                    return Redirect(Request.UrlReferrer.ToString());
                }

                CERLLAB.Models.f_cerl f_cerl = db.f_cerl.Find(fcerl.ID);

                f_cerl = CopyFCERL(fcerl, f_cerl);
                string[] initActions = { "A", "C", "R","T" };
                f_cerl.Action = (!initActions.Contains(f_cerl.Action)) ? "D" : f_cerl.Action;

                if (f_cerl.State >= 1000)
                {
                    InitDDLShow(null, "EDIT");
                    return View(fcerl);
                }

                bool rsltValid = Validation(fcerl, f_cerl);
                if (rsltValid == false)
                    return Redirect(Request.UrlReferrer.ToString());

                UpdateFCERL(fc: fc, f_cerl: f_cerl);

                if (f_cerl.Action == "D")
                {
                    return Redirect(Request.UrlReferrer.ToString());
                }
                else
                {
                    RouteValueDictionary rv = new RouteValueDictionary();
                    rv.Add("id", f_cerl.ID);

                    if (f_cerl.Action == "A" && f_cerl.State == 18)
                        return RedirectToAction("EditLAB", rv);

                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                syserrdb.InitErrorData(Src: fnEditLAB, content: msg, editor: UserId);
                return Redirect(Request.UrlReferrer.ToString());
            }
            //return View(f_cerl);
        }

        public ActionResult Edit(int id = 0)
        {
            f_cerl f_cerl = db.f_cerl.Find(id);
            if (f_cerl == null)
            {
                return HttpNotFound();
            }

            if (f_cerl.State >= 1000)
            {
                RouteValueDictionary rv = new RouteValueDictionary();
                rv.Add("id", f_cerl.ID);
                return RedirectToAction("Details", rv);
            }
            RouteWork routewk = new RouteWork((WindowsIdentity)HttpContext.User.Identity, fcontroller: this, session: Session);
            IEnumerable<FormActions> formactions = routewk.GetFormAction(f_cerl.FlowCode, f_cerl.State);
            ViewData["FormAction"] = formactions;

            IEnumerable<vTaskDetail> vTaskDetail = routewk.GetTaskDetail(f_cerl.fID);
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

            UpdateFCERL(fc:fc, f_cerl:f_cerl);

            RouteValueDictionary rv = new RouteValueDictionary();

            if (f_cerl.Action == "D")
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                rv.Add("id", f_cerl.ID);
                return RedirectToAction("Index");
            }
            //return View(f_cerl);
        }

        //
        // GET: /F_CERL/CancelLAB/5

        public ActionResult CancelLAB(int id = 0)
        {
            ViewBag.id = id;
            int? RoleId = null;
            string UserId = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            var user = db.vUsers.Where(x => x.BadgeCode == UserId && x.RoleId == 20).FirstOrDefault();
            if (user != null)
                RoleId = user.RoleId;

            f_cerl f_cerl = db.f_cerl.Find(id);
            FnGetFCERL_Result vfcerl = edb.FnGetFCERL(f_cerl.fID, UserId).FirstOrDefault();

            if (vfcerl == null)
            {
                return HttpNotFound();
            }
            InitAttachFiles(vfcerl.fID);

            RouteWork routewk = new RouteWork((WindowsIdentity)HttpContext.User.Identity, fcontroller: this, session: Session);
            IEnumerable<vTaskDetail> vTaskDetail = routewk.GetTaskDetail(vfcerl.fID);
            ViewData["vTaskDetail"] = vTaskDetail;

            int[] instate = { 40, 1000, 1100 };

            if (instate.Contains(f_cerl.State))
            {
                if (RoleId.HasValue)
                    Session["AnalysisSummary_Visible_LABSupervisor"] = true;
                else
                    Session["AnalysisSummary_Visible_LABSupervisor"] = false;
            }
            else
                Session["AnalysisSummary_Visible_LABSupervisor"] = false;

            return View(vfcerl);
        }

        [HttpPost, ActionName("CancelLAB")]
        [ValidateAntiForgeryToken]
        public ActionResult CancelLABConfirmed(int id)
        {
            f_cerl f_cerl = db.f_cerl.Find(id);
            f_cerl.Action = "C";
            f_cerl.udt = DateTime.Now;
            f_cerl.editor = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            db.Entry(f_cerl).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("IndexNextStrainGague");
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