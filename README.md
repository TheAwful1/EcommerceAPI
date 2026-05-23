# 🛒 EcommerceAPI

API REST para una plataforma de comercio electrónico, desarrollada con **.NET 8** y **MySQL**. Incluye autenticación JWT, gestión de productos, carrito de compras y órdenes.

---

## 🚀 Stack Tecnológico

| Capa | Tecnología |
|------|-----------|
| Framework | .NET 8 (ASP.NET Core) |
| Base de Datos | MySQL 8 |
| ORM | Entity Framework Core + Pomelo |
| Autenticación | JWT Bearer |
| Documentación | Swagger / OpenAPI |
| Deploy | Railway |

---

## ⚙️ Variables de Entorno

Crea un archivo `appsettings.Development.json` en la raíz del proyecto:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=HOST;port=PORT;database=DB;user=USER;password=PASSWORD"
  },
  "Jwt": {
    "Key": "tu_clave_secreta_larga",
    "Issuer": "EcommerceAPI",
    "Audience": "EcommerceClient"
  }
}
```

> ⚠️ Nunca subas este archivo a GitHub. Está incluido en `.gitignore`.

---

## 🏃 Correr el proyecto localmente

```bash
# Clonar el repositorio
git clone https://github.com/tu-usuario/EcommerceAPI.git
cd EcommerceAPI

# Restaurar dependencias
dotnet restore

# Aplicar migraciones
dotnet ef database update --connection "tu_connection_string"

# Correr la API
dotnet run
```

## 📦 Migraciones

```bash
# Crear una nueva migración
dotnet ef migrations add NombreMigracion

# Aplicar migraciones
dotnet ef database update --connection "tu_connection_string"

# Revertir última migración
dotnet ef migrations remove
```

---

## 📡 Endpoints

### 🔐 Auth — `/api/auth`
| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| POST | `/api/auth/register` | Registrar nuevo usuario | ❌ |
| POST | `/api/auth/login` | Iniciar sesión, retorna JWT | ❌ |

### 🛍️ Productos — `/api/products`
| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| GET | `/api/products` | Listar todos los productos | ❌ |
| GET | `/api/products/{id}` | Obtener producto por ID | ❌ |
| POST | `/api/products` | Crear producto | ✅ Admin |
| PUT | `/api/products/{id}` | Editar producto | ✅ Admin |
| DELETE | `/api/products/{id}` | Eliminar producto | ✅ Admin |

### 📂 Categorías — `/api/categories`
| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| GET | `/api/categories` | Listar categorías | ❌ |
| POST | `/api/categories` | Crear categoría | ✅ Admin |
| PUT | `/api/categories/{id}` | Editar categoría | ✅ Admin |
| DELETE | `/api/categories/{id}` | Eliminar categoría | ✅ Admin |

### 🛒 Carrito — `/api/cart`
| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| GET | `/api/cart` | Ver carrito del usuario | ✅ |
| POST | `/api/cart` | Agregar producto al carrito | ✅ |
| PUT | `/api/cart/{itemId}` | Actualizar cantidad | ✅ |
| DELETE | `/api/cart/{itemId}` | Eliminar item del carrito | ✅ |

### 📋 Órdenes — `/api/orders`
| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| GET | `/api/orders` | Ver órdenes del usuario | ✅ |
| POST | `/api/orders` | Crear orden desde el carrito | ✅ |
| GET | `/api/orders/{id}` | Detalle de una orden | ✅ |

---

## 🔑 Autenticación

La API usa **JWT Bearer**. Para endpoints protegidos incluye el token en el header:

```
Authorization: Bearer {tu_token}
```

Puedes probar todos los endpoints directamente desde **Swagger UI**, que incluye soporte para JWT integrado.

---

## 🗄️ Diagrama de Base de Datos

```
Users ──────┬──── UserRoles ──── Roles
            │
            ├──── Carts ──── CartItems ──── Products ──── Categories
            │                                    │
            │                               ProductImages
            │
            └──── Orders ──── OrderItems
                     │
                  Payments
```

---

## 🌐 Deploy

La API está desplegada en **Railway**:

```
https://ecommerceapi-production-0cb5.up.railway.app/swagger
```

En producción, la API se conecta a una instancia de MySQL también hosteada en Railway usando la red interna (`mysql.railway.internal`).

---

## 📁 Estructura del Proyecto

```
EcommerceAPI/
├── Controllers/        # Endpoints de la API
├── Models/             # Entidades y AppDbContext
├── Services/           # Lógica de negocio
├── Migrations/         # Migraciones de EF Core
├── Program.cs          # Configuración de la app
└── appsettings.json    # Configuración general
```

---

## 👤 Autor

Desarrollado por **[Tu Nombre]** como proyecto de portafolio.

[![LinkedIn](https://img.shields.io/badge/LinkedIn-blue?logo=linkedin)](https://linkedin.com/in/tu-perfil)
[![GitHub](https://img.shields.io/badge/GitHub-black?logo=github)](https://github.com/tu-usuario)