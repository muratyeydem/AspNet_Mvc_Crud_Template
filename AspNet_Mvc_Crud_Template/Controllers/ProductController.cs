using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AspNet_Mvc_Crud_Template.Models.Entities;
using AspNet_Mvc_Crud_Template.Models.VM;

namespace AspNet_Mvc_Crud_Template.Controllers
{
    public class ProductController : Controller
    {
        private readonly NorthwindContext db;

        public ProductController(NorthwindContext context)
        {
            db = context;
        }

        public IActionResult Index(int? page)
        {
            int pageSize = 7;
            int pageNumber = page ?? 1;//değişkenin null olup olmadığını kontrol eder ve null ise belirtilen varsayılan değeri kullanır.

            IEnumerable<Product> productList = db.Products.OrderBy(c => c.ProductId)
                                            .Skip((pageNumber - 1) * pageSize)
                                            .Take(pageSize)
                                            .ToList();

            int totalProducts = db.Products.Count();

            var viewModel = new IndexVM<Product>(); 
            viewModel.Items = productList;
            viewModel.TotalCount = totalProducts;
            viewModel.CurrentPage = pageNumber;
            viewModel.PageSize = pageSize;

            return View(viewModel);
        }
        public IActionResult Details(int? id)
        {
            if (id == null || db.Products == null)
            {
                return NotFound();
            }

            var product = db.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefault(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        public IActionResult Add()
        {
            ViewBag.CategoryListesi = db.Categories.ToList();
            ViewBag.SpplyierListesi = db.Suppliers.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Add(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        public IActionResult Edit(int? id)
        {

            var product = db.Products.Find(id);

            ViewBag.CategoryListesi = db.Categories.ToList();
            ViewBag.SpplyierListesi = db.Suppliers.ToList();

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            try
            {
               
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the product: " + ex.Message);
            }

            return View(product);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var product = db.Products.Find(id);

                if (product == null)
                {
                    return RedirectToAction("Index");
                }

                db.Products.Remove(product);
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
            return RedirectToAction("Index");
        }

    }
}
