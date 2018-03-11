

using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace FaceServer
{
    public class Class1
    {
        public static string CreateNewQueue()
        {
            string name = ".\\private$\\" + "test";
            if (MessageQueue.Exists(name))
            {
                return (name + "已经存在");
            }
            else
            {
                var mq = MessageQueue.Create(name);
                mq.Label = name;
                return (name + "创建成功");
            }
        }
    }
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Log.Info(Class1.CreateNewQueue());
           
        }
    }
}
