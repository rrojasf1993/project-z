using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandwritenRecognition.API.Migrations
{
    /// <inheritdoc />
    public partial class RelationshipsForRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExtractedFields_DocumentTypes_DocumentTypeId",
                table: "ExtractedFields");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldRules_DocumentTypes_DocumentTypeId",
                table: "FieldRules");

            migrationBuilder.DropForeignKey(
                name: "FK_OcrBoundingBox_Lines_SourceLineId",
                table: "OcrBoundingBox");

            migrationBuilder.DropIndex(
                name: "IX_ExtractedFields_DocumentTypeId",
                table: "ExtractedFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OcrBoundingBox",
                table: "OcrBoundingBox");

            migrationBuilder.DropColumn(
                name: "DocumentTypeId",
                table: "ExtractedFields");

            migrationBuilder.RenameTable(
                name: "OcrBoundingBox",
                newName: "OcrBoundingBoxes");

            migrationBuilder.RenameColumn(
                name: "DocumentTypeId",
                table: "FieldRules",
                newName: "AssociatedDocumentTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_FieldRules_DocumentTypeId",
                table: "FieldRules",
                newName: "IX_FieldRules_AssociatedDocumentTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_OcrBoundingBox_SourceLineId",
                table: "OcrBoundingBoxes",
                newName: "IX_OcrBoundingBoxes_SourceLineId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Jobs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "DocumentTypes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "DocumentTypes",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "AssociatedTypeId",
                table: "Documents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OcrBoundingBoxes",
                table: "OcrBoundingBoxes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_AssociatedTypeId",
                table: "Documents",
                column: "AssociatedTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_DocumentTypes_AssociatedTypeId",
                table: "Documents",
                column: "AssociatedTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldRules_DocumentTypes_AssociatedDocumentTypeId",
                table: "FieldRules",
                column: "AssociatedDocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OcrBoundingBoxes_Lines_SourceLineId",
                table: "OcrBoundingBoxes",
                column: "SourceLineId",
                principalTable: "Lines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_DocumentTypes_AssociatedTypeId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldRules_DocumentTypes_AssociatedDocumentTypeId",
                table: "FieldRules");

            migrationBuilder.DropForeignKey(
                name: "FK_OcrBoundingBoxes_Lines_SourceLineId",
                table: "OcrBoundingBoxes");

            migrationBuilder.DropIndex(
                name: "IX_Documents_AssociatedTypeId",
                table: "Documents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OcrBoundingBoxes",
                table: "OcrBoundingBoxes");

            migrationBuilder.DropColumn(
                name: "AssociatedTypeId",
                table: "Documents");

            migrationBuilder.RenameTable(
                name: "OcrBoundingBoxes",
                newName: "OcrBoundingBox");

            migrationBuilder.RenameColumn(
                name: "AssociatedDocumentTypeId",
                table: "FieldRules",
                newName: "DocumentTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_FieldRules_AssociatedDocumentTypeId",
                table: "FieldRules",
                newName: "IX_FieldRules_DocumentTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_OcrBoundingBoxes_SourceLineId",
                table: "OcrBoundingBox",
                newName: "IX_OcrBoundingBox_SourceLineId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Jobs",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentTypeId",
                table: "ExtractedFields",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "DocumentTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "DocumentTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OcrBoundingBox",
                table: "OcrBoundingBox",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ExtractedFields_DocumentTypeId",
                table: "ExtractedFields",
                column: "DocumentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExtractedFields_DocumentTypes_DocumentTypeId",
                table: "ExtractedFields",
                column: "DocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldRules_DocumentTypes_DocumentTypeId",
                table: "FieldRules",
                column: "DocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OcrBoundingBox_Lines_SourceLineId",
                table: "OcrBoundingBox",
                column: "SourceLineId",
                principalTable: "Lines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
