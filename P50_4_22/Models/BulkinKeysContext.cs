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

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=JULIANLOKI\\SQLEXPRESS;Initial Catalog=BulkinKeys;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory).HasName("PK__Categori__6DB3A68A24499FE7");

            entity.HasIndex(e => e.CategoryName, "UQ__Categori__8517B2E04E484707").IsUnique();

            entity.Property(e => e.IdCategory).HasColumnName("ID_Category");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("PK__Clients__B5AE4EC8D946E0C7");

            entity.HasIndex(e => e.ClientLogin, "UQ__Clients__5BF1A311190C428D").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "UQ__Clients__85FB4E3860760A52").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Clients__A9D1053493111182").IsUnique();

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

            entity.HasOne(d => d.ClientAddress).WithMany(p => p.Clients)
                .HasForeignKey(d => d.ClientAddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Clients__ClientA__403A8C7D");
        });

        modelBuilder.Entity<ClientsAddress>(entity =>
        {
            entity.HasKey(e => e.IdClientAddress).HasName("PK__ClientsA__FA731670F743274F");

            entity.HasIndex(e => e.AddressLocation, "UQ__ClientsA__853D68BA82B02EED").IsUnique();

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
            entity.HasKey(e => e.IdOrder).HasName("PK__Orders__EC9FA95572304ED7");

            entity.HasIndex(e => e.OrderNumber, "UQ__Orders__CAC5E743E1B9C2B6").IsUnique();

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
                .HasConstraintName("FK__Orders__Client_I__4F7CD00D");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__Status_I__4E88ABD4");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("PK__OrderSta__5AC2A734285A6F53");

            entity.ToTable("OrderStatus");

            entity.HasIndex(e => e.StatusName, "UQ__OrderSta__05E7698A719CCF08").IsUnique();

            entity.Property(e => e.IdStatus).HasColumnName("ID_Status");
            entity.Property(e => e.StatusName)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PositionOrder>(entity =>
        {
            entity.HasKey(e => e.IdPositionOrder).HasName("PK__Position__969ABB7CA855C32D");

            entity.Property(e => e.IdPositionOrder).HasColumnName("ID_PositionOrder");
            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.PositionOrderPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductId).HasColumnName("Product_ID");

            entity.HasOne(d => d.Order).WithMany(p => p.PositionOrders)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PositionO__Order__52593CB8");

            entity.HasOne(d => d.Product).WithMany(p => p.PositionOrders)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PositionO__Produ__534D60F1");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("PK__Products__522DE49681C8CB41");

            entity.HasIndex(e => e.ProductImage, "UQ__Products__465B783C0274BFBC").IsUnique();

            entity.HasIndex(e => e.ProductName, "UQ__Products__DD5A978A3709E1DA").IsUnique();

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
                .HasConstraintName("FK__Products__Catego__47DBAE45");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PK__Roles__43DCD32D195E6516");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B616016B38791").IsUnique();

            entity.Property(e => e.IdRole).HasColumnName("ID_Role");
            entity.Property(e => e.RoleName)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
