using Company.G02.BLL;
using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Threading.Tasks;

namespace Company.G02.PL.Controllers
{
    // MVC Controller 
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepositories _departmentRepository;

        private readonly IUnitOfwork _unitOfwork;

        //Ask CLR Create oject From DepartmentRepository

        public DepartmentController(/*IDepartmentRepositories departmentRepository*/ IUnitOfwork unitOfwork)
        {
            //_departmentRepository = departmentRepository;
            _unitOfwork = unitOfwork;
        }
        [HttpGet] // Get : /Department/Index
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfwork.DepartmentRepository.GetAllAsync();

            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto model)
        {
            if(ModelState.IsValid) // Server Side Validation
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };
                await _unitOfwork.DepartmentRepository.AddAsync(department);
                var count = await _unitOfwork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id,string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id"); // 400

            var department = await _unitOfwork.DepartmentRepository.GetAsync(id.Value);
            if(department is null) return NotFound(new { StatusCode = 404, Message = $"Department With This Id :{id} is not found" });

            return View(viewName ,department);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest("Invalid Id"); // 400

            var department = await _unitOfwork.DepartmentRepository.GetByIdAsync(id.Value);
            if (department is null) 
             return NotFound(new { StatusCode = 404, Message = $"Department With This Id :{id} is not found" });

            return  View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( [FromRoute]int id, Department department)
        {
            if (id != department.Id) return BadRequest();
            if (!ModelState.IsValid)
            {

                _unitOfwork.DepartmentRepository.Update(department);
                var count = await _unitOfwork.CompleteAsync();

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(department);
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

            return await Details(id,"Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _unitOfwork.DepartmentRepository.GetByIdAsync(id);
            if (department == null) return NotFound();

                _unitOfwork.DepartmentRepository.Delete(department);
                var count = await _unitOfwork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            

            return View(department);
        }
    }
}
