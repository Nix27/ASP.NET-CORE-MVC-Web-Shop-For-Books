using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> listOfProducts = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return View(listOfProducts);
        }

        public IActionResult Details(int productId)
        {
            ShoopingCart cartObj = new()
            {
                Product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == productId, includeProperties: "Category,CoverType"),
                ProductId = productId,
                Amount = 1
            };
            return View(cartObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoopingCart shoopingCartObj)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoopingCartObj.ApplicationUserId = claim.Value;

            ShoopingCart cartFromDb = _unitOfWork.ShoopingCart.GetFirstOrDefault(
                u => u.ApplicationUserId == claim.Value && shoopingCartObj.ProductId == u.ProductId    
            );

            if ( cartFromDb == null )
            {
                _unitOfWork.ShoopingCart.Add(shoopingCartObj);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoopingCart.GetAll(sh => sh.ApplicationUser.Id == claim.Value).ToList().Count());
            }
            else
            {
                _unitOfWork.ShoopingCart.IncrementAmount(cartFromDb, shoopingCartObj.Amount);
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}