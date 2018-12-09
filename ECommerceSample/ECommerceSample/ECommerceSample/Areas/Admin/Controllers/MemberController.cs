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
    public class MemberController : Controller
    {
        // GET: Admin/Member
        MemberRepository mr = new MemberRepository();
        InstanceResult<Member> result = new InstanceResult<Member>();
        public ActionResult List(string m,int? id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("AdminLogin", "AdminLogin");
            }
            else
            {
                List<Member> mm = mr.List().ProcessResult.Where(t => t.RoleId == 1).ToList();
                result.resultList = mr.List();
                if (m != null)
                    ViewBag.Mesaj = string.Format("{0} nolu kaydin silme islemi {1}", id, m);
                else
                    ViewBag.Mesaj = "";
                return View(mm);
            }
            
        }
        public ActionResult AddMember()
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
        public ActionResult AddMember(Member model)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("AdminLogin", "AdminLogin");
            }
            else
            {
                model.RoleId = 1;
                result.resultint = mr.Insert(model);
                if (result.resultint.IsSuccessed)
                    return RedirectToAction("List");
                else
                    return View(model);
            }
            
        }

        public ActionResult Edit(int id)
        {
            return View(mr.GetObjById(id).ProcessResult);
        }
        [HttpPost]
        public ActionResult Edit(Member model)
        {
            result.resultint = mr.Update(model);
            if (result.resultint.IsSuccessed)
                return RedirectToAction("List");
            else
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            result.resultint = mr.Delete(id);
            return RedirectToAction("List", new { @m = result.resultint.UserMessage, @id = id });
        }
    }
}