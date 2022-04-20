using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SproomInbox.WebApp.Client;
using SproomInbox.WebApp.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
Console.WriteLine(builder.HostEnvironment.BaseAddress);
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IDocumentsFromWebServerService, DocumentsFromWebServerService>();
builder.Services.AddScoped<IUsersFromWebServerService, UsersFromWebServerService>();


await builder.Build().RunAsync();
