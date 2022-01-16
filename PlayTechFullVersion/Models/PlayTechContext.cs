using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WinFormsAppModel.Models;

#nullable disable

namespace Task.Models
{
    public partial class PlayTechContext : DbContext
    {
        public PlayTechContext()
        {
        }

        public PlayTechContext(DbContextOptions<PlayTechContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CreditMonth> CreditMonths { get; set; }
        public virtual DbSet<Dept> Depts { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<Firm> Firms { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<SaleType> SaleTypes { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=SQL5103.site4now.net;Database=db_a4f2ac_playtechdb;User Id= db_a4f2ac_playtechdb_admin; Password=PlayTech_2021;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Fullname).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(200);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CategoryName).HasMaxLength(100);
            });

            modelBuilder.Entity<CreditMonth>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Month).HasMaxLength(50);
            });

            modelBuilder.Entity<Dept>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Fullname).HasMaxLength(50);

                entity.Property(e => e.TakeDate).HasColumnType("datetime");

                entity.Property(e => e.TakeMoney).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TakeWhy).HasMaxLength(500);
            });

            modelBuilder.Entity<Expense>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AllMoney).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CalcDate).HasColumnType("datetime");

                entity.Property(e => e.Communal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FloorPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Fullname).HasMaxLength(50);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.WorkerSalary).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Firm>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FirmName).HasMaxLength(100);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.OrderCode)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.SaleDate).HasColumnType("datetime");

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreditMonthId).HasColumnName("CreditMonthID");

                entity.Property(e => e.DailySaleDate).HasColumnType("datetime");

                entity.Property(e => e.ItemPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.SaleTypeId).HasColumnName("SaleTypeID");

                entity.HasOne(d => d.CreditMonth)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.CreditMonthId)
                    .HasConstraintName("FK_OrderItems_CreditMonths");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderItems_Orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_OrderItems_Products1");

                entity.HasOne(d => d.SaleType)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.SaleTypeId)
                    .HasConstraintName("FK_OrderItems_SaleTypes");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BarCode).HasMaxLength(50);

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.FirmId).HasColumnName("FirmID");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductName).HasMaxLength(200);

                entity.Property(e => e.PublishDate).HasColumnType("datetime");

                entity.Property(e => e.SalePrice).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Products_Categories");

                entity.HasOne(d => d.Firm)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.FirmId)
                    .HasConstraintName("FK_Products_Firms");
            });

            modelBuilder.Entity<SaleType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Type).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Fullname).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(200);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.RePassword).HasMaxLength(200);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
