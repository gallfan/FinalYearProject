using MechanicTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MechanicTracker.Controllers
{
    public class ErrorController : Controller
    {
        //Handles 404 Errors
        public ActionResult NoPageFound(string url)
        {
            //Gets the requested url from the errror query string
            var requested = url ?? Request.QueryString["aspxerrorpath"] ?? Request.Url.OriginalString;

            var controllerName = (string)RouteData.Values["controller"];
            var actionName = (string)RouteData.Values["action"];
            //Creates a new NotFoundModel
            var model = new NotFoundModel(new HttpException(404, "Failed to find page"), controllerName, actionName)
            {
                RequestedUrl = requested
            };

            Response.StatusCode = 404;
            return View("NoPageFound", model);
        }

    }//End Class
}//End Namespace
