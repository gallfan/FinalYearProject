using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data;
using MechanicTracker.DAL;
using System.Data.Entity.Infrastructure;
using PagedList;
using System.Net;

namespace MechanicTracker.Controllers
{
    [Authorize(Roles = "Manager")]
    public class JobsController : Controller
    {
        private IMechanicTrackerRepository _repo;
        public JobsController(IMechanicTrackerRepository repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            return View(); 
        }

        //displays a list of the job. Filter can be changes between "all jobs", "unassigned jobs" and "completed jobs"
        public PartialViewResult JobDisplay(string filteredJobs, int? page)
        {
            ViewBag.filter = filteredJobs;
            page = page ?? 1;

            int pageSize = 7;

            var displayJobList = _repo.DisplayJob(filteredJobs);

            if (displayJobList != null)
            {
                return PartialView("_JobDisplay", displayJobList.ToPagedList((int)page, pageSize));
            }
            return null;

        }

        //show mechanics to assign to a job (but do not show mechanics who have already been assigned to that job
        public PartialViewResult DisplayAllMechanics(int? id, string filteredMechanics, int? page)
        {
            if (id != null)
            {
                ViewBag.JobId = id;
                ViewBag.FilteredMechanics = filteredMechanics;

                job j = _repo.GetJob(id);
                ViewBag.JobId = id;
                ViewBag.Reg = j.booking.car.Registration;

                page = page ?? 1;

                int pageSize = 5;

                var displayMechanics = _repo.DisplyMechanics(id, filteredMechanics);

                if (displayMechanics != null)
                {
                    return PartialView("DisplayAllMechanics", displayMechanics.ToPagedList((int)page, pageSize));
                }
                else
                {
                    RedirectToAction("Index");
                }
                return null;
            }
            else
            {
                return PartialView("NoPageFound");
            }
        }


        //Gets the time of the chosen job sothat itwill be displayed in the time allowed box while adding a mechanic to a job
        [HttpGet]
        public int GetTimeOfJob(int? id)
        {
            if (id != null || id !=null)
            {
                job j = _repo.GetJob(id);
                int time = _repo.CurrentTimeOfJob(j);
                return time;
            }
            else
            {
                return 0;
            }
        }

        //Assigns a mechanic to a job and adds the recommended amount of time it should take mechanic to complete the job
        [HttpPost]
        public ActionResult AssignMechanic(int? jobID,int? mechanicID, int? time)
        {
            if (time == null)
                time = 10;
            if (jobID != null && mechanicID != null && time!=null)
            {
                job incomingJob = _repo.GetJob(jobID);
                incomingJob.MechanicID = mechanicID;
                incomingJob.TimeAllowed = (short)time;

                if (ModelState.IsValid)
                {
                    try
                    {
                        _repo.AssigningMechanic(incomingJob);

                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        var entry = ex.Entries.Single();
                        var databaseValues = (job)entry.GetDatabaseValues().ToObject();
                        var clientValues = (job)entry.Entity;

                        if (databaseValues.MechanicID != null)
                        {
                            entry.Reload();
                            //ModelState.AddModelError("Message", "");
                            //ViewData.Add("ConcurrencyError", "A Mechanic has already been assigned to this job");
                            //ViewBag.Concurrency = string.Format("A Mechanic has already been assigned to this job");
                            return new HttpStatusCodeResult(500);
                        }
                    }
                }
                _repo.newNotification(incomingJob);
                return new HttpStatusCodeResult(200);
            }
            else 
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }

        //Adds Notification of a job that mechanics will later see on the phone
        [HttpPost]
        public HttpStatusCodeResult AddNotification(int id)
        {
            job j = _repo.GetJob(id);

            return _repo.newNotification(j);
     
        }

     
    }
}
