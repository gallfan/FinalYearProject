using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MechanicTracker.Models;
using MechanicTracker.DAL;
namespace MechanicTracker.Controllers
{
    [Authorize(Roles = "Mechanic")]
    public class PhoneMechanicController : Controller
    {
        //

         
         IMechanicTrackerRepository _repo;
         public PhoneMechanicController()
        {
            _repo = new MechanicTrackerRepository();
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MenuMobile()
        {
            return View("MenuMobile");
        }


    }
}
