using Repository.Data;
using WebShop.Notifications;
using WebShop.UnitOfWork;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Registrera Unit of Work i DI-container
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<INotificationObserver, EmailNotification>();
builder.Services.AddSingleton<ProductSubject>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext with SQL Server or any other provider
builder.Services.AddDbContext<MyDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    dbContext.Database.Migrate();
}

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
var productSubject = app.Services.GetRequiredService<ProductSubject>();
var emailNotification = app.Services.GetRequiredService<INotificationObserver>();

// Attach observers during startup
lifetime.ApplicationStarted.Register(() =>
{
    productSubject.Attach(emailNotification);
    Console.WriteLine("Observer attached at startup.");
});

// Detach observers during shutdown
lifetime.ApplicationStopping.Register(() =>
{
    productSubject.Detach(emailNotification);
    Console.WriteLine("Observer detached at shutdown.");
});

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

