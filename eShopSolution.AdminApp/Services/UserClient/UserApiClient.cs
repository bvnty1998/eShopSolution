using eShopSolution.ViewModels.System.Users;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Http;
using eShopSolution.ViewModels.Common;

namespace eShopSolution.AdminApp.Services
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httClientFactory;
        public UserApiClient(IHttpClientFactory httpClientFactory)
        {
            _httClientFactory = httpClientFactory;
        }
        public async Task<string> Authenticate(LoginRequestViewModel viewModel)
        {

            var json = JsonConvert.SerializeObject(viewModel);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://apieshop.somee.com");
            var response = await client.PostAsync("/api/User/authenticate", httpContent);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var token = await response.Content.ReadAsStringAsync();
                return token;
            }
            else
            {
                var status = response.StatusCode;
                return status.ToString();
            }

        }

        public async Task<PageResult<UserViewModel>> UserPagingRequest(UserPagingRequest request, string token)
        {
            var json = JsonConvert.SerializeObject(request); // convert from object to json 
            // new request
            var httpcontent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://apieshop.somee.com");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await client.PostAsync("/api/User/GetUserPaging", httpcontent);
            var data = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<ResultApiGetUserPaging>(data);
            return users.data;


        }

        public async Task<UserViewModel> CreateUser(RegisterRequestViewModel viewModel, string token)
        {
            var json = JsonConvert.SerializeObject(viewModel);// convert object to json
            // create request
            var client = _httClientFactory.CreateClient();
            var httpcontent = new StringContent(json, Encoding.UTF8, "application/json");
            client.BaseAddress = new Uri("http://apieshop.somee.com");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await client.PostAsync("/api/User/register", httpcontent);
            var data = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<ResultApiCreateUser>(data);
            return user.data;
        }

        public async Task<int> DeleteUserById(Guid id, string token)
        {

            var client = _httClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://apieshop.somee.com");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await client.DeleteAsync("/api/User/Delete?id=" + id);
            var data = await response.Content.ReadAsStringAsync();

            if (data != null)
            {
                return Int32.Parse(data);
            }
            else
            {
                return 0;
            }

        }

        public async Task<UserViewModel> GetUserById(Guid Id, string token)
        {
            var client = _httClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://apieshop.somee.com");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await client.GetAsync("/api/User/GetUserById?Id=" + Id);
            var data = await response.Content.ReadAsStringAsync();

            if (data != null)
            {
                var user = JsonConvert.DeserializeObject<ResultApiGetUserById>(data);
                return user.data;
            }
            else
            {
                var user = new UserViewModel();
                return user;
            }
        }

        public async Task<UserViewModel> UpdateUser(UpdateUserViewModel viewModel, string token)
        {
            var json = JsonConvert.SerializeObject(viewModel);// convert object to json
            // create request
            var client = _httClientFactory.CreateClient();
            var httpcontent = new StringContent(json, Encoding.UTF8, "application/json");
            client.BaseAddress = new Uri("http://apieshop.somee.com");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await client.PostAsync("/api/User/UpdateUser", httpcontent);
            var data = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<ResultApiUser>(data);
            return user.data;
        }

        public async Task<ResultApiGetAllUser> GetAllUser(string token)
        {
            var client = _httClientFactory.CreateClient();
            client.BaseAddress=new Uri("http://apieshop.somee.com");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await client.GetAsync("/api/User/GetAllUser");
            var data = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<ResultApiGetAllUser>(data);
            return users;
        }
    }

}
public class ResultApiGetUserPaging
{
    public int statusCode { set; get; }
    public PageResult<UserViewModel> data { set; get; }
}

public class ResultApiCreateUser
{
    public int statusCode { set; get; }
    public UserViewModel data { set; get; }
}

public class ResultApiUser
{
    public int statusCode { set; get; }
    public UserViewModel data { set; get; }
}

public class ResultApiGetUserById
{
    public int statusCode { set; get; }
    public UserViewModel data { set; get; }
}

public class ResultApiGetAllUser
{
    public int statusCode { set; get; }
    public string message { set; get; }
    public List<UserViewModel>data{set;get;}
}

