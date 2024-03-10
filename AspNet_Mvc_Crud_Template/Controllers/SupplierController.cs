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
    public class SupplierController : Controller
    {
        private readonly NorthwindContext db;

        public SupplierController(NorthwindContext context)
        {
            db = context;
        }

        public IActionResult Index(int? page)
        {
            int pageSize = 7;
            int pageNumber = page ?? 1;//değişkenin null olup olmadığını kontrol eder ve null ise belirtilen varsayılan değeri kullanır.

            IEnumerable<Supplier> suppliyerList = db.Suppliers.OrderBy(c => c.SupplierId)
                                            .Skip((pageNumber - 1) * pageSize)
                                            .Take(pageSize)
                                            .ToList();

            int totalProducts = db.Suppliers.Count();

            var viewModel = new IndexVM<Supplier>();
            viewModel.Items = suppliyerList;
            viewModel.TotalCount = totalProducts;
            viewModel.CurrentPage = pageNumber;
            viewModel.PageSize = pageSize;

            return View(viewModel);
        }
        public IActionResult Details(int? id)
        {
            if (id == null || db.Suppliers == null)
            {
                return NotFound();
            }

            var supplier =db.Suppliers
                .FirstOrDefault(m => m.SupplierId == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Add(supplier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || db.Suppliers == null)
            {
                return NotFound();
            }

            var supplier =db.Suppliers.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [HttpPost]
        public IActionResult Edit(int id,Supplier supplier)
        {
            if (id != supplier.SupplierId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(supplier);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                   
                }
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        public IActionResult Delete(int? id)
        {
            try
            {
                var suppliyer = db.Suppliers.Find(id);

                if (suppliyer == null)
                {
                    return RedirectToAction("Index");
                }

                db.Suppliers.Remove(suppliyer);
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
            return RedirectToAction("Index");
        }
    }
}
