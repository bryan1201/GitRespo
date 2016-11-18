using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Meetings.Models;
using Meetings.Controllers;

namespace Meetings.Models
{
    public class DBServers
    {
    }

    // SqlServer
    class SqlServerMeeting : IMeeting
    {
        Meeting m = new Meeting();
        public void Insert(Meeting meeting)
        {
            m.Insert(meeting);
        }

        public void Update(Meeting meeting)
        {
            m.Update(meeting);
        }

        public IEnumerable<Meeting> Get()
        {
            return m.Get();
        }

        public Meeting Get(Guid Id)
        {
            return m.Get(Id);
        }
        public void Remove(Guid id)
        {
            m.Remove(id);
        }
    }

    class SqlServerMeetingDate : IMeetingDate
    {
        MeetingDate m = new MeetingDate();
        public void Insert(MeetingDate md)
        {
            m.Insert(md);
        }

        public void Update(MeetingDate meetingdate)
        {
            m.Update(meetingdate);
        }

        public IEnumerable<vMeetingDate> Get(Guid meetingId)
        {
            return m.Get(meetingId);
        }

        public vMeetingDate GetCurrent(Guid meetingdateId)
        {
            return m.GetCurrent(meetingdateId);
        }

        public void Remove(Guid id)
        {
            m.Remove(id);
        }
    }

    class SqlServerMeetingDateActivate : IMeetingDateActivate
    {
        MeetingDateActivate m = new MeetingDateActivate();
        public void Insert(MeetingDateActivate mda)
        {
            m.Insert(mda);
        }

        public void Update(MeetingDateActivate mda)
        {
            m.Update(mda);
        }

        public IEnumerable<vMeetingDateActivate> Get(Guid meetingDateId)
        {
            return m.Get(meetingDateId);
        }

        public vMeetingDateActivate GetCurrent(Guid activateId)
        {
            return m.GetCurrent(activateId);
        }

        public MeetingDateActivate GetCurrentEdit(Guid activateId)
        {
            return m.GetCurrentEdit(activateId);
        }

        public void Remove(Guid id)
        {
            m.Remove(id);
        }
    }

    class SqlServerMeetingDateMember : IMeetingDateMember
    {
        MeetingDateMember m = new MeetingDateMember();
        public void Insert(MeetingDateMember mda)
        {
            m.Insert(mda);
        }

        public void Update(MeetingDateMember mdm)
        {
            m.Update(mdm);
        }

        public IEnumerable<vMeetingDateMember> Get()
        {
            return m.Get();
        }

        public IEnumerable<vMeetingDateMember> GetMeetingDateMembers(Guid meetingdateId)
        {
            return m.GetMeetingDateMembers(meetingdateId);
        }

        public vMeetingDateMember Get(Guid uniqueId)
        {
            return m.Get(uniqueId);
        }

        public MeetingDateMember GetCurrentEdit(Guid uniqueId)
        {
            return m.GetCurrentEdit(uniqueId);
        }

        public void Remove(Guid uniqueId)
        {
            m.Remove(uniqueId);
        }
    }

    class SqlServerMeetingDateMemberLog : IMeetingDateMemberLog
    {
        MeetingDateMemberLog m = new MeetingDateMemberLog();

        public void BulkInsertLog(Guid activateid, string UserCodes, out string message)
        {
            m.BulkInsertLog(activateid, UserCodes, out message);
        }

        public void Insert(MeetingDateMemberLog mdml)
        {
            m.Insert(mdml);
        }

        public void Update(MeetingDateMemberLog mdml)
        {
            m.Update(mdml);
        }

        public IEnumerable<vMeetingDateMemberLog> Get(Guid activateId)
        {
            return m.Get(activateId);
        }

        public IEnumerable<vMeetingDateAttendStatus> GetReport(Guid activateId)
        {
            return m.GetReport(activateId);
        }

        public vMeetingDateMemberLog GetMeetingDateMemberLog(Guid logId)
        {
            return m.GetMeetingDateMemberLog(logId);
        }

        public void Remove(Guid logId)
        {
            m.Remove(logId);
        }

        public void Login(Guid activateId, string UserCode, out string RetRslt)
        {
            m.Login(activateId, UserCode, out RetRslt);
        }
    }

    class SqlServerUserProfile : IUserProfile
    {
        UserProfile m = new UserProfile();
        public void Insert(UserProfile up)
        {
            up.uniqueId = (up.uniqueId == null) ? Guid.NewGuid() : up.uniqueId;
            up.cdt = DateTime.Now;
            up.udt = DateTime.Now;
            up.Enable = true;
            m.Insert(up);
        }

        public void Update(UserProfile up)
        {
            m.Update(up);
        }

        public IEnumerable<UserProfile> Get()
        {
            return m.Get();
        }

        public IEnumerable<UserProfile> Search(string condition)
        {
            return m.Search(condition);
        }

        public UserProfile Get(string userinfo)
        {
            return m.Get(userinfo);
        }

        public UserProfile GetProfile(string userinfo)
        {
            return m.GetProfile(userinfo);
        }

        public void Remove(Guid id)
        {
            m.Remove(id);
        }
    }
}