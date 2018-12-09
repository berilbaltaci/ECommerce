using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECommerce.Entity;
using ECommerce.Repository;
using ECommerceSample.Areas.Admin.Models.ResultModel;

namespace ECommerceSample.Controllers
{
    public class HomeController : Controller
    {

        ProductRep pr = new ProductRep();
        // GET: Home
        public ActionResult Index()
        {
            return View(pr.GetLatestObj(6).ProcessResult);
        }
        public ActionResult Detail(int id)
        {
            Product p = pr.GetObjById(id).ProcessResult;
            return View(p);
        }
        public ActionResult List(Guid? id)
        {
            List<Product> pList = pr.List().ProcessResult.Where(t => t.CategoryId == id).ToList();
            return View(pList);
        }
        public ActionResult ListByBrand(int? id)
        {
            List<Product> pList = pr.List().ProcessResult.Where(t => t.BrandId == id).ToList();
            return View(pList);
        }
        public ActionResult ListAllProduct()
        {
            return View(pr.List().ProcessResult);
        }
        MemberRepository mr = new MemberRepository();
        InstanceResult<Member> result = new InstanceResult<Member>();
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(Member member)
        {
            member.RoleId = 2;
            result.resultint = mr.Insert(member);
            if (result.resultint.IsSuccessed)
            {
                Session["UserID"] = member;
                return RedirectToAction("ListAllProduct");
            } 
            else
                return View(member);
        }
        public ActionResult SignIn()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "Admin/Admin");
            }
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SignIn(Member member)
        {
            using (MyECommerceEntities db = new MyECommerceEntities())
            {
                var userDetails = db.Members.FirstOrDefault(t => t.Email == member.Email && t.Password == member.Password);
                if (userDetails == null)
                {
                    return View("SignIn", member);
                }
                else
                {
                    Session["UserID"] = userDetails;
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        public ActionResult LogOut()
        {
            Session["UserID"] = null;
            return RedirectToAction("SignIn", "Home");
        }
        public ActionResult LatestOrders(Member member)
        {
            return View("LatestOrders");
        }
        OrderDetailRep ordDetRep = new OrderDetailRep();
        public ActionResult OrderDet(int id)
        {
            List<OrderDetail> ordDetList = ordDetRep.GetListByOrdId(id).ProcessResult;
            return View(ordDetList);
        }
    }
}