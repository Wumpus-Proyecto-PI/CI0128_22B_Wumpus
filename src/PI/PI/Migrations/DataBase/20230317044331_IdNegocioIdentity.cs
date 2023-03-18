﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PI.Migrations.DataBase
{
    public partial class IdNegocioIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateIndex(
                name: "IX_ANALISIS_idNegocio",
                table: "ANALISIS",
                column: "idNegocio");

            migrationBuilder.AddForeignKey(
                name: "FK__ANALISIS__idNegocio",
                table: "ANALISIS",
                column: "idNegocio",
                principalTable: "NEGOCIO",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__ANALISIS__idNegocio",
                table: "ANALISIS");

            migrationBuilder.DropIndex(
                name: "IX_ANALISIS_idNegocio",
                table: "ANALISIS");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "NEGOCIO",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "IdNegocioNavigationId",
                table: "ANALISIS",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ANALISIS_IdNegocioNavigationId",
                table: "ANALISIS",
                column: "IdNegocioNavigationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ANALISIS_NEGOCIO_IdNegocioNavigationId",
                table: "ANALISIS",
                column: "IdNegocioNavigationId",
                principalTable: "NEGOCIO",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}