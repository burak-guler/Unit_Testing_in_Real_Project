using Microsoft.EntityFrameworkCore;

namespace UdemyRealWorldUnitTest.Web.Models
{
    public partial class UdemyUnitTestDBContext : DbContext
    {
        //public UdemyUnitTestDBContext()
        //{
        //}

        public UdemyUnitTestDBContext(DbContextOptions<UdemyUnitTestDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; } = null!;

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Color).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
