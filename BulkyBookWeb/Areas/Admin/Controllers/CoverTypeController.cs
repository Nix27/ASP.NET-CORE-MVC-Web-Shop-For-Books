using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = SD.Role_Admin)]
	public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var cvrTypes = _unitOfWork.CoverType.GetAll();
            return View(cvrTypes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(coverType);
                _unitOfWork.Save();
                TempData["success"] = "Cover type created successfully!";
                return RedirectToAction("Index");
            }

            return View(coverType);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var coverTypeForEdit = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);

            if (coverTypeForEdit == null)
            {
                return NotFound();
            }

            return View(coverTypeForEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(coverType);
                _unitOfWork.Save();
                TempData["success"] = "Cover type updated successfully!";
                return RedirectToAction("Index");
            }

            return View(coverType);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var coverTypeForDelete = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);

            if (coverTypeForDelete == null)
            {
                return NotFound();
            }

            return View(coverTypeForDelete);
        }

        public IActionResult DeletePOST(int? id)
        {
            var coverTypeForDelete = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);

            if (coverTypeForDelete == null)
            {
                return NotFound();
            }

            _unitOfWork.CoverType.Remove(coverTypeForDelete);
            _unitOfWork.Save();
            TempData["success"] = "Cover type deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
