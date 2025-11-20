### 30-Day .NET Plan With Exact Time Estimates

**Your 1-month stack (locked, no jumping):**

	•	Backend: .NET 9 Web API + C#
	•	Database: EF Core + SQL Server
	•	Core Concepts: Clean structure, validation, global exception handling, logging, JWT auth
	•	Excluded: React, Redis, Azure, design patterns (for now)


### 7-Day Execution Plan (with exact hours)

**Day 1 & Day 2 — Reservation Core**
Total needed: 6 hours (3 hours/day)

Tasks:
	•	Build clean entity: Reservation
	•	Build Repository (with AnyAsync, queries, etc.)
	•	Build Service (RoomIsAvailable logic)
	•	Build Controller (CRUD: Create, Read, Cancel)

Time breakdown:
	•	Entity + Repository: 1.5 hrs
	•	Service logic (RoomIsAvailable + conflicts): 2 hrs
	•	Controller + endpoints: 1.5 hrs
	•	Manual testing (Swagger/Postman): 1 hr

---

**Day 3 & Day 4 — Validation + Error Handling**

Total needed: 6 hours (3 hours/day)

Tasks:
	•	DTO validation:
	•	Invalid date → 400
	•	Room not found → 404
	•	Not available → 422
	•	Create consistent response structure
	•	Build Global Exception Middleware

Time breakdown:
	•	DTO validation (FluentValidation or manual): 2 hrs
	•	Standard API response model: 1 hr
	•	Global Exception Middleware: 2 hrs
	•	Manual testing: 1 hr

---

**Day 5 & Day 6 — Logging**

Total needed: 6 hours (3 hours/day)

Tasks:
	•	Add logging for Reservation operations:
	•	start request
	•	finish request
	•	failed reservation
	•	Add logging inside Global Exception Middleware:
	•	correlation ID
	•	path
	•	status code

Time breakdown:
	•	Request logging middleware: 2 hrs
	•	Exception logging: 2 hrs
	•	Logging inside service methods: 1 hr
	•	Review logs + testing: 1 hr

---

**Day 7 — Review + Documentation**

Total needed: 3 hours

Tasks:
	•	Go through entire project end-to-end
	•	Write clear README (for interviews)
	•	Cleanup code: naming, structure, remove dead code

Time breakdown:
	•	Verbal review (talk through pipeline): 1 hr
	•	README.md writing: 1 hr
	•	Code cleanup: 1 hr

---

**Your 30-Day Month: Full Breakdown**

**Week 1 (21 hours)**

Everything above (Reservation + Validation + Error Handling + Logging)

**Week 2 (15 hours)**
	•	Implement User, Hotel, Room modules cleanly
	•	CRUD + Validation + Basic Filtering
	•	Add pagination + sorting basics
	•	Add basic unit tests for services

**Week 3 (15 hours)**

	•	Add JWT Auth (Register + Login)
	•	Add Roles: Admin / Customer
	•	Secure endpoints
	•	Add simple caching (optional but small)

**Week 4 (15 hours)**
	•	Refactor for interview quality
	•	Add more tests
	•	Improve logs
	•	Add README + Architecture Diagram
	•	Practice explaining the project

---

### 📌 Month 2 Summary You Can Save

```Csharp
MONTH 2 — JOB-READY SKILL UPGRADE

Week 1 — Advanced .NET
: DI, middleware, logging, async, filters.

Week 2 — EF Core Level-Up
: relationships, tracking, migrations, transactions.

Week 3 — Light Cloud + DevOps
: Azure App Service, GitHub Actions, App Insights, Key Vault.

Week 4 — Interview Prep
: explain project, system design basics, mock interviews, simple DS/Algo.
```

---

📌 Month 3 — CLEAN Printable Summary

```Csharp
MONTH 3 — BECOME MID-LEVEL .NET BACKEND

Week 1: System Design (backend-level)
- Endpoint design, pagination, versioning, error flow, caching concept.

Week 2: Testing
- Unit tests (services), mocks, integration tests, validation tests.

Week 3: Light Architecture
- DTOs, mapping, layers, config options pattern, clean services.

Week 4: Deployment
- Docker, Azure App Service, Key Vault, GitHub Actions, App Insights.
```

---

🔥 MONTH 4 — SOLID MID-LEVEL FOUNDATIONS

(Focus: Depth without overwhelm + rewriting your Booking System cleanly)

Month 4 is about cleaning, strengthening, and solidifying everything you built.

Most candidates fail here because they jump — you won’t.

---

🔥 MONTH 5 — ADVANCED JOB MARKET PREP + SECOND PROJECT

(Focus: breadth + confidence + portfolio)

Now that your booking system is stable, we build a small second project to show versatility.

This will dramatically increase your job chances.

---

🔥 MONTH 6 — INTERVIEW EXECUTION + REAL JOB APPLICATIONS

(Focus: confidence, consistency, precision)

This is the month you behave like a mid-level dev.