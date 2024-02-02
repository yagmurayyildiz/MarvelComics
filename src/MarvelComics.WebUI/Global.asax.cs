using MarvelComics.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MarvelComics.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutofacConfig.RegisterComponents();
        }

        /// <summary>
        /// This method provides basic global error handling, and logging logic.
        /// Unhandled exceptions will be caught by this override method and will be logged.
        /// If anything goes wrong in this method customs error in web.config will 
        /// redirect to the related error page.
        /// It will redirect NotFound errors to a specific page.
        /// All other errors will be redirected to the general error page.
        /// 
        /// TODO:
        /// Request can be transfer to custom error handler and log operations can be handle
        /// by custom error handler.
        /// 
        /// More specific error details can be provided by creating custom error pages and
        /// handling more status code.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            var logger = log4net.LogManager.GetLogger(typeof(MvcApplication));
            logger.Error("Unhandled exception", exception);

            var httpException = exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
                Response.Redirect("~/Error/NotFound");
            else
                Response.Redirect("~/Error/Index");

            Server.ClearError();
        }
    }
}
