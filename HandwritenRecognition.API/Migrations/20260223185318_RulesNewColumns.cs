using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandwritenRecognition.API.Migrations
{
    /// <inheritdoc />
    public partial class RulesNewColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RegexPattern",
                table: "FieldRules",
                newName: "ValidationPattern");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "FieldRules",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "DetectionPattern",
                table: "FieldRules",
                type: "nvarchar(1500)",
                maxLength: 1500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FieldName",
                table: "FieldRules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "MinConfidence",
                table: "FieldRules",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "Scope",
                table: "FieldRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "UseNextLine",
                table: "FieldRules",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DetectionPattern",
                table: "FieldRules");

            migrationBuilder.DropColumn(
                name: "FieldName",
                table: "FieldRules");

            migrationBuilder.DropColumn(
                name: "MinConfidence",
                table: "FieldRules");

            migrationBuilder.DropColumn(
                name: "Scope",
                table: "FieldRules");

            migrationBuilder.DropColumn(
                name: "UseNextLine",
                table: "FieldRules");

            migrationBuilder.RenameColumn(
                name: "ValidationPattern",
                table: "FieldRules",
                newName: "RegexPattern");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "FieldRules",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
