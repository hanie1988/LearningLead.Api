# 🔥 SQL Interview — Level 1 (ABSOLUTE MUST-KNOW)

> If you don’t know these topics **clearly and confidently**, you will fail most SQL interviews.  
> This is the non-negotiable foundation.

---

## 1️⃣ SQL JOINs  
**INNER, LEFT, RIGHT, FULL**

You must be able to:
- Explain each join in plain language
- Write a correct query from scratch
- Say **when** to use each one

What interviewers expect:
- INNER JOIN → matching rows only  
- LEFT JOIN → all left rows + matching right rows  
- RIGHT JOIN → all right rows + matching left rows  
- FULL JOIN → everything from both sides  

❗ Many candidates *name* joins but can’t **reason about the result set**.

---

## 2️⃣ GROUP BY, HAVING, ORDER BY

You must understand:
- `GROUP BY` → aggregation boundary
- `HAVING` → filter **after aggregation**
- `ORDER BY` → final result sorting

⚠️ Classic interview trap:
**- Confusing `HAVING` with `WHERE`**

	•	**WHERE** → filters rows before aggregation
	•	**HAVING** → filters groups after aggregation

Rule to remember:
- `WHERE` filters rows
- `HAVING` filters groups

---

## 3️⃣ WHERE vs ON vs HAVING

Interviewers **love** this one.

You must clearly explain:
- `WHERE` → filters rows **before grouping**
- `ON` → controls how tables are joined
- `HAVING` → filters aggregated results

If you hesitate here, it’s a red flag.

---

## 4️⃣ Subqueries & Common Table Expressions (CTE)

You **will** be asked about CTEs.

You must know:
- What a CTE is
- Why it improves readability
- How it differs from subqueries
- When a subquery is still better

Expect questions like:
- “Rewrite this subquery using a CTE”
- “Why would you choose a CTE here?”

---

## 5️⃣ Indexes  
**Clustered vs Nonclustered**

You must clearly know:
- What a **clustered index** is
- What a **nonclustered index** is
- Why a table can have only **one** clustered index
- When **NOT** to add an index
- Why indexes **slow down INSERT / UPDATE / DELETE**

❗ Saying “indexes make queries faster” is not enough.  
You must explain the **trade-off**.

---

## 🧠 Coach Rule (Remember This)

> SQL interviews don’t fail people on syntax.  
> They fail people on **thinking and reasoning**.

Interviewers often ask:

> “If performance drops on a table with millions of rows, what do you check first?”

✅ Correct answer:  
**Indexes → Execution Plan → Missing Index hints**

---

### 6. Execution Plans  
You don’t need to be an expert. Just know:
- What a **scan** is
- What a **seek** is
- Why seeks are faster
- How execution plans show missing indexes

---

### 7. Transactions & Isolation Levels  
Know the basics:
- READ COMMITTED
- READ UNCOMMITTED
- REPEATABLE READ
- SERIALIZABLE

And **why dirty reads happen**.

---

### 8. Stored Procedures  
Basic CRUD procedures with parameters.

---

### 9. Views  
- What they are
- When to use them
- When **not** to use them

---

### 10. SQL Functions  
Know the difference between:
- Scalar functions
- Table-valued functions
- Built-in functions (LEN, GETDATE, etc.)

---

### 11. Constraints  
You must know:
- PRIMARY KEY
- FOREIGN KEY
- UNIQUE
- CHECK
- DEFAULT

---

### 12. NULL Handling  
Understand:
- ISNULL
- COALESCE
- NULL behavior in joins

---

## 🔥 LEVEL 2 — EXPECTED FOR INTERMEDIATE DEVELOPER (.NET)

### 13. Pagination  
OFFSET / FETCH

---

### 14. Temporary Tables vs Table Variables  
Know differences in:
- Performance
- Transaction behavior
- Indexing

---

### 15. Window Functions  
Very important.
- ROW_NUMBER
- RANK
- DENSE_RANK
- OVER()

---

### 16. Deadlocks & How to Avoid Them  
Your reservation / booking system knowledge helps here.

---

### 17. SQL Injection  
Know:
- What it is
- How parameterized queries prevent it

---

### 18. ACID Properties  
Very common interview question.

---

### 19. DELETE vs TRUNCATE vs DROP  
Know differences in:
- Logging
- Speed
- Rollback
- Schema impact

---

### 20. Normalization Basics  
Just know:
- 1NF
- 2NF
- 3NF
- Why normalization helps

---

## 🟦 LEVEL 3 — BONUS (Nice to Have)

These make you look senior, but are **not mandatory** for intermediate roles:
- Partitioning
- Index fragmentation (REBUILD vs REORGANIZE)
- In-memory tables
- Query Store
- CROSS APPLY
- Dynamic SQL

---

## 🎯 What You Should Do in the Next 7 Days

Follow this exactly to be interview-ready:

- **Day 1:** Joins + CTE + Subqueries  
- **Day 2:** Indexes + Execution Plans  
- **Day 3:** Window Functions  
- **Day 4:** Transactions + Isolation Levels  
- **Day 5:** Stored Procedures + Functions + Views  
- **Day 6:** Pagination + Temp Tables + Constraints  
- **Day 7:** Practice 20 interview questions  

---