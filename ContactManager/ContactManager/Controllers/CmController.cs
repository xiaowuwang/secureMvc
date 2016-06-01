using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContactManager.Models;

using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;


namespace ContactManager.Controllers
{
    public class CmController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Cm
        public ActionResult Index()
        {
            ViewBag.UserID = User.Identity.GetUserId();
            return View(db.Contacts.ToList());
        }

        Contact FindContact(int? id)
        {
            Contact contact = db.Contacts.Find(id);
            if (contact == null || contact.ApplicationUser_Id != User.Identity.GetUserId())
            {
                return null;
            }
            return contact;
        }

        // GET: Cm/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.UserID = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null )
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: Cm/Create
        public ActionResult Create()
        {
            return View(new Contact
            {
                Address = "123 N 456 S",
                City = "GF",
                Email = "joe@example.com",
                Name = "Joe Smith",
                State = "MT",
                Zip = "59405"
            });
        }

        // POST: Cm/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContactId,Name,Address,City,State,Zip,Email")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.ApplicationUser_Id = User.Identity.GetUserId();
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contact);
        }

        // GET: Cm/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = FindContact(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Cm/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContactId,Name,Address,City,State,Zip,Email,ApplicationUser_Id")] Contact contact)
        {
            if (contact.ApplicationUser_Id != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: Cm/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = FindContact(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Cm/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = FindContact(id);
            if (contact == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.Contacts.Remove(contact);
            db.SaveChanges();
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
