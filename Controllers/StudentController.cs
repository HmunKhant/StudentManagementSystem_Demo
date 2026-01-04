using StudentManagementSystem.Models.ViewModels;
using StudentManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace StudentManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly ILoggingService _loggingService;

        public StudentController(IStudentService studentService, ILoggingService loggingService)
        {
            _studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
        }

        // LIST
        public ActionResult Index(string search, int page = 1, int pageSize = 10)
        {
            try
            {
                _loggingService.Log("View Students List", $"Search term: {search ?? "None"}, Page: {page}");
                
                var model = _studentService.GetStudentsPaged(search, page, pageSize);

                return View(model);
            }
            catch (Exception ex)
            {
                _loggingService.Log("Error - View Students List", ex.Message, true);
                ViewBag.ErrorMessage = "An error occurred while loading students. Please try again.";
                return View(new PagedStudentListVM { Students = new List<StudentListVM>() });
            }
        }

        // CREATE GET
        public ActionResult Create()
        {
            try
            {
                _loggingService.Log("View Create Student Form", "User accessed registration page");
                
                var vm = _studentService.GetCreateViewModel();

                return View(vm);
            }
            catch (Exception ex)
            {
                _loggingService.Log("Error - View Create Student Form", ex.Message, true);
                ViewBag.ErrorMessage = "An error occurred while loading the form. Please try again.";
                return View(new StudentCreateVM());
            }
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentCreateVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.Subjects = _studentService.GetCreateViewModel().Subjects;
                    return View(model);
                }

                string errorMessage;
                if (!_studentService.CreateStudent(model, out errorMessage))
                {
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        if (errorMessage.Contains("NRIC"))
                        {
                            ModelState.AddModelError("NRIC", errorMessage);
                        }
                        else
                        {
                            ModelState.AddModelError("", errorMessage);
                        }
                    }
                    model.Subjects = _studentService.GetCreateViewModel().Subjects;
                    return View(model);
                }

                TempData["SuccessMessage"] = "Student registered successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _loggingService.Log("Error - Create Student", ex.Message, true);
                ModelState.AddModelError("", "An error occurred while saving. Please try again.");
                model.Subjects = _studentService.GetCreateViewModel().Subjects;
                return View(model);
            }
        }

        // EDIT GET
        public ActionResult Edit(string nric)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nric))
                {
                    _loggingService.Log("Error - Edit Student", "NRIC parameter is missing", true);
                    return RedirectToAction("Index");
                }

                _loggingService.Log("View Edit Student Form", $"NRIC: {nric}");
                
                var vm = _studentService.GetEditViewModel(nric);

                if (vm == null)
                {
                    _loggingService.Log("Error - Edit Student", $"Student not found with NRIC: {nric}", true);
                    TempData["ErrorMessage"] = "Student not found.";
                    return RedirectToAction("Index");
                }

                return View(vm);
            }
            catch (Exception ex)
            {
                _loggingService.Log("Error - View Edit Student Form", ex.Message, true);
                ViewBag.ErrorMessage = "An error occurred while loading the student. Please try again.";
                return RedirectToAction("Index");
            }
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StudentEditVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var existingVm = _studentService.GetEditViewModel(model.NRIC);
                    if (existingVm != null)
                    {
                        model.Subjects = existingVm.Subjects;
                    }
                    return View(model);
                }

                string errorMessage;
                if (!_studentService.UpdateStudent(model, out errorMessage))
                {
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        if (errorMessage.Contains("NRIC"))
                        {
                            ModelState.AddModelError("NRIC", errorMessage);
                        }
                        else if (errorMessage.Contains("not found"))
                        {
                            TempData["ErrorMessage"] = errorMessage;
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", errorMessage);
                        }
                    }
                    var existingVm = _studentService.GetEditViewModel(model.NRIC);
                    if (existingVm != null)
                    {
                        model.Subjects = existingVm.Subjects;
                    }
                    return View(model);
                }

                TempData["SuccessMessage"] = "Student updated successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _loggingService.Log("Error - Edit Student", ex.Message, true);
                ModelState.AddModelError("", "An error occurred while saving. Please try again.");
                var existingVm = _studentService.GetEditViewModel(model.NRIC);
                if (existingVm != null)
                {
                    model.Subjects = existingVm.Subjects;
                }
                return View(model);
            }
        }
    }

}