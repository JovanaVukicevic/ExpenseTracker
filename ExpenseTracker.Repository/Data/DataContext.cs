using ExpenseTracker.Repository.Constants;
using ExpenseTracker.Repository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repository.Data;


public class DataContext : IdentityDbContext<User>
{
     public DataContext(DbContextOptions<DataContext> options) : base(options)
     {
     }

     public DbSet<Account> Accounts { get; set; }

     public DbSet<SavingsAccount> SavingsAccounts { get; set; }

     public DbSet<Transaction> Transactions { get; set; }

     public DbSet<Scheduled> Schedules { get; set; }

     public DbSet<Category> Categories { get; set; }

     protected override void OnModelCreating(ModelBuilder builder)
     {

          builder.Entity<IdentityUserLogin<string>>(entity =>
          {
               entity.HasKey(l => new { l.LoginProvider, l.ProviderKey });
          });

          builder.Entity<IdentityUserRole<string>>(entity =>
          {
               entity.HasKey(r => new { r.UserId, r.RoleId });
          });

          builder.Entity<IdentityUserToken<string>>().HasKey(u => new { u.UserId, u.LoginProvider, u.Name });
          var user1 =
              new User
              {
                   Id = "userId1",
                   FirstName = "Ivan",
                   LastName = "Ivanovic",
                   Email = "ivan.ivanovic123gmail.com",
                   UserName = "ivan1234",
                   IsPremuium = false
              };
          var user2 =
               new User
               {
                    Id = "userId2",
                    FirstName = "Jovan",
                    LastName = "Ivanovic",
                    Email = "jovan.ivanovic123gmail.com",
                    UserName = "jovan1234",
                    IsPremuium = false


               };
          var user3 =
               new User
               {
                    Id = "userId3",
                    FirstName = "Milica",
                    LastName = "Bulatovic",
                    Email = "milica.bulat@gmail.com",
                    UserName = "milica1234",
                    IsPremuium = false
               };

          var user4 =
               new User
               {
                    Id = "userId4",
                    FirstName = "Ivana",
                    LastName = "Milosevic",
                    Email = "ivana.milos@gmail.com",
                    UserName = "ivana123456",
                    IsPremuium = false
               };

          var user5 =
               new User
               {
                    Id = "userId5",
                    FirstName = "Katarina",
                    LastName = "Bulatovic",
                    Email = "kaca.bulat@gmail.com",
                    UserName = "katarina1234",
                    IsPremuium = false
               };

          var passwordHasher = new PasswordHasher<User>();
          user1.PasswordHash = passwordHasher.HashPassword(user1, "passworD1!");
          user2.PasswordHash = passwordHasher.HashPassword(user2, "passworD2!");
          user3.PasswordHash = passwordHasher.HashPassword(user3, "passworD3!");
          user4.PasswordHash = passwordHasher.HashPassword(user4, "passworD4!");
          user5.PasswordHash = passwordHasher.HashPassword(user5, "passworD5!");
          builder.Entity<User>().HasData(user1, user2, user3, user4, user5);


          builder.Entity<IdentityRole>().HasData(
              new IdentityRole<string> { Id = RoleIds.Admin, Name = Constants.Roles.Admin },
              new IdentityRole<string> { Id = RoleIds.Premium, Name = Constants.Roles.Premium },
              new IdentityRole<string> { Id = RoleIds.User, Name = Constants.Roles.User });


          builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
          {
               RoleId = RoleIds.User,
               UserId = user1.Id
          });
          builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
          {
               RoleId = RoleIds.User,
               UserId = user2.Id
          });
          builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
          {
               RoleId = RoleIds.User,
               UserId = user3.Id
          });
          builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
          {
               RoleId = RoleIds.User,
               UserId = user4.Id
          });

          builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
          {
               RoleId = RoleIds.User,
               UserId = user5.Id
          });
          builder.Entity<Account>().HasKey(a => a.ID);
          builder.Entity<Account>()
          .HasOne(a => a.user)
          .WithMany(u => u.Accounts)
          .HasForeignKey(a => a.UserId);

          builder.Entity<SavingsAccount>().HasKey(sa => sa.ID);
          builder.Entity<SavingsAccount>()
          .HasOne(sa => sa.Account)
          .WithOne(a => a.SavingsAccount)
          .HasForeignKey<SavingsAccount>(a => a.AccountID)
          .IsRequired(true);

          builder.Entity<Transaction>().HasKey(t => t.ID);
          builder.Entity<Transaction>()
          .HasOne(t => t.Account)
          .WithMany(a => a.Transactions)
          .HasForeignKey(t => t.AccountID);

          builder.Entity<Transaction>()
          .HasOne(t => t.Category)
          .WithMany(a => a.TransactionsPerCategory)
          .HasForeignKey(t => t.CategoryName)
          .OnDelete(DeleteBehavior.Restrict);

          builder.Entity<Scheduled>().HasKey(t => t.ID);
          builder.Entity<Scheduled>()
          .HasOne(s => s.Account)
          .WithMany(a => a.ScheduledTransactions)
          .HasForeignKey(s => s.AccountID);

          builder.Entity<Category>().HasKey(c => c.Name);
          builder.Entity<Category>()
          .HasOne(c => c.User)
          .WithMany(u => u.Categories)
          .HasForeignKey(c => c.UserId);
     }
}