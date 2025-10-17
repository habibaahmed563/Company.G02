using Company.G02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.BLL.Interfaces
{
    public interface IDepartmentRepositories : IGenericRepository<Department>
    {
        Task<Department?> GetByIdAsync(int id);
    }
}
