using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.Objects;
using PagedList;
using MechanicTracker.DAL;

namespace MechanicTracker.Controllers
{
    //Autorized only for use by the manager
    [Authorize(Roles = "Manager")]
    public class HomeController : Controller
    {
        private IMechanicTrackerRepository _repo;
        public HomeController(IMechanicTrackerRepository repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            return View();
        }

        //Retrieve all the recent bookings (in the last day)
        public PartialViewResult RecentBookings(int? page)
        {
            DateTime OneDayAgo = DateTime.Now.AddDays(-1); ;
            DateTime CurrentDate = DateTime.Now;
            List<booking> Bookings = _repo.GetRecentBookings(OneDayAgo, CurrentDate).ToList();

            //Using PagedList, If the page is null set it to 1
            var pageNumber = page ?? 1;
            int RecordsPerPage = 10;
            //Return the partial view with 10 bookings of the page requested
            return PartialView("_RecentBookings", Bookings.ToPagedList(pageNumber, RecordsPerPage));
        }

        //Get all jobs that are currently been worked on
        public PartialViewResult ActiveJobs(int? page)
        {
            List<job> Jobs = _repo.GetActiveJobs().ToList();
            int pageNumber = page ?? 1;
            int RecordsPerPage = 10;
            return PartialView("_ActiveJobs", Jobs.ToPagedList(pageNumber, RecordsPerPage));
        }

        //Retrieve jobs completed in the last day
        public PartialViewResult RecentCompletedJobs(string filter, int? page)
        {
            List<job> Jobs = _repo.GetRecentlyCompleteJobs(filter).ToList();
            int pageNumber = page ?? 1;
            int RecordsPerPage = 10;
            return PartialView("_RecentCompletedJobs", Jobs.ToPagedList(pageNumber, RecordsPerPage));
        }

    }//End Class
}//End Namespace
