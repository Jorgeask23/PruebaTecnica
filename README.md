# Prueba Técnica – API REST

## 1. Descripción

API REST desarrollada en **C# y .NET 10** para la gestión de usuarios, direcciones y monedas, incluyendo conversión de divisas.

La solución utiliza:

- ASP.NET Core **Minimal API**
- Entity Framework Core
- SQLite
- MediatR / CQRS
- FluentValidation
- Repository Pattern
- Swagger / OpenAPI
- `PasswordHasher` para almacenar contraseñas mediante hash
- Inyección de dependencias

## 2. Requisitos de la prueba

La prueba técnica solicita:

- CRUD de Users.
- CRUD de Addresses relacionados con Users mediante una relación 1:N.
- Módulo de conversión de divisas usando la tabla Currency.
- Seguridad mediante API Key.
- Entity Framework Core con SQLite.
- FluentValidation para validar requests.
- Patrón CQRS mediante Commands / Queries.
- Proyecto que compile y levante correctamente.
- README con versión de .NET, ejecución, migraciones, API Key, ejemplos y estado de implementación.

## 3. Estructura de la solución

La solución está dividida en cuatro proyectos:

```text
PruebaTecnica
├── PruebaTecnica.Api
├── PruebaTecnica.Application
├── PruebaTecnica.Domain
└── PruebaTecnica.Infrastructure
```

### `PruebaTecnica.Api`

Contiene:

- Minimal API.
- Endpoints HTTP.
- Swagger / OpenAPI.
- Middleware de API Key.
- Middleware global de excepciones.
- Configuración de inyección de dependencias.

### `PruebaTecnica.Application`

Contiene:

```text
Users/
├── Commands/
└── Queries/

Addresses/
├── Commands/
└── Queries/

Currencies/
├── Commands/
└── Queries/

CurrencyConversion/

Handlers/
├── Users/
├── Addresses/
└── Currencies/

Validators/
DTOs/
Interfaces/
Behaviors/
```

Las carpetas de Users, Addresses, Currencies y CurrencyConversion se mantienen separadas para organizar Commands, Queries, conversión y validaciones según el dominio funcional.

### `PruebaTecnica.Domain`

Contiene las entidades:

- `User`
- `Address`
- `Currency`

### `PruebaTecnica.Infrastructure`

Contiene:

- `AppDbContext`
- Repositorios
- Migraciones
- Persistencia SQLite

## 4. Modelo de datos

### User

Campos principales:

- `Id` – clave primaria.
- `Name` – requerido.
- `Email` – requerido y único.
- `IsActive` – por defecto `true`.
- `Password` – almacenado mediante hash.

### Address

Campos principales:

- `Id` – clave primaria.
- `UserId` – requerido y clave foránea hacia `Users.Id`.
- `Street` – requerido.
- `City` – requerido.
- `Country` – requerido.
- `ZipCode` – opcional.

Relación:

```text
User 1 ───── N Address
```

### Currency

Campos principales:

- `Id` – clave primaria.
- `Code` – requerido y único.
- `Name` – requerido.
- `RateToBase` – requerido y mayor que cero.

Ejemplo:

```text
PYG = 1
USD = 7300
```

## 5. Requisitos previos

- **.NET 10 SDK**
- Visual Studio 2022 compatible con .NET 10 o una instalación equivalente del SDK.
- `dotnet-ef` para aplicar migraciones desde la terminal, si se utiliza ese método.

## 6. Configuración

En:

```text
PruebaTecnica.Api/appsettings.json
```

se configura la conexión a SQLite y la API Key de prueba:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=pruebatecnica.db"
  },
  "ApiKey": "mi-clave-secreta-123"
}
```

Esta API Key se utiliza únicamente como valor de prueba.

Para entornos reales se recomienda utilizar variables de entorno o User Secrets y no publicar secretos reales.

## 7. Base de datos y migraciones

La aplicación utiliza **SQLite + Entity Framework Core Code First**.

El `AppDbContext` incluye:

```csharp
DbSet<User> Users
DbSet<Address> Addresses
DbSet<Currency> Currencies
```

La solución incluye una migración inicial:

```text
PruebaTecnica.Infrastructure/Migrations/20260930185222_InitialCreate.cs
```

y:

```text
PruebaTecnica.Infrastructure/Migrations/AppDbContextModelSnapshot.cs
```

### Consola del Administrador de paquetes de Visual Studio

Ejecutar:

```powershell
Update-Database -Project PruebaTecnica.Infrastructure -StartupProject PruebaTecnica.Api
```

### Terminal

Desde la carpeta de la solución:

```bash
dotnet ef database update --project PruebaTecnica.Infrastructure --startup-project PruebaTecnica.Api
```

La base SQLite se crea utilizando:

```text
Data Source=pruebatecnica.db
```

Los archivos locales de SQLite se encuentran excluidos del repositorio mediante `.gitignore`.

## 8. Ejecución

1. Abrir `PruebaTecnica.sln`.
2. Seleccionar `PruebaTecnica.Api` como proyecto de inicio.
3. Verificar `appsettings.json`.
4. Aplicar las migraciones.
5. Ejecutar la aplicación.
6. Abrir Swagger en:

```text
/swagger
```

La URL y el puerto exactos dependen del perfil de ejecución.

## 9. Seguridad mediante API Key

La API requiere el header:

```http
X-API-KEY: mi-clave-secreta-123
```

En Swagger:

1. Seleccionar **Authorize**.
2. Introducir la API Key.
3. Autorizar.
4. Ejecutar los endpoints.

Comportamiento comprobado durante las pruebas iniciales:

- Sin API Key → `401 Unauthorized`.
- API Key incorrecta → `401 Unauthorized`.

Swagger se deja accesible para facilitar la documentación y las pruebas.

## 10. Endpoints

### Users

| Método | Endpoint | Descripción |
|---|---|---|
| POST | `/users` | Crear usuario |
| GET | `/users` | Listar usuarios |
| GET | `/users/{id}` | Obtener usuario por ID |
| PUT | `/users/{id}` | Modificar usuario |
| DELETE | `/users/{id}` | Eliminar usuario |

El listado admite opcionalmente:

```text
GET /users?isActive=true
GET /users?isActive=false
```

### Addresses

| Método | Endpoint | Descripción |
|---|---|---|
| POST | `/users/{userId}/addresses` | Crear dirección para un usuario |
| GET | `/users/{userId}/addresses` | Listar direcciones de un usuario |
| PUT | `/addresses/{id}` | Modificar dirección |
| DELETE | `/addresses/{id}` | Eliminar dirección |

### Currencies

La consigna solicita listar y crear monedas:

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/currencies` | Listar monedas |
| POST | `/currencies` | Crear moneda |

La implementación contiene además:

| Método | Endpoint | Descripción |
|---|---|---|
| PUT | `/currencies/{id}` | Actualizar moneda (funcionalidad adicional) |

### Currency conversion

| Método | Endpoint | Descripción |
|---|---|---|
| POST | `/currency/convert` | Convertir una cantidad entre dos monedas |

## 11. Ejemplos de requests

### Crear usuario

La solicitud mínima indicada por la consigna:

```http
POST /users
```

```json
{
  "name": "Juan",
  "email": "juan@test.com"
}
```

También se acepta una contraseña:

```json
{
  "name": "Juan",
  "email": "juan@test.com",
  "password": "12345678"
}
```

Cuando se envía una contraseña, se almacena su hash.

Cuando no se envía, la aplicación genera internamente un valor aleatorio y almacena su hash, ya que la prueba no define un endpoint de autenticación.

### Obtener usuario

```http
GET /users/1
```

### Modificar usuario

```http
PUT /users/1
```

```json
{
  "name": "Juan Pérez",
  "email": "juan.perez@test.com",
  "isActive": true
}
```

### Crear dirección

```http
POST /users/1/addresses
```

```json
{
  "street": "Calle Falsa 123",
  "city": "Asunción",
  "country": "Paraguay",
  "zipCode": "9999"
}
```

### Modificar dirección

```http
PUT /addresses/1
```

```json
{
  "street": "Nueva dirección 456",
  "city": "Asunción",
  "country": "Paraguay",
  "zipCode": "1000"
}
```

### Crear moneda

```http
POST /currencies
```

```json
{
  "code": "USD",
  "name": "Dólar Estadounidense",
  "rateToBase": 7300
}
```

Otra moneda de ejemplo:

```json
{
  "code": "PYG",
  "name": "Guaraní Paraguayo",
  "rateToBase": 1
}
```

### Conversión de divisas

La solicitud utiliza los nombres solicitados por la consigna:

```http
POST /currency/convert
```

```json
{
  "fromCurrencyCode": "USD",
  "toCurrencyCode": "PYG",
  "amount": 100
}
```

Respuesta:

```json
{
  "fromCurrency": "USD",
  "toCurrency": "PYG",
  "originalAmount": 100,
  "convertedAmount": 730000
}
```

## 12. Fórmula de conversión

Se utiliza `RateToBase`:

```text
montoBase = amount * from.RateToBase
convertedAmount = montoBase / to.RateToBase
```

Ejemplo:

```text
100 USD * 7300 = 730000 PYG
730000 / 1 = 730000 PYG
```

## 13. Validaciones

### Users

- `name` requerido.
- `email` requerido.
- `email` debe tener formato válido.
- `email` debe ser único.
- `password`, cuando se envía, debe tener al menos 8 caracteres.

### Addresses

- `street` requerido.
- `city` requerido.
- `country` requerido.
- `zipCode` opcional.
- `userId` debe ser mayor que cero.
- El usuario debe existir para crear la dirección.

### Currencies

- `code` debe contener exactamente 3 letras.
- `code` debe ser único.
- `name` requerido.
- `rateToBase` debe ser mayor que cero.

### Currency conversion

- `fromCurrencyCode` debe contener exactamente 3 letras.
- `toCurrencyCode` debe contener exactamente 3 letras.
- `amount` debe ser mayor que cero.
- Ambas monedas deben existir.

## 14. Manejo de errores

Se utiliza un middleware global para convertir excepciones controladas en respuestas HTTP:

- `400 Bad Request` – errores de validación.
- `401 Unauthorized` – API Key ausente o inválida.
- `404 Not Found` – recurso inexistente.
- `409 Conflict` – registros duplicados o moneda inexistente durante la conversión.
- `500 Internal Server Error` – errores no controlados.

## 15. CQRS + MediatR + FluentValidation

La aplicación separa Commands y Queries.

MediatR registra los handlers de `Application`.

Además, existe un `ValidationBehavior<TRequest, TResponse>` que ejecuta FluentValidation antes del handler correspondiente.

Esto permite centralizar la validación de requests.

## 16. Repository Pattern

La capa Application depende de interfaces:

```text
IUserRepository
IAddressRepository
ICurrencyRepository
```

La implementación concreta se encuentra en Infrastructure.

Esto mantiene separado el acceso a datos de la lógica de aplicación.

## 17. Estado de implementación

### Incluido

- CRUD completo de Users.
- CRUD completo de Addresses.
- Listado de Currencies.
- Creación de Currencies.
- Conversión de divisas.
- API Key.
- SQLite + Entity Framework Core.
- Migración inicial.
- FluentValidation.
- CQRS con MediatR.
- Repository Pattern.
- Minimal API.
- Swagger / OpenAPI.
- Middleware global de excepciones.

### Funcionalidad adicional

- Actualización de monedas mediante `PUT /currencies/{id}`.

## 18. Revisión final antes de entregar

Antes de subir el proyecto a Git:

- Compilar la solución y confirmar que no existan errores.
- Aplicar la migración en una base SQLite limpia.
- Ejecutar la API.
- Verificar Swagger.
- Probar API Key válida, ausente e inválida.
- Probar todas las operaciones de Users.
- Probar todas las operaciones de Addresses.
- Probar creación y listado de Currencies.
- Probar conversión con monedas existentes.
- Probar monto menor o igual a cero.
- Probar conversión con una moneda inexistente.
- Ejecutar `git status`.
- No subir `.vs`, `bin`, `obj` ni archivos SQLite.
- No subir secretos reales.

