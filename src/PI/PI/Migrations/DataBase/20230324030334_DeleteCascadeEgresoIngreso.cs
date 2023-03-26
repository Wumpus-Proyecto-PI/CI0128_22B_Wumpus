using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PI.Migrations.DataBase
{
    public partial class DeleteCascadeEgresoIngreso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__EGRESO__1F63A897",
                table: "EGRESO");

            migrationBuilder.DropForeignKey(
                name: "FK__INGRESO__2610A626",
                table: "INGRESO");

            migrationBuilder.AddForeignKey(
                name: "FK__EGRESO__1F63A897",
                table: "EGRESO",
                columns: new[] { "mes", "fechaAnalisis" },
                principalTable: "MES",
                principalColumns: new[] { "nombre", "fechaAnalisis" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__INGRESO__2610A626",
                table: "INGRESO",
                columns: new[] { "mes", "fechaAnalisis" },
                principalTable: "MES",
                principalColumns: new[] { "nombre", "fechaAnalisis" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__EGRESO__1F63A897",
                table: "EGRESO");

            migrationBuilder.DropForeignKey(
                name: "FK__INGRESO__2610A626",
                table: "INGRESO");

            migrationBuilder.AddForeignKey(
                name: "FK__EGRESO__1F63A897",
                table: "EGRESO",
                columns: new[] { "mes", "fechaAnalisis" },
                principalTable: "MES",
                principalColumns: new[] { "nombre", "fechaAnalisis" });

            migrationBuilder.AddForeignKey(
                name: "FK__INGRESO__2610A626",
                table: "INGRESO",
                columns: new[] { "mes", "fechaAnalisis" },
                principalTable: "MES",
                principalColumns: new[] { "nombre", "fechaAnalisis" });
        }
    }
}
