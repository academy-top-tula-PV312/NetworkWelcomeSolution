using NetworkHttpClientRestApp;
using System.Net.Http.Json;

string server = "https://localhost:7223/api/employees";
HttpClient client = new HttpClient();

while(true)
{
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

}

async Task EmployeeAdd()
{

}

async Task EmployeeEdit()
{

}

async Task EmployeeDelete()
{

}
