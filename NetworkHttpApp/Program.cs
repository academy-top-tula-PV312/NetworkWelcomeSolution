using NetworkHttpApp;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

HttpClient client = new HttpClient();

// SEND STRING
//StringContent content = new("Clent send string text");

//using var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7223/text");
//request.Content = content;

//using var response = await client.SendAsync(request);

//string responseContent = await response.Content.ReadAsStringAsync();
//Console.WriteLine(responseContent);

// SEND JSON
Employee? employee = new Employee() { Name = "Tommy", Age = 28 };

//JsonContent content = JsonContent.Create(employee);
//using var response = await client.PostAsync("https://localhost:7223/json", content);
//employee = await response.Content.ReadFromJsonAsync<Employee>();

using var response = await client.PostAsJsonAsync("https://localhost:7223/json", employee);
employee = await response.Content.ReadFromJsonAsync<Employee>();

Console.WriteLine($"Id: {employee?.Id}, Name: {employee?.Name}, Age: {employee?.Age}");