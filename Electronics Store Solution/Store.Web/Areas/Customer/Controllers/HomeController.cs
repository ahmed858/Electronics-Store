using Microsoft.AspNetCore.Mvc;
using Store.DataAccess.Implementation;
using Store.Entities.Repositories;
using Store.Entities.ViewModels;

namespace Store.Web.Areas.Customer.Controllers
{
        [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork; 
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var product = _unitOfWork.Product.GetAll();
            return View(product);
        }
        public IActionResult ProductDetails(int id)
        {
            ProductDetailsVM productDetailsVM = new ProductDetailsVM()
            {
                Product = _unitOfWork.Product.GetFirstOrDefault(predicate: x => x.Id == id , IncludeWords: "Category"),
                CartCount = 1
            };
            ViewBag.CategoryName = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == productDetailsVM.Product.CategoryId).Name;

            return View(productDetailsVM);

        }
    }
}
