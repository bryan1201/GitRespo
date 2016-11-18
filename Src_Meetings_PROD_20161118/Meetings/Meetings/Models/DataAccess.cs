using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Configuration;

namespace Meetings.Models
{
    public class DataAccess
    {
        private static readonly string AssemblyName = "Meetings"; // The string is the current namespace
        private static readonly string Meeting = "Meeting";
        private static readonly string MeetingDate = "MeetingDate";
        private static readonly string MeetingDateMember = "MeetingDateMember";
        private static readonly string MeetingDateActivate = "MeetingDateActivate";
        private static readonly string MeetingDateMemberLog = "MeetingDateMemberLog";
        private static readonly string UserProfile = "UserProfile";
        private static readonly string Models = "Models";
        private static readonly string db = "SqlServer";

        public static IMeeting CreateMeeting()
        {
            //Meetings.Models.SqlServerMeeting
            string className = string.Format("{0}.{1}.{2}{3}", AssemblyName, Models, db, Meeting);
            return (IMeeting)Assembly.Load(AssemblyName).CreateInstance(className);
        }

        public static IMeetingDate CreateMeetingDate()
        {
            //Meetings.Models.SqlServerMeeting
            string className = string.Format("{0}.{1}.{2}{3}", AssemblyName, Models, db, MeetingDate);
            return (IMeetingDate)Assembly.Load(AssemblyName).CreateInstance(className);
        }

        public static IMeetingDateMember CreateMeetingDateMember()
        {
            //Meetings.Models.SqlServerMeeting
            string className = string.Format("{0}.{1}.{2}{3}", AssemblyName, Models, db, MeetingDateMember);
            return (IMeetingDateMember)Assembly.Load(AssemblyName).CreateInstance(className);
        }

        public static IMeetingDateActivate CreateMeetingDateActivate()
        {
            //Meetings.Models.SqlServerMeeting
            string className = string.Format("{0}.{1}.{2}{3}", AssemblyName, Models, db, MeetingDateActivate);
            return (IMeetingDateActivate)Assembly.Load(AssemblyName).CreateInstance(className);
        }

        public static IMeetingDateMemberLog CreateMeetingDateMemberLog()
        {
            //Meetings.Models.SqlServerMeeting
            string className = string.Format("{0}.{1}.{2}{3}", AssemblyName, Models, db, MeetingDateMemberLog);
            return (IMeetingDateMemberLog)Assembly.Load(AssemblyName).CreateInstance(className);
        }

        public static IUserProfile CreateUserProfile()
        {
            //Meetings.Models.SqlServerUserProfile
            string className = string.Format("{0}.{1}.{2}{3}", AssemblyName, Models, db, UserProfile);
            return (IUserProfile)Assembly.Load(AssemblyName).CreateInstance(className);
        }
    }
}