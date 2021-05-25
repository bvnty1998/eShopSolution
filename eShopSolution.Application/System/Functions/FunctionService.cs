using eShopSolution.Data.EF;
using eShopSolution.ViewModels.System.Functions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.Application.System.Functions
{
    public class FunctionService : IFunctionService
    {
        private readonly EShopDbContext _context;
        public FunctionService(EShopDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddFunction(AddFunctionViewModel viewModel)
        {
            var function = new Function()
            {
                Name = viewModel.Name,
                Description = viewModel.Description
            };
            await _context.AddAsync(function);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<FunctionViewModel>> GetAllFunction()
        {
            var functions = await (from f in _context.Functions
                             select new FunctionViewModel()
                             {
                                 Id = f.Id,
                                 Name = f.Name,
                                 Description = f.Description

                             }).ToListAsync();
            return functions;
        }
    }
}
