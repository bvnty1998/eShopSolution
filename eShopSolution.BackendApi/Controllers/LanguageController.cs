using eShopSolution.Application.Catalog.Language;
using eShopSolution.ViewModels.Catalog.Language;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    public class LanguageController : Controller
    {
        private ILanguageService _language;
        public LanguageController(ILanguageService language)
        {
            _language = language;
        }

        [HttpPost("CreateLanguage")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromForm]LanguageCreateViewModel viewModel)
        {
            var reslut = await _language.CreateLanguage(viewModel);
            if (reslut == "")
            {
                return BadRequest();
            }
            else
            {
                return new OkObjectResult(await _language.GetLanguageById(reslut));
            }
        }

        [HttpPut("UpdateLanguage")]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromForm] LanguageUpdateViewModel viewModel)
        {
            var reslut = await _language.UpdateLanguage(viewModel);
            if (reslut == "")
            {
                return BadRequest();
            }
            else
            {
                return new OkObjectResult(_language.GetLanguageById(reslut));
            }
        }

        [HttpDelete("Delete/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(string Id)
        {
            var result =await _language.Delete(Id);
            if(result == false)
            {
                return BadRequest();
            }
            else
            {
                return new OkObjectResult(new
                {
                    status = "Success",
                    Message = "Delete Language Successfully !!"
                });
            }
        }

        [HttpGet("GetLanguageById")]
        [Authorize]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _language.GetLanguageById(id);
            return new OkObjectResult(result);
        }
    }
}
