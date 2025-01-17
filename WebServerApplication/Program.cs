using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

int globalId = 1;

List<Employee> employees = new List<Employee>()
{
    new(){ Id = globalId++, Name = "Jimmy", Age = 32},
    new(){ Id = globalId++, Name = "Bobby", Age = 28},
    new(){ Id = globalId++, Name = "Sammy", Age = 19},
};




app.MapGet("/{id:int?}", (HttpContext context, int? id) =>
{
    foreach(var h in context.Request.Headers)
    {
        Console.Write($"{h.Key}: ");
        foreach(var v in h.Value)
            Console.WriteLine(v);
    }

    if (id is null)
        return Results.BadRequest(new { Message = "Incorrect data request" }); // 400
    else if (id != 123)
        return Results.NotFound(new { Message = "Employee not found" }); // 404
    else
        return Results.Json(new Employee() { Id = 123, Name = "Bobby", Age = 32 }); // 200
});

app.MapPost("/text", async (HttpContext context) =>
{
    using StreamReader reader = new StreamReader(context.Request.Body);
    string requestContent = await reader.ReadToEndAsync();
    return $"Client send to server: {requestContent}";
});

app.MapPost("/json", (Employee employee) =>
{
    employee.Id = 200;
    return employee;
});

// GET api/employees - view
app.MapGet("/api/employees", () => employees);

// GET api/employees/id - view
app.MapGet("/api/employees/{id:int}", (int id) =>
{
    Employee? employee = employees.FirstOrDefault(x => x.Id == id);
    if (employee is null)
        return Results.NotFound(new { Message = "Employee not found" });

    return Results.Json(employee);
});

// POST api/employees Employee - insert
app.MapPost("/api/employees", (Employee employee) =>
{
    employee.Id = globalId++;
    employees.Add(employee);
    return Results.Json(employee);
});

// PUT api/employees Employee - update
app.MapPut("/api/employees", (Employee data) =>
{
    Employee? employee = employees.FirstOrDefault(e => e.Id == data.Id);
    if (employee is null)
        return Results.NotFound(new { Message = "Employee not found" });

    employee.Name = data.Name;
    employee.Age = data.Age;

    return Results.Json(employee);
});

// DELETE api/employees/id - delete
app.MapDelete("/api/employees/{id:int}", (int id) =>
{
    Employee? employee = employees.FirstOrDefault(e => e.Id == id);
    if (employee is null)
        return Results.NotFound(new { Message = "Employee not found" });

    employees.Remove(employee);
    return Results.Json(employee);
});


app.MapPost("/form", async (HttpContext context) =>
{
    var form = context.Request.Form;
    string? name = form["name"];
    string? age = form["age"];

    await context.Response.WriteAsync($"Form data: Name: {name}, Age: {age}");
});

app.MapPost("/image", async (HttpContext context) =>
{
    var path = Directory.GetCurrentDirectory() + "/images";
    Directory.CreateDirectory(path);

    string fileName = Guid.NewGuid().ToString();

    using(var stream = new FileStream($"{path}/{fileName}.jpg", FileMode.Create))
    {
        await context.Request.Body.CopyToAsync(stream);
    }

    await context.Response.WriteAsync($"file {fileName} save to server");
});

app.MapPost("/bytes", async (HttpContext context) =>
{
    using StreamReader reader = new StreamReader(context.Request.Body);
    string text = await reader.ReadToEndAsync();
    await context.Response.WriteAsync($"Client send text: {text}");
});

app.MapPost("/files", async (HttpContext context) =>
{
    var files = context.Request.Form.Files;

    var path = Directory.GetCurrentDirectory() + "/files";
    Directory.CreateDirectory(path);

    foreach(var file in files)
    {
        string filePath = $"{path}/{file.FileName}";
        using(var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
    }

    await context.Response.WriteAsync("All files saved to server");
});


app.Run();


class Employee
{
    public Employee() { }
    //public Employee(string name, int age)
    //{ 
    //    Name = name;
    //    Age = age;
    //}

    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Age { get; set; }
}


