using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Web;
using TechSDK;
using TechSDK.Auth;

namespace Mobile_Deura
{
    public class SmsConfigs
    {
        public static ClientCredentials getTechAuthorization()
        {
            Constant.configs(new Dictionary<string, object>
            {
                {"mode", Constant.MODE_LIVE},
                {"connect_timeout", 15 },
                {"enable_cache", true},
                {"enable_log", true},
                {"log_path",  HttpContext.Current.Server.MapPath("/App_Code/logs/")}
            });

            //  Client client = new Client("client_id", "client_secret", new string[] { "send_brandname", "send_brandname_otp" });

            Client client = new Client(System.Configuration.ConfigurationManager.AppSettings["ClientID"], System.Configuration.ConfigurationManager.AppSettings["Secret"],
                new string[] { "send_brandname_otp" });

            return new ClientCredentials(client);
        }

    }
}