using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcCourier.Models;

namespace MvcCourier.Controllers
{
    public class PackageController : Controller
    {
        private PackageDBContext db = new PackageDBContext();

        //
        // GET: /Package/

        public ActionResult Index(string searchString)
        {
            var packages = from p in db.Packages
                           select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                packages = packages.Where(s => s.PackageId.Equals(searchString));

                if (packages.Count() != 0)
                {
                    ViewBag.showResult = true;
                }
                else
                {
                    ViewBag.showError = true;
                    ViewBag.Error = "No package found with code '" + searchString + "'";
                }

            }

            return View(packages);
        }

        //
        // GET: /Package/Details/5

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
        // GET: /Package/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Package/Create

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

            return View(package);
        }

        //
        // GET: /Package/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }

        //
        // POST: /Package/Edit/5

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
            return View(package);
        }

        //
        // GET: /Package/Delete/5

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
        // POST: /Package/Delete/5

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