using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Common
{
    public class SessionHelper
    {
        public static int Id
        {
            get
            {
                return (int)(HttpContext.Current.Session["Id"] == null ? 0 : HttpContext.Current.Session["Id"]);
            }
            set
            {
                HttpContext.Current.Session["Id"] = value;
            }
        }

        public static string Username
        {
            get
            {
                return (string)(HttpContext.Current.Session["Username"] == null ? "" : HttpContext.Current.Session["Username"]);
            }
            set
            {
                HttpContext.Current.Session["Username"] = value;
            }
        }
    }
}