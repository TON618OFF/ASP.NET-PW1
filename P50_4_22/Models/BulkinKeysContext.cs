using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace P50_4_22.Models;

public partial class BulkinKeysContext : DbContext
{
    public BulkinKeysContext()
    {
    }

    public BulkinKeysContext(DbContextOptions<BulkinKeysContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientsAddress> ClientsAddresses { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<PositionOrder> PositionOrders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=JULIANLOKI\\SQLEXPRESS;Initial Catalog=BulkinKeys;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.IdCartItem).HasName("PK__CartItem__B943DA22C2014D95");

            entity.Property(e => e.IdCartItem).HasColumnName("ID_CartItem");
            entity.Property(e => e.ClientId).HasColumnName("Client_ID");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(d => d.Client).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItems__Clien__6FE99F9F");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItems__Produ__70DDC3D8");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory).HasName("PK__Categori__6DB3A68A23453D55");

            entity.HasIndex(e => e.CategoryName, "UQ__Categori__8517B2E0BA022990").IsUnique();

            entity.Property(e => e.IdCategory).HasColumnName("ID_Category");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("PK__Clients__B5AE4EC8FA8267C5");

            entity.HasIndex(e => e.ClientLogin, "UQ__Clients__5BF1A31142AC2BB3").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "UQ__Clients__85FB4E38597994E3").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Clients__A9D10534817699CF").IsUnique();

            entity.Property(e => e.IdClient).HasColumnName("ID_Client");
            entity.Property(e => e.ClientAddressId).HasColumnName("ClientAddress_ID");
            entity.Property(e => e.ClientLogin)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ClientMiddleName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ClientPassword).IsUnicode(false);
            entity.Property(e => e.ClientSurname)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("Role_ID");

            entity.HasOne(d => d.ClientAddress).WithMany(p => p.Clients)
                .HasForeignKey(d => d.ClientAddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Clients__ClientA__52593CB8");

            entity.HasOne(d => d.Role).WithMany(p => p.Clients)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Clients__Role_ID__534D60F1");
        });

        modelBuilder.Entity<ClientsAddress>(entity =>
        {
            entity.HasKey(e => e.IdClientAddress).HasName("PK__ClientsA__FA7316702227EC17");

            entity.HasIndex(e => e.AddressLocation, "UQ__ClientsA__853D68BA88B9DBAA").IsUnique();

            entity.Property(e => e.IdClientAddress).HasColumnName("ID_ClientAddress");
            entity.Property(e => e.AddressCity)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AddressCountry)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AddressLocation)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.AddressPostalCode)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("PK__Orders__EC9FA955ED036071");

            entity.HasIndex(e => e.OrderNumber, "UQ__Orders__CAC5E743F1470B7C").IsUnique();

            entity.Property(e => e.IdOrder).HasColumnName("ID_Order");
            entity.Property(e => e.ClientId).HasColumnName("Client_ID");
            entity.Property(e => e.OrderDate)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.OrderNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.OrderTotalSum).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.StatusId).HasColumnName("Status_ID");

            entity.HasOne(d => d.Client).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__Client_I__68487DD7");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__Status_I__6754599E");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("PK__OrderSta__5AC2A734F94C1012");

            entity.ToTable("OrderStatus");

            entity.HasIndex(e => e.StatusName, "UQ__OrderSta__05E7698A0738FC23").IsUnique();

            entity.Property(e => e.IdStatus).HasColumnName("ID_Status");
            entity.Property(e => e.StatusName)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PositionOrder>(entity =>
        {
            entity.HasKey(e => e.IdPositionOrder).HasName("PK__Position__969ABB7C683E32D6");

            entity.Property(e => e.IdPositionOrder).HasColumnName("ID_PositionOrder");
            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.PositionOrderPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductId).HasColumnName("Product_ID");

            entity.HasOne(d => d.Order).WithMany(p => p.PositionOrders)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PositionO__Order__6B24EA82");

            entity.HasOne(d => d.Product).WithMany(p => p.PositionOrders)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PositionO__Produ__6C190EBB");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("PK__Products__522DE496A334E1B6");

            entity.HasIndex(e => e.ProductImage, "UQ__Products__465B783CA6D84990").IsUnique();

            entity.HasIndex(e => e.ProductName, "UQ__Products__DD5A978AA03C17A0").IsUnique();

            entity.Property(e => e.IdProduct).HasColumnName("ID_Product");
            entity.Property(e => e.CategoryId).HasColumnName("Category_ID");
            entity.Property(e => e.ProductDescription)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ProductImage)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Catego__5AEE82B9");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.IdReview).HasName("PK__Reviews__E39E96474B8BFC14");

            entity.Property(e => e.IdReview).HasColumnName("ID_Review");
            entity.Property(e => e.ClientNameId).HasColumnName("ClientName_ID");
            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
            entity.Property(e => e.ReviewComment).IsUnicode(false);
            entity.Property(e => e.ReviewCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.ClientName).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ClientNameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__ClientN__5EBF139D");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__Product__5DCAEF64");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PK__Roles__43DCD32D9A200A71");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B616055FAB748").IsUnique();

            entity.Property(e => e.IdRole).HasColumnName("ID_Role");
            entity.Property(e => e.RoleName)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
