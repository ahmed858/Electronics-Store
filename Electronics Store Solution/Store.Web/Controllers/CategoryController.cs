using Microsoft.AspNetCore.Mvc;
using Store.Web.Data;
using Store.Web.Models;

namespace Store.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var Categories = _context.Categories.ToList();
            return View(Categories);
        }

        #region Create Action
        [HttpGet]
        public IActionResult Create()// action view 
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)// submit action
        {
            // server side validation 
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                TempData["ToasterMessage"] = "Category Created Successfuly";
                TempData["ToasterNotification"] = "success";
                return RedirectToAction("Index");
            }
            return View(category);// that take 
        }
        #endregion

        #region Edit Action

        [HttpGet]
        public IActionResult Edit(int? id)// Edit view 
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            // get that category from database
            var cate = _context.Categories.Find(id);
            return View(cate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)// submit action
        {
            // server side validation 
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();//commit
                TempData["ToasterMessage"] = "Category Updated Successfuly";
                TempData["ToasterNotification"] = "info";
                return RedirectToAction("Index");
            }

            return View(category);// that take 
        }
        #endregion

        #region Delete Action

        public IActionResult Delete(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            var Cate = _context.Categories.Find(id);
            return View(Cate);
        }
        public IActionResult DeleteCategory(int? id)
        {
             
            var Cate = _context.Categories.Find(id);

            if(Cate == null)
                return NotFound();

            _context.Categories.Remove(Cate);
            _context.SaveChanges();
            TempData["ToasterMessage"] = "Category Deleted Successfuly";
            TempData["ToasterNotification"] = "success";
            return RedirectToAction("Index");
        }


        #endregion
    }
}
