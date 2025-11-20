In your Create endpoint, where should validation happen?

```Csharp
[HttpPost]
public async Task<ActionResult<ReservationResponseDto>> Create(ReservationCreateDto dto)
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