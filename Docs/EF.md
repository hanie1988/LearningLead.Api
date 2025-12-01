	1.	Level 0 — I only know SaveChanges + DbSet basics.
	2.	Level 1 — I know CRUD, migrations, relationships.
	3.	Level 2 — I understand IQueryable, Include, tracking vs no-tracking.
	4.	Level 3 — I know transactions, concurrency, performance tuning.
	5.	Level 4 — I can design large models cleanly, write tests, optimize SQL.

1) Change Tracking & Performance

| Concept            | You must be able to answer                                      |
|-------------------|------------------------------------------------------------------|
| ChangeTracker     | What it stores + why it hurts on large queries                   |
| AsNoTracking()    | When to use for read-only endpoints                              |
| TrackGraph        | When updating graphs of related entities                         |
| DetectChanges cost| Why bulk reads slow down                                         |
---
2) Relationships in Depth

✔ Many-to-many
✔ Shadow properties
✔ Owned types
✔ Cascade rules (Restrict/SetNull/Cascade)

---
3) Transactions & Concurrency

✔ DbContextTransaction
✔ TransactionScope
✔ RowVersion & Concurrency Tokens
✔ Optimistic vs Pessimistic locking

---

4) IQueryable + Efficient Query Design

	•	Expression trees
	•	Deferred execution
	•	When to break IQueryable into IEnumerable
	•	Why projection (Select) is better than returned entity

---

📍 LESSON 1 — How EF Change Tracking Works

For each entity:
| Item              | What EF remembers                                                   |
|------------------|---------------------------------------------------------------------|
| Original values   | What the values were when the entity was first loaded               |
| Current values    | The values currently in memory                                      |
| State             | Added / Modified / Deleted / Unchanged                              |
| Navigation links  | Relations to other entities                                         |

Everything is stored inside:
```
DbContext.ChangeTracker.Entries()
```

🔄 What happens when you modify a tracked entity?

Example:
```Csharp
var room = await db.Rooms.FindAsync(1);  // tracked
room.PricePerNight = 199;                // change property
await db.SaveChangesAsync();
```
Behind the scenes:
	1.	Room is loaded + tracked
	2.	EF compares original and current values
	3.	It sees PricePerNight changed → sets State = Modified
	4.	When you call SaveChanges():
	•	EF generates SQL like:

```Csharp
UPDATE Rooms SET PricePerNight = 199 WHERE Id = 1
```

---
### What is Navigation property?

A navigation property lets EF Core load related entities.
In Reservation, Room represents the Room object connected by the RoomId foreign key.
With .Include(r => r.Room), EF loads the full Room details.