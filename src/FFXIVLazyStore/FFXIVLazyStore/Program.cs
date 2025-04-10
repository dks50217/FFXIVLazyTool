using FFXIVLazyStore.Components;
using Radzen;
using Core.Service;
using Core;
using Microsoft.EntityFrameworkCore;
using Core.Model;
using Service.Service;
using FFXIVLazyStore.Components.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using FFXIVLazyStore.Service;
using FFXIVLazyStore.Model;
using FFXIVLazyStore.Model.Setting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRadzenComponents();

builder.Services.AddHttpClient();

builder.Services.AddLocalization();

builder.Services.AddControllers();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<INetStoneService, NetStoneService>();

builder.Services.AddDbContext<HouseofsnowContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21))));

builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri("http://localhost")
    });

builder.Services.Configure<HomeSetting>(builder.Configuration.GetSection("Home"));

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IProtectLocalStorageService, ProtectLocalStorageService>();

builder.Logging.SetMinimumLevel(LogLevel.Warning);

var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Response.Headers.Add(
        "Content-Security-Policy",
        "frame-ancestors https://*.dks50217.com");
    await next();
});

var supportedCultures = new[] { "en-US", "ja-JP" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

app.Run();
