using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MunicipiosIbge.Api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMunicipalitiesSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Acronym = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Acronym = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RegionId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                    table.ForeignKey(
                        name: "FK_States_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IntermediateRegions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntermediateRegions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntermediateRegions_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mesorregions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesorregions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mesorregions_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImmediateRegions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    IntermediateRegionId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImmediateRegions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImmediateRegions_IntermediateRegions_IntermediateRegionId",
                        column: x => x.IntermediateRegionId,
                        principalTable: "IntermediateRegions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Microregions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    MesorregionId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Microregions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Microregions_Mesorregions_MesorregionId",
                        column: x => x.MesorregionId,
                        principalTable: "Mesorregions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Municipalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    MicroregionId = table.Column<int>(type: "integer", nullable: false),
                    ImmediateRegionId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipalities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Municipalities_ImmediateRegions_ImmediateRegionId",
                        column: x => x.ImmediateRegionId,
                        principalTable: "ImmediateRegions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Municipalities_Microregions_MicroregionId",
                        column: x => x.MicroregionId,
                        principalTable: "Microregions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImmediateRegions_IntermediateRegionId",
                table: "ImmediateRegions",
                column: "IntermediateRegionId");

            migrationBuilder.CreateIndex(
                name: "IX_IntermediateRegions_StateId",
                table: "IntermediateRegions",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Mesorregions_StateId",
                table: "Mesorregions",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Microregions_MesorregionId",
                table: "Microregions",
                column: "MesorregionId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipalities_ImmediateRegionId",
                table: "Municipalities",
                column: "ImmediateRegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipalities_MicroregionId",
                table: "Municipalities",
                column: "MicroregionId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipalities_Name",
                table: "Municipalities",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Acronym",
                table: "Regions",
                column: "Acronym",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_States_Acronym",
                table: "States",
                column: "Acronym",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_States_RegionId",
                table: "States",
                column: "RegionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Municipalities");

            migrationBuilder.DropTable(
                name: "ImmediateRegions");

            migrationBuilder.DropTable(
                name: "Microregions");

            migrationBuilder.DropTable(
                name: "IntermediateRegions");

            migrationBuilder.DropTable(
                name: "Mesorregions");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
