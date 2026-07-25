📘 Smart Asset Tracking System
Ett komplett .NET Console-projekt för att hantera företagsinventarier globalt.
Projektet följer kraven från skoluppgiften och är uppdelat i tydliga nivåer (Level 1–5).

How to start the project:
dotnet build

dotnet run

After you see the menu:

=== LOGIN ===                                                                                    
Username: admin                                                                                  
Password: admin  

and then you will able to see the whole Menu:

=== ADMIN MENU ===                                                                               
1. Asset Management                                                                              
2. Employee Management                                                                           
3. Maintenance                                                                                   
4. Dashboard                                                                                     
5. Office Management                                                                             
6. Exit                                                                                          
7. Export                                                                                        
8. Search                                                                                        
9. Mass‑Insert Mode                                                                              
Choose option: 

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

SmartAssetTracking.App/
│
├── /Data
│   ├── AssetDbContext.cs
│   ├── DesignTimeDbContextFactory.cs
│   └── assets.db
│
├── /Migrations
│   ├── initial.cs
│   ├── inheritanceSupport.cs
│   └── assetDBContextModelSnapshot.cs
│
├── /Models
│   ├── Asset.cs
│   ├── ComputerAsset.cs
│   ├── Desktop.cs
│   ├── Employee.cs
│   ├── iPhone.cs7
│   ├── Laptop.cs
│   ├── LifecycleStatus.cs
│   ├── MaintenanceRecord.cs
│   ├── MobileAsset.cs
│   ├── Nokia.cs
│   ├── Samsung.cs
│   ├── Office.cs
│   ├── Table.cs
│   ├── User.cs
│   └── UserRole.cs
│
├── /Services
│   ├── AssetService.cs
│   ├── CurrencyService.cs
│   ├── DashboardService.cs
│   ├── EmployeeService.cs
│   ├── ExportService.cs
│   ├── LoginService.cs
│   ├── MaintananceService.cs
│   └── SearchService.cs
│
├── /Reports
│   └── ExportTemplates/ They will deposited here.
│
├── /UI
│   └── Menu.cs
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
