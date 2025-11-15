### 🔥 Q1: Explain async/await in .NET and when you use it.

✅ Perfect Senior Answer (short + clear):

**“Async/await in .NET is a way to free the thread while waiting for I/O operations, like database queries, HTTP calls, or file access. It doesn’t make the code run faster — it just prevents blocking the thread so the server can handle more requests.

I use async/await for all I/O-bound operations, especially in Web APIs, because it improves scalability. But I don’t use async for CPU-bound work — for those cases I use background services, Hangfire, or a worker queue.”**

### 🔥 Q2: What is the difference between Task, Thread, and async/await?

✅ Perfect Senior Answer:

**“A Thread is a physical OS thread. Task is a higher-level abstraction that represents work that may run asynchronously.

Async/await is not a thread — it’s a compiler feature that unblocks the thread while waiting for I/O.

In Web APIs, async/await frees the thread so ASP.NET Core can reuse it for other requests. That’s why it improves scalability.”**

### 🔥 Q3: Why does async make Web APIs scale better?

✅ Perfect Senior Answer:

“Because the main thread is not blocked during I/O. ASP.NET Core returns the thread to the thread pool while waiting for data. That allows the server to handle more concurrent requests using fewer threads.”

### 🔥 Q4: What is AsNoTracking in EF Core and when do you use it?

✅ Perfect Senior Answer:

**“AsNoTracking tells EF Core not to track entities in the change tracker.
It makes queries faster and uses less memory.

I use AsNoTracking for read-only operations, like list pages, reports, or any query where I don’t need to update the entity.”**


### 🔥 Q5: How do you optimize SQL Server queries?

✅ Perfect Senior Answer:

*“I check the execution plan, look for table scans, missing indexes, or key lookups.
I add proper indexing, avoid SELECT , reduce joins, and use pagination.
I also check parameter sniffing and use stored procedures for performance-critical queries.”

### 🔥 Q6: What is the difference between Include and ThenInclude in EF Core?

✅ Perfect Senior Answer:

“Include loads related navigation properties.
ThenInclude is used when you want to load nested related data.
I use them carefully because eager loading can create large SQL queries. For large data I prefer projection with Select.”

### 🔥 Q7: How do you design a clean REST API?

✅ Perfect Senior Answer:

“I use DTOs, validation, proper status codes, async controllers, clean service layer, repository for DB access, and centralized error handling. I follow a consistent folder structure and return predictable responses.”

### 🔥 Q8: What is Dependency Injection and why is it important?

✅ Perfect Senior Answer:

“Dependency Injection helps with loose coupling, testability, and clean architecture.
Instead of creating objects inside classes, the framework provides them.
It also makes unit testing possible and keeps the code maintainable as the system grows.”


### 🔥 Q9: What is Clean Architecture in .NET?

✅ Perfect Senior Answer:

**“Clean Architecture separates the application into layers:
	•	API (controllers)
	•	Application (business logic)
	•	Domain (entities, rules)
	•	Infrastructure (database, external services)

The rule is: dependencies go inward.
This keeps the system maintainable, testable, and easy to change.”**

### 🔥 Q10: Explain Redis caching and when to use it.

✅ Perfect Senior Answer:

“Redis is an in-memory distributed cache.
I use it for expensive queries, frequently accessed data, and reducing load on SQL Server.
It improves performance and response time, especially in high-traffic APIs.”