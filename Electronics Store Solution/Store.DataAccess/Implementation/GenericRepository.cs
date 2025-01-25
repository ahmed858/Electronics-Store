using Microsoft.EntityFrameworkCore;
using Store.DataAccess.Data;
using Store.Entities.Models;
using Store.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccess.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _dbset;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbset = _context.Set<T>();
        }
        public void Add(T entity)
        {
            //Categories.add(category);
            _dbset.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate = null, string? IncludeWords = null)
        {
            IQueryable<T> query = _dbset;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (IncludeWords != null)
            {

                // context.product.include("Category,Logos,Users")
                foreach (var word in IncludeWords.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {

                    query = query.Include(word);
                }
            }

            return query.ToList();// convert it to IEnumerable
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>>? predicate = null, string? IncludeWords = null)
        {

            IQueryable<T> query = _dbset;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (IncludeWords != null)
            {
                // context.product.include("Category,Logos,Users")
                foreach (var word in IncludeWords.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query.Include(word);
                }
            }

            return query.SingleOrDefault();// convert it to IEnumerable
        }

        public void Remove(T entity)
        {
            _dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbset.RemoveRange(entities);

        }
    }
}
