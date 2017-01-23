using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSiteFactory
{
    class ConcreteWebSite:WebSite
    {
        private string name = string.Empty;
        public ConcreteWebSite(string name)
        {
            this.name = name;
        }

        public override void User(User user)
        {
            try
            {
                string result = string.Format("網站分類：{0}\t用戶：{1}", name, user.Name);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}
