using ECommerce.Entity;
using ECommerce.Repository;
using ECommerce.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerceSample.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        [HttpGet]
        public ActionResult Pay()
        {
            ViewBag.PaymentTypes = new SelectList(PaymentRep.List(), "PaymentId", "PaymentName");
            return View();
        }
        [HttpPost]
        public ActionResult Pay(Invoice model,List<string> PaymentTypes)
        {
            model.PaymentDate = DateTime.Now;
            foreach (string item in PaymentTypes)
            {
                int PaymentId = Convert.ToInt32(item);
                model.PaymentTypeId = PaymentId;
            }
            model.OrderId = ((Order)Session["Order"]).OrderId;
            InvoiceRep ip = new InvoiceRep();
            if (ip.Insert(model).IsSuccessed)
            {
                List<Product> proList = new List<Product>();
                Order ord = (Order)Session["Order"];
                OrderRep ordrep = new OrderRep();
                OrderDetailRep ordDetRep = new OrderDetailRep();
                ProductRep proRep = new ProductRep();
                Result<List<OrderDetail>> ordDetRes = ordDetRep.GetListByOrdId((int)model.OrderId);
                foreach (OrderDetail item in ordDetRes.ProcessResult)
                {
                    Product pr = proRep.GetObjById(item.ProductId).ProcessResult;
                    if (pr.Stock != null)
                    {
                        if (pr.Stock < item.Quantity)
                        {
                            string msg = "Yeterli sayida stok yok";
                            return RedirectToAction("DetailList", "Order", new { msg = msg });
                        }
                        else
                        {
                            pr.Stock -= item.Quantity;
                            proList.Add(pr);
                        }
                    }
                }
                Member mem = (Member)Session["UserID"];
                if (mem != null)
                {
                    ord.MemberId = mem.UserId;
                    ord.IsPay = true;
                    ordrep.Update(ord);
                    foreach (Product item in proList)
                    {
                        proRep.Update(item);
                    }
                    Session["Order"] = null;
                    return RedirectToAction("LatestOrders", "Home");
                }
                else
                {
                    return RedirectToAction("SignIn", "Home");
                }
            }
            else
            {
                return View(model);
            }
        }
    }
}