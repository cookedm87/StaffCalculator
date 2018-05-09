using WageCalculator.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Web.Helpers;
using System.Text;
using System.Web.Script.Serialization;
using System.Data.SqlClient;

namespace WageCalculator.Controllers
{ 
    public class HomeController : Controller
    {

        private StaffCalculatorEntities3 db = new StaffCalculatorEntities3();
        //GET: StaffMembers
        public ActionResult Index(string staffMember, string searchString)
        {
            var genreList = new List<string>();
            var NameQuery = from s in db.StaffMembers
                            orderby s.Name
                            select s.Name;
            genreList.AddRange(NameQuery.Distinct());
            ViewBag.staffMember = new SelectList(genreList);
            //LINQ query to get all movies
            var staff = from m in db.StaffMembers
                        select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                staff = staff.Where(x => x.Name.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(staffMember))
            {
                staff = staff.Where(x => x.Name == staffMember);
            }

            return View(staff);
        }
        //GET: StaffMembers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffMember staff = db.StaffMembers.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        //GET: StaffMembers/Create
        public ActionResult Create()
        {
            return View();
        }
        //POST: StaffMembers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name, HourlyRate, MonHours, TueHours, WedHours, ThuHours, FriHours, SatHours, SunHours")]StaffMember staff)
        {
            if (ModelState.IsValid)
            {
                db.StaffMembers.Add(staff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staff);
        }
        //GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffMember staff = db.StaffMembers.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        //POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, Name, HourlyRate, MonHours, TueHours, WedHours, ThuHours, FriHours, SatHours, SunHours")] StaffMember staff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staff);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffMember staff = db.StaffMembers.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StaffMember staff = db.StaffMembers.Find(id);
            db.StaffMembers.Remove(staff);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        //GET: /Home/
        public JsonResult GetStaff()
        {
            using (db)
            {
                var result = (from s in db.StaffMembers
                              orderby s.Name ascending
                              select new { s.Name, s.TotalPay, s.MonPay, s.TuePay, s.WedPay, s.ThuPay, s.FriPay, s.SatPay, s.SunPay }).ToList();
                return Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
            }
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