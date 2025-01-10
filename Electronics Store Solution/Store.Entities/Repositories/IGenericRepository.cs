using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Entities.Repositories
{
    public interface IGenericRepository<T> where T :class
    {
        // _context.Categories.Include("products").toList();
        // _context.Categories.Where(x=>x.id=id).toList();
        public IEnumerable<T> GetAll(Expression<Func<T,bool>>? predicate=null,string? IncludeWords=null);
        
        
        // _context.Categories.Include("products").ToSingleOrDefault()
        public T GetFirstOrDefault(Expression<Func<T, bool>>? predicate = null, string? IncludeWords = null);


        void Add(T entity);

        public void Remove(T entity);

        public void RemoveRange(IEnumerable<T> entities);


    }
}
