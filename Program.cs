using System.Threading.RateLimiting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Rankt.Features.RankingQuestion;
using Rankt.Features.Account;
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

builder.Services.AddIdentityApiEndpoints<IdentityUser>(opt =>
    {
        opt.Lockout.AllowedForNewUsers = true;
        opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        opt.Lockout.MaxFailedAccessAttempts = 3;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Rate limiting for APIs that do not require authentication (e.g., "login")
builder.Services.AddRateLimiter(o =>
{
    o.AddFixedWindowLimiter("fixed", options =>
    {
        // A maximum of 4 requests per each 12-second window are allowed
        options.PermitLimit = 4;
        options.Window = TimeSpan.FromSeconds(12);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2;
    });
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

// Handled by the reverse proxy
app.UseHttpsRedirection();

// Other security headers are appended by the reverse proxy (see OWASP recommendations).
app.UseSecurityHeaders();
app.UseHsts();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseRateLimiter();
app.UseForwardedHeaders();

app.UseStaticFiles();

app.MapAccountEndpoints();
app.MapRankingQuestionEndpoints();

app.MapFallbackToFile("index.html");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();

    // Can be changed with environment variables (see docker-compose.yml)
    var adminUsername = builder.Configuration["AppSetup:AdminUsername"] ?? "admin";
    var adminPassword = builder.Configuration["AppSetup:AdminPassword"] ?? "temp";

    var adminUser = dbContext.Users.First(x => x.Id == ApplicationDbContext.AdminUserId);
    adminUser.UserName = adminUsername;

    var pwHasher = new PasswordHasher<IdentityUser>();
    adminUser.PasswordHash = pwHasher.HashPassword(adminUser, adminPassword);

    dbContext.Update(adminUser);
    dbContext.SaveChanges();
}

app.Run();
