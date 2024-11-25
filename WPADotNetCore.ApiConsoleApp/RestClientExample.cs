using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace WPADotNetCore.ApiConsoleApp
{
    public class RestClientExample
    {
        private readonly RestClient _httpClient;
        private readonly string _postEndpoint = "https://jsonplaceholder.typicode.com/posts";

        public RestClientExample()
        {
            _httpClient = new RestClient();
        }

        public async Task Read()
        {
            RestRequest request = new RestRequest(_postEndpoint, Method.Get);
            var response = await _httpClient.GetAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string jsonStr = response.Content!;
                Console.WriteLine(jsonStr);
            }
        }

        public async Task Edit(int id)
        {
            RestRequest request = new RestRequest($"{_postEndpoint}/{id}", Method.Get);
            var response = await _httpClient.GetAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string jsonStr = response.Content!;
                Console.WriteLine(jsonStr);
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine("No Record Found");
            }
        }

        public async Task Create(int userId, string title, string body)
        {
            PostModel model = new PostModel()
            {
                userId = userId,
                title = title,
                body = body
            };
            RestRequest request = new RestRequest(_postEndpoint, Method.Post);
            request.AddJsonBody(model);
            var response = await _httpClient.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully Created");
            }
        }

        public async Task Update(int Id, int userId, string title, string body)
        {
            PostModel model = new PostModel()
            {
                id = Id,
                title = title,
                body = body,
                userId = userId
            };
            RestRequest request = new RestRequest(_postEndpoint, Method.Put);
            request.AddJsonBody(model);
            var response = await _httpClient.PutAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully Updated");
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
            RestRequest request = new RestRequest(_postEndpoint, Method.Patch);
            request.AddJsonBody(model);
            var response = await _httpClient.PatchAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully Updated");
            }
        }

        public async Task Delete(int id)
        {
            RestRequest request=new RestRequest($"{_postEndpoint}/{id}",Method.Delete);
            var response = await _httpClient.DeleteAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string jsonStr = response.Content!;
                Console.WriteLine(jsonStr);
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine("No Record Found");
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
}
