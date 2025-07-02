# 🍕 Tomasos Pizzeria Web API

This project is a backend Web API solution for **Tomasos Pizzeria**, an online food service that sells pizzas, pasta, and salads. The API is built with **ASP.NET Core Web API**, deployed to **Microsoft Azure**, and uses **Entity Framework Core** with **Code First** for SQL database interactions. All sensitive information is managed securely via **Azure Key Vault**.

---

## ✅ Project Goals

The purpose of this assignment is to develop a scalable backend for Tomasos Pizzeria with the following core features:

- Secure **user authentication** with **JWT tokens**.
- Manage **dishes**, **ingredients**, and **categories**.
- Create and retrieve **customer orders**.
- Provide **role-based access control** using **ASP.NET Core Identity**.
- Handle **discounts and loyalty points** for Premium users.
- Deploy and run the solution completely in **Azure**.

---

## 🚀 Technologies Used

- ASP.NET Core Web API
- Entity Framework Core (Code First)
- SQL Server (Azure-hosted)
- Azure App Service
- Azure Key Vault
- Swagger / OpenAPI
- ASP.NET Core Identity
- Git + GitHub (Version Control)
- Postman (Testing)

---

## 🔐 Core Functionality

### User Authentication & Authorization

- Users can register with username, password, email, and phone number.
- JWT is returned on successful login.
- Logged-in users can view and update their own profile.
- Identity roles:
  - **Admin**: Full access, can manage dishes, orders, and users.
  - **PremiumUser**: Gets discounts and earns loyalty points.
  - **RegularUser**: Default role on registration.

### Dish Management

- Dishes include: name, price, description, category, and ingredients.
- Categories include: Pizza, Pasta, Salad, etc.

### Orders

- Logged-in users can:
  - Place an order with selected dishes.
  - View their past orders.
- Order functionality requires token-based authentication.

---

## 🌟 Extra Features for Higher Grade

- **Role Management**: Admin can upgrade users to PremiumUser and vice versa.
- **PremiumUser Benefits**:
  - 20% discount for 3+ dishes in one order.
  - Earn 10 points per dish.
  - Redeem 100 points for one free pizza.
- **Admin Functionality**:
  - Add/update/remove dishes and ingredients.
  - Cancel or update the status of orders.
- **Async Programming**: All endpoints are asynchronous using `async/await` and Task-based programming.
- **Logging**: Application logs errors and important events to Azure Logs.

---

## 🧰 Project Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/tomasos-pizzeria-api.git
   ```

2. Configure the following:
   - `appsettings.json` with fallback settings
   - Use **Azure Key Vault** to store:
     - Connection strings
     - JWT secret keys

3. Run migrations:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

4. Run the API:
   ```bash
   dotnet run
   ```

5. Access Swagger:
   ```
   https://your-azure-app-url/swagger
   ```

---

## 📦 Deployment

The API is deployed to **Azure App Service**, and connected to a **SQL Azure Database**. Configuration settings and secrets are managed with **Azure Key Vault**. Swagger is enabled for testing endpoints directly in the browser.

---

## 🧪 Testing

All endpoints can be tested using:

- **Swagger UI** (`/swagger`)
- **Postman** with Bearer tokens for protected routes

---

## 📝 Notes

- The solution follows clean architecture principles with separation of concerns between controllers, services, repositories, and data models.
- GitHub is used for full version control and collaboration.
- Tutorials and examples were referenced, but no step-by-step copying or cloning was performed.

---

## 📄 License

This project is part of a school assignment and is not intended for commercial use.