### Kestrel is the built-in web server inside every ASP.NET Core app.

A small program that listens on a port (like 5000 or 5001) and waits for HTTP requests.

It replaces old servers like IIS for local development and production.

A web server is a program that:
	1.	Opens a port (like 80, 443, 5000…)
	2.	Listens for incoming HTTP requests
	3.	Sends back HTTP responses

Think of a port like an apartment number.

```Csharp
http://localhost:5000
localhost = your computer  
5000 = port (a specific entry door)
```

When Kestrel runs, it says:

“I will open port 5000 and wait for people to talk to me through it.”

```Csharp
http://localhost:5000/api/hotels
```

Example:
```Csharp
GET /api/hotels
```

Flow:
	1.	Browser sends request → computer network
	2.	Request arrives at port 5000
	3.	Kestrel sees it and handles it
	4.	Kestrel passes it to ASP.NET Core middleware
	5.	Middleware → Authentication, Authorization
	6.	Routing picks correct controller
	7.	Controller returns a response
	8.	Kestrel sends the response back to the browser

### ⭐ Why do we need Kestrel?

Because:
	•	It understands HTTP
	•	It can handle thousands of requests
	•	It’s fast
	•	It’s cross-platform
	•	It works on Windows, Linux, macOS

### ⭐ In one sentence:

**Kestrel is the program inside your project that opens a port and listens for HTTP requests so your controllers can run**