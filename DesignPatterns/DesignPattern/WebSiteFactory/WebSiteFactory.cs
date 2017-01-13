using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace WebSiteFactory
{
    public class User
    {
        private string name = string.Empty;
        public User(string name)
        {
            this.name = name;
        }
        
        public string Name { get { return name; } }
    }

    abstract class WebSite
    {
        public abstract void User(User user);
    }

    class WebSiteFactory
    {
        private Hashtable flyweights = new Hashtable();

        public WebSiteFactory()
        {

        }

        // 獲得網站分類
        public WebSite GetWebSiteCategory(string key)
        {
            if (!flyweights.ContainsKey(key))
                flyweights.Add(key, new ConcreteWebSite(key));

            WebSite w = (WebSite)flyweights[key];
            return w;
        }

        //獲得網站分類總數
        public int GetWebSiteCount()
        {
            return flyweights.Count;
        }
    }
}
