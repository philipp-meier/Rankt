﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rankt.Infrastructure.Persistence;

#nullable disable

namespace Rankt.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240529095952_InitRankingQuestionStatus")]
    partial class InitRankingQuestionStatus
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.5");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "cb8e00f1-3be0-4d4b-816f-163e8040628a",
                            ConcurrencyStamp = "cb8e00f1-3be0-4d4b-816f-163e8040628a",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "118d6207-3d51-4ad0-b059-ffab450e4458",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "54a7420f-ad5d-41b2-a73c-bfb77fd9072b",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            NormalizedUserName = "ADMIN",
                            PasswordHash = "AQAAAAIAAYagAAAAEEdaDnbnfqJRtGLD6s0+EcEGj/20n+TBKarQN2E3xIxDzeOc6Xa1iAFEy5fvqRwpNA==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "1800fe8f-67d3-40b5-b4a4-3810610c7490",
                            TwoFactorEnabled = false,
                            UserName = "admin"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = "118d6207-3d51-4ad0-b059-ffab450e4458",
                            RoleId = "cb8e00f1-3be0-4d4b-816f-163e8040628a"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Rankt.Entities.RankingQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedById")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ExternalIdentifier")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifiedById")
                        .HasColumnType("TEXT");

                    b.Property<int?>("MaxSelectableItems")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StatusId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("ExternalIdentifier")
                        .IsUnique();

                    b.HasIndex("LastModifiedById");

                    b.HasIndex("StatusId");

                    b.ToTable("RankingQuestions");
                });

            modelBuilder.Entity("Rankt.Entities.RankingQuestionOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedById")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ExternalIdentifier")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifiedById")
                        .HasColumnType("TEXT");

                    b.Property<int>("RankingQuestionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("ExternalIdentifier")
                        .IsUnique();

                    b.HasIndex("LastModifiedById");

                    b.HasIndex("RankingQuestionId");

                    b.ToTable("RankingQuestionOptions");
                });

            modelBuilder.Entity("Rankt.Entities.RankingQuestionResponse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ExternalIdentifier")
                        .HasColumnType("TEXT");

                    b.Property<int>("RankingQuestionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RankingQuestionId");

                    b.ToTable("RankingQuestionResponses");
                });

            modelBuilder.Entity("Rankt.Entities.RankingQuestionResponseItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Rank")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RankingQuestionOptionId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RankingQuestionResponseId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RankingQuestionOptionId");

                    b.HasIndex("RankingQuestionResponseId");

                    b.ToTable("RankingQuestionResponseItems");
                });

            modelBuilder.Entity("Rankt.Entities.RankingQuestionStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("RankingQuestionStatus");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "in progress"
                        },
                        new
                        {
                            Id = 2,
                            Name = "completed"
                        },
                        new
                        {
                            Id = 3,
                            Name = "archived"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Rankt.Entities.RankingQuestion", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "LastModifiedBy")
                        .WithMany()
                        .HasForeignKey("LastModifiedById");

                    b.HasOne("Rankt.Entities.RankingQuestionStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");

                    b.Navigation("LastModifiedBy");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("Rankt.Entities.RankingQuestionOption", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "LastModifiedBy")
                        .WithMany()
                        .HasForeignKey("LastModifiedById");

                    b.HasOne("Rankt.Entities.RankingQuestion", "RankingQuestion")
                        .WithMany("Options")
                        .HasForeignKey("RankingQuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");

                    b.Navigation("LastModifiedBy");

                    b.Navigation("RankingQuestion");
                });

            modelBuilder.Entity("Rankt.Entities.RankingQuestionResponse", b =>
                {
                    b.HasOne("Rankt.Entities.RankingQuestion", "RankingQuestion")
                        .WithMany("Responses")
                        .HasForeignKey("RankingQuestionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("RankingQuestion");
                });

            modelBuilder.Entity("Rankt.Entities.RankingQuestionResponseItem", b =>
                {
                    b.HasOne("Rankt.Entities.RankingQuestionResponse", "RankingQuestionOption")
                        .WithMany()
                        .HasForeignKey("RankingQuestionOptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Rankt.Entities.RankingQuestionResponse", "RankingQuestionResponse")
                        .WithMany("ResponseItems")
                        .HasForeignKey("RankingQuestionResponseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RankingQuestionOption");

                    b.Navigation("RankingQuestionResponse");
                });

            modelBuilder.Entity("Rankt.Entities.RankingQuestion", b =>
                {
                    b.Navigation("Options");

                    b.Navigation("Responses");
                });

            modelBuilder.Entity("Rankt.Entities.RankingQuestionResponse", b =>
                {
                    b.Navigation("ResponseItems");
                });
#pragma warning restore 612, 618
        }
    }
}
