# Immutability & State Transitions in C#
*A complete, practical guide with clear rules and examples*

---

## 1️⃣ What “state” means (core concept)

**State = data that can change over time**

Examples:
- Order status
- User email
- Appointment date
- Payment result

Most bugs happen because:
- State changes too easily
- State changes from the wrong place
- State changes at the wrong time (async / EF)

So the real design question is:

> **Who is allowed to change state, and how?**

---

## 2️⃣ Mutable vs Immutable (no philosophy, just facts)

### ❌ Mutable object (dangerous by default)

```csharp
public class Order
{
    public OrderStatus Status { get; set; }
}
```

Anyone can do:
```csharp
order.Status = OrderStatus.Shipped;
```

**Problems:**
- No validation
- No control
- No business rules
- Breaks easily with async and EF

---

### ✅ Immutable object (controlled)

```csharp
public record Order(OrderStatus Status);
```

This is **immutable**:
```csharp
order.Status = OrderStatus.Shipped; // ❌ compile error
```

State can change only by creating a **new instance**.

---

## 3️⃣ Property accessors: `set`, `private set`, `init`, `get-only`

These are NOT the same. This is a frequent source of confusion.

---

## 4️⃣ `set` — fully mutable (least safe)

```csharp
public class User
{
    public string Email { get; set; } = "";
}
```

✔ Can change:
- during construction
- after construction
- from anywhere

```csharp
user.Email = "a@test.com";
user.Email = "b@test.com";
```

**Use only when:**
- The object is a pure data container (DTO)
- No business rules apply

---

## 5️⃣ `private set` — controlled mutation

```csharp
public class User
{
    public string Email { get; private set; }

    public User(string email)
    {
        Email = email;
    }

    public void ChangeEmail(string email)
    {
        Email = email;
    }
}
```

✔ Can change:
- only inside the class
- through methods

❌ Cannot change:
```csharp
user.Email = "x@test.com"; // ❌
```

**This is the minimum acceptable design for entities.**

---

## 6️⃣ `init` — immutable after construction

```csharp
public class User
{
    public string Email { get; init; } = "";
}
```

Allowed:
```csharp
var user = new User { Email = "a@test.com" };
```

Not allowed:
```csharp
user.Email = "b@test.com"; // ❌
```

**Rules:**
- Value decided at creation
- Safe for DTOs and records
- Prevents runtime mutation

---

## 7️⃣ `get` only (no `set`, no `init`) — fully immutable

```csharp
public class User
{
    public string Email { get; }

    public User(string email)
    {
        Email = email;
    }
}
```

✔ Cannot change ever after construction  
✔ Strongest immutability for classes

---

## 8️⃣ Records are NOT automatically safe (important)

Many developers think:
> “I use record, so I’m safe”

Not true.

### ❌ Dangerous record

```csharp
public record Order(List<string> Items);
```

Why?
- `List<T>` is mutable
- `with` is **shallow copy**

```csharp
var o1 = new Order(new List<string>());
var o2 = o1 with { };

o2.Items.Add("X");

Console.WriteLine(o1.Items.Count); // 💥 changed
```

---

## 9️⃣ Correct pattern: explicit state transitions

### ❌ Bad (free mutation)

```csharp
order.Status = OrderStatus.Shipped;
```

### ✅ Good (explicit transition)

```csharp
public record Order(OrderStatus Status)
{
    public Order Ship()
    {
        if (Status != OrderStatus.Paid)
            throw new InvalidOperationException("Order not paid");

        return this with { Status = OrderStatus.Shipped };
    }
}
```

**Benefits:**
- Business rules are explicit
- Impossible to skip validation
- Easy to reason about
- Testable

---

## 🔟 Value Object vs Entity (light but precise)

### Value Object
- Equality by value
- Immutable
- No identity

```csharp
public readonly record struct Money(decimal Amount, string Currency);
```

Examples:
- Money
- Email
- Temperature
- DateRange

---

### Entity
- Identity matters
- State changes over time
- Equality by Id

```csharp
public class User
{
    public Guid Id { get; }
    public string Email { get; private set; }

    public User(Guid id, string email)
    {
        Id = id;
        Email = email;
    }

    public void ChangeEmail(string email)
    {
        Email = email;
    }
}
```

---

## 1️⃣1️⃣ Shallow copy vs Deep copy

### Shallow copy (default for records)

```csharp
public record Bag(List<string> Items);

var a = new Bag(new List<string> { "A" });
var b = a with { };

b.Items.Add("B");

Console.WriteLine(string.Join(",", a.Items)); // A,B
```

---

### Deep copy (explicit)

```csharp
public record Bag(List<string> Items)
{
    public Bag DeepClone()
        => new Bag(new List<string>(Items));
}
```

---

## 1️⃣2️⃣ Practical rules (memorize)

- Default to **immutable**
- Prefer `init` over `set`
- Use `private set` for entities
- Never expose mutable collections directly
- State changes must go through methods
- `with` ≠ deep copy

---

## 1️⃣3️⃣ Why this matters later (preview)

This directly affects:
- EF Core change tracking
- Async race conditions
- Equality bugs
- Production-only failures

---

## 1️⃣4️⃣ Final mental checklist

Before writing a property, ask:
1) Should this ever change?
2) Who is allowed to change it?
3) Do I need validation?
4) Should this be a value object?

If you can answer these, you’re writing **senior-level C#**.

---

# EF Core Change Tracking vs Immutability
*A practical, no‑nonsense guide for real C# backend systems*

---

## 1️⃣ What EF Core actually tracks (critical truth)

EF Core does **not** track “types” or “records”.  
It tracks **entity instances and their property snapshots**.

Simplified lifecycle:
1. EF loads an entity instance
2. Takes a snapshot of property values
3. You mutate the same instance
4. EF compares snapshot vs current values
5. Differences → `UPDATE`

**Key requirement:**
> EF expects the **same object instance** to change over time.

This is where immutability conflicts with EF.

---

## 2️⃣ Why pure immutability breaks EF tracking

### ❌ This looks clean but breaks EF

```csharp
public record Order(Guid Id, OrderStatus Status);
```

Usage:

```csharp
var order = await db.Orders.FindAsync(id);

order = order with { Status = OrderStatus.Shipped };

await db.SaveChangesAsync();
```

❌ Nothing is saved.

### Why?
- EF is tracking the **original instance**
- `with` creates a **new instance**
- EF does not auto‑switch tracking

This is **expected behavior**, not a bug.

---

## 3️⃣ Core rule (memorize this)

> **EF Core requires mutable entities.**  
> **Records are best for DTOs and value objects.**

If you violate this rule, EF becomes unpredictable.

---

## 4️⃣ Correct EF‑friendly entity design

### ✅ Mutable entity with controlled mutation

```csharp
public class Order
{
    public Guid Id { get; private set; }
    public OrderStatus Status { get; private set; }

    private Order() { } // EF

    public Order(Guid id)
    {
        Id = id;
        Status = OrderStatus.Created;
    }

    public void Ship()
    {
        if (Status != OrderStatus.Paid)
            throw new InvalidOperationException("Order not paid");

        Status = OrderStatus.Shipped;
    }
}
```

Usage:

```csharp
var order = await db.Orders.FindAsync(id);
order.Ship();
await db.SaveChangesAsync(); // ✅ works
```

Why this works:
- Same instance
- Controlled mutation
- Snapshot comparison succeeds

---

## 5️⃣ Where immutability still belongs with EF

### ✅ Value Objects (perfect match)

```csharp
public readonly record struct Money(decimal Amount, string Currency);
```

Used inside an entity:

```csharp
public class Order
{
    public Money Total { get; private set; }

    public void ChangeTotal(Money newTotal)
    {
        Total = newTotal;
    }
}
```

EF behavior:
- Entire value object replaced
- Change detected correctly
- No internal mutation

This is the **ideal balance**.

---

## 6️⃣ Why `init` is dangerous for EF entities

```csharp
public class User
{
    public string Email { get; init; }
}
```

Problems:
- EF sets values only at materialization
- Property cannot change later
- Updates silently fail or force full replacement

### Rule:
| Usage | Correct accessor |
|----|----|
| EF Entity | `private set` |
| DTO / Record | `init` |
| Value Object | get‑only |

---

## 7️⃣ Common EF + record mistakes (interview traps)

### ❌ Mistake 1: Using `record` as EF entity
- Tracking breaks
- Updates ignored
- Debugging becomes painful

### ❌ Mistake 2: Using `with` on tracked entities
- EF keeps old instance
- New instance is ignored

### ❌ Mistake 3: Public setters everywhere
- EF works
- Business rules don’t
- Bugs appear later in production

---

## 8️⃣ Correct mental model (pin this)

| Concept | Use |
|----|----|
| `class` + `private set` | EF entities |
| `record` | DTOs / read models |
| `record struct` | Value objects |
| `with` | Non‑EF state transitions |
| Methods | State changes |

If you follow this table, EF stops being “weird”.

---

## 9️⃣ Why this matters in production

Misunderstanding this causes:
- `SaveChangesAsync()` doing nothing
- Random update failures
- State reverting unexpectedly
- “It worked yesterday” bugs

Correct design prevents these entirely.

---

## 🔟 Final rules (non‑negotiable)

- EF entities must be mutable
- Mutation must be **controlled**
- Records are not entities
- `with` never updates tracked entities
- Value objects should be immutable

---

## 🔚 Summary (one screen)

- EF tracks **instances**
- Immutability replaces instances
- Replacement ≠ mutation
- Entities mutate
- Value objects don’t

Once this is clear, EF becomes predictable.

---

*End of guide.*

