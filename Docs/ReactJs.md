### 🔷 PART 1 — React Components & Props/State 

**Q1: What is a React component?**
Short answer (interview-ready):

A React component is a reusable UI function. It receives input (props) and returns JSX that describes what should appear on the screen.
**What they want to hear:**
You know it’s a function, reusable, and returns UI.

---
**Q2: What is the difference between Props and State?**

Short answer:
- Props = data passed from parent to child.
    (Read-only, cannot change inside the component)
- State = internal data owned by the component.
    (Can change using setState, triggers re-render)

---
**Q3: When do you use useState?**

Short answer:

When the component needs to store and update data that changes over time — like form inputs, loading states, or counters.

---
**Q4: What does useEffect do?**

Short answer:

useEffect runs side effects — like fetching data, subscribing to events, or reacting to state changes.

---
**Q: Explain the difference between props and state in your own words.**
“We can call components inside other components like HTML tags.
The values we pass to them are called props — they’re read-only inputs from parent to child.
Inside a component, we use state to store data that changes over time, and when state updates, React re-renders the UI.”

---
**State =** internal, mutable, triggers re-render

---
### 🔷 PART 2 — Fetching Availability (API + useEffect + useState)
✅ Q1: How do you fetch data from a backend in React?
Interview answer:

“I fetch data using useEffect so it runs on component mount, and I store the response in state so the UI re-renders with new data.”

---
✅ Q2: Why do we use useEffect for API calls?
“Because fetching is a side-effect. useEffect lets us run that logic when the component loads or when dependencies change.”

---
✅ Q3: Why do we store the result in useState?
“React uses state changes to update the UI. When the availability list arrives, I put it in state so the UI displays the new data.”

---
### 🔷 Real Example — Hotel Availability Component
Step 1 — TypeScript model
```ts
export interface RoomAvailability {
  roomId: number;
  roomNumber: string;
  pricePerNight: number;
  isAvailable: boolean;
}
```
Step 2 — API helper
```ts
export async function getAvailability(
  hotelId: number,
  start: string,
  end: string
): Promise<RoomAvailability[]> {
  const res = await fetch(
    `/api/rooms/availability?hotelId=${hotelId}&start=${start}&end=${end}`
  );

  if (!res.ok) throw new Error("Failed to fetch availability");

  return res.json();
}
```

---
Step 3 — React component
```tsx
import { useEffect, useState } from "react";
import { getAvailability } from "../api/getAvailability";
import type { RoomAvailability } from "../types/RoomAvailability";

export default function AvailabilityList() {
  const [rooms, setRooms] = useState<RoomAvailability[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getAvailability(10, "2025-12-01", "2025-12-05")
      .then(setRooms)
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <p>Loading…</p>;

  return (
    <div className="space-y-4">
      {rooms.map(r => (
        <div key={r.roomId} className="border p-4 rounded shadow">
          <p className="font-bold">Room {r.roomNumber}</p>
          <p>${r.pricePerNight} per night</p>
          <p className={r.isAvailable ? "text-green-600" : "text-red-600"}>
            {r.isAvailable ? "Available" : "Unavailable"}
          </p>
        </div>
      ))}
    </div>
  );
}
```

---
“For availability, I used a clean flow:
useEffect triggers the API call, TypeScript enforces response shapes, and useState stores the results so the UI re-renders. This keeps the component predictable and easy to maintain.”

---
Q: Why should API calls NOT be placed directly in the component body (outside of useEffect)?

❌ WRONG: YOU SAID: “we should fetch data in it so we can refresh data each time on component loads or rerender when a state updated”, ❌ Careful — putting wrong state as a dependency would cause infinite loops if you update state after fetch.

“If I put the API call in the component body, it will run on every render.
useEffect lets me run it only once on mount or when dependencies(Right ones) change.
This prevents infinite loops and makes the component predictable.”

---
### 🔷 PART 3 — React Hook Form 
✅ Q1: What is React Hook Form?
“React Hook Form is a lightweight library for managing form state, validation, and submission. It avoids unnecessary re-renders and makes forms cleaner than using multiple useState calls.”

---
✅ Q2: Why not use useState for every input?
“Using useState for every field creates a lot of re-renders. React Hook Form keeps form state inside refs, so the component stays fast.”

---
✅ Q3: How do you register inputs with React Hook Form?
“I use the register function to connect each input to the form state. On submit, handleSubmit gives me all validated values.”

---
### 🔷 Minimal Example — Booking Request Form

“I used React Hook Form to build the booking flow because it’s fast, avoids unnecessary re-renders, and integrates cleanly with TypeScript. The register API allowed me to define each input, and handleSubmit gave me validated form data to send to my .NET backend.”

How do you type forms?
Using a TypeScript interface + generic on useForm.

