using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using WebShopApp.Core.Contracts;
using WebShopApp.Infrastructure.Data;
using WebShopApp.Infrastructure.Data.Domain;

namespace WebShopApp.Core.Services
{
    public class ProductService:IProductService
    {
        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Create(string name, int brandId, int categoryId, string picture, int quantity, decimal price, decimal discount)
        {
            Product item = new Product
            {
                ProductName = name,
                Brand = _context.Brands.Find(brandId),
                Category = _context.Categories.Find(categoryId),

                Picture = picture,
                Quantity = quantity,
                Price = price,
                Discount = discount
            };
        _context.Products.Add(item);
            return _context.SaveChanges() != 0;
        }

        public Product GetProductById(int productId)
        {
            return _context.Products.Find(productId);
        }

        public List<Product> GetProducts()
        {
            List<Product> products= _context.Products.ToList();
            return products;
        }

        public List<Product> GetProducts(string searchCategoryName, string searchBrandName)
        {
            List<Product> products = _context.Products.ToList();
            if (!String.IsNullOrEmpty(searchCategoryName)&&!String.IsNullOrEmpty(searchBrandName)) 
            {
                products=products.Where(x=>x.Category.CategoryName.ToLower().Contains
                (searchCategoryName.ToLower())&&
                x.Brand.BrandName.ToLower().Contains(searchBrandName.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(searchCategoryName))
            {
                products = products.Where(x => x.Category.CategoryName.ToLower().Contains
                (searchCategoryName.ToLower())).ToList();
            }
            else if(!String.IsNullOrEmpty(searchBrandName))
            {
                products = products.Where(x => x.Brand.BrandName.ToLower().Contains(searchBrandName.ToLower())).ToList();
            }

            return products;
        }

        public bool RemoveBy(int productId)
        {
            var product = GetProductById(productId);
            if (product == default(Product))
            {
                return false;
            }

            _context.Remove(product);
            return _context.SaveChanges() != 0;
        }

        public bool Update(int productId, string name, int brandId, int categoryId, string picture, int quantity, decimal price, decimal discount)
        {
           var product = GetProductById(productId);
            if (product == default(Product))
            {
                return false;
            }
            product.ProductName = name;

            // product.BrandId == brandId;
            // product.CategoryId == categoryId;

            product.Brand = _context.Brands.Find(brandId);
            product.Category = _context.Categories.Find(categoryId);

            product.Picture= picture;
            product.Quantity = quantity;
            product.Price = price;
            product.Discount = discount;
            _context.Update(product);
            return _context.SaveChanges() != 0;
        }
    }
}
