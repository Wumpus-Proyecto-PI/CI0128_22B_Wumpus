using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PI.EntityModels
{
    public partial class DataBaseContext : DbContext
    {
        public DataBaseContext()
        {
        }

        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Analisis> Analisis { get; set; } = null!;
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<Componente> Componentes { get; set; } = null!;
        public virtual DbSet<Configuracion> Configuracion { get; set; } = null!;
        public virtual DbSet<Egreso> Egresos { get; set; } = null!;
        public virtual DbSet<EsEmpleadoDe> EsEmpleadoDes { get; set; } = null!;
        public virtual DbSet<GastoFijo> GastoFijos { get; set; } = null!;
        public virtual DbSet<Ingreso> Ingresos { get; set; } = null!;
        public virtual DbSet<InversionInicial> InversionInicials { get; set; } = null!;
        public virtual DbSet<Mes> Mes { get; set; } = null!;
        public virtual DbSet<Negocio> Negocios { get; set; } = null!;
        public virtual DbSet<Producto> Productos { get; set; } = null!;
        public virtual DbSet<Puesto> Puestos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:wumpus.database.windows.net,1433;Initial Catalog=WumpusTest;Persist Security Info=False;User ID=admin01;Password=Wumpus01!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Analisis>(entity =>
            {
                entity.HasKey(e => e.FechaCreacion)
                    .HasName("PK__ANALISIS__19DE4892862EBA95");

                entity.ToTable("ANALISIS");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCreacion");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("direccion");

                entity.Property(e => e.EstadoAnalisis)
                    .HasColumnName("estadoAnalisis")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FechaDescarga)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaDescarga");

                entity.Property(e => e.GananciaMensual)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("gananciaMensual")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IdNegocio).HasColumnName("idNegocio");

                entity.HasOne(d => d.IdNegocioNavigation)
                    .WithMany(p => p.Analisis)
                    .HasForeignKey(d => d.IdNegocio)
                    .HasConstraintName("FK__ANALISIS__idNegocio");
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");

                            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                        });
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Componente>(entity =>
            {
                entity.HasKey(e => new { e.NombreComponente, e.NombreProducto, e.FechaAnalisis })
                    .HasName("PK__COMPONEN__DA38DD99DF589705");

                entity.ToTable("COMPONENTE");

                entity.HasIndex(e => e.Cantidad, "index_componente_cantidad_");

                entity.Property(e => e.NombreComponente)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("nombreComponente");

                entity.Property(e => e.NombreProducto)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("nombreProducto");

                entity.Property(e => e.FechaAnalisis)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaAnalisis");

                entity.Property(e => e.Cantidad)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("cantidad");

                entity.Property(e => e.Monto)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("monto");

                entity.Property(e => e.Unidad)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("unidad");

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.Componentes)
                    .HasForeignKey(d => new { d.NombreProducto, d.FechaAnalisis })
                    .HasConstraintName("FK_ComponenteProducto");
            });

            modelBuilder.Entity<Configuracion>(entity =>
            {
                entity.HasKey(e => e.FechaAnalisis)
                    .HasName("PK__CONFIGUR__79D41A8629CB69EB");

                entity.ToTable("CONFIGURACION");

                entity.Property(e => e.FechaAnalisis)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaAnalisis");

                entity.Property(e => e.PorcentajePl)
                    .HasColumnType("decimal(5, 2)")
                    .HasColumnName("porcentajePL")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PorcentajeSs)
                    .HasColumnType("decimal(5, 2)")
                    .HasColumnName("porcentajeSS")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TipoNegocio).HasColumnName("tipoNegocio");

                entity.HasOne(d => d.FechaAnalisisNavigation)
                    .WithOne(p => p.Configuracion)
                    .HasForeignKey<Configuracion>(d => d.FechaAnalisis)
                    .HasConstraintName("FK_ANALISIS_DELETE");
            });

            modelBuilder.Entity<Egreso>(entity =>
            {
                entity.HasKey(e => new { e.Mes, e.FechaAnalisis, e.Tipo, e.Monto })
                    .HasName("PK_Egreso");

                entity.ToTable("EGRESO");

                entity.Property(e => e.Mes)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("mes");

                entity.Property(e => e.FechaAnalisis)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaAnalisis");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("tipo");

                entity.Property(e => e.Monto)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("monto");

                entity.HasOne(d => d.FechaAnalisisNavigation)
                    .WithMany(p => p.Egresos)
                    .HasForeignKey(d => d.FechaAnalisis)
                    .HasConstraintName("FK_EGRESO_FECHA_DELETE");

                entity.HasOne(d => d.Me)
                    .WithMany(p => p.Egresos)
                    .HasForeignKey(d => new { d.Mes, d.FechaAnalisis })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EGRESO__1F63A897");
            });

            modelBuilder.Entity<EsEmpleadoDe>(entity =>
            {
                entity.HasKey(e => new { e.NombreEmpleado, e.FechaEmpleado, e.NombreJefe, e.FechaJefe })
                    .HasName("PK__ES_EMPLE__25C3089BB9377AC2");

                entity.ToTable("ES_EMPLEADO_DE");

                entity.Property(e => e.NombreEmpleado)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("nombreEmpleado");

                entity.Property(e => e.FechaEmpleado)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaEmpleado");

                entity.Property(e => e.NombreJefe)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("nombreJefe");

                entity.Property(e => e.FechaJefe)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaJefe");

                entity.HasOne(d => d.Puesto)
                    .WithMany(p => p.EsEmpleadoDes)
                    .HasForeignKey(d => new { d.NombreJefe, d.FechaJefe })
                    .HasConstraintName("FK_EMPLEADO_JEFE");
            });

            modelBuilder.Entity<GastoFijo>(entity =>
            {
                entity.HasKey(e => new { e.Orden, e.Nombre, e.FechaAnalisis })
                    .HasName("PK__GASTO_FI__C10BC2E58A2E7E29");

                entity.ToTable("GASTO_FIJO");

                entity.Property(e => e.Orden).HasColumnName("orden");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.FechaAnalisis)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaAnalisis");

                entity.Property(e => e.Monto)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("monto");

                entity.HasOne(d => d.FechaAnalisisNavigation)
                    .WithMany(p => p.GastoFijos)
                    .HasForeignKey(d => d.FechaAnalisis)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GASTO_FECHA");
            });

            modelBuilder.Entity<Ingreso>(entity =>
            {
                entity.HasKey(e => new { e.Mes, e.FechaAnalisis, e.Tipo, e.Monto })
                    .HasName("PK_Ingreso");

                entity.ToTable("INGRESO");

                entity.Property(e => e.Mes)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("mes");

                entity.Property(e => e.FechaAnalisis)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaAnalisis");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("tipo");

                entity.Property(e => e.Monto)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("monto");

                entity.HasOne(d => d.Me)
                    .WithMany(p => p.Ingresos)
                    .HasForeignKey(d => new { d.Mes, d.FechaAnalisis })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__INGRESO__2610A626");
            });

            modelBuilder.Entity<InversionInicial>(entity =>
            {
                entity.HasKey(e => new { e.FechaAnalisis, e.Nombre })
                    .HasName("PK__INVERSIO__1EFEE14A47BDAE37");

                entity.ToTable("INVERSION_INICIAL");

                entity.Property(e => e.FechaAnalisis)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaAnalisis");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Valor)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("valor");

                entity.HasOne(d => d.FechaAnalisisNavigation)
                    .WithMany(p => p.InversionInicials)
                    .HasForeignKey(d => d.FechaAnalisis)
                    .HasConstraintName("FK__INVERSION__FECHA");
            });

            modelBuilder.Entity<Mes>(entity =>
            {
                entity.HasKey(e => new { e.Nombre, e.FechaAnalisis })
                    .HasName("PK__MES__0532FD6FC344EF0E");

                entity.ToTable("MES");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.FechaAnalisis)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaAnalisis");

                entity.Property(e => e.InversionPorMes)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("inversionPorMes")
                    .HasDefaultValueSql("((0.0))");

                entity.HasOne(d => d.FechaAnalisisNavigation)
                    .WithMany(p => p.Mes)
                    .HasForeignKey(d => d.FechaAnalisis)
                    .HasConstraintName("FK_MES_FECHA");
            });

            modelBuilder.Entity<Negocio>(entity =>
            {
                entity.ToTable("NEGOCIO");

                entity.HasIndex(e => e.IdUsuario, "Negocio_idUsuario");

                entity.HasIndex(e => e.FechaCreacion, "id_negocio");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Negocios)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_AspNetUser_CorreoUsuario");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => new { e.Nombre, e.FechaAnalisis })
                    .HasName("PK__PRODUCTO__0532FD6FC589957A");

                entity.ToTable("PRODUCTO");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.FechaAnalisis)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaAnalisis");

                entity.Property(e => e.ComisionDeVentas)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("comisionDeVentas")
                    .HasDefaultValueSql("((0.0))");

                entity.Property(e => e.CostoVariable)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("costoVariable");

                entity.Property(e => e.Lote).HasColumnName("lote");

                entity.Property(e => e.PorcentajeDeVentas)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("porcentajeDeVentas")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Precio)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("precio")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.FechaAnalisisNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.FechaAnalisis)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PRODUCTO__FECHA");
            });

            modelBuilder.Entity<Puesto>(entity =>
            {
                entity.HasKey(e => new { e.Nombre, e.FechaAnalisis })
                    .HasName("PK__PUESTO__0532FD6FB7898CB1");

                entity.ToTable("PUESTO");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.FechaAnalisis)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaAnalisis");

                entity.Property(e => e.Beneficios)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("beneficios")
                    .HasDefaultValueSql("((0.0))");

                entity.Property(e => e.CantidadPlazas).HasColumnName("cantidadPlazas");

                entity.Property(e => e.Orden)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("orden");

                entity.Property(e => e.SalarioBruto)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("salarioBruto");

                entity.HasOne(d => d.FechaAnalisisNavigation)
                    .WithMany(p => p.Puestos)
                    .HasForeignKey(d => d.FechaAnalisis)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PUESTO__FECHA__UPDATE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
