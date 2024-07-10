Prueba Técnica - Gestión de Turnos
Descripción
Este proyecto tiene como objetivo desarrollar una solución para la gestión de turnos que permita a los clientes de varios comercios reservar con anticipación un espacio de atención en un servicio específico ofrecido por cada comercio. La solución incluye la generación de turnos mediante un procedimiento almacenado en SQL Server y la exposición de una API RESTful para invocar dicho procedimiento.

Tecnologías Utilizadas
.NET Core: Framework principal para el desarrollo de la API.
SQL Server: Base de datos utilizada para almacenar la información de comercios, servicios y turnos.
Entity Framework Core: ORM utilizado para interactuar con la base de datos.
xUnit / NUnit: Frameworks de pruebas utilizados para asegurar la calidad del código.
Swagger: Herramienta para documentar y probar la API.
Características
Generación de Turnos: Procedimiento almacenado que genera turnos diarios desde una fecha de inicio hasta una fecha de fin, basado en la duración del servicio.
API RESTful: Endpoint para invocar el procedimiento almacenado de generación de turnos.
Pruebas Unitarias: Pruebas unitarias para asegurar el correcto funcionamiento de las funcionalidades principales.
