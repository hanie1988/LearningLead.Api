## 📌 class
- **Reference type**
- Stored on the **heap**
- Passed by **reference**
- Mutable by default
- Supports **inheritance**
- Best for: **Entities** in Domain layer, Services, Controllers
- Used in my project: `Event`, `User`, `Booking`, `AppDbContext`

---

## 📌 record class
- **Reference type**, but with **value-based equality**
- Good for **immutable objects**
- Best for: **Create or Update DTOs**, simple request models
- Supports **with-expressions** for easy cloning
- Used in my project: `UserCreateDto`, `UserUpdateDto`, `EventCreateDto`

---

## 📌 struct
- **Value type**  
- Allocated on the **stack** (fast, no GC)
- Mutable by default
- Copied on assignment (not shared)
- Should be **small** (≤ 16 bytes)
- Avoid mutating structs (can cause bugs)
- Rarely used in web APIs (usually only numeric structs, geometry, etc.)

---

## 📌 readonly struct
- Value type that **cannot change after creation**
- IMutable
- Prevents hidden copies due to mutations
- Safer and faster for small “data-only” types
- Used for: **Coordinates**, **Money**, **Tiny Value Objects**

---

## 📌 record struct
- **Value-type** record  
- Combines benefits of struct + record  
  - Struct = fast, no GC  
  - Mutable
  - Record = value-based equality  
- Good for: **Lightweight DTOs** that you want to be immutable
- Used in my project: `UserResponseDto`, `EventResponseDto`

---

## 📌 readonly record struct
- The “best” version of a struct for immutable responses
- Value type + immutable + value equality
- Zero memory garbage + safe + predictable
- Best for:  
  - Response DTOs  
  - Query results  
  - Small cross-layer data objects  
- Used in my project: `EventResponseDto`, `UserResponseDto`

---

# 📌 When to Use Each (Summary Table)

| Type                     | Ref/Value | Mutable | Equality | Best Use Case | Notes |
|--------------------------|-----------|---------|----------|----------------|-------|
| **class**                | Reference | Yes     | Reference-based | Entities, Services | Default choice for domain models |
| **record**               | Reference | No      | Value-based | Create/Update DTOs | Simple, clean, immutable |
| **struct**               | Value     | Yes*    | Value-based | Very small data | Avoid mutating |
| **readonly struct**      | Value     | No      | Value-based | Tiny value objects | Faster, no hidden copies |
| **record struct**        | Value     | No      | Value-based | Response DTOs | Fast + equality |
| **readonly record struct** | Value  | No      | Value-based | Best response DTO | Most recommended for APIs |

---

# 📌 Mapping to Your Project

**Create DTO → record**  
Because you send them **into** the system and don’t need value-type optimization.

**Update DTO → record**  
Same reasoning.

**Response DTO → readonly record struct**  
Because responses are:
- Immutable  
- Small  
- Returned a lot  
- Should not cause GC allocation pressure

**Entity → class**  
Because entities:
- Need navigation properties  
- Are tracked by EF Core  
- Must be reference types  

---

# 🔥 Which one should you use?

✔ For APIs, commands, queries, DTOs, responses

→ Use record (reference type)

✔ For small mathematical objects

(e.g., coordinates, RGB colors)
→ Use record struct

✔ For EF Core entities

→ Never use record
Use class because EF needs mutable reference objects.

## 📌 DTO Selection Summary (C# 12 Best Practice)

| DTO Type          | Recommended Type            | Reason |
|-------------------|-----------------------------|--------|
| **Create DTO**    | `sealed record`             | Reference type → works best with model binding & validation. Allows normalization (trim, lowercase). Sealed for safety and performance. |
| **Update DTO**    | `sealed record`             | Same reasons as Create DTO. Update operations often need partial binding + validation. |
| **Response DTO**  | `readonly record struct`    | Immutable, lightweight, high-performance value type. Ideal for returning pure data without mutation. Reduces GC pressure. |

---

```CSharp
var user1 = new User(1, "Sara");
var user2 = user1 with { Name = "Hanieh" };
```

User2 is the new instance with different reference.
```Csharp
ReferenceEquals(user1, user2) // false
```

CLR will make a new object.
Hidden Copy Constructor
non-destructive mutation

```Csharp
public record User(int Id, Profile Profile);
```

```Csharp
public record User(int Id, Profile Profile);
public class Profile
{
    public string City { get; set; }
}

var profile = new Profile { City = "Montreal" };

var user1 = new User(1, profile);
var user2 = user1 with { Id = 2 };

ReferenceEquals(user1.Profile, user2.Profile) // true

Profile will be copied by address.

user2.Profile.City = "Toronto";
user1.Profile.City == "Toronto" // true 😬
```

---

# C# Value vs Reference Types and Equality
*A practical guide with code for:* **class**, **struct**, **record (class)**, **record struct**, **readonly record struct**  
*(C# 10+; concepts apply broadly.)*

---

## 1) Quick mental model (don’t mix these up)

### Storage + identity
- **class** → *reference type* (variable holds a **reference**). Identity matters unless you override equality.
- **struct** → *value type* (variable holds the **data itself**). Copies happen on assignment/passing.
- **record** → *reference type* (still a class) **but** default equality is *value-based* (by members).
- **record struct** → *value type* (struct) **and** default equality is *value-based* (by members).
- **readonly record struct** → *value type*, value-based equality, and **readonly** semantics (safer immutability).

### Default equality behavior (most important)
| Type | `Equals` default | `==` default | Notes |
|---|---|---|---|
| `class` | reference equality | reference equality | unless you override / overload |
| `struct` | value equality (field-by-field via `ValueType.Equals`) | usually **not available** | unless you overload `==` |
| `record` (class) | value equality (by members) | generated `==` / `!=` | best default for “data objects” |
| `record struct` | value equality (by members) | generated `==` / `!=` | value type + good equality |
| `readonly record struct` | same as record struct | generated `==` / `!=` | plus readonly behavior |

---

## 2) `class` equality (reference by default)

### 2.1 Default behavior: reference equality
```csharp
public class Person
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

var a = new Person { Id = 1, Name = "Sara" };
var b = new Person { Id = 1, Name = "Sara" };

Console.WriteLine(a.Equals(b)); // False (different references)
Console.WriteLine(a == b);      // False (reference equality)
```

### 2.2 Value-based equality for a class (you must implement it)
You must:
- implement `IEquatable<T>`
- override `Equals(object?)` and `GetHashCode()`
- overload `==` and `!=` (optional but recommended if you define equality)

```csharp
public sealed class Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));
    }

    public bool Equals(Money? other)
        => other is not null
           && Amount == other.Amount
           && Currency == other.Currency;

    public override bool Equals(object? obj) => Equals(obj as Money);

    public override int GetHashCode() => HashCode.Combine(Amount, Currency);

    public static bool operator ==(Money? left, Money? right)
        => left is null ? right is null : left.Equals(right);

    public static bool operator !=(Money? left, Money? right) => !(left == right);
}
```

**When you should do this:** when the type is a *domain value object* and equality must be by value (e.g., Money, EmailAddress).

---

## 3) `struct` equality (value-based by default, but `==` is not)

### 3.1 Default behavior: `Equals` works, `==` may not compile
```csharp
public struct Size
{
    public int Width { get; set; }
    public int Height { get; set; }
}

var s1 = new Size { Width = 10, Height = 20 };
var s2 = new Size { Width = 10, Height = 20 };

Console.WriteLine(s1.Equals(s2)); // True (value equality)
Console.WriteLine(s1 == s2);      // ❌ Compile error: operator '==' cannot be applied
```

### 3.2 Add `==` / `!=` for a struct (operator overloading)
```csharp
public readonly struct Size : IEquatable<Size>
{
    public int Width { get; }
    public int Height { get; }

    public Size(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public bool Equals(Size other) => Width == other.Width && Height == other.Height;

    public override bool Equals(object? obj) => obj is Size other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Width, Height);

    public static bool operator ==(Size left, Size right) => left.Equals(right);
    public static bool operator !=(Size left, Size right) => !(left == right);
}
```

**When operator overloading is “needed” for structs:**  
If you expect consumers to naturally use `==` for logical equality, you should implement it (and make it match `Equals`).

---

## 4) `record` (reference type) equality + `==` are generated for you

### 4.1 Record (class) with positional syntax
```csharp
public record User(int Id, string Name);

var u1 = new User(1, "Sara");
var u2 = new User(1, "Sara");

Console.WriteLine(u1.Equals(u2)); // True (value-based)
Console.WriteLine(u1 == u2);      // True (generated)
```

### 4.2 Record (class) with init-only properties
```csharp
public record Profile
{
    public string Email { get; init; } = "";
    public string City  { get; init; } = "";
}

public record User(int Id, Profile Profile);

var a = new User(1, new Profile { Email = "a@x.com", City = "Toronto" });
var b = new User(1, new Profile { Email = "a@x.com", City = "Toronto" });

Console.WriteLine(a == b); // True (compares Id + Profile via equality rules)
```

**Important:** record equality is **member-based**, not reference-based.  
But if a record contains a **class member** that uses reference equality (default), that member can “break” value semantics.

Example:
```csharp
public class Settings
{
    public int Mode { get; set; }
}

public record AppState(int UserId, Settings Settings);

var s1 = new Settings { Mode = 1 };
var s2 = new Settings { Mode = 1 };

var a = new AppState(10, s1);
var b = new AppState(10, s2);

Console.WriteLine(a == b); // False, because Settings is a class with reference equality by default
```

**Fix options:**
- Make `Settings` a `record` (or implement value equality on it)
- Or keep it as a class but override equality (like `Money` example)

---

## 5) `record struct` and `readonly record struct`

### 5.1 `record struct` (value type + generated `==`)
```csharp
public record struct Point(int X, int Y);

var p1 = new Point(1, 2);
var p2 = new Point(1, 2);

Console.WriteLine(p1.Equals(p2)); // True
Console.WriteLine(p1 == p2);      // True (generated)
```

### 5.2 `readonly record struct` (recommended when it should be immutable)
```csharp
public readonly record struct Temperature(decimal Celsius);

var t1 = new Temperature(25m);
var t2 = new Temperature(25m);

Console.WriteLine(t1 == t2); // True
// t1.Celsius = 30m;         // ❌ not allowed (readonly)
```

**Why `readonly record struct` is strong:**  
- Avoids accidental mutation  
- Works well as dictionary keys  
- Predictable equality semantics + good performance

---

## 6) When you DO and DO NOT need operator overloading (`==`, `!=`)

### You typically **DO need** it when:
- You have a **struct** (non-record struct) and want `==` to represent logical equality.
- You create a **value object** and want ergonomic comparisons.

### You typically **do NOT need** it when:
- You use **record** (class) → compiler generates `==`/`!=` consistently.
- You use **record struct / readonly record struct** → compiler generates `==`/`!=` consistently.

### Absolute rule (no exceptions)
If you overload `==`, it **must** match `Equals` semantics.  
If they disagree, you’ll create subtle bugs in dictionaries/sets and comparisons.

---

## 7) Shallow copy vs Deep copy (don’t guess)

### 7.1 Shallow copy: copies the container, not the referenced objects
For **class** instances: assignment copies the reference (no copy of object).
```csharp
public class Bag
{
    public List<string> Items { get; set; } = new();
}

var bag1 = new Bag { Items = new List<string> { "A" } };
var bag2 = bag1; // same reference

bag2.Items.Add("B");

Console.WriteLine(string.Join(",", bag1.Items)); // A,B  (same list)
Console.WriteLine(ReferenceEquals(bag1, bag2));  // True
```

For **records (class)**: `with` expression makes a *shallow copy* of the record object.
```csharp
public record BagState(List<string> Items);

var a = new BagState(new List<string> { "A" });
var b = a with { }; // new record instance, but same Items list reference

b.Items.Add("B");

Console.WriteLine(string.Join(",", a.Items)); // A,B  (shared list)
Console.WriteLine(ReferenceEquals(a.Items, b.Items)); // True
Console.WriteLine(ReferenceEquals(a, b)); // False (different record instances)
```

### 7.2 Deep copy: copies the object AND nested referenced objects
You must do it explicitly. Example using a `Clone` method:
```csharp
public record BagState(List<string> Items)
{
    public BagState DeepClone()
        => new BagState(new List<string>(Items));
}

var a = new BagState(new List<string> { "A" });
var b = a.DeepClone();

b.Items.Add("B");

Console.WriteLine(string.Join(",", a.Items)); // A
Console.WriteLine(string.Join(",", b.Items)); // A,B
Console.WriteLine(ReferenceEquals(a.Items, b.Items)); // False
```

**Coach warning:**  
If you use records thinking `with` is “deep copy”, don’t go this way. It’s not. It’s shallow.

---

## 8) Records + class members: equality & copying pitfalls (common interview trap)

### 8.1 Equality pitfall
If a record contains a class member that does NOT implement value equality, record equality becomes partly reference-based.

**Bad (unexpected):**
```csharp
public class Address { public string City { get; set; } = ""; } // default ref equality
public record Person(string Name, Address Address);

var p1 = new Person("Sara", new Address { City = "Toronto" });
var p2 = new Person("Sara", new Address { City = "Toronto" });

Console.WriteLine(p1 == p2); // False (Address uses reference equality)
```

**Good (expected):**
```csharp
public record Address(string City); // record => value equality
public record Person(string Name, Address Address);

var p1 = new Person("Sara", new Address("Toronto"));
var p2 = new Person("Sara", new Address("Toronto"));

Console.WriteLine(p1 == p2); // True
```

### 8.2 Copy pitfall (`with` is shallow)
Even if equality is correct, copying a record that holds reference-type members still shares those references.

---

## 9) Practical guidelines (what to choose)

### Use **record (class)** when:
- It’s a DTO / message / “data shape”
- You want value equality + `with` for non-destructive updates
- Reference semantics are okay (heap allocation), and immutability is desired

### Use **readonly record struct** when:
- It’s a small value object (Money, Temperature, Point)
- You want immutability + high performance, and predictable equality

### Use **struct** (non-record) when:
- You need a low-level value type but want full control
- You are okay implementing equality + operators yourself (or you don’t need `==`)

### Use **class** when:
- Identity matters (entities: User entity with Id, tracked identity)
- Mutability and lifecycle behavior are core to the design

---

## 10) Operator overloading checklist (copy/paste mental checklist)

If you overload `==` / `!=`, verify:
1. `==` and `!=` are consistent with `Equals`
2. `GetHashCode` is consistent with `Equals`
3. `IEquatable<T>` implemented for performance (especially structs)
4. Null handling done correctly for reference types
5. The type is effectively immutable (recommended) if used as dictionary key

---

## 11) Mini summary (one screen)

- **class**: reference equality unless you implement value equality
- **struct**: value equality exists, but `==` is not automatic
- **record**: value equality + generated operators (`==`, `!=`)
- **record struct**: value equality + generated operators (`==`, `!=`)
- **readonly record struct**: best “value object” default (immutability + equality)

---

## 12) Quick self-test (to catch mistakes)
1) If a record has a `List<T>` property and you use `with`, do you get deep copy or shallow copy?  
2) For a normal struct, why does `s1 == s2` usually fail to compile?  
3) If you overload `==` but don’t override `GetHashCode`, what breaks?

*End of guide.*
