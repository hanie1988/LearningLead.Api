### Interview Whole Background

I built the booking front-end using React + TypeScript.
TypeScript ensured strong typing between backend DTOs and UI components, which reduced runtime errors and improved maintainability.
I used Tailwind to rapidly build responsive UI components without separate CSS files, keeping the code clean and consistent.
The flow included fetching room availability, validating date inputs, rendering dynamic availability cards, and submitting bookings to the API.


“I’m a full-stack .NET developer with about ten years of experience building business applications end to end.

Recently I worked on the Onyxum booking system, where I designed the backend using .NET Core, EF Core, PostgreSQL, clean architecture, caching, logging, and structured APIs. I also built the booking pages in React with TypeScript and Tailwind, including availability components and booking forms.

I also contributed to the Game Management System for the Canadian Olympic Committee, mainly focusing on backend services, API design, and data workflows.

Throughout these projects, I regularly communicated with clients and stakeholders — joining requirement-gathering meetings, clarifying features, and participating in daily stand-ups. We followed an agile approach with sprint planning, backlog refinement, and task management using Jira and Asana.

Earlier in my career, I built production systems across different technologies — starting with Windows applications for payroll, then ASP.NET Web Forms for library automation, and MVC + ExtJS for quantity-surveying applications. This gave me a solid understanding of how the .NET ecosystem evolved and helped me adapt quickly to new frameworks.

I’m also working on my own clean-architecture booking project to keep my skills sharp, focusing on EF Core performance, async programming, validation pipelines, global exception handling, and CI/CD with Docker and GitHub Actions.

Overall, I bring strong .NET experience, modern backend skills, and the ability to work closely with clients to deliver complete solutions.”

---
### WCF
“I haven’t used WCF directly in production, but I’m familiar with how it works in the .NET ecosystem.
WCF is used to expose service operations over SOAP, typically between the UI and backend, using XML-based messages. I understand service contracts, endpoints, bindings, and how SOAP communication works.
I have worked with older .NET technologies like ADO.NET, WinForms, WebForms, and ASP.NET MVC, so I’m comfortable maintaining or enhancing legacy systems that use WCF.
**1️⃣ “Operation”**
In WCF, an operation = a method that you allow other systems to call.
```Csharp
[OperationContract]
Employee GetEmployee(int id);
```
**2️⃣ “Endpoint”**
An endpoint = the network address where this service operation can be called.

Example WCF endpoint:
```Code
http://server:8080/PayrollService
```

The client proxy:
	•	Converts your C# call → SOAP Request XML
	•	Sends it to the endpoint
	•	Receives SOAP Response XML
	•	Converts it back into C# object

You never write XML manually — WCF handles it.

“SOAP is a protocol for exchanging structured messages using XML. Each request and response is wrapped inside a SOAP envelope. It provides strong contracts, built-in security standards, and can run over multiple transports like HTTP or TCP. It’s used mostly in older enterprise systems.”

Protocol = language + rules + structure of communication.
Examples:
	•	HTTP
	•	TCP
	•	SMTP
	•	FTP
	•	SOAP
Each defines how data must be formatted and transmitted.
✅ 2) What is HTTP?

HTTP = HyperText Transfer Protocol

This is the protocol your browser uses when visiting a website.

It’s a request–response protocol:
	1.	Client sends a request:
	•	GET /products
	•	POST /login
	2.	Server returns a response:
	•	200 OK
	•	404 Not Found
	•	JSON / HTML / images

HTTP sits on top of TCP.

Important characteristics:
	•	Stateless (each request is independent)
	•	Human-readable
	•	Uses verbs (GET, POST, PUT, DELETE)
	•	Used by REST APIs

---
### Possible Q&A
1. Tell me about yourself.

2. Explain one of the recent systems you worked on. (Onyxum)

3. What was your role in the Canadian Olympic project?

4. What experience do you have with desktop apps or older .NET technologies?

5. Have you used WCF or WebForms? What did you build?

6. Can you explain how you gather requirements from clients?

7. How do you prioritize work in a sprint?

8. What tools do you use for project management? (Jira, Asana)

9. How do you create a good user interface?

10. What is your experience with SQL Server?

11. What’s the difference between an interface and an abstract class?

12. What is dependency injection? Why is it used?

13. How do you test your code?

(E2E validation, unit testing, manual tests)

14. Can you explain async/await in simple terms?

15. How do you handle logs and errors in .NET?

16. What’s your experience with Git?

17. Tell me about a time you supported a customer or fixed a critical bug.

18. Are you comfortable working with older and newer technologies?

19. Are you able to work in office (Vaughan)?

20. Why do you want to join our company?

---
Q2:
“One of my recent projects was the Onyxum Booking System, which is used to manage hotel reservations, room availability, and customer bookings.

I worked mainly on the backend, designing APIs with .NET Core, EF Core, and PostgreSQL in a clean architecture structure. I implemented features like availability search, room management, validation rules, logging, and async operations for better performance.

On the front-end, I built the booking pages using React, TypeScript, and Tailwind — including forms, availability components, and API integration. TypeScript helped make the UI more predictable and easier to maintain.

I collaborated closely with the product team and joined daily meetings to clarify requirements, prioritize tasks, and deliver features in small increments. We used Jira for task tracking and worked in agile sprints.

Overall, my role covered full-stack development, requirement clarification, and delivering end-to-end booking functionality.”

---

Q3:
“I contributed to the Game Management System for the Canadian Olympic Committee, which handles athlete data, event scheduling, and operational workflows during competitions.

My role was mainly on the backend side, working with .NET, C#, Web APIs, and SQL Server to implement new features and improve existing modules. I worked on data models, validation rules, and API endpoints that supported the front-end and internal tools.

I also participated in requirement discussions, clarifying how features should behave, and collaborated with QA and front-end developers to ensure the system behaved correctly end to end.

We followed an agile process with daily stand-ups and sprint planning, using Asana to manage tasks and track progress.

Overall, my focus was delivering reliable backend functionality, improving data flow, and making sure the system was stable and ready for operational use during major events.”

---
Q4:
“Earlier in my career, I developed a payroll management system as a Windows desktop application using ADO.NET.
It required working directly with SQL Server, where I wrote several complex stored procedures, handled calculations, and optimized queries for performance.
That experience gave me a solid understanding of Windows applications, and older .NET technologies, which helps me maintain or enhance legacy systems confidently.”

---
Q6:
“When working with clients or stakeholders, I usually start by listening carefully to understand the business problem, not just the technical request.

I ask clarification questions, confirm edge cases, and break the requirement into small, clear features. I summarize my understanding back to the client to make sure we’re aligned.

During development, I keep communication open through short update meetings or messages to avoid misunderstandings.

We used tools like Asana to document requirements, track tasks, and prioritize them in sprints.

This approach helps me deliver exactly what the client needs and avoid rework.”
---
“I prioritize work by balancing business value, technical effort, and dependencies.

At the start of each sprint, we reviewed the backlog in Asana, clarified requirements, and estimated tasks. The most important items — customer-facing features, urgent fixes, or work that unblocks other team members — were pulled first.

I also make sure tasks are small and well-defined so they can be completed within the sprint.

During the sprint, I communicate early if a task needs clarification or if priorities shift, so the team stays aligned and delivery stays on track.”

---
Q8:
“We mainly used Asana for task management, sprint planning, and tracking progress.
Each feature or bug was created as a ticket with clear requirements, acceptance criteria, and estimates.

We reviewed tasks during sprint planning, assigned priorities, and used daily stand-ups to update status and remove blockers.

Alongside Asana, we used Git for version control and pull requests, which helped us review code and keep the project organized.

This combination kept the workflow structured and made collaboration with developers, QA, and product managers very smooth.”

---
“For me, a good user interface is one that is simple, predictable, and easy to navigate, no matter whether it’s a desktop or web application.

I start by understanding the user’s workflow — what tasks they perform most often, what information they need quickly, and where errors might occur. That helps me design screens that are clean and not overloaded.

On the web side, I use React, TypeScript, and Tailwind to build consistent layouts, clear forms, and components that behave the same way across the system.

For older .NET client applications, I focus on logical grouping of controls, consistent styling, responsive feedback, and making sure the behavior matches user expectations.

I always validate with the team or stakeholders to confirm the UI makes sense before finalizing it.

Overall, good UI comes from understanding the user’s needs and keeping the interface clear, fast, and consistent.”


