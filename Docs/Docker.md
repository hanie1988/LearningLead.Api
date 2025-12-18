### ⭐ THE REAL ENTERPRISE FLOW FOR A DOCKER-BASED BOOKING SYSTEM

**this booking system will pass through 4 stages:**
	1.	Local Development
	2.	CI (Continuous Integration)
	3.	CD (Continuous Deployment) => Image Build + Push to Registry

Each stage uses Docker differently.
🟦 1. LOCAL DEVELOPMENT (your laptop)

What you use:

✔ Dockerfile

✔ docker-compose.yml

✔ raw code on your machine (VS, Rider, etc.)

How it works:
	•	You write code locally in your editor (NOT inside Docker).
	•	You use Docker Compose to start all dependencies:

```
postgres
redis
pgadmin
email service (optional)
hangfire (inside API)
```
Example docker-compose for dev:
```yaml
version: '3.9'
services:
  api:
    build:
      context: .
      dockerfile: Src/Api/Dockerfile
    ports:
      - "8080:8080"
    depends_on:
      - postgres
      - redis
  postgres:
    image: postgres:16
    environment:
      POSTGRES_PASSWORD: postgres
      POSTGRES_USER: postgres
      POSTGRES_DB: booking
  redis:
    image: redis:alpine
```
👉 docker-compose is only for local development
Not for production.

👉 You rebuild API when needed:
```
docker compose up --build
```
👉 Benefits:
	•	No need to install PostgreSQL locally
	•	No need to install Redis locally
	•	Predictable dev environment
	•	Microservices can run together

🟦 2. CI (Continuous Integration)

What runs here:

✔ dotnet restore

✔ dotnet build

✔ dotnet test

✔ EF migrations (if testing with real DB)

✔ Docker “service” containers for Postgres**

What does NOT happen here:

❌ CI does NOT deploy
❌ CI does NOT run docker-compose
❌ CI does NOT publish
❌ CI does NOT run your Dockerfile

Example CI Flow:
	1.	Checkout code
	2.	Setup .NET
	3.	Start PostgreSQL Docker container
	4.	Apply migrations
	5.	Run unit tests
	6.	Run integration tests
	7.	Generate coverage
	8.	Stop the container

Important:

This CI does NOT use your Dockerfile.
It uses normal .NET commands.

Because CI is for testing code, not building images.

🟦 3. IMAGE BUILD + PUSH TO REGISTRY (production build stage)

This is where your Dockerfile becomes critical.

This step does:
	1.	Reads your Dockerfile
	2.	Builds your production image
	3.	Tags it
	4.	Pushes it to registry (GHCR, Azure ACR, AWS ECR)

Example:
```
ghcr.io/hanie1988/learninglead-api:latest
ghcr.io/hanie1988/learninglead-api:v42
```

What the Dockerfile does in enterprise:
	•	dotnet restore
	•	dotnet build
	•	dotnet publish
	•	Create a clean runtime image
	•	Expose ports
	•	Prepare final container for running in cloud

Dockerfile is ONLY used to build your production container image.

In GitHub Actions:
```yaml
- name: Build & Push Image
  uses: docker/build-push-action@v5
  with:
    context: .
    file: Src/Api/Dockerfile
    push: true
    tags: |
      ghcr.io/hanie1988/learninglead-api:latest
```

🟦 4. CD (Continuous Deployment)

Enterprise deployment does NOT take code files.
It does NOT run “dotnet publish” on the server.

CD tells Azure:

“Here is the image. Pull it and run it.”

Nothing else.

CD Flow:
	1.	GitHub Actions notifies Azure
	2.	Azure pulls your latest image from registry
	3.	Azure stops old container
	4.	Azure runs new container
	5.	Azure injects environment variables
	6.	Azure restarts on failure
	7.	Azure logs everything

Example YAML:
```yaml
- name: Deploy to Azure
  uses: azure/webapps-deploy@v2
  with:
    app-name: learninglead-api
    images: ghcr.io/hanie1988/learninglead-api:latest
```

Azure does NOT read:
	•	your Dockerfile
	•	your code
	•	your DLLs
	•	your folder structure

Azure uses ONLY the built image.

⭐ THE REAL ENTERPRISE TRUTH (memorize this):

✔ Dockerfile = Build artifact

✔ docker-compose = Local environment only

✔ CI = test code

✔ Image push = prepare for deployment

✔ CD = deploy container image to Azure

### ⭐ There are ALWAYS 2 pipelines in a real system

✔ CI pipeline

ci.yml → Test code, run unit/integration tests, build code, restore, etc.
❌ CI does NOT deploy
❌ CI does NOT push images

✔ CD pipeline

cd.yml → Push image to registry + deploy to Azure

You NEVER mix these inside one file unless you are doing a small hobby project.

Enterprise = 2 files.

### 📁 Folder structure in GitHub should be:
```
.github/workflows/
   ci.yml
   cd.yml
```

---

### ⭐ PHASE 1 SUMMARY — The Final Picture

🔵 Container = Runtime environment

Runs your app anywhere, identically.

🟦 Image = Template

Created from Dockerfile.

🟧 Dockerfile = Build instructions

Used by CI/CD to create the image.

🟪 Compose = Local environment orchestrator

Used ONLY for local dev, not deployment.

🟥 Registry = Storage for images

Azure pulls your image from here.

🟨 CI = Testing

NOT using Dockerfile.

🟩 CD = Deployment

Uses Dockerfile → builds image → pushes to registry → Azure runs image.