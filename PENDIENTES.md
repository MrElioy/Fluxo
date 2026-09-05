# Fluxo — Puntos a corregir

Este documento acompaña a `Vision.md` y registra los puntos detectados durante la revisión del estado actual del proyecto.

## Prioridad alta

- Inicializar SQLite al arrancar la aplicación mediante `DatabaseInitializer`.
- Crear y versionar la primera migración de Entity Framework Core.
- Verificar la materialización de `FinancialMovement` desde EF Core: el constructor recibe `date`, mientras la propiedad se llama `TransactionDate`.
- Implementar completamente `Update` y `Delete` para movimientos: contratos, servicios, repositorios y persistencia.
- Conectar correctamente categorías con sus contratos de Application, repositorio de Infrastructure y DI.
- Implementar realmente `Update` y `Delete` de categorías.
- Crear proyectos de pruebas para Domain, Application, Infrastructure y Console.
- Agregar pruebas de integración con una base SQLite temporal.

## Dominio y Application

- Agregar comportamiento explícito al dominio para modificar movimientos y cambiar su estado.
- Definir si la eliminación será física o lógica y aplicar la decisión de forma consistente.
- Aplicar los campos de categoría que actualmente se ignoran, como `Description` y `ParentCategoryId`.
- Completar las relaciones entre cuentas, movimientos relacionados, categorías y movimientos categorizados.
- Validar rangos de filtros en Application, no solo en tipos internos de Infrastructure.
- Usar `ErrorType` en `Error` o eliminar el enum si no forma parte del diseño final.
- Definir una estrategia para traducir errores de dominio, SQLite y concurrencia a `Result`.
- Revisar el uso combinado de `DateTime.Today` y `DateTime.UtcNow` para mantener una política temporal coherente.

## Infrastructure

- Alinear las versiones de `Microsoft.Data.Sqlite` y Entity Framework Core.
- Eliminar o utilizar `SQLiteConnectionFactory`, que actualmente está registrada pero no participa del flujo.
- Definir una ubicación estable y apropiada para la base de datos de usuario, en lugar de depender de la carpeta del ejecutable.
- Configurar restricciones y relaciones de foreign key que actualmente no están representadas completamente.
- Decidir y documentar la estrategia de backup de la información financiera.

## Console

- Usar cultura y formatos explícitos para importar montos y fechas.
- Evitar mezclar monedas en balances: agrupar resultados por moneda o implementar conversión.
- Agregar comandos para editar, eliminar y categorizar movimientos cuando los casos de uso estén disponibles.
- Manejar errores de inicialización y persistencia sin mostrar excepciones no controladas.
- Agregar pruebas del parser, dispatcher y comandos principales.

## WPF

- Configurar composición de dependencias al arrancar la aplicación.
- Implementar ViewModels, comandos y bindings.
- Conectar las vistas con Application sin introducir lógica de persistencia en WPF.
- Invocar la inicialización de la base de datos desde el arranque gráfico.
- Definir la estrategia de compilación y ejecución de WPF en Windows.

## Verificación

Antes de considerar estable la primera versión:

- `dotnet build Fluxo.sln` debe funcionar en el entorno objetivo.
- `dotnet test` debe ejecutar una suite real.
- La consola debe poder crear la base, registrar un movimiento y volver a leerlo.
- Update y Delete deben persistir cambios reales.
- Los balances deben ser correctos por moneda.
- Debe existir al menos una prueba de extremo a extremo del flujo principal.
