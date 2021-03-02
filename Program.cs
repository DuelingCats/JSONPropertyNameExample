using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;

namespace JSONPropertyNameExample
{
    class Program
    {
        public static IWebHost Host { get; set; }
        static void Main(string[] args)
        {
            Host = WebHost.CreateDefaultBuilder()
                .ConfigureServices(services => services.AddMvc())
                .Configure(app => app.UseMvc())
                .Build();
            Host.RunAsync();

            SendSampleWithJSONPropertyName();
            SendSampleAsClassPropertyName();

            Console.ReadLine();
            Host.StopAsync().Wait();
        }

        public static void RunService()
        {
            Host = WebHost.CreateDefaultBuilder()
                .ConfigureServices(services => services.AddMvc())
                .Configure(app => app.UseMvc())
                .Build();
            Host.Run();
        }

        public static void SendSampleWithJSONPropertyName()
        {
            var client = new RestClient("http://localhost:5000/TestRoute");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\r\n    \"id\" : 12,\r\n    \"message_contents\" : \"the contents of stuff\"\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }

        public static void SendSampleAsClassPropertyName()
        {
            var client = new RestClient("http://localhost:5000/TestRoute");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\r\n    \"NodeId\" : 12,\r\n    \"Contents\" : \"the contents of stuff\"\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }
    }


    [Route("TestRoute")]
    public class NodeRedController
    {

        [HttpPost]
        public void Post([FromBody] BasicMessage payload)
        {

            if (payload.NodeId == 0 || String.IsNullOrEmpty(payload.Contents))
            {
                Console.WriteLine("JSON was NOT deserialized correctly.");
            }
            else if (payload.NodeId == 12 && payload.Contents == "the contents of stuff")
            {
                Console.WriteLine("JSON was deserialized correctly.");
            }

        }
    }

    public class BasicMessage
    {
        [JsonPropertyName("id")]
        public int NodeId { get; set; }

        [JsonPropertyName("message_contents")]
        public string Contents { get; set; }
    }
}
