using HC.BLL.GenericRepository.IntRep;
using HC.Dal.Context;
using HC.Entity.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace HC.BLL.GenericRepository.BaseRep
{
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private HCContext _db;
        
        public BaseRepository()
        {
            _db = new HCContext();
        }

        int Save()
        {
            return _db.SaveChanges();
        }

        public T Add(T item)
        {
            item.CreatedDate = DateTime.Now;
            T result = _db.Set<T>().Add(item);
            Save();
            return result;
        }
        public int Update(T item)
        {
            T toBeUpdated = Find(item.Id);
            item.ModifiedDate = DateTime.Now;

            _db.Entry(toBeUpdated).CurrentValues.SetValues(item);
            return Save();
        }
        public void Delete(T item)
        {
            item.DeletedDate = DateTime.Now;
            item.IsActive = false;
            Save();
        }

        public void DeleteRange(List<T> item)
        {
            foreach (T element in item)
            {
                Delete(element);
            }
        }
        public bool Any(Expression<Func<T, bool>> exp)
        {
            return _db.Set<T>().Any(exp);
        }

        public T Find(int id)
        {
            return _db.Set<T>().Find(id);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> exp, string[] includeTables = null)
        {
            var dbQuery = new List<T>().AsQueryable();  //AsQueryable, sorgunun bir IQueryable(koşullu sorgulama vb. yapmak) örneğine dönüştürülmesine izin veren bir yöntemdir. 
            var query = _db.Set<T>();
            if (includeTables != null)
            {
                foreach (var include in includeTables)
                {
                    dbQuery = query.Include(include);
                }
                return dbQuery.FirstOrDefault(exp);
            }

            return query.FirstOrDefault(exp);
        }


        public List<T> Where(Expression<Func<T, bool>> exp, string[] includeTables = null)
        {
            var dbQuery = new List<T>().AsQueryable();
            var query = _db.Set<T>();
            if (includeTables != null)
            {
                foreach (var include in includeTables)
                {
                    dbQuery = query.Include<T>(include);
                }
                return dbQuery.Where(exp).ToList();
            }

            return query.Where(exp).ToList();
        }

        public List<T> GetActives(string[] includeTables = null) // eager loading yaptığımız için istediğimiz zaman alt tabloları da yüklenmesi için parametre olarak alt tabloların isimlerini array string olarak göndermemiz gerekiyor.
        {
            return Where(x => x.IsActive, includeTables);
        }


        public List<T> GetModifieds(string[] includeTables = null)
        {
            return Where((x => x.ModifiedDate != null && x.ModifiedById != null && (x.DeletedDate == null && x.DeletedById == null)), includeTables);
        }

        public List<T> GetPassives(string[] includeTables = null)
        {
            return Where((x => x.IsActive == false), includeTables);
        }
        public List<T> GetAll(string[] includeTables = null)
        {
            var dbQuery = new List<T>().AsQueryable();
            var query = _db.Set<T>();
            if (includeTables != null)
            {
                foreach (var include in includeTables)
                {
                    dbQuery = query.Include(include);
                }
                return dbQuery.ToList();
            }

            return query.ToList();
        }

    }
}
