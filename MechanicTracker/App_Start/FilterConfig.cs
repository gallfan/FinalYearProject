using MechanicTracker.Models;
using System.Web;
using System.Web.Mvc;

namespace MechanicTracker
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorAttribute());
        }
    }
}