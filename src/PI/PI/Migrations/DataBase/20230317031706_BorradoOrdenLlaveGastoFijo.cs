using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PI.Migrations.DataBase
{
    public partial class BorradoOrdenLlaveGastoFijo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__GASTO_FI__C10BC2E58A2E7E29",
                table: "GASTO_FIJO");

            migrationBuilder.AddPrimaryKey(
                name: "PK__GASTO_FI__C10BC2E58A2E7E29",
                table: "GASTO_FIJO",
                columns: new[] { "nombre", "fechaAnalisis" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__GASTO_FI__C10BC2E58A2E7E29",
                table: "GASTO_FIJO");

            migrationBuilder.AddPrimaryKey(
                name: "PK__GASTO_FI__C10BC2E58A2E7E29",
                table: "GASTO_FIJO",
                columns: new[] { "orden", "nombre", "fechaAnalisis" });
        }
    }
}
