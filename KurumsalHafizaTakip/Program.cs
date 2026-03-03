using Microsoft.EntityFrameworkCore;
using KurumsalHafizaTakip.VeriTabaniBaglanti;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Klasörlerin 'Gorunumlur' olduðunu sisteme öðretiyoruz
builder.Services.AddControllersWithViews().AddRazorOptions(options => {
    options.ViewLocationFormats.Clear();
    options.ViewLocationFormats.Add("/Gorunumlur/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Gorunumlur/WebSayfaDosyalari/{0}.cshtml");
});

builder.Services.AddDbContext<UygulamaDbBaglanti>(opt =>
    opt.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=KHATS_V1;Trusted_Connection=True;"));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt => opt.LoginPath = "/Hesap/Giris");

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Hesap}/{action=Giris}/{id?}");
app.Run();