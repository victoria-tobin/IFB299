using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using OnTheSpot.Models;
using OnTheSpot.DAL;
using System.Web.Security;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;

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


        private IEnumerable<SelectListItem> GetRoles(string r)
        {

            List<SelectListItem> roles = new List<SelectListItem>();
            roles.Add(new SelectListItem { Text = "Customer", Value = "Customer", Selected = (r == "Customer") });
            roles.Add(new SelectListItem { Text = "Courier", Value = "Courier", Selected = (r == "Courier") });
            roles.Add(new SelectListItem { Text = "Admin", Value = "Admin", Selected = (r == "Admin") });
            return new SelectList(roles, "Value", "Text");
        }


        //
        // GET: /Users/Edit/5

        public ActionResult Edit(int id = 0)
        {
            UserProfile userprofile = db.Users.Find(id);
            userprofile.UserRoles = GetRoles(userprofile.UserRole);
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

                string u = userprofile.UserName;
                string r = userprofile.UserRole;

                if (!Roles.GetUsersInRole(r).Contains(u))
                {
                    Roles.RemoveUserFromRoles(u, Roles.GetRolesForUser(u));
                    Roles.AddUserToRole(u, r);
                }

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
            if (Roles.GetRolesForUser(userprofile.UserName).Length != 0)
            {
                Roles.RemoveUserFromRoles(userprofile.UserName, Roles.GetRolesForUser(userprofile.UserName));
            }
            db.Users.Remove(userprofile);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Manage(ManageMessageId? message)
        {
            BigViewModel model = new BigViewModel();
            model.UserProfile = db.Users.Find(WebSecurity.GetUserId(User.Identity.Name));

            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");

            return View(model);
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(BigViewModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));

            int id = model.UserProfile.UserID;
            //UserProfile user = model.UserProfile; //= db.Users.Find(WebSecurity.GetUserId(User.Identity.Name));

            //UserProfile user = db.Users.Find(WebSecurity.GetUserId(User.Identity.Name)); //db.Users.Find(WebSecurity.GetUserId(User.Identity.Name));  

            //UserProfile user = db.Users.Find(WebSecurity.GetUserId(User.Identity.Name));

            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.SecurityModel.OldPassword, model.SecurityModel.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        
                        db.Entry(model.UserProfile).State = EntityState.Modified;
                        db.SaveChanges();

                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.SecurityModel.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}