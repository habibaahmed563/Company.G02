using Company.G02.BLL.Interfaces;
using Company.G02.DAL.Data.Contexts;
using Company.G02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepositories
    {
        private readonly CompanyDbContext _context;
        public DepartmentRepository(CompanyDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id);
        }
    }
}
