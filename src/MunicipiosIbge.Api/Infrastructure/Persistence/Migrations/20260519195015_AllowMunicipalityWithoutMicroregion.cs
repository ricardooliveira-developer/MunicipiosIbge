using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MunicipiosIbge.Api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AllowMunicipalityWithoutMicroregion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MicroregionId",
                table: "Municipalities",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MicroregionId",
                table: "Municipalities",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
