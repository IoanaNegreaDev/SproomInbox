using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SproomInbox.API.Domain;
using SproomInbox.API.Domain.Repositories;
using SproomInbox.API.Domain.Services;
using SproomInbox.API.Utils.DtoMapper;
using SproomInbox.API.Utils.ErrorHandling;
using SproomInbox.API.Utils.Paging;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SproomDocumentsDbContext>(options =>
                       options.UseSqlServer(builder.Configuration.GetConnectionString("SproomDocumentsDbConnection")));
builder.Services.AddTransient<DataSeeder>();


builder.Services.AddMvc(setupAction =>
{
    setupAction.ReturnHttpNotAcceptable = true;
 });

var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new ModelDtoMapper()));
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IDocumentStateRepository, DocumentStateRepository>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();  
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IPaginationUriBuilder, PaginationUriBuilder>();

builder.Services.AddControllers()
                .AddNewtonsoftJson()
                .AddFluentValidation(options =>
                {
                    options.ImplicitlyValidateChildProperties = true;
                    options.ImplicitlyValidateRootCollectionElements = true;
                    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Sproom Documents API",
        Description = "Visma Interview Assignment",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
        {
            Email = "ioana.ursu@gmail.com",
            Name = "Ioana Negrea",
        },
    });

    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
    options.IncludeXmlComments(xmlCommentsFullPath);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{   
    app.UseSwagger();
    app.UseSwaggerUI(setupAction =>
    {
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });
    }); 
    SeedData(app);
}

app.UseAuthorization();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseStaticFiles();
app.MapControllers();

app.Run();

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    if (scopedFactory == null)
        return;
   
    using (var scope = scopedFactory.CreateScope())
    {
        try { 
            var service = scope.ServiceProvider.GetService<DataSeeder>();
            if (service == null)
                return;
       
            service.Seed();
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
}