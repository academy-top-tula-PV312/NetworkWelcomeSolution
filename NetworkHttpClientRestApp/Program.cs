using NetworkHttpClientRestApp;
using System.Net;
using System.Net.Http.Json;

string server = "https://localhost:7223/api/employees";
HttpClient client = new HttpClient();

while(true)
{
    Console.Clear();

    Console.WriteLine("1 - View all employees");
    Console.WriteLine("2 - View employee");
    Console.WriteLine("3 - Add employee");
    Console.WriteLine("4 - Edit employee");
    Console.WriteLine("5 - Delete employee");
    Console.WriteLine("0 - Exit\n");
    
    Console.Write("Your choise: ");
    var choise = Console.ReadLine();

    if (choise == "0") break;

    switch (choise)
    {
        case "1": await EmployeesView(); break;
        case "2": await EmployeeView(); break;
        case "3": await EmployeeAdd(); break;
        case "4": await EmployeeEdit(); break;
        case "5": await EmployeeDelete(); break;
    }

    Console.WriteLine("Press any key...");
    Console.ReadKey();
}

async Task EmployeesView()
{
    List<Employee>? employees = await client.GetFromJsonAsync<List<Employee>>(server);
    if(employees is not null)
    {
        Console.WriteLine("List employees:");
        foreach (var employee in employees)
            Console.WriteLine($"\t{employee}");
        Console.WriteLine();
    }
}

async Task EmployeeView()
{
    Console.Write("Input id of employee: ");
    string? id = Console.ReadLine();

    using var response = await client.GetAsync(server + "/" + id);
    if(response.StatusCode == HttpStatusCode.NotFound)
    {
        ErrorMessage? error = await response.Content.ReadFromJsonAsync<ErrorMessage>();
        Console.WriteLine(error?.Message);
    }
    else if(response.StatusCode == HttpStatusCode.OK)
    {
        Employee? employee = await response.Content.ReadFromJsonAsync<Employee>();
        Console.WriteLine(employee);
    }
    Console.WriteLine();
}

async Task EmployeeAdd()
{
    Console.WriteLine("Add new employee");
    Employee? employee = new Employee();

    Console.Write("Input name: ");
    employee.Name = Console.ReadLine()!;
    Console.Write("Input age: ");
    employee.Age = Int32.Parse(Console.ReadLine()!);

    using var response = await client.PostAsJsonAsync<Employee>(server, employee);
    employee = await response.Content.ReadFromJsonAsync<Employee>();

    Console.WriteLine($"Employee {employee} add to server BD");
}

async Task EmployeeEdit()
{
    Console.Write("Input id of employee: ");
    int id = Int32.Parse(Console.ReadLine()!);

    List<Employee>? employees = await client.GetFromJsonAsync<List<Employee>>(server);
    if (employees is not null)
    {
        Employee? employeeEdit = employees.FirstOrDefault(e => e.Id == id);
        Console.WriteLine("Edit employee:");
        
        Console.Write($"Edit name ({employeeEdit!.Name}): ");
        employeeEdit.Name = Console.ReadLine()!;
        Console.Write($"Edit age ({employeeEdit!.Age}): ");
        employeeEdit.Age = Int32.Parse(Console.ReadLine()!);

        using var response = await client.PutAsJsonAsync<Employee>(server, employeeEdit);

        if(response.StatusCode == HttpStatusCode.NotFound)
        {
            ErrorMessage? error = await response.Content.ReadFromJsonAsync<ErrorMessage>();
            Console.WriteLine(error?.Message);
        }
        else if(response.StatusCode == HttpStatusCode.OK)
        {
            employeeEdit = await response.Content.ReadFromJsonAsync<Employee>();
            Console.WriteLine($"Employee {employeeEdit} edited at server BD");
        }
    }

}

async Task EmployeeDelete()
{
    Console.Write("Input id of employee: ");
    int id = Int32.Parse(Console.ReadLine()!);

    using var response = await client.DeleteAsync($"{server}/{id}");
    if(response.StatusCode == HttpStatusCode.NotFound)
    {
        ErrorMessage? error = await response.Content.ReadFromJsonAsync<ErrorMessage>();
        Console.WriteLine(error?.Message);
    }
    else if(response.StatusCode == HttpStatusCode.OK)
    {
        Employee? employee = await response.Content.ReadFromJsonAsync<Employee>();
        Console.WriteLine($"Employee {employee} deleted from server DB");
    }
}

class ErrorMessage
{
    public string? Message { get; set; }
}