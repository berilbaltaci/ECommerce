using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECommerce.Repository;
using ECommerce.Common;
using ECommerce.Entity;
using ECommerceSample.Areas.Admin.Models.ResultModel;

namespace ECommerceSample.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        CategoryRep cr = new CategoryRep();
        //Result<List<Category>> result = new Result<List<Category>>();
        //Result<int> resultint = new Result<int>();
        //Result<Category> resultt = new Result<Category>();
        // GET: Admin/Category
        InstanceResult<Category> result = new InstanceResult<Category>();
        public ActionResult List(string mesaj)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("AdminLogin", "AdminLogin");
            }
            else
            {
                result.resultList = cr.List();
                ViewBag.Mesaj = mesaj;
                return View(result.resultList.ProcessResult);

            }
        }
        [HttpGet] //get methodu ile yapilan istekler tarayicinin adres satirinda gorunur. 
        public ActionResult AddCategory()
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
        public ActionResult AddCategory(Category model)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("AdminLogin", "AdminLogin");
            }
            else
            {
                model.CategoryId = Guid.NewGuid();
                result.resultint = cr.Insert(model);
                ViewBag.Mesaj = result.resultint.UserMessage;
                return View();
            }
            
        }
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            result.TResult = cr.GetObjById(id);
            return View(result.TResult.ProcessResult);
        }
        [HttpPost]
        public ActionResult Edit(Category model)
        {
            result.resultint = cr.Update(model);
            ViewBag.Mesaj = result.resultint.UserMessage;
            return View();
        }

        public ActionResult Delete(Guid id)
        {
            result.resultint = cr.Delete(id);
            return RedirectToAction("List", new { @mesaj = result.resultint.UserMessage });
            
        }
    }
}