
## Descripción

Este proyecto tiene como objetivo desarrollar una solución para la gestión de turnos que permita a los clientes de varios comercios reservar con anticipación un espacio de atención en un servicio específico ofrecido por cada comercio. La solución incluye la generación de turnos mediante un procedimiento almacenado en SQL Server y la exposición de una API RESTful para invocar dicho procedimiento.

## Tecnologías Utilizadas

- **.NET Core**: Framework principal para el desarrollo de la API.
- **SQL Server**: Base de datos utilizada para almacenar la información de comercios, servicios y turnos.
- **Entity Framework Core**: ORM utilizado para interactuar con la base de datos.
- **xUnit / NUnit**: Frameworks de pruebas utilizados para asegurar la calidad del código.
- **Swagger**: Herramienta para documentar y probar la API.

## Características

- **Generación de Turnos**: Procedimiento almacenado que genera turnos diarios desde una fecha de inicio hasta una fecha de fin, basado en la duración del servicio.
- **API RESTful**: Endpoint para invocar el procedimiento almacenado de generación de turnos.
- **Pruebas Unitarias**: Pruebas unitarias para asegurar el correcto funcionamiento de las funcionalidades principales.

## Configuración del Proyecto

1. **Clonar el repositorio**:
   ```sh
   git clone https://github.com/darwin320/PruebaTecnicaScotiabank
2. **Ejecutar SCRIPTBASEDEDATOS.sql**:
   Se debera ejecutar el script anexo en el motor de SQL Server para la creacion de la base de datos , tablas y stored procedures.
3. **Ejecutar Proyecto PruebaTecnica.Api**:
Una vez ejecutado el proyecto se obtendra la siguiente ventana
![image](https://github.com/darwin320/PruebaTecnicaScotiabank/assets/81993592/1b52d7c5-94db-4eac-815b-c4bb26e55a61)
En esta parte se debera ingresar el ApiKey (apiKey), que para el caso sera: 12345-ABCDE.
4. **Consumir el Api Turnos**:
   para el consumo del API se puede testear con el siguiente JSON {
  "fechaInicio": "2024-07-10T09:00:00.000Z",
  "fechaFin": "2024-07-10T11:00:00.000Z",
  "idServicio": 1
}
y se obtendra el siguiente resultado con los turnos generados:
![image](https://github.com/darwin320/PruebaTecnicaScotiabank/assets/81993592/ec86e745-e6ae-438a-b1e1-84250c9637d8)

## Consideraciones Tecnicas
- **Version .Net Core**: Se empleo la version 7 de .Net Core
- **Version Visual Studio**: Se empleo la version 17.10.4 de Visual Studio 2022.
- **Version SQL Server**: Se empleo  Microsoft SQL Server 2022 (RTM) - 16.0.1000.6 (X64) 
	Oct  8 2022 05:58:25 
	Copyright (C) 2022 Microsoft Corporation
	Enterprise Evaluation Edition (64-bit) on Windows 10 Pro 10.0 <X64> (Build 22631: )
