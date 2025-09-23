using Company.G02.BLL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    // MVC Controller 
    public class DepartmentController : Controller
    {
        private readonly DepartmentRepository _departmentRepository;

        //Ask CLR Create oject From DepartmentRepository

        public DepartmentController(DepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        [HttpGet] // Get : /Department/Index
        public IActionResult Index()
        {
            DepartmentRepository departmentRepository = _departmentRepository;
            var departments = _departmentRepository.GetAll();

            return View(departments);
        }
    }
}
