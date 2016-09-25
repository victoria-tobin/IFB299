using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnTheSpot.Models;
using OnTheSpot.DAL;

namespace OnTheSpot.Controllers
{
    public class PackagesController : Controller
    {
        private DatabaseModels db = new DatabaseModels();

        //
        // GET: /Packages/

        public ActionResult Index(int? searchInt)
        {

            ViewBag.Search = searchInt;

            var packages = from p in db.Packages
                           select p;

            if (searchInt != null)
            {
                packages = from o in packages.Where(p => p.OrderID == searchInt)
                           select o;
            }

            return View(packages.ToList());
        }

        //
        // GET: /Packages/Details/5

        public ActionResult Details(int id = 0)
        {
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }

        //
        // GET: /Packages/Create

        public ActionResult Create()
        {
            // select list for couriers
            var cour = db.Users.Where(p => p.UserRole == "Courier");
            ViewBag.Cour = new SelectList(cour, "UserName", "UserName");

            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID");
            return View();
        }

        //
        // POST: /Packages/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Package package)
        {
            if (ModelState.IsValid)
            {
                db.Packages.Add(package);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // select list for couriers
            var cour = db.Users.Where(p => p.UserRole == "Courier");
            ViewBag.Cour = new SelectList(cour, "UserName", "UserName");

            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", package.OrderID);
            
            return View(package);
        }

        //
        // GET: /Packages/Edit/5

        public ActionResult Edit(int id = 0)
        {
            
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }

            // select list for couriers
            var cour = db.Users.Where(p => p.UserRole == "Courier");
            ViewBag.Cour = new SelectList(cour, "UserName", "UserName", package.AssignedCourier);

            return View(package);
        }

        //
        // POST: /Packages/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Package package)
        {
            if (ModelState.IsValid)
            {
                db.Entry(package).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", package.OrderID);
            return View(package);
        }

        //
        // GET: /Packages/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }

        //
        // POST: /Packages/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Package package = db.Packages.Find(id);
            db.Packages.Remove(package);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}