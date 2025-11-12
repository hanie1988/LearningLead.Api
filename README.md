# 🎫 Mini Ticketing / Booking System  
### Built with C# (.NET 9), Clean Architecture, CQRS, EF Core, SQL Server, Redis, and Modern API Engineering

This project is designed as a complete end-to-end **Ticketing / Booking System** to practice real-world backend architecture, scalable API design, asynchronous processing, and cloud integration.

The system includes:
- Ticket and appointment booking  
- Automatic reminder scheduling  
- Robust logging and retry logic  
- JWT authentication  
- Caching for performance  
- Clean Architecture + CQRS patterns  
- SQL Server or PostgreSQL (both compatible)

This project reflects **real production-quality design**, suitable for interviews and portfolio presentation.

## 🧠 Main Topics Covered

| Area | Capabilities | Interview Coverage |
|------|--------------|--------------------|
| **API Design** | CRUD for Appointment & Reminder | Routing, REST principles, HTTP verbs |
| **Architecture** | Clean Architecture + CQRS | Dependency inversion, separation of concerns |
| **Design Patterns** | Factory, Strategy, Observer, Repository, Mediator | "Explain a pattern you've used in a real project" |
| **Async & Threading** | Background reminder scheduling | async/await, scalability |
| **Database** | EF Core + SQL Server | relationships, migrations, performance tuning |
| **Testing** | xUnit + Mocking | testing best practices |
| **Logging / Retry** | Serilog + Polly | resilience, fault tolerance |
| **Caching** | Redis caching layer | distributed systems, performance |
| **Security** | JWT Authentication | authentication & authorization |

## 📁 Final Folder Structure (Clean Architecture)

```
/src
├── Core (Domain Layer)
│   ├── Entities
│   ├── Enums
│   └── Interfaces
│
├── Application (Use Case Layer)
│   ├── Commands
│   ├── Queries
│   ├── DTOs
│   ├── Validators
│   └── Services
│
├── Infrastructure (Data & Integration Layer)
│   ├── Data
│   ├── Repositories
│   ├── Logging
│   ├── Caching
│   └── Configurations
│
└── Api (Presentation Layer)
    ├── Controllers
    ├── Auth
    ├── Middlewares
    └── appsettings.json
```

This structure follows **strict separation of concerns**, **dependency direction**, and **testability principles**.

---

## 🚀 Core Features

### **✔ Clean Architecture**
Highly maintainable, scalable, and testable structure that keeps domain logic independent from frameworks and infrastructure.

### **✔ CQRS (Command & Query Separation)**
- Commands handle writes  
- Queries handle reads  
This keeps logic focused and improves performance.

### **✔ Appointment & Reminder Module**
- Create appointments  
- Background service schedules reminders  
- Jobs run asynchronously  
- Supports retries and fault-tolerant execution  

### **✔ Logging & Retry**
- Serilog for structured logs  
- Polly for retry policies, fallback behavior, and circuit breakers  

### **✔ Caching Layer**
- Redis for caching heavy queries  
- Reduces database load  
- Boosts API performance  

### **✔ Authentication & Authorization**
- JWT Bearer tokens  
- Token validation middleware  
- Secured endpoints  

---

## 🌐 API Endpoints (Examples)

GET     /api/tickets
POST    /api/tickets
PUT     /api/tickets/{id}
DELETE  /api/tickets/{id}

POST    /api/appointments
GET     /api/appointments/upcoming

POST    /api/auth/login
POST    /api/auth/register

---

## 🗄 Database
- SQL Server (local or Azure)  
- EF Core 9  
- Code-First Migrations  
- Proper indexing strategies  
- Relationships with navigation properties  

---

## 🧪 Testing Approach
- **xUnit** for unit tests  
- **Moq** for mocking dependencies  
- Repository tests  
- Service-level tests  
- Integration tests for API controllers  

---

## ☁️ Optional Azure Integration
The project can be connected to:
- **Azure SQL Server**
- **Azure App Service (for hosting the API)**
- **Azure Cache for Redis**
- **Azure Key Vault** for secure secrets

This is optional but included for cloud learning.

---

## 📘 Study Notes (What This Project Helps Me Practice)
- RESTful API design  
- Clean Architecture + CQRS  
- EF Core performance patterns  
- Asynchronous background jobs  
- Distributed caching  
- Retry logic & fault tolerance  
- Authentication/authorization with JWT  
- Cloud deployment basics  
- Real-world architecture explanation for interviews  

---

## 🏁 How to Run Locally

### 1. Update `appsettings.json`  
Add your SQL Server connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=TicketingDB;User Id=sa;Password=YourPassword;"
}

### 2. Apply database migrations:
dotnet ef database update


## 🚀 Core Features

- Clean Architecture
- CQRS
- Appointment & Reminder Module
- Logging & Retry
- Caching Layer
- JWT Auth

## 🏁 How to Run Locally

1. Update appsettings.json connection string  
2. Run `dotnet ef database update`  
3. Run API  
4. Open Swagger

