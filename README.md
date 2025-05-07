
# 📝 Todo Management Application

A full-stack Todo Management System built using **ASP.NET Core 8**, **Entity Framework Core**, and **Bootstrap 5**.  
This project was developed for a company-assigned task to demonstrate:

- Clean code architecture
- RESTful API design
- Multi-layered separation of concerns
- Responsive MVC UI
- Practical use of AJAX & HttpClient

---

## 📦 Features

- ✅ CRUD operations for Todos
- ✅ Status management: `Pending`, `InProgress`, `Completed`
- ✅ Priority levels: `Low`, `Medium`, `High`
- ✅ Filter todos by status and priority
- ✅ Apply todos order by descanding 
- ✅ Mark todos as Completed (AJAX)
- ✅ Server-side pagination support
- ✅ Client/server-side validation
- ✅ Responsive UI with Bootstrap 5
- ✅ Error-handled RESTful APIs
- ✅ MVC Frontend integrated with API using HttpClient

---

## 📐 Architecture

- **Backend:** ASP.NET Core 8 Web API (clean architecture)
- **Frontend:** ASP.NET Core MVC with Razor Views
- **Database:** SQL Server with Entity Framework Core
- **Design Patterns:** Repository + Unit of Work
- **Integration:** API consumption via `HttpClient` in MVC
- **UI Enhancement:** AJAX-based updates for better UX

---

## 🚀 Getting Started

### 🛠 Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB or full)
- Visual Studio 2022+ / VS Code
- Git

### 🔧 Setup Instructions

1. **Clone the Repository:**

```bash
git clone https://github.com/ahmedgamal23/todo-management-app.git
cd todo-management-app
```

2. **Configure the Database Connection:**

In `appsettings.json` of the API project:

```json
"ConnectionStrings": {
  "TodoConnectionString": "Server=.;Database=TodoDB;Trusted_Connection=True;"
}
```

In `appsettings.json` of the MVC project:

```json
"BaseUrl": "http://localhost:YourPortNumber/api"
```

3. **Apply Migrations:**

```bash
dotnet ef database update --project TodoManagementApplication.Domain
```

4. **Run the Application:**

```bash
# Run API
dotnet run --project TodoManagementApplication.Api

# Run MVC UI
dotnet run --project TodoManagementApplication.UI
```

5. **Open in Browser:**

- API: [https://localhost:PORT/swagger](https://localhost:PORT/swagger)
- UI: [https://localhost:PORT](https://localhost:PORT)

---

## 🧪 Test Features

- ✅ Add new Todo via MVC form
- ✅ Filter todos by status using dropdown
- ✅ Paginate todo list
- ✅ Click "Completed" button to update status via AJAX
- ✅ Edit or delete todos via UI
- ✅ View formatted details (status, priority)
- ✅ Validation with error messages for all fields

---

## 📁 Project Structure

```
TodoManagementApplication/
├── Domain/          → Models, Enums, Validation Attributes
├── Application/     → Interfaces, Services, Unit of Work
├── Infrastructure/  → EF Core DbContext & Repository Implementation
├── API/             → ASP.NET Core 8 API Endpoints (Controllers)
├── UI/              → ASP.NET Core MVC + Views + HttpClient Integration
```

---

## 📄 Task Reference

This project was implemented to meet the specifications provided in the company’s **Todo Management** task:

- Build CRUD APIs
- Manage Todo status based on DueDate
- Create a modern UI using Bootstrap
- Apply validation and error handling
- Implement real-time status update (AJAX)

---

## 👤 Author

**Ahmed Gamal Ibrahim**  
📧 [ahmedgamal52001@gmail.com](mailto:ahmedgamal52001@gmail.com)  
🔗 [GitHub](https://github.com/ahmedgamal23)  
🔗 [LinkedIn](https://www.linkedin.com/in/ahmed-gamal-667a061a3/)

---
