using SproomInbox.WebApp.Server.Services;

var builder = WebApplication.CreateBuilder(args);
string val =builder.Configuration.GetConnectionString("SproomDocumentsApiV1.0");
// Add services to the container.
builder.Services.AddHttpClient<IDocumentsFromApiService, DocumentsFromApiService>(client =>
                                                                    client.BaseAddress = 
                                                                    new Uri(builder.Configuration.GetConnectionString("SproomDocumentsApiV1.0")));
builder.Services.AddHttpClient<IUsersFromApiService, UsersFromApiService>(client =>
                                                                    client.BaseAddress =
                                                                    new Uri(builder.Configuration.GetConnectionString("SproomDocumentsApiV1.0")));
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
