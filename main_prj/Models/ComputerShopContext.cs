using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace main_prj.Models;

public partial class ComputerShopContext : DbContext
{
    public ComputerShopContext()
    {
    }

    public ComputerShopContext(DbContextOptions<ComputerShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartDetail> CartDetails { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Headphone> Headphones { get; set; }

    public virtual DbSet<Keyboard> Keyboards { get; set; }

    public virtual DbSet<Monitor> Monitors { get; set; }

    public virtual DbSet<Mouse> Mice { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Specification> Specifications { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=ComputerShop;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Carts__51BCD79712D6ECB6");

            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Carts__UserID__1F63A897");
        });

        modelBuilder.Entity<CartDetail>(entity =>
        {
            entity.HasKey(e => e.CartDetailId).HasName("PK__CartDeta__01B6A6D462E37706");

            entity.Property(e => e.CartDetailId).HasColumnName("CartDetailID");
            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartDetails)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartDetai__CartI__24285DB4");

            entity.HasOne(d => d.Product).WithMany(p => p.CartDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartDetai__Produ__2334397B");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2BA5AF0244");

            entity.HasIndex(e => e.CategoryName, "UQ__Categori__8517B2E0E4819EB8").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryImage).HasMaxLength(50);
            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<Headphone>(entity =>
        {
            entity.HasKey(e => e.HeadphoneId).HasName("PK__Headphon__97CC828A015F321C");

            entity.Property(e => e.Accessories).HasMaxLength(100);
            entity.Property(e => e.Battery).HasMaxLength(100);
            entity.Property(e => e.Brand).HasMaxLength(100);
            entity.Property(e => e.Color).HasMaxLength(20);
            entity.Property(e => e.Compatibilities).HasMaxLength(100);
            entity.Property(e => e.ConnectionType).HasMaxLength(100);
            entity.Property(e => e.HeadphoneType).HasMaxLength(20);
            entity.Property(e => e.Microphone).HasMaxLength(100);
            entity.Property(e => e.Model).HasMaxLength(100);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany(p => p.Headphones)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Headphone__Produ__74AE54BC");
        });

        modelBuilder.Entity<Keyboard>(entity =>
        {
            entity.HasKey(e => e.KeyboardId).HasName("PK__Keyboard__EB920E51A28FD35A");

            entity.Property(e => e.Battery).HasMaxLength(100);
            entity.Property(e => e.Brand).HasMaxLength(100);
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.ConnectionType).HasMaxLength(100);
            entity.Property(e => e.KeyboardType).HasMaxLength(100);
            entity.Property(e => e.Led).HasMaxLength(100);
            entity.Property(e => e.Model).HasMaxLength(100);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Switch).HasMaxLength(100);
            entity.Property(e => e.Weight).HasMaxLength(50);

            entity.HasOne(d => d.Product).WithMany(p => p.Keyboards)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Keyboards__Produ__6EF57B66");
        });

        modelBuilder.Entity<Monitor>(entity =>
        {
            entity.HasKey(e => e.MonitorId).HasName("PK__Monitors__DF5D95F89E0807B2");

            entity.Property(e => e.AspectRatio).HasMaxLength(20);
            entity.Property(e => e.Brand).HasMaxLength(100);
            entity.Property(e => e.Model).HasMaxLength(100);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Resolution).HasMaxLength(20);
            entity.Property(e => e.Size).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.Monitors)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Monitors__Produc__70DDC3D8");
        });

        modelBuilder.Entity<Mouse>(entity =>
        {
            entity.HasKey(e => e.MouseId).HasName("PK__Mice__156E0A6820A1A62A");

            entity.Property(e => e.Battery).HasMaxLength(100);
            entity.Property(e => e.Brand).HasMaxLength(100);
            entity.Property(e => e.Color).HasMaxLength(100);
            entity.Property(e => e.ConnectionType).HasMaxLength(100);
            entity.Property(e => e.Model).HasMaxLength(100);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Resolution).HasMaxLength(100);
            entity.Property(e => e.Weight).HasMaxLength(100);

            entity.HasOne(d => d.Product).WithMany(p => p.Mice)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Mice__ProductID__72C60C4A");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAF127732F4");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.ContactNumber).HasMaxLength(20);
            entity.Property(e => e.OrderDate).HasColumnType("datetimeoffset");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.Receiver).HasMaxLength(100);
            entity.Property(e => e.ShippingAddress).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Cart).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Cart");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Orders__UserID__693CA210");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6ED38D2BE2C");

            entity.HasIndex(e => e.ProductName, "UQ_ProductName").IsUnique();

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductName).HasMaxLength(255);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Product_Category");
        });

        modelBuilder.Entity<Specification>(entity =>
        {
            entity.HasKey(e => e.SpecId).HasName("PK__Specific__883D519BD13A82A9");

            entity.Property(e => e.SpecId).HasColumnName("SpecID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.SpecName).HasMaxLength(50);

            entity.HasOne(d => d.Product).WithMany(p => p.Specifications)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Specifica__Produ__43A1090D");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACE1626319");

            entity.HasIndex(e => e.Email, "UQ_Email").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UserName).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
