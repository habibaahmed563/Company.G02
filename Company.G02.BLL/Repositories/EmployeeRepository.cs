using Company.G02.BLL.Interfaces;
using Company.G02.DAL.Data.Contexts;
using Company.G02.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(CompanyDbContext context) : base(context) // Ask CLR Create Oject From CompanyDbContext
        {
            _Context = context;
        }

        private readonly CompanyDbContext _Context;

        public async Task<List<Employee>?> GetByNameAsync(string name)
        {
            return await _Context.Employees.Include(E=>E.Department).Where(E => E.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
    }
}
