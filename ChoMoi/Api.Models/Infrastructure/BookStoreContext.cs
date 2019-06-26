using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using ChoMoi.Api.Models;

namespace DemoAPI.Models
{
    public partial class BookStoreContext : IdentityDbContext<User>
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        public BookStoreContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual DbSet<AuthorContact> AuthorContact { get; set; }
        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<BookAuthors> BookAuthors { get; set; }
        public virtual DbSet<BookCategory> BookCategory { get; set; }
        public virtual DbSet<Publisher> Publisher { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                .UseSqlServer("server=HAPTXSE62665\\SQLEXPRESS;database=BookStoreManager;User ID=sa;password=1111;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public static BookStoreContext Create(DbContextOptions options, IHttpContextAccessor httpContextAccessor)
        {
            return new BookStoreContext(options, httpContextAccessor);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries();
            List<EntityEntry> modifiedEntries = new List<EntityEntry>();

            //Get entries add or update to database
            foreach (var entry in entries)
            {
                if (entry.Entity is IAuditableEntity && entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    modifiedEntries.Add(entry);
                }
            }

            //Set CreatedBy, CreatedDate, UpdatedBy, UpdatedDate for IAuditableEntity
            foreach (var entry in modifiedEntries)
            {
                IAuditableEntity entity = entry.Entity as IAuditableEntity;
                if (entity != null)
                {
                    //string identityName = Thread.CurrentPrincipal.Identity.Name;
                    string identityName = _httpContextAccessor.HttpContext.User.Identity.Name;
                    DateTime now = DateTime.Now;

                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedBy = string.IsNullOrEmpty(entity.CreatedBy) ? identityName : entity.CreatedBy;
                        entity.CreatedDate = now;
                    }
                    else
                    {
                        base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                        base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                    }

                    entity.UpdatedBy = identityName;
                    entity.UpdatedDate = now;
                }
            }

            return base.SaveChanges();
        }

        public DbSet<ChoMoi.Api.Models.BookBuy> BookBuy { get; set; }

    }
}
