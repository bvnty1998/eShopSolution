using eShopSolution.Data.Emun;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Category
    {
        public int  Id { set; get; }

        public int SortOrder { set; get; }
        public bool IsShowHome { set; get; }
        public int? PrarentId { set; get; }
        public Status Status { set; get; }
       
        public List<Product> Products { set; get; }
        public List<CategoryTranslation> CategoryTranslations { get; set; }
    }
}
