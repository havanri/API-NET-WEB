using EFCoreExam.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Attribute = EFCoreExam.Models.Attribute;

namespace EFCoreExam.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }

        #region DbSet
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<AlbumImage> AlbumImages { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<ProductTaxonomy> ProductTaxonomies { get; set; }
        public DbSet<Taxonomy> Taxonomies { get; set; }
        public DbSet<Attribute> Attributes { get; set; }
        public DbSet<SalePrice> SalePrices { get; set; }
        #endregion

        #region fluentAPI
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
           .HasOne(b => b.SalePrice)
           .WithOne(i => i.Product)
           .HasForeignKey<SalePrice>(b => b.ProductId);
            //category-product
            modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.CategoryId);

            //product-tag
            modelBuilder.Entity<ProductTag>()
            .HasKey(t => new { t.ProductId, t.TagId });
            modelBuilder.Entity<ProductTag>()
                .HasOne(pt => pt.Product)
                .WithMany(p => p.ProductTags)
                .HasForeignKey(pt => pt.ProductId);
            modelBuilder.Entity<ProductTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.ProductTags)
                .HasForeignKey(pt => pt.TagId);

            //Product-AlbumImage
            modelBuilder.Entity<AlbumImage>()
            .HasOne(p => p.Product)
            .WithMany(b => b.AlbumImages)
            .HasForeignKey(p => p.ProductId);

            /*//product-order(orderitem)
            modelBuilder.Entity<OrderItem>()
            .HasKey(t => new { t.ProductId, t.OrderId });
            modelBuilder.Entity<OrderItem>()
                .HasOne(pt => pt.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(pt => pt.ProductId);
            modelBuilder.Entity<OrderItem>()
                .HasOne(pt => pt.Order)
                .WithMany(t => t.OrderItems)
                .HasForeignKey(pt => pt.OrderId);*/

            /*//customer-order
            modelBuilder.Entity<Order>()
            .HasOne(p => p.Customer)
            .WithMany(b => b.Orders)
            .HasForeignKey(p => p.CustomerId);*/

            //product-taxonomy(productTaxonomy)
            modelBuilder.Entity<ProductTaxonomy>()
            .HasKey(t => new { t.ProductId, t.TaxonomyId });
            modelBuilder.Entity<ProductTaxonomy>()
                .HasOne(pt => pt.Product)
                .WithMany(p => p.ProductTaxonomies)
                .HasForeignKey(pt => pt.ProductId);
            modelBuilder.Entity<ProductTaxonomy>()
                .HasOne(pt => pt.Taxonomy)
                .WithMany(t => t.ProductTaxonomies)
                .HasForeignKey(pt => pt.TaxonomyId);

            //attribute-taxonomy
            modelBuilder.Entity<Taxonomy>()
            .HasOne(p => p.Attribute)
            .WithMany(b => b.Taxonomies)
            .HasForeignKey(p => p.AttributeId);

            base.OnModelCreating(modelBuilder);

        }
        #endregion

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }*/
    }
}
