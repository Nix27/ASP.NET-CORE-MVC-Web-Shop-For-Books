using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
            return View(objCategoryList);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category newCategory)
        {
            if (newCategory.Name == newCategory.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Display order can't be same like name!");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(newCategory);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }

            return View(newCategory);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var requestedCategory = _unitOfWork.Category.GetFirstOrDefault(i => i.Id == id);

            if (requestedCategory == null)
            {
                return NotFound();
            }

            return View(requestedCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category categoryForUpdate)
        {
            if (categoryForUpdate.Name == categoryForUpdate.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Display order can't be same like name!");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(categoryForUpdate);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }

            return View(categoryForUpdate);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var requestedCategory = _unitOfWork.Category.GetFirstOrDefault(i => i.Id == id);

            if (requestedCategory == null)
            {
                return NotFound();
            }

            return View(requestedCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var categoryForDelete = _unitOfWork.Category.GetFirstOrDefault(i => i.Id == id);
            if (categoryForDelete == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(categoryForDelete);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
