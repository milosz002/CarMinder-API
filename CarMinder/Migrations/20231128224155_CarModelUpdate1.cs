using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarMinder.Migrations
{
    /// <inheritdoc />
    public partial class CarModelUpdate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Car",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "car_local_id",
                table: "Car",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "car_local_id",
                table: "Car");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Car",
                newName: "id");
        }
    }
}
