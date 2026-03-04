USE master
GO
CREATE DATABASE ZombieHordeDefenseSystem
GO
USE ZombieHordeDefenseSystem
GO
-- TABLAS
CREATE TABLE TipoZombie(
	TipoZombieId UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
	NombreTipoZombie VARCHAR(20) UNIQUE NOT NULL,
)
GO
CREATE TABLE Zombie(
	ZombieId UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
	TipoZombieId UNIQUEIDENTIFIER NOT NULL REFERENCES TipoZombie(TipoZombieId),
	TiempoDisparo INT NOT NULL CHECK(TiempoDisparo >= 0) DEFAULT 0,
	BalasNecesarias INT NOT NULL CHECK(BalasNecesarias >= 0) DEFAULT 0,
	NivelAmenaza INT NOT NULL CHECK(NivelAmenaza BETWEEN 1 AND 10) DEFAULT 1,
	Puntaje INT NOT NULL CHECK(Puntaje >= 0) DEFAULT 0,
)
GO
CREATE TABLE Simulacion(
	SimulacionId UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
	Fecha DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	TiempoDisponible INT NOT NULL CHECK(TiempoDisponible >= 0) DEFAULT 0,
	BalasDisponibles INT NOT NULL CHECK(BalasDisponibles >= 0) DEFAULT 0,
)
GO
CREATE TABLE ZombieEliminado(
	ZombieEliminadoId UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
	ZombieId UNIQUEIDENTIFIER NOT NULL REFERENCES Zombie(ZombieId),
	SimulacionId UNIQUEIDENTIFIER NOT NULL REFERENCES Simulacion(SimulacionId),
	PuntosObtenidos INT NOT NULL CHECK(PuntosObtenidos >= 0) DEFAULT 0,
	Timestamp DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
)
GO
-- TYPE
CREATE TYPE ZombieEliminadoType AS TABLE(
	ZombieId UNIQUEIDENTIFIER NOT NULL,
	PuntosObtenidos INT NOT NULL CHECK(PuntosObtenidos > 0) DEFAULT 0
)
GO
-- STORED PROCEDURE
CREATE PROCEDURE SP_OBTENER_LISTA_DE_ZOMBIES
AS
BEGIN
	SELECT 
		Z.ZombieId, 
		TP.NombreTipoZombie,
		Z.TiempoDisparo, 
		Z.BalasNecesarias, 
		Z.NivelAmenaza, 
		Z.Puntaje
	FROM Zombie Z
	INNER JOIN TipoZombie TP
	ON TP.TipoZombieId = Z.TipoZombieId
	ORDER BY Z.NivelAmenaza DESC
END
GO
CREATE PROCEDURE SP_REGISTRAR_SIMULACION
	@SimulacionId UNIQUEIDENTIFIER OUTPUT,
	@TiempoDisponible INT,
	@BalasDisponibles INT
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			SET @SimulacionId = NEWID()			
			INSERT INTO Simulacion
			(
				SimulacionId, 
				TiempoDisponible, 
				BalasDisponibles
			)
			VALUES
			(
				@SimulacionId,
				@TiempoDisponible,
				@BalasDisponibles
			)
		COMMIT;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK;
		THROW;
	END CATCH
END
GO
CREATE PROCEDURE SP_REGISTRAR_ZOMBIES_ELIMINADOS_POR_SIMULACION
	@SimulacionId UNIQUEIDENTIFIER,
	@Zombies ZombieEliminadoType READONLY
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			INSERT INTO ZombieEliminado
			(
				ZombieEliminadoId, 
				ZombieId, 
				SimulacionId, 
				PuntosObtenidos
			)
			SELECT 
				NEWID(), 
				ZombieId, 
				@SimulacionId, 
				PuntosObtenidos
			FROM @Zombies
		COMMIT;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK;
		THROW;
	END CATCH
END
GO
CREATE PROCEDURE SP_CONSULTAR_ZOMBIES_ELIMINADOS_POR_SIMULACION '59F25214-91F1-484F-BB83-172E585BD03A'
	@SimulacionId UNIQUEIDENTIFIER
AS
BEGIN
	SELECT 
		TP.NombreTipoZombie, Z.NivelAmenaza, COUNT(ZE.ZombieEliminadoId) [TotalEliminados]
	FROM Simulacion S
	INNER JOIN ZombieEliminado ZE ON ZE.SimulacionId = S.SimulacionId
	INNER JOIN Zombie Z ON Z.ZombieId = ZE.ZombieId
	INNER JOIN TipoZombie TP ON TP.TipoZombieId = Z.TipoZombieId
	WHERE S.SimulacionId = @SimulacionId
	GROUP BY TP.NombreTipoZombie, Z.NivelAmenaza ORDER BY Z.NivelAmenaza DESC
END
GO
-- INSERT
INSERT INTO TipoZombie (TipoZombieId, NombreTipoZombie)
VALUES ('59A4090A-D784-4D09-9E6E-328280E9F1AA', 'Caminante'),
	   ('06170816-2E1A-4C35-8AC7-675B5ECBB1A2', 'Veloz'),
	   ('3F2719C7-8CAD-47BB-B1A7-E54DCD83C518', 'Nadador'),
	   ('F729EEED-0497-48B1-B5A2-7C7997737234', 'Aereo'),
	   ('7CCEA611-0C90-4D47-93AC-9BE4B4AE9310', 'Venenozo')
GO
INSERT INTO Zombie(ZombieId, TipoZombieId, TiempoDisparo, BalasNecesarias, NivelAmenaza, Puntaje)
VALUES (NEWID(), '59A4090A-D784-4D09-9E6E-328280E9F1AA', 5, 1, 1, 10),
	   (NEWID(), '06170816-2E1A-4C35-8AC7-675B5ECBB1A2', 10, 6, 2, 15),
	   (NEWID(), '3F2719C7-8CAD-47BB-B1A7-E54DCD83C518', 15, 11, 4, 20),
	   (NEWID(), 'F729EEED-0497-48B1-B5A2-7C7997737234', 20, 16, 6, 25),
	   (NEWID(), '7CCEA611-0C90-4D47-93AC-9BE4B4AE9310', 25, 21, 10, 20)
