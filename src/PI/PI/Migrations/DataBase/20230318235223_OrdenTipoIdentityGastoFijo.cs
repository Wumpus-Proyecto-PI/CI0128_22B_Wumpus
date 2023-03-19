using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PI.Migrations.DataBase
{
    public partial class OrdenTipoIdentityGastoFijo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "Orden",
                table: "GASTO_FIJO");

            migrationBuilder.AddColumn<int>(
                name: "Orden",
                table: "GASTO_FIJO",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Orden",
                table: "GASTO_FIJO");

            migrationBuilder.AddColumn<int>(
                name: "Orden",
                table: "GASTO_FIJO",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");
        }
    }
}
