HttpClient client = new();

List<string> files = new List<string>()
{
    @"D:\ada.jpg", @"D:\file01.txt", @"D:\E_tyudy.djvu"
};

using MultipartFormDataContent content = new MultipartFormDataContent();

foreach (string file in files)
{
    StreamContent stream = new(File.OpenRead(file));
    content.Add(stream, name: "file", fileName: file.Substring(file.LastIndexOf(@"\") + 1));
}

using var response = await client.PostAsync("https://localhost:7223/files", content);

string responseContent = await response.Content.ReadAsStringAsync();
Console.WriteLine(responseContent);
async Task FormSendExample()
{
    Dictionary<string, string> formData = new Dictionary<string, string>()
    {
        ["name"] = "Bobby",
        ["age"] = "29"
    };

    HttpContent content = new FormUrlEncodedContent(formData);

    using var response = await client.PostAsync("https://localhost:7223/form", content);

    string responseContent = await response.Content.ReadAsStringAsync();
    Console.WriteLine(responseContent);
}
async Task StreamSendExample()
{
    string filePath = @"D:\ada.jpg";
    using var stream = File.OpenRead(filePath);

    StreamContent content = new StreamContent(stream);

    using var response = await client.PostAsync("https://localhost:7223/image", content);

    string responseContent = await response.Content.ReadAsStringAsync();
    Console.WriteLine(responseContent);
}
async Task ByteArraySendExample()
{
    string text = "Hello world";
    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);

    ByteArrayContent content = new ByteArrayContent(bytes);

    using var response = await client.PostAsync("https://localhost:7223/bytes", content);

    string responseContent = await response.Content.ReadAsStringAsync();
    Console.WriteLine(responseContent);
}