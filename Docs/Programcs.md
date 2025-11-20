**WebApplication is the “brain + heart + lungs” of your API:**
	•	Brain → routes & controllers
	•	Heart → Kestrel server
	•	Lungs → middleware pipeline
	•	Blood vessels → DI container

### ⭐ How WebApplication handles routes & controllers

**✔ Step 1 — You register controllers**

This line tells ASP.NET Core:
```Csharp
builder.Services.AddControllers();
```

“I have controller classes in my project.
Please scan them, understand their routes, and prepare them.”

At this moment, .NET looks for:
	•	classes with [ApiController]
	•	methods with [HttpGet], [HttpPost], etc.
	•	routes like [Route("api/hotels")]

It does NOT activate them yet.
Just registers them inside the routing system.

**✔ Step 2 — You activate routing**

This line:
```Csharp
app.MapControllers();
```

tells WebApplication:

“Take all the controllers you found earlier
and turn them into real HTTP endpoints.”

This creates the final mapping table internally, like:
```Csharp
GET    /api/hotels              → HotelsController.GetAll()
POST   /api/hotels              → HotelsController.Create()
GET    /api/users/{id}          → UsersController.GetById()
POST   /api/users/login         → UsersController.Login()
```

Now the routes are live.
---

✔ builder.Services.AddXyz(...)

= REGISTER something for the application
= happens BEFORE the app runs
= used for Dependency Injection + configuration

✔ app.UseXyz(...)

= EXECUTE something in the request pipeline
= happens WHILE the app is running
= used for middleware (per-request logic)

⭐ 1) What does builder.Services.AddXyz() REALLY do?

It registers services inside Dependency Injection (DI).

⭐ 2) What does app.UseXyz() REALLY do?

It adds middleware to the request pipeline.

Middleware = little functions that run on every single HTTP request.

