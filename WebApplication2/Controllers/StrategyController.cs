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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
//using Microsoft.Office.Interop.Outlook;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Data.Entity.Validation;
//using Outlook;
//using OutLookApp = Outlook.Application;

namespace WebApplication2.Controllers
{
    public class StrategyController : Controller
    {
        private sjassoc_dbEntities db = new sjassoc_dbEntities();

        // GET: Todoes
        public ActionResult Index(string Groupddl, string Statusddl, string Prinddl, string OSRddl, string Customerddl, string osrsel, string prinsel, string statussel, string groupsel, string customersel,
            string statusdrop, string groupnew, string sortOrder, string customerName, string stratvar, string fltstring, Strategy selg, FormCollection form)
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


                    ViewBag.CustomerSort = sortOrder == "Name" ? "Name_desc" : "Name";

                    var model = from t in list
                                select t;


                    switch (sortOrder)
                    {
                        case "Name_desc":
                            model = model.OrderByDescending(t => t.Customer);
                            break;
                        case "Name":
                            model = model.OrderBy(t => t.Customer);
                            break;
                    }




                    string p = emailListItem.Perm;
                    string gr = emailListItem.Group;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT * FROM dbo.Strategy WHERE "); //change table name for whatever you need returned
                    foreach (char c in p.ToCharArray())
                    {
                        if (Groupddl == null)
                        {
                            sb.Append("Perm LIKE '%");
                            sb.Append(c);
                            sb.Append("%' OR ");
                        }

                    }
                    sb.Length = sb.Length - 4;


                    if (selg == null)
                    {
                        List<Strategy> list1 = db.Strategies.SqlQuery(sb.ToString()).ToList(); //change table name
                        return View(list1);
                    }

                    else
                    {
                        var groups = from g in list
                                     select g;
                        var prins = from pr in list
                                    select pr;
                        /*  var osrs = from o in db.Strategies
                                     select o;
                          var statuss = from s in db.Strategies
                                        select s; */



                        List<SelectListItem> groupListItems = list.Select(w => w.Group).Where(g => g != null).Distinct().Select(g => new SelectListItem { Value = g, Text = g }).ToList();
                        ViewBag.Groupddl = new SelectList(groupListItems, "Value", "Text").Distinct();

                        List<SelectListItem> prinListItems = list.Select(w => w.Principal).Where(pr => pr != null).Distinct().Select(pr => new SelectListItem { Value = pr, Text = pr }).ToList();
                        ViewBag.Prinddl = new SelectList(prinListItems, "Value", "Text").Distinct();

                        List<SelectListItem> osrListItems = list.Select(w => w.OSR).Where(o => o != null).Distinct().Select(o => new SelectListItem { Value = o, Text = o }).ToList();
                        ViewBag.OSRddl = new SelectList(osrListItems, "Value", "Text").Distinct();

                        List<SelectListItem> statusListItems = list.Select(w => w.Status).Where(g => g != null).Distinct().Select(g => new SelectListItem { Value = g, Text = g }).ToList();
                        ViewBag.Statusddl = new SelectList(statusListItems, "Value", "Text").Distinct();

                        List<SelectListItem> customerListItems = list.Select(w => w.Customer).Where(c => c != null).Distinct().Select(c => new SelectListItem { Value = c, Text = c }).ToList();
                        ViewBag.Customerddl = new SelectList(customerListItems, "Value", "Text").Distinct();



                        //List<SelectListItem> groupListItems = list.Where(w => w.Group != null).Select(group => new SelectListItem { Value = group.Group, Text = group.Group }).Distinct().ToList();
                        //ViewBag.Groupddl = new SelectList(groupListItems, "Value", "Text").Distinct();

                        //List<SelectListItem> prinListItems = list.Where(w => w.Principal != null).Select(prin => new SelectListItem { Value = prin.Principal, Text = prin.Principal }).Distinct().ToList();
                        //ViewBag.Prinddl = new SelectList(prinListItems, "Value", "Text").Distinct();

                        //List<SelectListItem> osrListItems = list.Where(w => w.OSR != null).Select(osr => new SelectListItem { Value = osr.OSR, Text = osr.OSR }).Distinct().ToList();
                        //ViewBag.OSRddl = new SelectList(osrListItems, "Value", "Text").Distinct();

                        //List<SelectListItem> statusListItems = list.Where(w => w.Status != null).Select(status => new SelectListItem { Value = status.Status, Text = status.Status }).Distinct().ToList();
                        //ViewBag.Statusddl = new SelectList(statusListItems, "Value", "Text").Distinct();

                        //List<SelectListItem> customerListItems = list.Where(w => w.Customer != null).Select(cust => new SelectListItem { Value = cust.Customer, Text = cust.Customer }).Distinct().ToList();
                        //ViewBag.Customerddl = new SelectList(customerListItems, "Value", "Text").Distinct();


                        //if all filters are null
                        if (Groupddl == null && stratvar == null && Prinddl == null && OSRddl == null && Statusddl == null && Customerddl == null)
                        {
                            if (sortOrder == null)
                            {
                                return View(model.ToList());
                            }
                            else
                            {
                                return View(model.ToList());
                            }
                        }


                        //if (prin != null && group != null && osr != null && status != null)
                        if (Prinddl != null && Groupddl != null && OSRddl != null && Statusddl != null && Customerddl != null)
                        {
                            prins = prins.Where(gpr => gpr.Principal.Contains(Prinddl) && gpr.Group.Contains(Groupddl) && gpr.OSR.Contains(OSRddl) && gpr.Status.Contains(Statusddl) && gpr.Customer.Contains(Customerddl));
                            Session["filtprins"] = Prinddl;
                            Session["filtgroup"] = Groupddl;
                            Session["filtstatus"] = Statusddl;
                            Session["filtosr"] = OSRddl;
                            Session["filtcustomer"] = Customerddl;

                            stratvar = null;

                            return View(prins.ToList());
                        }


                        return View(model);
                    }
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
                        if (Groupddl == null)
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
                        var groups = from g in db.Strategies
                                     select g;
                        var prins = from pr in db.Strategies
                                    select pr;
                        /*  var osrs = from o in db.Strategies
                                     select o;
                          var statuss = from s in db.Strategies
                                        select s; */

                        List<SelectListItem> groupListItems = db.Strategies.Where(w => w.Group != null).Select(group => new SelectListItem { Value = group.Group, Text = group.Group }).Distinct().ToList();
                        ViewBag.Groupddl = new SelectList(groupListItems, "Value", "Text").Distinct();

                        List<SelectListItem> prinListItems = db.Strategies.Where(w => w.Principal != null).Select(prin => new SelectListItem { Value = prin.Principal, Text = prin.Principal }).Distinct().ToList();
                        ViewBag.Prinddl = new SelectList(prinListItems, "Value", "Text").Distinct();

                        List<SelectListItem> osrListItems = db.Strategies.Where(w => w.OSR != null).Select(osr => new SelectListItem { Value = osr.OSR, Text = osr.OSR }).Distinct().ToList();
                        ViewBag.OSRddl = new SelectList(osrListItems, "Value", "Text").Distinct();

                        List<SelectListItem> statusListItems = db.Strategies.Where(w => w.Status != null).Select(status => new SelectListItem { Value = status.Status, Text = status.Status }).Distinct().ToList();
                        ViewBag.Statusddl = new SelectList(statusListItems, "Value", "Text").Distinct();

                        List<SelectListItem> customerListItems = db.Strategies.Where(w => w.Customer != null).Select(cust => new SelectListItem { Value = cust.Customer, Text = cust.Customer }).Distinct().ToList();
                        ViewBag.Customerddl = new SelectList(customerListItems, "Value", "Text").Distinct();


                        // Convert sort order
                        ViewBag.CustomerSort = sortOrder == "Name" ? "Name_desc" : "Name";



                        var model = from t in db.Strategies
                                    select t;



                        switch (sortOrder)
                        {
                            case "Name_desc":
                                model = model.OrderByDescending(t => t.Customer);
                                break;
                            case "Name":
                                model = model.OrderBy(t => t.Customer);
                                break;
                        }
                        //}
                        //if all filters are null
                        if (Groupddl == null && stratvar == null && Prinddl == null && OSRddl == null && Statusddl == null && Customerddl == null)
                        {
                            if (sortOrder == null)
                            {
                                return View(db.Strategies.ToList());
                            }
                            else
                            {
                                return View(model.ToList());
                            }
                        }

                        ////returns same search filter if a strategy was selected beforehand
                        ////if (stratvar != null)
                        //    if (stratvar != null)
                        //    {

                        //    if (Prinddl == null)//checks if there is a strategy already selected
                        //    {
                        //        //set the filters to the sessions
                        //        //if (Prinddl == null)
                        //        //{
                        //        //    Prinddl = "";
                        //        //    Session["fltprins"] = "";
                        //        //}
                        //        Prinddl = Session["filtprins"].ToString();
                        //        Groupddl = Session["filtgroup"].ToString();
                        //        Statusddl = Session["filtstatus"].ToString();
                        //        OSRddl = Session["filtosr"].ToString();
                        //    }
                        //    //  return View(group.ToList());

                        //    stratvar = null;
                        //};


                        //if (prin != null && group != null && osr != null && status != null)
                        if (Prinddl != null && Groupddl != null && OSRddl != null && Statusddl != null && Customerddl != null)
                        {
                            prins = prins.Where(gpr => gpr.Principal.Contains(Prinddl) && gpr.Group.Contains(Groupddl) && gpr.OSR.Contains(OSRddl) && gpr.Status.Contains(Statusddl) && gpr.Customer.Contains(Customerddl));
                            Session["filtprins"] = Prinddl;
                            Session["filtgroup"] = Groupddl;
                            Session["filtstatus"] = Statusddl;
                            Session["filtosr"] = OSRddl;
                            Session["filtcustomer"] = Customerddl;

                            stratvar = null;

                            return View(prins.ToList());
                        }
                        //return View(prins.ToList());
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

                var options = new List<Strategy>();

                options.Add(new Strategy() { Status = "New Request", Text = "New Request" });
                options.Add(new Strategy() { Status = "Reviewed", Text = "Reviewed" });
                options.Add(new Strategy() { Status = "Started", Text = "Started" });
                options.Add(new Strategy() { Status = "In Progress", Text = "In Progress" });
                options.Add(new Strategy() { Status = "Completed", Text = "Completed" });

                ViewBag.Status = options;
                var x = strat.Status;
                var p = strat.Perm;
                TempData["statussel"] = x;
                TempData["perm"] = p;


                int userid = Int32.Parse(Session["UserId"].ToString()); // Get the user id from the session
                String emailacc = db.UserAccounts.Find(userid).Email.ToString(); // Use the id to get the associated email address
                string osrid = strat.OSR; // Get the OSR initials
                UserAccount item = db.UserAccounts.FirstOrDefault(i => i.OSR == osrid); // Get all the user account information based that matches the osrid
                ViewBag.EmailTo = item.Email;
                strat.Updated = DateTime.Now;
                //string updatedate = strat.Updated.ToString("MM/dd/yy");


                //strat.Value = 


                string body = "";

                if (strat.FollowUpDate == null)
                {
                    body = "%0D%0A" + "Create Date: " + strat.CreateDate.ToString("MM/dd/yy") + "%0D%0A" + "Updated: " + strat.Updated.Value.ToString("MM/dd/yy") + "%0D%0A" + "Customer: " + strat.Customer + "%0D%0A" + "End Product: " + strat.EndProduct + "%0D%0A" + "OSR: " + strat.OSR + "%0D%0A" + "Principal: " + strat.Principal + "%0D%0A" + "Product: " + strat.Product + "%0D%0A" + "Followup Date: " + "%0D%0A" + "Value: " + strat.Value + "%0D%0A" + "Status: " + strat.Status + "%0D%0A" + "Next Action: " + strat.NextAction + "%0D%0A" + "Latest Comments: " + strat.ManagerComment + "%0D%0A" + "History: " + strat.History + "%0D%0A" + "Group: " + strat.Group;
                }
                else
                {
                    body = "%0D%0A" + "Create Date: " + strat.CreateDate.ToString("MM/dd/yy") + "%0D%0A" + "Updated: " + strat.Updated.Value.ToString("MM/dd/yy") + "%0D%0A" + "Customer: " + strat.Customer + "%0D%0A" + "End Product: " + strat.EndProduct + "%0D%0A" + "OSR: " + strat.OSR + "%0D%0A" + "Principal: " + strat.Principal + "%0D%0A" + "Product: " + strat.Product + "%0D%0A" + "Followup Date: " + strat.FollowUpDate.Value.ToString("MM/dd/yy") + "%0D%0A" + "Value: " + strat.Value + "%0D%0A" + "Status: " + strat.Status + "%0D%0A" + "Next Action: " + strat.NextAction + "%0D%0A" + "Latest Comments: " + strat.ManagerComment + "%0D%0A" + "History: " + strat.History + "%0D%0A" + "Group: " + strat.Group;
                }

                ViewBag.Body = body;
                ViewBag.Subject = "SJ App Request - Customer: " + strat.Customer + " Principal: " + strat.Principal + " Product: " + strat.Product;


                if (strat.ManagerComment == null)
                {
                    strat.Updated = DateTime.Now;
                    strat.ManagerComment = strat.Updated.Value.ToString("MM/dd/yy");
                    db.Entry(strat).State = EntityState.Modified;

                    if (strat.ManagerComment.Length <= 10 && strat.ManagerComment != null)
                    {
                        strat.Updated = DateTime.Now;
                        strat.ManagerComment = strat.Updated.Value.ToString("MM/dd/yy");
                        db.Entry(strat).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        strat.Updated = DateTime.Now;
                    }
                }
                db.SaveChanges();

                return View(strat);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StrategyId,Customer,EndProduct,Product,Status,NextAction,History,CreateDate,Updated,FollowUpDate,ManagerComment,OSR,Principal,Value,Group,Perm")] Strategy strat, string stratvar, string Groupddl, string Statusddl, string Prinddl, string OSRddl)
        {
            if (Session["UserId"] != null)
            {
                var stratvariable = from s in db.Strategies
                                    select s;
                
                stratvar = strat.ToString();

             

                if (stratvar != null)
                {
                    ViewBag.Val = strat.Value;

                }

                stratvariable = stratvariable.Where(s => s.Group.Contains(stratvar));

                var options = new List<Strategy>();

                options.Add(new Strategy() { Status = "New Request", Text = "New Request" });
                options.Add(new Strategy() { Status = "Reviewed", Text = "Reviewed" });
                options.Add(new Strategy() { Status = "Started", Text = "Started" });
                options.Add(new Strategy() { Status = "In Progress", Text = "In Progress" });
                options.Add(new Strategy() { Status = "Completed", Text = "Completed" });

                ViewBag.Status = options;

                //int id = Int32.Parse(Session["UserId"].ToString()); // Get the user id from the session
                //String emailacc = db.UserAccounts.Find(id).Email.ToString(); // Use the id to get the associated email address
                //ViewBag.EmailTo = emailacc;

                //var x = strat.Status;
                //strat.Status = TempData["statussel"].ToString();
                //strat.Perm = TempData["perm"].ToString();

                if (ModelState.IsValid)
                {
                    //Prinddl = Session["filtprins"].ToString();
                    //Groupddl = Session["filtgroup"].ToString();
                    //Statusddl = Session["filtstatus"].ToString();
                    //OSRddl = Session["filtosr"].ToString();

                    if (strat.ManagerComment == null)
                    {
                        strat.Updated = DateTime.Now;
                        strat.ManagerComment = strat.Updated.Value.ToString("MM/dd/yy");

                        db.Entry(strat).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    else if (strat.ManagerComment.Length <= 10 && strat.ManagerComment != null)
                    {
                        strat.Updated = DateTime.Now;
                        //strat.ManagerComment = null;
                        //db.SaveChanges();
                        strat.ManagerComment = strat.Updated.Value.ToString("MM/dd/yy");

                        db.Entry(strat).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    //else if (strat.ManagerComment.Length > 1000)
                    //{
                    //    ModelState.AddModelError("", "The Length of manager's comments is too many characters.");

                    //    return View();
                    //}

                    else
                    {
                        try
                        {
                        strat.Updated = DateTime.Now;
                        db.Entry(strat).State = EntityState.Modified;
                        db.SaveChanges();
                        }

                        catch (DbEntityValidationException ex)
                        {
                            foreach (var errors in ex.EntityValidationErrors)
                            {
                                foreach (var validationError in errors.ValidationErrors)
                                {
                                    // get the error message 
                                    string errorMessage = validationError.ErrorMessage;
                                }
                            }
                        }



                    }

                    db.Entry(strat).State = EntityState.Modified;
                    db.SaveChanges();
                    //return View(strat);
                    //return RedirectToAction("Index", new { stratvar = strat.Group });
                }

                if (stratvar != null && ViewBag.Val != null)
                {
                    strat.Value = ViewBag.Val;
                }

                db.Entry(strat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { stratvar = strat.Group });
                // return View(strat);
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

        // POST: Todoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserId"] != null)
            {
                Strategy strat = db.Strategies.Find(id);
                db.Strategies.Remove(strat);
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







        public ActionResult Update(int? id, string commentold, [Bind(Include = "ManagerComment")] Strategy strat, string stratvar)
        {
            strat = db.Strategies.FirstOrDefault(x => x.StrategyId == id);
            int uid = Int32.Parse(Session["UserId"].ToString()); // Get the user id from the session
            String em = db.UserAccounts.Find(uid).Email.ToString(); // Use the id to get the associated email address
            UserAccount emailListItem = db.UserAccounts.First(x => x.Email == em); // Use the email address to get the associated emaillist object which holds the group
            commentold = strat.ManagerComment;
            strat.Updated = DateTime.Now;

            if (strat.ManagerComment == null)
            {
                strat.ManagerComment = strat.Updated.Value.ToString("MM/dd/yy");
                db.Entry(strat).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                strat.ManagerComment = strat.Updated.Value.ToString("MM/dd/yy"); ;
                strat.History = commentold + "\r\n" + strat.History;
                db.Entry(strat).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Edit", "Strategy", new { id });
        }


        public ActionResult MeetingSch(int? id, string nextactionold)
        {
            int uid = Int32.Parse(Session["UserId"].ToString()); // Get the user id from the session
            String em = db.UserAccounts.Find(uid).Email.ToString(); // Use the id to get the associated email address
            UserAccount emailListItem = db.UserAccounts.First(x => x.Email == em); // Use the email address to get the associated emaillist object which holds the group

            Strategy strat = db.Strategies.FirstOrDefault(x => x.StrategyId == id);
            nextactionold = strat.NextAction;
            strat.Updated = DateTime.Now;

            if (strat.NextAction == null)
            {
                strat.NextAction = "Meeting Scheduled";
                db.Entry(strat).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                strat.NextAction = "Meeting Scheduled";
                strat.History = strat.Updated.Value.ToString("MM/dd/yy") + " (" + emailListItem.FirstName + " " + emailListItem.LastName + ") :" + nextactionold + "\r\n" + strat.History;
                db.Entry(strat).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Edit", "Strategy", new { id });
        }

        public ActionResult SchVisit(int? id, string nextactionold)
        {
            int uid = Int32.Parse(Session["UserId"].ToString()); // Get the user id from the session
            String em = db.UserAccounts.Find(uid).Email.ToString(); // Use the id to get the associated email address
            UserAccount emailListItem = db.UserAccounts.First(x => x.Email == em); // Use the email address to get the associated emaillist object which holds the group

            Strategy strat = db.Strategies.FirstOrDefault(x => x.StrategyId == id);
            nextactionold = strat.NextAction;
            strat.Updated = DateTime.Now;

            if (strat.NextAction == null)
            {
                strat.NextAction = "Need to schedule visit";
                db.Entry(strat).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                strat.NextAction = "Need to schedule visit";
                strat.History = strat.Updated.Value.ToString("MM/dd/yy") + " (" + emailListItem.FirstName + " " + emailListItem.LastName + ") :" + nextactionold + "\r\n" + strat.History;

                db.Entry(strat).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Edit", "Strategy", new { id });
        }

        public ActionResult ScheMeet(int? id, string nextactionold)
        {
            int uid = Int32.Parse(Session["UserId"].ToString()); // Get the user id from the session
            String em = db.UserAccounts.Find(uid).Email.ToString(); // Use the id to get the associated email address
            UserAccount emailListItem = db.UserAccounts.First(x => x.Email == em); // Use the email address to get the associated emaillist object which holds the group

            Strategy strat = db.Strategies.FirstOrDefault(x => x.StrategyId == id);
            nextactionold = strat.NextAction;
            strat.Updated = DateTime.Now;

            if (strat.NextAction == null)
            {
                strat.NextAction = "Need to schedule eMeeting";
                db.Entry(strat).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                strat.NextAction = "Need to schedule eMeeting";
                strat.History = strat.Updated.Value.ToString("MM/dd/yy") + " (" + emailListItem.FirstName + " " + emailListItem.LastName + ") :" + nextactionold + "\r\n" + strat.History;
                db.Entry(strat).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Edit", "Strategy", new { id });
        }

        public ActionResult NoAct(int? id, string nextactionold)
        {
            int uid = Int32.Parse(Session["UserId"].ToString()); // Get the user id from the session
            String em = db.UserAccounts.Find(uid).Email.ToString(); // Use the id to get the associated email address
            UserAccount emailListItem = db.UserAccounts.First(x => x.Email == em); // Use the email address to get the associated emaillist object which holds the group

            Strategy strat = db.Strategies.FirstOrDefault(x => x.StrategyId == id);
            nextactionold = strat.NextAction;
            strat.Updated = DateTime.Now;

            if (strat.NextAction == null)
            {
                strat.NextAction = "No action at this time, follow up planned";
                db.Entry(strat).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                strat.NextAction = "No action at this time, follow up planned";
                strat.History = strat.Updated.Value.ToString("MM/dd/yy") + " (" + emailListItem.FirstName + " " + emailListItem.LastName + ") :" + nextactionold + "\r\n" + strat.History;
                db.Entry(strat).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Edit", "Strategy", new { id });
        }

        public ActionResult SampReq(int? id, string nextactionold)
        {
            int uid = Int32.Parse(Session["UserId"].ToString()); // Get the user id from the session
            String em = db.UserAccounts.Find(uid).Email.ToString(); // Use the id to get the associated email address
            UserAccount emailListItem = db.UserAccounts.First(x => x.Email == em); // Use the email address to get the associated emaillist object which holds the group

            Strategy strat = db.Strategies.FirstOrDefault(x => x.StrategyId == id);
            nextactionold = strat.NextAction;
            strat.Updated = DateTime.Now;

            if (strat.NextAction == null)
            {
                strat.NextAction = "Samples Requested";
                db.Entry(strat).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                strat.NextAction = "Samples Requested";
                strat.History = strat.Updated.Value.ToString("MM/dd/yy") + " (" + emailListItem.FirstName + " " + emailListItem.LastName + ") :" + nextactionold + "\r\n" + strat.History;
                db.Entry(strat).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Edit", "Strategy", new { id });
        }

        public ActionResult EvalSamp(int? id, string nextactionold)
        {
            int uid = Int32.Parse(Session["UserId"].ToString()); // Get the user id from the session
            String em = db.UserAccounts.Find(uid).Email.ToString(); // Use the id to get the associated email address
            UserAccount emailListItem = db.UserAccounts.First(x => x.Email == em); // Use the email address to get the associated emaillist object which holds the group

            Strategy strat = db.Strategies.FirstOrDefault(x => x.StrategyId == id);
            nextactionold = strat.NextAction;
            strat.Updated = DateTime.Now;

            if (strat.NextAction == null)
            {
                strat.NextAction = "Evaluating Samples";
                db.Entry(strat).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                strat.NextAction = "Evaluating Samples";
                strat.History = strat.Updated.Value.ToString("MM/dd/yy") + " (" + emailListItem.FirstName + " " + emailListItem.LastName + ") :" + nextactionold + "\r\n" + strat.History;
                db.Entry(strat).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Edit", "Strategy", new { id });
        }

        public ActionResult ConfirmCEM(int? id, string nextactionold)
        {
            int uid = Int32.Parse(Session["UserId"].ToString()); // Get the user id from the session
            String em = db.UserAccounts.Find(uid).Email.ToString(); // Use the id to get the associated email address
            UserAccount emailListItem = db.UserAccounts.First(x => x.Email == em); // Use the email address to get the associated emaillist object which holds the group

            Strategy strat = db.Strategies.FirstOrDefault(x => x.StrategyId == id);
            nextactionold = strat.NextAction;
            strat.Updated = DateTime.Now;

            if (strat.NextAction == null)
            {
                strat.NextAction = "Need to Confirm CEM location";
                db.Entry(strat).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                strat.NextAction = "Need to Confirm CEM location";
                strat.History = strat.Updated.Value.ToString("MM/dd/yy") + " (" + emailListItem.FirstName + " " + emailListItem.LastName + ") :" + nextactionold + "\r\n" + strat.History;
                db.Entry(strat).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Edit", "Strategy", new { id });
        }


    }
}