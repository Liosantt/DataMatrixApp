using Microsoft.EntityFrameworkCore;

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
        public virtual DbSet<MaterialMaster> MaterialMasters { get; set; }
        public virtual DbSet<ProductClassification> ProductClassifications { get; set; }
        public virtual DbSet<SerialNo> SerialNos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=gbultlm223asrv.ad001.siemens.net;Database=Connectors1;User ID=ConnectorsDev; Password=Ygx$IXFsrcu6cQ$bd8*$; MultipleActiveResultSets=True;");
            }
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

            modelBuilder.Entity<MaterialMaster>(entity =>
            {
                entity.HasKey(e => e.Material)
                    .HasName("PK_Material_MaterialMaster");

                entity.ToTable("MaterialMaster", "sap");

                entity.Property(e => e.Material)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CostControl)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Mrpcontroller)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("MRPController");

                entity.Property(e => e.ProdnSuperv)
                    .HasMaxLength(3)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProductClassification>(entity =>
            {
                entity.HasKey(e => e.ClassMaterial)
                    .HasName("PK_Material");

                entity.ToTable("ProductClassification", "plm");

                entity.HasIndex(e => new { e.ClassRangeId, e.ClassMaterial }, "productclassificat_idx_classrangeid_classmaterial");

                entity.HasIndex(e => new { e.ClassTypeId, e.ClassMaterial }, "productclassificat_idx_classtypeid_classmaterial");

                entity.Property(e => e.ClassMaterial)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.ClassApplicableStandard)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.ClassDatasheetRef)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.ClassIsCemarked).HasColumnName("ClassIsCEMarked");

                entity.Property(e => e.ClassMawp).HasColumnName("ClassMAWP");

                entity.Property(e => e.ClassModifiedBy)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.ClassPsl)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ClassPSL");

                entity.Property(e => e.ClassRangeId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ClassRangeID");

                entity.Property(e => e.ClassRatedVoltage)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ClassStatusId)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ClassStatusID");

                entity.Property(e => e.ClassTemplateId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ClassTemplateID");

                entity.Property(e => e.ClassTimestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.ClassTypeId)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("ClassTypeID");

                entity.Property(e => e.ClassValidatedBy)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.ClassValidationDate).HasColumnType("date");
            });

            modelBuilder.Entity<SerialNo>(entity =>
            {
                entity.HasKey(e => new { e.ProdOrder, e.SerialNo1 })
                    .HasName("PK_ProdOrderSerialNo");

                entity.ToTable("SerialNo", "sap");

                entity.Property(e => e.SerialNo1)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("SerialNo");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
