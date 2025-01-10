using Store.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Entities.Repositories
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        public void Update(Product product);
    }
}
