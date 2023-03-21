using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PI.Migrations.DataBase
{
    public partial class MontoNoKeyEgresoIngreso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingreso",
                table: "INGRESO");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Egreso",
                table: "EGRESO");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingreso",
                table: "INGRESO",
                columns: new[] { "mes", "fechaAnalisis", "tipo" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Egreso",
                table: "EGRESO",
                columns: new[] { "mes", "fechaAnalisis", "tipo" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingreso",
                table: "INGRESO");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Egreso",
                table: "EGRESO");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingreso",
                table: "INGRESO",
                columns: new[] { "mes", "fechaAnalisis", "tipo", "monto" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Egreso",
                table: "EGRESO",
                columns: new[] { "mes", "fechaAnalisis", "tipo", "monto" });
        }
    }
}
