using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECommerce.Common;
using ECommerce.Entity;
using ECommerce.Repository;
using ECommerceSample.Areas.Admin.Models.ResultModel;

namespace ECommerceSample.Areas.Admin.Controllers
{
    public class BrandController : Controller
    {
        BrandRep br = new BrandRep();
        //Result<List<Brand>> resultList = new Result<List<Brand>>();
        //Result<int> resultint = new Result<int>();
        //Result<Brand> brandResult = new Result<Brand>();
        InstanceResult<Brand> result = new InstanceResult<Brand>();
        // GET: Admin/Brand
        public ActionResult List()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("AdminLogin", "AdminLogin");
            }
            else
            {
                result.resultList = br.List();
                return View(result.resultList.ProcessResult);
            }
            
        }
        public ActionResult AddBrand()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("AdminLogin", "AdminLogin");
            }
            else
            {
                Brand b = new Brand();
                b.Photo = "denemeTestdeneme";
                return View(b);
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddBrand(Brand model,HttpPostedFileBase photoPath)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("AdminLogin","AdminLogin");
            }
            else
            {
                string PhotoName = "";
                if (photoPath.ContentLength > 0 & photoPath != null)
                {
                    PhotoName = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                    string path = Server.MapPath("~/Upload/" + PhotoName);
                    photoPath.SaveAs(path);
                }
                model.Photo = PhotoName;
                if (ModelState.IsValid)
                {
                    result.resultint = br.Insert(model);
                    if (result.resultint.IsSuccessed)
                        return RedirectToAction("List");
                    else
                    {
                        ViewBag.Mesaj = result.resultint.UserMessage;
                        return View(model);
                    }
                }
                else
                return View();
            }
            
        }
        public ActionResult Edit(int id)
        {
            result.TResult = br.GetObjById(id);
            return View(result.TResult.ProcessResult);
        }
        [HttpPost]
        public ActionResult Edit(Brand model,HttpPostedFileBase PhotoPath)
        {
            string PhotoName = model.Photo;
            if (PhotoPath.ContentLength > 0 & PhotoPath != null)
            {
                PhotoName = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                string path = Server.MapPath("~/Upload/" + PhotoName);
                PhotoPath.SaveAs(path);
            }
            model.Photo = PhotoName;
            result.resultint = br.Update(model);
            if (result.resultint.IsSuccessed)
            {
                return RedirectToAction("List");
            }
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            result.resultint = br.Delete(id);
            return RedirectToAction("List", new { @mesaj = result.resultint.UserMessage });
        }
       
    }
}