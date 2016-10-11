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

            if (User.IsInRole("Courier"))
            {
                var packages = from p in db.Packages.Where(p => p.AssignedCourier == WebSecurity.CurrentUserName)
                               select p.Order;
                /*
                orders = from o in db.Orders.Where(o => o.OrderID )
                         //from p in packs.Where(p => p.AssignedCourier == WebSecurity.CurrentUserName)

                         select o;
                */

                orders = from p in packages
                         select p;


            }
            
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

        public Package CreatePackage(int orderID)
        {
            Package p = new Package
            {
                OrderID = orderID,
                Priority = null,
                Status = Status.ReadyForPickup,
                Weight = 0,
                Collected = null,
                AssignedCourier = null,
                Delivered = null
            };

            return p;
        }

        
        public ActionResult AddPackage(int id, int i)
        {
            if (i < 2)
            {
                i++;
            }
            return RedirectToAction("Create", new { numPack = i });
        }

        public ActionResult RemovePackage(int id, int i)
        {
            if (i > 0)
            {
                i--;
            }
            return RedirectToAction("Create", new { numPack = i });
        }

        //
        // GET: /Orders/Create

        public ActionResult Create(int? numPack)
        {

            // select list for couriers
            var cour = db.Users.Where(p => p.UserRole == "Courier");
            ViewBag.Cour = new SelectList(cour, "UserName", "UserName");
 
                Order o = new Order()
                {
                    Username = "",
                    OrderSubmitted = DateTime.Now,
                    PickupAddress = "",
                    DeliveryAddress = "",
                    Pickup = false,
                    Completed = false
                    
                };

                if (User.IsInRole("Customer"))
                {
                    o.Username = WebSecurity.CurrentUserName;
                }


                o.Packages = new List<Package>();
                o.Packages.Add(CreatePackage(o.OrderID));

                if (numPack == null)
                {
                    numPack = 0;
                } 

                ViewBag.OrderID = o.OrderID;
                ViewBag.numPack = numPack;

                for (int i = 0; i < numPack; i++)
                {
                    o.Packages.Add(CreatePackage(o.OrderID));
                }

                
                
                return View(o);
        }
        /*
        public ActionResult Packages(List<Package> packages)
        {
            // select list for couriers
            var cour = db.Users.Where(p => p.UserRole == "Courier");
            ViewBag.Cour = new SelectList(cour, "UserName", "UserName");

            return PartialView(packages);
        }
         */
        //
        // POST: /Orders/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {

            if (ModelState.IsValid)
            {

                if (WebSecurity.UserExists(order.Username))
                {

                        var o = new Order
                        {
                            Username = order.Username, 
                            OrderSubmitted = DateTime.Now,
                            PickupAddress = order.PickupAddress,
                            DeliveryAddress = order.DeliveryAddress,
                            Pickup = order.Pickup,
                            Completed = false,
                            Packages = new List<Package>()
                        };

                        //o.Packages = new List<Package>();

                        foreach (var p in order.Packages)
                        {

                            var pa = new Package { 
                                OrderID = o.OrderID,
                                Status = Status.ReadyForPickup,
                                Priority = p.Priority,
                                Weight = p.Weight,
                                AssignedCourier = p.AssignedCourier
                            };

                            o.Packages.Add(pa);
                            //db.Packages.Add(pa);
                                
                        }
                        

                       
                        db.Orders.Add(o);
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