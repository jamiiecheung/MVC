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
                // Get the user id from the session
                int id = Int32.Parse(Session["UserId"].ToString());
                // Use the id to get the associated email address
                String em = db.UserAccounts.Find(id).Email.ToString();
                // Use the email address to get the associated emaillist object which holds the group
                EmailList emailListItem = db.EmailLists.First(x => x.Email == em);

                if (emailListItem.Group.Equals("Wachtel"))
                {
                    return View(db.Todoes.ToList());
                }

                // Create a list to hold the Todos which we will end up showing
                List<Todo> list = new List<Todo>();

                // this is a foreach loop, it goes through all the Todos returned from db.Todoes.ToList()
                foreach (Todo td in db.Todoes.ToList())
                {
                    // makes sure that the group of a todo isn't null (empty)
                    if (!String.IsNullOrEmpty(td.Group))
                    {
                        // checks if the group of the user is equal to the group of the post. if so it adds the todo to the list.
                        if (emailListItem.Group.Equals(td.Group))
                        {
                            list.Add(td);
                        }
                    }
                }
                return View(list);
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
                options.Add(new Todo() { Status = "New Request", Text = "New Request" });
                options.Add(new Todo() { Status = "Reviewed", Text = "Reviewed" });
                options.Add(new Todo() { Status = "Started", Text = "Started" });
                options.Add(new Todo() { Status = "In Progress", Text = "In Progress" });
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
                owners.Add(new Todo() { Owner = "MW", Text = "MW" });
                owners.Add(new Todo() { Owner = "RN", Text = "RN" });
                owners.Add(new Todo() { Owner = "RW", Text = "RW" });
                owners.Add(new Todo() { Owner = "SH", Text = "SH" });
                owners.Add(new Todo() { Owner = "TF", Text = "TF" });
                owners.Add(new Todo() { Owner = "TL", Text = "TL" });
                owners.Add(new Todo() { Owner = "WL", Text = "WL" });
                owners.Add(new Todo() { Owner = "WL", Text = "WL" });

                ViewBag.Ownerddl = owners;




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

                //string osrid = todo.OSR; // Get the OSR initials
                // UserAccount item = db.UserAccounts.FirstOrDefault(i => i.OSR == osrid); // Get all the user account information based that matches the osrid
                //TempData["OSREmail"] = item.Email; //put tempdata and store osr email address in it


                //TempData["Jamie"] = "jcheung@sjassoc.com";
                TempData["message"] = "Create Date: " + todo.CreateDate + "\r\nDescription: " + todo.Description + "\r\nTask: " + todo.Task + "\r\nStatus: " + todo.Status + "\r\nFollowUp: " + todo.FollowUp + "\r\nGroup: " + todo.Group;

                TempData["id"] = id;

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


            mailMessage.From = new MailAddress(senderID);
            mailMessage.Subject = messageSubject;

            mailMessage.Body = Convert.ToString(TempData["message"]);
            mailMessage.IsBodyHtml = true;
            //mailMessage.Body = messageBody;
            mailMessage.Body = messageBody + "<br/><br/>" + "<b>Create Date: </b>" + todo.CreateDate + "<br/>" + "<b>Description: </b>" + todo.Description + "<br/>" + "<b>Task: </b>" + todo.Task + "<br/>" + "<b>Status: </b>" + todo.Status + "<br/>" + "<b>Followup: </b>" + todo.FollowUp + "<br/>" + "<b>Group: </b>" + todo.Group;
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

                SendHtmlFormattedEmail(form["txtto"], form["txtsubject"], form["txtbody"]);
                return RedirectToAction("Index", "Todoes");

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

    }
}
