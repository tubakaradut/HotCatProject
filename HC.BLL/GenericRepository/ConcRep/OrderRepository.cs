using HC.BLL.GenericRepository.BaseRep;
using HC.BLL.GenericRepository.IntRep;
using HC.Entity.Model;

namespace HC.BLL.GenericRepository.ConcRep
{
    public class OrderRepository:BaseRepository<Order>, IRepository<Order>
    {
    }
}
