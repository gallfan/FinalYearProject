using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MechanicTracker.Models
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        //Handles Exceptions in the Code
        public override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            var model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");

            //var directory = Path.GetFullPath("Log/ExceptionLog.txt");
            //StreamWriter LogWriter = new StreamWriter(directory, true);
            //LogWriter.WriteLine("{0}, {1}, {2}, {3}, {4}", DateTime.Now, filterContext.Exception.Message, filterContext.Exception.InnerException, filterContext.RouteData.Values["controller"], filterContext.RouteData.Values["action"]);
            //LogWriter.WriteLine("-----------------------------------------------------------------------------");
            //LogWriter.Close();

            if (filterContext.HttpContext.IsCustomErrorEnabled)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    //if the request is an ajax request return the _Error view
                    filterContext.Result = new ViewResult()
                    {
                        ViewName = "_Error",
                        ViewData = new ViewDataDictionary(model)
                    };
                }
                else
                {
                    //if the request is not an ajax request return the Error view
                    filterContext.Result = new ViewResult()
                    {
                        ViewName = "Error",
                        ViewData = new ViewDataDictionary(model)
                    };
                }
            }
        }
    }

    //Model that includes the Requested url on top of the usual HandleErrorInfo
    public class NotFoundModel : HandleErrorInfo
    {
        public NotFoundModel(Exception exception, string controllerName, string actionName)
            : base(exception, controllerName, actionName)
        {
        }
        public string RequestedUrl { get; set; }
    }
}