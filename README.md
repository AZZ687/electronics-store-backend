# Electronics Store – Backend

ASP.NET Core Web API backend for a full-stack Electronics Store application.

The backend provides product management, authentication, authorization, order processing, stock management, and admin functionality using **ASP.NET Core, Entity Framework Core, SQL Server, and JWT authentication**.

## 🚀 Technologies

* ASP.NET Core Web API
* .NET 10
* Entity Framework Core
* SQL Server
* JWT Authentication
* RESTful APIs
* xUnit
* Git & GitHub

## ✨ Features

### Authentication & Authorization

* User registration and login
* JWT-based authentication
* Role-based authorization
* Customer and Admin roles
* Protected API endpoints

### Products

* Get products
* Get product by ID
* Admin product creation
* Admin product updates
* Admin product deletion

### Orders

* Create orders
* View customer orders
* View order details
* Admin order management
* Update order status
* Automatic stock reduction
* Stock validation to prevent overselling

### Testing

The project includes automated unit tests using xUnit covering:

* Product validation
* Invalid price and stock validation
* Order total calculation
* Stock reduction after an order
* Preventing quantities greater than available stock

## 🗄️ Database

The application uses **SQL Server** with Entity Framework Core Code First.

Main entities:

* Products
* Users
* Orders
* OrderItems

Database migrations are included in the project.

## 🔐 Security

Sensitive configuration such as:

* JWT signing key
* Database connection string

is stored using **.NET User Secrets** during local development and is not committed to the repository.

## ▶️ Running the Project

### 1. Clone the repository

```bash
git clone https://github.com/AZZ687/electronics-store-backend.git
cd electronics-store-backend
```

### 2. Configure User Secrets

Set the required JWT key and SQL Server connection string using .NET User Secrets.

Example:

```bash
dotnet user-secrets set "Jwt:Key" "YOUR_SECRET_KEY" --project ".\WebApplication1\WebApplication1.csproj"

dotnet user-secrets set "ConnectionStrings:DefaultConnection" "YOUR_CONNECTION_STRING" --project ".\WebApplication1\WebApplication1.csproj"
```

### 3. Apply database migrations

```bash
dotnet ef database update --project ".\WebApplication1\WebApplication1.csproj"
```

### 4. Run the API

```bash
dotnet run --project ".\WebApplication1\WebApplication1.csproj"
```

The API runs locally using ASP.NET Core.

## 🧪 Run Tests

From the repository root:

```bash
dotnet test
```

## 🔗 Frontend

This backend is connected to the React frontend:

**Electronics Store Frontend**

https://github.com/AZZ687/electronics-store-frontend

## 📌 Project Structure

```text
electronics-store-backend/
│
├── WebApplication1/
│   ├── Controllers/
│   ├── Data/
│   ├── DTOs/
│   ├── Models/
│   ├── Services/
│   ├── Migrations/
│   ├── Program.cs
│   └── appsettings.json
│
├── WebApplication1.Tests/
│   └── Unit Tests
│
├── .gitignore
├── WebApplication1.slnx
└── README.md
```

## 👨‍💻 Project

This project was developed as a full-stack portfolio application to demonstrate practical experience with modern web development, REST APIs, authentication, databases, testing, and Git-based development.
