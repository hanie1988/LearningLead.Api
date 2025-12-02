### 📍 LEVEL 0 — What EF Core Really Is

**EF Core is an object-to-relational mapper.**
Meaning:
	•	Your C# objects ↔ Your database rows
	•	Your properties ↔ columns
	•	Your LINQ ↔ SQL
	•	Your DbContext ↔ database session

**You must see EF as a translator + memory tracker + unit-of-work engine.**
Foundation concepts:
	1.	DbContext = connection + change tracking + unit-of-work
	2.	DbSet = table
	3.	ModelBuilder = schema
	4.	LINQ to Entities = query language
	5.	SaveChanges() = batch SQL generator

---

### 📍 LEVEL 0.5 — What Happens When You Run a Query

Example:
```Csharp
var rooms = await db.Rooms.ToListAsync();
```
Steps:
	1.	EF inspects your LINQ expression
	2.	Builds expression tree
	3.	Translates it to SQL
	4.	Sends SQL to DB
	5.	DB returns rows
	6.	EF materializes objects (C# Room objects)
	7.	EF stores them in ChangeTracker

---

### 📍 LEVEL 1 — DbContext Anatomy

DbContext contains:

**1️⃣ Database Connection + Commands**

It opens/closes connections and sends SQL.

2️⃣ **ChangeTracker**

Keeps original + current values.

3️⃣ **Model (Schema)**

Generated from OnModelCreating.

4️⃣ **Unit of Work**

Tracks all changes → sends them as a batch in SaveChanges().

5️⃣ **DbSet**

Represents a table.
```Csharp
public DbSet<Room> Rooms { get; set; }
```

---

### 📍 LEVEL 1.5 — Entities & Mapping

An entity with exactly these characteristics:
	•	Has an identity (Id)
	•	Has scalar properties (value types)
	•	Has navigations (relations)

Example:
```Csharp
public class Room
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public int Capacity { get; set; }
    public decimal PricePerNight { get; set; }

    public Hotel Hotel { get; set; }  // navigation
}
```
Entity rules:

✔ Has key
✔ Is tracked
✔ Is mapped
✔ Has fixed lifetime per DbContext

---

### 📍 LEVEL 2 — Change Tracking (The Heart of EF)

**EF stores:**
	•	Current values
	•	Original values
	•	EntityState
	•	Metadata
	•	Navigation references

**States:**
	•	Added → INSERT
	•	Modified → UPDATE
	•	Deleted → DELETE
	•	Unchanged → nothing
	•	Detached → EF ignores it

**SaveChanges() Steps**
	1.	Detect changes
	2.	Generate INSERT / UPDATE / DELETE commands
	3.	Order commands (respect constraints)
	4.	Execute
	5.	Fix-up relationships
	6.	Refresh snapshots

---

### 📍 LEVEL 3 — Relationships (Deep, Not Shallow)

You MUST know these:

1️⃣ **One-to-Many**

Room → Reservations
Hotel → Rooms

2️⃣ **Many-to-Many**

Room ↔ Amenity
Usually with Auto-table or join entity.

3️⃣ **One-to-One**

User → UserProfile

4️⃣ **Owned Entities**

Value objects (Address, Money, etc.)

5️⃣ **Foreign Key Rules**
	•	Cascade
	•	SetNull
	•	Restrict

---

### 📍 LEVEL 4 — Query Pipeline (IQueryable Mastery)

**EF query pipeline:**
	1.	Build expression tree
	2.	Rewrite expression
	3.	Translate to SQL
	4.	Execute
	5.	Materialize
	6.	Optionally track

✔ AsQueryable()
✔ AsNoTracking()
✔ Select vs Include
✔ Projection best practices

---

### 📍 LEVEL 5 — SaveChanges + Transactions + Concurrency

**What SaveChanges does:**
	1.	DetectChanges
	2.	Build SQL
	3.	Group by type
	4.	Wrap in implicit transaction
	5.	Execute
	6.	Update states

**Transactions:**
	•	BeginTransaction()
	•	Commit()
	•	Rollback()

**Concurrency:**
	•	RowVersion
	•	Optimistic locking
	•	Pessimistic locking
	•	Handling DbUpdateConcurrencyException

---

### 📍 LEVEL 6 — Performance Optimization

**Core Performance Rules:**

✔ Use AsNoTracking() for read endpoints
✔ Use projection (never return full entities)
✔ Use Include only when needed
✔ Always index foreign keys
✔ Paginate properly
✔ Combine queries
✔ Avoid N+1 queries
✔ Use compiled queries for hotspots

---

### 📍 LEVEL 7 — Migrations + Schema Evolution

✔ reviews SQL for each migration
✔ avoids destructive changes
✔ uses zero-downtime patterns
✔ seeds data in migrations
✔ understands column rename safety

---

### 📍 LEVEL 8 — Real Testing Methods

In-memory DB is NOT real.

✔ SQLite InMemory
✔ Testcontainers PostgreSQL
✔ Repository isolation
✔ Service mocking
✔ Transaction rollback per test

---

### 📍 LESSON 1 — The Real Internals of DbContext
Think of DbContext not as a **“database class,” but as a database session + transaction scope + entity tracker.**

**📍 1) Change Tracker Engine (the brain of EF)**

Whenever an entity is loaded from the database: EF does three things:

**1) It creates an internal entry called EntityEntry**

This entry stores:
✔ Original values
✔ Current values
✔ State
✔ Foreign keys
✔ Navigation references

```
EntityEntry(Room #1):
    OriginalValues = { PricePerNight = 100 }
    CurrentValues  = { PricePerNight = 100 }
    State          = Unchanged
```
**2) It subscribes to property changes**
So when you do:
```
room.PricePerNight = 150;
```
EF marks state as: Modified

3) It stores snapshots

Snapshots let EF detect differences without querying DB again.

**📍 2) Model Engine (metadata builder)**

EF builds a complete schema metadata model when the app starts.
It contains:
✔ All entity types
✔ Keys
✔ Foreign keys
✔ Indexes
✔ Table/column mappings
✔ Configuration from Fluent API
✔ Annotations
✔ Conventions

This model is kept in memory and reused. Think of this as EF’s blueprint.

**📍 3) LINQ → Expression Tree → SQL Engine**

This is important. When you write:
```
var q = db.Rooms.Where(r => r.Capacity > 2);
```
It does not run immediately. Internally:
	1.	C# compiler converts your lambda into an expression tree
	2.	EF inspects that expression tree
	3.	EF rewrites it
	4.	EF translates it to SQL
	5.	SQL is sent to database

So EF is basically: a SQL generator based on expression trees

This is why not all C# methods can be translated (e.g., Console.WriteLine() cannot be converted to SQL).

**📍 4) Materialization Engine (SQL → objects)**

When SQL returns rows:
Id | PricePerNight | Capacity
1     150              2
EF creates actual C# objects:
var room = new Room
{
    Id = 1,
    PricePerNight = 150,
    Capacity = 2
};
Then EF:
✔ Fixes navigation properties
✔ Sets state to Unchanged
✔ Adds them to Change Tracker

This process is called materialization.

**📍 5) Unit of Work Engine (batching all changes)**

When you call: await db.SaveChangesAsync();

EF:
	1.	Runs DetectChanges()
	2.	Groups all entities by state (Added/Modified/Deleted)
	3.	Generates SQL commands
	4.	Wraps everything in a transaction
	5.	Sends SQL batch to DB
	6.	Updates snapshots
	7.	Clears temporary state

This is why you don’t manually open transactions for simple operations — EF does it for you.

**📍 6) Database Connection & Transaction Engine**

DbContext manages:
	•	opening/closing connections
	•	transaction boundaries
	•	command retries
	•	isolation levels

Example of EF behavior:
	•	If no transaction exists → it creates one automatically
	•	If you’re already inside a transaction → EF respects it

---

### Lesson 2 — Entities, Keys, and the Mapping System

📍 1) What EF Core considers an Entity (not everything is an entity)

EF uses conventions to decide:

An entity is ANY class that:

✔ has a public get/set property
✔ has a primary key
✔ is included in your DbContext or reachable through navigation properties

📍 2) How EF Selects the Primary Key (automatic rules)

EF chooses the key using exact rules:

✔ Rule 1: Property named Id
✔ Rule 2: Property named (ClassName)Id
```Csharp
public int RoomId { get; set; }
```
✔ If no key is found → EF refuses to map the type
You will get:

No key defined for entity type ‘Room’.

📍 3) How EF Determines Table Names (Conventions)

By default:
	•	Entity class name → table name
	•	Property name → column name
```Csharp
public class Room
{
    public int Id { get; set; }
}
```
Maps to:
```
Table: Rooms
Column: Id
```
Pluralization is default but can be disabled.

📍 4) Navigation Properties (The Real Logic)

EF discovers relationships by pairing:

✔ Navigation → Foreign key

✔ Foreign key → Navigation

```Csharp
public class Room
{
    public int HotelId { get; set; }
    public Hotel Hotel { get; set; }
}
```
EF detects:
	•	HotelId is FK
	•	Hotel is navigation to principal entity
	•	So EF builds a one-to-many relationship

It’s automatic — no Fluent API needed unless customizing.

📍 5) Shadow Properties (real senior concept)

EF can store FK values even if your class doesn’t have them.

Example:
```Csharp
public class Reservation
{
    public int Id { get; set; }
    public Room Room { get; set; }
}
```
There is no RoomId.
EF creates a shadow property:
```
RoomId (shadow)
```
This is used internally for relationships.

You can access it:
```Csharp
var roomId = db.Entry(reservation)
               .Property("RoomId")
               .CurrentValue;
```
Why this matters?

👉 In complex domain models (DDD), you may not want FK properties in your entities.
EF handles this for you.

📍 6) Fluent Configuration (ModelBuilder)

When conventions aren’t enough, you override them:
```Csharp
builder.Entity<Room>(entity =>
{
    entity.ToTable("Rooms");
    entity.HasKey(r => r.Id);

    entity.Property(r => r.PricePerNight)
          .HasColumnType("decimal(18,2)");

    entity.HasOne(r => r.Hotel)
          .WithMany(h => h.Rooms)
          .HasForeignKey(r => r.HotelId);
});
```
Fluent API lets you control:

✔ Column type
✔ Length
✔ Required fields
✔ Primary/Foreign keys
✔ Indexes
✔ Delete behavior
✔ Table names
✔ Relationships

📍 7) Owned Types — Value Objects (Most devs don’t know this)

If an object has no identity and belongs to an entity, you map it as owned.

Example:
```Csharp
public class Address
{
    public string Country { get; set; }
    public string City { get; set; }
}
```
Used inside:
```Csharp
public class Hotel
{
    public int Id { get; set; }
    public Address Address { get; set; }
}
```
Configuration:
```Csharp
builder.Entity<Hotel>()
       .OwnsOne(h => h.Address);
```
EF stores them in the same table.

Great for DDD and clean models.

📍 8) Table-per-Hierarchy (TPH) — EF’s Primary Inheritance Strategy

Example:
```Csharp
public abstract class Payment { ... }
public class CreditCardPayment : Payment { ... }
public class PaypalPayment : Payment { ... }
```

EF maps all of them to one table, with a Discriminator column.

You don’t need to do anything — it’s default.

---

### ⭐ LESSON 3 — Change Tracking
📍 1) What is Change Tracking?

EF keeps two sets of values:

🟦 OriginalValues
	•	The values when the entity was loaded or attached.

🟩 CurrentValues
	•	The values in your C# object right now.

EF compares Original vs Current to detect changes.

This comparison is the basis of:
	•	updates
	•	concurrency detection
	•	relationship fix-up
	•	snapshot rebuilding

📍 2) How EF Tracks an Entity (Step-by-step)

When you run:
```Csharp
var room = await db.Rooms.FindAsync(1);
```
Internally EF does:

1️⃣ Create an EntityEntry for this instance
```
EntityEntry (Room)
```
2️⃣ Store a snapshot
```
OriginalValues: { PricePerNight = 100, Capacity = 2 }
```
3️⃣ Store state
```
State = Unchanged
```
4️⃣ Link the entity to a tracking graph

EF now “owns” this entity until DbContext is disposed.

📍 3) When You Modify a Property
```Csharp
room.PricePerNight = 150;
```
EF checks:
```
Original: 100
Current: 150
```
This triggers: State = Modified

📍 4) SaveChanges — Real Internal Pipeline

This is the important part.
```
await db.SaveChangesAsync();
```
Behind the scenes, EF runs:

Step 1: DetectChanges()

Find entities whose current values differ from snapshot.

Step 2: Generate SQL
Step 3: EF wraps all SQL in a single transaction

Even if you didn’t create one.

Step 4: Execute commands

Step 5: Refresh snapshots

EF now sets OriginalValues = CurrentValues for all updated entities.

📍 5) Tracking vs No-Tracking (Interview Level)

🟦 Tracking Query (default)
```Csharp
var rooms = await db.Rooms.ToListAsync();
```
Every entity is tracked.

Cost: More RAM, ChangeTracker overhead
Use: When updating entities

⸻

🟩 No-Tracking Query
```Csharp
var rooms = await db.Rooms.AsNoTracking().ToListAsync();
```
EF does not:
	•	store snapshots
	•	track changes
	•	detect modifications

Cost: none
Use: Reads only (lists, searches, pagination)

This is critical for API read endpoints.

📍 6) Auto DetectChanges Behavior

EF calls DetectChanges() automatically:
	•	before SaveChanges
	•	before finding related entities
	•	sometimes before LINQ queries

This has a cost.

If you load 10,000 rows → ChangeTracker becomes heavy.

You can disable:
```Csharp
db.ChangeTracker.AutoDetectChangesEnabled = false;
```
Then manually call:
```Csharp
db.ChangeTracker.DetectChanges();
```
Used in bulk operations.

📍 7) Attaching Entities (Power Technique)

Instead of loading entity from DB, you can attach it:
```Csharp
var room = new Room { Id = 5 };
db.Attach(room);
```
State becomes: Unchanged
Then mark only one property:
```Csharp
db.Entry(room).Property(r => r.PricePerNight).IsModified = true;
await db.SaveChangesAsync();
```
⚡ This prevents EF from selecting the row first.
Huge performance win.

📍 8) Graph Tracking (Navigations)

If you load an entity with navigation:
```Csharp
var hotel = await db.Hotels
    .Include(h => h.Rooms)
    .FirstAsync();
```
EF assigns:
	•	Hotel tracked
	•	Each Room tracked
	•	Hotel.Rooms collection populated
	•	Room.Hotel reference populated

Tracking happens for the whole graph.

This is why Include is expensive for large graphs.

📍 9) Detaching Entities

If you want EF to forget an entity:
```Csharp
db.Entry(room).State = EntityState.Detached;
```
This is useful when:
	•	you want to re-load fresh data
	•	you want to avoid accidental updates
	•	you’re building background jobs
	•	you’re using a long-lived DbContext (bad practice)

📍 10) Tracking Performance Rules (Senior Level)

✔ Use AsNoTracking() for read-only endpoints
✔ Never track large lists (e.g., 50k rows)
✔ Attach entities manually for updates in high-load systems
✔ Disable AutoDetectChanges for bulk operations
✔ Avoid Include-heavy graphs
✔ Keep DbContext short-lived

---

### ⭐ LESSON 4 — Query Pipeline: How EF Transforms LINQ Into SQL
```
Your LINQ → Expression Tree → Query Compiler → SQL Translator → SQL → DB Result → Materialization → Tracking
```
📍 1) LINQ in EF is NOT LINQ-to-Objects

This is the first trap people fall into.

❌ LINQ-to-Objects

Runs in memory, iterates through C# collections.

✔ LINQ-to-Entities (EF Core)

Translates expressions into SQL for the database to execute.

This means:
	•	You cannot use ANY method you want
	•	Only methods that can be turned into SQL will work
	•	EF will throw exceptions for unsupported operations

Example that cannot translate:
```
.Where(r => CustomMethod(r.PricePerNight))
```
Example that always translates:
```
.Where(r => r.PricePerNight > 100)
```
📍 2) Expression Trees — the real secret

When you write:
```Csharp
var q = db.Rooms.Where(r => r.Capacity > 2);
```
The lambda r => r.Capacity > 2 is not executed.
Instead, it becomes an expression tree that EF inspects.

EF sees:
	•	Table = Rooms
	•	Filter = Capacity > 2

Then EF builds SQL:
```
SELECT * FROM Rooms WHERE Capacity > 2;
```









For each entity:
| Item              | What EF remembers                                                   |
|------------------|---------------------------------------------------------------------|
| Original values   | What the values were when the entity was first loaded               |
| Current values    | The values currently in memory                                      |
| State             | Added / Modified / Deleted / Unchanged                              |
| Navigation links  | Relations to other entities                                         |

Everything is stored inside:
```
DbContext.ChangeTracker.Entries()
```

🔄 What happens when you modify a tracked entity?

Example:
```Csharp
var room = await db.Rooms.FindAsync(1);  // tracked
room.PricePerNight = 199;                // change property
await db.SaveChangesAsync();
```
Behind the scenes:
	1.	Room is loaded + tracked
	2.	EF compares original and current values
	3.	It sees PricePerNight changed → sets State = Modified
	4.	When you call SaveChanges():
	•	EF generates SQL like:

```Csharp
UPDATE Rooms SET PricePerNight = 199 WHERE Id = 1
```
---