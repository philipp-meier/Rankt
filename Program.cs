using Carter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rankt.Extensions;
using Rankt.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.WebHost.UseKestrel(option => option.AddServerHeader = false);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// TODO: Clean up and move to infrastructure/web/...
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddCarter();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

// Handled by the reverse proxy
app.UseHttpsRedirection();

// Other security headers are appended by the reverse proxy (see OWASP recommendations).
app.UseHsts();

app.UseForwardedHeaders();

app.UseStaticFiles();

app.MapUserEndpoints();
app.MapCarter();

app.MapFallbackToFile("index.html");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();

    // Can be changed with environment variables (see docker-compose.yml)
    var adminUsername = builder.Configuration["AppSetup:AdminUsername"] ?? "admin";
    var adminPassword = builder.Configuration["AppSetup:AdminPassword"] ?? "temp";

    var adminUser = dbContext.Users.First(x => x.Id == dbContext.AdminUserId);
    adminUser.UserName = adminUsername;

    var pwHasher = new PasswordHasher<IdentityUser>();
    adminUser.PasswordHash = pwHasher.HashPassword(adminUser, adminPassword);

    dbContext.Update(adminUser);
    dbContext.SaveChanges();
}

app.Run();
