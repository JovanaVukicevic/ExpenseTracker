﻿// <auto-generated />
using System;
using ExpenseTracker.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ExpenseTracker.API.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ExpenseTracker.Repository.Models.Account", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SavingsAccountID")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasIndex("UserId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("ExpenseTracker.Repository.Models.Category", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("BudgetCap")
                        .HasColumnType("float");

                    b.Property<string>("Indicator")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Name");

                    b.HasIndex("UserId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ExpenseTracker.Repository.Models.SavingsAccount", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("AccountID")
                        .HasColumnType("int");

                    b.Property<double>("AmountPerMonth")
                        .HasColumnType("float");

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TargetAmount")
                        .HasColumnType("float");

                    b.Property<DateTime>("TargetDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("AccountID")
                        .IsUnique();

                    b.ToTable("SavingsAccounts");
                });

            modelBuilder.Entity("ExpenseTracker.Repository.Models.Scheduled", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("AccountID")
                        .HasColumnType("int");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Indicator")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TimeIntervalInDays")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("AccountID");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("ExpenseTracker.Repository.Models.Transaction", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("AccountID")
                        .HasColumnType("int");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Indicator")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("AccountID");

                    b.HasIndex("CategoryName");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("ExpenseTracker.Repository.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPremuium")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<int>("SavingsAccountID")
                        .HasColumnType("int");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = "userId1",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "8ae8974d-e187-46d4-974f-c2bc4fedf410",
                            Email = "ivan.ivanovic123gmail.com",
                            EmailConfirmed = false,
                            FirstName = "Ivan",
                            IsPremuium = false,
                            LastName = "Ivanovic",
                            LockoutEnabled = false,
                            PasswordHash = "AQAAAAIAAYagAAAAEEbhTlqUtBacLYRYyrTC2d3x4bD8Vbf11+pXjD1ArVEd8EKohwEcM2fUx8mbXrndWw==",
                            PhoneNumberConfirmed = false,
                            SavingsAccountID = 0,
                            SecurityStamp = "fd290807-bd90-4c1f-b22c-847f55f3f402",
                            TwoFactorEnabled = false,
                            UserName = "ivan1234"
                        },
                        new
                        {
                            Id = "userId2",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "0cc7a561-032f-4c14-ad3c-3c51d485e141",
                            Email = "jovan.ivanovic123gmail.com",
                            EmailConfirmed = false,
                            FirstName = "Jovan",
                            IsPremuium = false,
                            LastName = "Ivanovic",
                            LockoutEnabled = false,
                            PasswordHash = "AQAAAAIAAYagAAAAEFVaKw+TsZmjwJZre52QdpAjC5R7vzWw40oElnxWXqChwqYeRd3O3qm+Qo6LWiM59A==",
                            PhoneNumberConfirmed = false,
                            SavingsAccountID = 0,
                            SecurityStamp = "473f33c1-8daf-4c8d-b735-9d4969c81331",
                            TwoFactorEnabled = false,
                            UserName = "jovan1234"
                        },
                        new
                        {
                            Id = "userId3",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "b8b4eaf0-f2bf-4248-9586-aa2444c315af",
                            Email = "milica.bulat@gmail.com",
                            EmailConfirmed = false,
                            FirstName = "Milica",
                            IsPremuium = false,
                            LastName = "Bulatovic",
                            LockoutEnabled = false,
                            PasswordHash = "AQAAAAIAAYagAAAAEE10xzI0W5ydXJoauuyH4K28K3Qd+hA63bjVVCccOM0OT32iYLAKJz64A+5LwItJuQ==",
                            PhoneNumberConfirmed = false,
                            SavingsAccountID = 0,
                            SecurityStamp = "d81f7dbf-0f4d-44d3-9777-f9e73d5bff6e",
                            TwoFactorEnabled = false,
                            UserName = "milica1234"
                        },
                        new
                        {
                            Id = "userId4",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "582904b5-4f38-4147-8756-0664cef364ae",
                            Email = "ivana.milos@gmail.com",
                            EmailConfirmed = false,
                            FirstName = "Ivana",
                            IsPremuium = false,
                            LastName = "Milosevic",
                            LockoutEnabled = false,
                            PasswordHash = "AQAAAAIAAYagAAAAEHkLDG+wsJJiKhxcHhhlCv4Qhj1ppB/eSBZp3Hl4GILedV26C3fjgNEQ6n9GLF4LPQ==",
                            PhoneNumberConfirmed = false,
                            SavingsAccountID = 0,
                            SecurityStamp = "ef2f1522-a7db-470b-89c8-8dd0b6dc45af",
                            TwoFactorEnabled = false,
                            UserName = "ivana123456"
                        },
                        new
                        {
                            Id = "userId5",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "fd78cfcd-475c-4eee-8a83-192f43f00cd1",
                            Email = "kaca.bulat@gmail.com",
                            EmailConfirmed = false,
                            FirstName = "Katarina",
                            IsPremuium = false,
                            LastName = "Bulatovic",
                            LockoutEnabled = false,
                            PasswordHash = "AQAAAAIAAYagAAAAEO3dVWJEntMnZPuXm3NUvaWjQ3TYAmoresQ/pSE8SHVMpctgwkLmLgT5Fag0GetZWg==",
                            PhoneNumberConfirmed = false,
                            SavingsAccountID = 0,
                            SecurityStamp = "87c0dfc4-b4f2-49f9-a261-9498321be427",
                            TwoFactorEnabled = false,
                            UserName = "katarina1234"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = "0",
                            Name = "Admin"
                        },
                        new
                        {
                            Id = "1",
                            Name = "Premium"
                        },
                        new
                        {
                            Id = "2",
                            Name = "User"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.ToTable("UserRoles");

                    b.HasData(
                        new
                        {
                            UserId = "userId1",
                            RoleId = "2"
                        },
                        new
                        {
                            UserId = "userId2",
                            RoleId = "2"
                        },
                        new
                        {
                            UserId = "userId3",
                            RoleId = "2"
                        },
                        new
                        {
                            UserId = "userId4",
                            RoleId = "2"
                        },
                        new
                        {
                            UserId = "userId5",
                            RoleId = "2"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("ExpenseTracker.Repository.Models.Account", b =>
                {
                    b.HasOne("ExpenseTracker.Repository.Models.User", "user")
                        .WithMany("Accounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("ExpenseTracker.Repository.Models.Category", b =>
                {
                    b.HasOne("ExpenseTracker.Repository.Models.User", "User")
                        .WithMany("Categories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ExpenseTracker.Repository.Models.SavingsAccount", b =>
                {
                    b.HasOne("ExpenseTracker.Repository.Models.Account", "Account")
                        .WithOne("SavingsAccount")
                        .HasForeignKey("ExpenseTracker.Repository.Models.SavingsAccount", "AccountID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("ExpenseTracker.Repository.Models.Scheduled", b =>
                {
                    b.HasOne("ExpenseTracker.Repository.Models.Account", "Account")
                        .WithMany("ScheduledTransactions")
                        .HasForeignKey("AccountID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("ExpenseTracker.Repository.Models.Transaction", b =>
                {
                    b.HasOne("ExpenseTracker.Repository.Models.Account", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ExpenseTracker.Repository.Models.Category", "Category")
                        .WithMany("TransactionsPerCategory")
                        .HasForeignKey("CategoryName")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("ExpenseTracker.Repository.Models.Account", b =>
                {
                    b.Navigation("SavingsAccount")
                        .IsRequired();

                    b.Navigation("ScheduledTransactions");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("ExpenseTracker.Repository.Models.Category", b =>
                {
                    b.Navigation("TransactionsPerCategory");
                });

            modelBuilder.Entity("ExpenseTracker.Repository.Models.User", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Categories");
                });
#pragma warning restore 612, 618
        }
    }
}
