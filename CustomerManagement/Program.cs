using CustomerManagement.Business;
using CustomerManagement.Infra.AutoMapper;
using CustomerManagement.Infra.Core.Middlewares;
using CustomerManagement.Repository;
using CustomerManagement.Repository.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


// Remove 'Server' header - Security recommends
builder.WebHost.UseKestrel(options => options.AddServerHeader = false);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region AutoMapper

// AutoMapper settings
var mappingProfileAssembly = Assembly.GetAssembly(typeof(MappingProfiles));
var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

if (mappingProfileAssembly != null)
{
    assemblies.Add(mappingProfileAssembly!);
}

builder.Services.AddAutoMapper(assemblies);

#endregion AutoMapper

#region Database Connection

builder.Services.AddDbContext<CustomerManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CustomerManagement")));

#endregion Database Connection

#region DI

#region Business

builder.Services.AddScoped<ICustomerBusiness, CustomerBusiness>();

#endregion Business

#region Repository

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

#endregion Repository

#endregion DI

var app = builder.Build();

#region Database Migrations

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CustomerManagementContext>();

    dbContext.Database.Migrate();
}

#endregion Database Migration

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<AppExceptionMiddleware>();

app.MapControllers();

app.Run();
