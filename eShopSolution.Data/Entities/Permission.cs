using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Permission
    {
        public int Id { set; get; }
        public  Guid RoleId { set; get; }
        public string RoleName { set; get; }
        public Guid UserId { set; get; }
        public int FunctionId { set; get; }
        public string FunctionName { set; get; }
    }
}
