using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data;
using PagedList;
using System.Net;
using MechanicTracker.DAL;

namespace MechanicTracker.Controllers
{
    [HandleError]
    public class BookingController : Controller
    {


        private IMechanicTrackerRepository _repo;
        public BookingController(IMechanicTrackerRepository repo)
        {
            _repo = repo;
        }


        //FRONTDESK INDEX PAGE
        public ActionResult FrontDesk()
        {
            using (var db = new MTEntities())
            {
                // joins CARS, MAKE and MODEL to BOOKINGS table
                var query = db.bookings.Include(bookcar => bookcar.car).Include(carmodel => carmodel.car.model)
                            .Include(modelmake => modelmake.car.model.make).ToList();

                if (query != null)
                {
                    return View(query);
                }
                else
                {
                    return RedirectToAction("index");
                }
            }
        }



        // FRONTDESK EDIT BOOKING
        public PartialViewResult FrontDeskEditBooking(int id)
        {
            using (var db = new MTEntities())
            {
                var fuel = from b in db.bookings
                           join c in db.cars
                           on b.CarID equals c.CarID
                           select c.FuelType;

                var carmodel = from b in db.bookings
                               join c in db.cars
                               on b.CarID equals c.CarID
                               join cmod in db.models
                               on c.ModelID equals cmod.MakeID
                               select cmod.ModelName;

                var carmake = from b in db.bookings
                              join c in db.cars
                              on b.CarID equals c.CarID
                              join cmod in db.models
                              on c.ModelID equals cmod.MakeID
                              join cmake in db.makes
                              on cmod.MakeID equals cmake.MakeID
                              select cmake.MakeName;

                ViewBag.FuelType = db.cars.ToList();

                ViewBag.makes = db.makes.ToList();

                ViewBag.MakeID = new SelectList(db.makes, "MakeID", "MakeName");
                return PartialView("_FrontDeskEditBooking", db.bookings.Find(id));

                //var bookings = db.bookings.Include(b => b.car.model.make).OrderBy(b => b.CompleteDate); 


            }

        }
        //
        // GET: /Booking/EditBooking/5

        public ActionResult EditBooking(int id)
        {
            using (var db = new MTEntities())
            {
                return View(db.bookings.Find(id));
            }
        }

        //
        // POST: /Booking/EditBooking/5

        [HttpPost]
        public ActionResult EditBooking(booking editBooking)
        {
            using (var db = new MTEntities())
            {
                try
                {

                    db.Entry(editBooking).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("booking");
                }
                catch
                {
                    return View();
                }
            }
        }

        public PartialViewResult CreateBooking(string searchString)
        {
            using (var db = new MTEntities())
            {
                // Query for Searching Registration of car
                var queryReg = from b in db.bookings
                               join c in db.cars
                               on b.CarID equals c.CarID
                               select c.Registration;


                var fuel = from b in db.bookings
                           join c in db.cars
                           on b.CarID equals c.CarID
                           select c.FuelType;

                var carmodel = from b in db.bookings
                               join c in db.cars
                               on b.CarID equals c.CarID
                               join cmod in db.models
                               on c.ModelID equals cmod.MakeID
                               select cmod.ModelName;

                var carmake = from b in db.bookings
                              join c in db.cars
                              on b.CarID equals c.CarID
                              join cmod in db.models
                              on c.ModelID equals cmod.MakeID
                              join cmake in db.makes
                              on cmod.MakeID equals cmake.MakeID
                              select cmake.MakeName;

                ViewBag.makes = db.makes.ToList();
                ViewBag.FuelType = db.cars.ToList();

                return PartialView("_FrontDeskCreateBooking");
            }
        }

        [HttpPost]
        public ActionResult CreateBooking(booking inBooking)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var b = new MTEntities())
                    {
                        b.bookings.Add(inBooking);
                        b.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //Dropdown list for Make/Model
        [HttpGet]
        public PartialViewResult getMake(int id)
        {
            using (var db = new MTEntities())
            {
                ViewBag.models = db.models
                                    .Where(x => x.MakeID == id)
                                    .ToList();
                return PartialView("_CarMakeDropdown");
            }
        }


        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Booking/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }



        //
        // GET: /Booking/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Booking/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        /*--------------------------------------------------*/
        /////////////////////////////////////
        /////////////////////////////////////
        /////////////////////////////////////
        /////////////////////////////////////
        /////////////////////////////////////
        /*--------------------------------------------------*/

        //Manager actions for displaying bookings,displaying job already attached to cars,adding jobs to cars


        //Get list of bookings
        //Role only authorized to manager
        //Return five bookings a time for pagination
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public ActionResult Index(int? page)
        {
            int pageNum = page ?? 1;
            var bkns = _repo.GetBookingsList().ToPagedList(pageNum, 10);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Bookings", bkns);
            }

            return View(bkns);
        }


        //Return details for booking.
        [HttpGet]
        public PartialViewResult Details(int? id)
        {
            if (id != null)
            {
                ViewBag.count = _repo.countJobs(id);
                ViewBag.completejobs = _repo.countCompleteJobs(id);
                var bDetails = _repo.GetBookingDetails(id);
                return PartialView("_Details", bDetails);
            }

            else
            {
                return PartialView("_Error");
            }
        }

        //Return jobs that belong to car for a booking
        //Five at a time for pagination
        [HttpGet]
        public PartialViewResult Getjobs(int? id, int? page)
        {
            if (id != null)
            {
                int pageNum = page ?? 1;
                var jobs = _repo.GetJobsForBooking(id).ToPagedList(pageNum, 3);

                if (jobs != null)
                {
                    return PartialView("_JobDetails", jobs);
                }
                else
                {
                    return PartialView("_Error");
                }
            }
            else
            {
                return PartialView("_Error");
            }
        }

        //Call the form to create a job
        [HttpGet]
        public PartialViewResult createform(int? id)
        {
            if (id != null)
            {
                ViewBag.categories = _repo.GetListOfJobCatergories();
                ViewBag.bookingID = id;
                return PartialView("_CreateJobs");
            }
            else
            {
                return PartialView("_Error");
            }

        }
        //Get the jobs when the job catergory is selected and return json to view
        [HttpGet]
        public ActionResult getjobsFordropdown(int? id)
        {
            if (id != null)
            {
                var j = _repo.GetListOfJobLists(id);
                return Json(j, JsonRequestBehavior.AllowGet);
                //return Json(j, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Ajax call will alert error on client
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
        }

        //Create Job
        [HttpPost]
        public ActionResult CreateJob(job j)
        {

            if (ModelState.IsValid)
            {
                var job = _repo.CreateJob(j);
                return job;
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
    }//END CLASS
}//END NAMESPACE
