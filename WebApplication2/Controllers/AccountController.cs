using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class AccountController : Controller
    {


        private sjassoc_dbEntities db = new sjassoc_dbEntities();


        // GET: Account
        public ActionResult Index()
        {

            if (Session["UserId"] != null)
            {
                return View(db.UserAccounts.ToList());
            }
            else
            {
                return RedirectToAction("Login");
            }


        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        //public ActionResult Register(UserAccount account, EmailList emailList)
        public ActionResult Register(string email)
        {
            if (ModelState.IsValid)
            {

                var exist = db.UserAccounts.Any(x => x.Email == email);
                var reg = db.EmailLists.Any(x => x.Email == email);
                //if (!exists)
                //{

                //    if (reg == true)
                //    {

                //        db.userAccount.Add(account);
                //        db.SaveChanges();
                //        ModelState.Clear();
                //        ViewBag.Message = account.FirstName + " " + account.LastName + " successfully registered.";


                //    }
                //    else
                //    {
                //        ViewBag.Message = "You are not authorized to use this site. Please contact SJ Associates.";
                //    }
                //}
                //else
                //{
                //    //ModelState.AddModelError("", "Email already exist.");
                //    //return RedirectToAction("Register");
                //    ViewBag.Message = "The email " + account.Email + " already exists.";
                //}







                //using (OurDbContext db = new OurDbContext())
                //{
                //    var userNameToCheck = account.Email;
                //    var exists = db.userAccount.Any(x => x.Email == userNameToCheck);
                //    var reg = db.emailList.Any(x => x.Email == userNameToCheck);
                //    if (!exists)
                //    {

                //        if (reg == true)
                //        {

                //            db.userAccount.Add(account);
                //            db.SaveChanges();
                //            ModelState.Clear();
                //            ViewBag.Message = account.FirstName + " " + account.LastName + " successfully registered.";


                //        }
                //        else
                //        {
                //            ViewBag.Message = "You are not authorized to use this site. Please contact SJ Associates.";
                //        }    
                //    }
                //    else
                //    {
                //        //ModelState.AddModelError("", "Email already exist.");
                //        //return RedirectToAction("Register");
                //        ViewBag.Message = "The email " + account.Email + " already exists.";
                //    }


                //}

            }
            return View();
        }

        //Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserAccount user)
        {
            using (OurDbContext db = new OurDbContext())
            {
                var usr = db.userAccount.Where(u => u.Username == user.Username && u.Password == user.Password).FirstOrDefault();
                if (usr != null)
                {
                    Session["UserID"] = usr.UserID.ToString();
                    Session["Username"] = usr.Username.ToString();
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password is incorrect");
                }
            }
            return View();

        }

        public ActionResult LoggedIn()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}