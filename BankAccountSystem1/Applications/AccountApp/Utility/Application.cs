using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AccountApp.Utility
{
    public class Application
    {
        private static int _APPLICATION_ID
        {
            get
            {
                string value = ConfigurationManager.AppSettings["APPLICATION_ID"];
                int id;

                int.TryParse(value, out id);
                return id;
            }
        }

        public static readonly int APPLICATION_ID = _APPLICATION_ID;
        public static readonly string SESSION_LOGIN_KEY = "login";
    }
}