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
    public class ProductRepository:GenericRepository<Product>,IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context):base(context)
        { 
            _context = context;
        }

        public void Update(Product product)
        {
            var productInDb = _context.Products.SingleOrDefault(x=>x.Id==product.Id);
            if (productInDb != null)
            {
                productInDb.Name = product.Name;
                productInDb.Description = product.Description;
                productInDb.Price=product.Price;
                productInDb.ImgURL = product.ImgURL;
                productInDb.CategoryId = product.CategoryId;
            }
            
        }
    }
}
