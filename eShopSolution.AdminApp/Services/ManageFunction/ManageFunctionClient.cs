using eShopSolution.ViewModels.System.Functions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services.ManageFunction
{
    public class ManageFunctionClient : IManageFunctionClient
    {
        public readonly IHttpClientFactory _httpClientFactory;
        public ManageFunctionClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ResultApiAddFunction> AddFunction(string token, AddFunctionViewModel viewModel )
        {
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(viewModel);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            client.BaseAddress = new Uri("http://apieshop.somee.com");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await client.PostAsync("/api/Function/AddFunction", httpContent);
            var data = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResultApiAddFunction>(data);
            return result;
        }

        public async Task<ResultApiGetAllFunction> GetAllFunction(string token)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://apieshop.somee.com");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await client.GetAsync("/api/Function/GetAllFunction");
            var data = await response.Content.ReadAsStringAsync();
            var functions = JsonConvert.DeserializeObject<ResultApiGetAllFunction>(data);
            return functions;
        }
    }

    public class ResultApiGetAllFunction
    {
        public int statusCode { set; get; }
        public string message { set; get; }
        public List<FunctionViewModel> data { set; get; }
    }

    public class ResultApiAddFunction
    {
        public int statusCode { set; get; }
        public string message { set; get; }
        public bool data { set; get; }
    }
}
