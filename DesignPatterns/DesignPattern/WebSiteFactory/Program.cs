using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSiteFactory
{
    class Program
    {
        static void Main(string[] args)
        {
            WebSiteFactory f = new WebSiteFactory();

            WebSite fx = f.GetWebSiteCategory("產品展示");
            fx.User(new User("小菜"));

            WebSite fy = f.GetWebSiteCategory("產品展示");
            fy.User(new User("大鳥"));

            WebSite fz = f.GetWebSiteCategory("產品展示");
            fz.User(new User("JoJo"));

            WebSite fa = f.GetWebSiteCategory("Blog");
            fa.User(new User("Happy Man"));

            WebSite fb = f.GetWebSiteCategory("Blog");
            fb.User(new User("Six Ghosts"));

            WebSite fc = f.GetWebSiteCategory("Blog");
            fc.User(new User("South Crocodile Alligator Lacoste"));

            Console.WriteLine("得到網站分類總數為：{0}", f.GetWebSiteCount());

            Console.Read();



        }
    }
}
