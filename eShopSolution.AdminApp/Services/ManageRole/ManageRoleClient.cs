using eShopSolution.ViewModels.System.Roles;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
    public class ManageRoleClient : IManageRoleClient
    {
        private readonly IHttpClientFactory _httClientFactory;
        public ManageRoleClient(IHttpClientFactory httpClientFactory)
        {
            _httClientFactory = httpClientFactory;
        }

        public async Task<ResultApiAddRole> AddRole(string token, AddRoleViewModel request)
        {
            var json = JsonConvert.SerializeObject(request);// convert object to json
            // new request
            var client = _httClientFactory.CreateClient();
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            client.BaseAddress = new Uri("http://apieshop.somee.com");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await client.PostAsync("/api/ManageRole/AddRole", httpContent);
            var data = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResultApiAddRole>(data);
            return result;
        }

        public async Task<ResultApiAssignRoleForUser> AssignRoleForUser(string token, List<PermissionViewModel> viewModel)
        {
            var client = _httClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(viewModel);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            client.BaseAddress = new Uri("http://apieshop.somee.com");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var id = viewModel[0].userId;
            var resultDelete = await this.DeleteRoleForUser(token, id);
            if(resultDelete.statusCode == 200)
            {
                var response = await client.PostAsync("/api/ManageRole/AssignRoleToUser", httpContent);
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ResultApiAssignRoleForUser>(data);
                return result;
            }
            else
            {
                var result = new ResultApiAssignRoleForUser()
                {
                    statusCode = 401,
                    message = "Delete roles faild",
                    data = false
                };
                return result;
            }
        }

        public async Task<ResultApiDeleteRoleForUser> DeleteRoleForUser(string token, Guid Id)
        {
          
            var client = _httClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://apieshop.somee.com");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await  client.DeleteAsync("/api/ManageRole/DeleteRoleForUserById?Id=" + Id);
            var data = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResultApiDeleteRoleForUser>(data);
            return result;
        }

        public async Task<ResultApiGetRoleAll> GetAllRole(string token)
        {

            var client = _httClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://apieshop.somee.com");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await client.GetAsync("/api/ManageRole/GetAllRole");
            var data = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<ResultApiGetRoleAll>(data);
            return roles;
        }

        public async Task<ResultApiGettRoleById> GetRoleByUserId(string token, Guid Id)
        {
            var client = _httClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://apieshop.somee.com");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await client.GetAsync("/api/ManageRole/GetRoleByUserId?Id=" + Id);
            var data = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<ResultApiGettRoleById>(data);
            return roles;
        }
    }

    public class ResultApiGetRoleAll
    {
        public int statusCode { set; get; }
        public string message { set; get; }
        public List<RoleViewModel> data { set; get; }
    }

    public class ResultApiAddRole
    {
        public int statusCode { set; get; }
        public string message { set; get; }
        public bool data { set; get; }
    }

    public class ResultApiAssignRoleForUser
    {
        public int statusCode { set; get; }
        public string message { set; get; }
        public bool data { set; get; }
    }

    public class ResultApiGettRoleById
    {
        public int statusCode { set; get; }
        public string message { set; get; }
        public  List<GetPermissionViewModel> data { set; get; }
    }

    public class ResultApiDeleteRoleForUser
    {
        public int statusCode { set; get; }
        public string message { set; get; }
        public bool data { set; get; }
    }



}
