using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Text;

namespace WebApplication2.Controllers
{
    public class TodoesController : Controller
    {
        private sjassoc_dbEntities db = new sjassoc_dbEntities();


        // GET: Todoes
        public ActionResult Index(string Groupddl, string Statusddl, string osrsel, string prinsel, string statussel, string groupsel,
            string statusdrop, string groupnew, string stratvar, string fltstring, Todo selg, FormCollection form)
        {

            if (Session["UserId"] != null)
            {
                //return View(db.Opptys1.ToList());

                Todo strat = new Todo();

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
                    List<Todo> list = new List<Todo>();

                    // this is a foreach loop, it goes through all the Todos returned from db.Todoes.ToList()
                    foreach (Todo s in db.Todoes.ToList())
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
                    sb.Append("SELECT * FROM dbo.Todoes WHERE "); //change table name for whatever you need returned
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
                        List<Todo> list1 = db.Todoes.SqlQuery(sb.ToString()).ToList(); //change table name
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
                    sb.Append("SELECT * FROM dbo.Todoes WHERE "); //change table name for whatever you need returned
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
                        List<Todo> list = db.Todoes.SqlQuery(sb.ToString()).ToList(); //change table name
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
                        return View(db.Todoes.ToList());
                    }

                }
























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
                model.CreateDate = DateTime.Now;

                var options = new List<Todo>();
                options.Add(new Todo() { Status = "Assigned", Text = "Assigned" });
                options.Add(new Todo() { Status = "Accepted", Text = "Accepted" });
                options.Add(new Todo() { Status = "Rejected", Text = "Rejected" });
                options.Add(new Todo() { Status = "Pending", Text = "Pending" });
                options.Add(new Todo() { Status = "Completed", Text = "Completed" });

                ViewBag.Status = options;

                var owners = new List<Todo>();

                owners.Add(new Todo() { Owner = "CF", Text = "CF" });
                owners.Add(new Todo() { Owner = "DR", Text = "DR" });
                owners.Add(new Todo() { Owner = "JA", Text = "JA" });
                owners.Add(new Todo() { Owner = "JB", Text = "JB" });
                owners.Add(new Todo() { Owner = "JC", Text = "JC" });
                owners.Add(new Todo() { Owner = "JL", Text = "JL" });
                owners.Add(new Todo() { Owner = "JM", Text = "JM" });
                owners.Add(new Todo() { Owner = "JT", Text = "JT" });
                owners.Add(new Todo() { Owner = "KK", Text = "KK" });
                owners.Add(new Todo() { Owner = "MW", Text = "MW" });
                owners.Add(new Todo() { Owner = "RN", Text = "RN" });
                owners.Add(new Todo() { Owner = "RW", Text = "RW" });
                owners.Add(new Todo() { Owner = "SH", Text = "SH" });
                owners.Add(new Todo() { Owner = "TF", Text = "TF" });
                owners.Add(new Todo() { Owner = "TL", Text = "TL" });
                owners.Add(new Todo() { Owner = "WL", Text = "WL" });


                ViewBag.Owner = owners;


                var creators = new List<Todo>();

                creators.Add(new Todo() { Creator = "CF", Text = "CF" });
                creators.Add(new Todo() { Creator = "DR", Text = "DR" });
                creators.Add(new Todo() { Creator = "JA", Text = "JA" });
                creators.Add(new Todo() { Creator = "JB", Text = "JB" });
                creators.Add(new Todo() { Creator = "JC", Text = "JC" });
                creators.Add(new Todo() { Creator = "JL", Text = "JL" });
                creators.Add(new Todo() { Creator = "JM", Text = "JM" });
                creators.Add(new Todo() { Creator = "JT", Text = "JT" });
                creators.Add(new Todo() { Creator = "KK", Text = "KK" });
                creators.Add(new Todo() { Creator = "MW", Text = "MW" });
                creators.Add(new Todo() { Creator = "RN", Text = "RN" });
                creators.Add(new Todo() { Creator = "RW", Text = "RW" });
                creators.Add(new Todo() { Creator = "SH", Text = "SH" });
                creators.Add(new Todo() { Creator = "TF", Text = "TF" });
                creators.Add(new Todo() { Creator = "TL", Text = "TL" });
                creators.Add(new Todo() { Creator = "WL", Text = "WL" });


                ViewBag.Creator = creators;





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
        public ActionResult Create([Bind(Include = "ID,Description,CreatedDate,Task,FollowUp,Status,Group,Owner,Creator")] Todo todo)
        {

            if (Session["UserId"] != null)
            {
                if (ModelState.IsValid)
                {
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
        public ActionResult Edit(int? id, string owner)
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
                options.Add(new Todo() { Status = "Assigned", Text = "Assigned" });
                options.Add(new Todo() { Status = "Accepted", Text = "Accepted" });
                options.Add(new Todo() { Status = "Rejected", Text = "Rejected" });
                options.Add(new Todo() { Status = "Pending", Text = "Pending" });
                options.Add(new Todo() { Status = "Completed", Text = "Completed" });

                ViewBag.Status = options;



                ////List<SelectListItem> osrListItems = db.Label.Select(osr => new SelectListItem { Value = osr.LabelId.ToString(), Text = osr.LabelName }).ToList();
                //List<SelectListItem> osrListItems1 = db.UserAccounts.Where(w => w.UserID == null).Select(osr => new SelectListItem { Value = osr.OSR.ToString(), Text = osr.OSR, Selected = true }).Distinct().ToList();
                //List<SelectListItem> osrListItems2 = db.UserAccounts.Where(w => w.UserID != null).Select(osr => new SelectListItem { Value = osr.OSR.ToString(), Text = osr.OSR, Selected = false }).Distinct().ToList();
                //List<SelectListItem> osrListItems = new List<SelectListItem>();
                //osrListItems.AddRange(osrListItems1);
                //osrListItems.AddRange(osrListItems2);
                //osrListItems = osrListItems.OrderBy(x => x.Value).ToList();

                //ViewBag.OSRddl = new SelectList(osrListItems, "Value", "Text", 2).Distinct();




                var owners = new List<Todo>();

                owners.Add(new Todo() { Owner = "CF", Text = "CF" });
                owners.Add(new Todo() { Owner = "DR", Text = "DR" });
                owners.Add(new Todo() { Owner = "JA", Text = "JA" });
                owners.Add(new Todo() { Owner = "JB", Text = "JB" });
                owners.Add(new Todo() { Owner = "JC", Text = "JC" });
                owners.Add(new Todo() { Owner = "JL", Text = "JL" });
                owners.Add(new Todo() { Owner = "JM", Text = "JM" });
                owners.Add(new Todo() { Owner = "JT", Text = "JT" });
                owners.Add(new Todo() { Owner = "KK", Text = "KK" });
                owners.Add(new Todo() { Owner = "MW", Text = "MW" });
                owners.Add(new Todo() { Owner = "RN", Text = "RN" });
                owners.Add(new Todo() { Owner = "RW", Text = "RW" });
                owners.Add(new Todo() { Owner = "SH", Text = "SH" });
                owners.Add(new Todo() { Owner = "TF", Text = "TF" });
                owners.Add(new Todo() { Owner = "TL", Text = "TL" });
                owners.Add(new Todo() { Owner = "WL", Text = "WL" });


                ViewBag.Ownerddl = owners;



                var creators = new List<Todo>();

                creators.Add(new Todo() { Owner = "CF", Text = "CF" });
                creators.Add(new Todo() { Owner = "DR", Text = "DR" });
                creators.Add(new Todo() { Owner = "JA", Text = "JA" });
                creators.Add(new Todo() { Owner = "JB", Text = "JB" });
                creators.Add(new Todo() { Owner = "JC", Text = "JC" });
                creators.Add(new Todo() { Owner = "JL", Text = "JL" });
                creators.Add(new Todo() { Owner = "JM", Text = "JM" });
                creators.Add(new Todo() { Owner = "JT", Text = "JT" });
                creators.Add(new Todo() { Owner = "KK", Text = "KK" });
                creators.Add(new Todo() { Owner = "MW", Text = "MW" });
                creators.Add(new Todo() { Owner = "RN", Text = "RN" });
                creators.Add(new Todo() { Owner = "RW", Text = "RW" });
                creators.Add(new Todo() { Owner = "SH", Text = "SH" });
                creators.Add(new Todo() { Owner = "TF", Text = "TF" });
                creators.Add(new Todo() { Owner = "TL", Text = "TL" });
                creators.Add(new Todo() { Owner = "WL", Text = "WL" });


                ViewBag.Creatorddl = creators;





                //List<SelectListItem> osrListItems = db.UserAccounts.Where(w => w.OSR != null).Select(osr => new SelectListItem { Value = osr.OSR, Text = osr.OSR }).Distinct().ToList();
                //ViewBag.OSRddl = new SelectList(osrListItems, "Value", "Text").Distinct();





                //string x = ViewBag.OSRddl.Value;
                //if (todo.Owner != null)
                //{

                //}


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
        public ActionResult Edit([Bind(Include = "ID,Description,CreatedDate,Task,Status,FollowUp,Group,Owner")] string OSRddl, Todo todo)
        {

            if (Session["UserId"] != null)
            {

                //document.getElementById('FollowUp').value=
                if (ModelState.IsValid)
                {

                    //if (OSRddl != null)
                    //{
                    //todo.Owner = OSRddl;

                    //}

                    //if (Ownerddl != null)
                    //{
                    //todo.Owner = Ownerddl;
                    //}

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



        public ActionResult Email(int? id, FormCollection form, string mailMessage)
        {
            if (Session["UserId"] != null)
            {
                Todo todo = db.Todoes.Find(id);

                string osrid = todo.Owner; // Get the OSR initials
                UserAccount item = db.UserAccounts.FirstOrDefault(i => i.OSR == osrid); // Get all the user account information based that matches the osrid
                TempData["OSREmail"] = item.Email; //put tempdata and store osr email address in it
                DateTime? createdate = todo.CreateDate;
                //fcreatedate = todo.CreateDate.Value.ToString("MM/dd/yy");

                DateTime? followupdate = todo.FollowUp;
                string ffollowupdate = todo.FollowUp.Value.ToString("MM/dd/yy");

                //TempData["Jamie"] = "jcheung@sjassoc.com";



                //var osrs = new List<Todo>();

                //osrs.Add(new Todo() { Owner = "CF", Text = "CF" });
                //osrs.Add(new Todo() { Owner = "DR", Text = "DR" });
                //osrs.Add(new Todo() { Owner = "JA", Text = "JA" });
                //osrs.Add(new Todo() { Owner = "JB", Text = "JB" });
                //osrs.Add(new Todo() { Value = "jcheung@sjassoc.com", Text = "jcheung@sjassoc.com" });
                //osrs.Add(new Todo() { Owner = "JL", Text = "JL" });
                //osrs.Add(new Todo() { Owner = "JM", Text = "JM" });
                //osrs.Add(new Todo() { Owner = "JT", Text = "JT" });
                //osrs.Add(new Todo() { Owner = "KK", Text = "KK" });
                //osrs.Add(new Todo() { Owner = "MW", Text = "MW" });
                //osrs.Add(new Todo() { Owner = "RN", Text = "RN" });
                //osrs.Add(new Todo() { Owner = "RW", Text = "RW" });
                //osrs.Add(new Todo() { Owner = "SH", Text = "SH" });
                //osrs.Add(new Todo() { Owner = "TF", Text = "TF" });
                //osrs.Add(new Todo() { Owner = "TL", Text = "TL" });
                //osrs.Add(new Todo() { Text = "wlondon@sjassoc.com" });

                //ViewBag.OSRsddl = osrs;
                //TempData["ToAddress"] = osrs;

                //List<SelectListItem> statusListItems = db.Todoes.Where(w => w.OSRs != null).Select(status => new SelectListItem { Value = status.Status, Text = status.Status }).Distinct().ToList();
                //ViewBag.Statusddl = new SelectList(statusListItems, "Value", "Text").Distinct();







                TempData["message"] = "Create Date: \t" + todo.CreateDate.Value.ToString("MM/dd/yy") + "\r\nDescription: \t" + todo.Description + "\r\nTask: \t\t" + todo.Task + "\r\nStatus: \t\t" + todo.Status + "\r\nFollowUp: \t" + todo.FollowUp.Value.ToString("MM/dd/yy") + "\r\nGroup: \t\t" + todo.Group;

                //TempData["OSRddl"] = ViewBag.OSRsddl;

                TempData["subject"] = "SJ Associates To Do: " + todo.Description;

                TempData["id"] = id;

                return View(todo);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


        public ActionResult Updateemail(int? id, FormCollection form, string mailMessage, string dropdown)
        {
            if (Session["UserId"] != null)
            {
                Todo todo = db.Todoes.Find(id);

                string osrid = todo.Owner; // Get the OSR initials
                UserAccount item = db.UserAccounts.FirstOrDefault(i => i.OSR == osrid); // Get all the user account information based that matches the osrid
                TempData["OSREmail"] = item.Email; //put tempdata and store osr email address in it

                //DateTime? createdate = todo.CreateDate;
                //fcreatedate = todo.CreateDate.Value.ToString("MM/dd/yy");

                //DateTime? followupdate = todo.FollowUp;
                //string ffollowupdate = todo.FollowUp.Value.ToString("MM/dd/yy");

                //TempData["Jamie"] = "jcheung@sjassoc.com";
                TempData["message"] = "Create Date: \t" + todo.CreateDate.Value.ToString("MM/dd/yy") + "\r\nDescription: \t" + todo.Description + "\r\nTask: \t\t" + todo.Task + "\r\nStatus: \t\t" + todo.Status + "\r\nFollowUp: \t" + todo.FollowUp.Value.ToString("MM/dd/yy") + "\r\nGroup: \t\t" + todo.Group;


                TempData["subject"] = "SJ Associates To Do: " + todo.Description;

                TempData["id"] = id;

                //TempData["to"] = dropdown;

                return View(todo);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }




        private void SendHtmlFormattedEmail(string toAddress, string messageSubject, string messageBody)
        {
            int id = Int32.Parse(Session["UserId"].ToString()); // Get the user id from the session
            String emailacc = db.UserAccounts.Find(id).Email.ToString(); // Use the id to get the associated email address
            String passw = db.UserAccounts.Find(id).epass.ToString(); // Use the id to get the associated email address
            string senderID = "";
            string senderPassword = "";

            string result = "Message Successfully Sent!!!";


            //catching if client or SJ Assoc.
            if (passw == null)
            {
                senderID = "jcheung@sjassoc.com";// use sender’s email id here..
                senderPassword = "1Direction!!"; // sender password here…
            }
            else
            {
                senderID = emailacc;// use sender’s email id here..
                senderPassword = passw; // sender password here…
            }

            Todo todo = db.Todoes.Find(TempData["id"]);
            MailMessage mailMessage = new MailMessage();

            //DateTime? createdate = todo.CreateDate;
            string fcreatedate = todo.CreateDate.Value.ToString("MM/dd/yy");

            //DateTime? followupdate = todo.FollowUp;
            string ffollowupdate = todo.FollowUp.Value.ToString("MM/dd/yy");
            //toAddress = ViewBag.OSRsddl;
            //toAddress = TempData["ToAddress"].ToString();

            //toAddress = TempData["to"].ToString();

            mailMessage.From = new MailAddress(senderID);
            //mailMessage.Subject = messageSubject;
            mailMessage.Subject = "SJ Associates To Do: " + todo.Description;

            mailMessage.Body = Convert.ToString(TempData["message"]);
            mailMessage.IsBodyHtml = true;


            mailMessage.Body = messageBody + "<br/><br/>" + "<b>Create Date: &emsp;</b>" + fcreatedate + "<br/>" + "<b>Description: &emsp;</b>" + todo.Description + "<br/>" + "<b>Task: &emsp;&emsp;&emsp;&nbsp;</b>" + todo.Task + "<br/>" + "<b>Status: &emsp;&emsp;&emsp;&nbsp;</b>" + todo.Status + "<br/>" + "<b>Followup: &emsp;&emsp;</b>" + ffollowupdate + "<br/>" + "<b>Group: &emsp;&emsp;&emsp;&nbsp;&nbsp;</b>" + todo.Group;
            mailMessage.To.Add(toAddress);
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.office365.com";
            smtp.EnableSsl = true;
            System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
            NetworkCred.UserName = senderID;
            NetworkCred.Password = senderPassword;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = 25;
            smtp.Send(mailMessage);
        }

        [HttpPost]
        //string txtsubject, string txtto, string txtbody, 
        public ActionResult Send(FormCollection form)
        {
            if (Session["UserId"] != null)
            {

                //SendHtmlFormattedEmail(form["txtto"], form["txtsubject"], form["txtbody"]);
                SendHtmlFormattedEmail(form["dropdown"], form["txtsubject"], form["txtbody"]);
                return RedirectToAction("Index", "Todoes");

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

    }
}
