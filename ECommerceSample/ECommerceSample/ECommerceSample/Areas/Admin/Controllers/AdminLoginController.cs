using ECommerce.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerceSample.Areas.Admin.Controllers
{
    public class AdminLoginController : Controller
    {
        // GET: Admin/AdminLogin
        public ActionResult AdminLogin()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("AdminHome");
            }
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AdminLogin(Member member)
        {
            using (MyECommerceEntities db = new MyECommerceEntities())
            {
                var userDetails = db.Members.FirstOrDefault(t => t.Email == member.Email && t.Password == member.Password && t.RoleId==1);
                if (userDetails == null)
                {
                    ViewBag.Mesaj = "E-mail veya sifre yanlis";
                    return RedirectToAction("AdminLogin", "AdminLogin");
                }
                else
                {
                    Session["UserID"] = userDetails;
                    return RedirectToAction("AdminHome", "AdminHome");
                }
            }
        }
    }
}