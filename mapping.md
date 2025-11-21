# Object-Relational Mapping in Allors SqlClient Adapter

## Table of Contents
1. [Overview](#overview)
2. [Core Architecture](#core-architecture)
3. [Database Schema](#database-schema)
4. [Object Identity and Lifecycle](#object-identity-and-lifecycle)
5. [Relation Mapping Strategies](#relation-mapping-strategies)
6. [CRUD Operations](#crud-operations)
7. [Caching Strategy](#caching-strategy)
8. [Session Management](#session-management)
9. [Query Architecture](#query-architecture)
10. [Performance Optimization](#performance-optimization)

---

## Overview

The Allors SqlClient adapter implements a sophisticated object-relational mapping (ORM) system that maps domain objects to SQL Server tables. Unlike traditional ORMs that use a simple table-per-class approach, Allors employs an intelligent mapping strategy that optimizes storage based on relation types and cardinality.

### Key Design Principles
- **Identity-based object management**: Each object has a unique bigint ID
- **Version tracking**: Objects have version numbers for concurrency control
- **Adaptive relation storage**: Relations are stored either inline (in object tables) or in separate relation tables based on cardinality
- **Meta-model driven**: The entire mapping is generated from the meta-model at runtime
- **Stored procedure-based**: All database operations use stored procedures for performance
- **Table-valued parameters**: Batch operations use TVPs for efficiency

---

## Core Architecture

### Major Components

```
┌─────────────────────────────────────────────────────────────┐
│                         Database                             │
│  - Manages connection and schema                            │
│  - Maintains meta-model mapping                             │
│  - Provides cache                                           │
└────────────────────────┬────────────────────────────────────┘
                         │
                         │ creates
                         ↓
┌─────────────────────────────────────────────────────────────┐
│                         Session                              │
│  - Unit of work pattern                                     │
│  - Transaction boundary                                     │
│  - Maintains session state                                  │
└────────────────────────┬────────────────────────────────────┘
                         │
                         │ manages
                         ↓
┌─────────────────────────────────────────────────────────────┐
│                          State                               │
│  - ReferenceByObjectId: Dictionary<long, Reference>        │
│  - ChangeSet tracking                                       │
│  - Association caches                                       │
└────────────────────────┬────────────────────────────────────┘
                         │
                         │ contains
                         ↓
┌─────────────────────────────────────────────────────────────┐
│                        Reference                             │
│  - Represents object identity                               │
│  - Tracks version and existence                            │
│  - Creates Strategy on demand                               │
└────────────────────────┬────────────────────────────────────┘
                         │
                         │ contains
                         ↓
┌─────────────────────────────────────────────────────────────┐
│                        Strategy                              │
│  - Implements IStrategy interface                           │
│  - Provides object behavior                                 │
│  - Manages Roles                                            │
└────────────────────────┬────────────────────────────────────┘
                         │
                         │ manages
                         ↓
┌─────────────────────────────────────────────────────────────┐
│                         Roles                                │
│  - Stores modified roles                                    │
│  - Caches role values                                       │
│  - Tracks changes for flush                                 │
└─────────────────────────────────────────────────────────────┘
```

---

## Database Schema

### Schema Structure

The database schema consists of:
1. **Master object table** (`_o`): Stores all object IDs with their class and version
2. **Class tables**: One per concrete class, stores objects and their directly accessible properties
3. **Relation tables**: For many-to-many and non-exclusive relations

### Master Object Table

```sql
CREATE TABLE allors._o (
    o BIGINT IDENTITY(1,1) PRIMARY KEY,  -- Object ID
    c UNIQUEIDENTIFIER,                   -- Class ID (GUID from meta-model)
    v BIGINT                              -- Version number
)
```

**Purpose**:
- Generates unique object IDs using IDENTITY
- Stores the object's concrete class
- Maintains version for optimistic concurrency
- Enables class-based queries without joins

### Class Tables

Each concrete class in the meta-model gets a table:

```sql
CREATE TABLE allors.person (
    o BIGINT PRIMARY KEY,        -- References _o.o
    c UNIQUEIDENTIFIER,          -- Class ID (redundant for performance)
    -- Unit roles (stored inline)
    firstname NVARCHAR(256),
    lastname NVARCHAR(256),
    birthdate DATETIME2,
    -- One-to-one exclusive composite roles (stored inline)
    address BIGINT,              -- Foreign key to object ID
    -- Many-to-one exclusive composite associations (stored inline)
    employer BIGINT              -- Association back reference
)
```

**Inline Storage Rules**:
A relation is stored inline (as a column in the class table) when:
1. It's a unit role (primitive value)
2. It's a one-to-one composite role with exclusive classes
3. It's a many-to-one composite role with exclusive classes
4. It's a one-to-many composite association with exclusive classes

**Exclusive Classes**: When a relation type has a fixed set of classes that participate in it (not polymorphic across different class hierarchies).

### Relation Tables

For relations that cannot be stored inline:

```sql
-- Many-to-many relation
CREATE TABLE allors.person_skill (
    a BIGINT,                    -- Association (Person object ID)
    r BIGINT,                    -- Role (Skill object ID)
    PRIMARY KEY (a, r)
)

-- One-to-many non-exclusive relation
CREATE TABLE allors.organization_employee (
    a BIGINT,                    -- Association (Organization object ID)
    r BIGINT,                    -- Role (Person object ID)
    PRIMARY KEY (a, r)
)

-- One-to-one non-exclusive relation
CREATE TABLE allors.person_passport (
    a BIGINT PRIMARY KEY,        -- Association (Person object ID)
    r BIGINT                     -- Role (Passport object ID)
)
```

**Relation Table Rules**:
A separate relation table is created when:
1. Many-to-many relationships (always)
2. Relations without exclusive classes (polymorphic relations)

---

## Object Identity and Lifecycle

### Object Identity

Every object in Allors has:
- **ObjectId** (`long`): Unique identifier, generated by SQL Server IDENTITY
- **Class** (`IClass`): The concrete class from the meta-model
- **Version** (`long`): Version number for optimistic concurrency, starts at 0

### Reference Lifecycle

```
┌──────────────┐
│   Unknown    │ ← Object ID known, but not yet loaded
└──────┬───────┘
       │ Instantiate
       ↓
┌──────────────┐
│     New      │ ← Newly created, not yet in database
└──────┬───────┘
       │ Commit
       ↓
┌──────────────┐
│   Existing   │ ← Persisted in database
└──────┬───────┘
       │ Delete
       ↓
┌──────────────┐
│   Deleted    │ ← Marked for deletion
└──────────────┘
```

**Reference Flags**:
```csharp
[Flags]
public enum Flags : byte
{
    MaskIsNew = 1,           // Object created in this session
    MaskExists = 2,          // Object exists in database
    MaskExistsKnown = 4,     // Existence has been verified
}
```

### Object Creation

1. **Single Object**:
```csharp
var person = session.Create<Person>();
```

SQL executed:
```sql
EXEC allors.co_person @c='<class-guid>'
-- Returns: new object ID
```

Stored procedure creates:
- Entry in `_o` table with new IDENTITY ID and version 0
- Entry in `person` table with same ID

2. **Batch Creation**:
```csharp
var persons = session.Create<Person>(100);
```

Uses optimized batch creation procedure that generates multiple IDs efficiently.

---

## Relation Mapping Strategies

### Mapping Decision Matrix

| Association | Role    | Exclusive | Storage Location         | Example                    |
|-------------|---------|-----------|--------------------------|----------------------------|
| One         | One     | Yes       | Column in class table    | Person → Address           |
| One         | One     | No        | Relation table           | Party → ContactMechanism   |
| One         | Many    | Yes       | Column in role table     | Person → EmailAddresses    |
| One         | Many    | No        | Relation table           | Party → Orders             |
| Many        | One     | Yes       | Column in class table    | Person ← Employer          |
| Many        | One     | No        | Relation table           | Orders ← Product           |
| Many        | Many    | N/A       | Relation table (always)  | Person ↔ Skill             |

### Unit Role Storage

All unit (primitive) types are stored as columns in the class table:

```csharp
// Meta-model role type
RoleType: FirstName
  ObjectType: String
  Size: 256

// SQL column
firstname NVARCHAR(256)
```

**Supported Unit Types**:
- `String` → `NVARCHAR(size)` or `NVARCHAR(MAX)`
- `Integer` → `INT`
- `Decimal` → `DECIMAL(precision, scale)`
- `Float` → `FLOAT`
- `Boolean` → `BIT`
- `DateTime` → `DATETIME2`
- `Unique` → `UNIQUEIDENTIFIER`
- `Binary` → `VARBINARY(size)` or `VARBINARY(MAX)`

### Composite Role Storage - Inline

**One-to-One Exclusive**:
```csharp
Person.Address → Address (one-to-one, exclusive)
```

Stored as:
```sql
-- In allors.person table
address BIGINT  -- Contains Address object ID
```

**Many-to-One Exclusive**:
```csharp
Person.Employer → Organization (many-to-one, exclusive)
```

Stored as:
```sql
-- In allors.person table
employer BIGINT  -- Contains Organization object ID
```

**One-to-Many Exclusive** (association stored in role table):
```csharp
Organization.Employees → Person[] (one-to-many, exclusive)
```

Stored as:
```sql
-- In allors.person table
organization BIGINT  -- Back-reference to Organization
```

### Composite Role Storage - Relation Table

**Many-to-Many**:
```csharp
Person.Skills → Skill[] (many-to-many)
Skill.Persons → Person[] (inverse)
```

Stored as:
```sql
CREATE TABLE allors.person_skill (
    a BIGINT,  -- Person ID
    r BIGINT,  -- Skill ID
    PRIMARY KEY (a, r)
)
```

**Non-Exclusive Relations**:
```csharp
Party.Orders → Order[] (one-to-many, non-exclusive)
// Party is interface implemented by Person and Organization
```

Must use relation table because Party is polymorphic:
```sql
CREATE TABLE allors.party_order (
    a BIGINT,  -- Party ID (could be Person or Organization)
    r BIGINT,  -- Order ID
    PRIMARY KEY (a, r)
)
```

---

## CRUD Operations

### Create

**Flow**:
```
session.Create<Person>()
    ↓
Commands.CreateObject(IClass)
    ↓
EXEC co_person @c=<class-guid>
    ↓
INSERT INTO _o (c, v) VALUES (@c, 0)
SELECT @o = SCOPE_IDENTITY()
INSERT INTO person (o, c) VALUES (@o, @c)
RETURN @o
    ↓
Reference created with IsNew=true
    ↓
Strategy created
    ↓
Domain object created
```

### Read (Instantiate)

**Single Object**:
```
session.Instantiate(objectId)
    ↓
Check State.ReferenceByObjectId[objectId]
    ↓ (miss)
Commands.InstantiateObject(objectId)
    ↓
EXEC i @table=[objectId]
    ↓
SELECT o, c, v FROM _o WHERE o IN (@table)
    ↓
Cache.GetObjectType(objectId) or query database
    ↓
Reference created
    ↓
Strategy created
```

**Batch Instantiate**:
```csharp
var persons = session.Instantiate(objectIds);
```

Uses table-valued parameter for efficient batch loading:
```sql
EXEC i @table=[@objectIds]
SELECT o, c, v FROM _o WHERE o IN (SELECT _o FROM @table)
```

### Update

**Unit Roles**:
```
strategy.SetRole(roleType, value)
    ↓
Roles.SetUnitRole(roleType, value)
    ↓
Mark role as modified
    ↓
Add to RequireFlushRoles
    ↓
session.RequireFlush(reference, roles)
    ↓
[On session.Commit()]
    ↓
Flush.SetUnitRole(reference, roleType, value)
    ↓
Batch by class and role type
    ↓
EXEC sc_person_firstname @table=[(personId, 'John')]
    ↓
UPDATE person SET firstname = r._r
FROM person INNER JOIN @table AS r ON o = r._a
```

**Composite Roles (Inline)**:
```
strategy.SetCompositeRole(roleType, roleObject)
    ↓
Roles.SetCompositeRole(roleType, roleStrategy)
    ↓
Handle association updates
    ↓
Mark for flush
    ↓
[On Flush]
    ↓
EXEC sc_person_address @table=[(personId, addressId)]
    ↓
UPDATE person SET address = r._r
FROM person INNER JOIN @table AS r ON o = r._a
```

**Composite Roles (Relation Table)**:
```
strategy.AddCompositeRole(roleType, roleObject)
    ↓
CompositesRole.Add(objectId)
    ↓
Track in added set
    ↓
[On Flush]
    ↓
Flush.AddCompositeRole(reference, roleType, addedSet)
    ↓
EXEC ac_person_skill @table=[(personId, skillId)]
    ↓
INSERT INTO person_skill (a, r)
SELECT _a, _r FROM @table
```

### Delete

**Flow**:
```
strategy.Delete()
    ↓
Remove all composite roles (clean up relations)
    ↓
Remove from all associations (back-references)
    ↓
Commands.DeleteObject(strategy)
    ↓
EXEC do_person @o=<objectId>
    ↓
DELETE FROM _o WHERE o=@o
DELETE FROM person WHERE o=@o
    ↓
Reference.Exists = false
    ↓
ChangeSet.OnDeleted(objectId)
```

**Cascade Behavior**:
- No automatic cascade deletion
- Application must explicitly handle referential integrity
- Associations are cleaned up before deletion

---

## Caching Strategy

### Multi-Level Cache Architecture

```
┌────────────────────────────────────────────────────┐
│              Session State (L1)                    │
│  - ReferenceByObjectId                            │
│  - In-memory for current session                  │
│  - Transaction-scoped                             │
└────────────────────┬───────────────────────────────┘
                     │
                     ↓
┌────────────────────────────────────────────────────┐
│            Database Cache (L2)                     │
│  - CachedObject per object                        │
│  - Shared across sessions                         │
│  - Invalidated on commit/rollback                 │
└────────────────────┬───────────────────────────────┘
                     │
                     ↓
┌────────────────────────────────────────────────────┐
│            SQL Server (L3)                         │
│  - Stored procedures                              │
│  - Query plan cache                               │
│  - Data pages                                     │
└────────────────────────────────────────────────────┘
```

### Reference Cache

**State.ReferenceByObjectId**:
- Dictionary mapping object IDs to Reference instances
- Ensures object identity within session
- Cleared on commit/rollback (except for persisted references)

```csharp
Dictionary<long, Reference> ReferenceByObjectId
```

**Access Pattern**:
```csharp
if (!State.ReferenceByObjectId.TryGetValue(objectId, out var reference))
{
    // Load from database
    reference = Commands.InstantiateObject(objectId);
    State.ReferenceByObjectId[objectId] = reference;
}
return reference.Strategy.GetObject();
```

### Role Cache

**CachedObject**:
- Stores loaded unit and composite roles
- Managed by ICacheFactory (pluggable)
- Default implementation: DefaultCachedObject

```csharp
public interface ICachedObject
{
    bool TryGetValue(IRoleType roleType, out object value);
    void SetValue(IRoleType roleType, object value);
    long Version { get; set; }
}
```

**Cache Population**:
1. Unit roles: Loaded on first access via `GetUnitRoles` procedure
2. Composite roles: Loaded on-demand via `GetRole` procedures
3. Prefetch: Batch loading via prefetch policies

### Cache Invalidation

**On Commit**:
```csharp
Cache.OnCommit(accessed, changed)
    ↓
Invalidate changed objects
    ↓
Update versions for accessed objects
```

**On Rollback**:
```csharp
Cache.OnRollback(accessed)
    ↓
Invalidate all accessed objects
```

**Strategy**:
- Conservative invalidation (invalidate on any change)
- Version-based validation
- No distributed cache (session-per-request pattern assumed)

---

## Session Management

### Session Lifecycle

```
Database.CreateSession()
    ↓
Connection.BeginTransaction(IsolationLevel.Snapshot)
    ↓
State initialized
    ↓
[Work with objects]
    ↓
Session.Commit() OR Session.Rollback()
    ↓
Connection.Commit() OR Connection.Rollback()
    ↓
State cleaned up
```

### Transaction Isolation

**Snapshot Isolation Level**:
- Default isolation level
- Row versioning-based
- Prevents dirty reads, non-repeatable reads, and phantom reads
- Writers don't block readers
- Optimistic concurrency control

**Configuration**:
```sql
ALTER DATABASE SET ALLOW_SNAPSHOT_ISOLATION ON
```

### Unit of Work Pattern

**State Tracking**:
```csharp
public sealed class State
{
    // References
    Dictionary<long, Reference> ReferenceByObjectId;

    // Modified roles (pending flush)
    Dictionary<Reference, Roles> UnflushedRolesByReference;
    Dictionary<Reference, Roles> ModifiedRolesByReference;

    // Association caches
    Dictionary<IAssociationType, Dictionary<Reference, Reference>>
        AssociationByRoleByAssociationType;
    Dictionary<IAssociationType, Dictionary<Reference, long[]>>
        AssociationsByRoleByAssociationType;

    // Change tracking
    ChangeSet ChangeSet;
}
```

### Flush Strategy

**Write-Behind Pattern**:
1. Changes accumulated in memory (`UnflushedRolesByReference`)
2. Flushed before queries or on commit
3. Batched by operation type for efficiency

**Flush Triggers**:
- Session.Commit()
- Session.Flush() (explicit)
- Query execution (implicit - ensures query sees pending changes)
- Extent evaluation

**Batch Flush**:
```csharp
// Batch operations by type
Dictionary<IRoleType, List<UnitRelation>> setUnitRoleRelations;
Dictionary<IRoleType, List<CompositeRelation>> setCompositeRoleRelations;
Dictionary<IRoleType, List<CompositeRelation>> addCompositeRoleRelations;
Dictionary<IRoleType, List<CompositeRelation>> removeCompositeRoleRelations;

// Execute in optimal order
ExecuteSetUnitRoles();
ExecuteSetCompositeRoles();
ExecuteAddCompositeRoles();
ExecuteRemoveCompositeRoles();
```

### Change Tracking

**ChangeSet**:
```csharp
public class ChangeSet : IChangeSet
{
    HashSet<long> Created;          // New objects
    HashSet<long> Deleted;          // Deleted objects
    HashSet<long> Associations;     // Objects with changed associations

    // Detailed change tracking
    Dictionary<long, HashSet<IRoleType>> RoleTypesByAssociation;

    void OnCreated(long objectId);
    void OnDeleted(long objectId);
    void OnChangingUnitRole(long objectId, IRoleType roleType);
    void OnChangingCompositeRole(long objectId, IRoleType roleType,
                                  long? previousRole, long? newRole);
    void OnChangingCompositesRole(long objectId, IRoleType roleType,
                                   IObject role);
}
```

**Usage**:
```csharp
var changeSet = session.Checkpoint();  // Get and reset
foreach (var objectId in changeSet.Created)
{
    // Process created objects
}
```

---

## Query Architecture

### Extent System

**Extent Types**:
1. `ExtentFiltered`: Base filtered extent with predicates
2. `ExtentOperation`: Union, Intersect, Except operations
3. `ExtentRoles`: Extent for composite roles
4. `ExtentAssociations`: Extent for associations

**SQL Generation**:
```
Extent
    ↓
ExtentStatement.Create()
    ↓
Build SQL with joins and predicates
    ↓
Execute query
    ↓
Return object IDs
    ↓
Instantiate objects
```

### Predicate System

**Predicate Types**:
- `Equals`: Role equals value
- `Between`: Role between values
- `Like`: String pattern matching
- `In`: Role in collection
- `Contains`: Collection contains value
- `Exists`: Role is not null
- `InstanceOf`: Object is instance of type
- `And`, `Or`, `Not`: Logical combinations

**SQL Mapping**:
```csharp
// C# predicate
extent.Filter
    .AddEquals(person.FirstName, "John")
    .AddGreaterThan(person.Age, 18);

// Generated SQL
SELECT o FROM allors.person
WHERE firstname = @p0 AND age > @p1
```

### Prefetching

**Prefetch Policy**:
```csharp
var prefetchPolicy = new PrefetchPolicyBuilder()
    .WithRule(m.Person.Address)
    .WithRule(m.Person.Orders,
        new PrefetchPolicyBuilder()
            .WithRule(m.Order.OrderLines))
    .Build();

session.Prefetch(prefetchPolicy, persons);
```

**Execution**:
1. Collect all object IDs to prefetch
2. Batch load roles by type using stored procedures
3. Populate caches
4. Recursive prefetch for nested rules

**Stored Procedure Usage**:
```sql
-- Prefetch unit roles
EXEC pu_person @table=[personIds]
SELECT o, firstname, lastname, birthdate, ...
FROM person WHERE o IN (@table)

-- Prefetch composite role
EXEC pc_person_address @table=[personIds]
SELECT o, address FROM person WHERE o IN (@table)

-- Prefetch composites roles
EXEC gc_organization_employee @table=[orgIds]
SELECT a, r FROM organization_employee WHERE a IN (@table)
```

---

## Performance Optimization

### Table-Valued Parameters

**Definition**:
```sql
CREATE TYPE allors._t_o AS TABLE (_o BIGINT)
CREATE TYPE allors._t_c AS TABLE (_a BIGINT, _r BIGINT)
CREATE TYPE allors._t_s AS TABLE (_a BIGINT, _r NVARCHAR(MAX))
-- ... one per unit type
```

**Usage**:
```csharp
var objectIds = new[] { 1L, 2L, 3L };
var tvp = Database.CreateObjectTable(objectIds);

var command = connection.CreateCommand();
command.CommandType = CommandType.StoredProcedure;
command.CommandText = "allors.i";  // Instantiate

var param = command.CreateParameter();
param.SqlDbType = SqlDbType.Structured;
param.TypeName = "allors._t_o";
param.Value = tvp;  // IEnumerable<SqlDataRecord>

command.Parameters.Add(param);
command.ExecuteReader();
```

**Advantages**:
- Single round-trip for batch operations
- Set-based operations in SQL
- Query plan caching
- Reduced network overhead

### Stored Procedures

**Naming Convention**:
```
Prefix_Class_RelationType

Prefixes:
- i: Instantiate
- co_: Create Object
- cos_: Create Objects
- do_: Delete Object
- l_: Load Object
- gu_: Get Units
- pu_: Prefetch Units
- gc_: Get Composite/Composites
- pc_: Prefetch Composite/Composites
- sc_: Set Composite
- cc_: Clear Composite
- ac_: Add Composite
- rc_: Remove Composite
- ga_: Get Association
- pa_: Prefetch Association
- gv: Get Version
- uv: Update Version
```

**Example**:
```sql
CREATE PROCEDURE allors.gc_person_address
    @table allors._t_o READONLY
AS
BEGIN
    SELECT o, address
    FROM allors.person
    WHERE o IN (SELECT _o FROM @table)
END
```

**Benefits**:
- Query plan compilation and caching
- Reduced SQL injection risk
- Consistent performance
- Centralized SQL logic

### Batching Strategy

**Flush Batching**:
```csharp
private const int BatchSize = 1000;

// Automatically flush when batch size reached
if (relations.Count > BatchSize)
{
    session.Commands.SetUnitRole(relations, exclusiveClass, roleType);
    relations.Clear();
}
```

**Instantiate Batching**:
```csharp
// Instantiate multiple objects in one call
var objects = session.Instantiate(objectIds);

// Uses single stored procedure call with TVP
EXEC allors.i @table=[@objectIds]
```

### Index Strategy

**Automatic Index Creation**:
- Primary key on object ID (clustered)
- Index on class ID for type-based queries
- Indices on relation columns marked with `IsIndexed`

**Index Creation Example**:
```csharp
if (relationType.IsIndexed)
{
    var indexName = $"idx_{class.Name}_{roleType.SingularFullName}";
    CREATE INDEX {indexName} ON {tableName} ({columnName})
}
```

### Command Reuse

**Command Caching**:
```csharp
private Dictionary<IClass, Command> getUnitRolesByClass;
private Dictionary<IRoleType, Command> getCompositeRoleByRoleType;
// ... etc.

// Reuse command, update parameters
if (!getUnitRolesByClass.TryGetValue(@class, out var command))
{
    command = CreateCommand(...);
    getUnitRolesByClass[@class] = command;
}
else
{
    command.Parameters["@o"].Value = objectId;
}
command.ExecuteReader();
```

**Benefits**:
- Avoid command creation overhead
- Parameter rebinding is fast
- Works with SQL Server's query plan cache

### Optimistic Concurrency

**Version-Based Conflict Detection**:
1. Read object and version
2. Modify in memory
3. On commit, increment version
4. If another session modified, version mismatch occurs

**Update Version**:
```sql
CREATE PROCEDURE allors.uv
    @table allors._t_o READONLY
AS
BEGIN
    UPDATE allors._o
    SET v = v + 1
    FROM allors._o
    WHERE o IN (SELECT _o FROM @table)
END
```

**Conflict Resolution**:
- Application-level handling
- Last-write-wins by default
- Can implement custom conflict resolution

---

## Advanced Topics

### Schema Evolution

**Validation on Startup**:
```csharp
var validation = Database.Validate();
if (!validation.IsValid)
{
    // Schema must be recreated
    Database.Init();
}
```

**Validation Checks**:
- All required tables exist
- All required columns exist
- Column types match meta-model
- All stored procedures exist
- All table types exist

**Schema Initialization**:
```csharp
Database.Init()
    ↓
AllowSnapshotIsolation()
    ↓
CreateSchema() if not exists
    ↓
DropProcedures()
    ↓
DropTables()
    ↓
CreateTables()
    ↓
DropTableTypes()
    ↓
CreateTableTypes()
    ↓
CreateProcedures()
    ↓
CreateIndices()
```

### Polymorphic Queries

**Interface Queries**:
```csharp
// Party is interface, Person and Organization are implementations
var extent = session.Extent<Party>();

// Generated SQL uses IN with multiple class IDs
SELECT o FROM allors._o
WHERE c IN (@personClassId, @organizationClassId)
```

**InstanceOf Predicate**:
```csharp
extent.Filter.AddInstanceOf(m.Person);

// SQL
SELECT o FROM allors._o
WHERE c = @personClassId
   OR c IN (@subclassId1, @subclassId2, ...)
```

### Serialization

**XML Save**:
```csharp
Database.Save(xmlWriter)
    ↓
Iterate all objects by class
    ↓
Write object data
    ↓
Write relation data
```

**XML Load**:
```csharp
Database.Load(xmlReader)
    ↓
Parse XML
    ↓
Bulk insert into tables using stored procedures
    ↓
Preserve object IDs from XML
```

### Multi-Tenancy Considerations

**Schema Per Tenant**:
```csharp
var config = new Configuration
{
    ConnectionString = connectionString,
    SchemaName = "tenant_" + tenantId  // e.g., tenant_acme
};
var database = new Database(serviceProvider, config);
```

**Benefits**:
- Complete data isolation
- Independent schema evolution
- Simple backup/restore
- Clear security boundary

---

## Conclusion

The Allors SqlClient adapter implements a sophisticated ORM that:

1. **Optimizes Storage**: Uses intelligent relation mapping based on cardinality
2. **Maximizes Performance**: Leverages stored procedures, TVPs, and batching
3. **Ensures Correctness**: Implements proper transaction isolation and change tracking
4. **Provides Flexibility**: Meta-model driven with pluggable caching
5. **Scales Well**: Efficient batch operations and query optimization

The mapping strategy adapts to the specific characteristics of each relation type, storing data inline when possible for performance, and using relation tables when necessary for flexibility. This hybrid approach provides the best of both worlds: the simplicity and performance of inline storage for common cases, and the flexibility of relation tables for complex polymorphic scenarios.
