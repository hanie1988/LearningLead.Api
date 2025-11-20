### ✅ Real Logging + Global Exception Handling (The Full Picture)

A production-grade backend ALWAYS has 4 levels of protection:

**1️⃣ Global Exception Middleware (Central Brain)**

This is the most important part.

It catches everything that happens in your API:
	•	NullReferenceException
	•	SQL exceptions
	•	Unauthorized
	•	Validation errors
	•	Business rule failures
	•	Any unhandled bug

This middleware:
	•	logs the error
	•	logs correlation ID
	•	logs request body (optional)
	•	returns a clean JSON response
	•	prevents leaking stack traces

**2️⃣ Logging in Controllers (BUT minimal)**

➡️ Controllers should NOT have heavy logs.
➡️ They should NOT use try/catch everywhere.

Instead, they should only log:
	•	Entry / Exit (optional)
	•	Important inputs (masked)
	•	High-level events

**3️⃣ Business Logic Exceptions (Custom Domain Exceptions)**

This is the part you asked about.

In a booking system, these are common custom domain exceptions:
	•	RoomNotAvailableException
	•	PaymentFailedException
	•	OverbookingException
	•	InvalidBookingDateException
	•	UserNotAuthorizedException

**4️⃣ Logging inside Services (Business-Level Logs)**

This is where most companies log the MOST important things.

Service logs include:
	•	“User attempted login”
	•	“Hotel created with ID 23”
	•	“Room booking attempt for dates …”
	•	“Overbooking prevented”
	•	“Payment started”
	•	“Payment success”
	•	“Payment failed: {Error}”
	•	“User reservation cancelled”

These logs help you debug production without stepping into code.

---

	•	how to design a global exception middleware
	•	how to build a logging pipeline
	•	how to add correlation IDs
	•	how to map domain errors to HTTP responses
	•	how to do structured logging
	•	how to debug production issues
	•	how to track requests across multiple services
	•	how to integrate with Azure monitoring

### ⭐ Before we start: What will “Enterprise Logging + Global Errors” give you?
You will gradually learn:
	1.	How to catch all errors in one place (instead of try/catch everywhere)
	2.	How to convert domain errors → clean HTTP responses
	3.	How to normalize logs (same format everywhere)
	4.	How to track a single request across all layers
	5.	How to debug production issues like a real senior

**❗ A good backend system NEVER throws raw exceptions to controllers.**

Instead:
	•	Domain layer throws business errors (like “RoomUnavailable”).
	•	Middleware converts errors → proper HTTP status codes.
	•	Logging framework records errors with context.
	•	Developers debug by reading one clean log.

---

### ⭐ Step 1 — Add a Custom Exception for Business Logic (very small step)

This helps us separate:
	•	real unexpected system errors → 500
	•	expected business errors → 400 / 409 / 422

👉 Add this file:

Application/Common/AppException.cs

```Csharp
namespace Application.Common;

public sealed class AppException : Exception
{
    public int StatusCode { get; }

    public AppException(string message, int statusCode = 400)
        : base(message)
    {
        StatusCode = statusCode;
    }
}
```

Why this is important?

Because in real companies:
	•	“User already exists” → 409
	•	“Room unavailable” → 422
	•	“Invalid date range” → 400

We never treat these as server crashes.

### ⭐ Step 2 — Modify your reservation overlap logic to use AppException

Example:

```Csharp
if (overlap)
    throw new AppException("Room is already booked for these dates.", 422);
```

Meaning:
	•	If you throw an exception → .NET returns 500 Internal Server Error
	•	If you return BadRequest() in controller → .NET returns 400
	•	If you do nothing special → .NET does NOT guess the status code

This is exactly why we create:
✔ AppException
✔ Global Error Middleware
✔ Business rules that throw typed errors

This makes .NET behave like a real enterprise system.

.NET returns:
```Csharp
500 Internal Server Error
```

But that’s WRONG.
Because it’s NOT a server crash —
it’s a client mistake.

---

### 🔥 Let me show you the FULL FLOW (super clear)

1️⃣ Service Layer (you throw custom error)

```Csharp
if (emailExists)
    throw new AppException("User already exists", 409);
```

2️⃣ Middleware catches AppException

Example:

```Csharp
catch (AppException ex)
{
    context.Response.StatusCode = ex.StatusCode;
    await context.Response.WriteAsJsonAsync(new { error = ex.Message });
}
```

3️⃣ API returns correct status:

Example error responses:

409 Conflict
```Csharp
{
  "error": "User already exists"
}
```
.NET DOES NOT DO THIS AUTOMATICALLY.
WE add this behavior.

---

### ⭐ Step 3 — Add Global Error Middleware

(Handles all AppException & unexpected errors)

This middleware will:
	•	Catch your custom business exceptions
	•	Catch unexpected server errors
	•	Return clean JSON responses
	•	Avoid ugly .NET stack traces
	•	Make your logs + API predictable

---

### 🔥 2️Create the Middleware

```Csharp
using Application.Common.Exceptions;

public sealed class ErrorHandlingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (AppException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;

            await context.Response.WriteAsJsonAsync(new
            {
                error = "Internal server error",
                detail = ex.Message // remove this in production
            });
        }
    }
}
```

✔ Catches business exceptions
✔ Catches unexpected errors
✔ Returns JSON response
✔ No stack trace leaks
