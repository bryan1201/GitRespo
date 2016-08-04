using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Data.Entity.Validation;
using Meetings.Controllers.General;
using System.Text;

namespace Meetings.Models
{
    public class Meeting
    {
        [Key]
        public Guid meetingId { get; set; }
        [Display(Name ="名稱")]
        public string MeetingName { get; set; }
        [Display(Name ="期別場次")]
        public IEnumerable<vMeetingDate> vmeetingDates {
            get {
                return _db.vMeetingDates.Where(x => x.meetingId == meetingId);
            }
        }
        private ChruchLifeDBContext _db { get; set; }
        public Meeting(ChruchLifeDBContext db)
        {
            this._db = db;
        }

        public Meeting()
        {
            if (this._db == null)
                this._db = new ChruchLifeDBContext();
        }

        public void Insert(Meeting meeting)
        {
            _db.Meetings.Add(meeting);
            _db.SaveChanges();
        }

        public void Update(Meeting meeting)
        {
            _db.Entry(meeting).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Remove(Guid Id)
        {
            Meeting m = _db.Meetings.Find(Id);
            if (m != null)
            {
                _db.Meetings.Remove(m);
                _db.SaveChanges();
            }
        }

        public IEnumerable<Meeting> Get()
        {
            return _db.Meetings;
        }

        public Meeting Get(Guid Id)
        {
            Meeting m = _db.Meetings.Find(Id);
            return m;
        }
    }

    public class MeetingList
    {
        public IList<Meeting> meetingList { get; set; }
        public MeetingList()
        {
            /*
            meetingList = new List<Meeting>();
            meetingList.Add(new Meeting(0, "區負責弟兄訓練"));
            meetingList.Add(new Meeting(1, "姊妹召集人訓練"));
            meetingList.Add(new Meeting(2, "生命成全訓練"));
            meetingList.Add(new Meeting(3, "北投大區排組成全訓練"));
            */
        }

        public void Clear()
        {
            meetingList.Clear();
        }

        public IEnumerable<Meeting> QueryMeeting()
        {
            IEnumerable<Meeting> b = meetingList;
            return b;
        }

        public Meeting Query(Guid Id)
        {
            Meeting b = new Meeting();
            if (Id == null)
                return b;
            b = meetingList.Where(x => x.meetingId == Id).FirstOrDefault();
            return b;
        }
    }

    public class MeetingDate
    {
        [Key]
        public Guid meetingDateId { get; set; }
        public Guid meetingId { get; set; }
        public DateTime CDT { get; set; }
        public DateTime STDT { get; set; }
        public DateTime ENDDT { get; set; }
        public string Description { get; set; }
        [Display(Name ="場次")]
        public IEnumerable<vMeetingDateActivate> vmeetingDateActivates
        {
            get
            {
                return _db.vMeetingDateActivates.Where(x => x.meetingDateId == meetingDateId);
            }
        }
        private ChruchLifeDBContext _db { get; set; }
        public MeetingDate(ChruchLifeDBContext db)
        {
            this._db = db;
        }

        public MeetingDate()
        {
            if (this._db == null)
                this._db = new ChruchLifeDBContext();
        }

        public void Insert(MeetingDate md)
        {
            if (md == null)
                return;
            _db.MeetingDates.Add(md);
            _db.SaveChanges();
        }

        public void Update(MeetingDate md)
        {
            if (md == null)
                return;
            _db.Entry(md).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Remove(Guid? meetingDateId)
        {
            if (meetingDateId == null)
                return;
            MeetingDate md = _db.MeetingDates.Find(meetingDateId);
            if (md == null)
                return;

            _db.MeetingDates.Remove(md);
            _db.SaveChanges();
        }

        public IEnumerable<vMeetingDate> Get(Guid meetingId)
        {
            return _db.vMeetingDates.Where(x => x.meetingId == meetingId);
        }

        public vMeetingDate GetCurrent(Guid meetingdateId)
        {
            return _db.vMeetingDates.Where(x => x.meetingDateId == meetingdateId).FirstOrDefault();
        }
    }

    public class vMeetingDate
    {
        [Key]
        public Guid meetingDateId { get; set; }
        public Guid meetingId { get; set; }
        public string MeetingName { get; set; }
        public string Description { get; set; }
        public DateTime CDT { get; set; }
        [Display(Name = "開始")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime STDT { get; set; }
        [Display(Name = "結束")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime ENDDT { get; set; }
        [Display(Name = "場次")]
        public IEnumerable<vMeetingDateActivate> vmeetingDateActivate { get
            {
                return _db.vMeetingDateActivates.Where(x => x.meetingDateId == meetingDateId);
            } }
        private ChruchLifeDBContext _db { get; set; }
        public vMeetingDate(ChruchLifeDBContext db)
        {
            this._db = db;
        }

        public vMeetingDate()
        {
            if (this._db == null)
                this._db = new ChruchLifeDBContext();
        }

    }

    public class vMeetingDateActivate
    {
        [Key]
        public Guid ActivateId { get; set; }
        public Guid meetingDateId { get; set; }
        [Display(Name = "聚會日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime ActivateDate { get; set; }
        [Display(Name ="場次說明")]
        public string ActivateDesc { get; set; }
        [Display(Name = "開始")]
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public DateTime? ActivateSDT { get; set; }
        [Display(Name = "結束")]
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public DateTime? ActivateEDT { get; set; }
        public Guid meetingId { get; set; }
        public string MeetingName { get; set; }
        public string Description { get; set; }
        public DateTime CDT { get; set; }
        public DateTime STDT { get; set; }
        public DateTime ENDDT { get; set; }

    }

    public class vMeetingDateMember
    {
        [Key]
        public Guid uniqueId { get; set; }
        public Guid meetingDateId { get; set; }
        public string UserCode { get; set; }
        public Guid UserProfileId { get; set; }
        public string UserName { get; set; }
        public string Sex { get; set; }
        public string MeetingName { get; set; }
        public string Description { get; set; }
        public DateTime STDT { get; set; }
        public DateTime ENDDT { get; set; }
        public DateTime CDT { get; set; }
        private ChruchLifeDBContext _db { get; set; }
        public vMeetingDateMember(ChruchLifeDBContext db)
        {
            this._db = db;
        }

        public vMeetingDateMember()
        {
            if (this._db == null)
                this._db = new ChruchLifeDBContext();
        }

        public IEnumerable<vMeetingDateMember> GetMeetingMemberList(Guid meetingdateId)
        {
            return _db.vMeetingDateMembers.Where(x=>x.meetingDateId== meetingdateId);
        }

        public vMeetingDateMember GetMeetingMember(Guid meetingdateId, string usercode)
        {
            usercode = (string.IsNullOrEmpty(usercode)) ? string.Empty : usercode;

            return _db.vMeetingDateMembers.Where(x => x.UserCode == usercode && x.meetingDateId == meetingdateId).FirstOrDefault();
        }
    }

    public class vMeetingDateActivateMember
    {
        [Key]
        public Guid uniqueId { get; set; }
        public Guid meetingDateId { get; set; }
        public string UserCode { get; set; }
        public Guid UserProfileId { get; set; }
        public string UserName { get; set; }
        public string MeetingName { get; set; }
        public string Description { get; set; }
        public DateTime STDT { get; set; }
        public DateTime ENDDT { get; set; }
        public DateTime CDT { get; set; }
        public Guid? ActivateId { get; set; }
        public DateTime? ActivateDate { get; set; }
        public string ActivateDesc { get; set; }
        private ChruchLifeDBContext _db { get; set; }
        public vMeetingDateActivateMember(ChruchLifeDBContext db)
        {
            this._db = db;
        }

        public vMeetingDateActivateMember()
        {
            if (this._db == null)
                this._db = new ChruchLifeDBContext();
        }

        public IEnumerable<vMeetingDateActivateMember> GetMeetingDateActivateMemberList(Guid activateid)
        {
            return _db.vMeetingDateActivateMembers.Where(x => x.ActivateId == activateid);
        }

        public vMeetingDateActivateMember GetMeetingDateActivateMember(Guid activateid, string usercode)
        {
            usercode = (string.IsNullOrEmpty(usercode)) ? string.Empty : usercode;

            return _db.vMeetingDateActivateMembers.Where(x => x.UserCode == usercode && x.ActivateId == activateid).FirstOrDefault();
        }
    }

    public class MeetingDateMember
    {
        [Key]
        public Guid uniqueId { get; set; }
        public Guid meetingDateId { get; set; }
        public string UserCode { get; set; }
        public Guid UserProfileId { get; set; }
        public DateTime CDT { get; set; }
        public string GetSex()
        {
            return _db.UserProfile.Where(x => x.uniqueId == UserProfileId).FirstOrDefault().Sex;
        }

        public string GetUserName()
        {
            return _db.UserProfile.Find(UserProfileId).UserName;
        }

        public string GetMeetingDateName()
        {
            return _db.MeetingDates.Find(meetingDateId).Description;
        }

        private ChruchLifeDBContext _db { get; set; }
        public MeetingDateMember(ChruchLifeDBContext db)
        {
            this._db = db;
        }
        public MeetingDateMember()
        {
            if (this._db == null)
                this._db = new ChruchLifeDBContext();
        }

        public void Insert(MeetingDateMember mdm)
        {
            mdm.UserProfileId = _db.UserProfile.Where(x => x.UserCode == mdm.UserCode).FirstOrDefault().uniqueId;
            mdm.CDT = DateTime.Now;
            vMeetingDateMember chkmdm = _db.vMeetingDateMembers.Where(x => x.UserProfileId == mdm.UserProfileId && x.meetingDateId == mdm.meetingDateId).FirstOrDefault();
            if (chkmdm != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(chkmdm.UserCode);
                sb.Append(".");
                sb.Append(chkmdm.UserName);
                sb.Append(" 已報名過 ");
                sb.Append(chkmdm.STDT.ToString("yyyy"));
                sb.Append(chkmdm.MeetingName);
                sb.Append(".");
                sb.Append(chkmdm.Description);
                sb.Append(" (");
                sb.Append(chkmdm.STDT.ToString("MM/dd"));
                sb.Append(" ~ ");
                sb.Append(chkmdm.ENDDT.ToString("MM/dd"));
                sb.Append(")");
                throw new Exception(sb.ToString());
            }
            _db.MeetingDateMembers.Add(mdm);
            _db.SaveChanges();
        }

        public void Insert(Guid meetingdateId, string usercode)
        {
            bool chk = false;
            chk = ChkMeetingMember(meetingdateId, usercode);
            if (chk)
                return;
            MeetingDateMember mdm = new MeetingDateMember();
            mdm.uniqueId = Guid.NewGuid();
            mdm.meetingDateId = meetingdateId;
            mdm.UserCode = usercode;
            mdm.CDT = DateTime.Now;

            _db.MeetingDateMembers.Add(mdm);
            _db.SaveChanges();
        }

        public void Update(MeetingDateMember mdm)
        {
            if (mdm == null)
                return;
            _db.Entry(mdm).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Remove(Guid uniqueId)
        {
            if (uniqueId == null)
                return;
            MeetingDateMember mdm = _db.MeetingDateMembers.Find(uniqueId);
            if (mdm == null)
                return;
            _db.MeetingDateMembers.Remove(mdm);
            _db.SaveChanges();
        }

        public void Remove(Guid meetingdateId, string usercode)
        {
            bool chk = false;
            chk = ChkMeetingMember(meetingdateId, usercode);
            if (chk)
                return;
            MeetingDateMember mdm = _db.MeetingDateMembers.Where(x => x.meetingDateId == meetingdateId && x.UserCode == usercode).FirstOrDefault();
            if (mdm == null)
                return;
            _db.MeetingDateMembers.Remove(mdm);
            _db.SaveChanges();
        }

        public IEnumerable<string> GetUserList(Guid meetingdateId)
        {
            IEnumerable<string> rslt = _db.MeetingDateMembers.Where(x => x.meetingDateId == meetingdateId).Select(x => x.UserCode);
            return rslt;
        }

        public IEnumerable<vMeetingDateMember> GetMeetingDateMembers(Guid meetingdateid)
        {
            return _db.vMeetingDateMembers.Where(x=>x.meetingDateId==meetingdateid);
        }

        public IEnumerable<vMeetingDateMember> Get()
        {
            return _db.vMeetingDateMembers;
        }

        public vMeetingDateMember Get(Guid uniqueId)
        {
            return _db.vMeetingDateMembers.Find(uniqueId);
        }

        public MeetingDateMember GetCurrentEdit(Guid uniqueId)
        {
            return _db.MeetingDateMembers.Find(uniqueId);
        }

        public bool ChkMeetingMember(Guid meetingdateId, string usercode)
        {
            bool rslt = false;
            MeetingDateMember mdm = _db.MeetingDateMembers.Where(x => x.meetingDateId == meetingdateId && x.UserCode == usercode).FirstOrDefault();
            rslt = (mdm == null) ? false : true;
            return rslt;
        }
    }

    public class MeetingDateActivate
    {//ActivateId, meetingId, ActivateDate, ActivateDesc
        [Key]
        public Guid ActivateId { get; set; }
        public Guid meetingDateId { get; set; }
        public DateTime ActivateDate { get; set; }
        public string ActivateDesc { get; set; }
        public DateTime? SDT { get; set; }
        public DateTime? EDT { get; set; }
        private ChruchLifeDBContext _db { get; set; }
        public MeetingDateActivate()
        {
            if (_db == null)
                _db = new ChruchLifeDBContext();
        }

        public MeetingDateActivate(ChruchLifeDBContext db)
        {
            if (db == null)
                _db = new ChruchLifeDBContext();
            else
            {
                if (this._db == null)
                    this._db = db;
            }
        }

        private bool ChkActivate(Guid activateId)
        {
            _db = new ChruchLifeDBContext();
            IEnumerable<MeetingDateActivate> malist = _db.MeetingDateActivateList.Where(x => x.ActivateId == activateId);
            MeetingDateActivate ma = null;
            if (malist != null)
            {
                ma = malist.FirstOrDefault();
            }
            if (ma == null)
                return true;
            else
                return false;
        }

        public void Insert(MeetingDateActivate meetingdateactivate)
        {
            _db.MeetingDateActivateList.Add(meetingdateactivate);
            _db.SaveChanges();
        }

        public void Update(MeetingDateActivate meetingdateactivate)
        {
            if (meetingdateactivate == null)
                return;
            _db.Entry(meetingdateactivate).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Remove(Guid Id)
        {
            if (Id == null)
                return;
            MeetingDateActivate mda = _db.MeetingDateActivateList.Find(Id);
            _db.MeetingDateActivateList.Remove(mda);
            _db.SaveChanges();
        }

        public IEnumerable<vMeetingDateActivate> Get(Guid meetingDateId)
        {
            _db = new ChruchLifeDBContext();
            IEnumerable<vMeetingDateActivate> vmda = _db.vMeetingDateActivates.Where(x => x.meetingDateId == meetingDateId);
            return vmda;
        }

        public vMeetingDateActivate GetCurrent(Guid activateid)
        {
            vMeetingDateActivate vmda = _db.vMeetingDateActivates.Where(x => x.ActivateId == activateid).FirstOrDefault();
            return vmda;
        }

        public MeetingDateActivate GetCurrentEdit(Guid activateid)
        {
            MeetingDateActivate mda = _db.MeetingDateActivateList.Where(x => x.ActivateId == activateid).FirstOrDefault();
            return mda;
        }
    }

    public class MeetingDateMemberLog
    {
        //LogId, MeetingDateId, UserProfileId, UserCode, LoginTime
        [Key]
        public Guid LogId {get;set;}
        public Guid ActivateId { get; set; }
        public Guid UserProfileId { get; set; }
        public string UserCode { get; set; }
        public DateTime LoginTime { get; set; }
        private ChruchLifeDBContext _db { get; set; }
        public MeetingDateMemberLog()
        {
            if (_db == null)
                _db = new ChruchLifeDBContext();
        }

        public MeetingDateMemberLog(ChruchLifeDBContext db)
        {
            if (db == null)
                _db = new ChruchLifeDBContext();
            else
            {
                if (this._db == null)
                    this._db = db;
            }
        }

        private bool ChkLogin(Guid activateid, string usercode)
        {
            _db = new ChruchLifeDBContext();
            MeetingDateMemberLog mdmlog = null;
            vMeetingDateMemberLog vmdml = _db.vMeetingDateMemberLogs.Where(x => x.ActivateId == activateid && x.UserCode == usercode).FirstOrDefault();
            if (vmdml != null)
            {
                mdmlog = _db.MeetingDateMemberLogs.Where(x => x.ActivateId == activateid && x.UserCode == usercode).FirstOrDefault();
            }
            if (mdmlog == null)
                return true;
            else
                return false;
        }

        private bool ChkMeetingDateMember(Guid meetingdateId, string usercode)
        {
            vMeetingDateMember mdm = new vMeetingDateMember();
            mdm = mdm.GetMeetingMember(meetingdateId, usercode);
            if (mdm == null)
                return false;
            else
                return true;
        }

        public void Login(Guid activateid, string usercode, out string message)
        {
            try
            {
                UserProfile up = new UserProfile();
                up = up.Get(usercode);
                MeetingDateMemberLog mdml = new MeetingDateMemberLog();

                if (!ChkMeetingDateMember(activateid, usercode))
                {
                    message = Constant.NOTMEETINGDATEMEMBER;
                }

                if (ChkLogin(activateid, usercode))
                {
                    mdml.LogId = Guid.NewGuid();
                    mdml.ActivateId = activateid;
                    mdml.UserProfileId = up.uniqueId;
                    mdml.UserCode = usercode;
                    mdml.LoginTime = DateTime.Now;
                    Insert(mdml);
                    //StringBuilder sb = new StringBuilder();
                    //sb.Append("INSERT INTO MeetingDateMemberLog(LogId, MeetingDateId, UserProfileId, UserCode, LoginTime) VALUES(@)");
                    //string values = @"'" + Guid.NewGuid().ToString() + "','" + mdml.MeetingDateId.ToString() + "','" + mdml.UserProfileId.ToString() + "','" + mdml.UserCode + "','" + mdml.LoginTime.ToString("yyyy/MM/dd HH:mm:ss") + "'";
                    //string sql = sb.ToString().Replace("@", values);
                    //Controllers.General.DAO.sqlCmd("ChurchLifeDBContext", sql);
                    message = Constant.SUCCESSFUL;
                }
                else
                {
                    //string rslt = outmdmlog.LoginTime.ToString("yyyy/MM/dd (ddd) HH:mm:ss");
                    message =  Constant.DOUBLELOGIN;
                }
            }
            catch (Exception ex)
            {
                string rslt = string.Empty;
                rslt = Constant.FAIL + " " + ex.Message;
                message =  rslt;
            }
        }

        public void Insert(MeetingDateMemberLog mdml)
        {
            if (mdml == null)
                return;
            _db.MeetingDateMemberLogs.Add(mdml);
            _db.SaveChanges();
        }

        public void Update(MeetingDateMemberLog mdml)
        {
            if (mdml == null)
                return;
            _db.Entry(mdml).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Remove(Guid logId)
        {
            if (logId == null)
                return;

            MeetingDateMemberLog mdml = _db.MeetingDateMemberLogs.Find(logId);
            _db.MeetingDateMemberLogs.Remove(mdml);
            _db.SaveChanges();
        }

        public IEnumerable<vMeetingDateMemberLog> Get(Guid activateId)
        {
            IEnumerable<vMeetingDateMemberLog> mdml = _db.vMeetingDateMemberLogs.Where(x => x.ActivateId == activateId);
            return mdml;
        }

        public IEnumerable<vMeetingDateAttendStatus> GetReport(Guid activateId)
        {
            IEnumerable<vMeetingDateAttendStatus> vmdas = _db.vMeetingDateAttendStatus.Where(x => x.ActivateId == activateId);
            return vmdas;
        }

        public vMeetingDateMemberLog GetMeetingDateMemberLog(Guid logId)
        {
            if (logId == null)
                return null;
            vMeetingDateMemberLog vmdml = _db.vMeetingDateMemberLogs.Find(logId);
            return vmdml;
        }
    }

    public class vMeetingDateMemberLog
    {
        [Key]
        public Guid LogId { get; set; }
        public Guid ActivateId { get; set; }
        public string ActivateDesc { get; set; }
        public string UserCode { get; set; }
        public Guid UserProfileId { get; set; }
        public string UserName { get; set; }
        public DateTime LoginTime { get; set; }
        public Guid meetingDateId { get; set; }
        public string MeetingName { get; set; }
        public string Description { get; set; }
        public DateTime STDT { get; set; }
        public DateTime ENDDT { get; set; }
        private ChruchLifeDBContext _db { get; set; }
        public vMeetingDateMemberLog(ChruchLifeDBContext db)
        {
            this._db = db;
        }

        public vMeetingDateMemberLog()
        {
            if (this._db == null)
                this._db = new ChruchLifeDBContext();
        }

        public IEnumerable<vMeetingDateMemberLog> GetMeetingDateMemberList(Guid activateid)
        {
            IEnumerable<vMeetingDateMemberLog> vmdmls = _db.vMeetingDateMemberLogs.Where(x => x.ActivateId == activateid);
            return vmdmls;
        }

        public vMeetingDateMemberLog GetMeetingDateMemberLog(Guid activateid, string usercode)
        {
            usercode = string.IsNullOrEmpty(usercode) ? string.Empty : usercode;
            vMeetingDateMemberLog vmdml = _db.vMeetingDateMemberLogs.Where(x => x.UserCode == usercode && x.ActivateId == activateid).FirstOrDefault();
            if(vmdml==null)
            {
                vmdml = new vMeetingDateMemberLog();
                vmdml.ActivateId = activateid;
                vmdml.LoginTime = DateTime.Now;
            }
            return vmdml;
        }
    }

    public class vMeetingDateAttendStatus
    {
        [Key]
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Sex { get; set; }
        public DateTime? LoginTime { get; set; }
        public bool? Attend { get; set; }
        public Guid? ActivateId { get; set; }
    }

    public class UserProfile
    {
        [Key]
        public Guid uniqueId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Sex { get; set; } 
        public string UserCodeOld { get; set; }
        private ChruchLifeDBContext _db { get; set; }
        public UserProfile(ChruchLifeDBContext db)
        {
            this._db = db;
        }

        public UserProfile()
        {
            if (this._db == null)
                this._db = new ChruchLifeDBContext();
        }

        private DateTime _createdate { get; set; }
        public DateTime cdt
        {
            get
            {
                return _createdate;
            }
            set
            {
                if (value != null)
                    _createdate = value;
                else
                    _createdate = DateTime.Now;
            }
        }
        public DateTime udt { get; set; }
        public bool Enable { get; set; }
        public UserProfile(string barcode, string username, DateTime current)
        {
            this.UserCode = barcode.Trim();
            this.UserName = username.Trim();
            this.udt = (current == null) ? DateTime.Now : current;
        }
    
        public UserProfile Get(string usercode)
        {
            usercode = string.IsNullOrEmpty(usercode.Trim()) ? string.Empty : usercode.Trim();
            return _db.UserProfile.Where(x => x.UserCode == usercode).FirstOrDefault();
        }

        public UserProfile GetProfile(string userinfo)
        {
            userinfo = string.IsNullOrEmpty(userinfo.Trim()) ? string.Empty : userinfo.Trim();
            Guid uniqueId = Guid.Empty;
            Guid.TryParse(userinfo, out uniqueId);
            return _db.UserProfile.Where(x => x.UserCode.Contains(userinfo) || x.uniqueId == uniqueId || x.UserName.Contains(userinfo)).FirstOrDefault();
        }

        public IEnumerable<UserProfile> Get()
        {
            return _db.UserProfile.OrderBy(x => x.UserCode);
        }

        public IEnumerable<UserProfile> Search(string condition)
        {
            Guid uniqueId = Guid.Empty;
            Guid.TryParse(condition, out uniqueId);
            IEnumerable<UserProfile> up = _db.UserProfile.Where(x => x.UserCode.Contains(condition) || x.uniqueId == uniqueId || x.UserName.Contains(condition));
            return up.OrderBy(x => x.UserCode);
        }

        public void Insert(UserProfile up)
        {
            _db.UserProfile.Add(up);
            _db.SaveChanges();
        }

        public void Update(UserProfile up)
        {
            up.udt = DateTime.Now;
            _db.Entry(up).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Remove(Guid id)
        {
            UserProfile up = _db.UserProfile.Find(id);
            _db.UserProfile.Remove(up);
            _db.SaveChanges();
        }
    }
    
    public class ChruchLifeDBContext: DbContext
    {
        public ChruchLifeDBContext():base("ChurchLifeDBContext")
        {

        }

        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<MeetingDate> MeetingDates { get; set; }
        public DbSet<vMeetingDate> vMeetingDates { get; set; }
        public DbSet<MeetingDateActivate> MeetingDateActivateList { get; set; }
        public DbSet<vMeetingDateActivate> vMeetingDateActivates { get; set; }
        public DbSet<MeetingDateMember> MeetingDateMembers { get; set; }
        public DbSet<vMeetingDateMember> vMeetingDateMembers { get; set; }
        public DbSet<vMeetingDateActivateMember> vMeetingDateActivateMembers { get; set; }
        public DbSet<MeetingDateMemberLog> MeetingDateMemberLogs { get; set; }
        public DbSet<vMeetingDateMemberLog> vMeetingDateMemberLogs { get; set; }
        public DbSet<vMeetingDateAttendStatus> vMeetingDateAttendStatus { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }

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
}