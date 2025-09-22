using Company.G02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.BLL.Interfaces
{
    public interface IDepartmentRepositories
    {
        IEnumerable<Department> GetAll();
        Department? Get(int id);
        int Add(Department mondel);
        int Update(Department model);
        int Delete(Department model);
    }
}
