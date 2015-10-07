using MechanicTracker.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MechanicTracker.Controllers
{
    public class MechanicJobsController : Controller
    {
        private IMechanicTrackerRepository _repo;
        public MechanicJobsController(IMechanicTrackerRepository repo)
        {
            _repo = repo;
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SelectNotifications()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userName = HttpContext.User.Identity.Name;
                var id = _repo.getUserID(userName);
                if (id != null)
                {
                    ViewBag.Count = _repo.CountNotications(id);
                    return View();
                }
                else
                {

                    return View("notFound");


                }
            }
            else 
            {

                return View("notFound");
            }
            

        }



        public ActionResult Notifications(bool? read)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userName = HttpContext.User.Identity.Name;
                var id = _repo.getUserID(userName);
                if (id != null)
                {

                    var ntfs = _repo.GetNotications(id, read).ToList();
                    return View(ntfs);
                }
                else
                {

                    return View("notFound");

                }
            }
            else 
            {
                return View("notFound");
            }
        }

        public ActionResult UpdateNotification(int id)
        {
            if (id != null)
            {
               return _repo.UpdateNotification(id);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
                
               
        }
        

        public ActionResult DisplayJobs(int? id)   
        {
            if (id != null)
            {
                var userName = HttpContext.User.Identity.Name;
                var userid = _repo.getUserID(userName);
                var joblst = _repo.GetMechanicsJobs(id, userid);
                return View(joblst);
            }
            else
            {

                return View("notFound");

            }
        }

        public ActionResult EnterReg()
        {
            return View();
        }


        //public PartialViewResult SingleNotification(int? id)
        //{
        //    if (id != null)
        //    {
        //        var n = _repo.getNotification(id);
        //        return PartialView("_SingleNotification", n);
        //    }
        //    else
        //    {
        //        return PartialView("_Error");
        //    }
        //}

        public ActionResult SearchDisplay(string carReg)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (carReg != null)
                {
                    var userName = HttpContext.User.Identity.Name;
                    var userid = _repo.getUserID(userName);
                    var jobs = _repo.searchedJobs(carReg, userid);
                    return View(jobs);
                }
                else
                {

                    return View("notFound");

                }
            }
            else
            {

                return View("notFound");

            }
        }


        public ActionResult ActiveJobs()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userName = HttpContext.User.Identity.Name;
                var id = _repo.getUserID(userName);
                var jobs = _repo.GetMechanicsActiveJobs(id);
                return View(jobs);
            }
            else
            {
                return View("notFound");
            }
        }

        //Loads page where mechanic can start, stop and pause a job the is matching the incoming JobId.  
        public ActionResult JobMenu(int? id)
        {
            if (id != null)
            {

                job j = _repo.GetJob(id);
                if (j.Status == "active")
                {
                    timessheet incomingTimesheet = _repo.FindCurrentTimeSheet(j);

                    ViewBag.TimeSheetId = incomingTimesheet.TimeSheetID;
                    ViewBag.Time = (incomingTimesheet.StartTime).ToString("HH:mm");
                    ViewBag.JobName = j.jobslist.Name;
                }

                return View(j);
            }
            else
            {
                return View("notFound");
            }
        }

        //Posting the start time for a job with incoming JobId. Called when the Start Job button.
        public int PostStartTime(int? id)
        {
            if (id == null)
            {
                return 0;
            }
            int a = _repo.TimeSheetStartTime(id);
                return a;
        }

        //Posting the end time for a job with incoming JobId. Called when the Complete Job and Pause Job button.
        public HttpStatusCodeResult  EndStartTime(int? id, int? jobId, string status)
        {
            if (id != null && jobId != null)
            {
                 var httpStatus =_repo.TimeSheetEndTime(id, jobId, status);
                 return httpStatus;
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
           
        }

        //Loads the partial view page where a mechanic can leave a comment on the job they have just completed.
        public PartialViewResult CommentPage(int? id)       
        {
            if (id != null)
            {
                var jobIncoming = _repo.GetJob(id);
                return PartialView("_CommentPage", jobIncoming);
         
            }
            else
            {
                return PartialView("NoPageFound");
            }
        }

        //Posts the comment the a mechanic makes about the job.
        [HttpPost]  
        public ActionResult LeaveComment(int? id, string comment)
        {
            if (id != null)
            {
                job j = _repo.GetJob(id);
                j.Comments = comment;
                _repo.UserComment(j);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
                //RedirectToAction("Index", "MechanicJobs");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

    }
}
