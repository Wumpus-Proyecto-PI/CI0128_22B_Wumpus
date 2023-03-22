﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PI.EntityModels;

#nullable disable

namespace PI.Migrations.DataBase
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20230321225414_MontoNoKeyEgresoIngreso")]
    partial class MontoNoKeyEgresoIngreso
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AspNetUserRole", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("PI.EntityModels.Analisis", b =>
                {
                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaCreacion");

                    b.Property<string>("Direccion")
                        .HasMaxLength(512)
                        .IsUnicode(false)
                        .HasColumnType("varchar(512)")
                        .HasColumnName("direccion");

                    b.Property<int>("EstadoAnalisis")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("estadoAnalisis")
                        .HasDefaultValueSql("((1))");

                    b.Property<DateTime?>("FechaDescarga")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaDescarga");

                    b.Property<decimal?>("GananciaMensual")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("gananciaMensual")
                        .HasDefaultValueSql("((0))");

                    b.Property<int>("IdNegocio")
                        .HasColumnType("int")
                        .HasColumnName("idNegocio");

                    b.HasKey("FechaCreacion")
                        .HasName("PK__ANALISIS__19DE4892862EBA95");

                    b.HasIndex("IdNegocio");

                    b.ToTable("ANALISIS", (string)null);
                });

            modelBuilder.Entity("PI.EntityModels.AspNetRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "NormalizedName" }, "RoleNameIndex")
                        .IsUnique()
                        .HasFilter("([NormalizedName] IS NOT NULL)");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("PI.EntityModels.AspNetRoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "RoleId" }, "IX_AspNetRoleClaims_RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("PI.EntityModels.AspNetUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "NormalizedEmail" }, "EmailIndex");

                    b.HasIndex(new[] { "NormalizedUserName" }, "UserNameIndex")
                        .IsUnique()
                        .HasFilter("([NormalizedUserName] IS NOT NULL)");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("PI.EntityModels.AspNetUserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "UserId" }, "IX_AspNetUserClaims_UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("PI.EntityModels.AspNetUserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex(new[] { "UserId" }, "IX_AspNetUserLogins_UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("PI.EntityModels.AspNetUserToken", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("PI.EntityModels.Componente", b =>
                {
                    b.Property<string>("NombreComponente")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)")
                        .HasColumnName("nombreComponente");

                    b.Property<string>("NombreProducto")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)")
                        .HasColumnName("nombreProducto");

                    b.Property<DateTime>("FechaAnalisis")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaAnalisis");

                    b.Property<decimal?>("Cantidad")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("cantidad");

                    b.Property<decimal?>("Monto")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("monto");

                    b.Property<string>("Unidad")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)")
                        .HasColumnName("unidad");

                    b.HasKey("NombreComponente", "NombreProducto", "FechaAnalisis")
                        .HasName("PK__COMPONEN__DA38DD99DF589705");

                    b.HasIndex("NombreProducto", "FechaAnalisis");

                    b.HasIndex(new[] { "Cantidad" }, "index_componente_cantidad_");

                    b.ToTable("COMPONENTE", (string)null);
                });

            modelBuilder.Entity("PI.EntityModels.Configuracion", b =>
                {
                    b.Property<DateTime>("FechaAnalisis")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaAnalisis");

                    b.Property<decimal?>("PorcentajePl")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(5,2)")
                        .HasColumnName("porcentajePL")
                        .HasDefaultValueSql("((0))");

                    b.Property<decimal?>("PorcentajeSs")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(5,2)")
                        .HasColumnName("porcentajeSS")
                        .HasDefaultValueSql("((0))");

                    b.Property<int>("TipoNegocio")
                        .HasColumnType("int")
                        .HasColumnName("tipoNegocio");

                    b.HasKey("FechaAnalisis")
                        .HasName("PK__CONFIGUR__79D41A8629CB69EB");

                    b.ToTable("CONFIGURACION", (string)null);
                });

            modelBuilder.Entity("PI.EntityModels.Egreso", b =>
                {
                    b.Property<string>("Mes")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("mes");

                    b.Property<DateTime>("FechaAnalisis")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaAnalisis");

                    b.Property<string>("Tipo")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("tipo");

                    b.Property<decimal>("Monto")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("monto");

                    b.HasKey("Mes", "FechaAnalisis", "Tipo")
                        .HasName("PK_Egreso");

                    b.HasIndex("FechaAnalisis");

                    b.ToTable("EGRESO", (string)null);
                });

            modelBuilder.Entity("PI.EntityModels.EsEmpleadoDe", b =>
                {
                    b.Property<string>("NombreEmpleado")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)")
                        .HasColumnName("nombreEmpleado");

                    b.Property<DateTime>("FechaEmpleado")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaEmpleado");

                    b.Property<string>("NombreJefe")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)")
                        .HasColumnName("nombreJefe");

                    b.Property<DateTime>("FechaJefe")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaJefe");

                    b.HasKey("NombreEmpleado", "FechaEmpleado", "NombreJefe", "FechaJefe")
                        .HasName("PK__ES_EMPLE__25C3089BB9377AC2");

                    b.HasIndex("NombreJefe", "FechaJefe");

                    b.ToTable("ES_EMPLEADO_DE", (string)null);
                });

            modelBuilder.Entity("PI.EntityModels.GastoFijo", b =>
                {
                    b.Property<string>("Nombre")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)")
                        .HasColumnName("nombre");

                    b.Property<DateTime>("FechaAnalisis")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaAnalisis");

                    b.Property<decimal?>("Monto")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("monto");

                    b.Property<int>("Orden")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("orden");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Orden"), 1L, 1);

                    b.HasKey("Nombre", "FechaAnalisis")
                        .HasName("PK__GASTO_FI__C10BC2E58A2E7E29");

                    b.HasIndex("FechaAnalisis");

                    b.ToTable("GASTO_FIJO", (string)null);
                });

            modelBuilder.Entity("PI.EntityModels.Ingreso", b =>
                {
                    b.Property<string>("Mes")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("mes");

                    b.Property<DateTime>("FechaAnalisis")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaAnalisis");

                    b.Property<string>("Tipo")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("tipo");

                    b.Property<decimal>("Monto")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("monto");

                    b.HasKey("Mes", "FechaAnalisis", "Tipo")
                        .HasName("PK_Ingreso");

                    b.ToTable("INGRESO", (string)null);
                });

            modelBuilder.Entity("PI.EntityModels.InversionInicial", b =>
                {
                    b.Property<DateTime>("FechaAnalisis")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaAnalisis");

                    b.Property<string>("Nombre")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)")
                        .HasColumnName("nombre");

                    b.Property<decimal?>("Valor")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("valor");

                    b.HasKey("FechaAnalisis", "Nombre")
                        .HasName("PK__INVERSIO__1EFEE14A47BDAE37");

                    b.ToTable("INVERSION_INICIAL", (string)null);
                });

            modelBuilder.Entity("PI.EntityModels.Mes", b =>
                {
                    b.Property<string>("Nombre")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("nombre");

                    b.Property<DateTime>("FechaAnalisis")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaAnalisis");

                    b.Property<decimal?>("InversionPorMes")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("inversionPorMes")
                        .HasDefaultValueSql("((0.0))");

                    b.HasKey("Nombre", "FechaAnalisis")
                        .HasName("PK__MES__0532FD6FC344EF0E");

                    b.HasIndex("FechaAnalisis");

                    b.ToTable("MES", (string)null);
                });

            modelBuilder.Entity("PI.EntityModels.Negocio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("FechaCreacion")
                        .HasColumnType("datetime");

                    b.Property<string>("IdUsuario")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("idUsuario");

                    b.Property<string>("Nombre")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)")
                        .HasColumnName("nombre");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "IdUsuario" }, "Negocio_idUsuario");

                    b.HasIndex(new[] { "FechaCreacion" }, "id_negocio");

                    b.ToTable("NEGOCIO", (string)null);
                });

            modelBuilder.Entity("PI.EntityModels.Producto", b =>
                {
                    b.Property<string>("Nombre")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)")
                        .HasColumnName("nombre");

                    b.Property<DateTime>("FechaAnalisis")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaAnalisis");

                    b.Property<decimal?>("ComisionDeVentas")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("comisionDeVentas")
                        .HasDefaultValueSql("((0.0))");

                    b.Property<decimal>("CostoVariable")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("costoVariable");

                    b.Property<int?>("Lote")
                        .HasColumnType("int")
                        .HasColumnName("lote");

                    b.Property<decimal?>("PorcentajeDeVentas")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("porcentajeDeVentas")
                        .HasDefaultValueSql("((0))");

                    b.Property<decimal?>("Precio")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("precio")
                        .HasDefaultValueSql("((0))");

                    b.HasKey("Nombre", "FechaAnalisis")
                        .HasName("PK__PRODUCTO__0532FD6FC589957A");

                    b.HasIndex("FechaAnalisis");

                    b.ToTable("PRODUCTO", (string)null);
                });

            modelBuilder.Entity("PI.EntityModels.Puesto", b =>
                {
                    b.Property<string>("Nombre")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)")
                        .HasColumnName("nombre");

                    b.Property<DateTime>("FechaAnalisis")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaAnalisis");

                    b.Property<decimal?>("Beneficios")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("beneficios")
                        .HasDefaultValueSql("((0.0))");

                    b.Property<int?>("CantidadPlazas")
                        .HasColumnType("int")
                        .HasColumnName("cantidadPlazas");

                    b.Property<long>("Orden")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("orden");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Orden"), 1L, 1);

                    b.Property<decimal?>("SalarioBruto")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("salarioBruto");

                    b.HasKey("Nombre", "FechaAnalisis")
                        .HasName("PK__PUESTO__0532FD6FB7898CB1");

                    b.HasIndex("FechaAnalisis");

                    b.ToTable("PUESTO", (string)null);
                });

            modelBuilder.Entity("AspNetUserRole", b =>
                {
                    b.HasOne("PI.EntityModels.AspNetRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PI.EntityModels.AspNetUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PI.EntityModels.Analisis", b =>
                {
                    b.HasOne("PI.EntityModels.Negocio", "IdNegocioNavigation")
                        .WithMany("Analisis")
                        .HasForeignKey("IdNegocio")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__ANALISIS__idNegocio");

                    b.Navigation("IdNegocioNavigation");
                });

            modelBuilder.Entity("PI.EntityModels.AspNetRoleClaim", b =>
                {
                    b.HasOne("PI.EntityModels.AspNetRole", "Role")
                        .WithMany("AspNetRoleClaims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("PI.EntityModels.AspNetUserClaim", b =>
                {
                    b.HasOne("PI.EntityModels.AspNetUser", "User")
                        .WithMany("AspNetUserClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PI.EntityModels.AspNetUserLogin", b =>
                {
                    b.HasOne("PI.EntityModels.AspNetUser", "User")
                        .WithMany("AspNetUserLogins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PI.EntityModels.AspNetUserToken", b =>
                {
                    b.HasOne("PI.EntityModels.AspNetUser", "User")
                        .WithMany("AspNetUserTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PI.EntityModels.Componente", b =>
                {
                    b.HasOne("PI.EntityModels.Producto", "Producto")
                        .WithMany("Componentes")
                        .HasForeignKey("NombreProducto", "FechaAnalisis")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ComponenteProducto");

                    b.Navigation("Producto");
                });

            modelBuilder.Entity("PI.EntityModels.Configuracion", b =>
                {
                    b.HasOne("PI.EntityModels.Analisis", "FechaAnalisisNavigation")
                        .WithOne("Configuracion")
                        .HasForeignKey("PI.EntityModels.Configuracion", "FechaAnalisis")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ANALISIS_DELETE");

                    b.Navigation("FechaAnalisisNavigation");
                });

            modelBuilder.Entity("PI.EntityModels.Egreso", b =>
                {
                    b.HasOne("PI.EntityModels.Analisis", "FechaAnalisisNavigation")
                        .WithMany("Egresos")
                        .HasForeignKey("FechaAnalisis")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_EGRESO_FECHA_DELETE");

                    b.HasOne("PI.EntityModels.Mes", "Me")
                        .WithMany("Egresos")
                        .HasForeignKey("Mes", "FechaAnalisis")
                        .IsRequired()
                        .HasConstraintName("FK__EGRESO__1F63A897");

                    b.Navigation("FechaAnalisisNavigation");

                    b.Navigation("Me");
                });

            modelBuilder.Entity("PI.EntityModels.EsEmpleadoDe", b =>
                {
                    b.HasOne("PI.EntityModels.Puesto", "Puesto")
                        .WithMany("EsEmpleadoDes")
                        .HasForeignKey("NombreJefe", "FechaJefe")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_EMPLEADO_JEFE");

                    b.Navigation("Puesto");
                });

            modelBuilder.Entity("PI.EntityModels.GastoFijo", b =>
                {
                    b.HasOne("PI.EntityModels.Analisis", "FechaAnalisisNavigation")
                        .WithMany("GastoFijos")
                        .HasForeignKey("FechaAnalisis")
                        .IsRequired()
                        .HasConstraintName("FK_GASTO_FECHA");

                    b.Navigation("FechaAnalisisNavigation");
                });

            modelBuilder.Entity("PI.EntityModels.Ingreso", b =>
                {
                    b.HasOne("PI.EntityModels.Mes", "Me")
                        .WithMany("Ingresos")
                        .HasForeignKey("Mes", "FechaAnalisis")
                        .IsRequired()
                        .HasConstraintName("FK__INGRESO__2610A626");

                    b.Navigation("Me");
                });

            modelBuilder.Entity("PI.EntityModels.InversionInicial", b =>
                {
                    b.HasOne("PI.EntityModels.Analisis", "FechaAnalisisNavigation")
                        .WithMany("InversionInicials")
                        .HasForeignKey("FechaAnalisis")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__INVERSION__FECHA");

                    b.Navigation("FechaAnalisisNavigation");
                });

            modelBuilder.Entity("PI.EntityModels.Mes", b =>
                {
                    b.HasOne("PI.EntityModels.Analisis", "FechaAnalisisNavigation")
                        .WithMany("Mes")
                        .HasForeignKey("FechaAnalisis")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_MES_FECHA");

                    b.Navigation("FechaAnalisisNavigation");
                });

            modelBuilder.Entity("PI.EntityModels.Negocio", b =>
                {
                    b.HasOne("PI.EntityModels.AspNetUser", "IdUsuarioNavigation")
                        .WithMany("Negocios")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_AspNetUser_CorreoUsuario");

                    b.Navigation("IdUsuarioNavigation");
                });

            modelBuilder.Entity("PI.EntityModels.Producto", b =>
                {
                    b.HasOne("PI.EntityModels.Analisis", "FechaAnalisisNavigation")
                        .WithMany("Productos")
                        .HasForeignKey("FechaAnalisis")
                        .IsRequired()
                        .HasConstraintName("FK__PRODUCTO__FECHA");

                    b.Navigation("FechaAnalisisNavigation");
                });

            modelBuilder.Entity("PI.EntityModels.Puesto", b =>
                {
                    b.HasOne("PI.EntityModels.Analisis", "FechaAnalisisNavigation")
                        .WithMany("Puestos")
                        .HasForeignKey("FechaAnalisis")
                        .IsRequired()
                        .HasConstraintName("FK__PUESTO__FECHA__UPDATE");

                    b.Navigation("FechaAnalisisNavigation");
                });

            modelBuilder.Entity("PI.EntityModels.Analisis", b =>
                {
                    b.Navigation("Configuracion");

                    b.Navigation("Egresos");

                    b.Navigation("GastoFijos");

                    b.Navigation("InversionInicials");

                    b.Navigation("Mes");

                    b.Navigation("Productos");

                    b.Navigation("Puestos");
                });

            modelBuilder.Entity("PI.EntityModels.AspNetRole", b =>
                {
                    b.Navigation("AspNetRoleClaims");
                });

            modelBuilder.Entity("PI.EntityModels.AspNetUser", b =>
                {
                    b.Navigation("AspNetUserClaims");

                    b.Navigation("AspNetUserLogins");

                    b.Navigation("AspNetUserTokens");

                    b.Navigation("Negocios");
                });

            modelBuilder.Entity("PI.EntityModels.Mes", b =>
                {
                    b.Navigation("Egresos");

                    b.Navigation("Ingresos");
                });

            modelBuilder.Entity("PI.EntityModels.Negocio", b =>
                {
                    b.Navigation("Analisis");
                });

            modelBuilder.Entity("PI.EntityModels.Producto", b =>
                {
                    b.Navigation("Componentes");
                });

            modelBuilder.Entity("PI.EntityModels.Puesto", b =>
                {
                    b.Navigation("EsEmpleadoDes");
                });
#pragma warning restore 612, 618
        }
    }
}
