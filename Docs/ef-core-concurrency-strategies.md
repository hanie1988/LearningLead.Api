# Concurrency Control in EF Core: What Is Better and When?

This document explains **Atomic SQL updates**, and compares the main concurrency strategies in EF Core:

- Pessimistic locking
- Optimistic locking
- Transactions
- Atomic SQL updates

All explained in **real-world Web API terms**.

---

## What Is an Atomic SQL Update?

An **atomic SQL update** is a single SQL statement that:
- Checks a condition
- Modifies data
- Succeeds or fails as ONE indivisible operation

Example (inventory reservation):

```sql
UPDATE Products
SET Stock = Stock - 1
WHERE Id = @id AND Stock > 0;
```

### Why this is atomic
- The database guarantees this runs as one unit
- No other transaction can sneak in between the check and the update
- Either 1 row is updated, or 0 rows

### How you use it
- Execute the command
- Check affected rows
- If rows == 0 → out of stock

No locks in application memory.
No race conditions.

---

## 1. Atomic SQL Update (BEST)

### EF Core example
```csharp
var rows = await _dbContext.Database.ExecuteSqlRawAsync(
    "UPDATE Products SET Stock = Stock - 1 WHERE Id = {0} AND Stock > 0",
    productId);

if (rows == 0)
    return BadRequest("Out of stock");
```

### Pros
- Fastest
- Fully safe
- Works across multiple servers
- Minimal DB locking
- Industry standard

### Cons
- Slightly less expressive
- Business logic partly in SQL

### Verdict
⭐ **BEST choice for counters, inventory, quotas, balances**

---

## 2. Optimistic Locking (Version / Concurrency Token)

### How it works
- Read row + version
- Update row only if version matches
- If not → conflict

### EF Core example
```csharp
public class Product
{
    public int Id { get; set; }
    public int Stock { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; }
}
```

```csharp
try
{
    product.Stock -= 1;
    await _dbContext.SaveChangesAsync();
}
catch (DbUpdateConcurrencyException)
{
    // conflict → retry or fail
}
```

### Pros
- Clean domain model
- No blocking
- Good for user-edit scenarios

### Cons
- Requires retry logic
- Not ideal for hot counters

### Verdict
✅ Best for **user edits**, profiles, forms

---

## 3. Pessimistic Locking

### How it works
- Lock the row when reading
- Other transactions must wait

### EF Core example
```csharp
using var tx = await _dbContext.Database
    .BeginTransactionAsync(IsolationLevel.Serializable);

var product = await _dbContext.Products
    .Where(p => p.Id == productId)
    .FirstAsync();

if (product.Stock <= 0)
    return BadRequest();

product.Stock -= 1;
await _dbContext.SaveChangesAsync();

await tx.CommitAsync();
```

### Pros
- Simple mental model
- Strong consistency

### Cons
- Blocks other requests
- Risk of deadlocks
- Poor scalability

### Verdict
⚠️ Use only when absolutely necessary

---

## 4. Transactions (by themselves)

### Important clarification
A transaction alone does **NOT** prevent race conditions.

```csharp
using var tx = await _dbContext.Database.BeginTransactionAsync();
var stock = product.Stock;
product.Stock = stock - 1;
await _dbContext.SaveChangesAsync();
await tx.CommitAsync();
```

❌ Still unsafe without isolation or atomic logic.

### Verdict
⚠️ Transactions are a **tool**, not a solution

---

## Final Comparison Table

| Approach              | What it is                         | Correctness      | Performance | Scalability | When to use                          |
|----------------------|-----------------------------------|------------------|-------------|-------------|--------------------------------------|
| Pessimistic locking  | `SELECT ... FOR UPDATE`            | ✅ Very safe     | ❌ Slower    | ⚠️ Limited  | Rare, short critical sections        |
| Optimistic locking   | `RowVersion` / `ConcurrencyToken` | ⚠️ Retry needed | ✅ Fast      | ✅ Good     | Low–medium contention                |
| Transaction (EF)     | `BeginTransaction` + logic         | ⚠️ Depends      | ⚠️ Medium   | ⚠️ Medium  | Multi-step business logic            |
| Atomic SQL update    | Single conditional `UPDATE`        | ⭐ BEST          | ⭐ BEST     | ⭐ BEST     | Counters, inventory, quotas          |

---

## Coach Rule (Remember This)

> If multiple users hit the same row frequently,  
> **Atomic SQL beats all EF-level locking strategies.**

This is how real high-scale systems work.

---

### The correct mental model (this is important)

**❌ Wrong thinking**

“Which one should I always use?”

**✅ Correct thinking**

“What is my business invariant and contention level?”
