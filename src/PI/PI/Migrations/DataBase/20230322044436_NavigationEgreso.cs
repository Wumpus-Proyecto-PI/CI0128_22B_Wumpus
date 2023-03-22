using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PI.Migrations.DataBase
{
    public partial class NavigationEgreso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EGRESO_FECHA_DELETE",
                table: "EGRESO");

            migrationBuilder.DropIndex(
                name: "IX_EGRESO_fechaAnalisis",
                table: "EGRESO");

            migrationBuilder.AddColumn<DateTime>(
                name: "AnalisisFechaCreacion",
                table: "EGRESO",
                type: "datetime",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EGRESO_AnalisisFechaCreacion",
                table: "EGRESO",
                column: "AnalisisFechaCreacion");

            migrationBuilder.AddForeignKey(
                name: "FK_EGRESO_ANALISIS_AnalisisFechaCreacion",
                table: "EGRESO",
                column: "AnalisisFechaCreacion",
                principalTable: "ANALISIS",
                principalColumn: "fechaCreacion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EGRESO_ANALISIS_AnalisisFechaCreacion",
                table: "EGRESO");

            migrationBuilder.DropIndex(
                name: "IX_EGRESO_AnalisisFechaCreacion",
                table: "EGRESO");

            migrationBuilder.DropColumn(
                name: "AnalisisFechaCreacion",
                table: "EGRESO");

            migrationBuilder.CreateIndex(
                name: "IX_EGRESO_fechaAnalisis",
                table: "EGRESO",
                column: "fechaAnalisis");

            migrationBuilder.AddForeignKey(
                name: "FK_EGRESO_FECHA_DELETE",
                table: "EGRESO",
                column: "fechaAnalisis",
                principalTable: "ANALISIS",
                principalColumn: "fechaCreacion",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
