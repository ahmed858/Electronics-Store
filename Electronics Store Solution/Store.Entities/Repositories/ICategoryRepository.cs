using Store.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Entities.Repositories
{
    public interface ICategoryRepository:IGenericRepository<Category>
    {
        
        public void Update(Category category);
            
    }
}
