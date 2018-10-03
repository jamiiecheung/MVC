using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Models
{
    public class OpptysController : Controller
    {

        private sjassoc_dbEntities db = new sjassoc_dbEntities();

        // GET: Opptys
        public ActionResult Index(string Groupddl, string Statusddl, string Prinddl, string OSRddl, string Customerddl, string osrsel, string prinsel, string statussel, string groupsel, string customersel,
            string statusdrop, string groupnew, string sortOrder, string customerName, string stratvar, string fltstring, Opptys selg)
        {
            if (Session["UserId"] != null)
            {
                //return View(db.Opptys1.ToList());

                Opptys strat = new Opptys();

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
                    List<Opptys> list = new List<Opptys>();

                    // this is a foreach loop, it goes through all the Todos returned from db.Todoes.ToList()
                    foreach (Opptys s in db.Opptys1.ToList())
                    {
                        // makes sure that the group of a todo isn't null (empty)
                        if (!String.IsNullOrEmpty(s.Principal))
                        {
                            // checks if the group of the user is equal to the group of the post. if so it adds the todo to the list.
                            if (emailListItem.Group.Equals(s.Principal))
                            {
                                list.Add(s);
                            }
                        }
                    }


                    //ViewBag.CustomerSort = sortOrder == "Name" ? "Name_desc" : "Name";

                    //var model = from t in list
                    //            select t;


                    //switch (sortOrder)
                    //{
                    //    case "Name_desc":
                    //        model = model.OrderByDescending(t => t.Customer);
                    //        break;
                    //    case "Name":
                    //        model = model.OrderBy(t => t.Customer);
                    //        break;
                    //}




                    string p = emailListItem.Perm;
                    string gr = emailListItem.Group;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT * FROM dbo.Opptys1 WHERE "); //change table name for whatever you need returned
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
                        List<Opptys> list1 = db.Opptys1.SqlQuery(sb.ToString()).ToList(); //change table name
                        return View(list1);
                    }

                    //else
                    //{
                    //var groups = from g in list
                    //             select g;
                    //var prins = from pr in list
                    //            select pr;
                    ///*  var osrs = from o in db.Strategies
                    //             select o;
                    //  var statuss = from s in db.Strategies
                    //                select s; */



                    //List<SelectListItem> groupListItems = list.Select(w => w.Group).Where(g => g != null).Distinct().Select(g => new SelectListItem { Value = g, Text = g }).ToList();
                    //ViewBag.Groupddl = new SelectList(groupListItems, "Value", "Text").Distinct();

                    //List<SelectListItem> prinListItems = list.Select(w => w.Principal).Where(pr => pr != null).Distinct().Select(pr => new SelectListItem { Value = pr, Text = pr }).ToList();
                    //ViewBag.Prinddl = new SelectList(prinListItems, "Value", "Text").Distinct();

                    //List<SelectListItem> osrListItems = list.Select(w => w.OSR).Where(o => o != null).Distinct().Select(o => new SelectListItem { Value = o, Text = o }).ToList();
                    //ViewBag.OSRddl = new SelectList(osrListItems, "Value", "Text").Distinct();

                    //List<SelectListItem> statusListItems = list.Select(w => w.Status).Where(g => g != null).Distinct().Select(g => new SelectListItem { Value = g, Text = g }).ToList();
                    //ViewBag.Statusddl = new SelectList(statusListItems, "Value", "Text").Distinct();

                    //List<SelectListItem> customerListItems = list.Select(w => w.Customer).Where(c => c != null).Distinct().Select(c => new SelectListItem { Value = c, Text = c }).ToList();
                    //ViewBag.Customerddl = new SelectList(customerListItems, "Value", "Text").Distinct();


                    ////if all filters are null
                    //if (Groupddl == null && stratvar == null && Prinddl == null && OSRddl == null && Statusddl == null && Customerddl == null)
                    //{
                    //    if (sortOrder == null)
                    //    {
                    //        return View(model.ToList());
                    //    }
                    //    else
                    //    {
                    //        return View(model.ToList());
                    //    }
                    //}


                    ////if (prin != null && group != null && osr != null && status != null)
                    //if (Prinddl != null && Groupddl != null && OSRddl != null && Statusddl != null && Customerddl != null)
                    //{
                    //    prins = prins.Where(gpr => gpr.Principal.Contains(Prinddl) && gpr.Group.Contains(Groupddl) && gpr.OSR.Contains(OSRddl) && gpr.Status.Contains(Statusddl) && gpr.Customer.Contains(Customerddl));
                    //    Session["filtprins"] = Prinddl;
                    //    Session["filtgroup"] = Groupddl;
                    //    Session["filtstatus"] = Statusddl;
                    //    Session["filtosr"] = OSRddl;
                    //    Session["filtcustomer"] = Customerddl;

                    //    stratvar = null;

                    //    return View(prins.ToList());
                    //}


                    //return View(model);
                    //}

                    return View(list);
                }
                else
                {
                    // This is the query building code. 
                    string p = emailListItem.Perm;
                    string gr = emailListItem.Group;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT * FROM dbo.Opptys1 WHERE "); //change table name for whatever you need returned
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
                        List<Opptys> list = db.Opptys1.SqlQuery(sb.ToString()).ToList(); //change table name
                        return View(list);
                    }

                    else
                    {
                        //var groups = from g in db.Opptys1
                        //             select g;
                        //var prins = from pr in db.Opptys1
                        //            select pr;
                        ///*  var osrs = from o in db.Strategies
                        //             select o;
                        //  var statuss = from s in db.Strategies
                        //                select s; */

                        //List<SelectListItem> groupListItems = db.Strategies.Where(w => w.Group != null).Select(group => new SelectListItem { Value = group.Group, Text = group.Group }).Distinct().ToList();
                        //ViewBag.Groupddl = new SelectList(groupListItems, "Value", "Text").Distinct();

                        //List<SelectListItem> prinListItems = db.Strategies.Where(w => w.Principal != null).Select(prin => new SelectListItem { Value = prin.Principal, Text = prin.Principal }).Distinct().ToList();
                        //ViewBag.Prinddl = new SelectList(prinListItems, "Value", "Text").Distinct();

                        //List<SelectListItem> osrListItems = db.Strategies.Where(w => w.OSR != null).Select(osr => new SelectListItem { Value = osr.OSR, Text = osr.OSR }).Distinct().ToList();
                        //ViewBag.OSRddl = new SelectList(osrListItems, "Value", "Text").Distinct();

                        //List<SelectListItem> statusListItems = db.Strategies.Where(w => w.Status != null).Select(status => new SelectListItem { Value = status.Status, Text = status.Status }).Distinct().ToList();
                        //ViewBag.Statusddl = new SelectList(statusListItems, "Value", "Text").Distinct();

                        //List<SelectListItem> customerListItems = db.Strategies.Where(w => w.Customer != null).Select(cust => new SelectListItem { Value = cust.Customer, Text = cust.Customer }).Distinct().ToList();
                        //ViewBag.Customerddl = new SelectList(customerListItems, "Value", "Text").Distinct();


                        //// Convert sort order
                        //ViewBag.CustomerSort = sortOrder == "Name" ? "Name_desc" : "Name";



                        //var model = from t in db.Strategies
                        //            select t;



                        //switch (sortOrder)
                        //{
                        //    case "Name_desc":
                        //        model = model.OrderByDescending(t => t.Customer);
                        //        break;
                        //    case "Name":
                        //        model = model.OrderBy(t => t.Customer);
                        //        break;
                        //}
                        ////}
                        ////if all filters are null
                        //if (Groupddl == null && stratvar == null && Prinddl == null && OSRddl == null && Statusddl == null && Customerddl == null)
                        //{
                        //    if (sortOrder == null)
                        //    {
                        //        return View(db.Strategies.ToList());
                        //    }
                        //    else
                        //    {
                        //        return View(model.ToList());
                        //    }
                        //}


                        ////if (prin != null && group != null && osr != null && status != null)
                        //if (Prinddl != null && Groupddl != null && OSRddl != null && Statusddl != null && Customerddl != null)
                        //{
                        //    prins = prins.Where(gpr => gpr.Principal.Contains(Prinddl) && gpr.Group.Contains(Groupddl) && gpr.OSR.Contains(OSRddl) && gpr.Status.Contains(Statusddl) && gpr.Customer.Contains(Customerddl));
                        //    Session["filtprins"] = Prinddl;
                        //    Session["filtgroup"] = Groupddl;
                        //    Session["filtstatus"] = Statusddl;
                        //    Session["filtosr"] = OSRddl;
                        //    Session["filtcustomer"] = Customerddl;

                        //    stratvar = null;

                        //    return View(prins.ToList());
                        //}
                        //return View(prins.ToList());
                        return View(db.Opptys1.ToList());
                    }

                }
























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