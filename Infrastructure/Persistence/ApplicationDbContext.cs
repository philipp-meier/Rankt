using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rankt.Entities;
using Rankt.Shared.Audit;

namespace Rankt.Infrastructure.Persistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
    : IdentityDbContext<IdentityUser>(options)
{
    internal const string AdminUserId = "118d6207-3d51-4ad0-b059-ffab450e4458";

    public DbSet<Question> Questions => Set<Question>();
    public DbSet<QuestionOption> QuestionOptions => Set<QuestionOption>();
    public DbSet<QuestionResponse> QuestionResponses => Set<QuestionResponse>();
    public DbSet<QuestionResponseItem> QuestionResponseItems => Set<QuestionResponseItem>();
    public DbSet<QuestionStatus> QuestionStatus => Set<QuestionStatus>();
    public DbSet<QuestionType> QuestionType => Set<QuestionType>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.AddInterceptors(auditableEntitySaveChangesInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<QuestionStatus>().HasData(
            Entities.QuestionStatus.New,
            Entities.QuestionStatus.Published,
            Entities.QuestionStatus.InProgress,
            Entities.QuestionStatus.Completed,
            Entities.QuestionStatus.Archived
        );

        builder.Entity<QuestionType>().HasData(
            Entities.QuestionType.RankingQuestion,
            Entities.QuestionType.Voting
        );

        builder.Entity<Question>()
            .HasIndex(x => x.ExternalIdentifier)
            .IsUnique();

        builder.Entity<QuestionOption>()
            .HasIndex(x => x.ExternalIdentifier)
            .IsUnique();

        const string AdminRoleId = "cb8e00f1-3be0-4d4b-816f-163e8040628a";
        builder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Name = "Admin", NormalizedName = "ADMIN", Id = AdminRoleId, ConcurrencyStamp = AdminRoleId
        });

        var adminUser = new IdentityUser
        {
            Id = AdminUserId, UserName = "admin", NormalizedUserName = "ADMIN", LockoutEnabled = true
        };

        var pwHasher = new PasswordHasher<IdentityUser>();
        adminUser.PasswordHash = pwHasher.HashPassword(adminUser, "admin");

        builder.Entity<IdentityUser>().HasData(adminUser);

        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = AdminRoleId, UserId = AdminUserId
        });
    }
}
