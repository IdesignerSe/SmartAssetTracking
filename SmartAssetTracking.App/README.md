📘 Smart Asset Tracking System
Ett komplett .NET Console-projekt för att hantera företagsinventarier globalt.
Projektet följer kraven från skoluppgiften och är uppdelat i tydliga nivåer (Level 1–5).

🚀 Features (Sammanfattning)
Systemet stödjer:

Asset‑registrering (Laptop, Desktop, iPhone, Samsung, Nokia, Tablet)

Full CRUD (Create, Read, Update, Delete)

Asset lifecycle (3 år, YELLOW < 3 mån, RED < 6 mån)

Office‑hantering (Sweden, USA, Germany, Turkey)

Valutaomvandling (USD → lokal valuta)

Rapporter (värde per office, expiring assets, dyraste assets)

Avancerad sökning & filtrering

Export (TXT, CSV, JSON)

Roller (Admin, Manager, Employee)

Login-simulering (admin / 1234)

Asset assignment till employees

Maintenance tracking

Dashboard-statistik

📁 Projektstruktur (GitHub‑redo)

```text

SmartAssetTracking/
│
├── SmartAssetTracking.sln
│
├── /Data
│   ├── AssetDbContext.cs
│   ├── SeedData.cs
│   └── Migrations/
│
├── /Models
│   ├── Asset.cs
│   ├── ComputerAsset.cs
│   ├── MobileAsset.cs
│   ├── Office.cs
│   ├── Employee.cs
│   ├── MaintenanceRecord.cs
│
├── /Repositories
│   ├── IAssetRepository.cs
│   ├── AssetRepository.cs
│   ├── IEmployeeRepository.cs
│   ├── EmployeeRepository.cs
│   └── OfficeRepository.cs
│
├── /Services
│   ├── AssetService.cs
│   ├── OfficeService.cs
│   ├── EmployeeService.cs
│   ├── CurrencyService.cs
│   └── ReportService.cs
│
├── /Helpers
│   ├── InputValidator.cs
│   ├── ConsoleMenu.cs
│   ├── CurrencyConverter.cs
│   ├── AssetStatusCalculator.cs
│   └── Exporter.cs   // TXT, CSV, JSON
│
├── /Reports
│   ├── OfficeReport.cs
│   ├── AssetReport.cs
│   └── ExportTemplates/
│
├── /Auth
│   ├── LoginService.cs
│   └── Roles.cs
│
├── Program.cs
└── README.md

``` 

🏗️ Arkitektur
✔ Clean Architecture
Models = datatyper

Repositories = datalager (EF Core)

Services = logik + regler

Helpers = små verktyg (validering, konvertering, status)

Reports = rapportgenerering

Auth = roller + login

🗄️ Databas (EF Core)
SQL Server eller SQLite

Migrations

Relations:

Office → Assets (1‑många)

Employee → Assets (1‑många)

Asset → MaintenanceRecords (1‑många)

🧮 Asset Lifecycle Rules
Från dokumentet:

“Asset lifetime = 3 years… YELLOW < 3 months… RED < 6 months”

Systemet beräknar automatiskt:

Remaining lifetime

Status (NORMAL / YELLOW / RED)

🌍 Office & Currency
Från dokumentet:

“Convert prices into local currency… Use exchange rates”

Stöd för:

SEK

USD

EUR

TRY

🔍 Search & Filtering
Search by brand

Search by model

Search by office

Search by purchase year

Filter: expired, computers, mobile devices, office‑specific

📤 Export
TXT

CSV

JSON

🔐 Roles & Login
Admin

Manager

Employee

Login-simulering:

“Username: admin, Password: 1234”

📊 Dashboard
Total assets

Total office value

Expiring assets

Assets per employee

Most used asset type

Most expensive office

🧪 Bonus (Optional)
REST API

Blazor/MVC UI

Docker

Unit tests