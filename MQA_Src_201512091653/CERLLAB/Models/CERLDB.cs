using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Linq;
using System.Linq;
using System.Web;
using CERLLAB.Controllers.General;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using System.Configuration;
using System.Data.Entity.Validation;

namespace CERLLAB.Models
{
    public class CERLDB
    {
        
    }

    public class vReturnSite
    {
        [Key]
        public int SiteID { get; set; }
        public string SiteNAME { get; set; }
        public Nullable<int> ReturnTypeID { get; set; }
        public string ReturnTypeName { get; set; }
    }

    public class ReturnSite
    {
        [Key]
        public int SiteID { get; set; }
        public string SiteNAME { get; set; }
        public Nullable<int> ReturnTypeID { get; set; }
    }

    public class ReturnType
    {
        [Key]
        public int TypeID { get; set; }
        public string TypeName { get; set; }
    }

    public class TestPurpose
    {
        [Key]
        public int ID { get; set; }
        public string TextValue { get; set; }
    }

    public class CustomerName
    {
        [Key]
        public int ID { get; set; }
        public string TextValue { get; set; }
    }

    public class IssueSource
    {
        [Key]
        public int ID { get; set; }
        public string TextValue { get; set; }
    }

    public class PCBVendor
    {
        [Key]
        public int ID { get; set; }
        public string TextValue { get; set; }
    }

    public class ProductStage
    {
        [Key]
        public int ID { get; set; }
        public string TextValue { get; set; }
        public int CustomerID { get; set; }
    }

    public class ProcessStep
    {
        [Key]
        public int ID { get; set; }
        public string TextValue { get; set; }
    }

    public class GeneralDropDownList
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class MenuNode
    {
        [Key]
        public int Id { get; set; }

        public int ParentId { get; set; }
        public MenuNode Parent { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int NodeType { get; set; }
        public int NodeOrder { get; set; }
        public int IsUrl { get; set; }
        public string actionname { get; set; }
        public string controlname { get; set; }
        public List<MenuNode> Children = new List<MenuNode>();
    }

    public class Menu
    {
        private MenuNode root_Node;
        private string Url;
        private int IsUrl;

        public MenuNode RootNode
        {
            get { return root_Node; }
            set
            {
                if (RootNode != null)
                {
                    Nodes.Remove(RootNode.Id);
                }
                value.Url = Url + @"/" + value.Id.ToString();
                if (IsUrl > 1)
                    value.IsUrl = 1;
                Nodes.Add(value.Id, value);
                root_Node = value;
            }
        }

        public Dictionary<int, MenuNode> Nodes { get; set; }

        public Menu(string mUrl, int mIsUrl)
        {
            Url = mUrl;
            IsUrl = mIsUrl;
        }

        public void BuildMenu(string Url, int mIsUrl)
        {
            MenuNode itemparent;
            foreach (var node in Nodes.Values)
            {
                node.Url = Url + @"/" + node.Id.ToString();
                if (Nodes.TryGetValue(node.ParentId, out itemparent) && node.Id != node.ParentId)
                {
                    node.Parent = itemparent;
                    if (mIsUrl >= 2)
                        node.IsUrl = 1;
                    itemparent.Children.Add(node);
                }
            }
        }
    }

    public class MenuDbSet
    {
        private CERLDBContext db = new CERLDBContext();
        private CERLEntities edb = new CERLEntities();
        private string mUrl;
        public MenuDbSet(string Url)
        {
            mUrl = Url;
        }

        public Menu GetData(string Url, string BadgeCode, int mIsUrl)
        {
            var vrolefunction = db.vRoleFunctions.Where(r => r.BadgeCode == BadgeCode && r.menuType == 2).Select(x => x.menuId).Distinct().ToList();
            var tree = new Menu(Url, mIsUrl);

            tree.Nodes = db.MenuNode.Where(x => vrolefunction.Contains(x.Id)).ToList()
                .Select(t => new MenuNode { Id = t.Id, Name = t.Name, Url = t.Url, ParentId = t.ParentId, IsUrl = t.IsUrl, actionname = t.actionname, controlname=t.controlname })
                .ToDictionary(t => t.Id);

            tree.RootNode = new MenuNode { Id = 0, Name = "Menu", Url = Url };

            tree.BuildMenu(Url, mIsUrl);
            return tree;
        }

        public IList<FnGetParentListById_Result> GetParentList(int Id)
        {
            IList<FnGetParentListById_Result> parentlist = edb.FnGetParentListById(Id).ToList();
            return parentlist;
        }
    }

    public class ItemTreeNode
    {
        [Key]
        public int Id { get; set; }

        public int ParentId { get; set; }
        public ItemTreeNode Parent { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Url { get; set; }
        public int NodeType { get; set; }
        public int NodeOrder { get; set; }
        public int IsUrl { get; set; }
        public int IsShowReport { get; set; }
        public int IsShowUnsignForm { get; set; }
        public int CountReports { get; set; }
        public int CountInProcessingForm { get; set; }
        public List<ItemTreeNode> Children = new List<ItemTreeNode>();
    }

    public class ItemTreeDbSet
    {
        private CERLDBContext db = new CERLDBContext();
        private CERLEntities edb = new CERLEntities();
        private string mUrl;

        public ItemTreeDbSet(string Url)
        {
            mUrl = Url;
        }

        public ItemTree GetData(string Url, string BadgeCode, int mIsUrl, int mIsShowReport, int mIsShowUnsignForm)
        {
            //var vrolefunction = db.vRoleFunctions.Where(r => r.BadgeCode == BadgeCode && r.menuType == 1).Select(x => x.menuId).ToList();
            var tree = new ItemTree(Url, mIsUrl, mIsShowReport:mIsShowReport, mIsShowUnsignForm:mIsShowUnsignForm);

            try
            { 
            if (mIsShowReport > 0 && mIsShowUnsignForm > 0)
                tree.Nodes = db.ItemTreeNode.ToList()//.Where(x => vrolefunction.Contains(x.Id)).ToList()
                    .Select(t => new ItemTreeNode { Id = t.Id, Name = t.Name, FullName = t.FullName, Url = t.Url, ParentId = t.ParentId, IsUrl = t.IsUrl, IsShowReport = mIsShowReport, IsShowUnsignForm = mIsShowUnsignForm, CountReports = t.CountReports, CountInProcessingForm = t.CountInProcessingForm })
                    .ToDictionary(t => t.Id);

            if(mIsShowReport>0 && mIsShowUnsignForm<=0)
                tree.Nodes = db.ItemTreeNode.Where(x=>x.CountReports>0).ToList()
                .Select(t => new ItemTreeNode { Id = t.Id, Name = t.Name, FullName = t.FullName, Url = t.Url, ParentId = t.ParentId, IsUrl = t.IsUrl, IsShowReport = mIsShowReport, IsShowUnsignForm = mIsShowUnsignForm, CountReports = t.CountReports, CountInProcessingForm = t.CountInProcessingForm })
                .ToDictionary(t => t.Id);

            if (mIsShowReport <= 0 && mIsShowUnsignForm > 0)
                tree.Nodes = db.ItemTreeNode.Where(x => x.CountInProcessingForm > 0).ToList()
                .Select(t => new ItemTreeNode { Id = t.Id, Name = t.Name, FullName = t.FullName, Url = t.Url, ParentId = t.ParentId, IsUrl = t.IsUrl, IsShowReport = mIsShowReport, IsShowUnsignForm = mIsShowUnsignForm, CountReports = t.CountReports, CountInProcessingForm = t.CountInProcessingForm })
                .ToDictionary(t => t.Id);
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
            }

            tree.RootNode = new ItemTreeNode { Id = 0, Name = "Item", Url = Url };

            tree.BuildTree(Url, mIsUrl);
            return tree;
        }

        public IList<FnGetItemParentListById_Result> GetParentList(int Id)
        {
            IList<FnGetItemParentListById_Result> parentlist = edb.FnGetItemParentListById(Id.ToString()).ToList();
            return parentlist;
        }
    }

    public class ItemTree
    {
        private ItemTreeNode root_Node;
        private string Url;
        private int IsUrl;
        private int IsShowReport;
        private int IsShowUnsignForm;

        public ItemTreeNode RootNode
        {
            get { return root_Node; }
            set
            {
                if (RootNode != null)
                {
                    Nodes.Remove(RootNode.Id);
                }
                value.Url = Url + @"/" + value.Id.ToString();
                if (IsUrl > 1)
                    value.IsUrl = 1;
                Nodes.Add(value.Id, value);
                root_Node = value;
            }
        }

        public Dictionary<int, ItemTreeNode> Nodes { get; set; }

        public ItemTree(string mUrl, int mIsUrl, int mIsShowReport, int mIsShowUnsignForm)
        {
            Url = mUrl;
            IsUrl = mIsUrl;
            IsShowReport = mIsShowReport;
            IsShowUnsignForm = mIsShowUnsignForm;
        }

        public void BuildTree(string Url, int mIsUrl)
        {
            ItemTreeNode itemparent;
            foreach (var node in Nodes.Values)
            {
                node.Url = Url + @"/" + node.Id.ToString();
                if (Nodes.TryGetValue(node.ParentId, out itemparent) && node.Id != node.ParentId)
                {
                    node.Parent = itemparent;
                    if (mIsUrl >= 2)
                        node.IsUrl = 1;
                    itemparent.Children.Add(node);
                }
            }
        }
    }


    public class TreeNode
    {
        [Key]
        public int Id { get; set; }

        public int ParentId { get; set; }
        public TreeNode Parent { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int NodeType { get; set; }
        public int NodeOrder { get; set; }
        public int IsUrl { get; set; }
        public List<TreeNode> Children = new List<TreeNode>();
    }


    public class TreeDbSet
    {
        private CERLDBContext db = new CERLDBContext();
        private CERLEntities edb = new CERLEntities();
        private string mUrl;
        public TreeDbSet(string Url)
        {
            mUrl = Url;
        }

        public Tree GetData(string Url, string BadgeCode, int mIsUrl)
        {
            if (BadgeCode.Trim().Length == 0 || BadgeCode == null)
                BadgeCode = Constant.LogonUserId;

            var vrolefunction = db.vRoleFunctions.Where(r => r.BadgeCode == BadgeCode && r.menuType == 1).Select(x => x.menuId).ToList();
            var tree = new Tree(Url, mIsUrl);

            tree.Nodes = (from t in db.TreeNode
                          select t).Where(x => vrolefunction.Contains(x.Id)).ToList()
                .Select(t => new TreeNode { Id = t.Id, Name = t.Name, Url = t.Url, ParentId = t.ParentId, IsUrl = t.IsUrl })
                .ToDictionary(t => t.Id);

            tree.RootNode = new TreeNode { Id = 0, Name = "Menu", Url = Url };

            tree.BuildTree(Url, mIsUrl);
            return tree;
        }

        public IList<FnGetParentListById_Result> GetParentList(int Id)
        {
            IList<FnGetParentListById_Result> parentlist = edb.FnGetParentListById(Id).ToList();
            return parentlist;
        }
    }

    public class Tree
    {
        private TreeNode root_Node;
        private string Url;
        private int IsUrl;

        public TreeNode RootNode
        {
            get { return root_Node; }
            set
            {
                if (RootNode != null)
                {
                    Nodes.Remove(RootNode.Id);
                }
                value.Url = Url + @"/" + value.Id.ToString();
                if (IsUrl > 1)
                    value.IsUrl = 1;
                Nodes.Add(value.Id, value);
                root_Node = value;
            }
        }

        public Dictionary<int, TreeNode> Nodes { get; set; }

        public Tree(string mUrl, int mIsUrl)
        {
            Url = mUrl;
            IsUrl = mIsUrl;
        }

        public void BuildTree(string Url, int mIsUrl)
        {
            TreeNode parent;
            foreach (var node in Nodes.Values)
            {
                node.Url = Url + @"/" + node.Id.ToString();
                if (Nodes.TryGetValue(node.ParentId, out parent) && node.Id != node.ParentId)
                {
                    node.Parent = parent;
                    if (mIsUrl >= 2)
                        node.IsUrl = 1;
                    parent.Children.Add(node);
                }
            }
        }
    }

    public class CCUserSet
    {
        private CERLDBContext db = new CERLDBContext();
        // 初始化CC to Users，並設定第一關CC給Manager，結案時CC給LABSup指定的收件人。
        // 在此副本收件人的人員，有Read表單的權限
        public s_form_authority GetFormAuthority(string fID, string Editor)
        {
            int outState = 20;
            var cc = db.s_form_authority.Where(x => x.fID == fID && x.outState == outState).FirstOrDefault();
            if(cc==null)
            {
                string Manager = "";
                var fcerl = db.f_cerl.Where(x => x.fID == fID).FirstOrDefault();
                Manager = (fcerl == null) ? db.vUsers.Where(x => x.BadgeCode == Editor).FirstOrDefault().Manager : fcerl.Manager;
                
                cc = new s_form_authority();
                cc.fID = fID;
                cc.cdt = DateTime.Now;
                cc.udt = DateTime.Now;
                cc.editor = Editor;
                cc.MemberCodeList = Manager;
                cc.outState = outState;
                db.s_form_authority.Add(cc);
                db.SaveChanges();
                cc = db.s_form_authority.Where(x => x.fID == fID && x.outState == outState).FirstOrDefault();
            }

            outState = 1000;
            cc = db.s_form_authority.Where(x => x.fID == fID && x.outState == outState).FirstOrDefault();
            if (cc == null)
            {
                cc = new s_form_authority();
                cc.fID = fID;
                cc.cdt = DateTime.Now;
                cc.udt = DateTime.Now;
                cc.editor = Editor;
                cc.outState = outState;
                db.s_form_authority.Add(cc);
                db.SaveChanges();
                cc = db.s_form_authority.Where(x => x.fID == fID && x.outState == outState).FirstOrDefault();
            }
            return cc;
        }
    }

    public class AttachFileSet
    {
        private CERLDBContext db = new CERLDBContext();

        public List<attachFile> GetFiles(string fID, string State)
        {
            string strState = State.ToString();
            var att = db.attachFiles.Where(x => x.fID == fID && x.folderId == strState).ToList();
            return att;
        }
    }

    // layoff by Bryan Wu 2015/3/5
    public class AttachFileVerSet
    {
        private CERLDBContext db = new CERLDBContext();

        public List<attachFile> GetFiles(string fID, string State)
        {
            string strState = State.ToString();
            var att = db.attachFiles.Where(x => x.fID == fID && x.folderId == strState).ToList();
            return att;
        }
    }

    public class attachFile
    {
        [Key]
        public int fileId { get; set; }

        [Display(Name = "File Path")]
        public string filePath { get; set; }
        public string fileName { get; set; }
        public string fID { get; set; }
        public DateTime cdt { get; set; }
        public DateTime udt { get; set; }
        public string editor { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }
        public string folderId { get; set; }
        public string displayname { get; set; }
        public int Version { get; set; }
    }

    // layoff by Bryan 2015/3/6
    public class attachFileVer
    {
        [Key]
        public int fileId { get; set; }

        [Display(Name = "File Path")]
        public string filePath { get; set; }
        public string fileName { get; set; }
        public string fID { get; set; }
        public DateTime cdt { get; set; }
        public DateTime udt { get; set; }
        public string editor { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }
        public string folderId { get; set; }
        public string displayname { get; set; }
        public int Version { get; set; }
    }

    public class AttachFileVerMaxSet
    {
        private CERLDBContext db = new CERLDBContext();

        public List<vattachFileMaxVer> GetFiles(string fID, string State)
        {
            string strState = State.ToString();
            var att = db.vattachFileMaxVer.Where(x => x.fID == fID && x.folderId == strState).ToList();
            return att;
        }
    }

    public class vattachFileMaxVer
    {
        [Key]
        public int fileId { get; set; }

        [Display(Name = "File Path")]
        public string filePath { get; set; }
        public string fileName { get; set; }
        public string fID { get; set; }
        public DateTime cdt { get; set; }
        public DateTime udt { get; set; }
        public string editor { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }
        public string folderId { get; set; }
        public string displayname { get; set; }
        public int Version { get; set; }
        public string srcTable { get; set; }
    }

    public class FormActions
    {
        public string FormAction { get; set; }
        public string ActionName { get; set; }
    }

    public class vTaskDetail
    {
        //TaskID, FID, EquID, FormCode, FlowCode, CurrentState, HistoryState, Applicant, Action, GroupCode, Comment, S_date, F_date, Editor
        //SELECT ID, TaskID, FID, FormCode, FlowCode, CurrentState, HistoryState, HistoryStateName, AssignedTo, AssignedToName, Applicant, ApplicantName, Action, ActionName, Comment, S_date, F_date, Editor, EditorName FROM vTask WHERE FID = 'A286C309-944B-415B-AF29-F6AD297C4346'
        public int ID { get; set; }
        public string TaskID { get; set; }
        public string FID { get; set; }
        public int FormCode { get; set; }
        public int FlowCode { get; set; }
        public int CurrentState { get; set; }
        public int HistoryState { get; set; }
        public string HistoryStateName { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedToName { get; set; }
        public string Applicant { get; set; }
        public string ApplicantName { get; set; }

        public string Action { get; set; }
        public string ActionName { get; set; }

        public string Comment { get; set; }

        [Display(Name = "開始日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime S_date { get; set; }

        [Display(Name = "完成日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> F_date { get; set; }

        public string Editor { get; set; }
        public string EditorName { get; set; }
    }

    [Table (Name="vFCERL")]
    public class vFCERL
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "FID")]
        public string fID { get; set; }

        [Display(Name = "Form Code")]
        public int FormCode { get; set; }

        [Display(Name = "Flow Code")]
        public int FlowCode { get; set; }

        [Display(Name = "UID")]
        public string UID { get; set; }

        [Display(Name = "Case ID")]
        public string CaseID { get; set; }

        public string ApplicantId { get; set; }

        [Display(Name = "Applicant")]
        public string Applicant { get; set; }

        [Display(Name = "CustomerID")]
        public Nullable<int> CustomerID { get; set; }

        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Part Number")]
        public string PartNumber { get; set; }

        [Display(Name = "SiteId")]
        public int SiteId { get; set; }

        [Display(Name = "Site")]
        public string Site { get; set; }

        public int ParentTestItemId { get; set; }

        [Display(Name = "ParentTest Item")]
        public string ParentTestItem { get; set; }

        public int TestItemId { get; set; }

        [Display(Name = "Test Item")]
        public string TestItem { get; set; }

        public int RequestItemId { get; set; }

        [Display(Name = "Request Item")]
        public string RequestItem { get; set; }

        [Display(Name = "Return Type")]
        public string ReturnType { get; set; }

        [Display(Name = "Failure Site")]
        public string FailureSite { get; set; }

        [Display(Name = "Background Description")]
        [DataType(DataType.MultilineText)]
        public string BackgroundDesc { get; set; }

        [Display(Name = "Issue Source")]
        public string IssueSource { get; set; }

        [Display(Name = "Sample Qty")]
        public Nullable<int> SampleQty { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Test Purpose")]
        public string TestPurpose { get; set; }

        public Nullable<int> PCBVendorID { get; set; }
        public string PCBVendorName { get; set; }

        public Nullable<int> ProductStageID { get; set; }
        public string ProductStageName { get; set; }
        [Display(Name = "Product Stage")]
        public string ProductStage { get; set; }

        [Display(Name = "Process Step")]
        public string ProcessStep { get; set; }

        [Display(Name = "Fixture No")]
        public string FixtureNo { get; set; }

        [Display(Name = "Fixture Version No")]
        public string FixtureVersionNo { get; set; }

        [Display(Name = "Fixture Supplier")]
        public string FixtureSupplier { get; set; }

        public string LabMemberId { get; set; }

        [Display(Name = "Lab Member")]
        public string LabMember { get; set; }

        [Display(Name = "Receipt Qty")]
        public Nullable<int> ReceiptQty { get; set; }

        [Display(Name = "Receipt Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> ReceiptDate { get; set; }

        [Display(Name = "Finish Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> FinishDate { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Analysis Result")]
        public string AnalysisResult { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Analysis Summary")]
        public string AnalysisSummary { get; set; }

        [Display(Name = "Next Test Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> NextTestDate { get; set; }

        [Display(Name = "Create")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime cdt { get; set; }

        [Display(Name = "Update")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime udt { get; set; }

        public string editor { get; set; }
        public string Action { get; set; }
        public int State { get; set; }

        public string StateName { get; set; }

        [Display(Name = "Employee No")]
        public string BadgeCode { get; set; }

        [Display(Name = "Department")]
        public string Dept { get; set; }

        [Display(Name = "CorpTel")]
        public string CorpTel { get; set; }

        [Display(Name = "Manager")]
        public string Manager { get; set; }

        public string ListAssignTo { get; set; }
        public string Comment { get; set; }
        public string CopyfID { get; set; }
        public string VirtualAnalysisSummary { get; set; }

        public string PreTestDate { get; set; }
    }

    [Table(Name = "vFCERLHeader")]
    public class vFCERLHeader
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "FID")]
        public string fID { get; set; }

        [Display(Name = "UID")]
        public string UID { get; set; }

        [Display(Name = "Case ID")]
        public string CaseID { get; set; }

        public string ApplicantId { get; set; }

        [Display(Name = "Applicant")]
        public string Applicant { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Part Number")]
        public string PartNumber { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }

        public int TestItemId { get; set; }

        [Display(Name = "Test Item")]
        public string TestItem { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Test Purpose")]
        public string TestPurpose { get; set; }

        [Display(Name = "Process Step")]
        public string ProcessStep { get; set; }

        public string LabMemberId { get; set; }

        [Display(Name = "Lab Member")]
        public string LabMember { get; set; }

        [Display(Name = "Next Test Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> NextTestDate { get; set; }

        [Display(Name = "Finish Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> FinishDate { get; set; }

        [Display(Name = "Create")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime cdt { get; set; }

        [Display(Name = "Update")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime udt { get; set; }

        public string editor { get; set; }
        public string Action { get; set; }
        public int State { get; set; }

        public string StateName { get; set; }

        public string PreTestDate { get; set; }
    }

    public class LabInformation
    {
        [Key]
        public int id { get; set; }

        [Display(Name = "FID")]
        public string fID { get; set; }

        [Display(Name = "Title")]
        public string title { get; set; }

        [Display(Name = "Content")]
        public string content { get; set; }

        [Display(Name = "類別")]
        public string type { get; set; }

        [Display(Name = "狀態")]
        public string enable { get; set; }

        [Display(Name = "維護公告訊息")]
        public bool IsShowMessage { get; set; }

        [Display(Name = "維護圖片")]
        public bool IsShowPicture { get; set; }

        public string editor { get; set; }

        [Display(Name = "Create")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime cdt { get; set; }

        [Display(Name = "Update")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime udt { get; set; }

        [Required]
        public string Width { get; set; }

        [Required]
        public string Height { get; set; }

    }

    //fileId, title, content, enable, FullName, cdt, udt, editor, ChtName
    public class vLabInfoAttachFile
    {
        [Key]
        public int fileId { get; set; }
        public string fID { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string enable { get; set; }
        public string FullFilePath { get; set; }
        public string FullName { get; set; }
        public bool IsShowMessage { get; set; }
        public bool IsShowPicture { get; set; }

        [Display(Name = "Create")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime cdt { get; set; }

        [Display(Name = "Update")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime udt { get; set; }

        public string editor { get; set; }
        public string ChtName { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
    }

    public class vLabInformation
    {
        [Key]
        public int id { get; set; }

        [Display(Name = "FID")]
        public string fID { get; set; }

        [Display(Name = "Title")]
        public string title { get; set; }

        [Display(Name = "Content")]
        public string content { get; set; }

        [Display(Name = "類別")]
        public string type { get; set; }

        [Display(Name = "狀態")]
        public string enable { get; set; }

        [Display(Name = "維護公告訊息")]
        public bool IsShowMessage { get; set; }

        [Display(Name = "維護圖片")]
        public bool IsShowPicture { get; set; }

        public string editor { get; set; }

        [Display(Name = "Editor")]
        public string ChtName { get; set; }

        [Display(Name = "Create")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime cdt { get; set; }

        [Display(Name = "Update")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime udt { get; set; }

        public string Width { get; set; }
        public string Height { get; set; }

    }

    public class LABMemberOnHandWork
    {
        [Key]
        public string LABMember { get; set; }
        public string Site { get; set; }
        public string TestItem { get; set; }
        public string RequestItem { get; set; }
        public int JobNumber { get; set; }
    }

    public class vLABMemberOnHandWork
    {
        [Key]
        public string LABMember { get; set; }
        public int JobNumber { get; set; }
    }

    public class vCountOnHandWork
    {
        [Key]
        public string LABMember { get; set; }
        public int JobNumber { get; set; }
    }

    public class queryCondition
    { 
    //int? id, string CaseID, string SerialNumber, string FromDate, string EndDate
        public int id { get; set; }
        public int FlowState { get; set; }
        public string LabMember { get; set; }
        public string Applicant { get; set; }
        public string UID { get; set; }
        public string CaseID { get; set; }
        public string SerialNumber { get; set; }
        public int CustomerID { get; set; }
        public string FromDate { get; set; }
        public string EndDate { get; set; }
        public string NextTestFromDate { get; set; }
        public string NextTestEndDate { get; set; }
        public string searchString { get; set; }
        public string UserId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<string> FID { get; set; }
    }

    public class s_form_authority
    {
        [Key]
        public int ID { get; set; }
        public string fID { get; set; }
        public string MemberCodeList { get; set; }
        public string RoleIdList { get; set; }
        public int outState { get; set; }
        public string editor { get; set; }
        public DateTime cdt { get; set; }
        public DateTime udt { get; set; }
    }

    public class f_cerl
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "FID")]
        public string fID { get; set; }

        [Display(Name = "Form Code")]
        public int FormCode { get; set; }

        [Display(Name = "Flow Code")]
        public int FlowCode { get; set; }

        [Display(Name = "UID")]
        public string UID { get; set; }

        [Display(Name = "Case ID")]
        public string CaseID { get; set; }

        [Display(Name = "Applicant")]
        public string Applicant { get; set; }

        [Display(Name = "Manager")]
        public string Manager { get; set; }

        [Display(Name = "Customer ID")]
        public Nullable<int> CustomerID { get; set; }

        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }

        [Display(Name = "Part Number")]
        public string PartNumber { get; set; }

        [Display(Name = "Site")]
        public Nullable<int> Site { get; set; }

        [Display(Name = "Test Item")]
        public Nullable<int> TestItem { get; set; }

        [Display(Name = "Request Item")]
        public Nullable<int> RequestItem { get; set; }

        [Display(Name = "Return Type")]
        public Nullable<int> ReturnType { get; set; }

        [Display(Name = "Failure Site")]
        public Nullable<int> FailureSite { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Background Description")]
        public string BackgroundDesc { get; set; }

        [Display(Name = "Issue Source")]
        public Nullable<int> IssueSource { get; set; }

        [RegularExpression(@"^[1-9]\d{0,}$", ErrorMessage="Please input great than 0.")]
        //[DataType(DataType.Text)]
        [Display(Name = "Sample Qty")]
        public Nullable<int> SampleQty { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Test Purpose")]
        public Nullable<int> TestPurpose { get; set; }

        [Display(Name = "Product Stage")]
        public string ProductStage { get; set; }

        [Display(Name = "Process Step")]
        public Nullable<int> ProcessStep { get; set; }

        [Display(Name = "Fixture No")]
        public string FixtureNo { get; set; }

        [Display(Name = "Fixture Version No")]
        public string FixtureVersionNo { get; set; }

        [Display(Name = "Fixture Supplier")]
        public string FixtureSupplier { get; set; }

        [Display(Name = "Supervisor")]
        public string Supervisor { get; set; }

        [Display(Name = "Local Supervisor")]
        public string LocalSupervisor { get; set; }

        [Display(Name = "Lab Member")]
        public string LabMember { get; set; }

        [Range(0, 20000)]
        [Display(Name = "Receipt Qty")]
        public Nullable<int> ReceiptQty { get; set; }

        [Display(Name = "Lab Work Hour")]
        public Nullable<double> LabWorkHour { get; set; }

        [Display(Name = "Receipt Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> ReceiptDate { get; set; }

        [Display(Name = "Finish Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> FinishDate { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Analysis Result")]
        public string AnalysisResult { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Analysis Summary")]
        public string AnalysisSummary { get; set; }

        [Display(Name = "Next Test Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> NextTestDate { get; set; }

        [Display(Name = "Create")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime cdt { get; set; }

        [Display(Name = "Update")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime udt { get; set; }

        public string editor { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        public string ListAssignTo { get; set; }

        [Display(Name = "Action")]
        public string Action { get; set; }

        [Display(Name = "State")]
        public int State { get; set; }

        public string CopyfID { get; set; }

        public string VirtualAnalysisSummary { get; set; }

        public Nullable<int> PCBVendorID { get; set; }

        public Nullable<int> ProductStageID { get; set; }

        public string CopyFCERL(f_cerl fcerl, int FlowCode, int State)
        {
            f_cerl newfcerl = new f_cerl();
            newfcerl = fcerl;
            newfcerl.ID = int.Parse(null);
            newfcerl.udt = DateTime.Now;
            newfcerl.cdt = DateTime.Now;
            newfcerl.FlowCode = FlowCode;
            newfcerl.State = State;
            newfcerl.fID = Guid.NewGuid().ToString();
            return newfcerl.fID;
        }
    }

    public class RoleMenu
    {
        [Key]
        public int id { get; set; }

        [Display(Name = "目錄")]
        public int menuId { get; set; }

        [Display(Name = "角色")]
        public int RoleId { get; set; }

    }

    //id, menuId, Name, menuText, actionname, controlname, RoleId, RoleName, parentMenuId, menuType, sortOrder
    public class vRoleMenu
    {
        [Key]
        public int id { get; set; }
        public int menuId { get; set; }

        [Display(Name = "使用中的功能")]
        public string Name { get; set; }

        [Display(Name = "目錄名稱")]
        public string menuText { get; set; }

        public int RoleId { get; set; }

        [Display(Name = "角色")]
        public string RoleName { get; set; }

        public int parentMenuId { get; set; }

        public int menuType { get; set; }

        public int sortOrder { get; set; }

        [Display(Name = "Action")]
        public string actionname { get; set; }

        [Display(Name = "Control")]
        public string controlname { get; set; }
    }

    public class vUser
    {
        //BadgeCode, ChtName, Email, CorpTel, RoleName
        [Key]
        [Display(Name = "登入帳號")]
        public string BadgeCode { get; set; }

        [Display(Name = "姓名")]
        public string ChtName { get; set; }

        [Display(Name = "電子郵件")]
        public string Email { get; set; }

        [Display(Name = "公司電話")]
        public string CorpTel { get; set; }

        [Display(Name = "角色權限")]
        public string RoleName { get; set; }

        [Display(Name = "角色權限Id")]
        public int RoleId { get; set; }

        [Display(Name = "部門")]
        public string Dept { get; set; }

        [Display(Name = "工作說明")]
        public string Description { get; set; }

        public string Manager { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsGlobalReader { get; set; }
    }

    public class vRoleFunction
    {
        //menuId, menuText, RoleId, RoleName, BadgeCode, ChtName, parentMenuId, menuType, sortOrder
        [Key]
        public int menuId { get; set; }
        public string menuText { get; set; }
        public string actionname { get; set; }
        public string controlname { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string BadgeCode { get; set; }
        public string ChtName { get; set; }
        public int parentMenuId { get; set; }
        public int menuType { get; set; }
        public int sortOrder { get; set; }
    }

    public class CERLMenu
    {
        [Key]
        public int guid { get; set; }

        [Display(Name = "menuId")]
        public int menuId { get; set; }

        [Display(Name = "menuType")]
        public int menuType { get; set; }

        [Display(Name = "menuText")]
        public string menuText { get; set; }

        [Display(Name = "parentMenuId")]
        public int parentMenuId { get; set; }

        [Display(Name = "Order")]
        public int sortOrder { get; set; }

        [Display(Name = "Navigate Url")]
        public string navigateUrl { get; set; }

        [Display(Name = "menuTarget")]
        public string menuTarget { get; set; }

        public string actionname { get; set; }

        public string controlname { get; set; }

        [Display(Name = "Create")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime cdt { get; set; }

        [Display(Name = "Update")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime udt { get; set; }

        public string editor { get; set; }
    }

    public class vCERLMenu
    {
        [Key]
        public int guid { get; set; }

        [Display(Name = "menuId")]
        public int menuId { get; set; }

        [Display(Name = "menuType")]
        public int menuType { get; set; }

        [Display(Name = "menuText")]
        public string menuText { get; set; }

        [Display(Name = "parentMenuId")]
        public int parentMenuId { get; set; }

        [Display(Name = "Order")]
        public int sortOrder { get; set; }

        [Display(Name = "Navigate Url")]
        public string navigateUrl { get; set; }

        [Display(Name = "menuTarget")]
        public string menuTarget { get; set; }

        [Display(Name = "Action")]
        public string actionname { get; set; }

        [Display(Name = "Control")]
        public string controlname { get; set; }

        [Display(Name = "Create")]
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime cdt { get; set; }

        [Display(Name = "Update")]
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime udt { get; set; }

        public string editor { get; set; }

        public string TreeMenuText { get; set; }
        public string ChtName { get; set; }
        public string ParentMenuText { get; set; }
    }

    public class TestItemMenu
    {
        [Key]
        public int guid { get; set; }

        [Display(Name = "menuId")]
        public int menuId { get; set; }

        [Display(Name = "menuType")]
        public int menuType { get; set; }

        [Display(Name = "FlowCode")]
        public int FlowCode { get; set; }

        [Display(Name = "menuText")]
        public string menuText { get; set; }

        [Display(Name = "Full Name")]
        public string menuFullName { get; set; }

        [Display(Name = "ParentId")]
        public int parentMenuId { get; set; }

        [Display(Name = "Order")]
        public int sortOrder { get; set; }

        [Display(Name = "Navigate Url")]
        public string navigateUrl { get; set; }

        [Display(Name = "Create")]
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime cdt { get; set; }

        [Display(Name = "Update")]
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime udt { get; set; }

        public string editor { get; set; }
    }

    public class vTestItemMenu
    {
        //guid, menuId, menuText, menuFullName, menuType, FlowCode, parentMenuId, sortOrder, navigateUrl, cdt, udt, editor, MenuName, ParentMenuName, ChtName
        [Key]
        public int guid { get; set; }

        [Display(Name = "menuId")]
        public int menuId { get; set; }

        [Display(Name = "menuType")]
        public int menuType { get; set; }

        [Display(Name = "FlowCode")]
        public int FlowCode { get; set; }

        [Display(Name = "menuText")]
        public string menuText { get; set; }

        [Display(Name = "Full Name")]
        public string menuFullName { get; set; }

        [Display(Name = "ParentId")]
        public int parentMenuId { get; set; }

        [Display(Name = "Order")]
        public int sortOrder { get; set; }

        [Display(Name = "Navigate Url")]
        public string navigateUrl { get; set; }

        [Display(Name = "Create")]
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime cdt { get; set; }

        [Display(Name = "Update")]
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime udt { get; set; }

        public string editor { get; set; }

        [Display(Name = "階層顯示")]
        public string MenuName { get; set; }

        [Display(Name = "父階顯示")]
        public string ParentMenuName { get; set; }
        public string ChtName { get; set; }
    }

    public class mRole
    {
        [Key]
        public int id { get; set; }

        [Display(Name = "群組")]
        public int RoleId { get; set; }

        [Display(Name = "名稱")]
        public string RoleName { get; set; }

        [Display(Name = "Create")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime cdt { get; set; }

        [Display(Name = "Update")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime udt { get; set; }

        public string editor { get; set; }
    }

    //id, RoleId, RoleName, Parent, menuId, editor, cdt, udt, ParentRoleName, menuFullName
    public class vRole
    {
        [Key]
        public int id { get; set; }

        [Display(Name = "群組")]
        public int RoleId { get; set; }

        [Display(Name = "名稱")]
        public string RoleName { get; set; }

        public Nullable<int> Parent { get; set; }

        public Nullable<int> menuId { get; set; }
        public string ParentRoleName { get; set; }
        public string menuFullName { get; set; }

        [Display(Name = "Create")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime cdt { get; set; }

        [Display(Name = "Update")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime udt { get; set; }

        public string editor { get; set; }

        public string ChtName { get; set; }
    }

    //id, BadgeCode, ChtName, RoleId, RoleName, Parent, menuId, Description
    public class vUserRole
    {
        [Key]
        public int id { get; set; }
        public string BadgeCode { get; set; }
        public string ChtName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public Nullable<int> Parent { get; set; }
        public int menuId { get; set; }
        public string menuName { get; set; }
        public string Description { get; set; }
    }

    //id, BadgeCode, RoleId, Description
    public class UserRole
    {
        [Key]
        public int id { get; set; }
        public string BadgeCode { get; set; }
        public int RoleId { get; set; }
        public string Description { get; set; }
    }

    //BadgeCode, ChtName, EngName, Corp, Bplace, Dept, Email, CorpTel, PrivateTel, Description, Agent, EnableAgent, EnableAgent2, Manager
    public class m_userprofile
    {
        [Key]
        public string BadgeCode { get; set; }
        public string ChtName { get; set; }
        public string EngName { get; set; }
        public string Corp { get; set; }
        public string Bplace { get; set; }
        public string Dept { get; set; }
        public string Email { get; set; }
        public string CorpTel { get; set; }
        public string PrivateTel { get; set; }
        public string Description { get; set; }
        public string Agent { get; set; }
        public string EnableAgent { get; set; }
        public Nullable<bool> EnableAgent2 { get; set; }
        public string Manager { get; set; }
    }

    public class FlowPath
    {
        //inStateName,outState,outStateName,outStateAction
        public Nullable<int> inState { get; set; }
        public string inStateName { get; set; }
        public string inStateAction { get; set; }
        public Nullable<int> outState { get; set; }
        public string outStateName { get; set; }
        public string outStateAction { get; set; }
        public Nullable<int> lvl { get; set; }
    }

    public class FlowState
    {
        [Key]
        public int State { get; set; }
        public string Title { get; set; }
    }

    public class sysErrorMessage
    {
        [Key]
        public Int32 ID { get; set; }
        public string SourceFunction { get; set; }
        public string ContentMessage { get; set; }
        public string editor { get; set; }
        public DateTime cdt { get; set; }
    }

    public class sysErrorMessageDBSet
    {
        private CERLDBContext db = new CERLDBContext();
        private sysErrorMessage mSysErrorMessage = new sysErrorMessage();
        
        public sysErrorMessageDBSet()
        {

        }

        public void InitErrorData(string Src, string content, string editor)
        {
            mSysErrorMessage.SourceFunction = Src;
            mSysErrorMessage.ContentMessage = content;
            mSysErrorMessage.editor = editor;
            mSysErrorMessage.cdt = DateTime.Now;
            SetErrorData(mSysErrorMessage);
        }

        private void SetErrorData(sysErrorMessage syserror)
        {
            if(syserror != null)
            {
                db.sysErrorMessages.Add(syserror);
                db.SaveChanges();
            }
        }
    }

    public class CERLDBContext : DbContext
    {
        public CERLDBContext() : base("CERLDBContext")
        {
            
        }
        public DbSet<ReturnSite> ReturnSite { get; set; }
        public DbSet<ReturnType> ReturnTypes { get; set; }
        public DbSet<TestPurpose> TestPurposes { get; set; }
        public DbSet<IssueSource> IssueSources { get; set; }
        public DbSet<CustomerName> CustomerNames { get; set; }
        public DbSet<PCBVendor> PCBVendor { get; set; }
        public DbSet<ProcessStep> ProcessSteps { get; set; }
        public DbSet<ProductStage> ProductStage { get; set; }
        public DbSet<mRole> mRoles { get; set; }
        public DbSet<RoleMenu> RoleMenu { get; set; }
        public DbSet<CERLMenu> CERLMenu { get; set; }
        public DbSet<vCERLMenu> vcerlmenu { get; set; }
        public DbSet<s_form_authority> s_form_authority { get; set; }
        public DbSet<f_cerl> f_cerl { get; set; }
        public DbSet<vFCERL> vFCERL { get; set; }
        public DbSet<vFCERLHeader> vFCERLHeader { get; set; }
        public DbSet<attachFile> attachFiles { get; set; }
        public DbSet<attachFileVer> attachFileVer { get; set; }
        public DbSet<vattachFileMaxVer> vattachFileMaxVer { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<m_userprofile> userprofile { get; set; }
        public DbSet<vRoleMenu> vRoleMenu { get; set; }
        public DbSet<vRole> vRole { get; set; }
        public DbSet<vUserRole> vUserRole { get; set; }
       
        public DbSet<vUser> vUsers { get; set; }
        public DbSet<vRoleFunction> vRoleFunctions { get; set; }
        public DbSet<TreeNode> TreeNode { get; set; }
        public DbSet<ItemTreeNode> ItemTreeNode { get; set; }
        public DbSet<MenuNode> MenuNode { get; set; }
        public DbSet<TestItemMenu> TestItemMenu { get; set; }
        public DbSet<vTestItemMenu> vTestItemMenus { get; set; }
        public DbSet<LabInformation> LabInformation { get; set; }
        public DbSet<vLabInformation> vLabInformation { get; set; }
        public DbSet<vLabInfoAttachFile> vLabInfoAttachFile { get; set; }
        public DbSet<FlowState> FlowState { get; set; }

        public DbSet<vCountOnHandWork> vCountOnHandWork { get; set; }
        public DbSet<vLABMemberOnHandWork> vLABMemberOnHandWork { get; set; }

        public DbSet<sysErrorMessage> sysErrorMessages { get; set; }

        public IEnumerable<ProductStage > fn_GetProductStage(int CustomerID)
        {
            CERLDBContext db = new CERLDBContext();
            IEnumerable<ProductStage> ps = db.ProductStage.Where(x => x.CustomerID == CustomerID);
            return ps;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, "[SaveChanges] The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
    }

    public class CERLDataContent: System.Data.Linq.DataContext
    {
        CERLDBContext db = new CERLDBContext();
        public CERLDataContent()
            : base("CERLDBContext")
        {

        }

        public IEnumerable<vFCERLHeader> sp_GetVFCERL(List<string> fIDList)
        {
            DataTable fIDListTable = new DataTable();
            fIDListTable.Columns.Add("fID", typeof(string));
            foreach(string f in fIDList)
            {
                fIDListTable.Rows.Add(f);
            }

            string ConnStr = ConfigurationManager.ConnectionStrings[Constant.ConnCERLDBContext].ToString();
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                SqlCommand cmd = new SqlCommand("sp_GetVFCERL", conn);
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable("vFCERLHeader");
                cmd.CommandTimeout = 1800;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter p = cmd.Parameters.AddWithValue("@fIDList", fIDListTable);
                p.SqlDbType = System.Data.SqlDbType.Structured;
                p.TypeName = "fID_List";
                da.SelectCommand = cmd;
                da.Fill(dt);

                IEnumerable<vFCERLHeader> vfcerl = from f in dt.AsEnumerable()
                                                   select new vFCERLHeader
                                                   {
                                                 ID = f.Field<int>("ID"),
                                                 fID = f.Field<string>("fID"),
                                                 UID = f.Field<string>("UID"),
                                                 CaseID = f.Field<string>("CaseID"),
                                                 ApplicantId = f.Field<string>("ApplicantId"),
                                                 Applicant = f.Field<string>("Applicant"),
                                                 SerialNumber = f.Field<string>("SerialNumber"),
                                                 TestItemId = f.Field<int>("TestItemId"),
                                                 TestItem = f.Field<string>("TestItem"),
                                                 LabMemberId = f.Field<string>("LabMemberId"),
                                                 LabMember = f.Field<string>("LabMember"),
                                                 PartNumber = f.Field<string>("PartNumber"),
                                                 TestPurpose = f.Field<string>("TestPurpose"),
                                                 ProcessStep = f.Field<string>("ProcessStep"),
                                                 FinishDate = f.Field<DateTime?>("FinishDate"),
                                                 NextTestDate = f.Field<DateTime?>("NextTestDate"),
                                                 cdt = f.Field<DateTime>("cdt"),
                                                 udt = f.Field<DateTime>("udt"),
                                                 editor = f.Field<string>("editor"),
                                                 Action = f.Field<string>("Action"),
                                                 State = f.Field<int>("State"),
                                                 StateName = f.Field<string>("StateName"),
                                                 PreTestDate = f.Field<string>("PreTestDate")
                                             };
                //conn.Open();
                //SqlDataReader reader = cmd.ExecuteReader();
                //return this.CreateMethodCallQuery<vFCERL>(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), fIDList);
                //return this.Translate<vFCERL>(reader).ToList();
                return vfcerl;
            }
        }
    }
}