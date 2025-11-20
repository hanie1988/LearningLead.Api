“Controller receives the request, validates input, calls the service, and returns an HTTP response. No business logic.”


⭐ IActionResult vs ActionResult

🔵 1. IActionResult → “I can return ANY type of HTTP response.”

When your method returns:
```Csharp
IActionResult
```

It means:

“I will return different kinds of HTTP responses (Ok, BadRequest, NotFound, etc.),
and the compiler shouldn’t care about the type.”

```Csharp
[HttpGet("{id}")]
public IActionResult Get(int id)
{
    if (id <= 0)
        return BadRequest("Invalid id");

    return Ok(new { Message = "Success" });
}
```
Here you can return:
	•	Ok()
	•	BadRequest()
	•	NotFound()
	•	NoContent()
	•	Created()

---

🔵 2. ActionResult → “I return a MODEL + also HTTP responses.”

This one is more modern and cleaner.

When you write:
```Csharp
ActionResult<ReservationDto>
```

“IActionResult is untyped and can return any HTTP response.
ActionResult is typed, so it returns a specific model and also supports all HTTP responses.
For APIs that return data, ActionResult is cleaner and better for Swagger.”

🔵 Why your code works

```Csharp
public async Task<IActionResult> Create(ReservationCreateDto dto)
{
    var created = await service.CreateReservationAsync(dto);

    var response = new ReservationResponseDto(
        created.Id,
        created.RoomId,
        created.UserId,
        created.CheckIn,
        created.CheckOut
    );

    return Ok(response);
}
```

This is valid C#.
Ok(response) returns an ObjectResult with the DTO.
No errors.
Everything functions.

⸻

🔴 But it is not the cleanest way for modern APIs (ASP.NET Core 6/7/8/9)

Why?

Because:

1. IActionResult hides your return type

Swagger cannot automatically show the response shape.
The compiler does not know what type you return.

You lose type-safety and documentation clarity.

2. ActionResult<T> tells the API and Swagger EXACTLY what you return

Like this:

```Csharp
public async Task<ActionResult<ReservationResponseDto>> Create(ReservationCreateDto dto)
```
Now:
	•	Swagger shows the schema
	•	API explorer knows the type
	•	Consumers have better documentation
	•	The method is easier to test
	•	The method is self-explanatory
	•	Modern style
	•	Recruiters expect this

---

🔥 Short rule for your mind (very important)
	•	If your endpoint returns a DTO → use ActionResult<T>
	•	If your endpoint returns only HTTP status/no content → use IActionResult

---

[HttpDelete("{id}")]
public async Task<IActionResult> Cancel(int id)
{
    var success = await service.CancelReservationAsync(id);

    if (!success)
        return NotFound("Reservation not found");

    return NoContent();
}

“DELETE normally returns 204 NoContent on success.
If I need to return a confirmation message or a DTO, I return 200 OK with a structured object, not a raw string.”