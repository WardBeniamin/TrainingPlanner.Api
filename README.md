📘 TrainingPlanner API
📌 Projektbeskrivning

TrainingPlanner är ett backend-API byggt i ASP.NET Core Web API för att hantera träningsrelaterad funktionalitet såsom övningar, träningspass och bokningar.
Projektet är framtaget som en del av en fullstack-uppgift där backend konsumeras av en frontend (React).

API:t följer modern praxis med tydlig struktur, testbar kod och kontinuerlig integration via GitHub Actions.


🏗 Arkitekturöversikt

Projektet är uppbyggt enligt Clean Architecture och är uppdelat i flera lager:

📂 Projektstruktur

TrainingPlanner.Api

Controllers

DTOs

Services

API-endpoints

TrainingPlanner.Domain

Domänmodeller

Affärslogik

TrainingPlanner.Infrastructure

Databaskoppling

Repository-implementationer

TrainingPlanner.Tests

Enhetstester

🎯 Arkitekturprinciper

Separation of Concerns

Dependency Injection

Testbar och underhållbar kod

API-first-approach

▶️ Starta projektet lokalt
🔹 Förutsättningar

.NET 8 SDK

Node.js (för frontend)

Git

Visual Studio eller VS Code

▶️ Starta Backend (API)

Klona repot: git clone https://github.com/WardBeniamin/TrainingPlanner.Api.git

Gå till projektmappen: cd TrainingPlanner.Api

Starta API:

dotnet run --project TrainingPlanner.Api


API:t körs nu på:

https://localhost:3000


Swagger finns på:

https://localhost:3000/swagger

▶️ Starta Frontend (React)

Frontend körs i ett separat projekt

Gå till frontend-mappen:

cd frontend


Installera beroenden:

npm install


Skapa .env:

VITE_API_URL=https://localhost:3000


Starta frontend:

npm run dev

▶️ Databas

Projektet använder lokal databas via Infrastructure-lagret

Connection string konfigureras i:

appsettings.json

🔌 API Endpoints (exempel)
Metod	Endpoint	Beskrivning
GET	/api/exercises	Hämta alla övningar
GET	/api/exercises/{id}	Hämta övning via ID
POST	/api/exercises	Skapa ny övning
PUT	/api/exercises/{id}	Uppdatera övning
DELETE	/api/exercises/{id}	Ta bort övning

👉 Fullständig lista finns i Swagger.

🧪 Tester

Projektet innehåller enhetstester

Tester körs automatiskt via GitHub Actions

CI-pipeline:

dotnet restore

dotnet build

dotnet test

Status: ✅ Godkänd CI

⚠️ Kända buggar / Begränsningar

Ingen autentisering (JWT) implementerad ännu

Felhantering kan förbättras för edge cases

Frontend saknar viss avancerad validering

🔄 CI / GitHub Actions

Projektet använder GitHub Actions för kontinuerlig integration.

Pipeline triggas vid:

push till master

pull request mot master

👤 Författare

Ward Beniamin
Systemutvecklare .NET (student)


