from django.db import connection

SP_CREAR_FACTURA = """
CREATE OR ALTER PROCEDURE [dbo].[SP_CREAR_FACTURA]
    @descfac VARCHAR(100),
    @monto INT,
    @rutcli VARCHAR(20),
    @idprod INT,
    @nrofac INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO FACTURA (nrofac, fechafac, descfac, monto, rutcli, idprod)
    VALUES (@nrofac, GETDATE(), @descfac, @monto, @rutcli, @idprod);

END
"""

SP_CREAR_SOLICITUD_SERVICIO = """
CREATE OR ALTER PROCEDURE [dbo].[SP_CREAR_SOLICITUD_SERVICIO]
    @tiposol VARCHAR(100),
    @fechavisita DATETIME2,
    @descsol VARCHAR(400),
    @descfac VARCHAR(100),
    @monto INT,
    @rutcli VARCHAR(20),
    @idprod INT

AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @nrosol INT;
    DECLARE @ruttec VARCHAR(20);
    DECLARE @nrofac INT;

    SET @nrofac = (SELECT ISNULL(MAX(nrofac), 0) + 1 FROM FACTURA);
    SET @nrosol = (SELECT ISNULL(MAX(nrosol), 0) + 111 FROM SOLICITUDSERVICIO);

    SET @ruttec = (
        SELECT TOP 1 ruttec
        FROM SOLICITUDSERVICIO
        GROUP BY ruttec
        ORDER BY COUNT(*) ASC
    );

    SET @monto = CASE WHEN @tiposol = 'Instalación' THEN @monto ELSE 25000 END;
    SET @idprod = CASE WHEN @tiposol = 'Instalación' THEN @idprod ELSE 1 END;
    
    EXEC SP_CREAR_FACTURA
        @descfac = @descfac,
        @monto = @monto,
        @rutcli = @rutcli,
        @idprod = @idprod,
        @nrofac = @nrofac;
   
    INSERT INTO SOLICITUDSERVICIO (nrosol, tiposol, fechavisita, descsol, estadosol, nrofac, ruttec)
    VALUES (@nrosol, @tiposol, @fechavisita, @descsol, 'Aceptada', @nrofac, @ruttec)

    IF @tiposol = 'Instalación'
    BEGIN
        EXEC SP_CREAR_GUIA_DESPACHO
            @nrofac = @nrofac,
            @idprod = @idprod;
    END

END
"""

SP_CREAR_GUIA_DESPACHO = """
CREATE OR ALTER PROCEDURE [dbo].[SP_CREAR_GUIA_DESPACHO]
    @nrofac INT,
    @idprod INT

AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @nrogd INT;
    SET @nrogd = (SELECT ISNULL(MAX(nrogd), 0) + 11 FROM GUIADESPACHO);
    
    INSERT INTO GUIADESPACHO (nrogd, estadogd, nrofac, idprod)
    VALUES (@nrogd, 'En bodega', @nrofac, @idprod)

    UPDATE TOP (1) StockProducto
    SET nrofac = @nrofac
    WHERE idprod = @idprod AND nrofac IS NULL;

END
"""

SP_OBTENER_FACTURAS = """
CREATE OR ALTER PROCEDURE [dbo].[SP_OBTENER_FACTURAS]
	@tipousu VARCHAR(50),
	@rut     VARCHAR(20)
 
AS
BEGIN
	SET NOCOUNT ON;

	IF (@tipousu = 'Cliente')
		SELECT
        fac.nrofac,
        usucli.first_name + ' '  + usucli.last_name AS nomcli,
        fac.fechafac,
        fac.descfac,
        fac.monto,
        ss.nrosol,
        ss.estadosol
        FROM FACTURA fac
        INNER JOIN PerfilUsuario      percli ON fac.rutcli = percli.rut
        INNER JOIN auth_user          usucli ON percli.user_id =  usucli.id
        LEFT JOIN SolicitudServicio ss ON fac.nrofac = ss.nrofac
        WHERE rutcli = @rut
        ORDER BY fac.nrofac;

	IF (@tipousu = 'Administrador')
		SELECT
        fac.nrofac,
        usucli.first_name + ' '  + usucli.last_name AS nomcli,
        fac.fechafac,
        fac.descfac,
        fac.monto,
        ss.nrosol,
        ss.estadosol
        FROM FACTURA fac
        INNER JOIN PerfilUsuario      percli ON fac.rutcli = percli.rut
        INNER JOIN auth_user          usucli ON percli.user_id =  usucli.id
        LEFT JOIN SolicitudServicio ss ON fac.nrofac = ss.nrofac
        ORDER BY fac.nrofac;

END
"""

SP_OBTENER_GUIAS_DE_DESPACHO = """
CREATE OR ALTER PROCEDURE [dbo].[SP_OBTENER_GUIAS_DE_DESPACHO]
    @tipousu VARCHAR(50),
    @rut     VARCHAR(20)

AS
BEGIN

    SET NOCOUNT ON;
    
    IF (@tipousu = 'Cliente')
    BEGIN
        SELECT
            fac.nrofac,
            CASE 
                WHEN gd.nrogd IS NULL THEN 'No aplica'
                ELSE CAST(gd.nrogd AS VARCHAR(20))
            END AS nrogd,
            CASE 
                WHEN gd.estadogd IS NULL THEN 'No aplica'
                ELSE CAST(gd.estadogd AS VARCHAR(20)) 
            END AS estadogd
        FROM FACTURA fac
        LEFT JOIN GuiaDespacho gd ON fac.nrofac = gd.nrofac
        WHERE fac.rutcli = @rut
        ORDER BY fac.nrofac
    END
    ELSE IF (@tipousu = 'Administrador')
    BEGIN
        SELECT
            fac.nrofac,
            CASE 
                WHEN gd.nrogd IS NULL THEN 'No aplica'
                ELSE CAST(gd.nrogd AS VARCHAR(20))
            END AS nrogd,
            CASE 
                WHEN gd.estadogd IS NULL THEN 'No aplica'
                ELSE CAST(gd.estadogd AS VARCHAR(20)) 
            END AS estadogd
        FROM FACTURA fac
        LEFT JOIN GuiaDespacho gd ON fac.nrofac = gd.nrofac
        ORDER BY fac.nrofac
    END
    ELSE IF (@tipousu = 'Bodeguero')
    BEGIN
        SELECT
            gd.nrogd,
            p.nomprod,
            gd.estadogd,
			gd.nrofac,
			usucli.first_name + ' '  + usucli.last_name AS nomcli
        FROM GuiaDespacho gd
        LEFT JOIN Producto p ON p.idprod = gd.idprod
		LEFT JOIN Factura f ON f.idprod = gd.idprod
		INNER JOIN PerfilUsuario      percli ON f.rutcli = percli.rut
        INNER JOIN auth_user          usucli ON percli.user_id =  usucli.id
        ORDER BY gd.nrogd
    END

END
"""

SP_ACTUALIZAR_SOLICITUD_DE_SERVICIO = """
CREATE OR ALTER PROCEDURE [dbo].[SP_ACTUALIZAR_SOLICITUD_DE_SERVICIO]
    @accion VARCHAR(50),
    @nrosol INT,
    @fechavisita DATETIME

AS
BEGIN

    SET NOCOUNT ON;

    IF (@accion = 'aceptar')
    BEGIN

        UPDATE SolicitudServicio
            SET estadosol = 'Aceptada'
        WHERE nrosol = @nrosol;
        
    END
    ELSE IF (@accion = 'modificar')
    BEGIN

        UPDATE SolicitudServicio
            SET 
                fechavisita = @fechavisita,
                estadosol = 'Modificada'
        WHERE nrosol = @nrosol;
        
    END
    ELSE IF (@accion = 'cerrar')
    BEGIN

        UPDATE SolicitudServicio
            SET estadosol = 'Cerrada'
        WHERE nrosol = @nrosol;
        
    END

END
"""

SP_OBTENER_EQUIPOS_EN_BODEGA = """
CREATE OR ALTER PROCEDURE [dbo].[SP_OBTENER_EQUIPOS_EN_BODEGA]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT 
    p.idprod,
    p.nomprod,
    p.descprod,
    p.precio, 
    p.imagen, 
    COUNT(s.idprod) AS cantidad, 
    CASE 
        WHEN COUNT(s.idprod) = 0 
        THEN 'AGOTADO' 
        ELSE 'DISPONIBLE' 
    END AS disponibilidad
FROM
    Producto p
    LEFT JOIN (SELECT * FROM StockProducto WHERE nrofac IS NULL) s on p.idprod = s.idprod
GROUP BY
    p.idprod,
    p.nomprod,
    p.descprod,
    p.precio,
    p.imagen
ORDER BY p.idprod

END
"""

SP_OBTENER_PRODUCTOS = """
CREATE OR ALTER PROCEDURE [dbo].[SP_OBTENER_PRODUCTOS]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM Producto
END
"""

SP_OBTENER_SOLICITUDES_DE_SERVICIO = """
CREATE OR ALTER PROCEDURE [dbo].[SP_OBTENER_SOLICITUDES_DE_SERVICIO]
	@tipousu VARCHAR(50),
	@rut     VARCHAR(50)
AS
BEGIN
	/*
		Ejemplos de ejecucion del procedimiento:
		EXEC SP_OBTENER_SOLICITUDES_DE_SERVICIO 'Técnico', '6666-6'
		EXEC SP_OBTENER_SOLICITUDES_DE_SERVICIO 'Técnico', '7777-7'
		EXEC SP_OBTENER_SOLICITUDES_DE_SERVICIO 'Técnico', '8888-8'
		EXEC SP_OBTENER_SOLICITUDES_DE_SERVICIO 'Cliente', '1111-1'
		EXEC SP_OBTENER_SOLICITUDES_DE_SERVICIO 'Todos', ''
	*/

	SET NOCOUNT ON;

	IF (@tipousu = 'Técnico')
		SELECT 
			sol.nrosol, 
			usucli.first_name + ' '  + usucli.last_name AS nomcli, 
			sol.tiposol,
			CONVERT(date, sol.fechavisita), 
			FORMAT(sol.fechavisita, 'HH:mm')            AS hora,
			usutec.first_name + ' '  + usutec.last_name AS nomtec,
			sol.descsol       + ': ' + fac.descfac      AS descser,
			sol.estadosol
		FROM 
			SolicitudServicio sol 
			INNER JOIN Factura       fac    ON sol.nrofac     = fac.nrofac
			INNER JOIN PerfilUsuario percli ON fac.rutcli     = percli.rut
			INNER JOIN PerfilUsuario pertec ON sol.ruttec     = pertec.rut
			INNER JOIN auth_user     usucli ON percli.user_id =  usucli.id
			INNER JOIN auth_user     usutec ON pertec.user_id =  usutec.id
		WHERE
			pertec.rut = @rut
		ORDER BY 
			usucli.first_name

	IF (@tipousu = 'Cliente')
		SELECT 
			sol.nrosol, 
			usucli.first_name + ' '  + usucli.last_name AS nomcli, 
			sol.tiposol,
			CONVERT(date, sol.fechavisita), 
			FORMAT(sol.fechavisita, 'HH:mm')            AS hora,
			usutec.first_name + ' '  + usutec.last_name AS nomtec,
			sol.descsol       + ': ' + fac.descfac      AS descser,
			sol.estadosol
		FROM 
			SolicitudServicio sol 
			INNER JOIN Factura       fac    ON sol.nrofac     = fac.nrofac
			INNER JOIN PerfilUsuario percli ON fac.rutcli     = percli.rut
			INNER JOIN PerfilUsuario pertec ON sol.ruttec     = pertec.rut
			INNER JOIN auth_user     usucli ON percli.user_id =  usucli.id
			INNER JOIN auth_user     usutec ON pertec.user_id =  usutec.id
		WHERE
			percli.rut = @rut
		ORDER BY 
			usutec.first_name

	IF (@tipousu = 'Todos')
		SELECT 
			sol.nrosol, 
			usucli.first_name + ' '  + usucli.last_name AS nomcli, 
			sol.tiposol,
			CONVERT(date, sol.fechavisita), 
			FORMAT(sol.fechavisita, 'HH:mm')            AS hora,
			usutec.first_name + ' '  + usutec.last_name AS nomtec,
			sol.descsol       + ': ' + fac.descfac      AS descser,
			sol.estadosol
		FROM 
			SolicitudServicio sol 
			INNER JOIN Factura       fac    ON sol.nrofac     = fac.nrofac
			INNER JOIN PerfilUsuario percli ON fac.rutcli     = percli.rut
			INNER JOIN PerfilUsuario pertec ON sol.ruttec     = pertec.rut
			INNER JOIN auth_user     usucli ON percli.user_id =  usucli.id
			INNER JOIN auth_user     usutec ON pertec.user_id =  usutec.id
		ORDER BY
			sol.nrosol

END
"""

SP_OBTENER_TODOS_LOS_USUARIOS = """
CREATE OR ALTER PROCEDURE [dbo].[SP_OBTENER_TODOS_LOS_USUARIOS]
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		*
	FROM 
		PerfilUsuario per
		INNER JOIN auth_user usu ON per.user_id = usu.id
END
"""

SP_RESERVAR_EQUIPO_ANWO = """
CREATE OR ALTER PROCEDURE [dbo].[SP_RESERVAR_EQUIPO_ANWO]
    @nroserieanwo NVARCHAR(100),
	@reservado NVARCHAR(1)
AS
BEGIN
	/*
		Ejemplos de ejecucion del procedimiento:
		
        EXEC SP_RESERVAR_EQUIPO_ANWO 'A9', 'S'
        SELECT * FROM AnwoStockProducto WHERE nroserieanwo = 'A9' --DEBE ENTREGAR UNA FILA CON reservado = 'S'

		EXEC SP_RESERVAR_EQUIPO_ANWO 'A9', 'N'
        SELECT * FROM AnwoStockProducto WHERE nroserieanwo = 'A9' --DEBE ENTREGAR UNA FILA CON reservado = 'N'
	*/

    SET NOCOUNT ON;

    UPDATE AnwoStockProducto
    SET reservado = @reservado
    WHERE nroserieanwo = @nroserieanwo;
END
"""

#agregar
SP_OBTENER_EQUIPOS_ANWO = """
CREATE OR ALTER PROCEDURE [dbo].[SP_OBTENER_EQUIPOS_ANWO]
    @nroserieanwo VARCHAR(200) = NULL
    
AS
BEGIN

    SET NOCOUNT ON;
    
    IF (@nroserieanwo IS NULL)
    BEGIN

        SELECT 
            nroserieanwo,
            nomprodanwo,
            precioanwo,
            reservado
        FROM AnwoStockProducto
    END
    ELSE
    BEGIN

        SELECT 
            nroserieanwo,
            nomprodanwo,
            precioanwo,
            reservado
        FROM AnwoStockProducto
        WHERE nroserieanwo = @nroserieanwo
    END
END
"""



def exec_sql(query):
    with connection.cursor() as cursor:
        cursor.execute(query)

def run():

    # Poblar tabla Producto

    exec_sql("INSERT INTO dbo.Producto (idprod, nomprod, descprod, precio, imagen) VALUES (1,N'Aire Wifi 9000 btu',  N'Enfría 9-16 m2',     299990,'images/0001.png');")
    exec_sql("INSERT INTO dbo.Producto (idprod, nomprod, descprod, precio, imagen) VALUES (2,N'Split Inv 12000 btu', N'Frío/Calor AR12',    450000,'images/0002.png');")
    exec_sql("INSERT INTO dbo.Producto (idprod, nomprod, descprod, precio, imagen) VALUES (3,N'Anwo VP',             N'9000 Virus Protect', 288990,'images/0003.png');")
    exec_sql("INSERT INTO dbo.Producto (idprod, nomprod, descprod, precio, imagen) VALUES (4,N'Anwo Portátil',       N'12000 btu f/c',      131990,'images/0004.png');")
    exec_sql("INSERT INTO dbo.Producto (idprod, nomprod, descprod, precio, imagen) VALUES (5,N'GPORT-14',            N'Anwo 14000 btu',     399990,'images/0005.png');")
    exec_sql("INSERT INTO dbo.Producto (idprod, nomprod, descprod, precio, imagen) VALUES (6,N'Kendal 12000',        N'Climat 22-24 m2',    335990,'images/0006.png');")
    exec_sql("INSERT INTO dbo.Producto (idprod, nomprod, descprod, precio, imagen) VALUES (7,N'Kendal Wifi 4000',    N'Climat 32-34 m2',    335990,'images/0006.png');")
    exec_sql("INSERT INTO dbo.Producto (idprod, nomprod, descprod, precio, imagen) VALUES (8,N'Anwo 12000',          N'Climat 42-44 m2',    335990,'images/0006.png');")

    # Poblar tabla PerfilUsuario

    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'1234-5',  N'Superusuario',  N'La Florida',  1);")
    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'1111-1',  N'Cliente',       N'La Florida',  2);")
    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'2222-2',  N'Cliente',       N'Santiago',    3);")
    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'3333-3',  N'Cliente',       N'Providencia', 4);")
    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'4444-4',  N'Cliente',       N'Las Condes',  5);")
    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'5555-5',  N'Cliente',       N'Maipú',       6);")
    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'6666-6',  N'Técnico',       N'Cerro Navia', 7);")
    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'7777-7',  N'Técnico',       N'Peñalolén',   8);")
    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'8888-8',  N'Técnico',       N'La Reina',    9);")
    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'9999-9',  N'Bodeguero',     N'La Florida',  10);")
    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'0000-0',  N'Administrador', N'La Reina',    11);")
    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'4321-0',  N'Vendedor',      N'Las Condes',  12);")

    # Poblar tabla Factura

    exec_sql("INSERT INTO dbo.Factura (nrofac, rutcli, idprod, fechafac, descfac, monto) VALUES (1, N'1111-1', 1, CAST('20220223' AS datetime), N'Aire Wifi 9000 btu',  25000);")
    exec_sql("INSERT INTO dbo.Factura (nrofac, rutcli, idprod, fechafac, descfac, monto) VALUES (2, N'2222-2', 2, CAST('20220224' AS datetime), N'Split Inv 12000 btu', 450000);")
    exec_sql("INSERT INTO dbo.Factura (nrofac, rutcli, idprod, fechafac, descfac, monto) VALUES (3, N'3333-3', 4, CAST('20220303' AS datetime), N'Anwo Portátil',       25000);")
    exec_sql("INSERT INTO dbo.Factura (nrofac, rutcli, idprod, fechafac, descfac, monto) VALUES (4, N'4444-4', 4, CAST('20220308' AS datetime), N'Anwo Portátil',       25000);")
    exec_sql("INSERT INTO dbo.Factura (nrofac, rutcli, idprod, fechafac, descfac, monto) VALUES (5, N'5555-5', 4, CAST('20220315' AS datetime), N'Anwo Portátil',       25000);")
    exec_sql("INSERT INTO dbo.Factura (nrofac, rutcli, idprod, fechafac, descfac, monto) VALUES (6, N'1111-1', 5, CAST('20220327' AS datetime), N'Mantención',          25000);")
    exec_sql("INSERT INTO dbo.Factura (nrofac, rutcli, idprod, fechafac, descfac, monto) VALUES (7, N'1111-1', 5, CAST('20220403' AS datetime), N'GPORT-14',            25000);")
    exec_sql("INSERT INTO dbo.Factura (nrofac, rutcli, idprod, fechafac, descfac, monto) VALUES (8, N'1111-1', 3, CAST('20220408' AS datetime), N'Anwo VP',             25000);")
    exec_sql("INSERT INTO dbo.Factura (nrofac, rutcli, idprod, fechafac, descfac, monto) VALUES (9, N'1111-1', 5, CAST('20220415' AS datetime), N'GPORT-14',            25000);")

    # Poblar tabla GuiaDespacho

    exec_sql("INSERT INTO dbo.GuiaDespacho (nrogd, nrofac, idprod, estadogd) VALUES (11, 1, 1, N'Entregado');")
    exec_sql("INSERT INTO dbo.GuiaDespacho (nrogd, nrofac, idprod, estadogd) VALUES (22, 2, 2, N'Depachado');")
    exec_sql("INSERT INTO dbo.GuiaDespacho (nrogd, nrofac, idprod, estadogd) VALUES (88, 8, 3, N'EnBodega');")

    # Poblar tabla SolicitudServicio

    exec_sql("INSERT INTO dbo.SolicitudServicio (nrosol, nrofac, tiposol, fechavisita, ruttec, descsol, estadosol) VALUES (111, 1, N'Instalación', CAST('20220302 09:00' AS datetime), N'6666-6', N'Instalar equipo', N'Cerrada');")
    exec_sql("INSERT INTO dbo.SolicitudServicio (nrosol, nrofac, tiposol, fechavisita, ruttec, descsol, estadosol) VALUES (222, 2, N'Instalación', CAST('20220303 10:00' AS datetime), N'6666-6', N'Instalar equipo', N'Aceptada');")
    exec_sql("INSERT INTO dbo.SolicitudServicio (nrosol, nrofac, tiposol, fechavisita, ruttec, descsol, estadosol) VALUES (333, 3, N'Mantención',  CAST('20220310 15:00' AS datetime), N'6666-6', N'Cambiar enchufe', N'Aceptada');")
    exec_sql("INSERT INTO dbo.SolicitudServicio (nrosol, nrofac, tiposol, fechavisita, ruttec, descsol, estadosol) VALUES (444, 4, N'Mantención',  CAST('20220315 09:00' AS datetime), N'6666-6', N'Limpiar filtro',  N'Aceptada');")
    exec_sql("INSERT INTO dbo.SolicitudServicio (nrosol, nrofac, tiposol, fechavisita, ruttec, descsol, estadosol) VALUES (555, 5, N'Reparación',  CAST('20220322 17:00' AS datetime), N'6666-6', N'Reparar equipo',  N'Aceptada');")
    exec_sql("INSERT INTO dbo.SolicitudServicio (nrosol, nrofac, tiposol, fechavisita, ruttec, descsol, estadosol) VALUES (666, 6, N'Mantención',  CAST('20220403 11:00' AS datetime), N'7777-7', N'Limpiar filtro',  N'Cerrada');")
    exec_sql("INSERT INTO dbo.SolicitudServicio (nrosol, nrofac, tiposol, fechavisita, ruttec, descsol, estadosol) VALUES (777, 7, N'Reparación',  CAST('20220410 16:00' AS datetime), N'6666-6', N'Reparar aire',    N'Modificada');")
    exec_sql("INSERT INTO dbo.SolicitudServicio (nrosol, nrofac, tiposol, fechavisita, ruttec, descsol, estadosol) VALUES (888, 8, N'Instalación', CAST('20220415 10:00' AS datetime), N'7777-7', N'Instalar equipo', N'Modificada');")
    exec_sql("INSERT INTO dbo.SolicitudServicio (nrosol, nrofac, tiposol, fechavisita, ruttec, descsol, estadosol) VALUES (999, 9, N'Reparación',  CAST('20220422 18:00' AS datetime), N'8888-8', N'Enchufe quemado', N'Aceptada');")

    # Poblar tabla StockProducto

    exec_sql("INSERT INTO dbo.StockProducto (idstock, idprod, nrofac) VALUES (11111, 1, 1);")
    exec_sql("INSERT INTO dbo.StockProducto (idstock, idprod, nrofac) VALUES (22222, 2, 2);")
    exec_sql("INSERT INTO dbo.StockProducto (idstock, idprod, nrofac) VALUES (88888, 3, 8);")
    exec_sql("INSERT INTO dbo.StockProducto (idstock, idprod, nrofac) VALUES (90001, 1, null);")
    exec_sql("INSERT INTO dbo.StockProducto (idstock, idprod, nrofac) VALUES (90002, 3, null);")
    exec_sql("INSERT INTO dbo.StockProducto (idstock, idprod, nrofac) VALUES (90003, 4, null);")
    exec_sql("INSERT INTO dbo.StockProducto (idstock, idprod, nrofac) VALUES (90004, 6, null);")
    exec_sql("INSERT INTO dbo.StockProducto (idstock, idprod, nrofac) VALUES (90005, 1, null);")
    exec_sql("INSERT INTO dbo.StockProducto (idstock, idprod, nrofac) VALUES (90006, 3, null);")
    exec_sql("INSERT INTO dbo.StockProducto (idstock, idprod, nrofac) VALUES (90007, 4, null);")

    # Poblar tabla AnwoStockProducto

    exec_sql("INSERT INTO dbo.ANWOSTOCKPRODUCTO (NROSERIEANWO, NOMPRODANWO, PRECIOANWO, RESERVADO) VALUES ('A1', 'Aire Anwo 111',  '500000', 'N');")
    exec_sql("INSERT INTO dbo.ANWOSTOCKPRODUCTO (NROSERIEANWO, NOMPRODANWO, PRECIOANWO, RESERVADO) VALUES ('A2', 'Aire Anwo 222',  '400000', 'N');")
    exec_sql("INSERT INTO dbo.ANWOSTOCKPRODUCTO (NROSERIEANWO, NOMPRODANWO, PRECIOANWO, RESERVADO) VALUES ('A3', 'Aire Anwo 333',  '300000', 'S');")
    exec_sql("INSERT INTO dbo.ANWOSTOCKPRODUCTO (NROSERIEANWO, NOMPRODANWO, PRECIOANWO, RESERVADO) VALUES ('A4', 'Aire Anwo 444',  '200000', 'S');")
    exec_sql("INSERT INTO dbo.ANWOSTOCKPRODUCTO (NROSERIEANWO, NOMPRODANWO, PRECIOANWO, RESERVADO) VALUES ('A5', 'Aire Anwo 555',  '500000', 'N');")
    exec_sql("INSERT INTO dbo.ANWOSTOCKPRODUCTO (NROSERIEANWO, NOMPRODANWO, PRECIOANWO, RESERVADO) VALUES ('A6', 'Aire Anwo 666',  '400000', 'N');")
    exec_sql("INSERT INTO dbo.ANWOSTOCKPRODUCTO (NROSERIEANWO, NOMPRODANWO, PRECIOANWO, RESERVADO) VALUES ('A7', 'Aire Anwo 777',  '300000', 'S');")
    exec_sql("INSERT INTO dbo.ANWOSTOCKPRODUCTO (NROSERIEANWO, NOMPRODANWO, PRECIOANWO, RESERVADO) VALUES ('A8', 'Aire Anwo 888',  '200000', 'S');")
    exec_sql("INSERT INTO dbo.ANWOSTOCKPRODUCTO (NROSERIEANWO, NOMPRODANWO, PRECIOANWO, RESERVADO) VALUES ('A9', 'Aire Anwo 999',  '500000', 'N');")


    try:
        exec_sql(SP_CREAR_FACTURA)
    except:
        pass

    try:
        exec_sql(SP_CREAR_SOLICITUD_SERVICIO)
    except:
        pass

    try:
        exec_sql(SP_CREAR_GUIA_DESPACHO)
    except:
        pass
    
    try:
        exec_sql(SP_OBTENER_FACTURAS)
    except:
        pass

    try:
        exec_sql(SP_OBTENER_GUIAS_DE_DESPACHO)
    except:
        pass

    try:
        exec_sql(SP_ACTUALIZAR_SOLICITUD_DE_SERVICIO)
    except:
        pass

    try:
        exec_sql(SP_OBTENER_EQUIPOS_EN_BODEGA)
    except:
        pass

    try:
        exec_sql(SP_OBTENER_PRODUCTOS)
    except:
        pass

    try:
        exec_sql(SP_OBTENER_SOLICITUDES_DE_SERVICIO)
    except:
        pass
    
    try:
        exec_sql(SP_OBTENER_TODOS_LOS_USUARIOS)
    except:
        pass
    
    try:
        exec_sql(SP_RESERVAR_EQUIPO_ANWO)
    except:
        pass
    
    #agregar
    try:
        exec_sql(SP_OBTENER_EQUIPOS_ANWO)
    except:
        pass
    
    

    
    
