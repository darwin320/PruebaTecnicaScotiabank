-- creacion de la base de datos

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Asesoftware')
BEGIN
    CREATE DATABASE Asesoftware;
    PRINT 'Base de datos Asesoftware creada.';
END
ELSE
BEGIN
    PRINT 'La base de datos Asesoftware ya existe.';
END
GO

-- Usar la base de datos Asesoftware
USE Asesoftware;
GO
-- Script de creacion de las tablas en la base de datos 

CREATE TABLE comercios (
    id_comercio INT PRIMARY KEY,
    nom_comercio VARCHAR(100) NOT NULL,
    aforo_maximo INT NOT NULL
);

CREATE TABLE servicios (
    id_servicio INT PRIMARY KEY,
    id_comercio INT NOT NULL,
    nom_servicio VARCHAR(100) NOT NULL,
    hora_apertura TIME NOT NULL,
    hora_cierre TIME NOT NULL,
    duracion INT NOT NULL,
    FOREIGN KEY (id_comercio) REFERENCES comercios(id_comercio)
);

CREATE TABLE turnos (
    id_turno INT PRIMARY KEY IDENTITY(1,1),
    id_servicio INT NOT NULL,
    fecha_turno DATE NOT NULL,
    hora_inicio TIME NOT NULL,
    hora_fin TIME NOT NULL,
    estado VARCHAR(20) NOT NULL,
    FOREIGN KEY (id_servicio) REFERENCES servicios(id_servicio)
);



-- Inserción de datos en la tabla `comercios`
INSERT INTO dbo.comercios (id_comercio, nom_comercio, aforo_maximo)
VALUES 
(1, 'Comercio A', 50),
(2, 'Comercio B', 100);

-- Inserción de datos en la tabla `servicios`
INSERT INTO dbo.servicios (id_servicio, id_comercio, nom_servicio, hora_apertura, hora_cierre, duracion)
VALUES 
(1, 1, 'Servicio 1', '09:00:00', '17:00:00', 30),  -- Comercio A, servicio de 30 minutos
(2, 1, 'Servicio 2', '10:00:00', '18:00:00', 60),  -- Comercio A, servicio de 60 minutos
(3, 2, 'Servicio 3', '08:00:00', '16:00:00', 45);  -- Comercio B, servicio de 45 minutos



-- creacion del procedimiento almacenado

CREATE PROCEDURE [dbo].[GenerarTurnos]
    @fecha_inicio DATETIME,
    @fecha_fin DATETIME,
    @id_servicio INT
AS
BEGIN
    DECLARE @hora_apertura TIME
    DECLARE @hora_cierre TIME
    DECLARE @duracion INT

    -- Obtener detalles del servicio
    SELECT 
        @hora_apertura = hora_apertura,
        @hora_cierre = hora_cierre,
        @duracion = duracion
    FROM servicios
    WHERE id_servicio = @id_servicio

    -- Asegurarse de que las fechas están en el rango correcto
    SET @fecha_inicio = CASE 
                            WHEN CAST(@fecha_inicio AS TIME) < @hora_apertura THEN CAST(CAST(@fecha_inicio AS DATE) AS DATETIME) + CAST(@hora_apertura AS DATETIME)
                            WHEN CAST(@fecha_inicio AS TIME) > @hora_cierre THEN NULL
                            ELSE @fecha_inicio
                        END

    SET @fecha_fin = CASE 
                            WHEN CAST(@fecha_fin AS TIME) > @hora_cierre THEN CAST(CAST(@fecha_fin AS DATE) AS DATETIME) + CAST(@hora_cierre AS DATETIME)
                            WHEN CAST(@fecha_fin AS TIME) < @hora_apertura THEN NULL
                            ELSE @fecha_fin
                        END

    IF @fecha_inicio IS NOT NULL AND @fecha_fin IS NOT NULL AND @fecha_inicio < @fecha_fin
    BEGIN
        DECLARE @fecha_actual DATE = CAST(@fecha_inicio AS DATE)
        DECLARE @hora_actual TIME = CAST(@fecha_inicio AS TIME)

        WHILE @fecha_actual <= CAST(@fecha_fin AS DATE)
        BEGIN
            SET @hora_actual = @hora_apertura

            WHILE @hora_actual < @hora_cierre AND (@fecha_actual < CAST(@fecha_fin AS DATE) OR @hora_actual < CAST(@fecha_fin AS TIME))
            BEGIN
                IF @fecha_actual > CAST(@fecha_inicio AS DATE) OR @hora_actual >= CAST(@fecha_inicio AS TIME)
                BEGIN
                    INSERT INTO turnos (id_servicio, fecha_turno, hora_inicio, hora_fin, estado)
                    VALUES (
                        @id_servicio,
                        @fecha_actual,
                        @hora_actual,
                        DATEADD(MINUTE, @duracion, CAST(@hora_actual AS DATETIME)),
                        'Disponible'
                    )
                END

                SET @hora_actual = CAST(DATEADD(MINUTE, @duracion, CAST(@hora_actual AS DATETIME)) AS TIME)
            END

            SET @fecha_actual = DATEADD(DAY, 1, @fecha_actual)
        END

        -- Seleccionar y devolver los turnos generados
        SELECT id_turno, id_servicio, fecha_turno, hora_inicio, hora_fin, estado
        FROM turnos
        WHERE id_servicio = @id_servicio
        AND fecha_turno BETWEEN CAST(@fecha_inicio AS DATE) AND CAST(@fecha_fin AS DATE)
    END
END




-- procedimientos almacenados para garantizar la seguridad del apiKey por medio de un cliente de acceso


SET ANSI_NULLS ON

GO
 
SET QUOTED_IDENTIFIER ON

GO
 
CREATE PROCEDURE [dbo].[procClienteSelect]

AS

BEGIN
	SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SELECT	idcliente,
			nombre,
			apikey,
			codigo_convenio
	FROM dbo.clientes
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 
CREATE PROCEDURE [dbo].[procClienteSelectByapikey]
(
	@@apikey	VARCHAR(50)
)
AS

BEGIN
	SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SELECT	idcliente,
			nombre,
			apikey,
			codigo_convenio
	FROM dbo.clientes
	WHERE apikey = @@apikey
END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 


CREATE PROCEDURE [dbo].[procClienteSelectById]
(
	@@idcliente	INT
)
AS

BEGIN
	SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SELECT	idcliente,
			nombre,
			apikey,
			codigo_convenio
	FROM dbo.clientes
	WHERE idcliente = @@idcliente
END

GO
 
 
 -- insercion de un cliente de prueba
 
USE [Asesoftware]
GO

INSERT INTO [dbo].[clientes] (nombre, apikey, codigo_convenio)
VALUES ('Cliente Prueba', '12345-ABCDE', 'CONV01');
GO
