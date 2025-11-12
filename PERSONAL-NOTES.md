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