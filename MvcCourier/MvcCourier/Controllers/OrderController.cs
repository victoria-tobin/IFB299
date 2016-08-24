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
    public class OrderController : Controller
    {
        private OrdersDBContext db = new OrdersDBContext();

        //
        // GET: /Order/

        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        //
        // GET: /Order/Details/5

        public ActionResult Details(int id = 0)
        {
            OrderModels ordermodels = db.Orders.Find(id);
            if (ordermodels == null)
            {
                return HttpNotFound();
            }
            return View(ordermodels);
        }

        //
        // GET: /Order/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Order/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderModels ordermodels)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(ordermodels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ordermodels);
        }

        //
        // GET: /Order/Edit/5

        public ActionResult Edit(int id = 0)
        {
            OrderModels ordermodels = db.Orders.Find(id);
            if (ordermodels == null)
            {
                return HttpNotFound();
            }
            return View(ordermodels);
        }

        //
        // POST: /Order/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrderModels ordermodels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordermodels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ordermodels);
        }

        //
        // GET: /Order/Delete/5

        public ActionResult Delete(int id = 0)
        {
            OrderModels ordermodels = db.Orders.Find(id);
            if (ordermodels == null)
            {
                return HttpNotFound();
            }
            return View(ordermodels);
        }

        //
        // POST: /Order/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderModels ordermodels = db.Orders.Find(id);
            db.Orders.Remove(ordermodels);
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