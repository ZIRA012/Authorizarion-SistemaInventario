using BlazorApp1.Components;
//using Syncfusion.Blazor;
using Infrastructure.SI.DependencyInjection;
using Application.SI.DependencyInjection;
using BlazorApp1.Components.Layout.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
//Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense
// ("Ngo9BigBOggjHTQxAR8/V1NNaF5cXmBCe0x0Q3xbf1x1ZFRHallZTnZeUiweQnxTdEBjXX1YcXdQRmJYWUZxWElfag==");


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructureService(builder.Configuration)
    .AddApplicationService();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthStateProvider>();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
// Para laauthorizacion por paginas
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOrManager", policy =>
    {
        policy.RequireAssertion(context =>
            context.User.HasClaim(ClaimTypes.Role, "Admin") ||
            context.User.HasClaim(ClaimTypes.Role, "Manager"));
    });
});

//builder.Services.AddSyncfusionBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapSignOutEnPoint(); 
app.Run();
