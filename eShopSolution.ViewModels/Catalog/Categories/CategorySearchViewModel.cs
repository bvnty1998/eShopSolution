using eShopSolution.Data.Emun;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Categories
{
   public class CategorySearchViewModel
    {
        public int Id { set; get; }
        public int SortOrder { set; get; }
        public bool IsShowHome { set; get; }
        public int? PrarentId { set; get; }
        public Status Status { set; get; }
        public string Name { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }
        public string LanguageId { set; get; }
        public string SeoAlias { set; get; }
    }
}
