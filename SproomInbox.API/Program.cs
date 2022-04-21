using AutoMapper;
using FluentValidation.AspNetCore;
using Marvin.Cache.Headers;
using Microsoft.EntityFrameworkCore;
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

//builder.Services.AddResponseCaching();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{   
    app.UseSwagger();
    app.UseSwaggerUI();
    SeedData(app);
}

app.UseAuthorization();

//app.UseResponseCaching();

app.UseRouting();

//app.UseHttpCacheHeaders();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.Run();

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<DataSeeder>();
        service.Seed();
    }
}