using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OutfitPlaner_Applcation.Data;
using OutfitPlaner_Applcation.Models;

var builder = WebApplication.CreateBuilder(args);

// ������������ ���� ������
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "Data", "OutfitPlanner.db");
if (!Directory.Exists(Path.GetDirectoryName(dbPath)))
{
    Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
}

builder.Services.AddDbContext<WardrobeDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// ��������� �������������� ����� ����
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
            ? CookieSecurePolicy.None
            : CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

// ������ �������
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

var app = builder.Build();

// ���������� ��������
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<WardrobeDbContext>();
    db.Database.Migrate();
}

// ������������ middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // ������ ���� ����� UseAuthorization
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();