using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mobile_Deura.Untils
{
    public static class AccountUntils
    {
        public static User GetUser()
        {
            User user = new User();
            if (HttpContext.Current.Session["admin"] != null)
            {
                user = (User) HttpContext.Current.Session["admin"];
            }
            
            return user;
        }
        public static User GetEmp()
        {

            User user = new User();
//            if (HttpContext.Current.Session["Emp"] != null)
//            {
//                user = (User)HttpContext.Current.Session["Emp"];
//                return user;
//            }
                if (HttpContext.Current.Request.Cookies["id"] != null)
                {
                    user.Id = Int32.Parse(Cookies_Get("id"));
                    user.FullName = Cookies_Get("FullName");
                    user.UserName = Cookies_Get("UserName");
                    user.agent = Cookies_Get("agent");
                  //  user.LoaiTK = Int32.Parse(Cookies_Get("LoaiTK"));
                    return user;
                }
            return null;

        }

        public static string Cookies_Get(string key)
        {
            string value = "";
            if (HttpContext.Current.Request.Cookies.AllKeys.Contains(key))
            {
                value = HttpContext.Current.Request.Cookies[key].Value;
            }

            return HttpUtility.UrlDecode(value);
        }
    }
    
}