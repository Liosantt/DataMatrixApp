using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Scan.Shared;
#nullable disable

namespace Scan.Server.Model
{
    public partial class Connectors1Context : DbContext
    {
        public Connectors1Context()
        {
        }

        public Connectors1Context(DbContextOptions<Connectors1Context> options)
            : base(options)
        {
        }

        public virtual DbSet<CooisComponent> CooisComponents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<CooisComponent>(entity =>
            {
                entity.HasKey(e => e.ComponentId)
                    .HasName("PK_ID");

                entity.ToTable("CooisComponents", "sap");

                entity.HasIndex(e => e.Material, "IX_CooisComponents_Material");

                entity.HasIndex(e => new { e.Material, e.RequirementQty }, "cooiscomponents_idx_material_requirementqty");

                entity.HasIndex(e => e.ProdOrder, "cooiscomponents_idx_prodorder_material_requirementqty");

                entity.HasIndex(e => e.RequirementQty, "cooiscomponents_idx_requirementqty");

                entity.Property(e => e.ComponentId)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("ComponentID");

                entity.Property(e => e.BaseUoM)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Batch)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Material)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.QtyWithdrawn).HasColumnType("decimal(9, 3)");

                entity.Property(e => e.RequirementQty).HasColumnType("decimal(9, 3)");

                entity.Property(e => e.StatusId)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("StatusID");

                entity.Property(e => e.StorageLocation)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
