using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace WPADotNetCore.ApiConsoleApp
{
    public class HttpClientExample
    {
        private readonly HttpClient _httpClient;
        private readonly string _postEndpoint = "https://jsonplaceholder.typicode.com/posts";

        public HttpClientExample()
        {
            _httpClient = new HttpClient();
        }

        public async Task Read()
        {
            var response = await _httpClient.GetAsync(_postEndpoint);
            if (response.IsSuccessStatusCode)
            {
                string jsonStr = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonStr);
            }
        }

        public async Task Edit(int id)
        {
            var response = await _httpClient.GetAsync($"{_postEndpoint}/{id}");
            if (response.IsSuccessStatusCode)
            {
                string jsonStr = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonStr);
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine("No Record Found");
            }
        }

        public async Task Create(int userId,string title,string body)
        {
            PostModel model = new PostModel()
            {
                userId = userId,
                title = title,
                body = body
            };
            var jsonRequest=JsonConvert.SerializeObject(model);
            var content=new StringContent(jsonRequest, Encoding.UTF8, Application.Json);
            var response = await _httpClient.PostAsync(_postEndpoint, content);
            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully Created");
            }
        }

        public async Task Update(int Id,int userId,string title,string body)
        {
            PostModel model = new PostModel()
            {
                id = Id,
                title = title,
                body = body,
                userId = userId
            };
            var jsonRequest=JsonConvert.SerializeObject(model);
            var content=new StringContent(jsonRequest,Encoding.UTF8, Application.Json);
            var response = await _httpClient.PutAsync($"{_postEndpoint}/{Id}", content);
            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully Upated");
            }
        }

        public async Task PatchUpdate(int Id, int userId, string title, string body)
        {
            PostModel model = new PostModel()
            {
                id = Id,
                title = title,
                body = body,
                userId = userId
            };
            var jsonRequest = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonRequest, Encoding.UTF8, Application.Json);
            var response = await _httpClient.PatchAsync($"{_postEndpoint}/{Id}", content);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully Upated");
            }
        }

        public async Task Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_postEndpoint}/{id}");
            if (response.IsSuccessStatusCode)
            {
                string jsonStr = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonStr);
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine("No Record Found");
            }
        }
    }

    public class PostModel
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
    }

}
