	•	What goes inside a JWT?
	•	How do you prevent token tampering?
	•	Difference between access token & refresh token?
	•	How long should tokens be valid?
	•	Why password hashing matters?
	•	Why we never store plaintext passwords?
    KeyCloak
    SML

### 1) Security

Hashing, token lifetime, validation, middleware.

### 2) Architecture

UserService → AuthService → Controllers → JWT middleware.

### 3) Real experience

You can talk about authentication flows, roles, tokens, expiration, etc.

### ⭐ Why Do We Need a User Table + Login Flow?

Because ANY real application — booking, ticketing, e-commerce, social media — must know:
	•	Who is the user?
	•	Are they allowed to do this action?
	•	What is their identity?
	•	What is their role? (Admin/Customer/Manager)

### ⭐ Why do we need Login Flow?
1️⃣ The user sends email + password
2️⃣ The API checks the User table
3️⃣ If valid → API creates a JWT token
4️⃣ The token is returned
5️⃣ The frontend stores the token
6️⃣ Every future request includes

### ⭐ Login Is NOT “front side authentication”

**✔ CORRECT:**

The backend is ALWAYS responsible for authentication.
Frontend only sends a request.

### 🔐 Login + JWT Flow (Full Overview)

**1. Frontend → sends email + password**
```Http
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "Secret123!"
}
```
**2. Backend → verifies user, generates JWT**
```HTTP 
200 { "token": "...jwt..." }
```
**3. Frontend → stores token**
```HTTP 
localStorage or HttpOnly Cookie
```

**4. Frontend → uses token in all requests**
```HTTP 
GET /api/hotels
Authorization: Bearer <token>
```

Token generation must always be done in the backend because the secret key and validation logic must stay in a trusted environment. The frontend is untrusted, can be modified by users, and cannot securely store secrets. If the frontend generates tokens, users can forge their identity, roles, or permissions. Therefore, authentication, role assignment, and token signing must always be handled server-side.

### 🔐 Why we need Login + Token (The Story Version)

Imagine a user wants to book a hotel on your platform (Onyxum).
Before the user can see hotel details, or create bookings, we must know:
	•	Who is this person?
	•	Are they authenticated?
	•	Do they have permission?

That’s why we need:
	•	A User table → to store email + hashed password
	•	A Login flow → to validate the user
	•	A Token (JWT) → to identify them on every request afterward

This is the standard system used by React, Next.js, mobile apps, and APIs.

---

**1️⃣ Frontend → sends email + password**

The user presses “Login” on the frontend.

The browser sends:

```Http
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "Secret123!"
}
```
At this point:
🔥 There is no token yet. Only raw credentials.

---

**2️⃣ Backend → verifies user, generates JWT**

In ASP.NET:
	1.	Find the user in the database
	2.	Verify the password
	3.	If correct → generate a JWT token

What is a JWT?

It’s just a long string like this:

```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9
.eyJzdWIiOiIxMjMiLCJlbWFpbCI6InVzZXJAZXhhbXBsZS5jb20iLCJyb2xlIjoiQ3VzdG9tZXIifQ
.p2A6h3GZq7O6NqN6Z...
```

A JWT has 3 parts:
### Header(public) → algorithm(The algorithm used to sign the token)
```
    {
        "alg": "HS256",
        "typ": "JWT"
    }
```
**Example:**
	•	HS256 = HMAC SHA-256 (shared secret key)
	•	RS256 = RSA (public/private key pair)
	•	typ: The type → always JWT
### Payload(public) → user data
**❗ Important note:**
	•	Payload is Base64-encoded, not encrypted
	•	Anyone can decode it
	•	It is NOT secure data
	•	DO NOT put passwords or sensitive information inside

Example payload:
```Json
{
  "sub": "123",	sub → subject → most commonly user ID
  "email": "user@example.com",
  "role": "Customer",
  "exp": 1731250000
}
```
It’s like a digital ID card, signed by the server.

### Signature(public) → prevents hacking
This is the most important part.

How signature is created:
```Csharp
Signature = HMACSHA256(
   base64(header) + "." + base64(payload),
   secret_key
)
```
**if signature is public, can hacker reverse it to find the secret key**
No — because HMACSHA256 is one-way cryptography.

If a hacker changes anything inside the token:
The signature will no longer match.

---

**3️⃣ Backend → returns the token**
Two options:

✳️ Option 1: Return token in JSON (for SPA apps like React)

```
HTTP 200 OK
Content-Type: application/json

{
  "token": "<jwt_here>"
}
```
Frontend reads it and stores it.


✳️ Option 2: Put token inside an HttpOnly Cookie (more secure)
```
HTTP 200 OK
Set-Cookie: auth=<jwt_here>; HttpOnly; Secure; SameSite=Strict; Path=/
```
The browser stores the cookie automatically.

JavaScript cannot read HttpOnly cookies = safer.

---

**4️⃣ Where does the frontend store the token?**

Two very common options:

⸻

**🧊 Option A — localStorage (easy but less secure)**

```
localStorage.setItem("access_token", token);
```
Then frontend adds it to every request:
```
const token = localStorage.getItem("access_token");

fetch("/api/hotels", {
  headers: {
    Authorization: `Bearer ${token}`
  }
});
```
Problem:
If you ever have an XSS vulnerability, the attacker can read the token.

⸻

**🍪 Option B — HttpOnly Cookie (recommended)**

Server sets:
```Csharp
Response.Cookies.Append(
    "auth",
    jwtToken,
    new CookieOptions
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = DateTimeOffset.UtcNow.AddHours(1)
    });
```

Browser automatically includes the cookie:
```
GET /api/hotels
Cookie: auth=<jwt_here>
```
JavaScript can’t read the token → safe from XSS.
But you must protect CSRF → using SameSite or Anti-CSRF token.

---

**5️⃣ How backend uses the token in next requests**

When request comes: 
```
GET /api/hotels
Authorization: Bearer <token>
```

or via Cookie:
```
Cookie: auth=<token>
```

ASP.NET validates the token automatically if you configure:
```Csharp
builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            // issuer, audience, key...
        };
    });
```

Controller:

```Csharp
[Authorize]
[HttpGet]
public async Task<IActionResult> GetAll()
{
    // Only executed if token is valid
}
```

---

**6️⃣ FINAL SUMMARY (Super Simple)**

	•	Token = card ID created by backend
	•	Frontend NEVER creates the token
	•	Backend creates, signs, and validates it
	•	Store token either:
	•	in localStorage → simple but vulnerable
	•	in HttpOnly cookie → secure recommended
	•	Every protected API requires:

```csharp
    Authorization: Bearer <token>
```
    or cookie.

---

### ⚠️ Common Mistakes Developers Make
❌ Thinking JWT payload is encrypted
→ It is not encrypted, only Base64.

❌ Putting passwords in JWT
→ Never do this.

❌ Returning tokens without expiration
→ Always include exp.

❌ Storing JWT in localStorage without protection
→ Vulnerable to XSS (but ok for interviews/projects if you know the risk).

### ✔️ A secret the hacker does NOT have.

**This secret key is stored ONLY on the backend:**
	•	In Azure Key Vault
	•	In appsettings.json (dev)
	•	In environment variables (prod)

```Csharp
    public sealed class TokenService(IConfiguration config) : ITokenService
    {
        public string CreateToken(User user)
        {
            var jwtSection = config.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSection["Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
```

```Json
    "Jwt": {
        "Key": "THIS_SHOULD_BE_LONG_AND_SECRET_CHANGE_ME",
        "Issuer": "LearningLead.Api",
        "Audience": "LearningLead.Api"
    }
```

```Csharp
    // JWT authentication
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var jwtSection = config.GetSection("Jwt");
            var keyBytes = Encoding.UTF8.GetBytes(jwtSection["Key"]!);

            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSection["Issuer"],
                ValidAudience = jwtSection["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
            };

            // 👇 This allows reading token from HttpOnly cookie later
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    // If Authorization header is empty, try cookie "auth"
                    if (string.IsNullOrEmpty(context.Token))
                    {
                        if (context.Request.Cookies.TryGetValue("auth", out var cookieToken))
                        {
                            context.Token = cookieToken;
                        }
                    }

                    return Task.CompletedTask;
                }
            };
        });

    builder.Services.AddAuthorization();

    var app = builder.Build();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();
```

**Login with JSON token (for localStorage)**

```Csharp
    [ApiController]
    [Route("api/auth")]
    public sealed class AuthController(
        IUserRepository users,
        ITokenService tokenService) : ControllerBase
    {
        // POST /api/auth/login-json
        [HttpPost("login-json")]
        public async Task<IActionResult> LoginJson(LoginRequestDto dto)
        {
            var user = await users.GetByEmailAsync(dto.Email);
            if (user is null)
            {
                return Unauthorized("Invalid credentials.");
            }

            // ❗ Replace this with proper password hashing check (BCrypt, etc.)
            var passwordIsValid = dto.Password == "Temp123!" || dto.Password == user.PasswordHash;
            if (!passwordIsValid)
            {
                return Unauthorized("Invalid credentials.");
            }

            var jwt = tokenService.CreateToken(user);

            return Ok(new LoginResponseDto(jwt));
        }
    }
```
**Frontend (React) – localStorage flow**

```Csharp
    // login.ts
    async function login(email: string, password: string) {
    const res = await fetch("http://localhost:5000/api/auth/login-json", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
    });

    if (!res.ok) throw new Error("Login failed");

    const data = await res.json(); // { token: "..." }
    localStorage.setItem("access_token", data.token);
    }

    async function getHotels() {
    const token = localStorage.getItem("access_token");
    const res = await fetch("http://localhost:5000/api/hotels", {
        headers: {
        Authorization: `Bearer ${token}`,
        },
    });

    return await res.json();
    }
```

**3️⃣ Login with HttpOnly cookie**

```Csharp
// still in AuthController

// POST /api/auth/login-cookie
[HttpPost("login-cookie")]
public async Task<IActionResult> LoginCookie(LoginRequestDto dto)
{
    var user = await users.GetByEmailAsync(dto.Email);
    if (user is null)
    {
        return Unauthorized("Invalid credentials.");
    }

    // ❗ Replace with secure hash check
    var passwordIsValid = dto.Password == "Temp123!" || dto.Password == user.PasswordHash;
    if (!passwordIsValid)
    {
        return Unauthorized("Invalid credentials.");
    }

    var jwt = tokenService.CreateToken(user);

    Response.Cookies.Append(
        "auth",
        jwt,
        new CookieOptions
        {
            HttpOnly = true,
            Secure = true,               // true in production (HTTPS)
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(1),
            Path = "/"
        });

    // You can return user info if you want
    return Ok(new { message = "Logged in", user = user.Email });
}
```

**Frontend – HttpOnly cookie flow**
```Csharp
async function loginWithCookie(email: string, password: string) {
  const res = await fetch("http://localhost:5000/api/auth/login-cookie", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email, password }),
    credentials: "include", // 🔥 important: send & receive cookies
  });

  if (!res.ok) throw new Error("Login failed");

  // No token in JS. Cookie is stored by browser automatically.
}
```

