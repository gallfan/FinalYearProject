using MechanicTracker.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity.Infrastructure;
﻿

namespace MechanicTracker.Controllers
{
    [Authorize(Roles = "Manager")]
    public class MechanicController : Controller
    {


        private IMechanicTrackerRepository _repo;
        public MechanicController(IMechanicTrackerRepository repo)
        {
            _repo = repo;
        }

        //Returns a list of mechanics
        public ActionResult Mechanics(int? page = 1)
        {
            int pageNum = page ?? 1;
            var mechs = _repo.GetMechanics().ToPagedList(pageNum, 10);
            //Returns updated list of mechanics when edit or create is done
            if (Request.IsAjaxRequest())
            {
                return PartialView("_mechanicList", mechs);
            }
            //First Get non ajax
            if (mechs != null)
            {
                return View(mechs);
            }
            else
            {
                return View("Error");
            }
        }

        //Returns a partialview of mechanic details
        [HttpGet]
        public PartialViewResult mechDetails(int? id)
        {

            if (id != null)
            {
                var mech = _repo.GetmechDetails(id);
                if (mech != null)
                {
                    return PartialView("_mechDetails", mech);
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

        //Add new mechanic to database
        [HttpPost]
        public ActionResult CreateMechanic(mechanic m)
        {
            if (ModelState.IsValid)
            {
                var mech = _repo.CreateMechanic(m);
                return mech;
            }
            else
            {
                //Ajax call will alert error on client
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        //Returns a form for adding mechanics
        [HttpGet]
        public PartialViewResult createmechanicsform()
        {
            return PartialView("_createmechanicsform");
        }



        //Returns a partialview of mechanic details for edit form
        [HttpGet]
        public PartialViewResult mechDetailsforForm(int? id)
        {
            if (id != null)
            {

                var mech = _repo.GetmechDetails(id);
                if (mech != null)
                {
                    return PartialView("_editMechanicsForm", mech);
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


        //Returns details of mechanic to be updated
        [HttpGet]
        public PartialViewResult editMechanicsForm(int? id)
        {
            if (id != null)
            {
                var mech = _repo.GetmechDetails(id);
                if (mech != null)
                {
                    return PartialView("_editMechanicsForm", mech);
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
        public ActionResult ChkUserName(string username)
        {

            var check = _repo.checkUsername(username);
            if (check)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        //Add edit mechanic details to database
        [HttpPost]
        public ActionResult EditMechanic(mechanic m)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var mech = _repo.EditMechanic(m);
                    //return mech;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var databaseValues = (mechanic)entry.GetDatabaseValues().ToObject();
                    var clientValues = (mechanic)entry.Entity;
                    if (databaseValues.Email != clientValues.Email)
                        ModelState.AddModelError("Email", "Current value: " + databaseValues.Email);
                    if (databaseValues.LastName != clientValues.LastName)
                        ModelState.AddModelError("LastName", "Current value: " + databaseValues.LastName);
                    if (databaseValues.Firstname != clientValues.Firstname)
                        ModelState.AddModelError("FirstName", "Current value: " + databaseValues.Firstname);
                    if (databaseValues.PhoneNumber != clientValues.PhoneNumber)
                        ModelState.AddModelError("PhoneNumber", "Current value: " + databaseValues.PhoneNumber);
                    if (databaseValues.SkillLevel != clientValues.SkillLevel)
                        ModelState.AddModelError("SkillLevel", "Current value: " + databaseValues.SkillLevel);

                    ModelState.AddModelError("Message", "The record you attempted to edit "
                      + "was modified after you got the original value. The "
                      + "edit operation was canceled and the current values in the database "
                      + "have been displayed. To edit this record, click "
                      + "the Save button again.");
                    m.RowVersion = databaseValues.RowVersion;
                    return PartialView("_editMechanicsForm", m);
                }
                return new HttpStatusCodeResult(200);
            }
            else
            {
                return new HttpStatusCodeResult(200);
            }
        }
    }
}
