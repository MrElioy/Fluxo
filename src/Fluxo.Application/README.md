# Fluxo.Application

## Propósito

`Fluxo.Application` contiene los casos de uso de Fluxo y coordina las operaciones que la aplicación ofrece a sus diferentes interfaces.

Esta capa define:

- qué operaciones puede ejecutar Fluxo;
- qué información necesita cada operación;
- qué datos devuelve;
- qué capacidades externas necesita;
- cómo se representan los errores esperados.

`Fluxo.Application` puede ser utilizada desde Console, WPF y futuras interfaces sin depender de ninguna de ellas.

## Responsabilidades

Application se encarga de:

- coordinar casos de uso;
- recibir y devolver DTOs;
- recuperar y persistir entidades mediante abstracciones;
- invocar comportamientos del Domain;
- convertir entidades en modelos de salida;
- representar éxitos y errores esperados de forma explícita;
- mantener las interfaces independientes de Infrastructure.

Application no debe contener:

- código de Entity Framework Core;
- conexiones o consultas SQLite;
- ventanas, controles o ViewModels de WPF;
- lectura o escritura de consola;
- reglas fundamentales que pertenecen al Domain;
- decisiones sobre cómo se muestran los errores;
- detalles específicos de una interfaz.

## Dirección de dependencias

```text
Fluxo.Console ───────┐
                     ├──> Fluxo.Application ──> Fluxo.Domain
Fluxo.Wpf ───────────┘              ▲
                                    │
                         Fluxo.Infrastructure
```

`Fluxo.Application` puede depender de `Fluxo.Domain`.

`Fluxo.Application` no debe depender de `Fluxo.Infrastructure`, `Fluxo.Console` ni `Fluxo.Wpf`.

`Fluxo.Infrastructure` implementa los contratos de persistencia definidos por Application.

## Estructura

```text
Fluxo.Application/
├── DTOs/
│   ├── Categories/
│   └── FinancialMovements/
├── Interfaces/
│   ├── Persistence/
│   └── Services/
├── Results/
└── Services/
```

## DTOs

Los DTOs representan los datos que entran y salen de los casos de uso.

Se utilizan para evitar que las interfaces dependan directamente de las entidades del Domain y para impedir que puedan modificar su estado fuera de sus comportamientos permitidos.

Los DTOs:

- pueden contener datos;
- pueden expresar una solicitud o una respuesta;
- no contienen reglas centrales del negocio;
- no contienen lógica de persistencia;
- no dependen de WPF, Console ni Entity Framework Core.

Los DTOs de creación y actualización no garantizan por sí solos que una operación sea válida. Las invariantes finales deben permanecer protegidas por el Domain.

## Interfaces de persistencia

`Interfaces/Persistence` contiene los contratos que Application necesita para guardar y recuperar información.

Application define estos contratos porque conoce las operaciones necesarias para sus casos de uso.

Infrastructure proporciona sus implementaciones concretas.

```text
Application                         Infrastructure

IFinancialMovementRepository  <──  FinancialMovementRepository
ICategoryRepository           <──  CategoryRepository
```

Los contratos no deben exponer:

- `DbContext`;
- `DbSet`;
- `IQueryable`;
- tipos específicos de Entity Framework Core;
- conexiones SQLite.

Un repositorio representa una capacidad de persistencia, no un servicio de negocio.

## Interfaces de servicios

`Interfaces/Services` define los casos de uso disponibles para las interfaces de usuario.

Console y WPF deben poder invocar las mismas operaciones mediante estos contratos, sin conocer los repositorios ni la tecnología de almacenamiento.

Los servicios trabajan con DTOs y resultados de Application. No exponen entidades para que sean modificadas por los consumidores.

## Results

`Results` contiene la representación común de éxito y fracaso.

Los errores esperados deben devolverse mediante `Result` o `Result<T>`.

Ejemplos de errores esperados:

- datos de entrada inválidos;
- movimiento inexistente;
- categoría inexistente;
- nombre de categoría duplicado;
- incumplimiento de una regla del negocio.

Las excepciones quedan reservadas para fallos inesperados o para invariantes del Domain que no deberían haber sido vulneradas por un flujo normal.

Las interfaces deciden cómo presentar un error. Application no muestra cuadros de diálogo ni escribe mensajes en la consola.

## Services

`Services` contiene las implementaciones de los casos de uso.

Un servicio de Application puede:

1. recibir un DTO;
2. validar la solicitud;
3. recuperar una entidad mediante un repositorio;
4. invocar comportamiento del Domain;
5. guardar el nuevo estado;
6. convertir el resultado en un DTO;
7. devolver un resultado explícito.

Un servicio de Application no debe:

- ejecutar SQL;
- consultar un `DbContext`;
- modificar propiedades privadas por mecanismos indirectos;
- duplicar invariantes que pertenecen al Domain;
- conocer ventanas, controles o comandos de interfaz;
- decidir cómo se presenta un mensaje al usuario.

## Validación

La validación se divide según su responsabilidad.

Application valida la forma de una solicitud y los requisitos de un caso de uso.

Ejemplos:

- que un identificador requerido haya sido enviado;
- que un intervalo de búsqueda sea coherente;
- que el recurso solicitado exista;
- que no exista una categoría duplicada.

Domain protege invariantes que deben cumplirse independientemente del caso de uso.

Ejemplos:

- que un movimiento tenga un monto válido;
- que una fecha permitida respete las reglas del negocio;
- que una categoría tenga un nombre válido;
- que una relación no utilice identificadores vacíos.

Infrastructure valida solamente requisitos técnicos de persistencia y configuración.

## Mapeo

En la etapa actual, el mapeo entre entidades y DTOs debe realizarse explícitamente dentro de Application.

No se incorpora una biblioteca de mapeo mientras el volumen y la complejidad de las conversiones no lo justifiquen.

El mapeo explícito mantiene visible qué información se expone y evita introducir configuración adicional prematuramente.

## Evolución incremental

La estructura inicial cubre movimientos y categorías.

No se incorporarán anticipadamente abstracciones para cuentas, presupuestos, inversiones, recurrencias o sincronización hasta que existan casos de uso concretos para ellas.

Las nuevas abstracciones deben responder a necesidades verificables y no solamente a posibilidades futuras.

## Regla práctica

Domain describe qué es Fluxo y protege sus reglas.

Application describe qué puede hacer Fluxo.

Infrastructure describe cómo se realizan las operaciones técnicas.

Presentation describe cómo interactúa el usuario con Fluxo.