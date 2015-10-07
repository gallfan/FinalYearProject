using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity.Infrastructure;

namespace MechanicTracker.DAL
{
    public class MechanicTrackerRepository : IMechanicTrackerRepository
    {
        private MTEntities db;

        public MechanicTrackerRepository()
        {
            db = new MTEntities();
        }

        //==============//
        //HomeController//
        //==============//

        //Return bookings that have been been added in the last day ordred by the booking date
        public IQueryable<booking> GetRecentBookings(DateTime date, DateTime CurrentDate)
        {
            return db.bookings.Include(book => book.car)
                            .Include(book => book.car.model)
                            .Include(book => book.car.model.make)
                            .Where(book => book.BookingDate <= CurrentDate && book.BookingDate > date)
                            .OrderByDescending(book => book.BookingDate);
        }

        //Returns jobs that are been actively worked on
        public IQueryable<job> GetActiveJobs()
        {
            return db.jobs.Include(jb => jb.jobslist)
                        .Include(jb => jb.jobslist.jobscategory)
                        .Where(jb => jb.Status == "active")
                        .OrderBy(jb => jb.TimeStarted);
        }

        //Returns Jobs that have been completed in the last day filtered by all, over time and on time.
        public IQueryable<job> GetRecentlyCompleteJobs(string filter)
        {
            IQueryable<job> jobs = db.jobs.Include(jb => jb.jobslist)
                        .Include(jb => jb.jobslist.jobscategory)
                        .Include(jb => jb.mechanic)
                        .Where(jb => jb.Status == "complete");

            DateTime FilterDate = DateTime.Now.AddDays(-1);
            switch (filter)
            {
                case "All":
                    jobs = jobs.Where(jb => jb.CompleteDate >= FilterDate);
                    break;
                case "OnTime":
                    jobs = jobs.Where(jb => jb.TimeTaken <= Math.Round((double)jb.TimeAllowed * 1.1) && jb.CompleteDate >= FilterDate);
                    break;
                case "OverRun":
                    jobs = jobs.Where(jb => jb.TimeTaken > Math.Round((double)jb.TimeAllowed * 1.1) && jb.CompleteDate >= FilterDate);
                    break;
                default:
                    jobs = jobs.Where(jb => jb.CompleteDate >= FilterDate);
                    break;
            }
            return jobs.OrderByDescending(jb => jb.CompleteDate); ;
        }

        //======================//
        //MechanicJobsController//
        //======================//

        public IEnumerable<job> GetMechanicsActiveJobs(int id)
        {
            var job = db.jobs.Include(x => x.jobslist).Where(x => x.MechanicID == id && x.Status == "active").ToList();
            return job;

        }

        public List<notification> GetNotications(int? id, bool? read)
        {

            return db.notifications.Where(x => x.MechanicID == id && x.Read == read).ToList();
        }

        public notification getNotification(int? id)
        {
            return db.notifications.Where(x => x.NotificationID == id).SingleOrDefault();
        }

        public int CountNotications(int id)
        {
            return db.notifications.Where(x => x.MechanicID == id && x.Read == false).Count();
        }

        public HttpStatusCodeResult UpdateNotification(int id)
        {
            var Notification = db.notifications.Where(x => x.NotificationID == id).Single();
            Notification.Read = true;
            db.SaveChanges();
            return new HttpStatusCodeResult(200);
        }

        public IEnumerable<job> searchedJobs(string reg, int id)
        {
            var jobs = db.jobs.Include(x => x.jobslist)
                         .Where(x => x.MechanicID == id && x.booking.car.Registration == reg && x.Status == "new").ToList();
            return jobs;
        }
        public int getUserID(string username)
        {

            return db.mechanics.Where(x => x.Username == username).Select(y => y.MechanicID).SingleOrDefault();

        }

        HttpStatusCodeResult StartJob(timessheet t)
        {
            db.timessheets.Add(t);
            db.SaveChanges();
            return new HttpStatusCodeResult(200);
        }

        public IEnumerable<job> GetJobs()
        {
            return db.jobs;
        }

        //////=====================/////////
        ////////Mechanics Controller////////
        //////=====================/////////

        // Get list of mechanics
        public List<mechanic> GetMechanics()
        {
            var mechs = db.mechanics
                          .OrderBy(x => x.Firstname)
                          .ToList();
            return mechs;
        }

        //Get mechanic with incoming id
        public mechanic GetmechDetails(int? id)
        {
            var mech = db.mechanics
                             .Where(x => x.MechanicID == id)
                             .SingleOrDefault(x => x.MechanicID == id);
            return mech;
        }

        //Create a new mechanic
        public HttpStatusCodeResult CreateMechanic(mechanic m)
        {

            db.mechanics.Add(m);
            db.SaveChanges();
            return new HttpStatusCodeResult(200);

        }

        //Edit details of mechanic
        public HttpStatusCodeResult EditMechanic(mechanic m)
        {
            db.Entry(m).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            return new HttpStatusCodeResult(200);
        }

        //////=====================/////////
        ////////Bookings Results////////
        //////=====================/////////

        public bool checkUsername(string username)
        {
            return db.mechanics.Any(x => x.Username == username);
        }

        public List<booking> GetBookingsList()
        {
            var Bookings = db.bookings.Include(book => book.car)
                                      .Include(cm => cm.car.model)
                                      .Include(c => c.car.model.make)
                                      .Where(x => x.Status != "complete")
                                      .OrderByDescending(x => x.BookingDate)
                                      .ToList();
            return Bookings;


        }

        public int countJobs(int? id)
        {
            return db.jobs.Where(x => x.BookingID == id).Count();
        }

        public int countCompleteJobs(int? id)
        {
            return db.jobs.Where(x => x.BookingID == id && x.Status == "complete").Count();
        }

        public booking GetBookingDetails(int? id)
        {
            var book = db.bookings.Include(bk => bk.car)
                                  .Include(j => j.jobs)
                                  .SingleOrDefault(x => x.BookingID == id);
            return book;
        }

        public List<job> GetJobsForBooking(int? id)
        {
            var jobs = db.jobs.Include(x => x.jobslist)
                              .Where(y => y.BookingID == id)
                              .ToList();

            return jobs;
        }
        public job GetJobsForMech(int id)
        {
            var jobs = db.jobs.Include(x => x.jobslist)
                              .SingleOrDefault(x => x.JobID == id);
            return jobs;
        }

        public List<jobscategory> GetListOfJobCatergories()
        {
            var cats = db.jobscategories.ToList();
            return cats;
        }

        public List<jobslist> GetListOfJobLists(int? id)
        {
            var joblist = db.jobslists.Where(x => x.JobCategoryID == id)
                            .ToList();
            return joblist;
        }

        public HttpStatusCodeResult CreateJob(job j)
        {
            var modelid = (from a in db.cars
                           join b in db.bookings
                           on a.CarID equals b.CarID
                           where b.BookingID == j.BookingID
                           select a.ModelID).SingleOrDefault();

            j.Difficulty = (from x in db.model_jobslist
                            where x.ModelID == modelid && x.JobslistID == j.JobsListID
                            select x.Difficulty).SingleOrDefault();
            j.TimeAllowed = (from x in db.model_jobslist
                             where x.ModelID == modelid && x.JobslistID == j.JobsListID
                             select x.ApproxTime).SingleOrDefault();

            j.Status = "new";
            db.jobs.Add(j);
            db.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        //JobsController - JobDisplay
        public IQueryable<job> DisplayJob(string filteredJobs)
        {
            if (filteredJobs == null)
            {
                filteredJobs = "allJobs";
            }

            IQueryable<job> jobs = db.jobs;

            switch (filteredJobs)
            {
                case "allJobs":
                    jobs = jobs.Include("jobslist").Include(j => j.booking.car);
                    break;

                case "unassignedJobs":
                    //jobs = jobs.Where(jbs => jbs.Status == "new").Include("jobslist");
                    jobs = jobs.Where(j => j.MechanicID == null && j.Status == "new").Include("jobslist").Include(j => j.booking.car);
                    break;

                case "completed":
                    jobs = jobs.Where(jbs => jbs.Status == "complete").Include("jobslist").Include(j => j.booking.car);
                    break;

                default:
                    jobs = jobs.Include("jobslist").Include(j => j.booking.car);
                    break;
            }

            return jobs.OrderByDescending(jb => new { jb.booking.BookingDate, jb.JobID });
        }

        ////JobsController - DisplayAllMechanics
        public List<mechanic> DisplyMechanics(int? id, string filteredMechanics)    //displys all mechanics on the Jobs contoller and filter for mechanics is changed
        {
            IQueryable<mechanic> mechanics = db.mechanics;
            IQueryable<job> jobmechanics = db.jobs;

            //ViewBag.JobId = id;


            if (filteredMechanics == null || filteredMechanics == "")
            {
                filteredMechanics = "allMechanics";
            }

            List<mechanic> mechanicList = db.mechanics.ToList();

            List<job> listOfMechanicIds = db.jobs.Where(j => j.JobID == id).ToList();   //list of jobs 


            foreach (job i in listOfMechanicIds)
            {
                mechanic me = db.mechanics.SingleOrDefault(mc => mc.MechanicID == i.MechanicID);    //removes all mechanics who are already assigned to the job
                mechanicList.Remove(me);
            }


            if (filteredMechanics == "available")
            {
                mechanicList = mechanicList.Where(mc => mc.Available == true).ToList();
            }

            return mechanicList;
        }

        //JobsController - CreateJobMechanic(job jobMechanic)

        public HttpStatusCodeResult AssigningMechanic(job incomingJob)
        {
            db.Entry(incomingJob).State = System.Data.EntityState.Modified;
            db.SaveChanges();


            return new HttpStatusCodeResult(200);
        }

        public HttpStatusCodeResult newNotification(job j)  //creates a new notification when mechanic is added to job
        {
            notification newNotification = new notification();
            newNotification.MechanicID = (int)j.MechanicID;
            newNotification.Read = false;

            newNotification.Message = "'" + UppercaseFirst(j.jobslist.Name) + "' on Car: "
                + UppercaseFirst(j.booking.car.model.make.MakeName) + " "
                + UppercaseFirst(j.booking.car.model.ModelName) + " with Reg "
                + j.booking.car.Registration;
            newNotification.Date = DateTime.Now;
            newNotification.MechanicID = (int)j.MechanicID;
            newNotification.JobID = j.JobID;
            db.notifications.Add(newNotification);
            db.SaveChanges();
            return new HttpStatusCodeResult(200);
        }

        public string UppercaseFirst(string incomingWord)//Alan Lawless
        {
            if (string.IsNullOrEmpty(incomingWord))
            {
                return string.Empty;
            }
            return char.ToUpper(incomingWord[0]) + incomingWord.Substring(1);
        }

        public void UpdateJobTime(int id, int timeAllow)    //updates the new time allowed for the job
        {
            job updateJob = db.jobs.Find(id);
            updateJob.TimeAllowed = (short)timeAllow;

            db.Entry(updateJob).State = EntityState.Modified;
            db.SaveChanges();
        }

        //================//
        //ReportController//
        //================//

        //Return a specific mechanic
        public mechanic GetMechanic(int MechanicId)
        {
            return db.mechanics.Include(mech => mech.jobs)
                .SingleOrDefault(mech => mech.MechanicID == MechanicId);
        }

        //Returns mechanics to be used in a dropdown
        public IQueryable<object> GetMechanicsForDropdown()
        {
            return db.mechanics.Select(mech => new { MechanicID = mech.MechanicID, FullName = mech.Firstname.Substring(0, 1).ToUpper() + mech.Firstname.Substring(1) + " " + mech.LastName.Substring(0, 1).ToUpper() + mech.LastName.Substring(1) })
                .OrderBy(mech => mech.FullName);
        }

        //Returns all the jobs a mechanic has complete
        public IQueryable<job> GetMechanicsCompleteJobs(int MechanicId)
        {
            return db.jobs.Include(jb => jb.jobslist)
                    .Include(jb => jb.booking.car.model.make)
                    .Include(jb => jb.jobslist)
                    .Include(jb => jb.jobslist.jobscategory)
                    .Where(jm => jm.MechanicID == MechanicId && jm.Status == "complete")
                    .OrderByDescending(jm => jm.CompleteDate);
        }

        //Returns the jobs a mechanic has complete in the last 12 months
        public IQueryable<job> GetJobsCompleteInPrevious12Months(int MechanicId, DateTime MinDate)
        {
            return db.jobs.Where(j => j.CompleteDate.Value.Year >= MinDate.Year
                && j.CompleteDate.Value.Month > MinDate.Month
                && j.MechanicID == MechanicId
                && j.Status == "complete");
        }

        //Returns all job categories
        public IQueryable<jobscategory> GetJobsCategories()
        {
            return db.jobscategories.OrderBy(jobcat => jobcat.Type);
        }

        //Returns the Jobs in each category for a mechanic
        public IQueryable<job> GetJobsInEachCategory(int CategoryId, int MechanicId)
        {
            return db.jobs.Where(jb => jb.MechanicID == MechanicId
                && jb.jobslist.jobscategory.JobsCategoryID == CategoryId)
                .OrderByDescending(jb => jb.CompleteDate);
        }

        //Return a specific category
        public jobscategory GetCategory(int CategoryId)
        {
            return db.jobscategories.SingleOrDefault(jc => jc.JobsCategoryID == CategoryId);
        }

        //Return all the jobs in a Category
        public IQueryable<job> GetCategoryJobs(int CategoryId)
        {
            return db.jobs.Where(jb => jb.jobslist.JobCategoryID == CategoryId);
        }

        //Return the complete jobs in a Category
        public IQueryable<job> GetCategoryCompleteJobs(int CategoryId)
        {
            return db.jobs.Where(jb => jb.jobslist.JobCategoryID == CategoryId
                && jb.Status == "complete")
                .OrderBy(jb => jb.CompleteDate);
        }

        //Returns the jobs complete in each category over the last 12 months
        public IQueryable<job> GetCategoryJobsCompleteInPrevious12Months(int CategoryId, DateTime MinDate)
        {
            return db.jobs.Where(j => j.CompleteDate >= MinDate
                && j.jobslist.JobCategoryID == CategoryId
                && j.Status == "complete")
                .OrderBy(j => j.CompleteDate.Value.Month);
        }

        //Returns all the Job List in a Job Category
        public IQueryable<jobslist> GetCategoryJobsList(int CategoryId)
        {
            return db.jobslists.Where(jl => jl.JobCategoryID == CategoryId);
        }

        //Returns all the jobs in a Job List
        public IQueryable<job> GetJobsForCategoryJobsList(int JobListId)
        {
            return db.jobs.Where(jb => jb.JobsListID == JobListId);
        }

        //Takes in a list of jobs and filters them by all, over time, on time and acceptable time
        public IQueryable<job> FilterCompleteJobs(string filter, IQueryable<job> jobs)
        {
            IQueryable<job> ReturnJobs;
            switch (filter)
            {
                case "1":
                    ReturnJobs = jobs;
                    break;
                case "2":
                    ReturnJobs = jobs
                        .Where(jb => jb.TimeTaken <= jb.TimeAllowed);
                    break;
                case "3":
                    ReturnJobs = jobs.Where(jb => jb.TimeTaken > jb.TimeAllowed && jb.TimeTaken <= Math.Round((double)jb.TimeAllowed * 1.1));
                    break;
                case "4":
                    ReturnJobs = jobs.Where(jb => jb.TimeTaken > Math.Round((double)jb.TimeAllowed * 1.1));
                    break;
                default:
                    ReturnJobs = jobs;
                    break;
            }
            return ReturnJobs.OrderByDescending(jb => jb.CompleteDate);
        }

        //Return mechanics with over time jobs with the number of over time jobs they have completed
        public IQueryable<object> Rankings()
        {
            return from jobs in db.jobs
                   join mech in db.mechanics
                   on jobs.MechanicID equals mech.MechanicID
                   where jobs.TimeTaken > Math.Round((double)jobs.TimeAllowed * 1.1)
                   group mech by mech.MechanicID into g
                   select new { Name = g.Select(gr => gr.Firstname.Substring(0, 1).ToUpper() + gr.Firstname.Substring(1) + " " + gr.LastName.Substring(0, 1).ToUpper() + gr.LastName.Substring(1)).FirstOrDefault(), Count = g.Count() };
        }

        //Get the number of overtime job
        public int OverTimeJobsCount()
        {
            return db.jobs.Where(jb => jb.TimeTaken > Math.Round((double)jb.TimeAllowed * 1.1)).Count();
        }

        //get job details of job with id
        public job JobDetailsOfId(int id)
        {
            return db.jobs.Include("jobslist").Include(jb => jb.booking.car.model)
                    .Include(jb => jb.booking.car.model.make)
                    .Include(jb => jb.jobslist)
                    .Include(jb => jb.jobslist.jobscategory)
                    .SingleOrDefault(jb => jb.JobID == id);
        }





        //===========//
        //MechanicJob//
        //===========//
        public int CurrentTimeOfJob(job j)
        {
            if (j.TimeAllowed == null)
                return 0;

            return (int)j.TimeAllowed;
        }

        public job GetJob(int? id)
        {
            id = (int)id;
            return db.jobs.
                Include(jb => jb.mechanic).
                Include(jb => jb.booking.car.model.make).
                Include(jb => jb.jobslist).
                Where(jb => jb.JobID == id).SingleOrDefault();
        }

        public int TimeSheetStartTime(int? id)   //when job is started by mechanic
        {
            timessheet tSheet = new timessheet();
            tSheet.JobID = (int)id;
            tSheet.StartTime = DateTime.Now;

            db.timessheets.Add(tSheet);

            job jb = GetJob(id);
            if (jb.Status == "new")
            {
                jb.TimeStarted = DateTime.Now;
            }
            jb.Status = "active";
            db.Entry(jb).State = System.Data.EntityState.Modified;


            mechanic mec = db.mechanics.Find(jb.MechanicID);
            mec.Available = false;
            db.Entry(mec).State = System.Data.EntityState.Modified;
            db.SaveChanges();

            return tSheet.TimeSheetID;
        }


        //when job is finished by clicked complete job (also when pausing job)
        public HttpStatusCodeResult TimeSheetEndTime(int? id, int? jobId, string status)
        {
            timessheet timeSheet = db.timessheets.Find(id);
            timeSheet.EndTime = DateTime.Now;



            db.Entry(timeSheet).State = System.Data.EntityState.Modified;

            if (status == "complete")
            {

                List<timessheet> timesheets = (from t in db.timessheets
                                               where t.JobID == jobId
                                               select t).ToList();

                TimeSpan overallTimeOfJob = new TimeSpan();


                //if job is complete by mechanic


                foreach (var t in timesheets)   //checks if job has been paused/started multiple times
                {
                    overallTimeOfJob += (DateTime)t.EndTime - (DateTime)t.StartTime;
                }

                int overallTimeInMinutes = Convert.ToInt32(overallTimeOfJob.TotalMinutes);

                job jb = GetJob(jobId);
                jb.Status = "complete";
                jb.CompleteDate = DateTime.Now;
                if (overallTimeInMinutes < 1)
                {
                    jb.TimeTaken = 1;
                }
                else
                {

                    jb.TimeTaken = (short)overallTimeInMinutes;
                }

                db.Entry(jb).State = System.Data.EntityState.Modified;
                db.SaveChanges();


                booking booking = db.bookings.Find(jb.BookingID);



                int activeJobsOnBooking = (from j in db.jobs
                                           where j.BookingID == booking.BookingID &&
                                           j.Status == "complete"
                                           select j.BookingID).Count();

                int overallJobsObBooking = (from j in db.jobs
                                            where j.BookingID == booking.BookingID
                                            select j.BookingID).Count();

                if (activeJobsOnBooking == overallJobsObBooking)    //if all jobs in booking is complete
                {
                    booking.Status = "complete";

                    booking.CompleteDate = DateTime.Now;

                    db.Entry(timeSheet).State = System.Data.EntityState.Modified;
                    db.SaveChanges();


                    mechanic mec = db.mechanics.Find(jb.MechanicID);
                    mec.Available = true;
                    db.Entry(mec).State = System.Data.EntityState.Modified;
                    db.SaveChanges();

                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
            }
            //if job is paused by mechanic
            else
            {
                job jb = GetJob(jobId);
                jb.Status = "active"; //active-----------

                db.Entry(jb).State = System.Data.EntityState.Modified;
                db.SaveChanges();

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            //return timeSheet;
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult UserComment(job incomingJob)    //mechanics comment when finishing job
        {
            db.Entry(incomingJob).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public job GetMechanicsJobs(int? id, int userid)
        {
            var job = db.jobs.Include(x => x.jobslist)
                             .Include(x => x.booking.car.model.make)
                             .Where(x => x.JobID == id && x.MechanicID == userid).SingleOrDefault();
            return job;
        }

        public timessheet FindCurrentTimeSheet(job jb)
        {
            var timesheet = db.timessheets.Where(x => x.JobID == jb.JobID).OrderByDescending(x => x.StartTime).First();
            return timesheet;
        }

        //Dispose of DbContext Connection
        public void Dispose()
        {
            db.Dispose();
        }

    }//End Class
}//End Namespace


