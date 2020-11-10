using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FabrikamResidences_Activities.Models;
using FabrikamResidences_Activities.Data;

namespace FabrikamResidences_Activities.Controllers
{
    public class ActivitiesController : Controller
    {

        IPortalRepository _portalRepository;

        public ActivitiesController(IPortalRepository portalRepository)
        {
            _portalRepository = portalRepository;
        }

        public IActionResult Index()
        {
            return View(_portalRepository.GetActivities().ToList());
        }

        // GET: Activities/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = _portalRepository.GetActivity(id.Value);

            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAttendee(int Id, [Bind("PortalActivityId, FirstName, LastName, Email")] Attendee attendee) 
        {
            _portalRepository.AddAttendee(attendee);
            return RedirectToAction(nameof(Details), new {Id = attendee.PortalActivityId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string modifyDate, [Bind("Name,Description,Date")] PortalActivity activity)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    activity.Date = Convert.ToDateTime(modifyDate);
                    _portalRepository.AddActivity(activity);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(activity);
        }

        // GET: Activities/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = _portalRepository.GetActivity(id.Value);
            if (activity == null)
            {
                return NotFound();
            }
            activity.ModifyDate = activity.Date.ToString("MM/dd/yyyy h:mm tt");
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string modifyDate, [Bind("Id,Name,Description,Date")] PortalActivity activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    activity.Date = Convert.ToDateTime(modifyDate);
                    _portalRepository.UpdateActivity(activity);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(activity);
        }

        // GET: Activities/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var activity = _portalRepository.GetActivity(id.Value);

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _portalRepository.DeleteActivity(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
