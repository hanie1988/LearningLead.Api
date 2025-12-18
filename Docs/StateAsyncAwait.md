# Async / Await & State in C#
*Why async does not create bugs — it reveals them*

---

## 1️⃣ First correction (critical)

Async **does NOT**:
- make code faster
- make code parallel
- make code complex by itself

Async only:
> **pauses execution and resumes it later**

If bugs appear after adding async, it means:
> **the state design was already unsafe**

Async just exposes it.

---

## 2️⃣ The real danger: shared mutable state

### ❌ Classic async bug

```csharp
public class Counter
{
    public int Value { get; set; }
}

var counter = new Counter();

async Task IncrementAsync()
{
    var temp = counter.Value;
    await Task.Delay(100);
    counter.Value = temp + 1;
}

await Task.WhenAll(
    IncrementAsync(),
    IncrementAsync()
);

Console.WriteLine(counter.Value); // ❌ maybe 1, not 2
```

### Why this happens
- Both tasks read `Value = 0`
- Both pause at `await`
- Both resume later
- Both write `1`

This is **not an async bug**.  
This is a **state design bug**.

---

## 3️⃣ Core rule (memorize)

> **`await` splits your method into multiple time slices**

Anything that:
- is mutable
- is shared
- is accessed before and after `await`

is **dangerous**.

---

## 4️⃣ Async + mutable domain state (common mistake)

### ❌ Unsafe pattern

```csharp
public async Task ProcessOrder(Order order)
{
    if (order.Status != OrderStatus.Paid)
        throw new InvalidOperationException();

    await SendEmailAsync();

    order.Ship(); // ❌ state may no longer be valid
}
```

Between `await` and resume:
- another request may run
- another handler may modify state
- another thread may update the DB

---

## 5️⃣ Safe pattern: re-check state after await

```csharp
public async Task ProcessOrder(Guid orderId)
{
    var order = await db.Orders.FindAsync(orderId);

    if (order.Status != OrderStatus.Paid)
        return;

    await SendEmailAsync();

    if (order.Status != OrderStatus.Paid)
        return;

    order.Ship();
    await db.SaveChangesAsync();
}
```

Async forces you to **respect time**.

---

## 6️⃣ Async + EF Core (critical interaction)

### ❌ Wrong mental model

```csharp
var order = await db.Orders.FindAsync(id);
await SomeAsyncWork();
order.Ship();
await db.SaveChangesAsync();
```

Problems:
- `DbContext` is **not thread-safe**
- entity state may be stale
- another request may have updated the row

### Rule
> **EF entities are reliable only inside short, controlled async flows**

---

## 7️⃣ Correct async design principles

### Principle 1: Keep async methods short  
Long async flows = stale state

### Principle 2: Do not hold mutable state across `await`  
Especially:
- EF entities
- shared services
- cached objects

### Principle 3: Prefer value snapshots

```csharp
var status = order.Status;
await SomethingAsync();
if (status != OrderStatus.Paid) return;
```

---

## 8️⃣ Why records work well with async

```csharp
public record EmailRequest(string To, string Subject);

public async Task SendAsync(EmailRequest request)
{
    await smtp.SendAsync(request);
}
```

Why this is safe:
- immutable
- no shared state
- no mutation after await

This is the **correct use** of records in async code.

---

## 9️⃣ Async does NOT replace locking

Async ≠ thread safety.

If state is shared, you still need:
- locking
- or redesign to avoid sharing

```csharp
lock (_sync)
{
    counter.Value++;
}
```

Design > locks.  
Immutability beats locking.

---

## 🔟 Mental model (lock this in)

| Concept | Reality |
|---|---|
| async | pause & resume |
| await | time boundary |
| bugs | caused by shared mutable state |
| immutability | reduces async bugs |
| EF + async | must be short & controlled |

---

## 1️⃣1️⃣ What NOT to do

- Don’t sprinkle async everywhere
- Don’t store EF entities in fields
- Don’t keep mutable state across awaits
- Don’t assume async = safe

---

## 1️⃣2️⃣ Self-check

You should be able to answer:
1) Why does `await` expose bugs?
2) What makes state unsafe across `await`?
3) Why are records good for async messages?
4) Why is EF entity state fragile in long async flows?

---

## 🔚 Summary

- Async reveals design flaws
- Shared mutable state is the enemy
- Immutability reduces risk
- EF + async requires discipline

