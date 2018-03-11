using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace FaceServer
{
   
    public class Logger
    {
        public static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Configure()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}