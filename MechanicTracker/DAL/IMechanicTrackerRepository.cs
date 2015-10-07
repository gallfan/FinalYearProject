using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MechanicTracker.DAL
{
    public interface IMechanicTrackerRepository : IDisposable
    {
        //==============//
        //HomeController//
        //==============//
        IQueryable<booking> GetRecentBookings(DateTime date, DateTime CurrentDate);
        IQueryable<job> GetActiveJobs();
        IQueryable<job> GetRecentlyCompleteJobs(string filter);

        //================//
        //ReportController//
        //================//
        IQueryable<job> GetMechanicsCompleteJobs(int MechanicId);
        mechanic GetMechanic(int MechanicId);
        IQueryable<job> GetJobsCompleteInPrevious12Months(int MechanicId, DateTime MinDate);
        IQueryable<jobscategory> GetJobsCategories();
        IQueryable<job> GetJobsInEachCategory(int CategoryId, int MechanicId);
        jobscategory GetCategory(int CategoryId);
        IQueryable<job> GetCategoryJobs(int CategoryId);
        IQueryable<job> GetCategoryCompleteJobs(int CategoryId);
        IQueryable<jobslist> GetCategoryJobsList(int CategoryId);
        IQueryable<job> GetJobsForCategoryJobsList(int JobListId);
        IQueryable<job> GetCategoryJobsCompleteInPrevious12Months(int CategoryId, DateTime MinDate);
        IQueryable<job> FilterCompleteJobs(string filter, IQueryable<job> jobs);
        IQueryable<object> Rankings();
        IQueryable<object> GetMechanicsForDropdown();
        int OverTimeJobsCount();
        job JobDetailsOfId(int id);




         job GetJobsForMech(int id);
         //HttpStatusCodeResult StartJob(timessheet t);






         //////=====================/////////
         ////////MechanicJobsController////////
         //////=====================/////////
        job GetMechanicsJobs(int? id,int userid);
        IEnumerable<job> GetMechanicsActiveJobs(int id);
        IEnumerable<job> GetJobs();
        List<notification> GetNotications(int? id, bool? read);
        IEnumerable<job> searchedJobs(string reg,int id);
        int getUserID(string usrename);
        int CountNotications(int id);
        HttpStatusCodeResult UpdateNotification(int id);
        int TimeSheetStartTime(int? id);
        HttpStatusCodeResult TimeSheetEndTime(int? id, int? jobId, string status);
        job GetJob(int? id);
        ActionResult UserComment(job incomingJob);
        HttpStatusCodeResult newNotification(job j);
        timessheet FindCurrentTimeSheet(job jb);

        //====================//
        //Mechanics Controller//
        //====================//
        List<mechanic> GetMechanics();
        mechanic GetmechDetails(int? id);
        HttpStatusCodeResult CreateMechanic(mechanic m); 
        HttpStatusCodeResult EditMechanic(mechanic m);   
        bool checkUsername(string username);

        //////=====================/////////
        ////////BookingsController////////
        //////=====================/////////
        List<booking> GetBookingsList();
        booking GetBookingDetails(int? id);
        List<job> GetJobsForBooking(int? id);
        List<jobscategory> GetListOfJobCatergories();
        List<jobslist> GetListOfJobLists(int? id);
        HttpStatusCodeResult CreateJob(job job);
        int countJobs(int? id);
        int countCompleteJobs(int? id);
        
        //////==================//////

        //JobsController - Index
        //List<job> GetjobIndex();

        //JobsController - JobDisplay
        IQueryable<job> DisplayJob(string filteredJobs);

        //JobsController - DisplayAllMechanics(int id, string filteredMechanics)
        List<mechanic> DisplyMechanics(int? id, string filteredMechanics);

        //JobsController - CreateJobMechanic(job jobMechanic)

        HttpStatusCodeResult AssigningMechanic(job incomingJob);

        int CurrentTimeOfJob(job j);
    }
}