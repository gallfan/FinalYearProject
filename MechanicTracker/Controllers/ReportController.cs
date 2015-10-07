using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using MechanicTracker.DAL;
using PagedList;

namespace MechanicTracker.Controllers
{
    //Autorized only for use by the manager
    [Authorize(Roles = "Manager")]
    public class ReportController : Controller
    {
        private IMechanicTrackerRepository repo;

        public ReportController(IMechanicTrackerRepository rep)
        {
            repo = rep;
        }

        //=============//
        //MECHANIC JOBS//
        //=============//
        public ActionResult MechanicJobs(string Filter, int? MechanicId)
        {
            ViewBag.MechanicsList = repo.GetMechanicsForDropdown().ToList();
            return View();
        }

        //Gets a list of jobs belonging to the mechanic matching the 
        //mechanic id and filtered by overtime, on time and acceptable time
        public ActionResult GetJobList(int? MechanicId, string filter, int? page)
        {
            if (MechanicId == null)
            {
                return HttpNotFound();
            }
            int pageNumber = page ?? 1;
            int RecordsPerPage = 10;
            var jobs = repo.GetMechanicsCompleteJobs((int)MechanicId);
            if (jobs != null)
            {
                jobs = repo.FilterCompleteJobs(filter, jobs);
            }
            return PartialView("_JobList", jobs.ToPagedList(pageNumber, RecordsPerPage));
        }

        //Gets the details of a job
        public ActionResult GetJobDetails(int? JobId)
        {
            if (JobId == null)
            {
                return HttpNotFound();
            }
            var jobDetail = repo.JobDetailsOfId((int)JobId);
            return PartialView("_JobDetails", jobDetail);
        }

        //===============//
        //OVER TIME RATIO//
        //===============//

        //Shows the mechanics with overtime jobs and how many they have
        public ActionResult Rankings()
        {
            ViewBag.OverTimeJobs = repo.OverTimeJobsCount();
            var Rankings = repo.Rankings();
            return View("Overtime", Rankings);
        }

        //==============//
        //MECHANIC STATS//
        //==============//

        public ActionResult MechanicStats()
        {
            ViewBag.MechanicsList = repo.GetMechanicsForDropdown().ToList();
            return View();
        }

        //Retrive the Mechanics Details for the Mechanic Stats page and Mechanic Jobs Page
        public ActionResult MechanicDetails(int? MechanicId)
        {
            if (MechanicId == null)
            {
                return HttpNotFound();
            }
            mechanic mechanic = repo.GetMechanic((int)MechanicId);
            if (mechanic != null)
            {
                //Get all complete jobs for mechanics
                var jobs = repo.GetMechanicsCompleteJobs((int)MechanicId);
                //get all complete jobs for mechanic
                ViewBag.NumberofJobsComplete = jobs.Count();
                //get all complete jobs complete on time
                ViewBag.NumberofJobsCompleteOnTime = repo.FilterCompleteJobs("2", jobs).Count();
            }
            return PartialView("_MechanicDetails", mechanic);
        }

        public ActionResult MechanicJobsMonth(int? Id)
        {
            if (Id == null)
            {
                return HttpNotFound();
            }
            DateTime MinDate = DateTime.Now.AddMonths(-12);
            var jobs = repo.GetJobsCompleteInPrevious12Months((int)Id, MinDate);
            List<object> ChartData = new List<object>();
            if (jobs != null)
            {
                //every month retrieve the number of jobs that were completed that month
                for (int i = 0; i < 12; i++)
                {
                    var CurrentDate = DateTime.Now.AddMonths(-12 + (i + 1));
                    var num = jobs.Where(jb => jb.CompleteDate.Value.Month == CurrentDate.Month).Count();
                    ChartData.Add(new { Month = CurrentDate.ToString("MMM"), Count = num });
                }
            }
            return Json(ChartData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MechanicTimeStats(int? Id)
        {
            if (Id == null)
            {
                return HttpNotFound();
            }
            List<int> ChartData = new List<int>();
            var jobs = repo.GetMechanicsCompleteJobs((int)Id);
            if (jobs != null)
            {
                //count of jobs done on time
                ChartData.Add(jobs.Where(jb => jb.TimeTaken <= jb.TimeAllowed).Count());
                //count of jobs done in acceptable time
                ChartData.Add(jobs.Where(jb => jb.TimeTaken > jb.TimeAllowed && jb.TimeTaken <= Math.Round((double)jb.TimeAllowed * 1.1)).Count());
                //count of jobs done over time
                ChartData.Add(jobs.Where(jb => jb.TimeTaken > Math.Round((double)jb.TimeAllowed * 1.1)).Count());
            }
            return Json(ChartData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MechanicCategoryJobs(int? Id)
        {
            if (Id == null)
            {
                return HttpNotFound();
            }
            var JobCategories = repo.GetJobsCategories().ToList();
            List<object> ChartData = new List<object>();

            if (JobCategories.Any())
            {
                foreach (jobscategory jobcat in JobCategories)
                {
                    var jobs = repo.GetJobsInEachCategory(jobcat.JobsCategoryID, (int)Id);
                    int[] stats = new int[4];
                    var completejobs = jobs.Where(jb => jb.Status == "complete");
                    //complete jobs completed on time
                    stats[0] = repo.FilterCompleteJobs("2", completejobs).Count();
                    //complete jobs completed in acceptable time
                    stats[1] = repo.FilterCompleteJobs("3", completejobs).Count();
                    //complete jobs completed over time
                    stats[2] = repo.FilterCompleteJobs("4", completejobs).Count();
                    //not complete jobs
                    stats[3] = jobs.Where(jb => jb.Status != "complete").Count();
                    //object containing the job category type and count of jobs
                    var anonObject = new { Group = jobcat.Type.Substring(0, 1).ToUpper() + jobcat.Type.Substring(1), Count = stats };
                    ChartData.Add(anonObject);
                }
            }
            return Json(ChartData, JsonRequestBehavior.AllowGet);
        }

        //==============//
        //CATEGORY STATS//
        //==============//

        public ActionResult JobCategoryStats()
        {
            //Used for the dropdown of categories
            ViewBag.CategoryList = repo.GetJobsCategories()
                .Select(jc => new { ID = jc.JobsCategoryID, Type = jc.Type.Substring(0, 1).ToUpper() + jc.Type.Substring(1) })
                .ToList();

            return View();
        }

        //Gets the details of a specific Job Category matching the id
        public ActionResult JobCategoryDetails(int? Id)
        {
            if (Id == null)
            {
                return HttpNotFound();
            }
            jobscategory jobcategory = repo.GetCategory((int)Id);
            if (jobcategory != null)
            {
                var jobs = repo.GetCategoryJobs((int)Id);
                ViewBag.JobsCount = jobs.Count();
                ViewBag.JobCountOnTime = repo.FilterCompleteJobs("2", jobs).Count();
            }
            return PartialView("_JobCategoryDetails", jobcategory);
        }

        //Return what jobs were finished on time, just over time and over time
        public ActionResult CategoryTimeStats(int? Id)
        {
            if (Id == null)
            {
                return HttpNotFound();
            }
            List<int> CategoryPie = new List<int>();
            var jobs = repo.GetCategoryCompleteJobs((int)Id);
            if (jobs != null)
            {
                //Adds the count of jobs on time, over time and acceptable time
                CategoryPie.Add(jobs.Where(jb => jb.TimeTaken <= jb.TimeAllowed).Count());
                CategoryPie.Add(jobs.Where(jb => jb.TimeTaken > jb.TimeAllowed && jb.TimeTaken <= Math.Round((double)jb.TimeAllowed * 1.1)).Count());
                CategoryPie.Add(jobs.Where(jb => jb.TimeTaken > jb.TimeAllowed * 1.1).Count());
            }
            return Json(CategoryPie, JsonRequestBehavior.AllowGet);
        }

        //Number of jobs for each JobList in the Categories
        public ActionResult CategoryJobListJobs(int? Id)
        {
            if (Id == null)
            {
                return HttpNotFound();
            }
            var JobCategoriesJoblist = repo.GetCategoryJobsList((int)Id).ToList();
            List<object> ChartData = new List<object>();
            List<int> Counts = new List<int>();
            if (JobCategoriesJoblist.Any())
            {
                foreach (jobslist joblst in JobCategoriesJoblist)
                {
                    var jobs = repo.GetJobsForCategoryJobsList(joblst.JobsListID);
                    //count of complete jobs
                    Counts.Add(jobs.Where(jb => jb.Status == "complete").Count());
                    //count of incomplete jobs
                    Counts.Add(jobs.Where(jb => jb.Status != "complete").Count());
                    //Anonymous Object holding the Joblist Name and Count of Jobs (Complete and Incomplete)
                    var anonObject = new { Group = joblst.Name.Substring(0, 1).ToUpper() + joblst.Name.Substring(1), Count = Counts };
                    ChartData.Add(anonObject);
                }
            }
            return Json(ChartData, JsonRequestBehavior.AllowGet);
        }

        //Return number of jobs in category for each month in last year
        public ActionResult CategoryJobsMonth(int? Id)
        {
            if (Id == null)
            {
                return HttpNotFound();
            }
            DateTime MinDate = DateTime.Now.AddMonths(-12);
            //Get all the jobs from the last year for the category and that have been complete
            var jobs = repo.GetCategoryJobsCompleteInPrevious12Months((int)Id, MinDate);
            List<object> ChartData = new List<object>();
            //For loop for each month in the last 12 months
            if (jobs != null)
            {
                for (int i = 0; i < 12; i++)
                {
                    //Used to get the date of each month 
                    var CurrentDate = DateTime.Now.AddMonths(-12 + (i + 1));
                    var num = jobs.Where(jb => jb.CompleteDate.Value.Month == CurrentDate.Month).Count();
                    //add a new anonymous object containing the month of the current month and number of jobs completed
                    ChartData.Add(new { Month = CurrentDate.ToString("MMM"), Count = num });
                }
            }
            return Json(ChartData, JsonRequestBehavior.AllowGet);
        }

    }//End Class
}//End Namespace
