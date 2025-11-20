### What is Navigation property?

A navigation property lets EF Core load related entities.
In Reservation, Room represents the Room object connected by the RoomId foreign key.
With .Include(r => r.Room), EF loads the full Room details.