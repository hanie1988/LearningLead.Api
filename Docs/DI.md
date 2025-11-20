### 🔥 How does .NET recognize [FromServices] in method parameters?

ASP.NET Core has **two types of Dependency Injection**:

**✅ 1. Constructor Injection (Common way)**

Example:
```Csharp
public sealed class UserController(IUserService userService) : ControllerBase
{
}
```
This is the standard method.
.NET injects dependencies when creating the controller.

---

**✅ 2. Method Injection (Less common, but built-in)**
Example:
```Csharp
public async Task<IActionResult> Login(UserLoginDto dto, [FromServices] JwtService jwt)
```
Here, the DI container injects JwtService only for that one method call, NOT for the whole class.

---

### 💡 Why does it work?

ASP.NET Core model binding has a rule:

**▶ When it sees [FromServices], it knows:**
	•	“Do not bind this from the request body”
	•	“Do not bind this from query string”
	•	“Do not bind this from route”
	•	“Take this instance from the DI container”

So .NET looks into:

```Csharp
builder.Services.AddScoped<JwtService>();
```

---

### 🔍 Why does .NET even support method injection?

Because sometimes:

**✔ You need a service only in ONE endpoint**

Example: JwtService is used only in Login, not in the whole controller.

**✔ Keeps the class constructor clean**

No need to inject 10 services you’ll only use once.

**✔ Helps with testing**

You can mock per-method easily.

Framework designers wanted flexibility.