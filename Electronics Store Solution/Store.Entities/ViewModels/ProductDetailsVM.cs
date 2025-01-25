using Store.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Entities.ViewModels
{
    public class ProductDetailsVM
    {
        public Product Product { get; set; }
        [Range(1,100,ErrorMessage ="Your quantity are out of stock")]
        public int CartCount { get; set; }
    }
}
