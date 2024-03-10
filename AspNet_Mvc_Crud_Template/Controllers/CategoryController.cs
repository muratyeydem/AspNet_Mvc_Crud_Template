using AspNet_Mvc_Crud_Template.Models.Entities;
using AspNet_Mvc_Crud_Template.Models.VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNet_Mvc_Crud_Template.Controllers
{
	public class CategoryController : Controller
	{
		NorthwindContext db;
        public CategoryController(NorthwindContext context)
        {
            db = context;
        }
        public IActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;//değişkenin null olup olmadığını kontrol eder ve null ise belirtilen varsayılan değeri kullanır.

            IEnumerable<Category> categoryList = db.Categories.OrderBy(c => c.CategoryId)
                                            .Skip((pageNumber - 1) * pageSize)
                                            .Take(pageSize)
                                            .ToList();

            int totalCategories = db.Categories.Count();

            var viewModel = new IndexVM<Category>(); // Parametresiz yapılandırıcıyı kullanarak örnek oluşturuldu.
            viewModel.Items = categoryList;
            viewModel.TotalCount = totalCategories;
            viewModel.CurrentPage = pageNumber;
            viewModel.PageSize = pageSize;

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Category category, IFormFile picture)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (picture != null && picture.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            picture.CopyTo(memoryStream);
                            category.Picture = memoryStream.ToArray();
                        }
                    }
                    db.Categories.Add(category);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while adding the category: " + ex.Message);
            }

            return View(category);
        }

        public IActionResult Detail(int id)
        {
            return View(db.Categories.Find(id));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = db.Categories.Find(id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category, IFormFile picture)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (picture != null && picture.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            picture.CopyTo(memoryStream);
                            category.Picture = memoryStream.ToArray();
                        }
                    }
                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the category: " + ex.Message);
            }

            return View(category);
        }

        public IActionResult Delete(int id)
		{
			try
			{
				var category = db.Categories.Find(id);

				if (category == null)
				{
					return RedirectToAction("Index");
				}

				db.Categories.Remove(category);
				db.SaveChanges();
			}
			catch (Exception)
			{

			}
			return RedirectToAction("Index");
		}
	}
}
