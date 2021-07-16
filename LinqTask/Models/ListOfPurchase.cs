using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace LinqTask.Models
{
    public class ListOfPurchase
    {
        public Customer customer { get; set; }
        public PurchaseOrder purchaseOrder { get; set; }
        public PurchaseOrderDetail purchaseOrderDetail { get; set; }
        public Product product { get; set; }
    }
}