using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Services;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SalesWebMVCContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SalesWebMVCContext")
    ?? throw new InvalidOperationException("Connection string 'SalesWebMVCContext' not found.")));
builder.Services.AddScoped<DepartmentServices>();
builder.Services.AddScoped<SellerServices>();
builder.Services.AddScoped<SalesRecordServices>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    SeedingService seed = new SeedingService(app.Services.CreateScope().ServiceProvider.GetRequiredService<SalesWebMVCContext>());
    seed.Seed();
}
app.UseRequestLocalization(
    new RequestLocalizationOptions()
    {
        DefaultRequestCulture = new RequestCulture(new CultureInfo("en-US")),
        SupportedCultures = new List<CultureInfo> { new CultureInfo("en-US") },
        SupportedUICultures = new List<CultureInfo> { new CultureInfo("en-US") },
    });
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
