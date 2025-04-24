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
- [Licencia](#licencia)

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
 ```bash
{
"Jwt": {
"Key": "ClaveSuperSecretaMuyLarga1234567890!!!KeyExtra",
"Issuer": "InventoryApi",
"Audience": "InventoryClient"
},
"AllowedHosts": "*"
}

