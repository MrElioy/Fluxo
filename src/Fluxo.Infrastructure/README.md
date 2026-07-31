# Fluxo.Infrastructure

## Propósito

`Fluxo.Infrastructure` contiene las implementaciones técnicas que permiten a Fluxo comunicarse con mecanismos externos.

En la etapa actual del proyecto, su responsabilidad principal es gestionar la persistencia de datos mediante:

* Entity Framework Core.
* SQLite.
* Repositorios.
* Migraciones e inicialización de la base de datos.
* Registro de dependencias.

Esta capa conoce los detalles técnicos de almacenamiento, pero no debe contener las reglas centrales del negocio.

---

## Responsabilidades

Infrastructure se encarga de:

* Configurar Entity Framework Core.
* Mapear las entidades del Domain a tablas.
* Administrar el acceso a SQLite.
* Guardar y recuperar movimientos financieros.
* Guardar y recuperar categorías.
* Configurar relaciones entre entidades.
* Preparar la base de datos al iniciar la aplicación.
* Registrar sus servicios mediante Dependency Injection.

Infrastructure no debe contener:

* Reglas de negocio.
* Validaciones propias del Domain.
* Casos de uso de Application.
* ViewModels.
* Vistas de WPF.
* Interacción directa con el usuario.
* Lógica específica de Console o WPF.

---

## Estructura

```text
Fluxo.Infrastructure/
├── Configuration/
│   └── DependencyInjection.cs 
│      // Registrar todos los servicios de Infrastructure.
│
└── Persistence/
    ├── FluxoDbContext.cs 
    │  // Representar la sesión de trabajo de EF Core con la base de datos.
    │
    ├── Configurations/
    │   ├── FinancialMovementConfiguration.cs 
    │   │  // Definir cómo se almacena FinancialMovement.
    │   │
    │   ├── CategoryConfiguration.cs 
    │   │  // Definir cómo se almacena Category.
    │   │
    │   └── MovementCategoryConfiguration.cs 
    │      // Definir cómo se almacena la relación entre movimientos y categorías.
    │
    ├── Repositories/
    │   ├── FinancialMovementRepository.cs 
    │   │  // Guardar, actualizar y consultar movimientos financieros.
    │   │
    │   └── CategoryRepository.cs 
    │      // Guardar, actualizar y consultar categorías.
    │
    └── SQLite/
        ├── DatabaseInitializer.cs 
        │  // Preparar la base cuando arranca la aplicación.
        │
        └── SQLiteConnectionFactory.cs
           // Construir o proporcionar conexiones específicas de SQLite.
```

---

## Configuration

### DependencyInjection.cs

Registra todos los servicios pertenecientes a Infrastructure.

Su objetivo es centralizar la configuración para que los proyectos ejecutables no tengan que conocer cómo se construye cada implementación concreta.

Puede registrar:

* `FluxoDbContext`.
* Proveedor de SQLite.
* `FinancialMovementRepository`.
* `CategoryRepository`.
* `DatabaseInitializer`.
* Otros servicios técnicos que se agreguen en el futuro.

El método esperado tendrá una forma similar a:

```csharp
services.AddInfrastructure(connectionString);
```

De esta forma, `Fluxo.Console` o `Fluxo.Wpf` pueden habilitar toda la infraestructura desde un único punto.

---

## Persistence

La carpeta `Persistence` contiene todo lo relacionado con el almacenamiento y recuperación de información.

Esta capa traduce las entidades del Domain a estructuras compatibles con la base de datos.

---

## FluxoDbContext.cs

`FluxoDbContext` representa una sesión de trabajo de Entity Framework Core con la base de datos.

Expone las entidades persistidas mediante propiedades `DbSet`.

Inicialmente administrará:

```csharp
DbSet<FinancialMovement>
DbSet<Category>
DbSet<MovementCategory>
```

Sus responsabilidades incluyen:

* Consultar entidades.
* Agregar nuevas entidades.
* Detectar modificaciones.
* Eliminar registros.
* Coordinar transacciones.
* Aplicar cambios mediante `SaveChanges` o `SaveChangesAsync`.
* Cargar las configuraciones de mapeo.

`FluxoDbContext` no debe contener:

* Reglas de negocio.
* Validaciones propias de las entidades.
* Lógica de interfaz.
* Consultas específicas de cada caso de uso.

Las consultas particulares deben ubicarse en los repositorios.

---

## Configurations

La carpeta `Configurations` contiene el mapeo entre las entidades del Domain y las tablas de la base de datos.

Cada archivo configura una entidad o relación concreta.

Estas clases implementan normalmente:

```csharp
IEntityTypeConfiguration<T>
```

Las configuraciones pueden definir:

* Nombre de la tabla.
* Clave primaria.
* Claves foráneas.
* Campos obligatorios.
* Longitudes máximas.
* Precisión decimal.
* Índices.
* Conversiones de enums.
* Comportamiento ante eliminaciones.

Estas decisiones pertenecen a Infrastructure porque describen cómo se almacenan los datos, no qué significan dentro del negocio.

---

## FinancialMovementConfiguration.cs

Define cómo se almacena la entidad `FinancialMovement`.

Puede configurar:

* Tabla `FinancialMovements`.
* Clave primaria `Id`.
* Campo `AccountId`.
* Fecha de la transacción.
* Fecha de registro.
* Monto y precisión decimal.
* Moneda.
* Tipo de movimiento.
* Descripción.
* Estado.
* Movimiento relacionado.
* Fecha de actualización.
* Índices para consultas frecuentes.

También puede definir cómo se convierten los enums antes de guardarlos.

Por ejemplo, `MovementType` puede almacenarse como texto:

```text
Income
Expense
Transfer
Other
```

Esta clase solo describe el mapeo.

No crea movimientos ni valida las reglas de negocio.

---

## CategoryConfiguration.cs

Define cómo se almacena la entidad `Category`.

Puede configurar:

* Tabla `Categories`.
* Clave primaria `Id`.
* Nombre obligatorio.
* Longitud máxima del nombre.
* Descripción opcional.
* Estado activo.
* Fecha de creación.
* Categoría padre opcional.
* Relación jerárquica entre categorías.
* Índices para búsquedas por nombre.

También configura la autorrelación de categorías.

Ejemplo:

```text
Comida
├── Supermercado
└── Restaurantes
```

En este caso, `Supermercado` y `Restaurantes` pueden utilizar el identificador de `Comida` como `ParentCategoryId`.

---

## MovementCategoryConfiguration.cs

Define cómo se almacena la relación entre movimientos financieros y categorías.

Esta relación es muchos-a-muchos:

```text
Un movimiento puede tener varias categorías.

Una categoría puede pertenecer a varios movimientos.
```

La tabla intermedia contiene:

```text
MovementId
CategoryId
```

Ambos campos forman una clave primaria compuesta:

```text
MovementId + CategoryId
```

Esto evita que una misma categoría sea asociada dos veces al mismo movimiento.

Ejemplo válido:

```text
Movimiento 1 + Comida
Movimiento 1 + Hogar
Movimiento 2 + Comida
```

Ejemplo inválido:

```text
Movimiento 1 + Comida
Movimiento 1 + Comida
```

También define las claves foráneas hacia:

* `FinancialMovement`.
* `Category`.

---

## Repositories

La carpeta `Repositories` contiene las clases encargadas de guardar y recuperar entidades.

Los repositorios utilizan `FluxoDbContext` internamente y encapsulan las consultas de Entity Framework Core.

Su objetivo es evitar que Application dependa directamente de EF Core.

Application puede declarar las operaciones que necesita mediante interfaces, mientras que Infrastructure implementa esas operaciones.

Ejemplo conceptual:

```text
Fluxo.Application:
IFinancialMovementRepository

Fluxo.Infrastructure:
FinancialMovementRepository
```

Los repositorios no deben contener reglas centrales del negocio.

---

## FinancialMovementRepository.cs

Gestiona las operaciones de persistencia relacionadas con movimientos financieros.

Puede implementar operaciones como:

```csharp
AddAsync()
GetByIdAsync()
GetAllAsync()
GetByAccountAsync()
GetByDateRangeAsync()
UpdateAsync()
DeleteAsync()
```

Ejemplos de consultas que pueden pertenecer a este repositorio:

* Obtener movimientos de una cuenta.
* Obtener movimientos entre dos fechas.
* Obtener movimientos recientes.
* Obtener movimientos asociados a una categoría.
* Buscar un movimiento por su identificador.

El repositorio no decide si un movimiento es válido.

La entidad `FinancialMovement` debe proteger sus propias reglas.

---

## CategoryRepository.cs

Gestiona las operaciones de persistencia relacionadas con categorías.

Puede implementar operaciones como:

```csharp
AddAsync()
GetByIdAsync()
GetAllAsync()
GetActiveAsync()
GetByNameAsync()
GetChildrenAsync()
UpdateAsync()
```

Ejemplos de consultas:

* Obtener todas las categorías activas.
* Buscar una categoría por nombre.
* Obtener subcategorías.
* Obtener una categoría padre.
* Comprobar si una categoría existe.

El repositorio administra el acceso a los datos, pero no define las reglas internas de `Category`.

---

## SQLite

La carpeta `SQLite` contiene los elementos específicos del motor de base de datos SQLite.

Estos componentes no deberían utilizarse directamente desde Domain o Application.

---

## DatabaseInitializer.cs

Prepara la base de datos cuando arranca la aplicación.

Puede encargarse de:

* Verificar que la base exista.
* Aplicar migraciones pendientes.
* Crear la estructura inicial.
* Insertar datos iniciales.
* Activar configuraciones específicas de SQLite.
* Detectar errores durante la inicialización.

Ejemplo esperado:

```csharp
await context.Database.MigrateAsync();
```

Más adelante puede utilizarse para insertar categorías iniciales, como:

```text
Comida
Transporte
Vivienda
Salud
Ingresos
```

Estos datos iniciales deben agregarse cuidadosamente para evitar duplicados cada vez que se inicia la aplicación.

---

## SQLiteConnectionFactory.cs

Centraliza la creación de conexiones específicas de SQLite.

Puede encargarse de:

* Construir la cadena de conexión.
* Determinar la ubicación del archivo de base de datos.
* Crear instancias de `SqliteConnection`.
* Configurar opciones particulares del motor.
* Ejecutar configuraciones `PRAGMA`.
* Facilitar conexiones manuales cuando no se utilice EF Core.

Ejemplo conceptual:

```csharp
new SqliteConnection(connectionString);
```

Este archivo puede permanecer inicialmente vacío o no implementarse hasta que exista una necesidad concreta.

Si EF Core administra completamente las conexiones mediante:

```csharp
options.UseSqlite(connectionString);
```

la factory puede no ser necesaria todavía.

No debe agregarse lógica solamente para completar la estructura.

---

## Flujo general de persistencia

El flujo esperado es:

```text
WPF o Console
      ↓
Application
      ↓
Interfaz de repositorio
      ↓
Repositorio de Infrastructure
      ↓
FluxoDbContext
      ↓
Entity Framework Core
      ↓
SQLite
```

Ejemplo:

```text
El usuario registra un gasto.
        ↓
Application ejecuta el caso de uso.
        ↓
FinancialMovement valida sus reglas.
        ↓
FinancialMovementRepository recibe la entidad.
        ↓
FluxoDbContext registra el cambio.
        ↓
Entity Framework Core genera el SQL.
        ↓
SQLite guarda el movimiento.
```

---

## Dependencias

`Fluxo.Infrastructure` puede depender de:

```text
Fluxo.Domain
Fluxo.Application
```

Infrastructure necesita conocer Domain para persistir sus entidades.

También puede conocer Application para implementar las interfaces de persistencia definidas allí.

La dirección inversa no debe existir.

```text
Domain ─────X─────> Infrastructure
Application ─X────> implementaciones concretas de Infrastructure
```

Domain no debe conocer:

* Entity Framework Core.
* SQLite.
* Repositorios concretos.
* Cadenas de conexión.
* Migraciones.

---

## Reglas de diseño

### Separar negocio de persistencia

Las reglas de negocio pertenecen a Domain.

Ejemplo:

```csharp
if (amount <= 0)
{
    throw new ArgumentOutOfRangeException(...);
}
```

Las reglas de almacenamiento pertenecen a Infrastructure.

Ejemplo:

```csharp
builder.Property(x => x.Amount)
    .HasPrecision(18, 2);
```

---

### Evitar consultas de EF Core fuera de Infrastructure

Application no debería utilizar directamente:

```csharp
DbContext
DbSet
Include
AsNoTracking
SaveChanges
```

Estas operaciones deben quedar encapsuladas dentro de Infrastructure.

---

### Evitar abstracciones prematuras

No se deben crear:

* Repositorios genéricos.
* Factories innecesarias.
* Servicios sin una responsabilidad concreta.
* Capas adicionales solamente por seguir un patrón.

Cada archivo debe existir porque resuelve una necesidad real del proyecto.

---

### Proteger la integridad de los datos

La base debe complementar las validaciones del Domain mediante:

* Claves primarias.
* Claves foráneas.
* Claves compuestas.
* Índices.
* Campos obligatorios.
* Restricciones de longitud.
* Relaciones correctamente configuradas.

Domain evita crear objetos inválidos.

La base evita persistir relaciones o datos inconsistentes.

---

## Entidades persistidas inicialmente

La primera versión de Infrastructure administrará:

```text
FinancialMovement
Category
MovementCategory
```

Relaciones principales:

```text
FinancialMovement N ─────── N Category
                    mediante
                MovementCategory
```

`FinancialMovement` también contiene un `AccountId`, aunque la entidad `Account` todavía no forma parte de esta estructura inicial.

Hasta que `Account` sea incorporada formalmente, ese campo no tendrá una relación de clave foránea con una tabla propia.

---

## Convenciones iniciales

* Los identificadores utilizan `Guid`.
* Los montos utilizan `decimal`.
* Las fechas técnicas se almacenan en UTC.
* Las entidades se definen en Domain.
* Los mapeos de EF Core se definen en Infrastructure.
* Los enums pueden almacenarse como texto.
* Las configuraciones se separan por entidad.
* Las migraciones se versionan junto con el código.
* Los repositorios encapsulan las consultas.
* SQLite es un detalle técnico reemplazable.

---

## Orden de implementación

El orden inicial recomendado es:

1. Crear `FluxoDbContext`.
2. Crear las configuraciones de las entidades.
3. Registrar EF Core y SQLite.
4. Implementar `DatabaseInitializer`.
5. Generar la primera migración.
6. Verificar que las tablas se creen correctamente.
7. Guardar y recuperar datos de prueba.
8. Implementar los repositorios.
9. Conectar los repositorios con Application.

---

## Estado actual

La capa Infrastructure se encuentra en construcción.

La estructura inicial ya está definida, pero las implementaciones deben agregarse progresivamente y probarse antes de incorporar nuevas abstracciones.
