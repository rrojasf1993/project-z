using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandwritenRecognition.API.Migrations
{
    /// <inheritdoc />
    public partial class FieldTypesNavPropByDocumentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SrcDocumentTypeId",
                table: "FieldType",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FieldType_SrcDocumentTypeId",
                table: "FieldType",
                column: "SrcDocumentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldType_DocumentTypes_SrcDocumentTypeId",
                table: "FieldType",
                column: "SrcDocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldType_DocumentTypes_SrcDocumentTypeId",
                table: "FieldType");

            migrationBuilder.DropIndex(
                name: "IX_FieldType_SrcDocumentTypeId",
                table: "FieldType");

            migrationBuilder.DropColumn(
                name: "SrcDocumentTypeId",
                table: "FieldType");
        }
    }
}
