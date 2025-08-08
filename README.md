# Employee Attendance Tracker
ASP.NET MVC application for managing employee attendance with department and employee management. Features include:

Department CRUD operations with unique code validation

Employee management with attendance summaries

Dynamic attendance recording with calendar UI

Live status updates using AJAX/jQuery

Filtering by department/employee/date range

Setup Instructions

Prerequisites: .NET 9 SDK

Clone repository:

bash

git clone https://github.com/MostafaElZoka/Attendance-Tracker.git

cd employee-attendance-tracker

Run application:

bash

dotnet run --project '.\Employee Attendace Tracker\Employee Attendace Tracker.csproj'

Access: https://localhost:5001 in browser

Architecture
N-Tier Structure:
Presentation Layer : contains Controllers and Views 
Business Layer : contains the services, Dtos, Interfaces and validation
Data Layer : contains the repositories, unit of work, ef core, in-memory db and data seeding

Presentation Layer depends on Business Layer interfaces

Business Layer depends on Data Layer repositories

Data Layer has no dependencies on other layers

Cross-cutting: DTOs shared between Presentation and Business layers
