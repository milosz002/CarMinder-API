using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarMinder.Migrations
{
    /// <inheritdoc />
    public partial class CarModelUpdate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "vehicle_technical_inspection_deadline_notification_id",
                table: "Car",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "vehicle_technical_inspection_deadline_notification_id",
                table: "Car",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
