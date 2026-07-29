# Fluxo — Visión del proyecto

## Intención

Fluxo es una aplicación personal de gestión financiera diseñada para registrar, organizar y analizar movimientos de dinero de forma clara y segura.

El proyecto nace como una reconstrucción de una aplicación de consola anterior. El objetivo no es solamente crear un CRUD funcional, sino construir una base arquitectónica limpia, modular y extensible que pueda evolucionar sin tener que reescribirse completamente cada vez que cambie la interfaz o la tecnología de persistencia.

## Problema que busca resolver

Quiero disponer de una herramienta propia que me permita entender con precisión:

- cuánto dinero entra;
- cuánto dinero sale;
- en qué se gasta;
- cuánto se ahorra;
- cuánto se invierte;
- cómo evoluciona el patrimonio a lo largo del tiempo.

La aplicación debe priorizar el control real de las finanzas personales por encima de la complejidad contable innecesaria.

## Principios del proyecto

1. **Separación de responsabilidades**

   La interfaz, la lógica de aplicación, el dominio y la infraestructura deben permanecer desacoplados.

2. **Independencia de la interfaz**

   La misma lógica debe poder utilizarse desde consola, WPF, web, móvil o una API.

3. **Independencia de la persistencia**

   El dominio no debe depender directamente de SQLite ni de otra base de datos específica.

4. **Código comprensible**

   Se priorizará una arquitectura explícita y fácil de recorrer antes que abstracciones innecesarias.

5. **Evolución incremental**

   Cada funcionalidad debe incorporarse en pasos pequeños, compilables y verificables.

6. **Privacidad y seguridad**

   Como Fluxo puede manejar información financiera sensible, la seguridad debe ser parte del diseño y no un agregado tardío.

## Arquitectura inicial

La solución se divide en los siguientes proyectos:

- `Fluxo.Domain`: entidades, enums y reglas fundamentales del dominio.
- `Fluxo.Application`: casos de uso, servicios, contratos y resultados.
- `Fluxo.Infrastructure`: persistencia, SQLite y otros detalles técnicos.
- `Fluxo.Console`: cliente de consola.
- `Fluxo.Wpf`: cliente gráfico de escritorio.
- `tests`: pruebas automáticas del dominio y de la aplicación.

## Alcance inicial

La primera versión debe permitir:

- registrar movimientos;
- distinguir gastos, ingresos e inversiones;
- consultar movimientos existentes;
- editar movimientos;
- eliminar movimientos;
- categorizar movimientos;
- calcular balances básicos;
- persistir la información localmente mediante SQLite.

## Posible evolución

En etapas posteriores, Fluxo podría incorporar:

- cuentas y billeteras;
- transferencias entre cuentas;
- presupuestos mensuales;
- gastos recurrentes;
- objetivos de ahorro;
- seguimiento de inversiones;
- evolución patrimonial;
- gráficos y paneles;
- importación y exportación de datos;
- múltiples usuarios;
- backend mediante API;
- clientes web y móviles;
- sincronización entre dispositivos.

Estas posibilidades no forman parte necesariamente del alcance inmediato. Sirven como dirección general y no deben justificar complejidad prematura.

## Qué no debe convertirse Fluxo

Fluxo no debe transformarse en:

- una arquitectura ceremonial llena de capas sin propósito;
- una colección de interfaces que solo tienen una implementación posible;
- una aplicación fuertemente acoplada a WPF;
- un sistema que mezcle SQL, validaciones y presentación;
- un proyecto imposible de retomar después de algunos meses;
- un producto que comprometa datos sensibles por priorizar velocidad.

## Criterio para tomar decisiones

Ante una decisión técnica, la pregunta principal será:

> ¿Esta decisión mantiene el núcleo de Fluxo comprensible, reutilizable y fácil de modificar?

Si una abstracción no resuelve un problema real o previsible, puede esperar.

## Estado actual

El proyecto se encuentra en su etapa inicial de reconstrucción.

Existe una implementación anterior utilizada como prototipo y referencia. Fluxo busca conservar lo aprendido en ese desarrollo, corrigiendo sus problemas de acoplamiento, nomenclatura, estructura y mantenibilidad.