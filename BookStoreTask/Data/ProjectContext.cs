using BookStoreTask.BookMod.Books.Model;
using BookStoreTask.BookMod.Catograzation.Author.model;
using BookStoreTask.BookMod.Catograzation.BaseCatgories.Model;
using BookStoreTask.BookMod.Catograzation.Genere.Model;
using BookStoreTask.Cart;
using BookStoreTask.Cart.CartItems;
using BookStoreTask.FilesMod;
using BookStoreTask.Orders;
using BookStoreTask.Users.Admins;
using BookStoreTask.Users.BaseUser;
using BookStoreTask.Users.Customers;
using BookStoreTask.Utli;
using Microsoft.EntityFrameworkCore;

namespace BookStoreTask.Data;

public class ProjectContext: DbContext
{
    
    public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
    {
    }

    public DbSet<ProjectFiles> Files { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<BooksModel> Books { get; set; }
    public DbSet<BaseCategory> Catgories { get; set; }
    public DbSet<Authors> Authors { get; set; }
    public DbSet<Genres> Genres { get; set; }
    public DbSet<Carts> Carts { get; set; }
    public DbSet<OrdersModel> Orders { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().HasBaseType<User>();
        modelBuilder.Entity<Admin>().HasBaseType<User>();
        
        modelBuilder.Entity<Genres>().HasBaseType<BaseCategory>();
        modelBuilder.Entity<Authors>().HasBaseType<BaseCategory>();
        
        // One-to-one relationship between Customer and Cart
        modelBuilder.Entity<Customer>()
            .HasOne(c => c.Cart) // Customer has one Cart
            .WithOne(c => c.Customer) // Cart has one Customer
            .HasForeignKey<Carts>(c => c.CustomerId) // Foreign key is on Cart
            .OnDelete(DeleteBehavior.Cascade); // Optional: specify delete behavior
        
        // Configure one-to-many relationship between Cart and CartItem
        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Cart)
            .WithMany(c => c.CartItems)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure one-to-many relationship between Book and CartItem
        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Book)
            .WithMany()
            .HasForeignKey(ci => ci.BookId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Configure one-to-many relationship between Order and CartItem
        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Order)
            .WithMany(o => o.CartItems)
            .HasForeignKey(ci => ci.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    
    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity<Guid> &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            var entity = (BaseEntity<Guid>)entityEntry.Entity;
            entity.UpdatedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }
        }
    }
    
}