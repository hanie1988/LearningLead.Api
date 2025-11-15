# 🔥 My Main Learning Goals for This Project

- Improve **API design** (Routing, HTTP verbs, clean structure)  
- Learn **Clean Architecture** + **CQRS**  
- Practice **EF Core** (relationships, migrations, performance tuning)  
- Learn **Redis caching layer** and performance optimization  
- Improve testing with **xUnit** and **Mock frameworks**  
- Learn **Azure fundamentals** (SQL Server, Storage, Compute, Cost management)  
- Master **async/await and threading** for scalability  
- Understand **retry logic (Polly)** + **structured logging (Serilog)**  

---

# 🧠 Architecture (Folder Structure)

```bash
/src
├─ Core (Domain Layer)
│  ├─ Entities
│  ├─ Enums
│  └─ Interfaces
│
├─ Application (Use Case Layer)
│  ├─ Commands
│  ├─ Queries
│  ├─ DTOs
│  ├─ Validators
│  └─ Services
│
├─ Infrastructure (Data & Integration Layer)
│  ├─ Data
│  ├─ Repositories
│  ├─ Logging
│  └─ Caching
│
└─ Api (Presentation Layer)
   ├─ Controllers
   ├─ Auth
   ├─ Middlewares
   └─ appsettings.json


💡 C# Latest Version Features (C# 12 / .NET 9)
Focus on writing modern, clean, analyzer-friendly code:
🔹 Language Features
Primary constructors
Required members
File-scoped namespaces
Init-only properties
Record types (for immutable models)
Auto-default non-nullable properties
Collection expressions
Using directives inside namespaces (new .NET 9 style)
Sealed classes (for performance and intent clarity)

dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

dotnet remove package Microsoft.EntityFrameworkCore
dotnet remove package Microsoft.EntityFrameworkCore.SqlServer
dotnet remove package Microsoft.EntityFrameworkCore.Tools

dotnet list . package

dotnet list reference

All the folders should have refrence to each other
All the Core, Api, Infrustucture, Core are class library

## 🔥 Summary Table

| Package                                  | Install In     | Why                                         |
|-------------------------------------------|----------------|---------------------------------------------|
| Microsoft.EntityFrameworkCore             | Infrastructure | EF runtime is used by DbContext here        |
| Microsoft.EntityFrameworkCore.SqlServer   | Infrastructure | Provider belongs with DbContext             |
| Microsoft.EntityFrameworkCore.Tools       | Infrastructure | Migrations live here                        |
| Microsoft.EntityFrameworkCore.Design      | API            | EF CLI needs Program.cs at design-time      |

git init
git add .
git commit -m "Initial commit"
git branch -M main
git remote add origin https://github.com/hanie1988/LearningLead.Api.git
git push -u origin main
```

# 1️⃣ [ApiController]

✔ What this attribute means

You are telling ASP.NET Core:

“This class is an API controller. Apply all API-specific behaviors.”

✔ What ASP.NET automatically enables:
	•	Automatic model validation
If your DTO has [Required] → returns 400 Bad Request without you writing any code.
	•	Automatic binding
JSON body → C# object
Query string → C# parameters
Route values → parameters
	•	Consistent error responses
API controllers return a standard error JSON format.
	•	No need to write [FromBody] on POST/PUT parameters.

This attribute basically gives you a safer, cleaner, more correct API behavior.

# 4️⃣ : ControllerBase

This means:

EventController inherits from ASP.NET Core’s ControllerBase.

✔ Why needed?

It gives your controller access to:
	•	Ok()
	•	NotFound()
	•	BadRequest()
	•	Created()
	•	StatusCode()
	•	ControllerContext
	•	ModelState
	•	Routing helpers

   # C# has three record types:

   1️⃣ Record Class (default) => Reference Type, Stored on heap
   2️⃣ Record Class With Body 
   3️⃣ Record Struct => Value type, Stored on Stack, ✔ Auto-generated Deconstruct, ToString, Equals, GetHashCode S1 == S2 true
   3️⃣ Struct => S1 == S2 False
   3️⃣ readonly record Struct => safe for multithreading


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

# Heap allocation & GC impact