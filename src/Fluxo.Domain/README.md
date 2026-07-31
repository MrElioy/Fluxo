# Fluxo.Domain

Este proyecto contiene el núcleo del negocio de Fluxo.

Su responsabilidad es representar los conceptos financieros de la aplicación,
sus relaciones y las reglas que deben cumplirse independientemente de la
interfaz gráfica, la base de datos o cualquier servicio externo.

## Qué pertenece al dominio

- Entidades financieras.
- Objetos de valor.
- Enumeraciones propias del negocio.
- Reglas e invariantes.
- Comportamientos que modifican el estado de las entidades.
- Excepciones específicas del dominio.

## Qué no pertenece al dominio

- Acceso a SQLite.
- Entity Framework u otros ORM.
- DTOs.
- ViewModels.
- Ventanas o controles WPF.
- Lectura de consola.
- Servicios HTTP.
- Serialización.
- Configuración de infraestructura.

## Dependencias

`Fluxo.Domain` no debe depender de ningún otro proyecto de la solución.

Las demás capas pueden depender del dominio, pero el dominio no debe conocerlas.

```text
Fluxo.Console ───────┐
Fluxo.Wpf ───────────┼──> Fluxo.Application ──> Fluxo.Domain
                     │             │
                     └──> Fluxo.Infrastructure ──> Fluxo.Domain