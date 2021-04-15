using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
   public class Product
    {
        public int Id { set; get; }
        public decimal Price { set; get; }
        public decimal OriginalPrice { set; get; }
        public int Stock { set; get; }
        public int ViewCount { set; get; }
        public DateTime DateCreate { set; get; }
        public string SeoAlias { set; get; }
        public int CategoryId { set; get; }
        public List<ProductTranslation> ProductTranslations { get; set; }
        public Category Category { set; get; }
    }
}
