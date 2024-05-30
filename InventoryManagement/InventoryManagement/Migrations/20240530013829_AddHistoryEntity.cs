using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddHistoryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_History",
                table: "History");

            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "History");

            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "History");

            migrationBuilder.DropColumn(
                name: "Warehouse",
                table: "History");

            migrationBuilder.RenameTable(
                name: "History",
                newName: "Histories");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Histories",
                newName: "ID");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "Histories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Histories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SupplierName",
                table: "Histories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WarehouseName",
                table: "Histories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Histories",
                table: "Histories",
                column: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Histories",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "SupplierName",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "WarehouseName",
                table: "Histories");

            migrationBuilder.RenameTable(
                name: "Histories",
                newName: "History");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "History",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "History",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "History",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransactionType",
                table: "History",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Warehouse",
                table: "History",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_History",
                table: "History",
                column: "Id");
        }
    }
}
