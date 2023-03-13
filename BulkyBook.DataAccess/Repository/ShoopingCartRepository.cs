using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ShoopingCartRepository : Repository<ShoopingCart>, IShoopingCartRepository
    {
        public ShoopingCartRepository(ApplicationDbContext db) : base(db)
        {
        }

        public int DecrementAmount(ShoopingCart shoopingCart, int amount)
        {
            shoopingCart.Amount -= amount;
            return shoopingCart.Amount;
        }

        public int IncrementAmount(ShoopingCart shoopingCart, int amount)
        {
            shoopingCart.Amount += amount;
            return shoopingCart.Amount;
        }
    }
}
