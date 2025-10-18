using AutoMapper;
using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Company.G02.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Threading.Tasks;

namespace Company.G02.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfwork _unitOfwork;

        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepositories _departmentRepository;
        private readonly IMapper _mapper;

        //Ask CLR Create oject From IEmployeeRepository

        public EmployeeController(
            //(IEmployeeRepository employeeRepository,
            //IDepartmentRepositories departmentRepositories,
            IUnitOfwork unitOfwork,
            IMapper mapper
            )
        {
            _unitOfwork = unitOfwork;
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepositories;
            _mapper = mapper;
        }
        [HttpGet] // Get : /Department/Index
        public async Task<IActionResult> Index(string? SearchInout)
        {
            IEnumerable<Employee> employees;
            if(string.IsNullOrEmpty(SearchInout))
            {
                employees = await _unitOfwork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                employees = await _unitOfwork.EmployeeRepository.GetByNameAsync(SearchInout);
            }
                // Dictionary : 3 Property
                // 1.ViewData : Transfer Extra Information From Controller (Action) To View
                //ViewData["Message"] = "Hello From ViewData";

                // 2.ViewBag  : Transfer Extra Information From Controller (Action) To View
                //ViewBag.Message = "Hello From ViewBag";

                return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfwork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                if (model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }

                var employee = _mapper.Map<Employee>(model);
                await _unitOfwork.EmployeeRepository.AddAsync(employee);
                var count = await _unitOfwork.CompleteAsync();
                if (count > 0)
                {
                    TempData["Message"] = "Employee is Created";
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return BadRequest("Invalid Id"); // 400

            var employee = await _unitOfwork.EmployeeRepository.GetAsync(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, Message = $"Employee With This Id :{id} is not found" });

            var dto = _mapper.Map<CreateEmployeeDto>(employee);
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id,string viewName = "Edit")
        {
            if (id == null)
                return BadRequest("Invalid Id");
            
            var departments = await _unitOfwork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;

            var employee = await _unitOfwork.EmployeeRepository.GetAsync(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, message = $"employee With Id :{id} Was Not Found" });

            var dto = _mapper.Map<CreateEmployeeDto>(employee);

            return View(viewName,dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateEmployeeDto model,string viewName="Edit")
        {
            if (ModelState.IsValid)
            {
                if(model.ImageName is not null && model.Image is not null)
                {
                    DocumentSettings.DeleteFile(model.ImageName, "images");
                }
                if(model.Image is not null)
                {
                   model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }

                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                 _unitOfwork.EmployeeRepository.Update(employee);
                var count = await _unitOfwork.CompleteAsync();
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
        public async Task<IActionResult> Delete(int? id)
        {
            //if (id is null) return BadRequest("Invalid Id"); // 400

            //var department = _departmentRepository.Get(id.Value);
            //if (department is null) return NotFound(new { StatusCode = 404, Message = $"Department With This Id :{id} is not found" });

            return await Edit(id,"Delete");
        }


        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                _unitOfwork.EmployeeRepository.Delete(employee);
                var count = await _unitOfwork.CompleteAsync();
                if (count > 0)
                {
                    if(model.ImageName is not null)
                    {
                        DocumentSettings.DeleteFile(model.ImageName, "images");
                    }
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
    }
}
