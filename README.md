# Allors2

[![CI](https://github.com/allors/allors2/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/allors/allors2/actions/workflows/ci.yml)

Allors is a comprehensive .NET-based application development platform that provides a meta-model driven architecture for building enterprise applications. It features an object-relational mapping system, code generation, multi-tier architecture, and client-side workspace synchronization.

## Table of Contents

- [Overview](#overview)
- [Key Features](#key-features)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Core Concepts](#core-concepts)
- [Database Layer](#database-layer)
- [Workspace Layer](#workspace-layer)
- [Code Generation](#code-generation)
- [Building the Project](#building-the-project)
- [Running the Application](#running-the-application)
- [Testing](#testing)
- [Development Guide](#development-guide)
- [Configuration](#configuration)
- [Documentation](#documentation)
- [Contributing](#contributing)
- [License](#license)

---

## Overview

Allors is a platform for building business applications using a **meta-model driven approach**. Instead of writing repetitive CRUD code, you define your domain model using the Allors meta-model, and the platform generates the necessary infrastructure code for:

- **Object-relational mapping (ORM)** with intelligent relation storage strategies
- **Domain classes** with full IntelliSense support
- **Database persistence** using SQL Server or in-memory storage
- **Client-side workspace** for disconnected operations
- **REST API** for client-server communication
- **TypeScript/JavaScript client libraries** for web applications

### Philosophy

Allors follows these principles:

1. **Meta-Model First**: Define your domain once, generate everywhere
2. **Multi-Tier Architecture**: Clear separation between Database and Workspace layers
3. **Protocol-Based Communication**: Standardized JSON protocol for database-workspace sync
4. **Type Safety**: Strong typing in both C# and TypeScript
5. **Performance**: Optimized for real-world enterprise applications with batching and caching

---

## Key Features

### 🎯 Meta-Model Driven Development
- Define domain model using C# attributes and interfaces
- Automatic code generation for domain classes, repositories, and more
- Support for inheritance, interfaces, and complex relationships
- Comprehensive validation and constraint system

### 💾 Advanced ORM
- **Adaptive Storage**: Relations stored inline or in separate tables based on cardinality
- **Two Database Adapters**:
  - `SqlClient`: Full-featured SQL Server adapter with stored procedures
  - `Memory`: In-memory adapter for testing and prototyping
- **Optimistic Concurrency**: Version-based conflict detection
- **Unit of Work**: Session-based transaction management with change tracking
- **Batch Operations**: Table-valued parameters for efficient bulk operations
- **Prefetching**: Sophisticated prefetch policies to minimize database round-trips

### 🌐 Workspace System
- **Client-Side Domain**: Full domain model available on client
- **Disconnected Operations**: Work with objects without constant server round-trips
- **Synchronization Protocol**: Efficient sync between database and workspace
- **Change Tracking**: Track all changes for conflict resolution
- **Multiple Adapters**: Support for different client platforms (C#, TypeScript, etc.)

### 🔧 Code Generation
- Domain classes with properties and methods
- Repository pattern implementations
- Unit tests
- Documentation (HTML, Markdown)
- UML diagrams
- Mermaid class diagrams
- XML relation definitions

### 🏗️ Enterprise-Ready
- Dependency injection support
- Comprehensive testing infrastructure
- Excel integration for data import/export
- Blazor components for web UIs
- Multi-tenancy support via schema separation

---

## Architecture

Allors uses a **three-layer architecture**:

```
┌─────────────────────────────────────────────────────────────┐
│                      Client Layer                            │
│  - TypeScript/JavaScript Workspace                          │
│  - C# Workspace (Excel, Blazor)                             │
│  - Local change tracking and caching                        │
└────────────────────────┬────────────────────────────────────┘
                         │
                         │ JSON Protocol (Push/Pull)
                         ↓
┌─────────────────────────────────────────────────────────────┐
│                      Server Layer                            │
│  - ASP.NET Core Web API                                     │
│  - Allors.Protocol (Request/Response handling)              │
│  - Business logic and workflows                             │
└────────────────────────┬────────────────────────────────────┘
                         │
                         │ ISession interface
                         ↓
┌─────────────────────────────────────────────────────────────┐
│                     Database Layer                           │
│  - IDatabase interface                                      │
│  - Database Adapters (SqlClient, Memory)                    │
│  - Object-relational mapping                                │
│  - SQL Server or in-memory storage                          │
└─────────────────────────────────────────────────────────────┘
```

### Key Interfaces

```csharp
// Database layer
IDatabase → Creates sessions
ISession → Unit of work, transaction boundary
IObject → Domain object interface
IStrategy → Object behavior and state management

// Meta-model
IMetaPopulation → Contains all type definitions
IClass → Concrete class definition
IInterface → Interface definition
IRelationType → Defines relationships between types
IRoleType → Target of a relation (property)
IAssociationType → Source of a relation (back-reference)

// Workspace layer
IWorkspace → Client-side container for objects
IWorkspaceObject → Client representation of domain object
```

---

## Project Structure

The solution is organized into two main sections: **Platform** and **Core**.

```
Allors2/
├── Platform/                    # Reusable platform components
│   ├── Repository/             # Meta-model definition system
│   │   ├── Allors.Repository/  # Repository infrastructure
│   │   ├── Domain/             # Attributes for meta-model
│   │   └── Generate/           # Code generation from repository
│   │
│   ├── Database/               # Database abstraction layer
│   │   ├── Allors.Database/    # Core interfaces (IDatabase, ISession, etc.)
│   │   └── Adapters/           # Database implementations
│   │       ├── Allors.Database.Adapters/          # Shared adapter code
│   │       ├── Allors.Database.Adapters.Memory/   # In-memory adapter
│   │       └── Allors.Database.Adapters.SqlClient/# SQL Server adapter
│   │
│   ├── Workspace/              # Client-side workspace system
│   │   └── CSharp/
│   │       ├── Allors.Workspace/         # Core workspace interfaces
│   │       └── Adapters/                 # Workspace implementations
│   │           ├── Allors.Workspace.Adapters/
│   │           ├── Allors.Workspace.Adapters.Remote/
│   │           └── Allors.Workspace.Adapters.Remote.RestSharp/
│   │
│   └── Protocol/               # JSON protocol for database-workspace sync
│       └── Allors.Protocol/    # Protocol message definitions
│
├── Core/                       # Application-specific implementation
│   ├── Repository/             # Domain model definition
│   │   └── Domain/             # Domain classes with attributes
│   │
│   ├── Database/               # Server-side database implementation
│   │   ├── Meta/               # Runtime meta-model
│   │   ├── Domain/             # Generated domain classes
│   │   ├── Generate/           # Code generation execution
│   │   ├── Commands/           # CLI commands
│   │   ├── Server/             # ASP.NET Core server
│   │   └── Services/           # Business services
│   │
│   └── Workspace/              # Client-side implementations
│       ├── CSharp/
│       │   ├── Domain/         # Generated workspace classes
│       │   ├── Meta/           # Client-side meta-model
│       │   ├── Excel/          # Excel integration
│       │   └── Blazor/         # Blazor components
│       │
│       └── Typescript.legacy/  # TypeScript/JavaScript workspace
│
├── build/                      # Build automation (NUKE)
│   ├── Build.cs                # Main build script
│   ├── Build.Core.cs           # Core build targets
│   └── Build.Platform.cs       # Platform build targets
│
└── mapping.md                  # ORM mapping documentation (see this file!)
```

### Solution Projects

The solution contains approximately 40+ projects organized into:

- **Platform Projects**: Reusable infrastructure (~20 projects)
- **Core Projects**: Application-specific implementation (~20 projects)
- **Build Project**: NUKE build automation (1 project)

---

## Prerequisites

### Required Software

1. **.NET 9.0 SDK or later**
   - Download from: https://dotnet.microsoft.com/download

2. **SQL Server 2019 or later** (for SqlClient adapter)
   - SQL Server Express is sufficient
   - LocalDB is supported for development
   - Download from: https://www.microsoft.com/sql-server/sql-server-downloads

3. **Node.js 18.x or later** (for TypeScript workspace)
   - Download from: https://nodejs.org/

4. **Visual Studio 2022** or **VS Code** (recommended)
   - Visual Studio: Community edition or higher
   - VS Code with C# Dev Kit extension

### Optional Tools

- **SQL Server Management Studio (SSMS)**: For database management
- **Git**: For version control
- **NUKE Global Tool**: For build automation (`dotnet tool install Nuke.GlobalTool --global`)

---

## Getting Started

### Quick Start (Windows)

1. **Clone the Repository**
   ```bash
   git clone https://github.com/Allors/allors2.git
   cd allors2
   ```

2. **Configure the Environment**
   - Copy the `config` directory to your drive root (e.g., `C:\config`)
   - The config contains database connection strings and environment settings

3. **Create the Database**
   ```sql
   CREATE DATABASE allors;
   ```
   Or use the name specified in your config files

4. **Build the Project**
   ```bash
   # Using NUKE (recommended)
   nuke

   # Or using .NET CLI
   dotnet build
   ```

5. **Populate the Database**
   ```bash
   # Navigate to Core\Database\Commands
   cd Core\Database\Commands

   # Run the commands tool
   Commands.cmd

   # In the commands prompt:
   populate          # Creates schema and basic data
   # OR
   populate -d       # Creates schema with demo data
   ```

6. **Run the Server**
   ```bash
   # Start the backend API server
   Server.cmd
   ```

7. **Run the Client** (if applicable)
   ```bash
   # Start the frontend (from appropriate workspace directory)
   # Example for Blazor:
   cd Core\Workspace\CSharp\Blazor\Blazor.Bootstrap.ServerSide
   dotnet run
   ```

8. **Login**
   - Navigate to the URL shown in the console
   - Username: `administrator`
   - Password: (leave empty)

### Building from Scratch

```bash
# Clean build from scratch
nuke --target Clean
nuke

# Run tests
nuke --target Test

# Generate code
nuke --target Generate
```

---

## Core Concepts

### Meta-Model

The meta-model is the foundation of Allors. It defines types, properties, and relationships.

**Key Meta-Model Concepts**:

- **ObjectType**: Base for all types (Classes, Interfaces, Units)
- **Class**: Concrete type that can be instantiated
- **Interface**: Abstract type for polymorphism
- **Unit**: Primitive type (String, Integer, DateTime, etc.)
- **RelationType**: Defines a relationship between types
  - **AssociationType**: The "from" side (e.g., Person)
  - **RoleType**: The "to" side (e.g., Address)
- **MethodType**: Defines methods on objects

### Defining Domain Model

Domain model is defined in the `Core/Repository/Domain` project using C# attributes:

```csharp
namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("8f0b4f3e-4f3e-4f3e-4f3e-4f3e4f3e4f3e")]
    #endregion
    public partial class Person : Object
    {
        #region Allors
        [Id("a1b2c3d4-e5f6-4a5b-6c7d-8e9f0a1b2c3d")]
        [Size(256)]
        #endregion
        public string FirstName { get; set; }

        #region Allors
        [Id("b2c3d4e5-f6a7-4b5c-6d7e-8f9a0b1c2d3e")]
        [Size(256)]
        #endregion
        public string LastName { get; set; }

        #region Allors
        [Id("c3d4e5f6-a7b8-4c5d-6e7f-8a9b0c1d2e3f")]
        #endregion
        public DateTime? BirthDate { get; set; }

        #region Allors
        [Id("d4e5f6a7-b8c9-4d5e-6f7a-8b9c0d1e2f3a")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        public Organization Employer { get; set; }

        #region Allors
        [Id("e5f6a7b8-c9d0-4e5f-6a7b-8c9d0e1f2a3b")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        public Skill[] Skills { get; set; }

        #region Allors
        [Id("f6a7b8c9-d0e1-4f5a-6b7c-8d9e0f1a2b3c")]
        #endregion
        public void DoSomething()
        {
            // Method implementation
        }
    }
}
```

**Key Attributes**:
- `[Id]`: Unique identifier (GUID) for meta-model element
- `[Size]`: Maximum length for strings
- `[Precision]`, `[Scale]`: For decimal types
- `[Multiplicity]`: Relationship cardinality (OneToOne, OneToMany, ManyToOne, ManyToMany)
- `[Indexed]`: Create database index
- `[Derived]`: Property is calculated, not stored
- `[Required]`: Property must have a value

### Object Lifecycle

```csharp
// Create a session (unit of work)
using var session = database.CreateSession();

// Create objects
var person = session.Create<Person>();
person.FirstName = "John";
person.LastName = "Doe";

var org = session.Create<Organization>();
org.Name = "Acme Corp";

// Set relationships
person.Employer = org;

// Commit changes
session.Commit();  // Flush to database, increment versions

// OR rollback
// session.Rollback();  // Discard all changes
```

### Querying

```csharp
// Extent-based queries
var extent = session.Extent<Person>();
extent.Filter.AddEquals(m.Person.FirstName, "John");
extent.Filter.AddGreaterThan(m.Person.BirthDate, new DateTime(1990, 1, 1));

var persons = extent.ToArray();

// Or use LINQ-style
var persons = session.Extent<Person>()
    .Where(p => p.FirstName == "John")
    .ToArray();
```

### Prefetching

Minimize database round-trips with prefetch policies:

```csharp
var prefetchPolicy = new PrefetchPolicyBuilder()
    .WithRule(m.Person.Employer)
    .WithRule(m.Person.Skills)
    .WithRule(m.Person.Orders, new PrefetchPolicyBuilder()
        .WithRule(m.Order.OrderLines)
        .WithRule(m.Order.ShipToAddress))
    .Build();

session.Prefetch(prefetchPolicy, persons);

// Now accessing Employer, Skills, Orders won't cause additional queries
foreach (var person in persons)
{
    Console.WriteLine($"{person.FirstName} works at {person.Employer.Name}");
    foreach (var skill in person.Skills)
    {
        Console.WriteLine($"  - {skill.Name}");
    }
}
```

---

## Database Layer

### Database Adapters

Allors provides two database adapters:

#### 1. SqlClient Adapter (Production)

**Location**: `Platform/Database/Adapters/Allors.Database.Adapters.SqlClient`

**Features**:
- Full SQL Server persistence
- Intelligent relation mapping (inline columns vs. relation tables)
- Stored procedures for all operations
- Table-valued parameters for batch operations
- Multi-level caching (session, database, SQL Server)
- Snapshot isolation for optimistic concurrency
- Schema validation and automatic initialization

**Configuration**:
```csharp
var configuration = new Configuration
{
    ObjectFactory = new ObjectFactory(metaPopulation, typeof(User)),
    ConnectionString = "Server=localhost;Database=allors;Integrated Security=true;",
    CommandTimeout = 600,
    IsolationLevel = IsolationLevel.Snapshot
};

var database = new Database(serviceProvider, configuration);
```

**See `mapping.md` for detailed ORM documentation.**

#### 2. Memory Adapter (Testing)

**Location**: `Platform/Database/Adapters/Allors.Database.Adapters.Memory`

**Features**:
- In-memory object storage
- No external dependencies
- Perfect for unit tests
- Fast and lightweight
- Implements same IDatabase interface

**Usage**:
```csharp
var configuration = new Configuration
{
    ObjectFactory = new ObjectFactory(metaPopulation, typeof(User))
};

var database = new Database(serviceProvider, configuration);
```

### Database Sessions

All database operations occur within a session:

```csharp
using (var session = database.CreateSession())
{
    try
    {
        // Create, read, update, delete operations
        var person = session.Create<Person>();
        person.FirstName = "John";

        // Commit the transaction
        session.Commit();
    }
    catch
    {
        session.Rollback();
        throw;
    }
}
```

**Session Features**:
- Transaction boundary
- Change tracking via ChangeSet
- Object identity within session
- Unit of work pattern
- Automatic flush before queries

### Change Tracking

```csharp
// Get changes since last checkpoint
var changeSet = session.Checkpoint();

// Check what changed
foreach (var objectId in changeSet.Created)
{
    Console.WriteLine($"Object {objectId} was created");
}

foreach (var objectId in changeSet.Deleted)
{
    Console.WriteLine($"Object {objectId} was deleted");
}

foreach (var objectId in changeSet.Associations)
{
    var roleTypes = changeSet.RoleTypes[objectId];
    Console.WriteLine($"Object {objectId} changed: {string.Join(", ", roleTypes)}");
}
```

---

## Workspace Layer

The Workspace layer provides a **client-side domain model** that can operate disconnected from the database.

### Architecture

```
┌──────────────────────────────────────────┐
│        Client Application                 │
│  (Blazor, WPF, Console, Excel, etc.)    │
└───────────────┬──────────────────────────┘
                │
                ↓
┌──────────────────────────────────────────┐
│         IWorkspace                        │
│  - Client-side object container          │
│  - Change tracking                       │
│  - Push/Pull synchronization             │
└───────────────┬──────────────────────────┘
                │
                ↓
┌──────────────────────────────────────────┐
│     IWorkspaceObject                      │
│  - Client representation of objects      │
│  - Local change tracking                 │
│  - Lazy loading of properties            │
└───────────────┬──────────────────────────┘
                │
                ↓ (via Adapter)
┌──────────────────────────────────────────┐
│         Server (Database)                 │
│  - Authoritative data source             │
│  - Handles conflicts                     │
└──────────────────────────────────────────┘
```

### Workspace Operations

```csharp
// Create workspace with remote connection
var configuration = new Configuration
{
    Url = "https://api.example.com"
};
var workspace = new Workspace(configuration);

// Pull objects from server
var pullRequest = new Pull
{
    ObjectType = m.Person,
    Results = new[]
    {
        new Result { Fetch = new Fetch { Include = new Tree(m.Person.Employer) } }
    }
};

var pullResponse = await workspace.PullAsync(pullRequest);
var persons = pullResponse.GetCollection<Person>();

// Work with objects locally
var person = persons.First();
person.FirstName = "Jane";  // Local change, not yet on server

// Push changes back to server
var pushRequest = new PushRequest();
var pushResponse = await workspace.PushAsync(pushRequest);

if (pushResponse.HasErrors)
{
    // Handle conflicts or errors
}
```

### Benefits of Workspace

1. **Reduced Server Round-Trips**: Load once, work locally
2. **Offline Support**: Continue working without connection
3. **Optimistic Concurrency**: Detect and resolve conflicts
4. **Lazy Loading**: Load related objects on demand
5. **Change Tracking**: Know exactly what changed

---

## Code Generation

Allors uses **StringTemplate 4** for code generation from the meta-model.

### Generation Process

```
Repository Definition (C# with attributes)
    ↓
Meta-Model Parser
    ↓
In-Memory Meta-Model (IMetaPopulation)
    ↓
StringTemplate Templates (.stg files)
    ↓
Generated Code
    ↓
- Domain.cs (database classes)
    - Meta.cs (meta-model)
    - Repository.cs (repository definitions)
    - UML diagrams
    - Documentation
    - Mermaid diagrams
    - XML relation definitions
```

### Templates Location

**Templates**: `Core/Database/Templates/*.stg`

Available templates:
- `domain.cs.stg`: Generated domain classes for database
- `repository.cs.stg`: Repository class definitions
- `uml.cs.stg`: UML class diagrams
- `mermaid.stg`: Mermaid class diagrams
- `docs.html.stg`: HTML documentation
- `relations.xml.stg`: XML relation definitions

### Running Code Generation

```bash
# From Core/Database/Generate
dotnet run

# Or using NUKE
nuke --target Generate
```

### Generated Code Example

From repository definition:
```csharp
[Id("...")]
public partial class Person : Object
{
    [Id("...")]
    public string FirstName { get; set; }
}
```

Generated domain class:
```csharp
public partial class Person : Allors.IObject
{
    public string FirstName
    {
        get => this.Strategy.GetRole<string>(M.Person.FirstName);
        set => this.Strategy.SetRole(M.Person.FirstName, value);
    }

    // Additional generated code for associations, methods, etc.
}
```

---

## Building the Project

### Using NUKE (Recommended)

NUKE is a build automation tool that provides typed build scripts.

```bash
# Install NUKE global tool (one time)
dotnet tool install Nuke.GlobalTool --global

# Show available targets
nuke --help

# Clean build
nuke --target Clean
nuke

# Run specific target
nuke --target Test
nuke --target Generate
nuke --target Publish

# Run Core build
nuke --target Core

# Run Platform build
nuke --target Platform
```

### Common Build Targets

- `Clean`: Clean all build outputs
- `Restore`: Restore NuGet packages
- `Generate`: Run code generation
- `Compile`: Compile all projects
- `Test`: Run all tests
- `Core`: Build Core projects
- `Platform`: Build Platform projects
- `Publish`: Create deployment packages

### Using .NET CLI

```bash
# Restore packages
dotnet restore

# Build solution
dotnet build

# Build specific project
dotnet build Core/Database/Domain/Domain.csproj

# Run tests
dotnet test

# Clean
dotnet clean
```

### Build Configuration

The build system is defined in:
- `build/Build.cs`: Main build entry point
- `build/Build.Core.cs`: Core-specific build logic
- `build/Build.Platform.cs`: Platform-specific build logic
- `build/Configuration.cs`: Build configuration

---

## Running the Application

### Backend (Server)

The server hosts the ASP.NET Core Web API that provides access to the database.

**Location**: `Core/Database/Server`

```bash
# Using command script
cd Core
Server.cmd

# Or using .NET CLI
cd Core/Database/Server
dotnet run

# Server will start on https://localhost:5001
```

**Server Features**:
- RESTful API endpoints
- JSON protocol for Push/Pull operations
- Dependency injection
- Middleware for authentication/authorization
- Swagger/OpenAPI documentation

### Frontend (Workspace Clients)

#### Blazor

```bash
cd Core/Workspace/CSharp/Blazor/Blazor.Bootstrap.ServerSide
dotnet run
```

#### Excel Add-In

Excel integration for data import/export:
```bash
cd Core/Workspace/CSharp/Excel/ExcelDNA
dotnet build
# Register the add-in in Excel
```

#### TypeScript/JavaScript (Legacy)

```bash
cd Core/Workspace/Typescript.legacy
npm install
npm start
```

### CLI Commands

The Commands project provides CLI utilities:

```bash
cd Core/Database/Commands
Commands.cmd

# Available commands:
# - populate: Initialize database schema
# - populate -d: Initialize with demo data
# - generate: Run code generation
# - test: Run domain tests
```

---

## Testing

### Test Projects

- `Platform/Database/Adapters/Tests.Static`: Static adapter tests
- `Platform/Database/Adapters/Tests.Dynamic`: Dynamic adapter tests
- `Core/Database/Domain.Tests`: Domain-specific tests
- `Core/Database/Server.Tests`: Server API tests
- `Core/Workspace/CSharp/Domain.Tests`: Workspace tests

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test Core/Database/Domain.Tests/Domain.Tests.csproj

# Run tests with filter
dotnet test --filter "FullyQualifiedName~Person"

# Using NUKE
nuke --target Test
```

### Test Structure

```csharp
[Fact]
public void WhenCreatingPerson_ThenPropertiesAreSet()
{
    // Arrange
    var session = this.Database.CreateSession();
    var m = this.Database.Services().M;

    // Act
    var person = session.Create<Person>();
    person.FirstName = "John";
    person.LastName = "Doe";
    session.Commit();

    // Assert
    Assert.Equal("John", person.FirstName);
    Assert.Equal("Doe", person.LastName);
}
```

### Memory Adapter for Testing

Use the Memory adapter for fast, isolated tests:

```csharp
public class PersonTests : IDisposable
{
    private readonly IDatabase database;

    public PersonTests()
    {
        // Use Memory adapter
        var configuration = new Allors.Database.Adapters.Memory.Configuration
        {
            ObjectFactory = new ObjectFactory(metaPopulation, typeof(Person))
        };

        this.database = new Allors.Database.Adapters.Memory.Database(
            serviceProvider,
            configuration);
    }

    [Fact]
    public void Test()
    {
        using var session = this.database.CreateSession();
        // Test code...
    }
}
```

---

## Development Guide

### Setting Up Development Environment

1. **Clone and Build**
   ```bash
   git clone https://github.com/Allors/allors2.git
   cd allors2
   nuke
   ```

2. **Open in IDE**
   - Visual Studio: Open `Allors.sln`
   - VS Code: Open folder and use C# Dev Kit

3. **Configure Database**
   - Create SQL Server database
   - Update connection string in `C:\config\allors.database.config`

4. **Generate Code**
   ```bash
   cd Core/Database/Generate
   dotnet run
   ```

5. **Populate Database**
   ```bash
   cd Core/Database/Commands
   Commands.cmd
   populate
   ```

### Development Workflow

1. **Define Domain Model**
   - Edit files in `Core/Repository/Domain`
   - Add/modify classes with Allors attributes
   - Assign unique GUIDs to all `[Id]` attributes

2. **Generate Code**
   ```bash
   nuke --target Generate
   ```

3. **Implement Business Logic**
   - Generated classes are `partial`
   - Add your logic in separate files:
     ```csharp
     // Generated: Person.g.cs
     public partial class Person { ... }

     // Your code: Person.cs
     public partial class Person
     {
         public string FullName => $"{FirstName} {LastName}";

         public void DoSomething()
         {
             // Your implementation
         }
     }
     ```

4. **Update Database Schema**
   ```bash
   Commands.cmd
   populate  # Reinitializes schema
   ```

5. **Write Tests**
   - Add tests in `Core/Database/Domain.Tests`
   - Use Memory adapter for fast tests

6. **Commit Changes**
   ```bash
   git add .
   git commit -m "Add Person.FullName property"
   ```

### Best Practices

#### Domain Model Design

- **Use meaningful names**: `FirstName` not `FN`
- **Add XML documentation**: Will be included in generated code
- **Assign unique GUIDs**: Never reuse IDs
- **Use interfaces for polymorphism**: Define common behavior
- **Keep validation in meta-model**: Use `[Required]`, `[Size]`, etc.

#### Performance

- **Use prefetch policies**: Minimize database round-trips
- **Batch operations**: Create multiple objects in one session.Commit()
- **Use extents for queries**: More efficient than loading all objects
- **Monitor SQL**: Enable logging to see generated queries
- **Use indexed properties**: Add `[Indexed]` for frequently queried properties

#### Testing

- **Test with Memory adapter**: Fast and isolated
- **Test with SqlClient adapter**: Integration tests
- **Use fixture pattern**: Share database setup across tests
- **Test associations**: Verify bidirectional relationships
- **Test cascading operations**: Verify deletion behavior

---

## Configuration

### Database Configuration

**Location**: `C:\config\allors.database.config` (or `~/.config/allors.database.config` on Linux/Mac)

```json
{
  "ConnectionString": "Server=localhost;Database=allors;Integrated Security=true;TrustServerCertificate=true;",
  "SchemaName": "allors",
  "CommandTimeout": 600
}
```

### Server Configuration

**Location**: `Core/Database/Server/appsettings.json`

```json
{
  "AllorsDatabase": {
    "ConnectionString": "Server=localhost;Database=allors;Integrated Security=true;",
    "SchemaName": "allors"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  }
}
```

### Workspace Configuration

**Location**: Varies by workspace type

Example for Remote workspace:
```csharp
var configuration = new Configuration
{
    Url = "https://api.example.com",
    AuthenticationTokenUrl = "https://api.example.com/auth/token"
};
```

---

## Documentation

### Core Documentation Files

- **`README.md`**: This file - comprehensive project overview
- **`mapping.md`**: Deep dive into object-relational mapping
- **`Core/Database/Templates/`**: Code generation templates
- **XML Comments**: Inline documentation in code

### Generated Documentation

Run code generation to produce:
- HTML documentation (from `docs.html.stg`)
- UML diagrams (from `uml.cs.stg`)
- Mermaid diagrams (from `mermaid.stg`)

### Additional Resources

- **Allors Website**: [https://www.allors.com](https://www.allors.com)
- **GitHub Issues**: Report bugs and request features
- **Stack Overflow**: Tag questions with `allors`

### Meta-Model Documentation

The meta-model is self-documenting. Access it at runtime:

```csharp
var metaPopulation = database.MetaPopulation;

foreach (var @class in metaPopulation.Classes)
{
    Console.WriteLine($"Class: {@class.Name}");

    foreach (var roleType in @class.RoleTypes)
    {
        Console.WriteLine($"  Property: {roleType.Name}");
        Console.WriteLine($"    Type: {roleType.ObjectType.Name}");
        Console.WriteLine($"    Multiplicity: {roleType.IsOne ? "One" : "Many"}");
    }
}
```

---

## Contributing

### How to Contribute

1. **Fork the Repository**
   ```bash
   # On GitHub, click "Fork"
   git clone https://github.com/YOUR-USERNAME/allors2.git
   cd allors2
   ```

2. **Create a Branch**
   ```bash
   git checkout -b feature/my-new-feature
   ```

3. **Make Changes**
   - Follow coding standards
   - Add tests for new features
   - Update documentation

4. **Test Your Changes**
   ```bash
   nuke --target Test
   ```

5. **Commit and Push**
   ```bash
   git add .
   git commit -m "Add my new feature"
   git push origin feature/my-new-feature
   ```

6. **Create Pull Request**
   - Go to GitHub and create PR
   - Describe your changes
   - Link related issues

### Coding Standards

- **C# Conventions**: Follow Microsoft C# coding conventions
- **Naming**: PascalCase for public members, camelCase for private
- **Comments**: XML documentation for public APIs
- **Testing**: Maintain >80% code coverage
- **Git Commits**: Clear, descriptive commit messages

### Reporting Issues

When reporting bugs, include:
- Allors version
- .NET version
- Database adapter (Memory or SqlClient)
- Steps to reproduce
- Expected vs actual behavior
- Error messages and stack traces

---

## License

Allors is licensed under the **LGPL v3** (GNU Lesser General Public License).

### What This Means

**You can**:
- Use Allors in commercial applications
- Modify Allors source code
- Distribute applications built with Allors

**You must**:
- Keep Allors modifications open source (LGPL)
- Include license and copyright notices
- Provide source code for Allors modifications

**You don't have to**:
- Open source your application code
- Release your domain model
- Pay licensing fees

### Full License

See the `LICENSE` file in the repository root.

---

## Troubleshooting

### Common Issues

#### Database Connection Errors

**Problem**: Cannot connect to SQL Server

**Solution**:
1. Verify SQL Server is running
2. Check connection string in config
3. Ensure user has appropriate permissions
4. For LocalDB: Install SQL Server LocalDB
5. Enable TCP/IP in SQL Server Configuration Manager

#### Code Generation Fails

**Problem**: Generate command fails or produces incorrect code

**Solution**:
1. Ensure all `[Id]` attributes have unique GUIDs
2. Check for circular references in meta-model
3. Verify all required attributes are present
4. Clean and rebuild: `nuke --target Clean && nuke`

#### Schema Validation Errors

**Problem**: Database schema doesn't match meta-model

**Solution**:
```bash
Commands.cmd
populate  # Reinitializes database schema
```

**Warning**: This drops all existing data!

#### Session Conflicts

**Problem**: Optimistic concurrency conflicts

**Solution**:
```csharp
try
{
    session.Commit();
}
catch (Exception)
{
    // Reload object and retry
    session.Rollback();
    // Re-fetch and retry operation
}
```

### Getting Help

1. **Check Documentation**: Read `mapping.md` for ORM details
2. **Search Issues**: Check GitHub issues for similar problems
3. **Ask on Stack Overflow**: Tag with `allors`
4. **Create GitHub Issue**: For bugs and feature requests
5. **Contact Community**: Join Allors community channels

---

## Migration from Allors v2 to v3

If you're working with both versions:

### Adding Allors v3 as Upstream

```bash
git remote add upstream https://github.com/Allors/allors3.git
git remote -v  # Confirm addition
```

### Pulling Changes from v3

```bash
git fetch upstream
git checkout -b v3-integration
git merge upstream/main

# Resolve conflicts
git add .
git commit -m "Integrate Allors v3 changes"
```

---

## Quick Reference

### Key Commands

```bash
# Build
nuke                           # Full build
nuke --target Clean            # Clean build
nuke --target Test             # Run tests
nuke --target Generate         # Generate code

# Database
Commands.cmd                   # Open commands CLI
populate                       # Initialize schema
populate -d                    # Initialize with demo data

# Run
Server.cmd                     # Start backend
dotnet run                     # Run specific project

# Development
dotnet build                   # Build solution
dotnet test                    # Run tests
dotnet clean                   # Clean outputs
```

### Key Directories

```bash
Platform/                      # Reusable platform
Core/                          # Application code
build/                         # Build scripts
C:\config\                     # Configuration files
```

### Key Files

```bash
Allors.sln                     # Visual Studio solution
build.cmd                      # Build script
mapping.md                     # ORM documentation
README.md                      # This file
LICENSE                        # License information
```

---

## Acknowledgments

Allors is developed and maintained by the Allors team and community contributors.

- **Lead Developer**: Koen Van Exem
- **Contributors**: See GitHub contributors page
- **Community**: Thanks to all who report issues and contribute

---

## Version Information

- **Current Version**: 2.x
- **Target Framework**: .NET 9.0
- **Minimum .NET Version**: .NET 9.0
- **Database Support**: SQL Server 2019+, LocalDB
- **Browser Support**: Modern browsers (Chrome, Firefox, Edge, Safari)

---

## Contact

- **Website**: https://www.allors.com
- **GitHub**: https://github.com/Allors/allors2
- **Issues**: https://github.com/Allors/allors2/issues
- **Email**: info@allors.com

---

**Last Updated**: 2025-01-21
**Document Version**: 1.0
**Allors Version**: 2.x

---

## Appendix: Architecture Diagrams

### Complete System Architecture

```
┌────────────────────────────────────────────────────────────────┐
│                     Client Applications                         │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐        │
│  │   Browser    │  │    Excel     │  │   Console    │        │
│  │  (Angular,   │  │   Add-In     │  │     App      │        │
│  │   Blazor)    │  │              │  │              │        │
│  └──────┬───────┘  └──────┬───────┘  └──────┬───────┘        │
└─────────┼──────────────────┼──────────────────┼────────────────┘
          │                  │                  │
          │ JSON Protocol    │ .NET API         │ .NET API
          ↓                  ↓                  ↓
┌────────────────────────────────────────────────────────────────┐
│              Workspace Layer (Client-Side)                      │
│  ┌────────────────────────────────────────────────────────────┐│
│  │          IWorkspace Implementation                          ││
│  │  - Local object cache                                       ││
│  │  - Change tracking                                          ││
│  │  - Push/Pull synchronization                               ││
│  └────────────────────────────────────────────────────────────┘│
└─────────────────────────────┬──────────────────────────────────┘
                              │
                              │ HTTP/REST
                              ↓
┌────────────────────────────────────────────────────────────────┐
│                    Server Layer                                 │
│  ┌────────────────────────────────────────────────────────────┐│
│  │          ASP.NET Core Web API                               ││
│  │  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐    ││
│  │  │   Protocol   │  │   Services   │  │     Auth     │    ││
│  │  │   Handlers   │  │   Layer      │  │  Middleware  │    ││
│  │  └──────┬───────┘  └──────┬───────┘  └──────────────┘    ││
│  └─────────┼──────────────────┼─────────────────────────────────┘│
└────────────┼──────────────────┼──────────────────────────────────┘
             │                  │
             │ ISession         │
             ↓                  ↓
┌────────────────────────────────────────────────────────────────┐
│                  Database Layer                                 │
│  ┌────────────────────────────────────────────────────────────┐│
│  │              IDatabase Implementation                        ││
│  │  ┌──────────────────────┐  ┌──────────────────────┐        ││
│  │  │   SqlClient Adapter  │  │   Memory Adapter     │        ││
│  │  │  - SQL Server        │  │  - In-Memory         │        ││
│  │  │  - Stored Procs      │  │  - For Testing       │        ││
│  │  │  - TVPs              │  │  - Fast & Simple     │        ││
│  │  └──────────┬───────────┘  └──────────────────────┘        ││
│  └─────────────┼────────────────────────────────────────────────┘│
└────────────────┼─────────────────────────────────────────────────┘
                 │
                 ↓
┌────────────────────────────────────────────────────────────────┐
│                    Data Storage                                 │
│  ┌────────────────────┐       ┌────────────────────┐          │
│  │   SQL Server       │       │   In-Memory        │          │
│  │   - Tables         │       │   - Dictionaries   │          │
│  │   - Indices        │       │   - Fast Access    │          │
│  │   - Procedures     │       │                    │          │
│  └────────────────────┘       └────────────────────┘          │
└────────────────────────────────────────────────────────────────┘
```

---

**End of README.md**

For detailed information about the object-relational mapping, see **`mapping.md`**.
