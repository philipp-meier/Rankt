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

    public DbSet<RankingQuestion> RankingQuestions => Set<RankingQuestion>();
    public DbSet<RankingQuestionOption> RankingQuestionOptions => Set<RankingQuestionOption>();
    public DbSet<RankingQuestionResponse> RankingQuestionResponses => Set<RankingQuestionResponse>();
    public DbSet<RankingQuestionResponseItem> RankingQuestionResponseItems => Set<RankingQuestionResponseItem>();
    public DbSet<RankingQuestionStatus> RankingQuestionStatus => Set<RankingQuestionStatus>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.AddInterceptors(auditableEntitySaveChangesInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<RankingQuestionStatus>().HasData(
            Entities.RankingQuestionStatus.New,
            Entities.RankingQuestionStatus.Published,
            Entities.RankingQuestionStatus.InProgress,
            Entities.RankingQuestionStatus.Completed,
            Entities.RankingQuestionStatus.Archived
        );

        builder.Entity<RankingQuestion>()
            .HasIndex(x => x.ExternalIdentifier)
            .IsUnique();

        builder.Entity<RankingQuestionOption>()
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
