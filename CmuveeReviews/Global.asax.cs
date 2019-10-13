using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CmuveeReviews
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            var path = System.AppDomain.CurrentDomain.BaseDirectory;
            X.ORMData.ORMBase.CreateDataSource(X.ORMData.DataSourceType.Sqlite, $"Data Source={path}\\bin\\movies.db");
        }
    }
}
