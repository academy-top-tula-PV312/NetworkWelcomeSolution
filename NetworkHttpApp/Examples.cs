using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetworkHttpApp
{
    static public class Examples
    {
        public static async Task HttpClientWelcomeExample()
        {
            HttpClient httpClient = new HttpClient();

            //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://stomatolog-tula.ru/");
            //HttpResponseMessage response = await httpClient.SendAsync(request);

            HttpResponseMessage response = await httpClient.GetAsync("https://stomatolog-tula.ru/");

            Console.WriteLine($"Status code: {response.StatusCode}");
            Console.WriteLine("Headers:");

            foreach (var header in response.Headers)
            {
                Console.Write($"{header.Key}: ");
                foreach (var value in header.Value)
                    Console.WriteLine($"{value}");
            }

            Console.WriteLine("\nContent:");

            string content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
        }

        public static async Task HttpClentReadJsonExample()
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("MyHeader", "Wow value!");

            HttpRequestMessage request = new();
            request.Headers.Add("MyHeader", "Wow value!");

            //object? data = await client.GetFromJsonAsync("https://localhost:7223/", typeof(Employee));

            //if(data is Employee e)
            //{
            //    Console.WriteLine($"Id: {e.Id}, Name: {e.Name}, Age: {e.Age}");
            //}

            //Employee? employee = await client.GetFromJsonAsync<Employee>("https://localhost:7223/");
            //Console.WriteLine($"Id: {employee?.Id}, Name: {employee?.Name}, Age: {employee?.Age}");

            var response = await client.GetAsync("https://localhost:7223/123");

            if (response.StatusCode == HttpStatusCode.BadRequest ||
                response.StatusCode == HttpStatusCode.NotFound)
            {
                Error? error = await response.Content.ReadFromJsonAsync<Error>();
                Console.WriteLine($"Code: {response.StatusCode} {error?.Message}");
            }
            else
            {
                Employee? employee = await response.Content.ReadFromJsonAsync<Employee>();
                Console.WriteLine($"Id: {employee?.Id}, Name: {employee?.Name}, Age: {employee?.Age}");
            }
        }
    }

    class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Age { get; set; }
    }

    class Error
    {
        public string? Message { get; set; }
    }
}
