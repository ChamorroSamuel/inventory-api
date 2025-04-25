# Inventory Management API

Este repositorio contiene la implementación del **backend** del sistema de gestión de inventarios para una tienda en línea, construido con **.NET 8.0** y **Entity Framework Core**.

## Tabla de contenido

- [Características](#características)
- [Requisitos previos](#requisitos-previos)
- [Instalación y ejecución](#instalación-y-ejecución)
- [Configuración](#configuración)
- [Endpoints de la API](#endpoints-de-la-api)
- [Autenticación y autorización](#autenticación-y-autorización)
- [Notificaciones de inventario bajo](#notificaciones-de-inventario-bajo)
- [Generación de reporte PDF](#generación-de-reporte-pdf)
- [Estructura de carpetas](#estructura-de-carpetas)
- [Contribuciones](#contribuciones)

---

## Características

- **Autenticación** basada en JWT.
- **Roles**: `Administrador`, `Empleado` y `Supervisor`.
- **CRUD** completo de productos.
- **Notificaciones** automáticas de inventario bajo (cantidad menor a 5 unidades).
- **Reporte PDF** de productos con inventario bajo.
- **Reporte PDF** de todos los productos usando **PdfSharpCore**.
- **Seguridad** de endpoints por roles.
- **Separación por capas** siguiendo una estructura limpia (inspirada en Clean Architecture).
- **Integración con Docker** para MySQL.

---

## Requisitos previos

- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download)
- [Angular CLI](https://angular.io/cli)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

---

## Instalación y ejecución

1. Clona este repositorio:
   ```bash
   git clone https://github.com/tu-org/inventory-api.git
   cd inventory-api
   ```

2. Restaura paquetes y compila el backend:
   ```bash
   dotnet restore
   dotnet build
   ```

3. Corre la base de datos MySQL en Docker:
   ```bash
   docker run -d --name inventory-mysql \
   -e MYSQL_ROOT_PASSWORD=TuPass123 \
   -e MYSQL_DATABASE=InventoryDb \
   -p 3306:3306 mysql:8.0
   ```

4. Aplica las migraciones para crear la base de datos:
   ```bash
   dotnet ef database update
   ```

5. Ejecuta la API:
   ```bash
   dotnet run
   ```

✅ La API estará corriendo en `http://localhost:5202`.

---

## Configuración

La configuración básica está en el archivo `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "ClaveSuperSecretaMuyLarga1234567890!!!KeyExtra"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=InventoryDb;User=root;Password=TuPass123;"
  },
  "AllowedHosts": "*"
}
```

---

## Endpoints de la API

| Ruta                         | Método | Descripción                                  | Roles permitidos        |
|-------------------------------|:------:|----------------------------------------------|-------------------------|
| `/api/login`                  | POST   | Generar JWT para autenticación               | Anónimo                 |
| `/api/register`               | POST   | Registrar nuevo usuario                     | Anónimo                 |
| `/api/products`               | GET    | Obtener lista de productos                   | Autenticado             |
| `/api/products/{id}`          | GET    | Obtener detalle de un producto               | Autenticado             |
| `/api/products`               | POST   | Crear nuevo producto                         | `Administrador` o `Supervisor` |
| `/api/products/{id}`          | PUT    | Actualizar un producto                       | `Administrador` o `Supervisor` |
| `/api/products/{id}`          | DELETE | Eliminar un producto                         | `Administrador`         |
| `/api/notifications`          | GET    | Listar notificaciones de inventario bajo     | `Administrador`         |
| `/api/notifications/check`    | POST   | Forzar chequeo manual de notificaciones      | `Administrador`         |
| `/api/reports/low-inventory`  | GET    | Descargar PDF de productos con stock bajo    | `Administrador`         |
| `/api/reports/all-pdfsharp`   | GET    | Descargar PDF de todos los productos         | `Administrador`         |

---

## Autenticación y autorización

- La API usa **Bearer JWT**.
- Para acceder a endpoints protegidos debes enviar el token en el header:

```bash
Authorization: Bearer {token}
```

- El token se obtiene a través del endpoint `/api/login`.

---

## Notificaciones de inventario bajo

- Un servicio automático verifica los productos cada minuto.
- Si detecta que la cantidad de stock es menor a 5, crea una notificación.
- Solo los **administradores** ven las notificaciones en el frontend.
- También puedes forzar manualmente una verificación con:

```bash
POST /api/notifications/check
```

---

## Generación de reporte PDF

- **Reporte de productos con stock bajo**:
  ```bash
  GET /api/reports/low-inventory
  ```
- **Reporte de todos los productos** (generado con PdfSharpCore):
  ```bash
  GET /api/reports/all-pdfsharp
  ```

---

## Estructura de carpetas

```
/Controllers           # Controladores API
/Domain/Entities       # Entidades de negocio (Product, Notification, User)
/Migrations            # Migraciones de Entity Framework Core
/DTOs                # DTOs para requests/responses
/Services              # Servicios internos (notificaciones de stock bajo)
Program.cs             # Configuración de la aplicación
appsettings.json       # Configuración de JWT, DB, etc.
```

---


## Notas adicionales

- Para ambientes de producción recomiendo:
    - Encriptar contraseñas.
    - Validar correctamente los datos de entrada.
    - Implementar roles más granulares si es necesario.
