using HC.BLL.GenericRepository.BaseRep;
using HC.BLL.GenericRepository.IntRep;
using HC.Entity.Model;

namespace HC.BLL.GenericRepository.ConcRep
{
    public  class EmployeeRepository:BaseRepository<Employee>, IRepository<Employee>
    {
    }
}
