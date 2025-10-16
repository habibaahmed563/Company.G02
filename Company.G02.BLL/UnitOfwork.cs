using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.BLL
{
    public class UnitOfwork : IUnitOfwork
    {
        private readonly CompanyDbContext _Context;
        public IDepartmentRepositories DepartmentRepository { get; } // Null

        public IEmployeeRepository EmployeeRepository { get; } // Null
        

        public UnitOfwork(CompanyDbContext context)
        {
            _Context = context;
            DepartmentRepository = new DepartmentRepository(_Context);
            EmployeeRepository = new EmployeeRepository(_Context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _Context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
           await _Context.DisposeAsync();
        }
    }
}
