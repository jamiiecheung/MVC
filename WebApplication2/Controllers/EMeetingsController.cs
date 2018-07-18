using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class EMeetingsController:Controller
    {
        private sjassoc_dbEntities db = new sjassoc_dbEntities();

        public ActionResult Index()
        {
            
            if (Session["UserId"] != null)
            {
                return View(db.EMeetings.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }
    }
}