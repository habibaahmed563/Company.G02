using AutoMapper;
using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace Company.G02.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepositories _departmentRepository;
        private readonly IMapper _mapper;

        //Ask CLR Create oject From IEmployeeRepository

        public EmployeeController
            (IEmployeeRepository employeeRepository,
            IDepartmentRepositories departmentRepositories,
            IMapper mapper
            )
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepositories;
            _mapper = mapper;
        }
        [HttpGet] // Get : /Department/Index
        public IActionResult Index(string? SearchInout)
        {
            IEnumerable<Employee> employees;
            if(string.IsNullOrEmpty(SearchInout))
            {
                employees = _employeeRepository.GetAll();
            }
            else
            {
                employees = _employeeRepository.GetByName(SearchInout);
            }
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
                var employee = _mapper.Map<Employee>(model);
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
        public IActionResult Details(int? id)
        {
            if (id is null) return BadRequest("Invalid Id"); // 400

            var employee = _employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, Message = $"Employee With This Id :{id} is not found" });

            var dto = _mapper.Map<CreateEmployeeDto>(employee);
            return View(dto);
        }

        [HttpGet]
        public IActionResult Edit(int? id,string viewName = "Edit")
        {
            if (id == null)
                return BadRequest("Invalid Id");
            
            var departments = _departmentRepository.GetAll();
            ViewData["departments"] = departments;

            var employee = _employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, message = $"employee With Id :{id} Was Not Found" });

            var dto = _mapper.Map<CreateEmployeeDto>(employee);

            return View(viewName,dto);
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int id, CreateEmployeeDto model,string viewName="Edit")
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                var count = _employeeRepository.Update(employee);
                
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(viewName,model);
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

            return Edit(id,"Delete");
        }


        [HttpPost]
        public IActionResult Delete([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                var count = _employeeRepository.Delete(employee);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
    }
}
