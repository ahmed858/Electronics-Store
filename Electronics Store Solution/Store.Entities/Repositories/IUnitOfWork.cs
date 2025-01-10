using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Entities.Repositories
{
    public interface IUnitOfWork:IDisposable
    {
        // All entities in your application
        public ICategoryRepository Category { get; }
        public IProductRepository Product { get; }
        public int Complete();
    }
}
