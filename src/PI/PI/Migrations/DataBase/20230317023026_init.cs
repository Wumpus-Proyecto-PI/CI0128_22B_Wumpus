using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PI.Migrations.DataBase
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NEGOCIO",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime", nullable: true),
                    idUsuario = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NEGOCIO", x => x.id);
                    table.ForeignKey(
                        name: "FK_AspNetUser_CorreoUsuario",
                        column: x => x.idUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onUpdate: ReferentialAction.Cascade,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ANALISIS",
                columns: table => new
                {
                    fechaCreacion = table.Column<DateTime>(type: "datetime", nullable: false),
                    direccion = table.Column<string>(type: "varchar(512)", unicode: false, maxLength: 512, nullable: true),
                    fechaDescarga = table.Column<DateTime>(type: "datetime", nullable: true),
                    gananciaMensual = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValueSql: "((0))"),
                    idNegocio = table.Column<int>(type: "int", nullable: false),
                    estadoAnalisis = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ANALISIS__19DE4892862EBA95", x => x.fechaCreacion);
                    table.ForeignKey(
                        name: "FK__ANALISIS__idNegocio",
                        column: x => x.idNegocio,
                        principalTable: "NEGOCIO",
                        principalColumn: "id",
                        onUpdate: ReferentialAction.Cascade,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CONFIGURACION",
                columns: table => new
                {
                    fechaAnalisis = table.Column<DateTime>(type: "datetime", nullable: false),
                    tipoNegocio = table.Column<int>(type: "int", nullable: false),
                    porcentajeSS = table.Column<decimal>(type: "decimal(5,2)", nullable: true, defaultValueSql: "((0))"),
                    porcentajePL = table.Column<decimal>(type: "decimal(5,2)", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CONFIGUR__79D41A8629CB69EB", x => x.fechaAnalisis);
                    table.ForeignKey(
                        name: "FK_ANALISIS_DELETE",
                        column: x => x.fechaAnalisis,
                        principalTable: "ANALISIS",
                        principalColumn: "fechaCreacion",
                        onUpdate: ReferentialAction.Cascade,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GASTO_FIJO",
                columns: table => new
                {
                    nombre = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    fechaAnalisis = table.Column<DateTime>(type: "datetime", nullable: false),
                    orden = table.Column<int>(type: "int", nullable: false),
                    monto = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GASTO_FI__C10BC2E58A2E7E29", x => new { x.orden, x.nombre, x.fechaAnalisis });
                    table.ForeignKey(
                        name: "FK_GASTO_FECHA",
                        column: x => x.fechaAnalisis,
                        principalTable: "ANALISIS",
                        principalColumn: "fechaCreacion",
                        onUpdate: ReferentialAction.Cascade,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "INVERSION_INICIAL",
                columns: table => new
                {
                    fechaAnalisis = table.Column<DateTime>(type: "datetime", nullable: false),
                    nombre = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    valor = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__INVERSIO__1EFEE14A47BDAE37", x => new { x.fechaAnalisis, x.nombre });
                    table.ForeignKey(
                        name: "FK__INVERSION__FECHA",
                        column: x => x.fechaAnalisis,
                        principalTable: "ANALISIS",
                        principalColumn: "fechaCreacion",
                        onUpdate: ReferentialAction.Cascade,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MES",
                columns: table => new
                {
                    nombre = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    fechaAnalisis = table.Column<DateTime>(type: "datetime", nullable: false),
                    inversionPorMes = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValueSql: "((0.0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MES__0532FD6FC344EF0E", x => new { x.nombre, x.fechaAnalisis });
                    table.ForeignKey(
                        name: "FK_MES_FECHA",
                        column: x => x.fechaAnalisis,
                        principalTable: "ANALISIS",
                        principalColumn: "fechaCreacion",
                        onUpdate: ReferentialAction.Cascade,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCTO",
                columns: table => new
                {
                    nombre = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    fechaAnalisis = table.Column<DateTime>(type: "datetime", nullable: false),
                    lote = table.Column<int>(type: "int", nullable: true),
                    porcentajeDeVentas = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValueSql: "((0))"),
                    precio = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValueSql: "((0))"),
                    costoVariable = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    comisionDeVentas = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValueSql: "((0.0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PRODUCTO__0532FD6FC589957A", x => new { x.nombre, x.fechaAnalisis });
                    table.ForeignKey(
                        name: "FK__PRODUCTO__FECHA",
                        column: x => x.fechaAnalisis,
                        principalTable: "ANALISIS",
                        principalColumn: "fechaCreacion",
                        onUpdate: ReferentialAction.Cascade,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PUESTO",
                columns: table => new
                {
                    nombre = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    fechaAnalisis = table.Column<DateTime>(type: "datetime", nullable: false),
                    cantidadPlazas = table.Column<int>(type: "int", nullable: true),
                    salarioBruto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    orden = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    beneficios = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValueSql: "((0.0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PUESTO__0532FD6FB7898CB1", x => new { x.nombre, x.fechaAnalisis });
                    table.ForeignKey(
                        name: "FK__PUESTO__FECHA__UPDATE",
                        column: x => x.fechaAnalisis,
                        principalTable: "ANALISIS",
                        principalColumn: "fechaCreacion",
                        onUpdate: ReferentialAction.Cascade,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EGRESO",
                columns: table => new
                {
                    mes = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    fechaAnalisis = table.Column<DateTime>(type: "datetime", nullable: false),
                    tipo = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Egreso", x => new { x.mes, x.fechaAnalisis, x.tipo, x.monto });
                    table.ForeignKey(
                        name: "FK__EGRESO__1F63A897",
                        columns: x => new { x.mes, x.fechaAnalisis },
                        principalTable: "MES",
                        principalColumns: new[] { "nombre", "fechaAnalisis" });
                    table.ForeignKey(
                        name: "FK_EGRESO_FECHA_DELETE",
                        column: x => x.fechaAnalisis,
                        principalTable: "ANALISIS",
                        principalColumn: "fechaCreacion",
                        onUpdate: ReferentialAction.Cascade,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "INGRESO",
                columns: table => new
                {
                    mes = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    fechaAnalisis = table.Column<DateTime>(type: "datetime", nullable: false),
                    tipo = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingreso", x => new { x.mes, x.fechaAnalisis, x.tipo, x.monto });
                    table.ForeignKey(
                        name: "FK__INGRESO__2610A626",
                        columns: x => new { x.mes, x.fechaAnalisis },
                        principalTable: "MES",
                        principalColumns: new[] { "nombre", "fechaAnalisis" },
                        onUpdate: ReferentialAction.Cascade,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "COMPONENTE",
                columns: table => new
                {
                    nombreComponente = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    nombreProducto = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    fechaAnalisis = table.Column<DateTime>(type: "datetime", nullable: false),
                    monto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    cantidad = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    unidad = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__COMPONEN__DA38DD99DF589705", x => new { x.nombreComponente, x.nombreProducto, x.fechaAnalisis });
                    table.ForeignKey(
                        name: "FK_ComponenteProducto",
                        columns: x => new { x.nombreProducto, x.fechaAnalisis },
                        principalTable: "PRODUCTO",
                        principalColumns: new[] { "nombre", "fechaAnalisis" },
                        onUpdate: ReferentialAction.Cascade,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ES_EMPLEADO_DE",
                columns: table => new
                {
                    nombreEmpleado = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    fechaEmpleado = table.Column<DateTime>(type: "datetime", nullable: false),
                    nombreJefe = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    fechaJefe = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ES_EMPLE__25C3089BB9377AC2", x => new { x.nombreEmpleado, x.fechaEmpleado, x.nombreJefe, x.fechaJefe });
                    table.ForeignKey(
                        name: "FK_EMPLEADO_JEFE",
                        columns: x => new { x.nombreJefe, x.fechaJefe },
                        principalTable: "PUESTO",
                        principalColumns: new[] { "nombre", "fechaAnalisis" },
                        onUpdate: ReferentialAction.Cascade,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ANALISIS_idNegocio",
                table: "ANALISIS",
                column: "idNegocio");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "([NormalizedName] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "([NormalizedUserName] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "index_componente_cantidad_",
                table: "COMPONENTE",
                column: "cantidad");

            migrationBuilder.CreateIndex(
                name: "IX_COMPONENTE_nombreProducto_fechaAnalisis",
                table: "COMPONENTE",
                columns: new[] { "nombreProducto", "fechaAnalisis" });

            migrationBuilder.CreateIndex(
                name: "IX_EGRESO_fechaAnalisis",
                table: "EGRESO",
                column: "fechaAnalisis");

            migrationBuilder.CreateIndex(
                name: "IX_ES_EMPLEADO_DE_nombreJefe_fechaJefe",
                table: "ES_EMPLEADO_DE",
                columns: new[] { "nombreJefe", "fechaJefe" });

            migrationBuilder.CreateIndex(
                name: "IX_GASTO_FIJO_fechaAnalisis",
                table: "GASTO_FIJO",
                column: "fechaAnalisis");

            migrationBuilder.CreateIndex(
                name: "IX_MES_fechaAnalisis",
                table: "MES",
                column: "fechaAnalisis");

            migrationBuilder.CreateIndex(
                name: "id_negocio",
                table: "NEGOCIO",
                column: "FechaCreacion");

            migrationBuilder.CreateIndex(
                name: "Negocio_idUsuario",
                table: "NEGOCIO",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTO_fechaAnalisis",
                table: "PRODUCTO",
                column: "fechaAnalisis");

            migrationBuilder.CreateIndex(
                name: "IX_PUESTO_fechaAnalisis",
                table: "PUESTO",
                column: "fechaAnalisis");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "COMPONENTE");

            migrationBuilder.DropTable(
                name: "CONFIGURACION");

            migrationBuilder.DropTable(
                name: "EGRESO");

            migrationBuilder.DropTable(
                name: "ES_EMPLEADO_DE");

            migrationBuilder.DropTable(
                name: "GASTO_FIJO");

            migrationBuilder.DropTable(
                name: "INGRESO");

            migrationBuilder.DropTable(
                name: "INVERSION_INICIAL");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "PRODUCTO");

            migrationBuilder.DropTable(
                name: "PUESTO");

            migrationBuilder.DropTable(
                name: "MES");

            migrationBuilder.DropTable(
                name: "ANALISIS");

            migrationBuilder.DropTable(
                name: "NEGOCIO");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
