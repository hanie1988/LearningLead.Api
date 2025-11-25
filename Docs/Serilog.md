### Correlation ID

**How to find all the logs related to one request?**

I implemented correlation-ID-based structured logging for tracing requests

Companies here EXPECT developers to know:

how to design global error handling

	•	logging architecture
	•	structured logging
	•	correlation IDs
	•	domain exceptions
	•	monitoring
	•	log pipelines


### ✅ 1. When to use Microsoft’s built-in logging (ILogger)

Use it everywhere in your codebase — services, controllers, repositories, middleware.

Why?

Because:
	•	It’s the standard abstraction layer
	•	It lets you swap logging providers without touching your code
	•	Every library in ASP.NET Core uses it
	•	It has DI built-in

So when do you use Microsoft’s built-in logger?

➡ Always, inside your code.
➡ Every project, every file.

You NEVER inject Serilog’s Log class directly into your services. That’s a beginner mistake.

---

### ✅ 2. When to use Serilog

Serilog is not for calling.
Serilog is for configuring and outputting logs.

You use Serilog for:

✔️ Structured logging (JSON)

✔️ Writing logs to files

✔️ Writing logs to Seq

✔️ Writing logs to Elasticsearch

✔️ Writing logs to Azure Application Insights

✔️ Creating enrichers (CorrelationId, UserId, RequestId, etc.)

✔️ Advanced filtering (e.g., don’t log health checks)

So Serilog is mostly used in:

	•	Program.cs
	•	global logging configuration
	•	request/response logging middleware
	•	enrichers
	•	sinks

Not inside your service code.

---

**Use Microsoft built-in logging when:**
	•	You are writing code (services, controllers, repos)
	•	You want dependency-injected loggers
	•	You want logging abstraction
	•	You want testability

**Use Serilog when:**
	•	You are configuring HOW and WHERE logs are written
	•	You want structured logs
	•	You want advanced sinks (Seq, Elastic, Azure)
	•	You want custom enrichers (Correlation ID, User ID)

---

**They work together.**

This is not a choice.
It’s a combination.**

---

The only two improvements you should add next:

1️⃣ Mask sensitive fields in logs

(e.g., password, token)

2️⃣ Filter out noisy logs

like /health, /swagger, Hangfire dashboards, etc.

Without these two, production logs become trash.

If you want, I can add:

✔ Sensitive data masker
✔ Log filters (ignore health checks)
✔ Better error logging in ErrorHandlingMiddleware
✔ Structured logging standard (what to log / what not to log)
✔ Serilog enrichment for UserId