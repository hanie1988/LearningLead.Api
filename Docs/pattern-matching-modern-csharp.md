# Pattern Matching in Modern C#
*Replacing defensive `if` logic with clear intent*

---

## What pattern matching really is

Pattern matching is **not**:
- syntactic sugar
- shorter `if`
- functional-only style

Pattern matching **is**:
> **Declaring intent about shape and value at the same time**

That’s why modern C# code is more readable, safer, and closer to domain language.

---

## 1️⃣ `is` pattern — the foundation

### ❌ Old style (avoid)

```csharp
if (user != null && user.Age >= 18)
{
    // ...
}
```

Problems:
- null check + logic mixed
- easy to forget one condition
- noisy and defensive

---

### ✅ Modern pattern matching

```csharp
if (user is { Age: >= 18 })
{
    // ...
}
```

What this guarantees:
- `user` is not null
- `user` has an `Age` property
- `Age` is ≥ 18

All expressed in **one declarative check**.

---

## 2️⃣ Type patterns (safe casting)

```csharp
if (obj is User user)
{
    Console.WriteLine(user.Email);
}
```

What happens:
- runtime type check
- safe cast
- scoped variable

❌ Old equivalent:
```csharp
var user = obj as User;
if (user != null)
{
    Console.WriteLine(user.Email);
}
```

**Rule:**  
If you see `as` + null check → replace with pattern matching.

---

## 3️⃣ Property patterns (core power)

```csharp
if (user is { IsActive: true, Age: >= 18 })
{
    // allowed
}
```

This checks:
- not null
- multiple properties at once
- no partial validation

You are describing a **valid state**, not execution steps.

---

## 4️⃣ Relational patterns

```csharp
if (score is >= 80 and < 90)
{
    // B grade
}
```

Better than:
```csharp
if (score >= 80 && score < 90)
```

Why:
- reads like a rule
- boundaries are explicit
- easier to review

---

## 5️⃣ Logical patterns (`and`, `or`, `not`)

```csharp
if (user is not null and { IsBlocked: false })
{
    // safe
}
```

```csharp
if (status is OrderStatus.Paid or OrderStatus.Shipped)
{
    // allowed
}
```

This is **domain language**, not mechanics.

---

## 6️⃣ Switch expressions (replace `if / else` chains)

### ❌ Old style

```csharp
string result;

if (status == OrderStatus.New)
    result = "New";
else if (status == OrderStatus.Paid)
    result = "Paid";
else
    result = "Unknown";
```

---

### ✅ Modern switch expression

```csharp
var result = status switch
{
    OrderStatus.New => "New",
    OrderStatus.Paid => "Paid",
    _ => "Unknown"
};
```

Benefits:
- expression-based
- exhaustive
- impossible to forget a return

---

## 7️⃣ Switch with property patterns

```csharp
var label = user switch
{
    { IsActive: true, Age: >= 18 } => "Active Adult",
    { IsActive: true } => "Active Minor",
    null => "Missing",
    _ => "Inactive"
};
```

This is **state-based logic**, not procedural branching.

---

## 8️⃣ Why pattern matching matters

Pattern matching:
- reduces null bugs
- enforces valid states
- improves readability
- works naturally with records and immutability

This is why modern C# favors it.

---

## ❌ Common mistakes

- Over-nesting patterns
- Writing unreadable one-liners
- Using pattern matching everywhere blindly

If readability drops, don’t use it.

---

## 🧠 Practice task (important)

Today’s task:
1. Take one old method with null checks and `if/else`
2. Rewrite it using:
   - `is` patterns
   - a switch expression

No new topic today.

---

## Next lesson

**List patterns + when NOT to use pattern matching**

---

# Pattern Matching — Lesson 2
## List Patterns + When **NOT** to Use Pattern Matching

---

## 1️⃣ What list patterns are (and what they are not)

**List patterns** let you match:
- length
- position
- shape of sequences

They are **not** for:
- complex algorithms
- heavy business logic
- unclear intent

They shine when **structure matters more than process**.

> Available in **C# 11+**

---

## 2️⃣ Basic list patterns (shape-based)

```csharp
int[] numbers = { 1, 2, 3 };

if (numbers is [1, 2, 3])
{
    // exact match
}
```

This checks:
- not null
- length == 3
- exact values in order

Declarative and clear. No loops.

---

## 3️⃣ Ignoring elements with `_`

```csharp
if (numbers is [1, _, 3])
{
    // middle value doesn't matter
}
```

Use when:
- position matters
- value doesn’t

---

## 4️⃣ Slice patterns (`..`)

```csharp
if (numbers is [1, .., 5])
{
    // starts with 1, ends with 5
}
```

Meaning:
- at least two elements
- first = 1
- last = 5
- anything in between is ignored

---

## 5️⃣ Head–tail pattern

```csharp
if (numbers is [var first, .. var rest])
{
    Console.WriteLine(first);
    Console.WriteLine(rest.Length);
}
```

Use when:
- you need the first element
- and the remaining slice

⚠️ For arrays, `rest` creates a copy — avoid in hot paths.

---

## 6️⃣ List patterns with relational conditions

```csharp
if (numbers is [> 0, > 0, > 0])
{
    // all three numbers are positive
}
```

Readable when short.  
Unreadable when long.

---

## 7️⃣ Switch expressions with list patterns

```csharp
string Describe(int[] values) => values switch
{
    [] => "Empty",
    [1] => "Single one",
    [1, 2] => "One then two",
    [1, ..] => "Starts with one",
    _ => "Other"
};
```

This is **classification**, not computation.

Perfect use case.

---

## 8️⃣ When list patterns are a BAD idea ❌

### ❌ Procedural logic

Bad:
```csharp
if (numbers is [var a, var b, var c] && a + b + c > 10)
{
}
```

Better:
```csharp
if (numbers.Length == 3 && numbers.Sum() > 10)
{
}
```

Pattern matching should **describe**, not **compute**.

---

### ❌ Long or fragile patterns

Bad:
```csharp
if (data is [1, _, _, _, _, _, 7])
```

If someone has to **count commas**, stop.

---

### ❌ Replacing loops blindly

Bad:
```csharp
if (items is [_, _, _, _, _])
```

Better:
```csharp
if (items.Length == 5)
```

---

## 9️⃣ The golden rule (memorize)

> **If a junior developer can’t understand it in 5 seconds, don’t use it.**

Pattern matching is for **clarity**, not cleverness.

---

## 🔟 Decision table

| Situation | Use pattern matching |
|---|---|
| Null + property check | ✅ Yes |
| Type + cast | ✅ Yes |
| State classification | ✅ Yes |
| Exact list shape | ✅ Yes |
| Computation / aggregation | ❌ No |
| Complex business rules | ❌ No |
| Performance-critical loops | ❌ No |

---

## 1️⃣1️⃣ Common good use cases

- HTTP route segment parsing
- Command parsing
- Validation rules
- DTO shape checks
- Result classification

---

## 1️⃣2️⃣ Practice task

1. Find a method that:
   - checks array length
   - checks first or last element
2. Rewrite it using:
   - list patterns
   - a switch expression

Then ask:
> “Is this clearer than before?”

If not, revert it. That’s maturity.

---

## What’s next

Next Modern C# topic:

## **Nullable Reference Types (deep, practical)**

---

*End of guide.*

