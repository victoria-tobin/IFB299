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

namespace OnTheSpot.Controllers
{
    public class UsersController : Controller
    {
        private DatabaseModels db = new DatabaseModels();

        //
        // GET: /Users/

        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        //
        // GET: /Users/Details/5

        public ActionResult Details(int id = 0)
        {
            UserProfile userprofile = db.Users.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        //
        // GET: /Users/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Users/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserProfile userprofile)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(userprofile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userprofile);
        }

---------------------------------------------------------------------------------Start
        private IEnumerable<SelectListItem> GetRoles(string r)
        {

            List<SelectListItem> roles = new List<SelectListItem>();
            roles.Add(new SelectListItem { Text = "Customer", Value = "Customer", Selected = (r == "Customer") });
            roles.Add(new SelectListItem { Text = "Courier", Value = "Courier", Selected = (r == "Courier") });
            roles.Add(new SelectListItem { Text = "Admin", Value = "Admin", Selected = (r == "Admin") });
            return new SelectList(roles, "Value", "Text");
        }
---------------------------------------------------------------------------------Finish

        //
        // GET: /Users/Edit/5

        public ActionResult Edit(int id = 0)
        {
            UserProfile userprofile = db.Users.Find(id);
            ***userprofile.UserRoles = GetRoles(userprofile.UserRole);***
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        //
        // POST: /Users/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserProfile userprofile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userprofile).State = EntityState.Modified;
---------------------------------------------------------------------------------Start
                string u = userprofile.UserName;
                string r = userprofile.UserRole;

                if (!Roles.GetUsersInRole(r).Contains(u))
                {
                    Roles.RemoveUserFromRoles(u, Roles.GetRolesForUser(u));
                    Roles.AddUserToRole(u, r);
                }
---------------------------------------------------------------------------------Finish
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userprofile);
        }

        //
        // GET: /Users/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UserProfile userprofile = db.Users.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        //
        // POST: /Users/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserProfile userprofile = db.Users.Find(id);
---------------------------------------------------------------------------------Start
            if (Roles.GetRolesForUser(userprofile.UserName).Length != 0)
            {
                Roles.RemoveUserFromRoles(userprofile.UserName, Roles.GetRolesForUser(userprofile.UserName));
            }
            db.Users.Remove(userprofile);
---------------------------------------------------------------------------------Finish
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