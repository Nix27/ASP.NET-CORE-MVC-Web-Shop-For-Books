using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IShoopingCartRepository : IRepository<ShoopingCart>
    {
        int IncrementAmount(ShoopingCart shoopingCart, int amount);
        int DecrementAmount(ShoopingCart shoopingCart, int amount);
    }
}
