using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandwritenRecognition.API.Migrations
{
    /// <inheritdoc />
    public partial class SupportTablesForRuleEngine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Scope",
                table: "FieldRules");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "FieldRules");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "FieldType",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "ValidationPattern",
                table: "FieldRules",
                type: "nvarchar(1500)",
                maxLength: 1500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1500)",
                oldMaxLength: 1500);

            migrationBuilder.AlterColumn<float>(
                name: "MinConfidence",
                table: "FieldRules",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<string>(
                name: "DetectionPattern",
                table: "FieldRules",
                type: "nvarchar(1500)",
                maxLength: 1500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1500)",
                oldMaxLength: 1500);

            migrationBuilder.AlterColumn<float>(
                name: "ConfidenceWeight",
                table: "FieldRules",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<Guid>(
                name: "ScopeId",
                table: "FieldRules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TypeId",
                table: "FieldRules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "RuleScope",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleScope", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RuleType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Kind = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FieldRules_ScopeId",
                table: "FieldRules",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldRules_TypeId",
                table: "FieldRules",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldRules_RuleScope_ScopeId",
                table: "FieldRules",
                column: "ScopeId",
                principalTable: "RuleScope",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldRules_RuleType_TypeId",
                table: "FieldRules",
                column: "TypeId",
                principalTable: "RuleType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldRules_RuleScope_ScopeId",
                table: "FieldRules");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldRules_RuleType_TypeId",
                table: "FieldRules");

            migrationBuilder.DropTable(
                name: "RuleScope");

            migrationBuilder.DropTable(
                name: "RuleType");

            migrationBuilder.DropIndex(
                name: "IX_FieldRules_ScopeId",
                table: "FieldRules");

            migrationBuilder.DropIndex(
                name: "IX_FieldRules_TypeId",
                table: "FieldRules");

            migrationBuilder.DropColumn(
                name: "ScopeId",
                table: "FieldRules");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "FieldRules");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "FieldType",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "ValidationPattern",
                table: "FieldRules",
                type: "nvarchar(1500)",
                maxLength: 1500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1500)",
                oldMaxLength: 1500,
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "MinConfidence",
                table: "FieldRules",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DetectionPattern",
                table: "FieldRules",
                type: "nvarchar(1500)",
                maxLength: 1500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1500)",
                oldMaxLength: 1500,
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "ConfidenceWeight",
                table: "FieldRules",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Scope",
                table: "FieldRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "FieldRules",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
