using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECommerce.Entity;
using ECommerce.Repository;
using ECommerceSample.Areas.Admin.Models.ResultModel;

namespace ECommerceSample.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        // GET: Admin/User
        MemberRepository mr = new MemberRepository();
        InstanceResult<Member> result = new InstanceResult<Member>();
        public ActionResult AddUser()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("AdminLogin", "AdminLogin");
            }
            else
            {
                return View();
            }
            
        }
        [HttpPost]
        public ActionResult AddUser(Member model)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("AdminLogin", "AdminLogin");
            }
            else
            {
                model.RoleId = 2;
                result.resultint = mr.Insert(model);
                if (result.resultint.IsSuccessed)
                    return RedirectToAction("ListUser");
                else
                    return View(model);
            }
            
        }
        public ActionResult ListUser(string m, int? id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("AdminLogin", "AdminLogin");
            }
            else
            {
                List<Member> mm = mr.List().ProcessResult.Where(t => t.RoleId == 2).ToList();
                if (m != null)
                    ViewBag.Mesaj = string.Format("{0} nolu kaydin silme islemi {1}", id, m);
                else
                    ViewBag.Mesaj = "";
                return View(mm);
            }
            
        }
        public ActionResult EditUser(int id)
        {
            return View(mr.GetObjById(id).ProcessResult);
        }
        [HttpPost]
        public ActionResult EditUser(Member member)
        {
            result.resultint = mr.Update(member);
            if (result.resultint.IsSuccessed)
                return RedirectToAction("ListUser");
            else
                return View(member);
        }
        public ActionResult DeleteUser(int id)
        {
            result.resultint = mr.Delete(id);
            return RedirectToAction("ListUser", new { @m = result.resultint.UserMessage, @id = id });
        }
    }
}