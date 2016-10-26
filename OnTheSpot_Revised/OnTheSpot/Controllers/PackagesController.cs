using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnTheSpot.Models;
using OnTheSpot.DAL;

using WebMatrix.WebData;


namespace OnTheSpot.Controllers
{
    public class PackagesController : Controller
    {
        // Forms a connection with the database.
        private DatabaseModels db = new DatabaseModels();

        /// <summary>
        /// Default controller for Packages. Displays packages for an order.
        /// </summary>
        /// <param name="searchInt"></param>
        /// <returns></returns>
        public ActionResult Index(int? searchInt)
        {
            ViewBag.Search = searchInt;
            var packages = from p in db.Packages
                           select p;           
            if (User.IsInRole("Courier"))
            {
                packages = from p in db.Packages.Where(p => p.AssignedCourier == WebSecurity.CurrentUserName)
                               select p;
            }
            if (User.IsInRole("Customer"))
            {
                packages = from p in db.Packages.Where(p => p.Order.Username == WebSecurity.CurrentUserName)
                           select p;             
            }
            if (searchInt != null)
            {
                packages = from o in packages.Where(p => p.OrderID == searchInt)
                           select o;
            }
            return View(packages.ToList());
        }

        /// <summary>
        /// Details controller, displays the details of a given package.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id = 0)
        {
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }

        /// <summary>
        /// Create controller for creating a package for an order.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {

            Package pack = new Package()
            {
                OrderID = 1,
                Priority = null,
                Status = Status.ReadyForPickup,
                Weight = 0,
                Collected = null,
                AssignedCourier = null,
                Delivered = null
            };

            // select list for couriers
            var cour = db.Users.Where(p => p.UserRole == "Courier");
            ViewBag.Cour = new SelectList(cour, "UserName", "UserName");

            // select list for order ID
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID");

            return View(pack);
        }

        /// <summary>
        /// Create controller that populates the properities of a package.
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Package package)
        {
            if (ModelState.IsValid)
            {
                var p = new Package
                {
                    OrderID = package.OrderID,
                    Status = Status.ReadyForPickup,
                    Priority = package.Priority,
                    Weight = package.Weight,
                    AssignedCourier = package.AssignedCourier
                };
                db.Packages.Add(p);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // select list for couriers
            var cour = db.Users.Where(p => p.UserRole == "Courier");
            ViewBag.Cour = new SelectList(cour, "UserName", "UserName");
            // select list for order ID
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", package.OrderID);
            return View(package);
        }

        /// <summary>
        /// Collected controller, for stating that a package is indeed collected.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Collected(int id = 0)
        {
            Package package = db.Packages.Find(id);

            package.Collected = DateTime.Now;
            package.Status = Status.AtWarehouse;
            db.Orders.Find(package.OrderID).Completed = false;


            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Intransit controller, for stating that a package is intransit.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult InTransit(int id = 0)
        {
            Package package = db.Packages.Find(id);

            if (package.Collected == null)
            {
                package.Collected = DateTime.Now;
            }
            package.Status = Status.InTransit;
            db.Orders.Find(package.OrderID).Completed = false;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Delivered controller, for stating that a package is delivered.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delivered(int id = 0)
        {
            Package package = db.Packages.Find(id);

            if (package.Collected == null)
            {
                package.Collected = DateTime.Now;
            }

            package.Delivered = DateTime.Now;
            package.Status = Status.Delivered;
            checkIfCompleted(id);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// CheckIfCompleted controller, for checking if a package is indeed complete.
        /// </summary>
        /// <param name="id"></param>
        private void checkIfCompleted(int id)
        {
            // set completed value in order to true if all packages have been delivered
            Package package = db.Packages.Find(id);

            var orderID = package.OrderID;
            var sameOrder = db.Packages.Count(p => p.OrderID == package.OrderID);
            if (sameOrder == 1)
            {
                db.Orders.Find(orderID).Completed = true;
            }
            else if (db.Packages.Count(p => p.OrderID == package.OrderID && p.Status == Status.Delivered) == (sameOrder - 1))
            {
                db.Orders.Find(orderID).Completed = true;
            }
            db.SaveChanges();
        }

        /// <summary>
        /// Edit controller, obtains the properities of a specific package.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Edit controller, for populating the properities of that package.
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Package package)
        {
            if (ModelState.IsValid)
            {
                db.Entry(package).State = EntityState.Modified;
                if (package.Status == Status.Delivered)
                {
                    checkIfCompleted(package.PackageID);
                }
                else
                {
                    db.Orders.Find(package.OrderID).Completed = false;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", package.OrderID);
            return View(package);
        }

        /// <summary>
        /// "Deletes" controller, for deleting a package.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id = 0)
        {
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }

        /// <summary>
        /// DeleteConfirmed controller for removing a package.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Package package = db.Packages.Find(id);
            db.Packages.Remove(package);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Disposes object.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}