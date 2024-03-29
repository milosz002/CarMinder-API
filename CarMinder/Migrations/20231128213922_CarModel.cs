using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarMinder.Migrations
{
    /// <inheritdoc />
    public partial class CarModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    model_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    model_year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    model_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    make_display = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    model_body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    model_engine_position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    model_engine_cc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    model_engine_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    model_engine_power_ps = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    model_top_speed_kph = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    model_drive = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    model_transmission_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    model_seats = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    model_weight_kg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    make_country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    model_lkm_hwy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    model_lkm_mixed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    model_lkm_city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    model_fuel_cap_l = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    car_image_url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vehicle_technical_inspection_deadline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vehicle_technical_inspection_deadline_notification_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car", x => x.id);
                    table.ForeignKey(
                        name: "FK_Car_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Car_UserId",
                table: "Car",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Car");
        }
    }
}
