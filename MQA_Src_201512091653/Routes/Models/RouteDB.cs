using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Routes.Controllers.General;

namespace Routes.Models
{
    public class RouteDB
    {

    }

    public class FlowPath2
    {
        //inStateName,outState,outStateName,outStateAction
        public Nullable<int> inState { get; set; }
        public string inStateName { get; set; }
        public Nullable<int> outState { get; set; }
        public string outStateName { get; set; }
        public string outStateAction { get; set; }
        public Nullable<int> lvl { get; set; }
    }
    

    public class FlowPath
    {
        private r_flow_path rootPath;
        private string inState { get; set; }
        public Dictionary<int, r_flow_path> Paths { get; set; }

        public r_flow_path RootPath
        {
            get { return rootPath; }
            set
            {
                if (RootPath != null)
                {
                    Paths.Remove(RootPath.inState);
                }
                Paths.Add(value.inState, value);
                rootPath=value;
            }

        }

        public void BuildPath()
        {
            r_flow_path inPath;
            foreach (var path in Paths.Values)
            {
                if (Paths.TryGetValue(path.inState, out inPath) && path.outState != path.inState)
                {
                    path.inPath = inPath;

                    inPath.outPath.Add(path);
                }
            }
        }

        public FlowPath()
        {

        }
    }

    public class r_flow_path
    {
        [Key]
        public int PathID { get; set; }
        public int inFlowCode { get; set; }
        public int inState { get; set; }
        public int outFlowCode { get; set; }
        public int outState { get; set; }
        public string outPathType { get; set; }
        public int doSeq { get; set; }
        public int toSeq { get; set; }
        public int ccSeq { get; set; }
        public int pathOrder { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Value { get; set; }
        public string Op { get; set; }
        public r_flow_path inPath { get; set; }
        public List<r_flow_path> outPath = new List<r_flow_path>();
    }

    public class r_flow_to
    {
        [Key]
        public int ID { get; set; }
        public string toCode { get; set; }
        public string toField { get; set; }
        public string toDept { get; set; }
        public Nullable<int> toRole { get; set; }
        public string toPrefix { get; set; }
        public Nullable<int> doSeq { get; set; }
        public string toGroup { get; set; }
    }

    public class r_flow_state
    {
        //ID, FlowCode, State, Title, Remark, Action, editor, cdt, udt
        [Key]
        public int ID { get; set; }
        public int FlowCode { get; set; }
        public int State { get; set; }
        public string Title { get; set; }
        public string Remark { get; set; }
        public string Action { get; set; }
        public string editor { get; set; }

        [Display(Name = "Create")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime cdt { get; set; }

        [Display(Name = "Update")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime udt { get; set; }
    }

    public class r_flow_action
    {
        [Key]
        public int ID { get; set; }
        public int FlowCode { get; set; }
        public int State { get; set; }
        public string Action { get; set; }
        public string Title { get; set; }
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

    //id, BadgeCode, RoleId, RoleName, Description
    public class vUserRole
    {
        [Key]
        public int id { get; set; }
        public string BadgeCode { get; set; }
        public string ChtName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
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
    }

    public class v_userrole
    {
        //id, BadgeCode, RoleId, Description, RoleName
        [Key]
        public int id { get; set; }
        public string BadgeCode { get; set; }
        public int RoleId { get; set; }
        public string Description { get; set; }
        public string RoleName { get; set; }
    }

    public class p_sign_off_task
    {
        //TaskID, FID, EquID, FormCode, FlowCode, State, Applicant, Cdt, Udt
        [Key]
        public string TaskID { get; set; }
        public string FID { get; set; }
        public string EquID { get; set; }
        public int FormCode { get; set; }
        public int FlowCode { get; set; }
        public int State { get; set; }
        public string Applicant { get; set; }

        [Display(Name = "Create")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime Cdt { get; set; }

        [Display(Name = "Update")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime Udt { get; set; }
        List<p_task_detail> taskDetail = new List<p_task_detail>();
    }

  

    public class vTask
    {
        //TaskID, FID, EquID, FormCode, FlowCode, CurrentState, HistoryState, Applicant, Action, GroupCode, Comment, S_date, F_date, Editor
        //SELECT ID, TaskID, FID, FormCode, FlowCode, CurrentState, HistoryState, HistoryStateName, AssignedTo, AssignedToName, Applicant, ApplicantName, Action, ActionName, Comment, S_date, F_date, Editor, EditorName FROM vTask WHERE FID = 'A286C309-944B-415B-AF29-F6AD297C4346'
        [Key]
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

    public class p_task_detail
    {
        //ID, TaskID, Action, AssignedTo, GroupCode, Comment, S_date, F_date, Editor, State
        [Key]
        public int ID { get; set; }
        public string TaskID { get; set; }
        public string Action { get; set; }
        public string AssignedTo { get; set; }
        public string GroupCode { get; set; }
        public string Comment { get; set; }

        [Display(Name = "開始日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime S_date { get; set; }

        [Display(Name = "完成日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime F_date { get; set; }
        public string Editor { get; set; }
        public int State { get; set; }
    }

    public class m_userprofile
    {
        //BadgeCode, ChtName, EngName, Corp, Bplace, Dept, Email, CorpTel, PrivateTel, Description, Agent, EnableAgent, EnableAgent2, Manager
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
        public bool EnableAgent2 { get; set; }
        public string Manager { get; set; }
    }

    public class RouteDBContext : DbContext
    {
        public RouteDBContext(): base("RouteDBContext")
        {

        }

        public DbSet<mRole> mRoles { get; set; }
        public DbSet<r_flow_path> r_flow_path { get; set; }
        public DbSet<r_flow_state> r_flow_state { get; set; }
        public DbSet<r_flow_to> r_flow_to { get; set; }
        public DbSet<r_flow_action> r_flow_action { get; set; }
        public DbSet<v_userrole> v_userrolw { get; set; }
        public DbSet<p_sign_off_task> p_sign_off_task { get; set; }
        public DbSet<p_task_detail> p_task_detail { get; set; }
        public DbSet<vTask> vTask { get; set; }
        public DbSet<vUser> vUser { get; set; }
        public DbSet<vUserRole> vUserRole { get; set; }
        public DbSet<m_userprofile> m_userprofile { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}