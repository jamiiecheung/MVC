using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Models
{
    public class OpptysController : Controller
    {

        private sjassoc_dbEntities db = new sjassoc_dbEntities();

        // GET: Opptys
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                return View(db.Opptys1.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


        public ActionResult Create()
        {
            if (Session["UserId"] != null)
            {
                var model = new Opptys();

                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Customer,Principal,Engineer,Status,Updated,Phone_Number,Email,Program,Application,Part_Number,EAU,Price,Units_Per_Board,Decision_Date,Production_Date,CM,CM_Location,Distributor,Disty_Email,Disty_Phone_Number,Factory_Email,Factory_Phone_Number")] Opptys oppt)
        {

            if (Session["UserId"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Opptys1.Add(oppt);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(oppt);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }






        // GET: Todoes/Edit/5
        public ActionResult Edit(int? id)
        {

            if (Session["UserId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Opptys oppt = db.Opptys1.Find(id);
                if (oppt == null)
                {
                    return HttpNotFound();
                }


                return View(oppt);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        // POST: Todoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Customer,Principal,Engineer,Status,Updated,Phone_Number,Email,Program,Application,Part_Number,EAU,Price,Units_Per_Board,Decision_Date,Production_Date,CM,CM_Location,Distributor,Disty_Email,Disty_Phone_Number,Factory_Email,Factory_Phone_Number")] Opptys oppt)
        {

            if (Session["UserId"] != null)
            {

                if (ModelState.IsValid)
                {
                    db.Entry(oppt).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(oppt);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }

        // GET: Todoes/Delete/5
        public ActionResult Delete(int? id)
        {

            if (Session["UserId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Opptys oppt = db.Opptys1.Find(id);
                if (oppt == null)
                {
                    return HttpNotFound();
                }
                return View(oppt);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }

        // POST: Todoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            if (Session["UserId"] != null)
            {
                Opptys oppt = db.Opptys1.Find(id);
                db.Opptys1.Remove(oppt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }





    }
}