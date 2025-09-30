using Company.G02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        //Employee? GetByName(string name);
        //IEnumerable<Employee> GetAll();
        //Employee? Get(int id);
        //int Add(Employee mondel);
        //int Update(Employee model);
        //int Delete(Employee model);
    }
}
