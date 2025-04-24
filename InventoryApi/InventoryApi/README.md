# Inventory Management API

Este repositorio contiene la implementación del **backend** del sistema de gestión de inventarios para una tienda en línea, construido con **.NET 9.0** y **Entity Framework Core**.

## Tabla de contenido

- [Características](#caracter%C3%ADsticas)
- [Requisitos previos](#requisitos-previos)
- [Instalación y ejecución](#instalaci%C3%B3n-y-ejecuci%C3%B3n)
- [Configuración](#configuraci%C3%B3n)
- [Endpoints de la API](#endpoints-de-la-api)
- [Autenticación y autorización](#autenticaci%C3%B3n-y-autorizaci%C3%B3n)
- [Notificaciones de inventario bajo](#notificaciones-de-inventario-bajo)
- [Generación de reporte PDF](#generaci%C3%B3n-de-reporte-pdf)
- [Estructura de carpetas](#estructura-de-carpetas)
- [Contribuciones](#contribuciones)

---

## Características

- **Autenticación** basada en JWT.
- **Roles**: `Administrador` y `Empleado`.
- **CRUD** completo de productos.
- **Notificaciones** automáticas de inventario bajo (cantidad menor a 5).
- **Reporte PDF** de productos con inventario bajo.
- API RESTful siguiendo buenas prácticas HTTP.

## Requisitos previos

- [.NET SDK 9.0](https://dotnet.microsoft.com/download)
- **IDE**: Visual Studio, Rider u otro compatible con .NET.

## Instalación y ejecución

1. Clona este repositorio:
   ```bash
   git clone https://github.com/tu-org/inventory-api.git
   cd inventory-api

2. Restaura paquetes y compila:
   ```bash
   dotnet restore
   dotnet build
   
3. Ejecuta la aplicación:
    ```bash
   dotnet run


## Configuración

Todas las variables de configuración están en el archivo appsettings.json:
        

## Endpoints de la API

| Ruta                         | Método | Descripción                                  | Roles permitidos        |
|------------------------------|:------:|----------------------------------------------|-------------------------|
| `/api/login`                 | POST   | Generar JWT para autenticación               | Anónimo                 |
| `/api/register`              | POST   | Registrar nuevo usuario                      | Anónimo                 |
| `/api/products`              | GET    | Obtener lista de productos                   | Autenticado             |
| `/api/products/{id}`         | GET    | Obtener detalle de un producto               | Autenticado             |
| `/api/products`              | POST   | Crear nuevo producto                         | `Administrador`         |
| `/api/products/{id}`         | PUT    | Actualizar un producto                       | `Administrador`         |
| `/api/products/{id}`         | DELETE | Eliminar un producto                         | `Administrador`         |
| `/api/notifications`         | GET    | Listar notificaciones de inventario bajo     | `Administrador`         |
| `/api/notifications/check`   | POST   | (Test) Forzar chequeo de notificaciones      | `Administrador`         |
| `/api/reports/low-inventory` | GET    | Descargar PDF de inventario bajo             | `Administrador`         |


## Autenticación y autorización

- La API usa **Bearer JWT**.
- Para endpoints protegidos, envía el header:

## Authorization: Bearer {token}

- El token se obtiene en `/api/login`.

## Notificaciones de inventario bajo

- Un `HostedService` corre periódicamente (por defecto cada 60s) y crea notificaciones en BD cuando `Quantity < 5`.
- Puedes forzar la ejecución inmediata con:
  ```bash
  POST /api/notifications/check

## Generación de reporte PDF

- El endpoint `/api/reports/low-inventory` genera un PDF con todos los productos con `Quantity < 5`.
- El PDF incluye:
    - Título y fecha de generación.
    - Tabla con `Name`, `Category` y `Quantity`.
    - Paginación automática si hay muchas filas.

## Estructura de carpetas

/Controllers # Controladores API /Data # DbContext e inicialización /Models # Entidades y DTOs /Services # HostedService para notificaciones /Program.cs # Configuración y arranque /appsettings.json /README.md # Este archivo

## Contribuciones

¡Bienvenidas! Por favor abre un _issue_ o un _pull request_.

