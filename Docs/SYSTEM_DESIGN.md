# 🏛 LearningLead Ticketing API — Architecture Overview  
**Clean Architecture • .NET 9 • C# 12 • EF Core • Azure SQL**

This document provides a complete explanation of the architecture used in the **LearningLead Ticketing System**, including folder structure, project dependencies, design principles, and communication flow between layers.

The goals of this architecture are:

- high testability  
- clear separation of concerns  
- maintainable and scalable codebase  
- clean business logic, independent from frameworks  
- senior-level structure suitable for interviews and real companies  

---

# 🧱 1. High-Level Architecture (Clean Architecture)

The solution follows the classic Clean Architecture circle:
[API Layer] → depends on → [Application Layer]
[Infrastructure Layer] → depends on → [Application + Core]
[Application Layer] → depends on → [Core Layer]
[Core Layer] → depends on no one

**Rule:**  
➡ **Inner layers never depend on outer layers.**  
➡ **Outer layers depend on inner layers only.**

---

# 📁 2. Folder Structure

•	Core (domain)
•	Application (use cases)
•	Infrastructure (EF, SQL)
•	API (presentation)

/Src
├─ Api                      → Presentation layer (Controllers, Endpoints)
│  ├─ Controllers
│  ├─ appsettings.json
│  └─ Program.cs
│
├─ Application              → Use Case layer (business flows)
│  ├─ Events
│  │   ├─ EventDtos.cs
│  │   ├─ EventService.cs
│  ├─ Users
│      ├─ UserDtos.cs
│      ├─ UserService.cs
│  ├─ Validators (future)
│  └─ Application.csproj
│
├─ Core                    → Domain layer (pure business objects)
│  ├─ Entities
│  │   ├─ Event.cs
│  │   ├─ Ticket.cs
│  │   └─ User.cs
│  └─ Core.csproj
│
└─ Infrastructure         → Data access, repositories, EF Core setup
├─ Data
│   ├─ AppDbContext.cs
├─ Repositories
│   ├─ EventRepository.cs
│   └─ UserRepository.cs
├─ Migrations
└─ Infrastructure.csproj

---

# 🧩 3. Responsibilities of Each Layer

## **3.1 Core Layer (Domain)**
✔ Only **business entities**  
✔ No EF Core  
✔ No framework dependencies  
✔ Pure C# 12 objects

# Project Architecture Explanation

### 3.1 Core Layer (Domain)

Example:
```csharp
public sealed class Event { … }
```

👉 The domain should NEVER depend on Application, Infrastructure, or API.

---

### 3.2 Application Layer (Use Cases)

Contains business logic, not data access.

Includes:
- DTOs  
- Services (EventService, UserService)  
- Commands & Queries (future CQRS)  
- Validation  
- Business rules  

Example:
```csharp
public sealed class EventService(EventRepository repo)
```

👉 Application depends ONLY on Core.

---

### 3.3 Infrastructure Layer

Implements all external concerns:
- EF Core  
- SQL Server  
- Migrations  
- Repositories  

Example:
```csharp
public sealed class EventRepository
{
    …
}
```

👉 The API depends on Application + Infrastructure + Core.  
👉 Infrastructure depends on Application + Core.

---

### Project Reference Rules

| Project        | Can Reference                          | Cannot Reference      |
|---------------|------------------------------------------|------------------------|
| **Core**      | nobody                                   | everyone               |
| **Application** | Core                                      | Infrastructure, API    |
| **Infrastructure** | Application, Core                       | API                    |
| **API**       | Application, Infrastructure, Core         | nobody                 |

---

### 3.4 API Layer (Presentation)

Contains:
- Controllers  
- API routes  
- DI setup  
- Swagger setup  

Example:
```csharp
[ApiController]
[Route("api/events")]
public sealed class EventController { … }
```

👉 The API depends on Application + Infrastructure + Core.

---

## 🧠 Why Each Project Is a Class Library?

### **Core**
Holds entities ⇒ shared across layers.

### **Application**
Contains pure logic, reusable in:
- API  
- background jobs  
- unit tests  

### **Infrastructure**
Class Library because:
- Should NOT expose controllers  
- Only provides implementation (DbContext, Repositories)  

### **API**
Only project that hosts the actual web server.

          +-------------+
          |    API      |  → HTTP only
          +-------------+
                 ↓
          +-------------+
          | Application |  → Use cases only
          +-------------+
                 ↓
          +-------------+
          |    Core     |  → Business logic only
          +-------------+
                 ↓
          +-------------+
          |Infrastructure| → SQL, Redis, Email, Logging
          +-------------+


