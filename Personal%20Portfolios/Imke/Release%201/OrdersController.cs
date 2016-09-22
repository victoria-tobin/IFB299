using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnTheSpot.Models;
using OnTheSpot.DAL;
using System.Web.Security;
using WebMatrix.WebData;


namespace OnTheSpot.Controllers
{
    public class OrdersController : Controller
    {
        private DatabaseModels db = new DatabaseModels();

        //
        // GET: /Orders/

        public ActionResult Index(bool? outstanding, string sortOrder, int? searchInt)
        {
            ViewBag.DateSortParm = string.IsNullOrEmpty(sortOrder) ? "Date_desc" : "";
            ViewBag.OrderIdSortParm = sortOrder == "OrderId" ? "OrderId_desc" : "OrderId";
            ViewBag.IsOut = outstanding;
            ViewBag.Search = searchInt;

            var orders = from o in db.Orders
                         select o;

            if (outstanding != null)
            {
                orders = from o in orders.Where(o => o.Completed == false)
                         select o;
            }
            
            if (searchInt != null)
            {
                orders = from o in orders.Where(o => o.OrderID == searchInt)
                             select o; 
            }

            switch (sortOrder)
            {
                case "Date_desc":
                    orders = from o in orders.OrderByDescending(o => o.OrderSubmitted)
                                 select o;
                    break;
                case "OrderId":
                    orders = from o  in orders.OrderBy(o => o.OrderID)
                                 select o;
                    break;
                case "OrderId_desc":
                    orders = from o in orders.OrderByDescending(o => o.OrderID)
                                 select o;
                    break;
                default:
                    orders = from o in orders.OrderBy(o => o.OrderSubmitted)
                                 select o;
                    break;
            }


            return View(orders.ToList());
             
        }

        //
        // GET: /Orders/Details/5

        public ActionResult Details(int id = 0)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }


        //
        // GET: /Orders/Create

        public ActionResult Create()
        {
            if (User.IsInRole("Admin"))
            {
                Order o = new Order()
                {
                    Username = "",
                    OrderSubmitted = DateTime.Now,
                    Completed = false,
                    Pickup = false
                };
                return View(o);
            }


            if(User.IsInRole("Customer")) {

                Order o = new Order()
                {
                    Username = WebSecurity.CurrentUserName,
                    OrderSubmitted = DateTime.Now,
                    Completed = false,
                    Pickup = false
                };
            
            
                o.Packages = new List<Package>
                {
                    new Package
                    {
                        OrderID = o.OrderID,
                        Priority = null,
                        Status = Status.ReadyForPickup,
                        PickupAddress = "",
                        DeliveryAddress = "",
                        Weight = 0,
                        Collected = null,
                        AssignedCourier = null, 
                        Delivered = null

                    }
                };
                /*
                Packages = db.Packages
                .Select(c => new Package
                {
                    OrderID = c.OrderID,
                    Priority = c.Priority,
                    Status = Status.ReadyForPickup,
                    PickupAddress = c.PickupAddress,
                    DeliveryAddress = c.DeliveryAddress,
                    Weight = c.Weight,
                    Collected = null,
                    AssignedCourier = null, 
                    Delivered = null
                })
                .ToList()
            
                 */
                return View(o);
            };
               return View(); 
        }


        //
        // POST: /Orders/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {

            if (ModelState.IsValid)
            {

                    if (User.IsInRole("Customer"))
                    {


                        var o = new Order
                        {
                            Username = WebSecurity.CurrentUserName,
                            OrderSubmitted = DateTime.Now,
                            Completed = false,
                            Pickup = order.Pickup
                        };

                        foreach (var p in order.Packages)
                        {
                            var pa = new Package { OrderID = o.OrderID };
                            db.Packages.Add(pa);
                        }

                        db.Orders.Add(o);
                        db.SaveChanges();

                        return RedirectToAction("Index");

                    }

                if (WebSecurity.UserExists(order.Username))
                {
                    db.Orders.Add(order);
                    db.SaveChanges();

                    return RedirectToAction("Index", "Orders");

                } else {
                    ViewBag.showError = true;
                    ModelState.AddModelError("error", "Username not found");
                }
            }

            return View(order);
        }

        //
        // GET: /Orders/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        //
        // POST: /Orders/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {

            //ViewBag.outs = outs;
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        //
        // GET: /Orders/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        //
        // POST: /Orders/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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