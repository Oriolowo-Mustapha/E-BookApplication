using E_BookApplication.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace E_BookApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

      
        public DbSet<Book> Books { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

           
            builder.Entity<Book>()
                .HasOne(b => b.Vendor)
                .WithMany(u => u.Books)
                .HasForeignKey(b => b.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

           
            builder.Entity<CartItem>()
                .HasOne(c => c.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CartItem>()
                .HasOne(c => c.Book)
                .WithMany(b => b.CartItems)
                .HasForeignKey(c => c.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>().OwnsOne(o => o.ShippingAddress);


            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order>()
                .HasOne(o => o.PaymentMethod)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order>()
                .HasOne(o => o.Coupon)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CouponId)
                .OnDelete(DeleteBehavior.SetNull);


            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Book)
                .WithMany(b => b.OrderItems)
                .HasForeignKey(oi => oi.BookId)
                .OnDelete(DeleteBehavior.Restrict);

      
            builder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
                .HasOne(r => r.Book)
                .WithMany(b => b.Reviews)
                .HasForeignKey(r => r.BookId)
                .OnDelete(DeleteBehavior.Cascade);

     
            builder.Entity<Wishlist>()
                .HasOne(w => w.User)
                .WithMany(u => u.Wishlists)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Wishlist>()
                .HasOne(w => w.Book)
                .WithMany(b => b.Wishlists)
                .HasForeignKey(w => w.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
                .HasIndex(r => new { r.UserId, r.BookId })
                .IsUnique(); 

            builder.Entity<Wishlist>()
                .HasIndex(w => new { w.UserId, w.BookId })
                .IsUnique();

            builder.Entity<CartItem>()
                .HasIndex(c => new { c.UserId, c.BookId })
                .IsUnique();

            builder.Entity<Coupon>()
                .HasIndex(c => c.Code)
                .IsUnique();
           
            SeedData(builder);
        }

        private void SeedData(ModelBuilder builder)
        {
           
            builder.Entity<PaymentMethod>().HasData(
                new PaymentMethod
                {
                    Id = 1,
                    BankName = "Bank Transfer",
                    BankCode = "TRANSFER",
                    PaymentType = "BankTransfer",
                    Instructions = "Transfer to the provided account details",
                    IsActive = true
                },
                new PaymentMethod
                {
                    Id = 2,
                    BankName = "Card Payment",
                    BankCode = "CARD",
                    PaymentType = "Card",
                    Instructions = "Pay with your debit or credit card",
                    IsActive = true
                },
                new PaymentMethod
                {
                    Id = 3,
                    BankName = "PayPal",
                    BankCode = "PAYPAL",
                    PaymentType = "PayPal",
                    Instructions = "Pay securely with PayPal",
                    IsActive = true
                }
            );

           
            builder.Entity<Coupon>().HasData(
                new Coupon
                {
                    Id = 1,
                    Code = "WELCOMEE-B00kKAY",
                    Description = "Welcome discount - 10% off",
                    DiscountAmount = 10,
                    IsPercentage = true,
                    ExpiryDate = DateTime.UtcNow.AddDays(20),
                    IsActive = true,
                    UsageLimit = 100,
                    UsedCount = 0,
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
