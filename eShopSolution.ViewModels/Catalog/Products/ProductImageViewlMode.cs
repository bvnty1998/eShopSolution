using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Products
{
    public class ProductImageViewlMode
    {
        public int Id{set;get;}
        public string FilePath { set; get; }
        public string Caption { set; get; }
        public bool IsDefault { set; get; }
        public long FileSize { set; get; }
    }
}
