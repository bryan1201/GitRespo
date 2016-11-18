using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Meetings.Models;

namespace Meetings.Models
{
    public interface IUserProfile
    {
        void Insert(UserProfile up);
        void Remove(Guid id);
        void Update(UserProfile up);
        IEnumerable<UserProfile> Get();
        UserProfile Get(string userinfo);
        UserProfile GetProfile(string userinfo);
        IEnumerable<UserProfile> Search(string condition);
    }
}
