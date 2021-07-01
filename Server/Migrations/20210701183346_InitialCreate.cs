using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Scan.Server.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sap");

            migrationBuilder.EnsureSchema(
                name: "plm");

            migrationBuilder.CreateTable(
                name: "CooisComponents",
                schema: "sap",
                columns: table => new
                {
                    ComponentID = table.Column<string>(type: "TEXT", unicode: false, maxLength: 25, nullable: false),
                    Material = table.Column<string>(type: "TEXT", unicode: false, maxLength: 20, nullable: true),
                    RequirementQty = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    QtyWithdrawn = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    BaseUoM = table.Column<string>(type: "TEXT", unicode: false, maxLength: 3, nullable: false),
                    ProdOrder = table.Column<long>(type: "INTEGER", nullable: false),
                    RequirementNo = table.Column<int>(type: "INTEGER", nullable: false),
                    Batch = table.Column<string>(type: "TEXT", unicode: false, maxLength: 10, nullable: true),
                    StorageLocation = table.Column<string>(type: "TEXT", unicode: false, maxLength: 4, nullable: false),
                    StatusID = table.Column<string>(type: "TEXT", unicode: false, maxLength: 4, nullable: false),
                    PhantomItem = table.Column<bool>(type: "INTEGER", nullable: true),
                    BulkMaterial = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ID", x => x.ComponentID);
                });

            migrationBuilder.CreateTable(
                name: "MaterialMaster",
                schema: "sap",
                columns: table => new
                {
                    Material = table.Column<string>(type: "TEXT", unicode: false, maxLength: 25, nullable: false),
                    Description = table.Column<string>(type: "TEXT", unicode: false, maxLength: 40, nullable: false),
                    CostControl = table.Column<string>(type: "TEXT", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    MRPController = table.Column<string>(type: "TEXT", unicode: false, maxLength: 3, nullable: true),
                    ProdnSuperv = table.Column<string>(type: "TEXT", unicode: false, maxLength: 3, nullable: true),
                    Configurable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material_MaterialMaster", x => x.Material);
                });

            migrationBuilder.CreateTable(
                name: "ProductClassification",
                schema: "plm",
                columns: table => new
                {
                    ClassMaterial = table.Column<string>(type: "TEXT", unicode: false, maxLength: 25, nullable: false),
                    ClassRangeID = table.Column<string>(type: "TEXT", unicode: false, maxLength: 10, nullable: false),
                    ClassTypeID = table.Column<string>(type: "TEXT", unicode: false, maxLength: 4, nullable: false),
                    ClassStatusID = table.Column<string>(type: "TEXT", unicode: false, maxLength: 3, nullable: false),
                    ClassRatedVoltage = table.Column<string>(type: "TEXT", unicode: false, maxLength: 15, nullable: true),
                    ClassMinTemp = table.Column<short>(type: "INTEGER", nullable: true),
                    ClassMaxTemp = table.Column<short>(type: "INTEGER", nullable: true),
                    ClassMaxWaterDepth = table.Column<short>(type: "INTEGER", nullable: true),
                    ClassMAWP = table.Column<short>(type: "INTEGER", nullable: true),
                    ClassPSL = table.Column<string>(type: "TEXT", unicode: false, maxLength: 2, nullable: true),
                    ClassMass = table.Column<short>(type: "INTEGER", nullable: true),
                    ClassTemplateID = table.Column<string>(type: "TEXT", unicode: false, maxLength: 10, nullable: true),
                    ClassApplicableStandard = table.Column<string>(type: "TEXT", unicode: false, maxLength: 12, nullable: true),
                    ClassIsCEMarked = table.Column<bool>(type: "INTEGER", nullable: true),
                    ClassTimestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true),
                    ClassDatasheetRef = table.Column<string>(type: "TEXT", unicode: false, maxLength: 32, nullable: true),
                    ClassModifiedBy = table.Column<string>(type: "TEXT", unicode: false, maxLength: 8, nullable: true),
                    ClassValidatedBy = table.Column<string>(type: "TEXT", unicode: false, maxLength: 8, nullable: true),
                    ClassValidationDate = table.Column<DateTime>(type: "date", nullable: true),
                    ClassPartNoAliased = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.ClassMaterial);
                });

            migrationBuilder.CreateTable(
                name: "SerialNo",
                schema: "sap",
                columns: table => new
                {
                    ProdOrder = table.Column<long>(type: "INTEGER", nullable: false),
                    SerialNo = table.Column<string>(type: "TEXT", unicode: false, maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdOrderSerialNo", x => new { x.ProdOrder, x.SerialNo });
                });

            migrationBuilder.CreateIndex(
                name: "cooiscomponents_idx_material_requirementqty",
                schema: "sap",
                table: "CooisComponents",
                columns: new[] { "Material", "RequirementQty" });

            migrationBuilder.CreateIndex(
                name: "cooiscomponents_idx_prodorder_material_requirementqty",
                schema: "sap",
                table: "CooisComponents",
                column: "ProdOrder");

            migrationBuilder.CreateIndex(
                name: "cooiscomponents_idx_requirementqty",
                schema: "sap",
                table: "CooisComponents",
                column: "RequirementQty");

            migrationBuilder.CreateIndex(
                name: "IX_CooisComponents_Material",
                schema: "sap",
                table: "CooisComponents",
                column: "Material");

            migrationBuilder.CreateIndex(
                name: "productclassificat_idx_classrangeid_classmaterial",
                schema: "plm",
                table: "ProductClassification",
                columns: new[] { "ClassRangeID", "ClassMaterial" });

            migrationBuilder.CreateIndex(
                name: "productclassificat_idx_classtypeid_classmaterial",
                schema: "plm",
                table: "ProductClassification",
                columns: new[] { "ClassTypeID", "ClassMaterial" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CooisComponents",
                schema: "sap");

            migrationBuilder.DropTable(
                name: "MaterialMaster",
                schema: "sap");

            migrationBuilder.DropTable(
                name: "ProductClassification",
                schema: "plm");

            migrationBuilder.DropTable(
                name: "SerialNo",
                schema: "sap");
        }
    }
}
