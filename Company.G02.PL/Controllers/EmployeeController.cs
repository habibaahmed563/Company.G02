using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepositories _departmentRepository;

        //Ask CLR Create oject From IEmployeeRepository

        public EmployeeController(IEmployeeRepository employeeRepository,IDepartmentRepositories departmentRepositories)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepositories;
        }
        [HttpGet] // Get : /Department/Index
        public IActionResult Index()
        {
            var employees = _employeeRepository.GetAll();
            // Dictionary : 3 Property
            // 1.ViewData : Transfer Extra Information From Controller (Action) To View
            //ViewData["Message"] = "Hello From ViewData";

            // 2.ViewBag  : Transfer Extra Information From Controller (Action) To View
            //ViewBag.Message = "Hello From ViewBag";

            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var departments = _departmentRepository.GetAll();
            ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var employee = new Employee()
                {
                    Name = model.Name,
                    Address = model.Address,
                    Age = model.Age,
                    CreateAt = model.CreateAt,
                    HiringDate = model.HiringDate,
                    Email = model.Email,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    Phone = model.Phone,
                    Salary = model.Salary
                };
                var count = _employeeRepository.Add(employee);
                if (count > 0)
                {
                    TempData["Message"] = "Employee is Created";
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id"); // 400

            var employee = _employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, Message = $"Employee With This Id :{id} is not found" });

            return View(viewName, employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null) return BadRequest("Invalid Id"); // 400

            var employee = _employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, Message = $"Employee With This Id :{id} is not found" });
            var employeeDto = new CreateEmployeeDto()
            {
                Name = employee.Name,
                Address = employee.Address,
                Age = employee.Age,
                CreateAt = employee.CreateAt,
                HiringDate = employee.HiringDate,
                Email = employee.Email,
                IsActive = employee.IsActive,
                IsDeleted = employee.IsDeleted,
                Phone = employee.Phone,
                Salary = employee.Salary
            };
            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                //if (id != model.Id) return BadRequest(); //400
                var employee = new Employee()
                {
                    Id = id,
                    Name = model.Name,
                    Address = model.Address,
                    Age = model.Age,
                    CreateAt = model.CreateAt,
                    HiringDate = model.HiringDate,
                    Email = model.Email,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    Phone = model.Phone,
                    Salary = model.Salary
                };
                var count = _employeeRepository.Update(employee);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }


        #region [HttpPost]

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int id, UpdateDepartmentDto model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var department = new Department()
        //        {
        //            Id = id,
        //            Name = model.Name,
        //            Code = model.Code,
        //            CreateAt = model.CreateAt
        //        };
        //        var count = _departmentRepository.Update(department);
        //        if (count > 0)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }

        //    return View(model);
        //} 
        #endregion

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            //if (id is null) return BadRequest("Invalid Id"); // 400

            //var department = _departmentRepository.Get(id.Value);
            //if (department is null) return NotFound(new { StatusCode = 404, Message = $"Department With This Id :{id} is not found" });

            return Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Employee model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest(); //400
                var count = _employeeRepository.Delete(model);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
    }
}
