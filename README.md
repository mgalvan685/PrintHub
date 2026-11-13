PrintHub
PrintHub is a 3D printing management platform designed to track printers, filaments, materials, projects, and project costs.

The goal is to provide accurate print costing, inventory management, and project tracking for hobbyists and small print businesses.

Technology Stack
Backend
.NET 8
ASP.NET Core Web API
Entity Framework Core
PostgreSQL
AutoMapper
FluentValidation
Swagger
Frontend
React
TypeScript
Vite
Testing
xUnit
FluentValidation.TestHelper
Features
Printers
Create printers
Update printers
Delete printers
Track power consumption
Printer type validation
Filaments
Create filaments
Update filaments
Delete filaments
Track spool weight
Track spool cost
Calculate cost per gram
Materials
Create materials
Update materials
Delete materials
Track quantity
Track cost per unit
Projects
Create projects
Update projects
Delete projects
Associate printers
Associate filaments and material usage
Store cost snapshots at time of project creation
Costing
Filament cost calculations
Material cost calculations
Electricity cost calculations
Labor cost calculations
Global settings support
Final price recommendations
Global Settings
Electricity Rate
Labor Rate
Default Markup
Default Waste Multiplier
Solution Structure
PrintHub/
│
├── Controllers/
├── Database/
│   ├── Models/
│   └── Migrations/
│
├── DTOs/
├── Helpers/
├── Mappers/
├── Services/
├── Validators/
│
├── PrintHub.Services_Tests/
│
└── PrintHub.Frontend/
Getting Started
Prerequisites
Install:

.NET 8 SDK
PostgreSQL
Node.js 20+
npm
Backend Setup
Restore Packages
dotnet restore
Apply Migrations
dotnet ef database update
Run API
dotnet run
Swagger will be available at:

https://localhost:7209/swagger
Frontend Setup
Navigate to the frontend:

cd PrintHub.Frontend
Install dependencies:

npm install
Start development server:

npm run dev
Frontend will be available at:

http://localhost:5173
Testing
Run all tests:

dotnet test
Run a specific project:

dotnet test PrintHub.Services_Tests
Pricing Model
Project cost is calculated using:

Filament Cost
+ Material Cost
+ Electricity Cost
+ Labor Cost
--------------------------------
Total Cost

Total Cost * Markup
--------------------------------
Suggested Price
Project filament and material costs are preserved using snapshot pricing:

Cost_At_Time
This ensures historical projects are not affected by future inventory price changes.

Current Roadmap
Backend
Price Modifiers
Additional Cost Reporting
Project Cost History
Frontend
Global Settings Page
Projects List Page
Project Details Page
Cost Breakdown Component
Future Ideas
Import from slicers
Inventory tracking
Print job history
Print farm support
Reporting dashboard
Customer management
Development Notes
Global Settings
PrintHub expects a single GlobalSettings record.

Default values are seeded during application startup.

Cost Snapshots
When a project is created:

Filament cost per gram is stored
Material cost per unit is stored
This prevents project costs from changing after inventory prices are updated.

Validation
FluentValidation is used for all API request validation.

Validator test coverage exists for:

Filaments
Materials
Printers
Projects
Global Settings
License
Personal project created by Matt Galvan.
