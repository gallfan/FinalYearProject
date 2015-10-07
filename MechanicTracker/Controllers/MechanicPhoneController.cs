using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MechanicTracker.Models;
using System.Data.Entity;
using MechanicTracker.DAL;

namespace MechanicTracker.Controllers
{
    
    public class MechanicPhoneController : ApiController
    {
        
        IMechanicTrackerRepository _repo;
        public MechanicPhoneController()
        {
            _repo = new MechanicTrackerRepository();
        }

        /*[HttpGet]
        public IEnumerable<job> GetJobs(string reg)
        {
            var jobs = _repo.GetMechanicsJobs(reg);
        }*/
        /*public IEnumerable<job> GetJobs()
        {
          var jobs = _repo.GetJob();
>>>>>>> MechanicPhone
          if (jobs != null)
          {
              return jobs;          
          }
          else
          {
              throw new HttpResponseException(HttpStatusCode.BadRequest);
          }
        }*/

        [HttpGet]
        public job Details(int id)
        {
            var job = _repo.GetJobsForMech(id);
            return job;
           
        }



 

      
    }
}
