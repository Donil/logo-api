namespace Logo.DataAccess
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using Logo.Domain.BusinessCards;
    using Logo.Domain.Shape;
    using Logo.DataAccess.Models;
    using Logo.Domain.Users;
    using System;

    /// <summary>
    /// Application db context.
    /// </summary>
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="options"></param>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Business cards db set.
        /// </summary>
        public DbSet<BusinessCard> BusinessCards { get; set; }

        /// <summary>
        /// Pimitives db set.
        /// </summary>
        public DbSet<Shape> Shapes { get; set; }

        /// <summary>
        /// Categories db set.
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Shapes categories relations.
        /// </summary>
        /// <returns></returns>
        public DbSet<ShapeCategory> ShapeCategory { get; set; }

        /// <summary>
        /// Users db set.
        /// </summary>
        public new DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ShapeCategory>()
                .HasAlternateKey(pt => new { pt.ShapeId, pt.CategoryId });

            builder.Entity<ShapeCategory>()
                .HasOne(pt => pt.Shape)
                .WithMany(p => p.ShapeCategories)
                .HasForeignKey(pt => pt.ShapeId);

            builder.Entity<ShapeCategory>()
                .HasOne(pt => pt.Category)
                .WithMany(t => t.CategoryShapes)
                .HasForeignKey(pt => pt.CategoryId);

            base.OnModelCreating(builder);
        }
    }
}
