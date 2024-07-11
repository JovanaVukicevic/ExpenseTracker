﻿// <auto-generated />
using System;
using ExpenseTracker.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ExpenseTrack.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240711102527_FirstMigration")]
    partial class FirstMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
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
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SavingsAccountID")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
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

                    b.HasKey("Name");

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
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TargetAmount")
                        .HasColumnType("float");

                    b.Property<DateTime>("TargetDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserID")
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
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Indicator")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("Name")
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
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Indicator")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("Name")
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
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPremuium")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
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
                            ConcurrencyStamp = "59b1ea18-926f-408a-ae22-8eb3e0199d97",
                            Email = "ivan.ivanovic123gmail.com",
                            EmailConfirmed = false,
                            FirstName = "Ivan",
                            IsPremuium = false,
                            LastName = "Ivanovic",
                            LockoutEnabled = false,
                            PasswordHash = "AQAAAAIAAYagAAAAEEoGTHxrd0FxEaBMjHaTsRNAwo29xdbPpAxtvLuLS9zL/oexxftJl4Awys59gA9qvw==",
                            PhoneNumberConfirmed = false,
                            SavingsAccountID = 0,
                            SecurityStamp = "bcbe6104-cc26-4e10-8cee-42eaab33b51a",
                            TwoFactorEnabled = false,
                            UserName = "ivan1234"
                        },
                        new
                        {
                            Id = "userId2",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "a3581ff2-7712-486c-b0ae-9541ce02cf3f",
                            Email = "jovan.ivanovic123gmail.com",
                            EmailConfirmed = false,
                            FirstName = "Jovan",
                            IsPremuium = false,
                            LastName = "Ivanovic",
                            LockoutEnabled = false,
                            PasswordHash = "AQAAAAIAAYagAAAAEOccT5tgOBsdDZ62Jh1plawosxK/sOcK1XsRY6O8yaXKYgPwa+ePTtg3jtk2sEP53A==",
                            PhoneNumberConfirmed = false,
                            SavingsAccountID = 0,
                            SecurityStamp = "b7dac079-371f-480a-88bb-8009756f2047",
                            TwoFactorEnabled = false,
                            UserName = "jovan1234"
                        },
                        new
                        {
                            Id = "userId3",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "24c91510-b3f3-4310-98d9-39e1b6acf6cd",
                            Email = "milica.bulat@gmail.com",
                            EmailConfirmed = false,
                            FirstName = "Milica",
                            IsPremuium = false,
                            LastName = "Bulatovic",
                            LockoutEnabled = false,
                            PasswordHash = "AQAAAAIAAYagAAAAEOHS+ayVdh5bDTSq9x23PHlfLg67GTPbn6vAbT/9xUmnKDZ2T/J9TJxqx872WiHMfg==",
                            PhoneNumberConfirmed = false,
                            SavingsAccountID = 0,
                            SecurityStamp = "0ea735d2-d2d9-4546-a67d-02bad549e332",
                            TwoFactorEnabled = false,
                            UserName = "milica1234"
                        },
                        new
                        {
                            Id = "userId4",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "38ce9013-a7aa-4a80-aeb1-9c1a8881d75a",
                            Email = "ivana.milos@gmail.com",
                            EmailConfirmed = false,
                            FirstName = "Ivana",
                            IsPremuium = false,
                            LastName = "Milosevic",
                            LockoutEnabled = false,
                            PasswordHash = "AQAAAAIAAYagAAAAECgvKW+E7jHD+6mZDkyM6BjyMZ64xyHzJJ3lp7P/XlFgrI/FPr0hIe+YZf12YsIUZA==",
                            PhoneNumberConfirmed = false,
                            SavingsAccountID = 0,
                            SecurityStamp = "70f8ae66-3ddb-4364-a434-cb86026c2e0b",
                            TwoFactorEnabled = false,
                            UserName = "ivana123456"
                        },
                        new
                        {
                            Id = "userId5",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "236d3101-9b0e-4855-9234-884633855006",
                            Email = "kaca.bulat@gmail.com",
                            EmailConfirmed = false,
                            FirstName = "Katarina",
                            IsPremuium = false,
                            LastName = "Bulatovic",
                            LockoutEnabled = false,
                            PasswordHash = "AQAAAAIAAYagAAAAEFZ86p2x+t8aZkH4TzAI2tXyTCBseUM1K75kgoL/fnCpXkXCvbf6+zP+IsvhCY13YQ==",
                            PhoneNumberConfirmed = false,
                            SavingsAccountID = 0,
                            SecurityStamp = "56e5fe84-7c0d-4164-beef-48ce4aa61cfd",
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
                        .HasForeignKey("UserId");

                    b.Navigation("user");
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
                        .HasForeignKey("CategoryName");

                    b.Navigation("Account");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("ExpenseTracker.Repository.Models.Account", b =>
                {
                    b.Navigation("SavingsAccount");

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
                });
#pragma warning restore 612, 618
        }
    }
}
