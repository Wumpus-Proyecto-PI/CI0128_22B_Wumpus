Use Wumpus_PROD
-- Tabla Usuario
CREATE TABLE [dbo].[USUARIO] (
    [correo] VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([correo] ASC)
);

-- Tabla Negocio
CREATE TABLE [dbo].[NEGOCIO] (
    [id]            INT          NOT NULL,
    [nombre]        VARCHAR (30) NULL,
    [correoUsuario] VARCHAR (50) NOT NULL,
    [FechaCreacion] DATETIME     NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK__CORREO__NEGOCIO] FOREIGN KEY ([correoUsuario]) REFERENCES [dbo].[USUARIO] ([correo]) ON UPDATE CASCADE,
    CONSTRAINT [FK__CORREO__NEGOCIO__DELETE] FOREIGN KEY ([correoUsuario]) REFERENCES [dbo].[USUARIO] ([correo]) ON DELETE CASCADE,
    FOREIGN KEY ([correoUsuario]) REFERENCES [dbo].[USUARIO] ([correo])
);

-- Tabla Analisis
CREATE TABLE [dbo].[ANALISIS] (
    [fechaCreacion]   DATETIME        NOT NULL,
    [direccion]       VARCHAR (512)   NULL,
    [fechaDescarga]   DATETIME        NULL,
    [gananciaMensual] DECIMAL (18, 2) NULL,
    [idNegocio]       INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([fechaCreacion] ASC),
    CONSTRAINT [FK__ANALISIS__UPDATE__idNegocio] FOREIGN KEY ([idNegocio]) REFERENCES [dbo].[NEGOCIO] ([id]) ON UPDATE CASCADE,
    CONSTRAINT [FK__ANALISIS__idNegocio] FOREIGN KEY ([idNegocio]) REFERENCES [dbo].[NEGOCIO] ([id]) ON DELETE CASCADE
);

-- Tabla Puesto
CREATE TABLE [dbo].[PUESTO] (
    [nombre]         VARCHAR (30)    NOT NULL,
    [fechaAnalisis]  DATETIME        NOT NULL,
    [cantidadPlazas] INT             NULL,
    [salarioBruto]   DECIMAL (18, 2) NULL,
    [orden]          BIGINT          IDENTITY (1, 1) NOT NULL,
    PRIMARY KEY CLUSTERED ([nombre] ASC, [fechaAnalisis] ASC),
    CONSTRAINT [FK__PUESTO__FECHA__UPDATE] FOREIGN KEY ([fechaAnalisis]) REFERENCES [dbo].[ANALISIS] ([fechaCreacion]) ON UPDATE CASCADE,
    FOREIGN KEY ([fechaAnalisis]) REFERENCES [dbo].[ANALISIS] ([fechaCreacion]) ON DELETE CASCADE
);

-- Tabla Mes
CREATE TABLE [dbo].[MES] (
    [nombre]                   VARCHAR (10) NOT NULL,
    [porcentajeEstacionalidad] TINYINT      NULL,
    [fechaAnalisis]            DATETIME     NOT NULL,
    PRIMARY KEY CLUSTERED ([nombre] ASC, [fechaAnalisis] ASC),
    CONSTRAINT [FK__MES__FECHA__UPDATE] FOREIGN KEY ([fechaAnalisis]) REFERENCES [dbo].[ANALISIS] ([fechaCreacion]) ON UPDATE CASCADE,
    CONSTRAINT [FK__MES__FECHA] FOREIGN KEY ([fechaAnalisis]) REFERENCES [dbo].[ANALISIS] ([fechaCreacion]) ON DELETE CASCADE
);

-- Tabla Configuracion
CREATE TABLE [dbo].[CONFIGURACION] (
    [fechaAnalisis] DATETIME        NOT NULL,
    [tipoCambio]    DECIMAL (18, 2) NULL,
    [moneda]        VARCHAR (20)    NULL,
    [tipoNegocio]   INT             NOT NULL,
    [porcentajeSS]  DECIMAL (5, 2)  CONSTRAINT [DEFAULT_PORCENTAJSS] DEFAULT ((0)) NULL,
    [porcentajePL]  DECIMAL (5, 2)  CONSTRAINT [DEFAULT_PORCENTAJPL] DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([fechaAnalisis] ASC),
    CONSTRAINT [FK__ANALISIS__DELETE] FOREIGN KEY ([fechaAnalisis]) REFERENCES [dbo].[ANALISIS] ([fechaCreacion]) ON DELETE CASCADE,
    CONSTRAINT [CK__CONFIGURACION__TN] CHECK ([tipoNegocio]>=(0) AND [tipoNegocio]<=(1))
);

-- Tabla Ajustes de Usuario
CREATE TABLE [dbo].[AJUSTES_USUARIO] (
    [correoUsuario] VARCHAR (50)  NOT NULL,
    [nombre]        VARCHAR (100) NULL,
    [apellido1]     VARCHAR (100) NULL,
    [apellido2]     VARCHAR (100) NULL,
    [contrasenya]   VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([correoUsuario] ASC),
	CONSTRAINT [FK__AJUSTEU__CORREO] FOREIGN KEY ([correoUsuario]) REFERENCES [dbo].[USUARIO] ([correo]) ON DELETE CASCADE
);

-- Tabla Gasto Fijo 
CREATE TABLE [dbo].[GASTO_FIJO] (
    [nombre]        VARCHAR (30)    NOT NULL,
    [fechaAnalisis] DATETIME        NOT NULL,
    [monto]         DECIMAL (18, 2) NULL,
    [orden]         INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([orden] ASC, [nombre] ASC, [fechaAnalisis] ASC),
    CONSTRAINT [FK__GASTO__FECHA] FOREIGN KEY ([fechaAnalisis]) REFERENCES [dbo].[ANALISIS] ([fechaCreacion]) ON UPDATE CASCADE,
    CONSTRAINT [FK__GASTO__FECHA__DELETE] FOREIGN KEY ([fechaAnalisis]) REFERENCES [dbo].[ANALISIS] ([fechaCreacion]) ON DELETE CASCADE
);

-- Tabla Producto 
CREATE TABLE [dbo].[PRODUCTO] (
    [nombre]             VARCHAR (30)    NOT NULL,
    [fechaAnalisis]      DATETIME        NOT NULL,
    [lote]               INT             NULL,
    [porcentajeDeVentas] TINYINT         NULL,
    [precio]             DECIMAL (18, 2) NULL,
    PRIMARY KEY CLUSTERED ([nombre] ASC, [fechaAnalisis] ASC),
    CONSTRAINT [FK__PRODUCTO__FECHA] FOREIGN KEY ([fechaAnalisis]) REFERENCES [dbo].[ANALISIS] ([fechaCreacion]) ON UPDATE CASCADE,
    CONSTRAINT [FK__PRODUCTO__FECHA_DELETE] FOREIGN KEY ([fechaAnalisis]) REFERENCES [dbo].[ANALISIS] ([fechaCreacion]) ON DELETE CASCADE,
    FOREIGN KEY ([fechaAnalisis]) REFERENCES [dbo].[ANALISIS] ([fechaCreacion])
);

-- Tabla Componente 
CREATE TABLE [dbo].[COMPONENTE] (
    [nombreComponente] VARCHAR (30)    NOT NULL,
    [nombreProducto]   VARCHAR (30)    NOT NULL,
    [fechaAnalisis]    DATETIME        NOT NULL,
    [monto]            DECIMAL (18, 2) NULL,
    PRIMARY KEY CLUSTERED ([nombreComponente] ASC, [nombreProducto] ASC, [fechaAnalisis] ASC),
    CONSTRAINT [FK__COMPONENTE__FECHA__UPDATE] FOREIGN KEY ([fechaAnalisis]) REFERENCES [dbo].[ANALISIS] ([fechaCreacion]) ON UPDATE CASCADE,
    CONSTRAINT [FK__COMPONENTE__FECHA] FOREIGN KEY ([fechaAnalisis]) REFERENCES [dbo].[ANALISIS] ([fechaCreacion]) ON DELETE CASCADE
);

-- Tabla Egreso
CREATE TABLE [dbo].[EGRESO] (
    [mes]           VARCHAR (10)    NOT NULL,
    [fechaAnalisis] DATETIME        NULL,
    [tipo]          BIT             NOT NULL,
    [monto]         DECIMAL (18, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([mes] ASC, [tipo] ASC, [monto] ASC),
    CONSTRAINT [FK__EGRESO__FECHA__DELETE] FOREIGN KEY ([fechaAnalisis]) REFERENCES [dbo].[ANALISIS] ([fechaCreacion]) ON DELETE CASCADE,
    CONSTRAINT [FK__EGRESO__FECHA__UPDATE] FOREIGN KEY ([fechaAnalisis]) REFERENCES [dbo].[ANALISIS] ([fechaCreacion]) ON UPDATE CASCADE,
    FOREIGN KEY ([mes], [fechaAnalisis]) REFERENCES [dbo].[MES] ([nombre], [fechaAnalisis])
);

-- Tabla Ingreso
CREATE TABLE [dbo].[INGRESO] (
    [mes]           VARCHAR (10) NOT NULL,
    [fechaAnalisis] DATETIME     NULL,
    [tipo]          BIT          NOT NULL,
    [monto]         DECIMAL (18) NOT NULL,
    PRIMARY KEY CLUSTERED ([mes] ASC, [tipo] ASC, [monto] ASC),
    CONSTRAINT [FK__INGRESO__DELETE] FOREIGN KEY ([mes], [fechaAnalisis]) REFERENCES [dbo].[MES] ([nombre], [fechaAnalisis]) ON DELETE CASCADE,
    CONSTRAINT [FK__INGRESO__UPDATE] FOREIGN KEY ([mes], [fechaAnalisis]) REFERENCES [dbo].[MES] ([nombre], [fechaAnalisis]) ON UPDATE CASCADE,
    FOREIGN KEY ([mes], [fechaAnalisis]) REFERENCES [dbo].[MES] ([nombre], [fechaAnalisis])
);

-- Tabla Beneficio
CREATE TABLE [dbo].[BENEFICIO] (
    [nombrePuesto]   VARCHAR (30)    NOT NULL,
    [fechaAnalisis]  DATETIME        NOT NULL,
    [nombre]         VARCHAR (30)    NOT NULL,
    [monto]          DECIMAL (18, 2) NOT NULL,
    [cantidadPlazas] INT             NULL,
    PRIMARY KEY CLUSTERED ([nombrePuesto] ASC, [fechaAnalisis] ASC, [nombre] ASC, [monto] ASC),
    CONSTRAINT [FK__NOMBRE__PUESTO] FOREIGN KEY ([nombrePuesto], [fechaAnalisis]) REFERENCES [dbo].[PUESTO] ([nombre], [fechaAnalisis]) ON UPDATE CASCADE,
    CONSTRAINT [FK__NOMBRE__PUESTO__DELETE] FOREIGN KEY ([nombrePuesto], [fechaAnalisis]) REFERENCES [dbo].[PUESTO] ([nombre], [fechaAnalisis]) ON DELETE CASCADE
);

-- Tabla Es Empleado De
CREATE TABLE [dbo].[ES_EMPLEADO_DE] (
    [nombreEmpleado] VARCHAR (30) NOT NULL,
    [fechaEmpleado]  DATETIME     NOT NULL,
    [nombreJefe]     VARCHAR (30) NOT NULL,
    [fechaJefe]      DATETIME     NOT NULL,
    PRIMARY KEY CLUSTERED ([nombreEmpleado] ASC, [fechaEmpleado] ASC, [nombreJefe] ASC, [fechaJefe] ASC),
    CONSTRAINT [FK__EMPLEADO__JEFE__UPDATE] FOREIGN KEY ([nombreJefe], [fechaJefe]) REFERENCES [dbo].[PUESTO] ([nombre], [fechaAnalisis]) ON UPDATE CASCADE,
    CONSTRAINT [FK__EMPLEADO__JEFE] FOREIGN KEY ([nombreJefe], [fechaJefe]) REFERENCES [dbo].[PUESTO] ([nombre], [fechaAnalisis]) ON DELETE CASCADE
);

-- Procedure 1
GO
/****** Object:  StoredProcedure [dbo].[actualizarBeneficios]    Script Date: 10/3/2022 7:12:27 PM ******/
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

-- Procedure 2
GO
/****** Object:  StoredProcedure [dbo].[actualizarGastoPrestaciones]    Script Date: 10/3/2022 7:13:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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

-- Procedure 3
GO
/****** Object:  StoredProcedure [dbo].[actualizarGastoSeguroSocial]    Script Date: 10/3/2022 7:14:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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

-- Procedure 4
GO
/****** Object:  StoredProcedure [dbo].[actualizarSalariosNeto]    Script Date: 10/3/2022 7:21:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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

-- Procedure 5
GO
/****** Object:  StoredProcedure [dbo].[borrar_beneficio]    Script Date: 10/3/2022 7:21:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[borrar_beneficio] @nombre varchar(30), @nombrePuesto varchar(30), @fechaAnalisis DateTime, @monto int, @plazas int
AS
DELETE FROM BENEFICIO WHERE nombre=@nombre and nombrePuesto=@nombrePuesto and fechaAnalisis=@fechaAnalisis and monto=@monto and cantidadPlazas=@plazas

-- Procedure 6
GO
/****** Object:  StoredProcedure [dbo].[eliminarGastoFijo]    Script Date: 10/3/2022 7:23:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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

-- Procedure 7
GO
/****** Object:  StoredProcedure [dbo].[insertarConfiguracionAnalisis]    Script Date: 10/3/2022 7:23:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   procedure [dbo].[insertarConfiguracionAnalisis](
	@fechaAnalisis datetime,
	@procentajeSS decimal(5,2),
	@procentajePL decimal(5,2),
	@moneda varchar(20) = null,
	@tipoCambio varchar(20) = null
)
as 
begin 

	update CONFIGURACION 
	set porcentajeSS= @procentajeSS, porcentajePL = @procentajePL , moneda = @moneda, tipoCambio = @tipoCambio
	where CONFIGURACION.fechaAnalisis=@fechaAnalisis
end

-- Procedure 8
GO
/****** Object:  StoredProcedure [dbo].[insertarGastoFijo]    Script Date: 10/3/2022 7:23:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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

-- Procedure 9
GO
/****** Object:  StoredProcedure [dbo].[ObtenerConfigAnalisis]    Script Date: 10/3/2022 7:24:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[ObtenerConfigAnalisis](
	@fechaAnalisis Datetime
)
as 
begin 
	select fechaAnalisis, ISNULL(tipoCambio,0) as tipoCambio, moneda, tipoNegocio, ISNULL(porcentajeSS, 0) as porcentajeSS, ISNULL(porcentajePL,0) as porcentajePL from Configuracion where fechaAnalisis = @fechaAnalisis
end

-- Procedure 10
GO
/****** Object:  StoredProcedure [dbo].[ObtenerPuestos]    Script Date: 10/3/2022 7:25:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[ObtenerPuestos] (
	@fechaAnalisis datetime
)
as
begin 
SELECT * FROM PUESTO WHERE fechaAnalisis=@fechaAnalisis order by orden ASC
end

-- Procedure 11
GO
/****** Object:  StoredProcedure [dbo].[obtGastosFijosList]    Script Date: 10/3/2022 7:25:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[obtGastosFijosList] (@fechaAnalisis DATETIME) AS
BEGIN
	SELECT * FROM GASTO_FIJO
	WHERE GASTO_FIJO.fechaAnalisis = @fechaAnalisis
	ORDER BY GASTO_FIJO.orden
END

-- Procedure 12
GO
/****** Object:  StoredProcedure [dbo].[obtNombreNegocio]    Script Date: 10/3/2022 7:25:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[obtNombreNegocio] (@fechaAnalisis DATETIME) AS
BEGIN
SELECT DISTINCT NEGOCIO.nombre 
FROM NEGOCIO JOIN ANALISIS ON NEGOCIO.id = ANALISIS.idNegocio 
WHERE ANALISIS.fechaCreacion = @fechaAnalisis
END

-- Procedure 13
GO
/****** Object:  StoredProcedure [dbo].[obtSumGastosFijos]    Script Date: 10/3/2022 7:25:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[obtSumGastosFijos] (@fechaAnalisis DATETIME) AS
BEGIN 
SELECT SUM(GASTO_FIJO.monto) totalMensual 
FROM GASTO_FIJO WHERE GASTO_FIJO.fechaAnalisis = @fechaAnalisis
END

-- Function 1
GO
/****** Object:  UserDefinedFunction [dbo].[convertTOdecimal]    Script Date: 10/3/2022 7:28:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[convertTOdecimal] (@num varchar(20))
RETURNS DEC(18,3) AS
BEGIN
	DECLARE @Result DEC(18,3)
	SELECT @Result = CAST(@num AS DECIMAL(18,3))
	RETURN @Result
END

-- Function 2
GO
/****** Object:  UserDefinedFunction [dbo].[obtGastoPrestaciones]    Script Date: 10/3/2022 7:28:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[obtGastoPrestaciones] (@fechaAnalisis DATETIME, @porcentaje DEC(6,3))
RETURNS DEC(18,3) AS
BEGIN
	DECLARE @Result DEC(18,3)
	SELECT @Result = dbo.obtSumSalarios (@fechaAnalisis) * @porcentaje
	RETURN @Result
END

-- Function 3
GO
/****** Object:  UserDefinedFunction [dbo].[obtGastoSeguroSocial]    Script Date: 10/3/2022 7:28:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[obtGastoSeguroSocial] (@fechaAnalisis DATETIME, @porcentaje DEC(6,3))
RETURNS DEC(18,3) AS
BEGIN
	DECLARE @Result DEC(18,3)
	SELECT @Result = dbo.obtSumSalarios (@fechaAnalisis) * @porcentaje
	RETURN @Result
END

-- Function 4
GO
/****** Object:  UserDefinedFunction [dbo].[obtSumSalarios]    Script Date: 10/3/2022 7:28:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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

-- Function 5
GO
/****** Object:  UserDefinedFunction [dbo].[obtTotalBeneficios]    Script Date: 10/3/2022 7:29:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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

-- Function 6
GO
/****** Object:  UserDefinedFunction [dbo].[obtTotalSalariosNeto]    Script Date: 10/3/2022 7:29:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[obtTotalSalariosNeto] (@fechaAnalisis DATETIME, @seguroSocial DEC(6,3), @prestaciones DEC(6,3))
RETURNS DEC(18,3) AS
BEGIN
	DECLARE @Result DEC(18,3)
	SELECT @Result = dbo.obtSumSalarios (@fechaAnalisis) - dbo.obtGastoSeguroSocial(@fechaAnalisis,@seguroSocial) - dbo.obtGastoPrestaciones(@fechaAnalisis,@prestaciones)
	RETURN @Result
END


-- Procedure - permite obtener un negocio a partir de la fecha de un análisis
GO 
CREATE PROCEDURE ObtenerNegocioDeAnalisis (@fechaAnalisis DATETIME)
AS
BEGIN
    SELECT N.id, N.nombre, N.correoUsuario, N.FechaCreacion 
    FROM NEGOCIO AS N JOIN ANALISIS AS A ON N.id = A.idNegocio
    WHERE A.fechaCreacion = @fechaAnalisis
END
