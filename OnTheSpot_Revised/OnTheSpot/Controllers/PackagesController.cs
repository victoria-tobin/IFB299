﻿using System;
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

        //
        // POST: /Packages/Create

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
                if(package.Status == Status.Delivered)
                {
                    var orderID = db.Packages.Where(p => p.OrderID == package.OrderID);
                    var sameOrder = db.Packages.Count(p => p.OrderID == package.OrderID);
                    if(sameOrder == 1)
                    {
                        db.Orders.Find(orderID).Completed = true;
                    } else if(db.Packages.Count(p => p.OrderID == package.OrderID && p.Status == Status.Delivered) == sameOrder)
                    {
                        db.Orders.Find(orderID).Completed = true;
                    }
                }
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