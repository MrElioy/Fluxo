# Fluxo.Console

`Fluxo.Console` is the command-line interface for Fluxo.

Its purpose is to provide a fast, lightweight and scriptable way to interact
with Fluxo without depending on a graphical interface.

The console acts as a presentation layer over `Fluxo.Application`.

It is responsible for:

- Reading user input.
- Parsing commands and arguments.
- Dispatching commands to the appropriate handlers.
- Calling Fluxo application services/use cases.
- Rendering results in a terminal-friendly format.
- Providing an interactive shell.
- Supporting direct CLI commands for scripting and automation.

It must not contain business logic or persistence logic.

---

## Design goals

Fluxo.Console should be:

- Fast.
- Simple to understand.
- Pleasant to use interactively.
- Scriptable from Bash or other shells.
- Independent from the persistence implementation.
- Consistent in its command syntax and output.

Example interactive usage:

```text
$ fluxo

FLUXO

> add expense 18500 "Supermercado" --category food

✓ Movement created

> balance

$1,284,420

> movements --last 5
```

Example direct CLI usage:

```bash
fluxo add expense 18500 "Supermercado" --category food

fluxo balance

fluxo movements --month september

fluxo export --month september > september.csv
```

---

# Architecture

```text
Fluxo.Console/
│
├── Commands/
│   ├── AddCommand.cs
│   ├── ListCommand.cs
│   ├── SummaryCommand.cs
│   └── HelpCommand.cs
│
├── Parsing/
│   ├── CommandParser.cs
│   ├── ParsedCommand.cs
│   └── CommandArguments.cs
│
├── Shell/
│   ├── FluxoShell.cs
│   ├── CommandDispatcher.cs
│   └── CommandHistory.cs
│
├── UI/
│   ├── ConsoleRenderer.cs
│   ├── TableRenderer.cs
│   ├── DashboardRenderer.cs
│   ├── PromptRenderer.cs
│   └── Theme.cs
│
├── Program.cs
└── Fluxo.Console.csproj
```

---

## Commands

Contains the implementation of the commands exposed by Fluxo.Console.

Commands translate parsed user intentions into calls to the application layer.

Commands should not contain business rules.

---

## Parsing

Responsible for translating raw command-line input into structured data.

Example:

```text
add expense 18500 "Supermercado" --category food
```

becomes something conceptually similar to:

```text
Command: add
Arguments:
    type = expense
    amount = 18500
    description = Supermercado

Options:
    category = food
```

---

## Shell

Contains the runtime behavior of the interactive Fluxo terminal.

It manages:

- Input loops.
- Command execution.
- Command dispatching.
- History.
- Shell lifecycle.

---

## UI

Responsible exclusively for presenting information to the user.

This includes:

- Text formatting.
- Tables.
- Dashboards.
- Prompts.
- Colors.
- Console layout.

The UI layer must not know how financial data is stored or calculated.

## Running the console

From the repository root:

```bash
dotnet run --project src/Fluxo.Console
dotnet run --project src/Fluxo.Console -- help
dotnet run --project src/Fluxo.Console -- list --last 5
```

The database is stored as `fluxo.db` beside the generated console executable.
Commands that create movements require an account identifier because that is a
required field in the Application contract:

```bash
dotnet run --project src/Fluxo.Console -- add expense 18500 "Supermercado" \
    --account 00000000-0000-0000-0000-000000000001 --currency ARS
```

The current implementation includes the command parser, direct command mode,
interactive shell, in-session history, command dispatching and terminal
renderers. `add`, `list`/`movements`, `summary`/`balance` and `help` are
available. New behavior should continue to call Application services instead
of repositories or Domain entities directly.

---

# Dependency direction

```text
User
 │
 ▼
Fluxo.Console
 │
 ▼
Fluxo.Application
 │
 ▼
Fluxo.Domain
```

Infrastructure dependencies are resolved through the application's configured
dependency injection/bootstrap process.

Fluxo.Console should never directly access repositories, SQLite or other
persistence implementations.

---

# Program.cs

`Program.cs` is only the application's entry point.

It should remain intentionally small.

Its responsibilities are limited to:

1. Configure dependencies.
2. Build the console application.
3. Start Fluxo.Console.

Conceptually:

```csharp
var app = bootstrapper.Build();

await app.RunAsync();
```

Application behavior belongs in dedicated classes rather than `Program.cs`.

---

# Short description of each .cs file

## Program.cs

Entry point. Configures Fluxo.Console and starts the application. It should
contain as little logic as possible.

## FolderMarker.cs

Empty class used as a stable reference to the assembly/project, mainly for
reflection, dependency injection or discovery.

## Commands/AddCommand.cs

Executes commands that create financial movements. It translates console
arguments into calls to application use cases.

## Commands/ListCommand.cs

Retrieves and displays collections of financial movements according to filters
received from the console.

## Commands/SummaryCommand.cs

Requests and presents financial summaries such as income, expenses, balance and
aggregate metrics.

## Commands/HelpCommand.cs

Displays available commands, syntax, options and contextual help.

## Parsing/CommandParser.cs

Converts text entered by the user into a structured command representation.

## Parsing/ParsedCommand.cs

Represents the parsing result: command name, detected arguments and options.

## Parsing/CommandArguments.cs

Encapsulates positional arguments and command options, providing convenient
access to them.

## Shell/FluxoShell.cs

Controls Fluxo's interactive mode: prompt, input reading and the main execution
loop.

## Shell/CommandDispatcher.cs

Determines which command or handler should execute for a given `ParsedCommand`.

## Shell/CommandHistory.cs

Manages the history of commands executed during an interactive session.

## UI/ConsoleRenderer.cs

General console renderer. Centralizes standard messages, errors, success output
and basic output blocks.

## UI/TableRenderer.cs

Renders tabular information in a terminal-friendly, readable format.

## UI/DashboardRenderer.cs

Renders richer financial views such as balances, statistics, bars and summaries.

## UI/PromptRenderer.cs

Renders the interactive prompt and visual elements related to user input.

## UI/Theme.cs

Defines Fluxo.Console's consistent appearance: symbols, styles, colors and visual
conventions.
