USE [Wumpus_PROD]
GO

/****** Object:  Table [dbo].[AJUSTES_GASTOFIJO]    Script Date: 31/10/2022 6:04:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AJUSTES_GASTOFIJO](
	[correoUsuario] [varchar](50) NOT NULL,
	[nombre] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[correoUsuario] ASC,
	[nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[AJUSTES_USUARIO]    Script Date: 31/10/2022 6:04:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AJUSTES_USUARIO](
	[correoUsuario] [varchar](50) NOT NULL,
	[nombre] [varchar](100) NULL,
	[apellido1] [varchar](100) NULL,
	[apellido2] [varchar](100) NULL,
	[contrasenya] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[correoUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ANALISIS]    Script Date: 31/10/2022 6:04:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ANALISIS](
	[fechaCreacion] [datetime] NOT NULL,
	[direccion] [varchar](512) NULL,
	[fechaDescarga] [datetime] NULL,
	[gananciaMensual] [decimal](18, 2) NULL,
	[idNegocio] [int] NOT NULL,
	[estadoAnalisis] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[fechaCreacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[BENEFICIO]    Script Date: 31/10/2022 6:04:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BENEFICIO](
	[nombrePuesto] [varchar](30) NOT NULL,
	[fechaAnalisis] [datetime] NOT NULL,
	[nombre] [varchar](30) NOT NULL,
	[monto] [decimal](18, 2) NOT NULL,
	[cantidadPlazas] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[nombrePuesto] ASC,
	[fechaAnalisis] ASC,
	[nombre] ASC,
	[monto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[COMPONENTE]    Script Date: 31/10/2022 6:04:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMPONENTE](
	[nombreComponente] [varchar](30) NOT NULL,
	[nombreProducto] [varchar](30) NOT NULL,
	[fechaAnalisis] [datetime] NOT NULL,
	[monto] [decimal](18, 2) NULL,
	[cantidad] [decimal](18, 2) NULL,
	[unidad] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[nombreComponente] ASC,
	[nombreProducto] ASC,
	[fechaAnalisis] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[CONFIGURACION]    Script Date: 31/10/2022 6:04:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CONFIGURACION](
	[fechaAnalisis] [datetime] NOT NULL,
	[tipoNegocio] [int] NOT NULL,
	[porcentajeSS] [decimal](5, 2) NULL,
	[porcentajePL] [decimal](5, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[fechaAnalisis] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[EGRESO]    Script Date: 31/10/2022 6:04:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EGRESO](
	[mes] [varchar](10) NOT NULL,
	[fechaAnalisis] [datetime] NULL,
	[tipo] [bit] NOT NULL,
	[monto] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[mes] ASC,
	[tipo] ASC,
	[monto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ES_EMPLEADO_DE]    Script Date: 31/10/2022 6:04:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ES_EMPLEADO_DE](
	[nombreEmpleado] [varchar](30) NOT NULL,
	[fechaEmpleado] [datetime] NOT NULL,
	[nombreJefe] [varchar](30) NOT NULL,
	[fechaJefe] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[nombreEmpleado] ASC,
	[fechaEmpleado] ASC,
	[nombreJefe] ASC,
	[fechaJefe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[GASTO_FIJO]    Script Date: 31/10/2022 6:04:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GASTO_FIJO](
	[nombre] [varchar](30) NOT NULL,
	[fechaAnalisis] [datetime] NOT NULL,
	[monto] [decimal](18, 2) NULL,
	[orden] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[orden] ASC,
	[nombre] ASC,
	[fechaAnalisis] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[INGRESO]    Script Date: 31/10/2022 6:04:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INGRESO](
	[mes] [varchar](10) NOT NULL,
	[fechaAnalisis] [datetime] NULL,
	[tipo] [bit] NOT NULL,
	[monto] [decimal](18, 0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[mes] ASC,
	[tipo] ASC,
	[monto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[INVERSION_INICIAL]    Script Date: 31/10/2022 6:04:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INVERSION_INICIAL](
	[fechaAnalisis] [datetime] NOT NULL,
	[nombre] [varchar](30) NOT NULL,
	[valor] [decimal](18, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[fechaAnalisis] ASC,
	[nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[MES]    Script Date: 31/10/2022 6:04:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MES](
	[nombre] [varchar](10) NOT NULL,
	[porcentajeEstacionalidad] [tinyint] NULL,
	[fechaAnalisis] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[nombre] ASC,
	[fechaAnalisis] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[NEGOCIO]    Script Date: 31/10/2022 6:04:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NEGOCIO](
	[id] [int] NOT NULL,
	[nombre] [varchar](30) NULL,
	[correoUsuario] [varchar](50) NOT NULL,
	[FechaCreacion] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[PRODUCTO]    Script Date: 31/10/2022 6:04:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRODUCTO](
	[nombre] [varchar](30) NOT NULL,
	[fechaAnalisis] [datetime] NOT NULL,
	[lote] [int] NULL,
	[porcentajeDeVentas] [decimal](18, 2) NULL,
	[precio] [decimal](18, 2) NULL,
	[costoVariable] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[nombre] ASC,
	[fechaAnalisis] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[pruebachar]    Script Date: 31/10/2022 6:04:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[pruebachar](
	[cadena] [varchar](20) NULL
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[PUESTO]    Script Date: 31/10/2022 6:04:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PUESTO](
	[nombre] [varchar](30) NOT NULL,
	[fechaAnalisis] [datetime] NOT NULL,
	[cantidadPlazas] [int] NULL,
	[salarioBruto] [decimal](18, 2) NULL,
	[orden] [bigint] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[nombre] ASC,
	[fechaAnalisis] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[USUARIO]    Script Date: 31/10/2022 6:04:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[USUARIO](
	[correo] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[correo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ANALISIS] ADD  CONSTRAINT [Default_ganancia_mensual]  DEFAULT ((0)) FOR [gananciaMensual]
GO

ALTER TABLE [dbo].[ANALISIS] ADD  CONSTRAINT [DF_ANALISIS_estadoAnalisis]  DEFAULT ((1)) FOR [estadoAnalisis]
GO

ALTER TABLE [dbo].[CONFIGURACION] ADD  CONSTRAINT [DEFAULT_PORCENTAJSS]  DEFAULT ((0)) FOR [porcentajeSS]
GO

ALTER TABLE [dbo].[CONFIGURACION] ADD  CONSTRAINT [DEFAULT_PORCENTAJPL]  DEFAULT ((0)) FOR [porcentajePL]
GO

ALTER TABLE [dbo].[PRODUCTO] ADD  CONSTRAINT [df_porcentajeVentas]  DEFAULT ((0)) FOR [porcentajeDeVentas]
GO

ALTER TABLE [dbo].[PRODUCTO] ADD  CONSTRAINT [df_precio]  DEFAULT ((0)) FOR [precio]
GO

ALTER TABLE [dbo].[PRODUCTO] ADD  CONSTRAINT [default_costoVariable]  DEFAULT ((0.0)) FOR [costoVariable]
GO

ALTER TABLE [dbo].[AJUSTES_GASTOFIJO]  WITH CHECK ADD  CONSTRAINT [FK_correoUsuario_delete] FOREIGN KEY([correoUsuario])
REFERENCES [dbo].[AJUSTES_USUARIO] ([correoUsuario])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AJUSTES_GASTOFIJO] CHECK CONSTRAINT [FK_correoUsuario_delete]
GO

ALTER TABLE [dbo].[AJUSTES_GASTOFIJO]  WITH CHECK ADD  CONSTRAINT [FK_correoUsuario_update] FOREIGN KEY([correoUsuario])
REFERENCES [dbo].[AJUSTES_USUARIO] ([correoUsuario])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[AJUSTES_GASTOFIJO] CHECK CONSTRAINT [FK_correoUsuario_update]
GO

ALTER TABLE [dbo].[ANALISIS]  WITH CHECK ADD  CONSTRAINT [FK__ANALISIS__idNegocio] FOREIGN KEY([idNegocio])
REFERENCES [dbo].[NEGOCIO] ([id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ANALISIS] CHECK CONSTRAINT [FK__ANALISIS__idNegocio]
GO

ALTER TABLE [dbo].[ANALISIS]  WITH CHECK ADD  CONSTRAINT [FK__ANALISIS__UPDATE__idNegocio] FOREIGN KEY([idNegocio])
REFERENCES [dbo].[NEGOCIO] ([id])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[ANALISIS] CHECK CONSTRAINT [FK__ANALISIS__UPDATE__idNegocio]
GO

ALTER TABLE [dbo].[BENEFICIO]  WITH CHECK ADD  CONSTRAINT [FK_nombre_del_puesto] FOREIGN KEY([nombrePuesto], [fechaAnalisis])
REFERENCES [dbo].[PUESTO] ([nombre], [fechaAnalisis])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[BENEFICIO] CHECK CONSTRAINT [FK_nombre_del_puesto]
GO

ALTER TABLE [dbo].[BENEFICIO]  WITH CHECK ADD  CONSTRAINT [FK_nombre_del_puesto_delete] FOREIGN KEY([nombrePuesto], [fechaAnalisis])
REFERENCES [dbo].[PUESTO] ([nombre], [fechaAnalisis])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[BENEFICIO] CHECK CONSTRAINT [FK_nombre_del_puesto_delete]
GO

ALTER TABLE [dbo].[COMPONENTE]  WITH CHECK ADD  CONSTRAINT [FK_ComponenteProducto] FOREIGN KEY([nombreProducto], [fechaAnalisis])
REFERENCES [dbo].[PRODUCTO] ([nombre], [fechaAnalisis])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[COMPONENTE] CHECK CONSTRAINT [FK_ComponenteProducto]
GO

ALTER TABLE [dbo].[COMPONENTE]  WITH CHECK ADD  CONSTRAINT [FK_ComponenteProducto_Update] FOREIGN KEY([nombreProducto], [fechaAnalisis])
REFERENCES [dbo].[PRODUCTO] ([nombre], [fechaAnalisis])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[COMPONENTE] CHECK CONSTRAINT [FK_ComponenteProducto_Update]
GO

ALTER TABLE [dbo].[CONFIGURACION]  WITH CHECK ADD  CONSTRAINT [FK_ANALISIS_DELETE] FOREIGN KEY([fechaAnalisis])
REFERENCES [dbo].[ANALISIS] ([fechaCreacion])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CONFIGURACION] CHECK CONSTRAINT [FK_ANALISIS_DELETE]
GO

ALTER TABLE [dbo].[EGRESO]  WITH CHECK ADD FOREIGN KEY([mes], [fechaAnalisis])
REFERENCES [dbo].[MES] ([nombre], [fechaAnalisis])
GO

ALTER TABLE [dbo].[EGRESO]  WITH CHECK ADD  CONSTRAINT [FK_EGRESO_FECHA_DELETE] FOREIGN KEY([fechaAnalisis])
REFERENCES [dbo].[ANALISIS] ([fechaCreacion])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[EGRESO] CHECK CONSTRAINT [FK_EGRESO_FECHA_DELETE]
GO

ALTER TABLE [dbo].[EGRESO]  WITH CHECK ADD  CONSTRAINT [FK_EGRESO_FECHA_UPDATE] FOREIGN KEY([fechaAnalisis])
REFERENCES [dbo].[ANALISIS] ([fechaCreacion])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[EGRESO] CHECK CONSTRAINT [FK_EGRESO_FECHA_UPDATE]
GO

ALTER TABLE [dbo].[ES_EMPLEADO_DE]  WITH CHECK ADD  CONSTRAINT [FK_EMPLEADO_JEFE] FOREIGN KEY([nombreJefe], [fechaJefe])
REFERENCES [dbo].[PUESTO] ([nombre], [fechaAnalisis])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ES_EMPLEADO_DE] CHECK CONSTRAINT [FK_EMPLEADO_JEFE]
GO

ALTER TABLE [dbo].[ES_EMPLEADO_DE]  WITH CHECK ADD  CONSTRAINT [FK_EMPLEADO_JEFE_UPDATE] FOREIGN KEY([nombreJefe], [fechaJefe])
REFERENCES [dbo].[PUESTO] ([nombre], [fechaAnalisis])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[ES_EMPLEADO_DE] CHECK CONSTRAINT [FK_EMPLEADO_JEFE_UPDATE]
GO

ALTER TABLE [dbo].[GASTO_FIJO]  WITH CHECK ADD  CONSTRAINT [FK_GASTO_FECHA] FOREIGN KEY([fechaAnalisis])
REFERENCES [dbo].[ANALISIS] ([fechaCreacion])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[GASTO_FIJO] CHECK CONSTRAINT [FK_GASTO_FECHA]
GO

ALTER TABLE [dbo].[GASTO_FIJO]  WITH CHECK ADD  CONSTRAINT [FK_GASTO_FECHA_DELETE] FOREIGN KEY([fechaAnalisis])
REFERENCES [dbo].[ANALISIS] ([fechaCreacion])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[GASTO_FIJO] CHECK CONSTRAINT [FK_GASTO_FECHA_DELETE]
GO

ALTER TABLE [dbo].[INGRESO]  WITH CHECK ADD FOREIGN KEY([mes], [fechaAnalisis])
REFERENCES [dbo].[MES] ([nombre], [fechaAnalisis])
GO

ALTER TABLE [dbo].[INGRESO]  WITH CHECK ADD  CONSTRAINT [FK_INGRESO_DELETE] FOREIGN KEY([mes], [fechaAnalisis])
REFERENCES [dbo].[MES] ([nombre], [fechaAnalisis])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[INGRESO] CHECK CONSTRAINT [FK_INGRESO_DELETE]
GO

ALTER TABLE [dbo].[INGRESO]  WITH CHECK ADD  CONSTRAINT [FK_INGRESO_UPDATE] FOREIGN KEY([mes], [fechaAnalisis])
REFERENCES [dbo].[MES] ([nombre], [fechaAnalisis])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[INGRESO] CHECK CONSTRAINT [FK_INGRESO_UPDATE]
GO

ALTER TABLE [dbo].[INVERSION_INICIAL]  WITH CHECK ADD  CONSTRAINT [FK__INVERSION__FECHA] FOREIGN KEY([fechaAnalisis])
REFERENCES [dbo].[ANALISIS] ([fechaCreacion])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[INVERSION_INICIAL] CHECK CONSTRAINT [FK__INVERSION__FECHA]
GO

ALTER TABLE [dbo].[INVERSION_INICIAL]  WITH CHECK ADD  CONSTRAINT [FK__INVERSION__FECHA__UPDATE] FOREIGN KEY([fechaAnalisis])
REFERENCES [dbo].[ANALISIS] ([fechaCreacion])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[INVERSION_INICIAL] CHECK CONSTRAINT [FK__INVERSION__FECHA__UPDATE]
GO

ALTER TABLE [dbo].[MES]  WITH CHECK ADD  CONSTRAINT [FK_MES_FECHA] FOREIGN KEY([fechaAnalisis])
REFERENCES [dbo].[ANALISIS] ([fechaCreacion])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[MES] CHECK CONSTRAINT [FK_MES_FECHA]
GO

ALTER TABLE [dbo].[MES]  WITH CHECK ADD  CONSTRAINT [FK_MES_FECHA_UPDATE] FOREIGN KEY([fechaAnalisis])
REFERENCES [dbo].[ANALISIS] ([fechaCreacion])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[MES] CHECK CONSTRAINT [FK_MES_FECHA_UPDATE]
GO

ALTER TABLE [dbo].[NEGOCIO]  WITH CHECK ADD  CONSTRAINT [FK__CORREO__NEGOCIO] FOREIGN KEY([correoUsuario])
REFERENCES [dbo].[USUARIO] ([correo])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[NEGOCIO] CHECK CONSTRAINT [FK__CORREO__NEGOCIO]
GO

ALTER TABLE [dbo].[NEGOCIO]  WITH CHECK ADD  CONSTRAINT [FK__CORREO__NEGOCIO__DELETE] FOREIGN KEY([correoUsuario])
REFERENCES [dbo].[USUARIO] ([correo])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[NEGOCIO] CHECK CONSTRAINT [FK__CORREO__NEGOCIO__DELETE]
GO

ALTER TABLE [dbo].[NEGOCIO]  WITH CHECK ADD FOREIGN KEY([correoUsuario])
REFERENCES [dbo].[USUARIO] ([correo])
GO

ALTER TABLE [dbo].[PRODUCTO]  WITH CHECK ADD  CONSTRAINT [FK__PRODUCTO__FECHA] FOREIGN KEY([fechaAnalisis])
REFERENCES [dbo].[ANALISIS] ([fechaCreacion])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[PRODUCTO] CHECK CONSTRAINT [FK__PRODUCTO__FECHA]
GO

ALTER TABLE [dbo].[PRODUCTO]  WITH CHECK ADD  CONSTRAINT [FK__PRODUCTO__FECHA_DELETE] FOREIGN KEY([fechaAnalisis])
REFERENCES [dbo].[ANALISIS] ([fechaCreacion])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[PRODUCTO] CHECK CONSTRAINT [FK__PRODUCTO__FECHA_DELETE]
GO

ALTER TABLE [dbo].[PRODUCTO]  WITH CHECK ADD FOREIGN KEY([fechaAnalisis])
REFERENCES [dbo].[ANALISIS] ([fechaCreacion])
GO

ALTER TABLE [dbo].[PUESTO]  WITH CHECK ADD  CONSTRAINT [FK__PUESTO__FECHA__UPDATE] FOREIGN KEY([fechaAnalisis])
REFERENCES [dbo].[ANALISIS] ([fechaCreacion])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[PUESTO] CHECK CONSTRAINT [FK__PUESTO__FECHA__UPDATE]
GO

ALTER TABLE [dbo].[PUESTO]  WITH CHECK ADD FOREIGN KEY([fechaAnalisis])
REFERENCES [dbo].[ANALISIS] ([fechaCreacion])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CONFIGURACION]  WITH CHECK ADD  CONSTRAINT [CK_CONFIGURACION_TN] CHECK  (([tipoNegocio]>=(0) AND [tipoNegocio]<=(1)))
GO

ALTER TABLE [dbo].[CONFIGURACION] CHECK CONSTRAINT [CK_CONFIGURACION_TN]
GO

/* Procedimientos------------------------------------------------------------------------------------------- */
USE [Wumpus_PROD]
GO
-- Wilmer Araya Rivas | B80538
/****** Object:  StoredProcedure [dbo].[actualizarBeneficios]    Script Date: 31/10/2022 6:07:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[actualizarBeneficios](@fechaAnalisis DATETIME)
AS
BEGIN
	IF EXISTS (SELECT * FROM GASTO_FIJO G JOIN ANALISIS A on G.fechaAnalisis = A.fechaCreacion WHERE G.nombre = 'Beneficios de empleados' AND G.fechaAnalisis = @fechaAnalisis)
		UPDATE GASTO_FIJO 
		SET monto =  dbo.obtTotalBeneficios(@fechaAnalisis)
		WHERE GASTO_FIJO.nombre = 'Beneficios de empleados' AND GASTO_FIJO.fechaAnalisis = @fechaAnalisis 
	ELSE
		INSERT INTO GASTO_FIJO (nombre, fechaAnalisis, monto, orden) VALUES ('Beneficios de empleados', @fechaAnalisis, dbo.obtTotalBeneficios(@fechaAnalisis), 4)
END
GO

/****** Object:  StoredProcedure [dbo].[ActualizarGananciaMensual]    Script Date: 31/10/2022 6:07:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Gabriel Bonilla Rivera | C01252
CREATE PROCEDURE [dbo].[ActualizarGananciaMensual] @ganancia decimal, @fecha DateTime
AS
UPDATE ANALISIS 
SET gananciaMensual = @ganancia
WHERE ANALISIS.fechaCreacion = @fecha
GO

/****** Object:  StoredProcedure [dbo].[actualizarGastoPrestaciones]    Script Date: 31/10/2022 6:07:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Wilmer Araya Rivas | B80538
CREATE PROCEDURE [dbo].[actualizarGastoPrestaciones](@fechaAnalisis DATETIME, @porcentaje varchar(20))
AS
BEGIN
	SET @porcentaje = REPLACE(@porcentaje, ',', '.')
	INSERT INTO pruebachar (cadena) values (@porcentaje)
	IF EXISTS (SELECT * FROM GASTO_FIJO G JOIN ANALISIS A on G.fechaAnalisis = A.fechaCreacion WHERE G.nombre = 'Prestaciones laborales' AND G.fechaAnalisis = @fechaAnalisis)
		UPDATE GASTO_FIJO 
		SET monto =  dbo.obtGastoPrestaciones (@fechaAnalisis, dbo.convertTOdecimal (@porcentaje))
		WHERE GASTO_FIJO.nombre = 'Prestaciones laborales' AND GASTO_FIJO.fechaAnalisis = @fechaAnalisis 
	ELSE
		INSERT INTO GASTO_FIJO (nombre, fechaAnalisis, monto, orden) VALUES ('Prestaciones laborales', @fechaAnalisis, dbo.obtGastoPrestaciones (@fechaAnalisis, dbo.convertTOdecimal (@porcentaje)), 3)
END
GO

/****** Object:  StoredProcedure [dbo].[actualizarGastoSeguroSocial]    Script Date: 31/10/2022 6:07:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Wilmer Araya Rivas | B80538
CREATE PROCEDURE [dbo].[actualizarGastoSeguroSocial](@fechaAnalisis DATETIME, @porcentaje varchar(20))
AS
BEGIN
	SET @porcentaje = REPLACE(@porcentaje, ',', '.')
	IF EXISTS (SELECT * FROM GASTO_FIJO G JOIN ANALISIS A on G.fechaAnalisis = A.fechaCreacion WHERE G.nombre = 'Seguridad social' AND G.fechaAnalisis = @fechaAnalisis)
		UPDATE GASTO_FIJO 
		SET monto =  dbo.obtGastoSeguroSocial (@fechaAnalisis, dbo.convertTOdecimal (@porcentaje))
		WHERE GASTO_FIJO.nombre = 'Seguridad social' AND GASTO_FIJO.fechaAnalisis = @fechaAnalisis 
	ELSE
		INSERT INTO GASTO_FIJO VALUES ('Seguridad social', @fechaAnalisis, dbo.obtGastoSeguroSocial (@fechaAnalisis, dbo.convertTOdecimal (@porcentaje)), 2)
END
GO

/****** Object:  StoredProcedure [dbo].[actualizarSalariosNeto]    Script Date: 31/10/2022 6:07:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Wilmer Araya Rivas | B80538
CREATE PROCEDURE [dbo].[actualizarSalariosNeto](@fechaAnalisis DATETIME, @seguroSocial varchar(20), @prestaciones varchar(20))
AS
BEGIN
	SET @seguroSocial = REPLACE(@seguroSocial, ',', '.')
	SET @prestaciones = REPLACE(@prestaciones, ',', '.')
	IF EXISTS (SELECT * FROM GASTO_FIJO G JOIN ANALISIS A on G.fechaAnalisis = A.fechaCreacion WHERE G.nombre = 'Salarios netos' AND G.fechaAnalisis = @fechaAnalisis)
		UPDATE GASTO_FIJO 
		SET monto =  dbo.obtTotalSalariosNeto(@fechaAnalisis, dbo.convertTOdecimal (@seguroSocial), dbo.convertTOdecimal ( @prestaciones))
		WHERE GASTO_FIJO.nombre = 'Salarios netos' AND GASTO_FIJO.fechaAnalisis = @fechaAnalisis 
	ELSE
		INSERT INTO GASTO_FIJO VALUES ('Salarios netos', @fechaAnalisis, dbo.obtTotalSalariosNeto(@fechaAnalisis, dbo.convertTOdecimal (@seguroSocial), dbo.convertTOdecimal ( @prestaciones)), 1)
END
GO

/****** Object:  StoredProcedure [dbo].[AgregarComponente]    Script Date: 31/10/2022 6:07:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Christopher Perez Blanco | C05881
CREATE procedure [dbo].[AgregarComponente]
(@nombreComponente varchar(30), @nombreProducto varchar(30), @fechaAnalisis datetime, @monto decimal(18, 2),  @cantidad decimal(18, 2), @unidad int)
as
Begin
	insert into Componente values(@nombreComponente, @nombreProducto, @fechaAnalisis,@monto, @cantidad, @unidad)
End
GO

/****** Object:  StoredProcedure [dbo].[borrar_beneficio]    Script Date: 31/10/2022 6:07:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Gabriel Bonilla Rivera | C01252
CREATE PROCEDURE [dbo].[borrar_beneficio] @nombre varchar(30), @nombrePuesto varchar(30), @fechaAnalisis DateTime, @monto int, @plazas int
AS
DELETE FROM BENEFICIO WHERE nombre=@nombre and nombrePuesto=@nombrePuesto and fechaAnalisis=@fechaAnalisis and monto=@monto and cantidadPlazas=@plazas
GO

/****** Object:  StoredProcedure [dbo].[BorrarComponente]    Script Date: 31/10/2022 6:07:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Christopher Perez Blanco | C05881
Create procedure [dbo].[BorrarComponente] 
(@nombreComponente varchar(30), @nombreProducto varchar(30), @fechaAnalisis datetime)
as
Begin
	Delete from COMPONENTE where COMPONENTE.nombreComponente=@nombreComponente and COMPONENTE.nombreProducto=@nombreProducto and COMPONENTE.fechaAnalisis=@fechaAnalisis
End
GO

/****** Object:  StoredProcedure [dbo].[CostoVariablePorLoteComponente]    Script Date: 31/10/2022 6:07:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Gabriel Bonilla Rivera | C01252
CREATE PROCEDURE [dbo].[CostoVariablePorLoteComponente] @nombreComponente varchar(30), @nombreProducto varchar(30), @fechaAnalisis datetime, @result decimal(18,2) output
AS
set @result = (select C.cantidad * C.monto
from Componente as C
where C.nombreComponente = @nombreComponente and C.nombreProducto = @nombreProducto and C.fechaAnalisis = @fechaAnalisis)
GO

/****** Object:  StoredProcedure [dbo].[CostoVariablePorUnidadComponente]    Script Date: 31/10/2022 6:07:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Gabriel Bonilla Rivera | C01252
CREATE PROCEDURE [dbo].[CostoVariablePorUnidadComponente] @nombreComponente varchar(30), @nombreProducto varchar(30), @fechaAnalisis datetime, @resultado decimal(18,2) output
AS
declare @lote int 
set @lote = (select lote from PRODUCTO as P where P.fechaAnalisis = @fechaAnalisis and P.nombre = @nombreProducto)
declare @CostoVarLoteComponente decimal(18,2)
exec CostoVariablePorLoteComponente @nombreComponente = @nombreComponente, @nombreProducto = @nombreProducto, @fechaAnalisis = @fechaAnalisis, @result = @CostoVarLoteComponente output
set @resultado = @CostoVarLoteComponente / @lote
GO

/****** Object:  StoredProcedure [dbo].[CostoVariableTotal]    Script Date: 31/10/2022 6:07:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Gabriel Bonilla Rivera | C01252
CREATE PROCEDURE [dbo].[CostoVariableTotal] @nombreProducto VARCHAR(30), @fechaAnalisis DATETIME
AS
declare @nomComponente varchar(30), @total decimal(18,2), @costoVarUnitario decimal(18,2)
set @total = 0
declare c1 cursor FOR
select C.nombreComponente from COMPONENTE AS C where c.nombreProducto = @nombreProducto AND C.fechaAnalisis = @fechaAnalisis
open c1
fetch next from c1 into @nomComponente
while @@FETCH_STATUS = 0
	begin
	exec CostoVariablePorUnidadComponente @nombreComponente = @nomComponente, @nombreProducto = @nombreProducto, @fechaAnalisis = @fechaAnalisis, @resultado = @costoVarUnitario output;
	set @total = @total + @costoVarUnitario
	Print @costoVarUnitario
	fetch next from c1 into @nomComponente
	end
close c1
deallocate c1

Update PRODUCTO
SET PRODUCTO.costoVariable = @total
WHERE producto.fechaAnalisis = @fechaAnalisis and producto.nombre = @nombreProducto
GO

/****** Object:  StoredProcedure [dbo].[eliminarGastoFijo]    Script Date: 31/10/2022 6:07:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Fabian Orozco Chaves | B95690
CREATE PROCEDURE [dbo].[eliminarGastoFijo] (
	@nombreGastoFijo varchar(30),
	@fechaAnalisis datetime
)
AS
BEGIN
    Delete from GASTO_FIJO
	where GASTO_FIJO.fechaAnalisis = @fechaAnalisis 
		and GASTO_FIJO.nombre = @nombreGastoFijo
END;
GO

/****** Object:  StoredProcedure [dbo].[EliminarGastoInicial]    Script Date: 31/10/2022 6:07:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Fabian Orozco Chaves | B95690
create procedure [dbo].[EliminarGastoInicial] (@fechaAnalisis datetime, @nombre varchar(30)) as
begin
	Delete from INVERSION_INICIAL where nombre=@nombre and fechaAnalisis=@fechaAnalisis
end
GO

/****** Object:  StoredProcedure [dbo].[EliminarProducto]    Script Date: 31/10/2022 6:07:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Christopher Perez Blanco | C05881
Create procedure [dbo].[EliminarProducto] (@nombreProducto varchar(30), @fechaAnalisis datetime) as
Begin 
Delete from Producto where nombre=@nombreProducto and fechaAnalisis=@fechaAnalisis
end 

GO

/****** Object:  StoredProcedure [dbo].[IngresarGastoInicial]    Script Date: 31/10/2022 6:07:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Fabian Orozco Chaves | B95690
create   procedure [dbo].[IngresarGastoInicial] ( @fechaAnalisis datetime, @nombre varchar(30), @valor decimal(18,2)) as
begin
	SET @valor = REPLACE(@valor, ',', '.')
	insert into INVERSION_INICIAL values (@fechaAnalisis, @nombre, dbo.convertTOdecimal(@valor))
end
GO

/****** Object:  StoredProcedure [dbo].[insertarConfiguracionAnalisis]    Script Date: 31/10/2022 6:07:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Daniel Escobar Giraldo | C02748
CREATE   procedure [dbo].[insertarConfiguracionAnalisis](
	@fechaAnalisis datetime,
	@procentajeSS decimal(5,2),
	@procentajePL decimal(5,2)
)
as 
begin 

    if @procentajeSS >= 0 
    begin 
	    update CONFIGURACION 
	    set porcentajeSS= @procentajeSS
	    where CONFIGURACION.fechaAnalisis=@fechaAnalisis
    end

    if @procentajePL >= 0 
    begin 
        update CONFIGURACION 
	    set porcentajePL = @procentajePL
	    where CONFIGURACION.fechaAnalisis=@fechaAnalisis
    end
end
GO

/****** Object:  StoredProcedure [dbo].[insertarGastoFijo]    Script Date: 31/10/2022 6:07:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- Fabian Orozco Chaves | B95690
CREATE procedure [dbo].[insertarGastoFijo] (
	@nombreAnterior varchar(30),
	@nombreGastoFijo varchar(30),
	@fechaAnalisis datetime,
	@monto varchar(30),
	@orden int
)
as 
BEGIN
	SET @monto = REPLACE(@monto, ',', '.')
	if exists (select GASTO_FIJO.nombre from GASTO_FIJO
		where GASTO_FIJO.fechaAnalisis = @fechaAnalisis and nombre=@nombreAnterior )
	BEGIN
		update GASTO_FIJO
		set GASTO_FIJO.nombre = @nombreGastoFijo, GASTO_FIJO.fechaAnalisis = @fechaAnalisis,  GASTO_FIJO.monto = dbo.convertTOdecimal(@monto), GASTO_FIJO.orden = @orden
		where GASTO_FIJO.fechaAnalisis = @fechaAnalisis and nombre=@nombreAnterior
	end
	ELSE
	begin
		insert into GASTO_FIJO values (@nombreGastoFijo, @fechaAnalisis, dbo.convertTOdecimal(@monto), @orden)
	end
END;
GO

/****** Object:  StoredProcedure [dbo].[InsertarProducto]    Script Date: 31/10/2022 6:07:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Christopher Perez Blanco | C05881
CREATE procedure [dbo].[InsertarProducto] 
(@nombreProducto varchar(30), @nombreAnterior varchar(30), @fechaAnalisis datetime, @lote int, 
@porcentajeDeVentas decimal(18,2), @precio decimal(18,2), @costoVariable decimal(18, 2)) as 
begin
if exists (Select PRODUCTO.nombre from Producto 
where @nombreAnterior=PRODUCTO.nombre and @fechaAnalisis=PRODUCTO.fechaAnalisis)
	begin 
		update Producto set nombre=@nombreProducto, lote=@lote, porcentajeDeVentas=@porcentajeDeVentas, precio=@precio, costoVariable=@costoVariable
		where nombre=@nombreAnterior and fechaAnalisis=@fechaAnalisis
	end
else 
	begin
		insert into Producto values(@nombreProducto, @fechaAnalisis, @lote, @porcentajeDeVentas, @precio, @costoVariable)
	end
end
GO

/****** Object:  StoredProcedure [dbo].[ObtenerComponentes]    Script Date: 31/10/2022 6:07:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Christopher Perez Blanco | C05881
Create Procedure [dbo].[ObtenerComponentes] (@nombreProducto varchar(30), @fechaAnalisis datetime)
as 
Begin
SELECT * from COMPONENTE where nombreProducto=@nombreProducto and fechaAnalisis=@fechaAnalisis
End
GO

/****** Object:  StoredProcedure [dbo].[ObtenerConfigAnalisis]    Script Date: 31/10/2022 6:07:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Daniel Escobar Giraldo | C02748
CREATE procedure [dbo].[ObtenerConfigAnalisis](
	@fechaAnalisis Datetime
)
as 
begin 
	select fechaAnalisis, tipoNegocio, ISNULL(porcentajeSS, 0) as porcentajeSS, ISNULL(porcentajePL,0) as porcentajePL from Configuracion where fechaAnalisis = @fechaAnalisis
end
GO

/****** Object:  StoredProcedure [dbo].[ObtenerGastosIniciales]    Script Date: 31/10/2022 6:07:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Fabian Orozco Chaves | B95690
create procedure [dbo].[ObtenerGastosIniciales] ( @fechaAnalisis datetime ) as
begin
	SELECT * FROM INVERSION_INICIAL WHERE fechaAnalisis=@fechaAnalisis order by nombre ASC
end
GO

/****** Object:  StoredProcedure [dbo].[ObtenerNegocioDeAnalisis]    Script Date: 31/10/2022 6:07:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Daniel Escobar Giraldo | C02748
CREATE PROCEDURE [dbo].[ObtenerNegocioDeAnalisis] (@fechaAnalisis DATETIME)
AS
BEGIN
    SELECT N.id, N.nombre, N.correoUsuario, N.FechaCreacion 
    FROM NEGOCIO AS N JOIN ANALISIS AS A ON N.id = A.idNegocio
    WHERE A.fechaCreacion = @fechaAnalisis
END
GO

/****** Object:  StoredProcedure [dbo].[ObtenerPorcentajeDeVentasTotal]    Script Date: 31/10/2022 6:07:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Wilmer Araya Rivas | B80538
CREATE PROCEDURE [dbo].[ObtenerPorcentajeDeVentasTotal] (@fechaAnalisis Datetime)
AS
BEGIN
	SELECT SUM(porcentajeDeVentas) totalPorcentajeVentas
	FROM PRODUCTO
	WHERE fechaAnalisis = @fechaAnalisis
END
GO

/****** Object:  StoredProcedure [dbo].[ObtenerPorcentajeVentas]    Script Date: 31/10/2022 6:07:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Wilmer Araya Rivas | B80538
CREATE PROCEDURE [dbo].[ObtenerPorcentajeVentas] (@fechaAnalisis Datetime, @nombreProducto varchar(30))
AS
BEGIN
	SELECT porcentajeDeVentas
	FROM PRODUCTO
	WHERE fechaAnalisis = @fechaAnalisis AND nombre = @nombreProducto
END
GO

/****** Object:  StoredProcedure [dbo].[ObtenerProductos]    Script Date: 31/10/2022 6:07:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Christopher Perez Blanco | C05881
CREATE procedure [dbo].[ObtenerProductos] (
	@fechaAnalisis datetime
)
as
begin 
SELECT * FROM PRODUCTO WHERE fechaAnalisis=@fechaAnalisis ORDER BY nombre ASC
end
GO

/****** Object:  StoredProcedure [dbo].[ObtenerPuestos]    Script Date: 31/10/2022 6:07:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Daniel Escobar Giraldo | C02748
CREATE procedure [dbo].[ObtenerPuestos] (
	@fechaAnalisis datetime
)
as
begin 
SELECT * FROM PUESTO WHERE fechaAnalisis=@fechaAnalisis order by orden ASC
end
GO

/****** Object:  StoredProcedure [dbo].[ObtenerSumInversionInicial]    Script Date: 31/10/2022 6:07:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Fabian Orozco Chaves | B95690
create   procedure [dbo].[ObtenerSumInversionInicial] ( @fechaAnalisis Datetime) as
begin
	Select SUM(valor) total from INVERSION_INICIAL where fechaAnalisis = @fechaAnalisis
end
GO

/****** Object:  StoredProcedure [dbo].[obtGastosFijosList]    Script Date: 31/10/2022 6:07:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Fabian Orozco Chaves | B95690
CREATE PROCEDURE [dbo].[obtGastosFijosList] (@fechaAnalisis DATETIME) AS
BEGIN
	SELECT * FROM GASTO_FIJO
	WHERE GASTO_FIJO.fechaAnalisis = @fechaAnalisis
	ORDER BY GASTO_FIJO.nombre ASC
END
GO

/****** Object:  StoredProcedure [dbo].[obtNombreNegocio]    Script Date: 31/10/2022 6:07:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Fabian Orozco Chaves | B95690
CREATE PROCEDURE [dbo].[obtNombreNegocio] (@fechaAnalisis DATETIME) AS
BEGIN
SELECT DISTINCT NEGOCIO.nombre 
FROM NEGOCIO JOIN ANALISIS ON NEGOCIO.id = ANALISIS.idNegocio 
WHERE ANALISIS.fechaCreacion = @fechaAnalisis
END
GO

/****** Object:  StoredProcedure [dbo].[obtNombreNegocioPorID]    Script Date: 31/10/2022 6:07:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Fabian Orozco Chaves | B95690
create procedure [dbo].[obtNombreNegocioPorID] (@IDNegocio INT) as
begin
	SELECT negocio.nombre from dbo.negocio where negocio.ID = @IDNegocio
end
GO

/****** Object:  StoredProcedure [dbo].[obtSumGastosFijos]    Script Date: 31/10/2022 6:07:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Fabian Orozco Chaves | B95690
CREATE PROCEDURE [dbo].[obtSumGastosFijos] (@fechaAnalisis DATETIME) AS
BEGIN 
SELECT SUM(GASTO_FIJO.monto) totalAnual
FROM GASTO_FIJO WHERE GASTO_FIJO.fechaAnalisis = @fechaAnalisis
END
GO

/* Funciones ----------------------------------------------------------------------------------------------*/
USE Wumpus_PROD
GO

/****** Object:  UserDefinedFunction [dbo].[convertTOdecimal]    Script Date: 31/10/2022 6:09:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Wilmer Araya Rivas | B80538
CREATE FUNCTION [dbo].[convertTOdecimal] (@num varchar(20))
RETURNS DEC(18,3) AS
BEGIN
	DECLARE @Result DEC(18,3)
	SELECT @Result = CAST(@num AS DECIMAL(18,3))
	RETURN @Result
END
GO

/****** Object:  UserDefinedFunction [dbo].[obtGastoPrestaciones]    Script Date: 31/10/2022 6:09:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Wilmer Araya Rivas | B80538
CREATE FUNCTION [dbo].[obtGastoPrestaciones] (@fechaAnalisis DATETIME, @porcentaje DEC(6,3))
RETURNS DEC(18,3) AS
BEGIN
	DECLARE @Result DEC(18,3)
	SELECT @Result = dbo.obtSumSalarios (@fechaAnalisis) * @porcentaje
	RETURN @Result
END
GO

/****** Object:  UserDefinedFunction [dbo].[obtGastoSeguroSocial]    Script Date: 31/10/2022 6:09:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Wilmer Araya Rivas | B80538
CREATE FUNCTION [dbo].[obtGastoSeguroSocial] (@fechaAnalisis DATETIME, @porcentaje DEC(6,3))
RETURNS DEC(18,3) AS
BEGIN
	DECLARE @Result DEC(18,3)
	SELECT @Result = dbo.obtSumSalarios (@fechaAnalisis) * @porcentaje
	RETURN @Result
END
GO

/****** Object:  UserDefinedFunction [dbo].[obtSumSalarios]    Script Date: 31/10/2022 6:09:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Wilmer Araya Rivas | B80538
CREATE FUNCTION [dbo].[obtSumSalarios] (@fechaAnalisis DATETIME)
RETURNS DEC(18,3) AS
BEGIN
	DECLARE @totalSalarios DEC(18,3)
	IF EXISTS (SELECT * FROM PUESTO P WHERE P.fechaAnalisis = @fechaAnalisis)
		SELECT @totalSalarios = SUM(P.cantidadPlazas * P.SalarioBruto)
		FROM PUESTO P
		WHERE P.fechaAnalisis = @fechaAnalisis
	ELSE
		RETURN 0
	RETURN @totalSalarios
END
GO

/****** Object:  UserDefinedFunction [dbo].[obtTotalBeneficios]    Script Date: 31/10/2022 6:09:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Wilmer Araya Rivas | B80538
CREATE FUNCTION [dbo].[obtTotalBeneficios] (@fechaAnalisis DATETIME)
RETURNS DEC(18,3) AS
BEGIN
	DECLARE @Result DEC(18,3)
	IF EXISTS (SELECT * FROM BENEFICIO B WHERE B.fechaAnalisis = @fechaAnalisis)
		SELECT @Result = SUM(B.monto * B.cantidadPlazas)
		FROM BENEFICIO B
		WHERE B.fechaAnalisis = @fechaAnalisis
	ELSE
		RETURN 0
	RETURN @Result
END
GO

/****** Object:  UserDefinedFunction [dbo].[obtTotalSalariosNeto]    Script Date: 31/10/2022 6:09:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Wilmer Araya Rivas | B80538
CREATE FUNCTION [dbo].[obtTotalSalariosNeto] (@fechaAnalisis DATETIME, @seguroSocial DEC(6,3), @prestaciones DEC(6,3))
RETURNS DEC(18,3) AS
BEGIN
	DECLARE @Result DEC(18,3)
	SELECT @Result = dbo.obtSumSalarios (@fechaAnalisis) - dbo.obtGastoSeguroSocial(@fechaAnalisis,@seguroSocial) - dbo.obtGastoPrestaciones(@fechaAnalisis,@prestaciones)
	RETURN @Result
END
GO

/* Triggers ----------------------------------------------------------------------------------------------*/
USE [Wumpus_PROD]
GO

/****** Object:  Trigger [dbo].[ActualizarCostoVariable_DeleteComponente]    Script Date: 31/10/2022 6:13:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Gabriel Bonilla Rivera | C01252
CREATE trigger [dbo].[ActualizarCostoVariable_DeleteComponente]
on [dbo].[COMPONENTE] after delete
as
declare @nomProd varchar(30), @fecha datetime
declare c2 cursor for select d.nombreProducto, d.fechaAnalisis from deleted as d
open c2
fetch next from c2 into @nomProd, @fecha
while @@FETCH_STATUS = 0
	begin
		exec CostoVariableTotal @nombreProducto = @nomProd, @fechaAnalisis = @fecha
	end
close c2
deallocate c2
GO

ALTER TABLE [dbo].[COMPONENTE] ENABLE TRIGGER [ActualizarCostoVariable_DeleteComponente]
GO

/****** Object:  Trigger [dbo].[ActualizarCostoVariable_UpdateComponente]    Script Date: 31/10/2022 6:13:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Gabriel Bonilla Rivera | C01252
CREATE trigger [dbo].[ActualizarCostoVariable_UpdateComponente]
on [dbo].[COMPONENTE] after update, insert
as
declare @nomProd varchar(30), @fecha datetime
declare c3 cursor for select i.nombreProducto, i.fechaAnalisis from inserted as i
open c3
fetch next from c3 into @nomProd, @fecha
while @@FETCH_STATUS = 0
	begin
		exec CostoVariableTotal @nombreProducto = @nomProd, @fechaAnalisis = @fecha
		fetch next from c3 into @nomProd, @fecha
	end
close c3
deallocate c3
GO

ALTER TABLE [dbo].[COMPONENTE] ENABLE TRIGGER [ActualizarCostoVariable_UpdateComponente]
GO

USE [Wumpus_PROD]
GO

/****** Object:  Trigger [dbo].[ActualizarCostoVariable_UpdateLote]    Script Date: 31/10/2022 6:14:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Gabriel Bonilla Rivera | C01252
CREATE trigger [dbo].[ActualizarCostoVariable_UpdateLote] 
on [dbo].[PRODUCTO] after update
as
declare @nomProd varchar(30), @fecha datetime
set @nomProd = (select i.nombre from inserted as i)
print @nomProd
set @fecha = (select i.fechaAnalisis from inserted as i)
print @fecha

if (update (lote))
begin
	exec CostoVariableTotal @nombreProducto = @nomProd, @fechaAnalisis = @fecha
end
GO

ALTER TABLE [dbo].[PRODUCTO] ENABLE TRIGGER [ActualizarCostoVariable_UpdateLote]
GO

