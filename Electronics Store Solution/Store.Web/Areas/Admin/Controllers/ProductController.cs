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

        #region Index Action & use getdata method to datatable frontend 

        public IActionResult Index()
        {
   
            return View();
        }
        public IActionResult GetData()
        {
            var products = _unitOfWork.Product.GetAll(IncludeWords: "Category");
            foreach (var item in products)
            {
                Console.WriteLine(item.Category.Name);
            }
            return Json(new { data = products });
        }
        #endregion
     

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
                Console.WriteLine(RootPath);
                if (file != null)
                {
                    string fileNameInServer = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(file.FileName);
                    //"80aa8105-4cf4-4fa6-bfea-9e33f799ad0d"

                    string uploadedPlace = Path.Combine(RootPath, "Images\\Products");

                    string PathInDB = Path.Combine("Images\\Products", fileNameInServer + extension);



                    using (var fileStream = new FileStream(Path.Combine( uploadedPlace , fileNameInServer + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImgURL = PathInDB;
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

            ProductVM productVM = new ProductVM()
            {
                Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id),

                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem{
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            

            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVM productVM, IFormFile? file)// file is nullable cause the model state be valid 
        {
            // server side validation 
            if (ModelState.IsValid)
            {

                string RootPath = _webHostEnvironment.WebRootPath;
                Console.WriteLine(RootPath);
                if (file != null)
                {
                    string fileNameInServer = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(file.FileName);
                    //"80aa8105-4cf4-4fa6-bfea-9e33f799ad0d"

                    string uploadedPlace = Path.Combine(RootPath, "Images\\Products");

                    string PathInDB = Path.Combine("Images\\Products", fileNameInServer + extension);

                    // Remove old file in uploaded place
                    if (productVM.Product.ImgURL != null)
                    {
                        //  ImgURL = Images\Products\0d3b62c0-2bb2-4b7e-a24b-8f2b2e721e37.png

                        var oldImg = Path.Combine(RootPath, productVM.Product.ImgURL).TrimStart('\\');
                        if (System.IO.File.Exists(oldImg))
                        {
                            System.IO.File.Delete(oldImg);
                        }
                    }


                    using (var fileStream = new FileStream(Path.Combine(uploadedPlace, fileNameInServer + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImgURL = PathInDB;
                }


                //_context.Categories.Update(product);
                _unitOfWork.Product.Update(productVM.Product);
                //_context.SaveChanges();//commit
                _unitOfWork.Complete();

                TempData["ToasterMessage"] = "Product Updated Successfuly";
                TempData["ToasterNotification"] = "info";
                return RedirectToAction("Index");
            }

            return View(productVM);// that take 
        }
        #endregion

        #region Delete Action

        [HttpDelete]
        public IActionResult Delete(int? id)
        {

            var productInDb = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id); //_context.Categories.Find(id);

            if (productInDb == null)
                return Json(new { success = false, message = "Error while Deleting" });


            // Remove old file in uploaded place
            if (productInDb.ImgURL != null)
            {
                //  ImgURL = Images\Products\0d3b62c0-2bb2-4b7e-a24b-8f2b2e721e37.png

                var oldImg = Path.Combine(_webHostEnvironment.WebRootPath, productInDb.ImgURL).TrimStart('\\');
                if (System.IO.File.Exists(oldImg))
                {
                    System.IO.File.Delete(oldImg);
                }
            }

            _unitOfWork.Product.Remove(productInDb);
            _unitOfWork.Complete();

            TempData["ToasterMessage"] = "Product Deleted Successfuly";
            TempData["ToasterNotification"] = "error";
            return Json(new { success = true, message = "Product has been deleted" });

        }


        #endregion
    }
}
