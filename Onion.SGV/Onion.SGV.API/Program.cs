using Onion.SGV.API.Data;
using Onion.SGV.API.Models;
using Onion.SGV.API.Services;
using Onion.SGV.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("_myAllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost/*", "https://localhost:44322", "http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyDbContext>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("_myAllowSpecificOrigins");

app.UseAuthorization();

app.MapControllers();

SeedProdutos(app);

app.Run();
void SeedProdutos(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<MyDbContext>();

        if (dbContext.Products.Any())
        {
            return; 
        }

        var products = new[]
        {
            new Product { Id = 1, Name = "Celular", Price = 1000.0 },
            new Product { Id = 2, Name = "Notebook", Price = 3000.0 },
            new Product { Id = 3, Name = "Televisão", Price = 5000.0 } 
        };

        foreach (var item in products)
        {
            dbContext.Products.Add(item);
        }

        dbContext.SaveChanges();
    }
}