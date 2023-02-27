using Faces.WebMvc.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMassTransit();
builder.Services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(
    config =>
    {
        config.Host(new Uri("rabbitmq://localhost/"), h => 
        {
            h.Username("guest"); 
            h.Password("guest"); 
        });

        builder.Services.AddSingleton(provider => provider.GetRequiredService<IBusControl>());
        builder.Services.AddSingleton<IHostedService, BusService>();
    }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
