using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
        }

        public void Update(Product product)
        {
            var productForUpdate = _db.Products.FirstOrDefault(p => p.Id == product.Id);
            if (productForUpdate != null)
            {
                productForUpdate.Title = product.Title;
                productForUpdate.ISBN = product.ISBN;
                productForUpdate.Price = product.Price;
                productForUpdate.Price50 = product.Price50;
                productForUpdate.Price100 = product.Price100;
                productForUpdate.ListPrice = product.ListPrice;
                productForUpdate.Description = product.Description;
                productForUpdate.Author = product.Author;
                productForUpdate.CategoryId = product.CategoryId;
                productForUpdate.CoverTypeId = product.CoverTypeId;
                if (product.ImageURL != null)
                {
                    productForUpdate.ImageURL = product.ImageURL;
                }
            }
        }
    }
}
