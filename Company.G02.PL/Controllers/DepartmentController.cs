using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentDto model)
        {
            if(ModelState.IsValid) // Server Side Validation
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };
                var count = _departmentRepository.Add(department);
                if(count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id is null) return BadRequest("Invalid Id"); // 400

            var department = _departmentRepository.Get(id.Value);
            if(department is null) return NotFound(new { StatusCode = 404, Message = $"Department With This Id :{id} is not found" });

            return View(department);
        }


    }
}
