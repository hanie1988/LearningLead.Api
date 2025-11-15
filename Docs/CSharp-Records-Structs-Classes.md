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