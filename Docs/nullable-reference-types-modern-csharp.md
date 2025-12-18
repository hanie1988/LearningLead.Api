# Nullable Reference Types (NRT) in Modern C#
*Designing APIs that tell the truth about null*

---

## 1️⃣ Why Nullable Reference Types exist

Before NRT, this code **lied**:

```csharp
public User GetUser()
{
    return null;
}
```

The signature promises:
> “I always return a User”

Reality:
> “Sometimes I return null”

This mismatch caused:
- `NullReferenceException`
- defensive code everywhere
- unclear APIs

**NRT makes this lie visible at compile time.**

---

## 2️⃣ What NRT actually is (important)

Nullable Reference Types:
- ❌ do NOT change runtime behavior
- ❌ do NOT prevent nulls at runtime
- ✅ ARE compiler flow analysis + annotations

They create:
> **a contract between you and the compiler**

---

## 3️⃣ Enabling NRT

Usually enabled at project level:

```xml
<Nullable>enable</Nullable>
```

Or per file:

```csharp
#nullable enable
```

From this point:
- reference types are **non-nullable by default**
- the compiler assumes you mean it

---

## 4️⃣ Core rule (memorize)

```csharp
string name;   // non-nullable
string? name;  // nullable
```

Meaning:

| Type | Meaning |
|---|---|
| `string` | Must never be null |
| `string?` | May be null |

This is **API design**, not syntax sugar.

---

## 5️⃣ Method return types (stop lying)

### ❌ Bad
```csharp
public User GetUser()
{
    return null;
}
```

### ✅ Honest
```csharp
public User? GetUser()
{
    return null;
}
```

Or better patterns:

```csharp
bool TryGetUser(out User user)
```

```csharp
User GetUserOrThrow()
```

**Rule:**
- Nullable return → uncertainty
- `TryXxx` → expected absence
- `OrThrow` → guaranteed presence or failure

---

## 6️⃣ Method parameters (caller responsibility)

```csharp
void Print(string text)     // caller must pass non-null
void Print(string? text)    // caller may pass null
```

This forces correct usage at call sites.

---

## 7️⃣ Properties (make intent explicit)

### ❌ Ambiguous
```csharp
public string Email { get; set; }
```

### ✅ Clear
```csharp
public string Email { get; set; } = "";
```

or

```csharp
public string? Email { get; set; }
```

Pick one. Don’t let null sneak in accidentally.

---

## 8️⃣ Constructor guarantees

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

The compiler now knows:
- `Email` is never null after construction

This enforces **correct object creation**.

---

## 9️⃣ Flow analysis (compiler intelligence)

```csharp
void SendEmail(string? email)
{
    if (email == null)
        return;

    Console.WriteLine(email.Length); // safe
}
```

The compiler understands that after the check, `email` is non-null.

---

## 🔟 The null-forgiving operator `!` (danger)

```csharp
string? name = GetName();
Console.WriteLine(name!.Length);
```

Meaning:
> “Compiler, trust me — it’s not null.”

Use **only when**:
- you validated earlier
- a framework guarantees it
- at deserialization / ORM boundaries

❌ Do NOT use to silence warnings.

---

## 1️⃣1️⃣ NRT + Collections (very common trap)

```csharp
List<string> emails;        // list not null, items not null
List<string?> emails;       // list not null, items may be null
List<string>? emails;       // list itself may be null
```

**Rule:**
> `?` applies to exactly what it is attached to

---

## 1️⃣2️⃣ NRT + Async

```csharp
Task<User?> GetUserAsync()
```

Means:
- task itself is non-null
- result may be null

Avoid:
```csharp
Task<User>?
```

A nullable task is almost always a design smell.

---

## 1️⃣3️⃣ NRT + Fields (hidden danger)

```csharp
public class Service
{
    private User _user;
}
```

Compiler warning is **correct**.

Valid fixes:

```csharp
private User? _user;
```

```csharp
private User _user = default!;
```

```csharp
public Service(User user)
{
    _user = user;
}
```

Fields are where null bugs hide.

---

## 1️⃣4️⃣ NRT + EF Core (reality)

EF may bypass constructors.

```csharp
public class User
{
    public string Email { get; set; } = null!;
}
```

Acceptable when:
- DB enforces NOT NULL
- EF guarantees assignment

Limit `null!` to ORM boundaries only.

---

## 1️⃣5️⃣ NRT + Records

```csharp
public record User(string Email);
```

Promises `Email` is never null.

If null is allowed:

```csharp
public record User(string? Email);
```

Records make null intent obvious.

---

## 1️⃣6️⃣ NRT + Generics

```csharp
public T Get<T>()
```

Is `T` nullable? Unknown.

Fix:

```csharp
public T Get<T>() where T : notnull
```

Now null is forbidden for `T`.

---

## 1️⃣7️⃣ Common mistakes to avoid

❌ Making everything nullable  
❌ Using `!` everywhere  
❌ Ignoring warnings  
❌ Treating warnings as noise  

Warnings are **design feedback**.

---

## 1️⃣8️⃣ Golden rules (print this)

1. Non-nullable = promise  
2. Nullable = possibility  
3. Collections need double thinking  
4. Async tasks should not be nullable  
5. `!` is a last resort  
6. Warnings improve design  

---

## 1️⃣9️⃣ Migration mindset

When enabling NRT in old code:
- don’t fix everything at once
- fix public APIs first
- fix constructors and invariants
- improve gradually

NRT is about **clarity**, not perfection.

---

## 🔚 Summary

Nullable Reference Types:
- expose hidden design flaws
- improve API clarity
- reduce runtime null bugs
- force honest contracts

Mastering NRT is a **permanent upgrade** to your C# skill.

---

*End of guide.*
