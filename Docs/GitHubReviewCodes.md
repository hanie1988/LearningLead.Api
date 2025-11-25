✅ 1. Code review happens inside Pull Requests (PRs)

Developers never review code by browsing random files.

Everything is reviewed through a Pull Request.

A PR shows:
	•	all changes compared to main branch
	•	comments
	•	discussions
	•	requested fixes
	•	approvals

This is where the real review happens.

⸻

✅ 2. Workflow developers follow

Step 1 — You create a branch

Example:

```Csharp
feature/reservation-cancellation
bugfix/fix-room-overlap-check
refactor/application-layer-cleanup
```

Step 2 — You write code

Commit in small chunks:
	•	clear messages
	•	not too many files
	•	not too large PRs (teams hate huge PRs)

Step 3 — You push the branch

```
git push origin feature/reservation-cancellation
```

Step 4 — You open a Pull Request

GitHub interface → “Compare & Pull Request”

You write:
	•	What you did
	•	Why you did it
	•	How to test it

⸻

✅ 3. How senior developers review your code

🔍 They look for architecture correctness first

(even before functionality)
	•	Is the logic in the correct layer?
	•	Are DTOs in Application, not in API?
	•	Is EF code in Infrastructure?
	•	Are domain rules inside Entities?

If not → they comment immediately.

⸻

🔍 They inspect the diff (line-by-line)

GitHub shows:

```
+ added lines (green)
- removed lines (red)
```

Reviewers check:
	•	readability
	•	naming
	•	method length
	•	duplicate code
	•	business logic correctness
	•	proper async/await
	•	correct repository usage
	•	correct service boundaries

⸻

🔍 They add inline comments

Example:

❌ Move this validation to Application layer.

❌ This belongs to ReservationService, not controller.

❌ Don’t catch exceptions here. Let middleware handle it.

❌ Use ValueObject for this logic.

❌ Room availability check is wrong — missing edge case.

❌ This should be async all the way.

⸻

🔍 They request changes

This sets the PR status to:
You must update the code and push again.

⸻

🔍 They approve

Once reviewers are satisfied, they mark:
```
Approved
```

Then the PR can be merged.

⸻

🧠 4. What developers NEVER do

They NEVER:
	•	read your entire repo randomly
	•	review code in the main branch
	•	give feedback on untracked files
	•	read code outside the PR diff

They ONLY work with Pull Requests, because PRs show exactly what changed.

⸻

🔥 5. What they check most (important for you)

✔ Architecture placement (your biggest weakness)
	•	Domain vs Application vs Infrastructure
	•	No EF in Application
	•	No business logic in API
	•	No DTOs in Domain
	•	No services inside Core
	•	No controllers in Application
	•	No repository implementation inside Core

✔ Clean code
	•	method size
	•	naming
	•	SRP
	•	async correctness
	•	Result usage
	•	exception handling boundaries

✔ Tests

If you add a feature without tests → flagged.

⸻

📌 6. This is how YOU should review code on GitHub too

When you start a mid-senior role in Canada, this is exactly what is expected.

You will:
	1.	Open the PR
	2.	Check the diff
	3.	Focus on architecture boundaries
	4.	Check business logic correctness
	5.	Suggest improvements
	6.	Approve or request changes

You can also pull the PR branch locally to run/test it.

⸻

🎯 If you want, I can teach you:
	•	how to create the perfect Pull Request
	•	how to review someone else’s PR
	•	how to respond to review comments like a senior
	•	how GitHub teams structure their workflows
	•	how to write better, shorter commits
	•	how to structure your branches properly