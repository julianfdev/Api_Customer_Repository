using EFCoreInMemory.DatabaseContext;
using EFCoreInMemory.DataModel;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("MyDb"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetService<DataContext>();
SeedData(context);

app.Run();


static void SeedData(DataContext context)
{
    CustomerDataModel customer_1 = new CustomerDataModel()
    {
        Id = Guid.NewGuid(),
        Name = "John Smith",
        DNI = "123456",
        Phone = "1111",
        Mobile = "2222",
        Address = "Kennedy 123",
        City = "CityName",
        Email = "john@mail.com",
        State = "Texas"
    };
    CustomerDataModel customer_2 = new CustomerDataModel()
    {
        Id = new Guid("33ae1669-eb5d-4cc6-a960-70bf06ff00b9"),
        Name = "Jennifer Stone",
        DNI = "987654",
        Phone = "3333",
        Mobile = "4444",
        Address = "Bush 321",
        City = "CityName",
        Email = "jenni@mail.com",
        State = "Orlando"
    };
    context.Customer.Add(customer_1);
    context.Customer.Add(customer_2);
    context.SaveChanges();
}