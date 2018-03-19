using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using System.Text;

namespace WebApplication2.Controllers
{
    public class StrategyController : Controller
    {
        private sjassoc_dbEntities db = new sjassoc_dbEntities();

        // GET: Todoes
        public ActionResult Index(string group, string prin, string osr, string status, string groupnew, string stratvar, string fltstring, Strategy selg, FormCollection form, string OSRcmb = null)
        {

            if (Session["UserId"] != null)
            {
                Strategy strat = new Strategy();

                int id = Int32.Parse(Session["UserId"].ToString()); // Get the user id from the session
                String em = db.UserAccounts.Find(id).Email.ToString(); // Use the id to get the associated email address
                EmailList emailListItem = db.EmailLists.First(x => x.Email == em); // Use the email address to get the associated emaillist object which holds the group

                string perm = emailListItem.Perm;

                if (perm == null)
                {
                    perm = "0";
                }
                ViewData["perm"] = perm;

                // if external
                if (!emailListItem.IntExt)
                {
                    // Create a list to hold the Todos which we will end up showing
                    List<Strategy> list = new List<Strategy>();

                    // this is a foreach loop, it goes through all the Todos returned from db.Todoes.ToList()
                    foreach (Strategy s in db.Strategies.ToList())
                    {
                        // makes sure that the group of a todo isn't null (empty)
                        if (!String.IsNullOrEmpty(s.Group))
                        {
                            // checks if the group of the user is equal to the group of the post. if so it adds the todo to the list.
                            if (emailListItem.Group.Equals(s.Group))
                            {
                                list.Add(s);
                            }
                        }
                    }

                    return View(list);

                }
                else
                {
                    // This is the query building code. 
                    string p = emailListItem.Perm;
                    string gr = emailListItem.Group;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT * FROM dbo.Strategy WHERE "); //change table name for whatever you need returned
                    foreach (char c in p.ToCharArray())
                    {
                        if (group == null)
                        {
                            sb.Append("Perm LIKE '%");
                            sb.Append(c);
                            sb.Append("%' OR ");
                        }

                    }
                    sb.Length = sb.Length - 4;


                    if (selg == null)
                    {

                        List<Strategy> list = db.Strategies.SqlQuery(sb.ToString()).ToList(); //change table name
                        return View(list);
                    }

                    else
                    {
                        //var selectedval = form["group"];

                        //var selectedvalpr = form["prin"];

                        //var selectedvalosr = form["osr"];

                        //var selectedvalstatus = form["status"];


                        var groups = from g in db.Strategies
                                     select g;
                        var prins = from pr in db.Strategies
                                    select pr;
                        var osrs = from o in db.Strategies
                                   select o;
                        var statuss = from s in db.Strategies
                                      select s;

                        ViewBag.Groupcmb = (from g in db.Strategies.Include(p)
                                            where g.Group != null
                                            select g.Group).Distinct();


                        ViewBag.Principalcmb = (from pr in db.Strategies.Include(p)
                                                where pr.Principal != null
                                                select pr.Principal).Distinct();

                        ViewBag.OSRcmb = (from o in db.Strategies.Include(p)
                                          where o.OSR != null
                                          select o.OSR).Distinct();

                        ViewBag.Statuscmb = (from s in db.Strategies.Include(p)
                                             where s.Status != null
                                             select s.Status).Distinct();

                        //groups = groups.Where(g => g.Group.Contains(group));
                        //prins = groups.Where(pr => pr.Principal.Contains(prin));

                        //if all filters are null
                        if (group == null && stratvar == null && prin == null && osr == null && status == null)
                        {
                            return View(db.Strategies.ToList());
                        }

                        //returns same search filter for group if edit 
                        if (stratvar != null && group == null)
                        {

                            group = stratvar;
                            groups = groups.Where(g => g.Group.Contains(group));
                            //  return View(group.ToList());
                        };



                        //if (prin != null && group != null && osr != null && status != null)
                        if (prin != null && group != null && osr != null && status != null)
                        {
                            prins = prins.Where(gpr => gpr.Principal.Contains(prin) && gpr.Group.Contains(group) && gpr.OSR.Contains(osr) && gpr.Status.Contains(status));
                            //fltstring = "gpr.Group.Contains(group)";
                            //prins = prins.Where(gpr => gpr.Group.Contains(group));
                            //  prins = prins.Where(gpr => gpr.Group.Contains(group));
                            stratvar = null;
                            //return View(db.Strategies.ToList());
                            return View(prins.ToList());
                        }






























              


                        return View(db.Strategies.ToList());
                    }

                }

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }





        public ActionResult Details(int? id)
        {

            if (Session["UserId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Strategy strat = db.Strategies.Find(id);
                if (strat == null)
                {
                    return HttpNotFound();
                }
                return View(strat);
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
                var model = new Strategy();
                model.CreateDate = DateTime.Now;

                var options = new List<Strategy>();
                options.Add(new Strategy() { Status = "New Request", Text = "New Request" });
                options.Add(new Strategy() { Status = "Reviewed", Text = "Reviewed" });
                options.Add(new Strategy() { Status = "Started", Text = "Started" });
                options.Add(new Strategy() { Status = "In Progress", Text = "In Progress" });
                options.Add(new Strategy() { Status = "Completed", Text = "Completed" });

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
        public ActionResult Create([Bind(Include = "Customer,EndProduct,Product,Status,NextAction,History,CreateDate,Updated,FollowUpDate,ManagerComment,OSR,Principal,Value,Group")] Strategy strat)
        {

            if (Session["UserId"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Strategies.Add(strat);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(strat);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }



        public ActionResult Edit(int? id, string groupflt)
        {

            if (Session["UserId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Strategy strat = db.Strategies.Find(id);
                if (strat == null)
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


                return View(strat);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StrategyId,Customer,EndProduct,Product,Status,NextAction,History,CreateDate,Updated,FollowUpDate,ManagerComment,OSR,Principal,Value,Group")] Strategy strat, string stratvar)
        {

            if (Session["UserId"] != null)
            {
                var stratvariable = from s in db.Strategies
                                    select s;
                stratvar = strat.ToString();
                stratvariable = stratvariable.Where(s => s.Group.Contains(stratvar));
                //document.getElementById('FollowUp').value=
                if (ModelState.IsValid)
                {
                    db.Entry(strat).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { stratvar = strat.Group });
                    //return View("Index", stratvar);
                }

                return View(stratvar, stratvar);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }


    }
}