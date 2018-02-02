using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class TodoesController : Controller
    {
        private sjassoc_dbEntities db = new sjassoc_dbEntities();



        // GET: Todoes
        public ActionResult Index()
        {

            if (Session["UserId"] != null)
            {
                return View(db.Todoes.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        // GET: Todoes/Details/5
        public ActionResult Details(int? id)
        {

            if (Session["UserId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Todo todo = db.Todoes.Find(id);
                if (todo == null)
                {
                    return HttpNotFound();
                }
                return View(todo);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        // GET: Todoes/Create
        public ActionResult Create()
        {

            if (Session["UserId"] != null)
            {
                var model = new Todo();
                model.CreatedDate = DateTime.Now;

                var options = new List<Todo>();
                options.Add(new Todo() { Status = "New Request", Text = "New Request" });
                options.Add(new Todo() { Status = "Reviewed", Text = "Reviewed" });
                options.Add(new Todo() { Status = "Started", Text = "Started" });
                options.Add(new Todo() { Status = "In Progress", Text = "In Progress" });
                options.Add(new Todo() { Status = "Completed", Text = "Completed" });

                ViewBag.Status = options;
                return View(model);

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }

        // POST: Todoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Description,CreatedDate,Task,FollowUp,Status,Group")] Todo todo)
        {

            if (Session["UserId"] != null)
            {
                if (ModelState.IsValid)
                {
                    //var val = todo.Val.ToString();
                    //todo.Status = val;

                    //todo.Val = todo.Status;
                    db.Todoes.Add(todo);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(todo);
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
                Todo todo = db.Todoes.Find(id);
                if (todo == null)
                {
                    return HttpNotFound();
                }


                var options = new List<Todo>();
                options.Add(new Todo() { Status = "New Request", Text = "New Request" });
                options.Add(new Todo() { Status = "Reviewed", Text = "Reviewed" });
                options.Add(new Todo() { Status = "Started", Text = "Started" });
                options.Add(new Todo() { Status = "In Progress", Text = "In Progress" });
                options.Add(new Todo() { Status = "Completed", Text = "Completed" });


                ViewBag.Status = options;


                return View(todo);
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
        public ActionResult Edit([Bind(Include = "ID,Description,CreatedDate,Task,Status,FollowUp,Group")] Todo todo)
        {

            if (Session["UserId"] != null)
            {

                //document.getElementById('FollowUp').value=
                if (ModelState.IsValid)
                {
                    //var val = todo.Val.ToString();
                    //todo.Status = val;
                    //todo.Val = todo.Status;
                    db.Entry(todo).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(todo);
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
                Todo todo = db.Todoes.Find(id);
                if (todo == null)
                {
                    return HttpNotFound();
                }
                return View(todo);
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
                Todo todo = db.Todoes.Find(id);
                db.Todoes.Remove(todo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login", "Account");
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
