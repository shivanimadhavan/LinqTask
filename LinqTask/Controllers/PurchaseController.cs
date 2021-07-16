using LinqTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinqTask.Controllers
{
    public class PurchaseController : Controller
    {
        // GET: Purchase
        public ActionResult Index()
        {
            using (dbTaskEntities db = new dbTaskEntities())
            {
                List<Product> products = db.Products.ToList();
                List<Customer> customers = db.Customers.ToList();
                List<PurchaseOrder> purchaseOrders = db.PurchaseOrders.ToList();
                List<PurchaseOrderDetail> purchaseOrderDetails = db.PurchaseOrderDetails.ToList();
                var listOfPurchaseOrder = (from c in customers
                                           join po in purchaseOrders on c.CustomerID equals po.CustomerID
                                           join pod in purchaseOrderDetails on po.POID equals pod.POID
                                           join p in products on pod.ProductID equals p.ProductID
                                           select new ListOfPurchase
                                           {
                                               customer = c,
                                               purchaseOrder = po,
                                               purchaseOrderDetail = pod,
                                               product = p
                                           });
                return View(listOfPurchaseOrder);
            }
        }
        public ActionResult ProductName()
        {
            using (dbTaskEntities db = new dbTaskEntities())
            {
                List<Product> products = db.Products.ToList();
                var productNameAsc = from p in products
                                     orderby p.ProductName
                                     select new ListOfPurchase
                                     {
                                         product = p
                                     };
                return View(productNameAsc);
            }
        }
        // GET: Purchase
        public ActionResult CustomerReport()
        {
            using (dbTaskEntities db = new dbTaskEntities())
            {
                List<Customer> customers = db.Customers.ToList();
                List<PurchaseOrder> purchaseOrders = db.PurchaseOrders.ToList();
                var customerReport = (from c in customers
                                      join po in purchaseOrders on c.CustomerID equals po.CustomerID
                                      group new { c, po } by new { po.Date.Value.Month } into G
                                      let firstproductgroup = G.FirstOrDefault()
                                      let DOP = firstproductgroup.po
                                      let CustomerName = firstproductgroup.c
                                      let maxamount = G.Sum(m => m.po.Amount)
                                      select new ProdReport
                                      {
                                          Date = (DateTime)DOP.Date,
                                          CustomerName = CustomerName,
                                          Amount = maxamount
                                      });
                return View(customerReport);
            }
        }
        public ActionResult ProductReport()
        {
            using (dbTaskEntities db = new dbTaskEntities())
            {
                List<Product> products = db.Products.ToList();
                List<PurchaseOrder> purchaseOrders = db.PurchaseOrders.ToList();
                List<PurchaseOrderDetail> purchaseOrderDetail = db.PurchaseOrderDetails.ToList();
                var SalesReport = (from p in products
                                   join pod in purchaseOrderDetail
                                        on p.ProductID equals pod.ProductID
                                   join po in purchaseOrders
                                  on pod.POID equals po.POID


                                   group new { p, pod, po } by new { po.Date.Value.Month } into G
                                   let firstproductgroup = G.FirstOrDefault()
                                   let DOP = firstproductgroup.po
                                   let ProductName = firstproductgroup.p
                                   let quantity = G.Sum(m => m.pod.Quantity)

                                   select new ProductsReport
                                   {
                                       Date = (DateTime)DOP.Date,
                                       ProductName = ProductName,
                                       Quantity = quantity
                                   });
                return View(SalesReport);
            }
        }
        public ActionResult MaximumPrice()
        {


            dbTaskEntities db = new dbTaskEntities();
            List<PurchaseOrder> purchaseOrder = db.PurchaseOrders.ToList();
            List<PurchaseOrderDetail> purchaseOrderDetail = db.PurchaseOrderDetails.ToList();
            var Maximum = (from po in purchaseOrder
                           join pod in purchaseOrderDetail on po.POID equals pod.POID
                           group new { po, pod } by new { po.Date.Value.Month } into G
                           let firstproductgroup = G.FirstOrDefault()
                           let DOP = firstproductgroup.po
                           let POID = firstproductgroup.pod
                           let maxprice = G.Max(m => m.pod.Price)
                           select new Monthwisemaxprice
                           {
                               Date = (DateTime)DOP.Date,
                               POID = POID,
                               Price = maxprice
                           });
            return View(Maximum);

        }

    }
}