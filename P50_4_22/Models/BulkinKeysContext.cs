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

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientsAddress> ClientsAddresses { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<PositionOrder> PositionOrders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=JULIANLOKI\\SQLEXPRESS;Initial Catalog=BulkinKeys;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory).HasName("PK__Categori__6DB3A68AC3D3BD13");

            entity.HasIndex(e => e.CategoryName, "UQ__Categori__8517B2E0E955CD13").IsUnique();

            entity.Property(e => e.IdCategory).HasColumnName("ID_Category");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("PK__Clients__B5AE4EC89BB8B3BD");

            entity.HasIndex(e => e.ClientLogin, "UQ__Clients__5BF1A3113CE3B178").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "UQ__Clients__85FB4E385FF3C163").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Clients__A9D1053444A7A429").IsUnique();

            entity.Property(e => e.IdClient).HasColumnName("ID_Client");
            entity.Property(e => e.ClientAddressId).HasColumnName("ClientAddress_ID");
            entity.Property(e => e.ClientLogin)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ClientMiddleName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ClientPassword)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ClientSurname)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(12)
                .IsUnicode(false);

            entity.HasOne(d => d.ClientAddress).WithMany(p => p.Clients)
                .HasForeignKey(d => d.ClientAddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Clients__ClientA__4F7CD00D");
        });

        modelBuilder.Entity<ClientsAddress>(entity =>
        {
            entity.HasKey(e => e.IdClientAddress).HasName("PK__ClientsA__FA731670938DA0FD");

            entity.HasIndex(e => e.AddressLocation, "UQ__ClientsA__853D68BA8BCB0CEA").IsUnique();

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
            entity.HasKey(e => e.IdOrder).HasName("PK__Orders__EC9FA955F7571AE8");

            entity.HasIndex(e => e.OrderNumber, "UQ__Orders__CAC5E7438465BFEA").IsUnique();

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
                .HasConstraintName("FK__Orders__Client_I__5EBF139D");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__Status_I__5DCAEF64");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("PK__OrderSta__5AC2A7345A23A772");

            entity.ToTable("OrderStatus");

            entity.HasIndex(e => e.StatusName, "UQ__OrderSta__05E7698A03FB198C").IsUnique();

            entity.Property(e => e.IdStatus).HasColumnName("ID_Status");
            entity.Property(e => e.StatusName)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PositionOrder>(entity =>
        {
            entity.HasKey(e => e.IdPositionOrder).HasName("PK__Position__969ABB7CF572D1A3");

            entity.Property(e => e.IdPositionOrder).HasColumnName("ID_PositionOrder");
            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.PositionOrderPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductId).HasColumnName("Product_ID");

            entity.HasOne(d => d.Order).WithMany(p => p.PositionOrders)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PositionO__Order__619B8048");

            entity.HasOne(d => d.Product).WithMany(p => p.PositionOrders)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PositionO__Produ__628FA481");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("PK__Products__522DE496C9D4ED60");

            entity.HasIndex(e => e.ProductImage, "UQ__Products__465B783C2D115645").IsUnique();

            entity.HasIndex(e => e.ProductName, "UQ__Products__DD5A978A80B33817").IsUnique();

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
                .HasConstraintName("FK__Products__Catego__571DF1D5");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
