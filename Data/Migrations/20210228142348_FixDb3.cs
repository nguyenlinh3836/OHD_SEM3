using Microsoft.EntityFrameworkCore.Migrations;

namespace OHD_SEM3.Data.Migrations
{
    public partial class FixDb3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Facilities_FacilityId",
                table: "Requests");

            migrationBuilder.AlterColumn<int>(
                name: "FacilityId",
                table: "Requests",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Facilities_FacilityId",
                table: "Requests",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "FacilityId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Facilities_FacilityId",
                table: "Requests");

            migrationBuilder.AlterColumn<int>(
                name: "FacilityId",
                table: "Requests",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Facilities_FacilityId",
                table: "Requests",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "FacilityId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
