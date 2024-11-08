# Proyecto de Gestión de Empresas y Productos

Este proyecto es una aplicación web construida con ASP.NET MVC, C#, .NET 8.0, HTML, JavaScript, API REST, SOAP y PostgreSQL. La aplicación permite gestionar información de empresas, productos y usuarios, además de generar y enviar reportes en PDF.

## Características

- **Vista Empresa**: Formulario para capturar y gestionar información de empresas.
- **Vista Productos**: Formulario para capturar y gestionar información de productos.
- **Vista de Inicio de Sesión**: Formulario para la autenticación de usuarios.
- **Vista de Inventario**: Generación y descarga de reportes en PDF, con capacidad para enviar el PDF por correo mediante una API REST o SOAP.

## Tecnologías Utilizadas

- C#
- .NET Framework 8.0
- ASP.NET MVC
- HTML, CSS, JavaScript
- API REST, SOAP
- PostgreSQL

## Requisitos Previos

- .NET Framework 8.0
- Visual Studio 2019 o superior
- PostgreSQL

## Configuración del Proyecto

### 1. Configurar Base de Datos

Asegúrate de tener PostgreSQL instalado y configura tu base de datos. Usa la siguiente cadena de conexión en tu archivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=PruebaTecnica;Username=Prueba;Password=Prueba123***"
  }
}
