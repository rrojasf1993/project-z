using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandwritenRecognition.API.Migrations
{
    /// <inheritdoc />
    public partial class FieldTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentType",
                table: "FieldRules");

            migrationBuilder.DropColumn(
                name: "FieldType",
                table: "FieldRules");

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentTypeId",
                table: "FieldRules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FieldTypeId",
                table: "FieldRules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "RuleId",
                table: "ExtractedFields",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentTypeId",
                table: "ExtractedFields",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ExtractedFields",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Documents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Documents",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FieldType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FieldRules_DocumentTypeId",
                table: "FieldRules",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldRules_FieldTypeId",
                table: "FieldRules",
                column: "FieldTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtractedFields_DocumentTypeId",
                table: "ExtractedFields",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtractedFields_RuleId",
                table: "ExtractedFields",
                column: "RuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExtractedFields_DocumentTypes_DocumentTypeId",
                table: "ExtractedFields",
                column: "DocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExtractedFields_FieldRules_RuleId",
                table: "ExtractedFields",
                column: "RuleId",
                principalTable: "FieldRules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldRules_DocumentTypes_DocumentTypeId",
                table: "FieldRules",
                column: "DocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldRules_FieldType_FieldTypeId",
                table: "FieldRules",
                column: "FieldTypeId",
                principalTable: "FieldType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExtractedFields_DocumentTypes_DocumentTypeId",
                table: "ExtractedFields");

            migrationBuilder.DropForeignKey(
                name: "FK_ExtractedFields_FieldRules_RuleId",
                table: "ExtractedFields");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldRules_DocumentTypes_DocumentTypeId",
                table: "FieldRules");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldRules_FieldType_FieldTypeId",
                table: "FieldRules");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "FieldType");

            migrationBuilder.DropIndex(
                name: "IX_FieldRules_DocumentTypeId",
                table: "FieldRules");

            migrationBuilder.DropIndex(
                name: "IX_FieldRules_FieldTypeId",
                table: "FieldRules");

            migrationBuilder.DropIndex(
                name: "IX_ExtractedFields_DocumentTypeId",
                table: "ExtractedFields");

            migrationBuilder.DropIndex(
                name: "IX_ExtractedFields_RuleId",
                table: "ExtractedFields");

            migrationBuilder.DropColumn(
                name: "DocumentTypeId",
                table: "FieldRules");

            migrationBuilder.DropColumn(
                name: "FieldTypeId",
                table: "FieldRules");

            migrationBuilder.DropColumn(
                name: "DocumentTypeId",
                table: "ExtractedFields");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ExtractedFields");

            migrationBuilder.AddColumn<string>(
                name: "DocumentType",
                table: "FieldRules",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FieldType",
                table: "FieldRules",
                type: "int",
                maxLength: 300,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "RuleId",
                table: "ExtractedFields",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Documents",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Documents",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
