using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Meetings.Models
{
    public interface IMeeting
    {
        void Insert(Meeting meeting);
        void Update(Meeting meeting);
        void Remove(Guid Id);
        IEnumerable<Meeting> Get();
        Meeting Get(Guid id);
    }

    public interface IMeetingDate
    {
        void Insert(MeetingDate meetingdate);
        void Update(MeetingDate meetingdate);
        void Remove(Guid Id);
        IEnumerable<vMeetingDate> Get(Guid meetingId);
        vMeetingDate GetCurrent(Guid meetingdateId);
    }

    public interface IMeetingDateActivate
    {
        void Insert(MeetingDateActivate mda);
        void Update(MeetingDateActivate mda);
        void Remove(Guid activateId);
        IEnumerable<vMeetingDateActivate> Get(Guid meetingDateId);
        vMeetingDateActivate GetCurrent(Guid activateId);
        MeetingDateActivate GetCurrentEdit(Guid activateId);
    }

    public interface IMeetingDateMember
    {
        void Insert(MeetingDateMember meetingdatemember);
        void Update(MeetingDateMember meetingdatemember);
        void Remove(Guid Id);
        IEnumerable<vMeetingDateMember> Get();
        vMeetingDateMember Get(Guid uniqueId);
        IEnumerable<vMeetingDateMember> GetMeetingDateMembers(Guid meetingDateId);
        MeetingDateMember GetCurrentEdit(Guid uniqueId);
    }

    public interface IMeetingDateMemberLog
    {
        void Insert(MeetingDateMemberLog meetingdatememberLog);
        void Update(MeetingDateMemberLog meetingdatememberLog);
        void Remove(Guid Id);
        IEnumerable<vMeetingDateMemberLog> Get(Guid activateId);
        IEnumerable<vMeetingDateAttendStatus> GetReport(Guid activateId);
        vMeetingDateMemberLog GetMeetingDateMemberLog(Guid logId);
        void Login(Guid activateId, string UserCode, out string RetRslt);

    }
}