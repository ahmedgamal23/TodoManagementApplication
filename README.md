
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
## Samples:
![Screenshot (615)](https://github.com/user-attachments/assets/85eb76bd-e6ad-424e-adb0-a7695903e363)
![Screenshot (616)](https://github.com/user-attachments/assets/d64b5f6f-835d-450f-91b8-4c5c2c7d88c9)
![Screenshot (617)](https://github.com/user-attachments/assets/4af9aacf-492a-4d52-8fb0-eaac6f4d86f0)
![Screenshot (618)](https://github.com/user-attachments/assets/a478ee5b-20fe-469d-b0a4-dafecadeb6ff)
![Screenshot (619)](https://github.com/user-attachments/assets/a18fdffc-918f-461b-8893-c52c95c58d56)
![Screenshot (620)](https://github.com/user-attachments/assets/3aff7fe9-8dfd-4d23-9c9d-e8defbd5f818)
![Screenshot (621)](https://github.com/user-attachments/assets/ab0542cc-e710-4ccb-aaac-c12b8dcb0919)
![Screenshot (622)](https://github.com/user-attachments/assets/a22ad980-0f3c-4e32-a1e2-3b0c0e46355d)

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
📞 +201147893607
---
