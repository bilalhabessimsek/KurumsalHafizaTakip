using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using KurumsalHafizaTakip.VeriTabaniBaglanti;
using KurumsalHafizaTakip.Hubs;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// 1. ÖZEL AYAR DOSYASINI TANIMLAMA
// builder.Configuration üzerinden sistemin hangi JSON dosyasýný okuyacaðýný söylüyoruz.
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
builder.Configuration.AddJsonFile("UygulamaAyarlari.json", optional: false, reloadOnChange: true);

// 2. SERVÝS TANIMLAMALARI
builder.Services.AddControllersWithViews();

// JSON içindeki "VarsayilanBaglanti" anahtarýný okuyoruz
var connectionString = builder.Configuration.GetConnectionString("VarsayilanBaglanti");

builder.Services.AddDbContext<UygulamaDbBaglanti>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddSignalR();

// GÜVENLÝK VE ANLIK KOVMA SÝSTEMÝ
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Hesap/Giris";
        options.Cookie.Name = "KHATS_Oturum";

        options.Events = new CookieAuthenticationEvents
        {
            OnValidatePrincipal = async context =>
            {
                var db = context.HttpContext.RequestServices.GetRequiredService<UygulamaDbBaglanti>();
                var userIdClaim = context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    int userId = int.Parse(userIdClaim.Value);
                    var kullanici = await db.Kullanicilar.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);

                    if (kullanici == null || kullanici.Durum != "Aktif")
                    {
                        context.RejectPrincipal();
                        await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    }
                }
            }
        };
    });

var app = builder.Build();

// 3. ARA KATMANLAR (MIDDLEWARE)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Gorev}/{action=Dashboard}/{id?}");

app.MapHub<BildirimHub>("/bildirimHub");

app.Run();