using LocalPharmacy.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LocalPharmacy.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(string SearchString)
        {
            decimal s = 0;
            if (!String.IsNullOrEmpty(SearchString))
            {
                s = Convert.ToDecimal(SearchString);

            }
            PharmaTest_NREntities entities = new PharmaTest_NREntities();
            List<Product> products = entities.Products.ToList();
            List<ProductToDisplay> productToDisplays = new List<ProductToDisplay>();
            

            foreach(var p in products)
            {
                ProductToDisplay pr = new ProductToDisplay();
                    pr.id = p.id;
                    pr.product_id = p.product_id;
                    pr.msr_prx = p.msr_prx;
                    pr.product_name = p.product_name;
                
                    pr.purchase_prx =Convert.ToDecimal( p.purchase_prx);
                     pr.manufacturer = p.manufacturer;
                    productToDisplays.Add(pr);
                    
                }
            if(s>0)
                {
                productToDisplays = productToDisplays.Where(pr => pr.purchase_prx<=s ).ToList() ;
                
                
                }
            var orderByManufacturer = from k in productToDisplays
                                      orderby k.manufacturer
                                select k;

            return View(orderByManufacturer);
        }
         [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            PharmaTest_NREntities entities = new PharmaTest_NREntities();
            entities.Products.Add(product);
            entities.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            PharmaTest_NREntities entities = new PharmaTest_NREntities();
           Product product = entities.Products.SingleOrDefault(pr=>pr.id==id);
            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            PharmaTest_NREntities entities = new PharmaTest_NREntities();
            if (ModelState.IsValid)
            { 
            entities.Entry(product).State = EntityState.Modified;
            entities.SaveChanges();
            return RedirectToAction("Index");
            }
            return View(product);
        }
    }
}