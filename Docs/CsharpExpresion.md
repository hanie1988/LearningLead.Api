# 🧠 C# Expression Trees — Dissecting (Core Guide)

> **Purpose**
> Build a *clear mental model* of `Expression<Func<T, bool>>`: what it is, how it is structured, and why frameworks like **EF Core** depend on it.

---

## 🧭 Big Picture (Read First)

Expression Trees are **not executable code**.
They are **structured data that describes code**.

> ✨ **One‑line rule**:
> `Func<T, bool>` = *how to execute*
> `Expression<Func<T, bool>>` = *what to execute*

If this difference is not solid, everything built on LINQ, EF, and query extensions becomes fragile.

---

## 1️⃣ The Starting Point

```csharp
Expression<Func<User, bool>> exp = u => u.Age > 18;
```

### What this is ❌ NOT

* ❌ Not a function
* ❌ Not executable logic
* ❌ Not runtime behavior

### What this IS ✅

* ✔ A **tree structure**
* ✔ A **description of intent**
* ✔ Inspectable by frameworks

---

## 2️⃣ How the Compiler Represents It

Visually, the expression becomes:

```text
LambdaExpression
├── Parameters
│   └── u : User
└── Body
    └── BinaryExpression ( > )
        ├── Left  → MemberExpression (u.Age)
        └── Right → ConstantExpression (18)
```

Nothing runs. Nothing executes.

This is pure metadata.

---

## 3️⃣ Inspecting the Expression in Code

```csharp
var parameter = exp.Parameters[0];
var body = exp.Body;
```

### 🔹 Parameter

```csharp
parameter.Name; // "u"
parameter.Type; // typeof(User)
```

This represents the **lambda variable**.

---

### 🔹 Body

```csharp
body.NodeType; // ExpressionType.GreaterThan
body.Type;     // bool
```

Meaning:

> The lambda returns a boolean produced by a comparison.

---

## 4️⃣ BinaryExpression — Breaking It Down

```csharp
var binary = (BinaryExpression)exp.Body;
```

| Side  | NodeType     | Meaning           |
| ----- | ------------ | ----------------- |
| Left  | MemberAccess | Access a property |
| Right | Constant     | Literal value     |

---

### Left Side → `u.Age`

```csharp
var left = (MemberExpression)binary.Left;
left.Member.Name; // "Age"
```

✔ Accessing property `Age` on parameter `u`

---

### Right Side → `18`

```csharp
var right = (ConstantExpression)binary.Right;
right.Value; // 18
```

✔ Fixed literal value

---

## 5️⃣ Why EF Core Depends on This Structure

From the tree, EF Core can safely read:

* Property → `Age`
* Operator → `>`
* Constant → `18`

And generate SQL:

```sql
WHERE Age > 18
```

⚠ No execution. No guessing. No reflection hacks.

---

## 6️⃣ Why `Func<T, bool>` Fails Here

```csharp
Func<User, bool> f = u => u.Age > 18;
```

What EF sees:

* Compiled IL
* Opaque logic
* No inspectable structure

❌ No SQL translation possible.

---

## 7️⃣ Critical Comparison Table

| Feature        | Func<T,bool> | Expression<Func<T,bool>> |
| -------------- | ------------ | ------------------------ |
| Executable     | ✔            | ❌                        |
| Inspectable    | ❌            | ✔                        |
| EF‑friendly    | ❌            | ✔                        |
| SQL generation | ❌            | ✔                        |
| Runtime logic  | ✔            | ❌                        |

---

## 8️⃣ Why This Matters for IQueryable Extensions

Example:

```csharp
Expression.Not(predicate.Body)
```

You are:

* ❌ Not executing logic
* ❌ Not touching data
* ✔ Only modifying the **description**

That’s why EF stays happy.

---

## 9️⃣ 🚫 Dangerous Anti‑Pattern

```csharp
predicate.Compile()(x); // ❌
```

Why this is bad:

* Forces in‑memory execution
* Breaks translation
* Kills performance

Once `Compile()` is called, **query providers are gone**.

---

## 🔎 Self‑Check (Be Honest)

You should be able to answer:

1. What exactly is stored in `exp.Body`?
2. Why does EF need `MemberExpression`?
3. Why must constants be explicit nodes?
4. Why does `ExpressionType` exist?

If any answer is fuzzy → reread sections 2–6.

---

## ▶ Next Step (Logical Continuation)

Now that you can **read** expression trees, the next skill is to **build them manually**:

* `Expression.Parameter`
* `Expression.Property`
* `Expression.Constant`
* `Expression.AndAlso`

This unlocks:

* Dynamic filters
* Specification pattern
* Advanced `IQueryable` extensions

---

> **Coach note**:
> Do not rush. Mastery here removes fear from LINQ, EF, and interview questions.

---

**End of guide.**
