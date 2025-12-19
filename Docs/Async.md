
### 🔥 Async Concepts You Should Master (Full Roadmap)

**Level 1 — Fundamentals (no skipping)**

These are core mental models. If weak, everything collapses later.
| Topic                     | Core Questions You Must Be Able to Answer                         |
|---------------------------|--------------------------------------------------------------------|
| Thread vs Task            | What executes code vs what represents work?                        |
| I/O-bound vs CPU-bound    | When does async help? When does it hurt?                           |
| Await continuation model  | What happens between `await` and resume?                           |
| ThreadPool & Scheduling   | How do threads get assigned? What is starvation?                   |
| Context switching         | Why too many threads slow apps?                                    |


**Level 2 — Async Mechanics**

This is where you already made progress — but need deeper practice.
| Topic                         | What you must be able to explain                                 |
|-------------------------------|-------------------------------------------------------------------|
| Continuation Queue            | Where, how, and when code resumes after `await`                   |
| SynchronizationContext        | Why UI apps deadlock and Web APIs don’t                           |
| ConfigureAwait(false) vs true | When context capture is useful, when it kills perf                |
| .Result, .Wait() deadlocks    | Why blocking async is poison in sync apps                         |
| ValueTask vs Task             | When it saves allocations and when it backfires                   |

**Level 3 — Concurrency**

This is where async stops being code and becomes engineering decision-making.
| Tool                   | You must know *when* to use it                                        |
|------------------------|------------------------------------------------------------------------|
| Task.WhenAll()         | Good bulk async, but dangerous without limits                          |
| SemaphoreSlim          | Controlled concurrency / throttling                                    |
| Parallel.ForEachAsync  | Hybrid CPU+async workloads, but heavy if misused                       |
| Thread starvation      | How to detect overload, how to avoid it                                |


**Level 4 — Advanced Performance**

(We can do this only after Level 1–3 are fully internalized)
| Topic                           | Why it matters                                                     |
|--------------------------------|---------------------------------------------------------------------|
| Batching async work            | 10k items processed in chunks of 200 instead of 10k at once        |
| Pipelining patterns            | Multi-stage processing like ETL, scrapers, brokers                 |
| Deduplication of work          | Avoid double tasks under load                                       |
| Retry policies & backoff       | Distributed-safe resiliency                                        |



**Level 5 — High-Scale Architecture**
We will reach this later — step by step.
| Topic                            | Where used                                         |
|----------------------------------|---------------------------------------------------|
| Channel<T> pipelines             | Background job systems, ingestion pipelines       |
| Backpressure design              | Prevent input from killing your system            |
| Async message queues (RabbitMQ/Kafka) | Horizontal scalability at company level     |


---

### 🔥 CPU Cores vs Threads vs Tasks (clear, clean model)

1️⃣ CPU Core
	•	A physical execution unit inside your processor.
	•	Each core can run only one thread at a time (ignoring hyper-threading for now).
	•	More cores = more real parallel work.

2️⃣ Thread
	•	A virtual execution path scheduled on a CPU core.

**If a machine has 8 CPU cores and I set MaxDegreeOfParallelism = 50,
what happens and why is it a bad idea?**

What I expect you to understand:

You have 8 physical execution seats (cores).
But you’re trying to schedule 50 workers (threads) to run at the same time.

Real outcome:
	•	Only 8 threads can execute at once.
	•	42 threads are waiting, doing nothing.
	•	CPU keeps switching between them, costing more time than doing work.
	•	More threads ≠ faster — it can be slower.

### 🔥 Why doesn’t the CPU simply finish the first 8 threads and then move to the next 42?

Because CPU scheduling is based on fairness + responsiveness, not completion order.

Real reason:

The OS assumes every thread is important.
So it tries to give each thread a slice of CPU time.

It does time slicing:
	1.	Thread runs for a tiny period (milliseconds or microseconds)
	2.	CPU switches to the next thread
	3.	Saves and restores registers, stack, cache, execution state
	4.	Repeat… over and over…

This switching is called context switching — and it is expensive.

### 🔥 The key principle to burn into your brain:

More threads ≠ more speed

More parallelism than cores = slower, not faster

Async ≠ parallel execution
---


| UI apps / old ASP.NET                                      | ASP.NET Core                                                                  |
| ---------------------------------------------------------- | ----------------------------------------------------------------------------- |
| Has SynchronizationContext                                 | **No SynchronizationContext**                                                 |
| Continuation must resume on original thread                | Continuation runs on **any ThreadPool thread**                                |
| `.Result` blocks UI thread → continuation is stuck waiting | `.Result` blocks a ThreadPool thread, but continuation can run somewhere else |
| Deadlock is common                                         | Deadlock does **not** happen the same way                                     |

---

UI frameworks & old ASP.NET:
    SynchronizationContext forces continuation to UI thread.
    .Result blocks UI thread.
    Continuation waits for UI thread → UI waits for continuation → deadlock.

ASP.NET Core:
    No SynchronizationContext.
    Continuation goes to ThreadPool.
    .Result doesn't deadlock, but blocks a ThreadPool thread → scalability death.

---

Q1: Why does .Result or .Wait() easily deadlock UI apps?

Q2: Why is the same code less likely to deadlock ASP.NET Core?

Q3: Even in ASP.NET Core — why is .Result still considered harmful?

Q4: Why do UI applications (WPF/WinForms) need SynchronizationContext?

Q5: What benefit does ConfigureAwait(false) give in server-side code (like Web APIs)?

Q6: What is the main performance advantage of ValueTask?

Q7: Why can ValueTask be worse than Task in many real systems?

Q8: When would you personally choose ValueTask in your codebase?

Q9: Why does limiting concurrency with SemaphoreSlim make a system more stable than running all tasks at once?

Q10: Why can Parallel.ForEachAsync be dangerous if we don’t set MaxDegreeOfParallelism?

### Q: Does ConfigureAwait(false) mean no context capture? Yes.
```Csharp
await SomeTask.ConfigureAwait(false);
// continuation does NOT try to return to the original context
```

But now the trap:
In ASP.NET Core there is no SynchronizationContext — correct.
So capturing context is pointless, because there's nothing meaningful to capture.
So far your logic is correct.

**Q: So why do we still care about ConfigureAwait(false) in Web API?**
Because you're stuck thinking only about UI synchronization.
You’re missing the real performance reason.
**Here is the real answer:**
Even though ASP.NET Core has no SyncContext, it does have an ExecutionContext.
ConfigureAwait(false) avoids restoring ExecutionContext + AsyncLocals + HttpContext flow, not just UI sync. That still has overhead.

**AsyncLocal solves a real problem:**
“How do I store per-request data when threads are not stable?”

Used by:
	•	ASP.NET Core (HttpContext)
	•	Logging scopes (ILogger.BeginScope)
	•	Correlation IDs
	•	OpenTelemetry Activity
	•	Security context
	•	Culture (CurrentCulture)
	•	EF Core diagnostics

Without AsyncLocal, async code would be untraceable chaos.
What actually happens under the hood (simplified), On await without ConfigureAwait(false)

```Code
Capture ExecutionContext
  └─ AsyncLocal #1
  └─ AsyncLocal #2
  └─ HttpContext
  └─ Logging scope
  └─ Culture
↓
Async I/O
↓
Restore ExecutionContext
  └─ Restore AsyncLocal values
```
Even though:
	•	Threads may change
	•	Execution is paused and resumed

CurrentUser.Value is still "Alice".
```Csharp
static AsyncLocal<string?> CurrentUser = new();

async Task HandleAsync()
{
    CurrentUser.Value = "Alice";

    await DoWorkAsync();

    Console.WriteLine(CurrentUser.Value); // "Alice"
}

async Task DoWorkAsync()
{
    await Task.Delay(10);
}
```
You used AsyncLocal below without knowing its name.
```Csharp
HttpContext.User
HttpContext.Request.Headers
HttpContext.TraceIdentifier

using (_logger.BeginScope("OrderId:{OrderId}", orderId))
{
    await ProcessAsync();
}

User.Identity.IsAuthenticated

Thread.CurrentThread.CurrentCulture
```

### Misconsuption

“more async = more threads,” which is not accurate.
Async increases concurrent I/O, not CPU threads. But it still overloads resources such as:

✔ DB connections
✔ SMTP servers
✔ Disk I/O
✔ ThreadPool dispatch for continuations
✔ Memory for task tracking

Using SemaphoreSlim keeps the system stable because it limits how many operations run concurrently.
Without it, a million users could trigger a million outgoing I/O requests, overwhelming networks, DB pools, SMTP servers or threadpool continuations.
With concurrency control, only a fixed number of async operations are active at any moment, letting the system process work gradually without collapse.

### When to use Task.WhenAll + SemaphoreSlim

Use when work is purely async + I/O bound.

Examples:
| Good for                 | Why                                                   |
|--------------------------|-------------------------------------------------------|
| Hundreds of DB queries   | Concurrency control prevents connection exhaustion    |
| Bulk email sending       | Avoids SMTP throttling errors                         |
| Many HTTP calls          | Prevents rate-limit disaster                          |
| File uploads/downloads   | Prevents threadpool starvation on continuation        |

Reason:

SemaphoreSlim is the safest throttle for async workloads.
No unnecessary threads → no CPU pressure → high stability.

---

If the work is mostly I/O → choose this.

When to use Parallel.ForEachAsync + MaxDegreeOfParallelism

Use when the workload involves CPU + async mixed processing.

Examples:
| Good for                         | Why                                     |
|----------------------------------|------------------------------------------|
| Parsing files and saving results | CPU work can scale across cores          |
| Image processing + upload        | CPU scaling + async write                |
| ETL transformations              | Multiple cores benefit                   |
| Processing thousands of records  | Parallel loops reduce wall time          |

Reason:

It can utilize multiple CPU cores efficiently.
But it’s more dangerous — misuse floods infrastructure.

If there is meaningful CPU work → choose this.


---

**Suppose you need to fetch 10,000 users, process each with CPU-heavy logic, and then save the result asynchronously to DB.
Which approach do you choose and why?**

I’d use Parallel.ForEachAsync with a controlled MaxDegreeOfParallelism.
The CPU work can scale across multiple cores for speed, while the degree limit prevents DB writes from exploding and exhausting the connection pool.
This gives parallel throughput without losing system stability.

---

**A) CPU & Execution (1–10)**
What does the CPU actually execute — instructions or tasks?
What is a thread in relation to CPU cores?
What is a context switch, and why is it expensive?
What happens when you have more threads than CPU cores?
How does cache locality affect CPU performance?
Why does parallel code sometimes run slower than single-threaded?
What is branch misprediction and why does it stall pipelines?
How does memory allocation impact CPU work?
What happens when GC pauses occur under heavy load?
Why is CPU-bound async often worse than synchronous code?

---

**B) Threading & Scheduling (11–22)**
How does the OS schedule threads across cores?
What is preemption?
Why do too many threads degrade performance?
What is thread starvation?
How does ThreadPool differ from raw threads?
How does ThreadPool grow when under pressure?
Why can ThreadPool exhaustion freeze an API?
What is work-stealing scheduling?
When should you use a dedicated thread instead of ThreadPool?
How do synchronization primitives block threads?
What is a deadlock?
What is livelock?

---

**C) Async/Await Continuation Model (23–35)**
What happens at the exact moment await is hit?
Where does continuation go after the awaited Task completes?
What is the difference between returning to context vs ThreadPool?
Why does .Result block a thread while await doesn’t?
What happens when a continuation requires a new ThreadPool thread?
Why is .Wait() / .Result toxic in scalable server code?
When can .Result deadlock a UI or legacy ASP.NET application?
Why does ASP.NET Core avoid deadlocks by default?
What exactly does ConfigureAwait(false) skip?
Why is ConfigureAwait(false) still useful in Web APIs?
What is ExecutionContext and why does it matter?
Why does async improve I/O-bound workloads but not CPU-bound?
When is ValueTask better than Task — and when does it backfire?

---

**D) Performance & Scalability (36–50)**
What is throughput vs latency?
Why is batching more scalable than firing 10k Tasks at once?
When should you use Task.WhenAll() vs parallel loops vs pipelines?
How do Channels improve ingestion throughput?
What is backpressure and how do you apply it?
Why is SemaphoreSlim essential for throttling async work?
When does parallel code saturate CPU instead of helping?
How do you detect when your service is CPU-bound vs thread-bound?
How do you diagnose ThreadPool starvation?
Why does retry with exponential backoff prevent cascading failures?
How do distributed systems handle idempotency?
What is a work-queue architecture good for?
Why do microservices fail under load when queues are missing?
When is Kafka/RabbitMQ mandatory for scaling?
Why does real scalability require eliminating blocking everywhere?

---
# Thread Safety & Synchronization in .NET
*Why concurrency bugs feel random (but are not)*

---

## 1️⃣ What “thread-safe” really means

**Thread-safe means:**
> Correct behavior even when accessed concurrently, without relying on timing or luck.

It does **not** mean:
- “It usually works”
- “It didn’t crash”
- “I tested it once”

If correctness depends on timing → **not thread-safe**.

---

## 2️⃣ The smallest broken example (race condition)

```csharp
int count = 0;

void Increment()
{
    count++;
}
```

This looks harmless. It is **not thread-safe**.

### Why `count++` is broken
`count++` is actually three steps:
1. Read `count`
2. Add 1
3. Write back

Possible interleaving:
```
Thread A reads 0
Thread B reads 0
Thread A writes 1
Thread B writes 1
```

Final value = **1 instead of 2**

This is a **race condition**.

---

## 3️⃣ Atomic vs non-atomic operations

- **Atomic** → happens as one indivisible step => Cannot be split into smaller steps that others can observe in between.
- **Non-atomic** → can be interrupted

### Atomic example, [Lock vs Interlocked](real-world-lock-vs-lock-this.md)
```csharp
Interlocked.Increment(ref count);
```

### Non-atomic examples
```csharp
count++;
list.Add(item);
dictionary[key] = value;
```

**Rule:**
> Simple-looking code is not automatically safe.

---

## 4️⃣ Visibility vs ordering (core mental model)

Two separate problems:

### Visibility
A thread updates a value, another thread may **not see it yet**.

### Ordering
Operations may execute in a different order than written.

Example:
```csharp
bool ready = false;
int data = 0;

// Thread A
data = 42;
ready = true;

// Thread B
if (ready)
{
    Console.WriteLine(data); // may print 0
}
```

Yes, this can really happen.

---

## 5️⃣ `lock` (Monitor) — what it actually guarantees

```csharp
private readonly object _sync = new();

lock (_sync)
{
    count++;
}
```

`lock` guarantees:
- Mutual exclusion (one thread at a time)
- Visibility of changes
- Correct ordering

It protects **invariants**, not just variables.

---

## 6️⃣ Common `lock` mistakes

### ❌ [Locking on `this`](real-world-lock-vs-lock-this.md)
```csharp
lock (this) { }
```
External code can lock it → hidden deadlocks.

### ❌ Locking on string
```csharp
lock ("abc") { }
```
Strings are interned and shared.

### ❌ Large lock scope
```csharp
lock (_sync)
{
    CallExternalService(); // ❌
}
```

Locks should be **short and boring**.

---

## 7️⃣ Deadlocks (why everything freezes)

Deadlock scenario:
- Thread A holds lock A, waits for B
- Thread B holds lock B, waits for A

Example:
```csharp
lock (a)
{
    lock (b) { }
}
```

Elsewhere:
```csharp
lock (b)
{
    lock (a) { }
}
```

**Rule:**
> Always lock multiple resources in the same order.

---

## 8️⃣ `Interlocked` — lock-free and fast

```csharp
Interlocked.Increment(ref count);
```

Benefits:
- Atomic
- No blocking
- Scales well

Limitations:
- Only simple operations
- No multi-variable invariants

Use for:
- Counters
- Flags
- Statistics

---

## 9️⃣ Concurrent collections (what they do and don’t)

```csharp
var dict = new ConcurrentDictionary<int, string>();
```

They guarantee:
- Internal thread safety
- No data corruption

They do **not** guarantee:
- Logical correctness
- Multi-step atomicity

❌ Unsafe:
```csharp
if (!dict.ContainsKey(key))
{
    dict[key] = value;
}
```

✅ Correct:
```csharp
dict.GetOrAdd(key, value);
```

---

## 🔟 Async-compatible synchronization

### ❌ Illegal
```csharp
lock (_sync)
{
    await Task.Delay(100);
}
```

Why?
- `lock` blocks threads
- `await` yields execution

---

### ✅ `SemaphoreSlim`

```csharp
private readonly SemaphoreSlim _sem = new(1, 1);

await _sem.WaitAsync();
try
{
    await DoWorkAsync();
}
finally
{
    _sem.Release();
}
```

This is **async-safe locking**.

Used for:
- Web APIs
- Rate limiting
- Throttling
- Shared async resources

---

## 1️⃣1️⃣ Golden rules

- Thread safety is about **state**
- Async does not make code safe
- `lock` protects invariants
- `Interlocked` > `lock` for simple ops
- Concurrent collections ≠ correct logic
- Prefer designs that avoid sharing

---

## 1️⃣2️⃣ Self-check

You should be able to answer:
1. Why is `count++` unsafe?
2. What does `lock` guarantee?
3. When is `Interlocked` better than `lock`?
4. Why are concurrent collections insufficient?
5. Why can’t you `await` inside a `lock`?

---

## 🔚 Summary

Concurrency bugs are not random.  
They are the result of **unprotected shared mutable state**.

Mastering synchronization turns “scary” async systems into predictable ones.

---

# ASP.NET Core: Shared State (“shared access variables”) + DI Rules (Transient vs Scoped)

> Goal: explain **where shared state lives**, **why it breaks**, and **how DI lifetimes (Transient/Scoped/Singleton)** control object creation and safety.

---

## 0) The core mental model (don’t go too deep here)

ASP.NET Core is a **multi-request, multi-threaded** server.

- Many requests run at the same time.
- A single “variable” can be shared accidentally across requests.
- Your DI lifetimes decide **who shares what**.

**Rule of thumb (coach rule):**
- If something must be isolated per request → **Scoped**
- If something is stateless → **Transient**
- If something is global and thread-safe → **Singleton**
- If you are “not sure” → default to **Scoped** for app services that touch data/context.

---

## 1) What “shared access variables” usually means in ASP.NET Core

People say “shared variables” when they notice:
- Data from one user appears in another user’s request
- A counter increments unexpectedly
- Random bugs under load (race conditions)

These come from **shared state** being stored in the wrong place:

### 1.1 Bad shared state sources
1) **`static` fields / singletons holding mutable data**
2) **Singleton services containing request-specific data**
3) **Capturing scoped services in singletons**
4) **Caching request data in global memory without user/request keys**
5) **Using “instance fields” inside middleware incorrectly (rare but possible)**

### 1.2 Good state sources (safe patterns)
- Per-request state: `HttpContext.Items`, scoped services, request DTOs
- Per-user state: distributed cache (Redis) keyed by user id, DB, cookie/session (if used)
- Shared read-only config: `IOptions<T>` (mostly safe), immutable objects

---

## 2) Lifetimes: Transient vs Scoped (and where Singleton fits)

### 2.1 Transient
**Meaning:** new instance every time it’s requested from DI.

- Resolve it twice in the same request → you get **two different objects**
- Great for **stateless helpers** (formatters, calculators, validators if stateless)

**Risk:** If it holds expensive resources (DB connections) or per-request state, you’ll create too many objects and/or inconsistent state.

### 2.2 Scoped
**Meaning:** one instance per **request scope**.

- Resolve it many times in the same request → you get the **same object**
- Next request → new object
- Perfect for:
  - `DbContext`
  - repositories
  - services that depend on `DbContext`
  - anything that must behave consistently inside a single request

### 2.3 Singleton (for context)
**Meaning:** one instance for the whole application lifetime.

- Shared by all requests, all users
- Safe only if:
  - fully stateless, or
  - state is immutable, or
  - state is protected with correct thread-safety (locks, concurrent collections) and you truly intend global sharing

**Common trap:** “I made it singleton to be faster” → then you put mutable fields inside → race conditions.

---

## 3) Concrete example: Transient vs Scoped resolution behavior

### 3.1 Example services
```csharp
public interface IRequestTracker
{
    Guid InstanceId { get; }
    int Count { get; }
    void Increment();
}

public sealed class RequestTracker : IRequestTracker
{
    public Guid InstanceId { get; } = Guid.NewGuid();
    public int Count { get; private set; }
    public void Increment() => Count++;
}
```

### 3.2 Controller that resolves twice in one request
```csharp
[ApiController]
[Route("demo")]
public sealed class DemoController : ControllerBase
{
    private readonly IRequestTracker _trackerA;
    private readonly IRequestTracker _trackerB;

    public DemoController(IRequestTracker trackerA, IRequestTracker trackerB)
    {
        _trackerA = trackerA;
        _trackerB = trackerB;
    }

    [HttpGet("lifetimes")]
    public IActionResult Get()
    {
        _trackerA.Increment();
        _trackerB.Increment();

        return Ok(new
        {
            A = new { _trackerA.InstanceId, _trackerA.Count },
            B = new { _trackerB.InstanceId, _trackerB.Count },
            SameInstance = ReferenceEquals(_trackerA, _trackerB)
        });
    }
}
```

### 3.3 Register as **Transient**
```csharp
builder.Services.AddTransient<IRequestTracker, RequestTracker>();
```

Expected result (same request):
- `SameInstance` → **false**
- `Count` for each will be `1` (each tracker is separate)

### 3.4 Register as **Scoped**
```csharp
builder.Services.AddScoped<IRequestTracker, RequestTracker>();
```

Expected result (same request):
- `SameInstance` → **true**
- `Count` will be `2` (both refs point to same instance)

**This is the simplest way to *prove* lifetime behavior.**

---

## 4) “Shared variable” bug: why singleton + mutable fields is dangerous

### 4.1 The bug
```csharp
public interface ICurrentUserCache
{
    string? CurrentUserEmail { get; set; }
}

public sealed class CurrentUserCache : ICurrentUserCache
{
    public string? CurrentUserEmail { get; set; }
}
```

If you register this as singleton:

```csharp
builder.Services.AddSingleton<ICurrentUserCache, CurrentUserCache>();
```

Now if request A sets `CurrentUserEmail = "a@x.com"` and request B sets it to `"b@x.com"`,
they can overwrite each other.

**Result:** user A can see user B’s data.

### 4.2 Correct fix: per-request state should be scoped
```csharp
builder.Services.AddScoped<ICurrentUserCache, CurrentUserCache>();
```

Or better: don’t store “current user” as mutable state at all — read it from `HttpContext.User`.

---

## 5) DI rules you must not break (practical, not academic)

### Rule #1: Don’t inject Scoped into Singleton (directly)
**Bad:**
```csharp
builder.Services.AddScoped<MyDbContext>();
builder.Services.AddSingleton<MyService>(); // depends on MyDbContext -> WRONG
```

Why it’s wrong:
- Singleton lives forever
- Scoped lives per request
- You’d end up with a scoped dependency that “leaks” across requests or causes runtime errors.

ASP.NET Core detects many of these and throws:
> Cannot consume scoped service 'X' from singleton 'Y'

**Fix options:**
1) Change singleton to scoped.
2) If singleton must remain singleton, inject `IServiceScopeFactory` and create a scope per operation (use carefully).

### Rule #2: DbContext is Scoped (almost always)
- `AddDbContext<T>()` defaults to scoped.
- Don’t make `DbContext` singleton. It’s not thread-safe.

### Rule #3: Avoid service locator anti-pattern (`IServiceProvider` everywhere)
This is when you inject `IServiceProvider` and manually call `GetService()` in random places.

**It hides dependencies and makes bugs harder to see.**

Use it only for:
- advanced factories,
- optional plugins,
- bridging old code.

### Rule #4: Middlewares: constructor injection is effectively singleton-like
Most middleware instances are created once at app start.

So:
- Injecting **scoped** services into middleware **constructor** is wrong.
- Instead, get scoped services inside `InvokeAsync(HttpContext context, RequestDelegate next)` via `context.RequestServices`.

**Correct middleware pattern:**
```csharp
public sealed class MyMiddleware
{
    private readonly RequestDelegate _next;

    public MyMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        // Resolve scoped service per request:
        var db = context.RequestServices.GetRequiredService<MyDbContext>();

        // use db...
        await _next(context);
    }
}
```

---

## 6) Service resolution: what actually happens

### 6.1 “Root provider” vs “request scope”
- App starts → builds a root `IServiceProvider` (global)
- Each HTTP request creates a **scope** (a child provider)
- Scoped services live inside that request scope

### 6.2 Where your controller gets services from
Controllers are created per request and resolved from the **request scope**.
So scoped services behave correctly inside controllers.

### 6.3 Manual scope creation (when you really need it)
**Use case:** background tasks (Hangfire, hosted services), where there is no HTTP request scope.

```csharp
public sealed class Worker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public Worker(IServiceScopeFactory scopeFactory)
        => _scopeFactory = scopeFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MyDbContext>();

            // do work with scoped services safely
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
```

**Coach warning:** Don’t do this inside hot paths if you can avoid it. Creating scopes is not “free”.

---

## 7) How to store per-request shared data safely

### 7.1 Use `HttpContext.Items` for small data passing
```csharp
public static class HttpContextItemKeys
{
    public const string CorrelationId = "CorrelationId";
}

app.Use(async (context, next) =>
{
    context.Items[HttpContextItemKeys.CorrelationId] = Guid.NewGuid().ToString("N");
    await next();
});

app.MapGet("/cid", (HttpContext context) =>
    Results.Ok(context.Items[HttpContextItemKeys.CorrelationId]));
```

**Good for:** correlation id, tiny flags, computed values used by later middleware.

### 7.2 Prefer scoped services for richer per-request state
```csharp
public interface IRequestContext
{
    string CorrelationId { get; set; }
}

public sealed class RequestContext : IRequestContext
{
    public string CorrelationId { get; set; } = "";
}

// registration
builder.Services.AddScoped<IRequestContext, RequestContext>();
```

Then fill it early in middleware, use it later anywhere in the request.

---

## 8) Thread-safety (the real reason “shared variables” explode)

Even scoped services can have concurrency problems if you:
- start parallel tasks inside one request and mutate shared fields
- use `Task.WhenAll` and write to shared lists without locking

### Example: unsafe mutation
```csharp
public sealed class Collector
{
    private readonly List<string> _items = new();

    public void Add(string item) => _items.Add(item); // not thread-safe
}
```

If multiple tasks call `Add` at the same time, it can crash or corrupt.

**Fix options:**
- avoid parallel mutation
- use `ConcurrentBag<T>` / `ConcurrentQueue<T>`
- lock around shared mutation (careful)
- design immutable results and merge

---

## 9) Choosing between Transient vs Scoped (decision table)

| Scenario | Recommended lifetime | Why |
|---|---:|---|
| Uses `DbContext` or repositories | Scoped | must be consistent per request |
| Holds per-request user data | Scoped | isolates per request |
| Stateless helper (pure functions) | Transient | cheap and safe |
| Expensive object but thread-safe and immutable | Singleton | reuse safely |
| Caches data globally (thread-safe) | Singleton | intentionally shared |
| Talks to external API via `HttpClient` | use `AddHttpClient` | manages handlers & lifetime correctly |

**Important:** `HttpClient` itself can be injected from `IHttpClientFactory`. Don’t new it per request.

---

## 10) Common mistakes you should stop doing

### Mistake A: singleton service with mutable fields
- causes cross-request leaks
- produces random user-mixing bugs

### Mistake B: transient DbContext / repository
- too many contexts created
- inconsistent tracking inside request

### Mistake C: resolving scoped service from singleton using root provider
```csharp
// WRONG: root provider resolving scoped
var scoped = rootProvider.GetRequiredService<MyDbContext>();
```
This breaks scoping rules.

### Mistake D: using static to “share” something
`static` is basically a global singleton. If it’s mutable, it’s dangerous.

---

## 11) Quick “correct setup” example (Program.cs)

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// DbContext: Scoped by default
builder.Services.AddDbContext<AppDbContext>(options =>
{
    // options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});

// App services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();

// Stateless helpers
builder.Services.AddTransient<IClock, SystemClock>();

// Global cache (only if thread-safe)
builder.Services.AddSingleton<ICacheKeyFactory, CacheKeyFactory>();

var app = builder.Build();
app.MapControllers();
app.Run();
```

---

## 12) Coach-level conclusion (what to do in your real code)

1) If you ever store request/user-specific data in a field:
   - **Don’t.**
   - Read it from `HttpContext`, pass it as method parameters, or use a scoped request-context object.

2) If your service touches DB or caching per request:
   - default to **Scoped**.

3) Use **Transient** only for stateless services.

4) If you pick **Singleton**, you must prove:
   - it is thread-safe,
   - it does not store request state,
   - and sharing is intentional.

---

## 13) Mini checklist for debugging “shared variable” issues

- [ ] Do I have any `static` mutable fields?
- [ ] Do I have any singleton services with fields that change?
- [ ] Am I injecting scoped into singleton/middleware constructor?
- [ ] Do I start parallel tasks that write to shared collections?
- [ ] Am I caching without user/request keys?
- [ ] Am I resolving services using `IServiceProvider` from the wrong scope?

If you want, paste one real service + its registration lines, and I’ll tell you **exactly** which lifetime you should use and where state is leaking.

