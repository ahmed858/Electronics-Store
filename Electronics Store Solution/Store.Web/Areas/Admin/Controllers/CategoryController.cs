using Microsoft.AspNetCore.Mvc;
using Store.DataAccess.Data;
using Store.DataAccess.Implementation;
using Store.Entities.Models;
using Store.Entities.Repositories;

namespace Store.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            //_context.Categories.ToList();
            var Categories = _unitOfWork.Category.GetAll();


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
                //_context.Categories.Add(category);
                //_context.SaveChanges();

                _unitOfWork.Category.Add(category);
                _unitOfWork.Complete();
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
            //_context.Categories.Find(id);
            var cate = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);

            return View(cate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)// submit action
        {
            // server side validation 
            if (ModelState.IsValid)
            {
                //_context.Categories.Update(category);
                _unitOfWork.Category.Update(category);
                //_context.SaveChanges();//commit
                _unitOfWork.Complete();

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
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var Cate = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);  //_context.Categories.Find(id);
            return View(Cate);
        }
        public IActionResult DeleteCategory(int? id)
        {

            var Cate = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id); //_context.Categories.Find(id);

            if (Cate == null)
                return NotFound();

            //_context.Categories.Remove(Cate);
            //_context.SaveChanges();
            _unitOfWork.Category.Remove(Cate);
            _unitOfWork.Complete();
            TempData["ToasterMessage"] = "Category Deleted Successfuly";
            TempData["ToasterNotification"] = "success";
            return RedirectToAction("Index");
        }


        #endregion
    }
}
