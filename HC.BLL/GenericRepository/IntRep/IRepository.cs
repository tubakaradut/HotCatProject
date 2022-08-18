using HC.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HC.BLL.GenericRepository.IntRep
{
    public interface IRepository<T> where T : BaseEntity
    {
        //Eager loading kullandıgımız için ilişkili olan entityler içindeki ilişkili olan tablolar null olarak gelir.Include yöntemi ile sorgu esnasında bu doldurulur. 


        //List Commands
        List<T> GetAll(string[] includeTables = null);
        List<T> GetPassives(string[] includeTables = null);
        List<T> GetModifieds(string[] includeTables = null);
        List<T> GetActives(string[] includeTables = null);


        //Modification Commands
        T Add(T item);
        void Delete(T item);
        void DeleteRange(List<T> item);
        int Update(T item);


        //Expression Commands
        List<T> Where(Expression<Func<T, bool>> exp, string[] includeTables = null);
        bool Any(Expression<Func<T, bool>> exp);
        T FirstOrDefault(Expression<Func<T, bool>> exp, string[] includeTables = null);


        //Find Command
        T Find(int id);


    }
}
