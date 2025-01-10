using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.DataAccess.Data;
using Store.DataAccess.Implementation;
using Store.Entities.Models;
using Store.Entities.Repositories;
using Store.Entities.ViewModels;

namespace Store.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            //_context.Categories.ToList();
            var Products = _unitOfWork.Product.GetAll();


            return View(Products);
        }

        #region Create Action
        [HttpGet]
        public IActionResult Create()// action view 
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem{
                   
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVM productVM, IFormFile file)// submit action
        {


            // server side validation 
            if (ModelState.IsValid)
            {

                string RootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileNameInServer = Guid.NewGuid().ToString();
                    string uploadedPlace = Path.Combine(RootPath, @"Images\Products");
                    string extension = Path.GetExtension(file.FileName);
                    string absolutePath = Path.Combine(uploadedPlace, fileNameInServer + extension);

                    using (var fileStream = new FileStream( absolutePath , FileMode.Create ))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImgURL = absolutePath;
                }



                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Complete();

                TempData["ToasterMessage"] = "Product Created Successfuly";
                TempData["ToasterNotification"] = "success";
                return RedirectToAction("Index");
            }
            return View(productVM);// that take 
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
            // get that product from database
            //_context.Categories.Find(id);
            var productInDb = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);

            return View(productInDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)// submit action
        {
            // server side validation 
            if (ModelState.IsValid)
            {
                //_context.Categories.Update(product);
                _unitOfWork.Product.Update(product);
                //_context.SaveChanges();//commit
                _unitOfWork.Complete();

                TempData["ToasterMessage"] = "Product Updated Successfuly";
                TempData["ToasterNotification"] = "info";
                return RedirectToAction("Index");
            }

            return View(product);// that take 
        }
        #endregion

        #region Delete Action

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var productInDb = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);  //_context.Categories.Find(id);
            return View(productInDb);
        }
        public IActionResult DeleteCategory(int? id)
        {

            var productInDb = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id); //_context.Categories.Find(id);

            if (productInDb == null)
                return NotFound();

            //_context.Categories.Remove(productInDb);
            //_context.SaveChanges();
            _unitOfWork.Product.Remove(productInDb);
            _unitOfWork.Complete();

            TempData["ToasterMessage"] = "Product Deleted Successfuly";
            TempData["ToasterNotification"] = "success";
            return RedirectToAction("Index");
        }


        #endregion
    }
}
