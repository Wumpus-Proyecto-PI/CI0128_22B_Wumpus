
----------------------------------------------------
-- procedures
----------------------------------------------------

-- Wilmer Araya Rivas | B80538
GO
/****** Object:  StoredProcedure [dbo].[actualizarBeneficios]    Script Date: 28/11/2022 11:01:31 PM ******/
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

-- Gabriel Bonilla Rivera | C01252
GO
/****** Object:  StoredProcedure [dbo].[ActualizarGananciaMensual]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ActualizarGananciaMensual] @ganancia decimal, @fecha DateTime
AS
UPDATE ANALISIS 
SET gananciaMensual = @ganancia
WHERE ANALISIS.fechaCreacion = @fecha

-- Wilmer Araya Rivas | B80538
GO
/****** Object:  StoredProcedure [dbo].[actualizarGastoPrestaciones]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[actualizarGastoPrestaciones](@fechaAnalisis DATETIME, @porcentaje varchar(20))
AS
BEGIN
	SET @porcentaje = REPLACE(@porcentaje, ',', '.')
	IF EXISTS (SELECT * FROM GASTO_FIJO G JOIN ANALISIS A on G.fechaAnalisis = A.fechaCreacion WHERE G.nombre = 'Prestaciones laborales' AND G.fechaAnalisis = @fechaAnalisis)
		UPDATE GASTO_FIJO 
		SET monto =  dbo.obtGastoPrestaciones (@fechaAnalisis, dbo.convertTOdecimal (@porcentaje))
		WHERE GASTO_FIJO.nombre = 'Prestaciones laborales' AND GASTO_FIJO.fechaAnalisis = @fechaAnalisis 
	ELSE
		INSERT INTO GASTO_FIJO (nombre, fechaAnalisis, monto, orden) VALUES ('Prestaciones laborales', @fechaAnalisis, dbo.obtGastoPrestaciones (@fechaAnalisis, dbo.convertTOdecimal (@porcentaje)), 3)
END

-- Wilmer Araya Rivas | B80538
GO
/****** Object:  StoredProcedure [dbo].[actualizarGastoSeguroSocial]    Script Date: 28/11/2022 11:01:31 PM ******/
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

-- Wilmer Araya Rivas | B80538
GO
/****** Object:  StoredProcedure [dbo].[actualizarSalariosNeto]    Script Date: 28/11/2022 11:01:31 PM ******/
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

-- Christopher Perez Blanco | C05881
GO
/****** Object:  StoredProcedure [dbo].[AgregarComponente]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[AgregarComponente]
(@nombreComponente varchar(30), @nombreProducto varchar(30), @fechaAnalisis datetime, @monto decimal(18, 2),  @cantidad decimal(18, 2), @unidad varchar(30))
as
Begin
	insert into Componente values(@nombreComponente, @nombreProducto, @fechaAnalisis,@monto, @cantidad, @unidad)
End
GO
/****** Object:  StoredProcedure [dbo].[AgregarEgresoMes]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AgregarEgresoMes] @fechaAnalisis datetime, @nombreMes varchar(10), @tipo bit, @monto decimal(18,2)
AS
	insert into EGRESO values (@nombreMes,@fechaAnalisis, @tipo, @monto);
GO
/****** Object:  StoredProcedure [dbo].[AgregarIngresoMes]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AgregarIngresoMes] @fechaAnalisis datetime, @nombreMes varchar(10), @tipo bit, @monto decimal(18,2)
AS
	insert into INGRESO values (@nombreMes,@fechaAnalisis, @tipo, @monto);

-- Gabriel Bonilla Rivera | C01252
GO
/****** Object:  StoredProcedure [dbo].[borrar_beneficio]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[borrar_beneficio] @nombre varchar(30), @nombrePuesto varchar(30), @fechaAnalisis DateTime, @monto int, @plazas int
AS
DELETE FROM BENEFICIO WHERE nombre=@nombre and nombrePuesto=@nombrePuesto and fechaAnalisis=@fechaAnalisis and monto=@monto and cantidadPlazas=@plazas

-- Christopher Perez Blanco | C05881
GO
/****** Object:  StoredProcedure [dbo].[BorrarComponente]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[BorrarComponente] 
(@nombreComponente varchar(30), @nombreProducto varchar(30), @fechaAnalisis datetime)
as
Begin
	Delete from COMPONENTE where COMPONENTE.nombreComponente=@nombreComponente and COMPONENTE.nombreProducto=@nombreProducto and COMPONENTE.fechaAnalisis=@fechaAnalisis
End
GO
/****** Object:  StoredProcedure [dbo].[BorrarNegocio]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   procedure [dbo].[BorrarNegocio](
	@idNegocio int
)
as
begin 
	set transaction isolation level read committed
	begin transaction BorrarNegocio
	begin try
		DELETE FROM NEGOCIO WHERE id = @idNegocio
		commit transaction BorrarNegocio
	end try
	begin catch
		rollback transaction BorrarNegocio
	end catch
end

-- Gabriel Bonilla Rivera | C01252
GO
/****** Object:  StoredProcedure [dbo].[CostoVariablePorLoteComponente]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CostoVariablePorLoteComponente] @nombreComponente varchar(30), @nombreProducto varchar(30), @fechaAnalisis datetime, @result decimal(18,2) output
AS
set @result = (select C.cantidad * C.monto
from Componente as C
where C.nombreComponente = @nombreComponente and C.nombreProducto = @nombreProducto and C.fechaAnalisis = @fechaAnalisis)

-- Gabriel Bonilla Rivera | C01252
GO
/****** Object:  StoredProcedure [dbo].[CostoVariablePorUnidadComponente]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CostoVariablePorUnidadComponente] @nombreComponente varchar(30), @nombreProducto varchar(30), @fechaAnalisis datetime, @resultado decimal(18,2) output
AS
declare @lote int 
set @lote = (select lote from PRODUCTO as P where P.fechaAnalisis = @fechaAnalisis and P.nombre = @nombreProducto)
declare @CostoVarLoteComponente decimal(18,2)
exec CostoVariablePorLoteComponente @nombreComponente = @nombreComponente, @nombreProducto = @nombreProducto, @fechaAnalisis = @fechaAnalisis, @result = @CostoVarLoteComponente output
set @resultado = @CostoVarLoteComponente / @lote

-- Gabriel Bonilla Rivera | C01252
GO
/****** Object:  StoredProcedure [dbo].[CostoVariableTotal]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
/****** Object:  StoredProcedure [dbo].[crearEgresosPorMes]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[crearEgresosPorMes](@fechaAnalisis DATETIME)
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM EGRESO E JOIN MES M on E.fechaAnalisis = M.fechaAnalisis WHERE E.fechaAnalisis = @fechaAnalisis)
        INSERT INTO EGRESO(mes, fechaAnalisis, tipo)
        VALUES
            ('Mes 1', @fechaAnalisis, 'contado'), ('Mes 2', @fechaAnalisis, 'contado'), ('Mes 3', @fechaAnalisis, 'contado'),
            ('Mes 4', @fechaAnalisis, 'contado'), ('Mes 5', @fechaAnalisis, 'contado'), ('Mes 6', @fechaAnalisis, 'contado'),
            ('Mes 1', @fechaAnalisis, 'credito'), ('Mes 2', @fechaAnalisis, 'credito'), ('Mes 3', @fechaAnalisis, 'credito'),
            ('Mes 4', @fechaAnalisis, 'credito'), ('Mes 5', @fechaAnalisis, 'credito'), ('Mes 6', @fechaAnalisis, 'credito'),
            ('Mes 1', @fechaAnalisis, 'otros'), ('Mes 2', @fechaAnalisis, 'otros'), ('Mes 3', @fechaAnalisis, 'otros'),
            ('Mes 4', @fechaAnalisis, 'otros'), ('Mes 5', @fechaAnalisis, 'otros'), ('Mes 6', @fechaAnalisis, 'otros');
END
GO
/****** Object:  StoredProcedure [dbo].[crearIngresosPorMes]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[crearIngresosPorMes](@fechaAnalisis DATETIME)
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM INGRESO I JOIN MES M on I.fechaAnalisis = M.fechaAnalisis WHERE I.fechaAnalisis = @fechaAnalisis)
        INSERT INTO INGRESO (mes, fechaAnalisis, tipo)
        VALUES
            ('Mes 1', @fechaAnalisis, 'contado'), ('Mes 2', @fechaAnalisis, 'contado'), ('Mes 3', @fechaAnalisis, 'contado'),
            ('Mes 4', @fechaAnalisis, 'contado'), ('Mes 5', @fechaAnalisis, 'contado'), ('Mes 6', @fechaAnalisis, 'contado'),
            ('Mes 1', @fechaAnalisis, 'credito'), ('Mes 2', @fechaAnalisis, 'credito'), ('Mes 3', @fechaAnalisis, 'credito'),
            ('Mes 4', @fechaAnalisis, 'credito'), ('Mes 5', @fechaAnalisis, 'credito'), ('Mes 6', @fechaAnalisis, 'credito'),
            ('Mes 1', @fechaAnalisis, 'otros'), ('Mes 2', @fechaAnalisis, 'otros'), ('Mes 3', @fechaAnalisis, 'otros'),
            ('Mes 4', @fechaAnalisis, 'otros'), ('Mes 5', @fechaAnalisis, 'otros'), ('Mes 6', @fechaAnalisis, 'otros');
END
GO
/****** Object:  StoredProcedure [dbo].[crearMesesDeAnalisis]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[crearMesesDeAnalisis](@fechaAnalisis DATETIME)
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM MES M JOIN ANALISIS A on M.fechaAnalisis = A.fechaCreacion WHERE M.fechaAnalisis = @fechaAnalisis)
		INSERT INTO MES (nombre, fechaAnalisis, inversionPorMes)
		VALUES
			('Mes1', @fechaAnalisis, 0.0),
			('Mes2', @fechaAnalisis, 0.0),
			('Mes3', @fechaAnalisis, 0.0),
			('Mes4', @fechaAnalisis, 0.0),
			('Mes5', @fechaAnalisis, 0.0),
			('Mes6', @fechaAnalisis, 0.0);
END
GO
/****** Object:  StoredProcedure [dbo].[EliminarAnalisis]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[EliminarAnalisis] @fechaCreacion datetime
AS
DELETE FROM ANALISIS WHERE fechaCreacion=@fechaCreacion 
GO
/****** Object:  StoredProcedure [dbo].[eliminarGastoFijo]    Script Date: 28/11/2022 11:01:31 PM ******/
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

-- Fabian Orozco Chaves | B95690
GO
/****** Object:  StoredProcedure [dbo].[EliminarGastoInicial]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[EliminarGastoInicial] (@fechaAnalisis datetime, @nombre varchar(30)) as
begin
	Delete from INVERSION_INICIAL where nombre=@nombre and fechaAnalisis=@fechaAnalisis
end

-- Christopher Perez Blanco | C05881
GO
/****** Object:  StoredProcedure [dbo].[EliminarProducto]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[EliminarProducto] (@nombreProducto varchar(30), @fechaAnalisis datetime) as
Begin 
Delete from Producto where nombre=@nombreProducto and fechaAnalisis=@fechaAnalisis
end 


-- Fabian Orozco Chaves | B95690
GO
/****** Object:  StoredProcedure [dbo].[IngresarGastoInicial]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[IngresarGastoInicial] ( @fechaAnalisis datetime, @nombre varchar(30), @valor decimal(18,2)) as
begin
	SET @valor = REPLACE(@valor, ',', '.')
	insert into INVERSION_INICIAL values (@fechaAnalisis, @nombre, dbo.convertTOdecimal(@valor))
end

-- se hizo en conjunto con todos los integrantes
GO
/****** Object:  StoredProcedure [dbo].[IngresarNegocio]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   procedure [dbo].[IngresarNegocio](
	@nextID int,
	@nombre varchar(30),
	@idUsuario nvarchar(450),
	@fechaCreacion datetime
)
as
begin 
	set transaction isolation level read uncommitted
	begin try
		begin transaction IngresarNegocio
		INSERT INTO Negocio (ID,Nombre,idUsuario,FechaCreacion) VALUES (@nextID, @nombre, @idUsuario, @fechaCreacion);
		commit transaction IngresarNegocio
	end try
	begin catch
		rollback transaction IngresarNegocio
	end catch
end

-- Daniel Escobar Giraldo | C02748
GO
/****** Object:  StoredProcedure [dbo].[insertarConfiguracionAnalisis]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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

-- Fabian Orozco Chaves | B95690
GO
/****** Object:  StoredProcedure [dbo].[insertarGastoFijo]    Script Date: 28/11/2022 11:01:31 PM ******/
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

-- Christopher Perez Blanco | C05881
GO
/****** Object:  StoredProcedure [dbo].[InsertarProducto]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[InsertarProducto] 
(@nombreProducto varchar(30), @nombreAnterior varchar(30), @fechaAnalisis datetime, @lote int, 
@porcentajeDeVentas decimal(18,2), @precio decimal(18,2), @costoVariable decimal(18, 2), @comisionDeVentas decimal(18, 2)) as 
begin
if exists (Select PRODUCTO.nombre from Producto 
where @nombreAnterior=PRODUCTO.nombre and @fechaAnalisis=PRODUCTO.fechaAnalisis)
	begin 
		update Producto set nombre=@nombreProducto, lote=@lote, porcentajeDeVentas=@porcentajeDeVentas, precio=@precio, costoVariable=@costoVariable, comisionDeVentas=@comisionDeVentas
		where nombre=@nombreAnterior and fechaAnalisis=@fechaAnalisis
	end
else 
	begin
		insert into Producto values(@nombreProducto, @fechaAnalisis, @lote, @porcentajeDeVentas, @precio, @costoVariable, @comisionDeVentas)
	end
end

-- Christopher Perez Blanco | C05881
GO
/****** Object:  StoredProcedure [dbo].[ObtenerComponentes]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[ObtenerComponentes] (@nombreProducto varchar(30), @fechaAnalisis datetime)
as 
Begin
SELECT * from COMPONENTE where nombreProducto=@nombreProducto and fechaAnalisis=@fechaAnalisis
End

-- Daniel Escobar Giraldo | C02748
GO
/****** Object:  StoredProcedure [dbo].[ObtenerConfigAnalisis]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[ObtenerConfigAnalisis](
	@fechaAnalisis Datetime
)
as 
begin 
	select fechaAnalisis, tipoNegocio, ISNULL(porcentajeSS, 0) as porcentajeSS, ISNULL(porcentajePL,0) as porcentajePL from Configuracion where fechaAnalisis = @fechaAnalisis
end
GO
/****** Object:  StoredProcedure [dbo].[ObtenerEgresosMes]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ObtenerEgresosMes] @fechaAnalisis datetime , @nombreMes varchar(10)
as 
select * from EGRESO where fechaAnalisis = @fechaAnalisis and mes = @nombreMes

-- Fabian Orozco Chaves | B95690
GO
/****** Object:  StoredProcedure [dbo].[ObtenerGastosIniciales]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[ObtenerGastosIniciales] ( @fechaAnalisis datetime ) as
begin
	SELECT * FROM INVERSION_INICIAL WHERE fechaAnalisis=@fechaAnalisis order by nombre ASC
end
GO
/****** Object:  StoredProcedure [dbo].[ObtenerIngresosMes]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ObtenerIngresosMes] @fechaAnalisis datetime , @nombreMes varchar(10)
as 
select * from INGRESO where fechaAnalisis = @fechaAnalisis and mes = @nombreMes

-- Daniel Escobar Giraldo | C02748
GO
/****** Object:  StoredProcedure [dbo].[ObtenerNegocioDeAnalisis]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ObtenerNegocioDeAnalisis] (@fechaAnalisis DATETIME)
AS
BEGIN
    SELECT N.id, N.nombre, N.idUsuario, N.FechaCreacion 
    FROM NEGOCIO AS N JOIN ANALISIS AS A ON N.id = A.idNegocio
    WHERE A.fechaCreacion = @fechaAnalisis
END

-- Se hizo en llamada con todos los integrantes
GO
/****** Object:  StoredProcedure [dbo].[ObtenerNegocios]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   procedure [dbo].[ObtenerNegocios](
	@idUsuario varchar(450)
)
as
begin 
	set transaction isolation level read committed
	begin try
		begin transaction ObtenerNegociosUsuario
		SELECT * FROM Negocio where idUsuario = @idUsuario ORDER BY FechaCreacion DESC
		commit transaction ObtenerNegociosUsuario
	end try
	begin catch
		rollback transaction ObtenerNegociosUsuario
	end catch
end

-- Wilmer Araya Rivas | B80538
GO
/****** Object:  StoredProcedure [dbo].[ObtenerPorcentajeDeVentasTotal]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ObtenerPorcentajeDeVentasTotal] (@fechaAnalisis Datetime)
AS
BEGIN
	SELECT SUM(porcentajeDeVentas) totalPorcentajeVentas
	FROM PRODUCTO
	WHERE fechaAnalisis = @fechaAnalisis
END

-- Wilmer Araya Rivas | B80538
GO
/****** Object:  StoredProcedure [dbo].[ObtenerPorcentajeVentas]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ObtenerPorcentajeVentas] (@fechaAnalisis Datetime, @nombreProducto varchar(30))
AS
BEGIN
	SELECT porcentajeDeVentas
	FROM PRODUCTO
	WHERE fechaAnalisis = @fechaAnalisis AND nombre = @nombreProducto
END


-- Christopher Perez Blanco | C05881
GO
/****** Object:  StoredProcedure [dbo].[ObtenerProductos]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[ObtenerProductos] (
	@fechaAnalisis datetime
)
as
begin 
SELECT * FROM PRODUCTO WHERE fechaAnalisis=@fechaAnalisis ORDER BY nombre ASC
end

-- Daniel Escobar Giraldo | C02748
GO
/****** Object:  StoredProcedure [dbo].[ObtenerPuestos]    Script Date: 28/11/2022 11:01:31 PM ******/
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

-- Fabian Orozco Chaves | B95690
GO
/****** Object:  StoredProcedure [dbo].[ObtenerSumInversionInicial]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[ObtenerSumInversionInicial] ( @fechaAnalisis Datetime) as
begin
	Select SUM(valor) total from INVERSION_INICIAL where fechaAnalisis = @fechaAnalisis
end
GO
/****** Object:  StoredProcedure [dbo].[obtGastosFijosList]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[obtGastosFijosList] (@fechaAnalisis DATETIME) AS
BEGIN
	SELECT * FROM GASTO_FIJO
	WHERE GASTO_FIJO.fechaAnalisis = @fechaAnalisis
	ORDER BY GASTO_FIJO.nombre ASC
END

-- Fabian Orozco Chaves | B95690
GO
/****** Object:  StoredProcedure [dbo].[obtNombreNegocio]    Script Date: 28/11/2022 11:01:31 PM ******/
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

-- Fabian Orozco Chaves | B95690
GO
/****** Object:  StoredProcedure [dbo].[obtNombreNegocioPorID]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[obtNombreNegocioPorID] (@IDNegocio INT) as
begin
	SELECT negocio.nombre from dbo.negocio where negocio.ID = @IDNegocio
end

-- Fabian Orozco Chaves | B95690
GO
/****** Object:  StoredProcedure [dbo].[obtSumGastosFijos]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[obtSumGastosFijos] (@fechaAnalisis DATETIME) AS
BEGIN 
SELECT SUM(GASTO_FIJO.monto) totalAnual
FROM GASTO_FIJO WHERE GASTO_FIJO.fechaAnalisis = @fechaAnalisis
END

----------------------------------------------------
-- triggers
----------------------------------------------------

GO
/****** Object:  Trigger [dbo].[CrearMesesAnalisis]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE trigger [dbo].[CrearMesesAnalisis]
on [dbo].[ANALISIS] after insert
as
declare  @fechaAnalisis datetime
select @fechaAnalisis = i.fechaCreacion from inserted as i

insert into mes values ('Mes 1', @fechaAnalisis, 0.0);
insert into mes values ('Mes 2', @fechaAnalisis, 0.0);
insert into mes values ('Mes 3', @fechaAnalisis, 0.0);
insert into mes values ('Mes 4', @fechaAnalisis, 0.0);
insert into mes values ('Mes 5', @fechaAnalisis, 0.0);
insert into mes values ('Mes 6', @fechaAnalisis, 0.0);
GO
ALTER TABLE [dbo].[ANALISIS] ENABLE TRIGGER [CrearMesesAnalisis]


-- Gabriel Bonilla Rivera | C01252
GO
/****** Object:  Trigger [dbo].[ActualizarCostoVariable_DeleteComponente]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
		fetch next from c2 into @nomProd, @fecha
	end
close c2
deallocate c2
GO
ALTER TABLE [dbo].[COMPONENTE] ENABLE TRIGGER [ActualizarCostoVariable_DeleteComponente]

-- Gabriel Bonilla Rivera | C01252
GO
/****** Object:  Trigger [dbo].[ActualizarCostoVariable_UpdateComponente]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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


-- Gabriel Bonilla Rivera | C01252
GO
/****** Object:  Trigger [dbo].[ActualizarCostoVariable_UpdateLote]    Script Date: 28/11/2022 11:01:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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


----------------------------------------------------
-- funciones
----------------------------------------------------

/****** Object:  UserDefinedFunction [dbo].[convertTOdecimal]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON

-- Wilmer Araya Rivas | B80538
GO
CREATE FUNCTION [dbo].[convertTOdecimal] (@num varchar(20))
RETURNS DEC(18,3) AS
BEGIN
	DECLARE @Result DEC(18,3)
	SELECT @Result = CAST(@num AS DECIMAL(18,3))
	RETURN @Result
END

-- Wilmer Araya Rivas | B80538
GO
/****** Object:  UserDefinedFunction [dbo].[obtGastoPrestaciones]    Script Date: 28/11/2022 11:01:31 PM ******/
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

-- Wilmer Araya Rivas | B80538
GO
/****** Object:  UserDefinedFunction [dbo].[obtGastoSeguroSocial]    Script Date: 28/11/2022 11:01:31 PM ******/
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
GO
/****** Object:  UserDefinedFunction [dbo].[obtSumSalarios]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON


-- Wilmer Araya Rivas | B80538
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
	RETURN @totalSalarios * 12
END
GO
/****** Object:  UserDefinedFunction [dbo].[obtTotalBeneficios]    Script Date: 28/11/2022 11:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON

-- Wilmer Araya Rivas | B80538
GO
CREATE FUNCTION [dbo].[obtTotalBeneficios] (@fechaAnalisis DATETIME)
RETURNS DEC(18,3) AS
BEGIN
	DECLARE @Result DEC(18,3)
		SELECT @Result = SUM(PUESTO.beneficios * PUESTO.cantidadPlazas)
		FROM Puesto
		WHERE Puesto.fechaAnalisis = @fechaAnalisis
	RETURN @Result *12
END


-- Wilmer Araya Rivas | B80538
GO
/****** Object:  UserDefinedFunction [dbo].[obtTotalSalariosNeto]    Script Date: 28/11/2022 11:01:31 PM ******/
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