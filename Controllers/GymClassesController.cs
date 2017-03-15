using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gymbokning_v0._1.Models;

namespace Gymbokning_v0._1.Controllers
{
    public class GymClassesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: GymClasses
        [Authorize]
        public async Task<ActionResult> Index()
        {
            return View(await db.GymClasses.ToListAsync());
        }

        // GET: GymClasses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GymClass gymClass = await db.GymClasses.FindAsync(id);
            if (gymClass == null)
            {
                return HttpNotFound();
            }
            return View(gymClass);
        }

        // GET: GymClasses/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: GymClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                db.GymClasses.Add(gymClass);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(gymClass);
        }

        // GET: GymClasses/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GymClass gymClass = await db.GymClasses.FindAsync(id);
            if (gymClass == null)
            {
                return HttpNotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gymClass).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(gymClass);
        }

        // GET: GymClasses/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GymClass gymClass = await db.GymClasses.FindAsync(id);
            if (gymClass == null)
            {
                return HttpNotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            GymClass gymClass = await db.GymClasses.FindAsync(id);
            db.GymClasses.Remove(gymClass);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult BookingToggle(int id)
        {
            GymClass CurrentClass = db.GymClasses.Where(g => g.Id == id).FirstOrDefault();
            ApplicationUser CurrentUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (CurrentClass.AttendingMembers.Contains(CurrentUser))
            {
                CurrentClass.AttendingMembers.Remove(CurrentUser);
                db.SaveChanges();
            }
            else
            {
                CurrentClass.AttendingMembers.Add(CurrentUser);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
