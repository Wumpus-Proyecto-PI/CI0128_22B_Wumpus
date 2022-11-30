use Wumpus_TEST
-- las pruebas se realizaron en Wumpus_TEST con dos usuarios de testing:
-- Usuario 1: email: indexuser1@gmail.com, id: 3676f6bb-cc05-43bc-966e-2842c6ba3e4c
-- Usuario 2: email: indexuser2@gmail.com, id: f94dccec-805d-4c6b-9d8f-ca1e7b77aebf

-- no se deben eliminar estos usuarios para evitar errores

/*
	Con un codigo C# ingresamos a cada usuario datos para poder realizar las pruebas.
	Este codigo se le mostro a la profesora en consulta.
*/

-- cantidad negocios del usuario 1
SELECT count(idUsuario) NegociosUsuario1 FROM Negocio
where idUsuario = '3676f6bb-cc05-43bc-966e-2842c6ba3e4c'

-- cantidad negocios del usuario 2
SELECT count(idUsuario) NegociosUsuario2 FROM Negocio
where idUsuario = 'f94dccec-805d-4c6b-9d8f-ca1e7b77aebf'


-- 1. Índice sobre la columna FechaCreacion de la tabla Negocio

-- indice para para hacer select de los negocios segun la fecha de creacion
-- la idea es que el dbms no tenga que hacer sort de la tabla segun la fecha
create index Negocio_FechaCreacion_DESC on Negocio(FechaCreacion desc) include (idUsuario)
drop index Negocio_FechaCreacion_DESC on Negocio

-- en nuestra aplicacion hacemos el siguiente select para desplegar los negocios de un usuario
-- estos los ordenamos segun la fecha de forma descendiente
SELECT * FROM Negocio
where idUsuario = 'f94dccec-805d-4c6b-9d8f-ca1e7b77aebf' order by FechaCreacion desc

-- select para correr en lote y poder ver si hubo mejoras de rendimiento.
SELECT * FROM Negocio

-- realizamos la misma prueba seleccionando solo la FechaCreacion
SELECT FechaCreacion FROM Negocio
where idUsuario = 'f94dccec-805d-4c6b-9d8f-ca1e7b77aebf' order by FechaCreacion desc

-- select para correr en lote y poder ver si hubo mejoras de rendimiento.
SELECT * FROM Negocio


-- 2. Índice sobre la columna idUsuario de la tabla Negocio

-- indice para para hacer select de los negocios segun el id del usuario
create index Negocio_idUsuario on Negocio(idUsuario) include (FechaCreacion)
drop index Negocio_idUsuario on Negocio

-- en nuestra aplicacion hacemos el siguiente select para desplegar los negocios de un usuario
-- estos los ordenamos segun la fecha de forma descendiente
SELECT * FROM Negocio
where idUsuario = 'f94dccec-805d-4c6b-9d8f-ca1e7b77aebf' order by FechaCreacion desc

-- select para correr en lote y poder ver si hubo mejoras de rendimiento.
SELECT * FROM Negocio


-- para vaciar facilmente los datos de los usuarios de prueba
delete from Negocio where idUsuario = '3676f6bb-cc05-43bc-966e-2842c6ba3e4c'
delete from Negocio where idUsuario = 'f94dccec-805d-4c6b-9d8f-ca1e7b77aebf'


-- 3. Índice sobre la columna fechaAnalisis de la tabla Puesto

/*
	fecha de analisis del usuario 1: 2022-11-30 15:41:39.807
	fecha de analisis del usuairo 2: 2022-11-30 15:42:02.040
*/

-- cantidad puestos del usuario 1
SELECT count(fechaAnalisis) CantidadPuestosUsuario1 FROM PUESTO WHERE fechaAnalisis='2022-11-30 15:41:39.807'

-- cantidad puestos del usuario 2
SELECT count(fechaAnalisis) CantidadPuestosUsuario2 FROM PUESTO WHERE fechaAnalisis='2022-11-30 15:42:02.040' 

-- este indice busca optimizar el select de los puestos de un analisis ya que fechaAnalisis es la condicion del where
create index Puesto_fechaAnalisis_asc on Puesto(fechaAnalisis ASC)
drop index Puesto_fechaAnalisis_asc on Puesto

-- en nuestra aplicacion hacemos el siguiente select para desplegar los puestos de un analisis
-- estos los ordenamos segun la fecha de forma ascendiente
SELECT * FROM PUESTO WHERE fechaAnalisis='2022-11-30 15:42:02.040'  order by orden ASC

-- select para correr en lote y poder ver si hubo mejoras de rendimiento.
select * 
from puesto


-- para vaciar facilmente los datos de los usuarios de prueba
delete from PUESTO where fechaAnalisis = '2022-11-25 23:35:27.493'
delete from PUESTO where fechaAnalisis = '2022-11-25 23:35:28.327'