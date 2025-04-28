# Employee Management System (ASP.NET Core Razor Pages) 🚀

![.NET 7.0](https://img.shields.io/badge/.NET-7.0-blueviolet)
![Bootstrap 5](https://img.shields.io/badge/Bootstrap-5-blue)
![NLog](https://img.shields.io/badge/NLog-Enabled-yellowgreen)

> A full-featured Employee Management System built with **ASP.NET Core Razor Pages**, **N-Tier Architecture**, **Bootstrap 5**, **Entity Framework Core**, and **NLog Logging**.

---

## 📚 Table of Contents

- [📁 Project Structure](#-project-structure)
- [🖼 Project Images](#-project-images)
- [✅ Features](#-features)
- [🛠️ Technologies Used](#️-technologies-used)
- [⚙️ Getting Started](#️-getting-started)
- [📦 Important Configuration](#-important-configuration)
- [📋 Notes](#-notes)
- [🧑‍💻 Author](#-author)
- [📢 Future Enhancements](#-future-enhancements)

---

## 📁 Project Structure

- **MyApp.Core**  
  ➤ Models, Constants, (Employee, Department), App Constants

- **MyApp.Data**  
  ➤ Entity Framework `AppDbContext`, Migrations, Database Contexts

- **MyApp.Service**  
  ➤ Business Logic, Interfaces, Implementations, Helpers (CommonHelpers, FileHelper)

- **MyApp.Web**  
  ➤ Razor Pages (CRUD for Employees and Departments), Bootstrap 5 UI, HTML Helpers, NLog setup

---

## ✅ Features

- 🔄 Full **CRUD Operations** for Employees and Departments
- 🖼 Profile Image **Upload and Preview**
- 🔍 **Search and Filter** Employees by Name, Department, and Status
- ✅ **Status Toggle** (Active/Inactive) for Employees
- 🗕 **Date Formatting** (MMM DD, YYYY) using a reusable HTML Helper
- 🔐 **Audit Logging** (Create, Edit, Delete actions) via NLog
- 📋 **Structured Application Logging** using **NLog**
- 🎨 **Responsive UI** with Bootstrap 5
- 🧠 No **Hardcoded Values** — all constants managed separately
- 🔁 **Utility Classes** (Get IP Address, Current UTC Time, Current Username)
- 🧾 Full **XML Comments** on Models, Services, Helpers
- 🛡️ Strong **Validation** with custom error messages
- 📦 Cleanly separated **N-Tier Architecture**

---

## 🖼 Project images
- Home Page
![Home](https://github.com/user-attachments/assets/41b4b018-1dc2-4cd2-bdf5-ccb0b22fc0e8)

- Employee List Page
![Employee List](https://github.com/user-attachments/assets/e5b47b51-bada-4c58-8de0-02b43941f833)

- Add Employee Page
![Employee Add](https://github.com/user-attachments/assets/f7932768-ed73-421b-81d0-12a52e85f8b2)

- Employee Delete Feature
![Employee Delete](https://github.com/user-attachments/assets/11a146b8-1a83-40ad-8cff-5ae69c7e9218)

- Department List Page
![Department List](https://github.com/user-attachments/assets/2668f795-9297-4c93-bb3b-030a0f9bfacd)

- Add Department Page
![Add Department](https://github.com/user-attachments/assets/78aceb7c-1402-4d0a-beef-63862cae7ac1)

- Update Department Page
![Department Update](https://github.com/user-attachments/assets/d13d6ed2-95b6-4b4d-b922-d3edc53b6476)

- Department Delete Feature
![Department Delete](https://github.com/user-attachments/assets/89c637ca-28bc-4088-9c76-a11088e85ac9)

- Employees Data Page
![Employee Data](https://github.com/user-attachments/assets/f2ba3478-4b65-4e36-8e33-fefb8d395184)


---

## 🛠️ Technologies Used

- ASP.NET Core 7.0 (Razor Pages)
- Entity Framework Core 7
- SQL Server (LocalDB or SQL Express)
- Bootstrap 5
- NLog for Logging
- Dependency Injection (built-in .NET Core)
- LINQ, EF Core Migrations
- C# 11

---

## ⚙️ Getting Started

### 1️⃣ Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- Visual Studio 2022 or newer
- SQL Server (LocalDB or SQL Express)

---

### 2️⃣ Database Setup

Open **Package Manager Console** inside Visual Studio and run:

```bash
Add-Migration InitialCreate
Update-Database
```

This will create the necessary tables:

- `Departments`
- `Employees`

---

### 3️⃣ Running the Application

- Set **MyApp.Web** as the **Startup Project**.
- Press **F5** or click **Start Debugging**.
- The app will open in your browser at:  
  `https://localhost:xxxx`

---

## 📦 Important Configuration

- **NLog** is configured via the `NLog.config` file.
- **Logs** are automatically stored under the `/Logs/` directory.
- **Profile Images** are uploaded and stored under `/wwwroot/uploads/`.
- Constants, HTML Helpers, and Utility classes are used to minimize code duplication.
- **Audit Logs** are automatically generated on Create, Edit, and Delete actions via NLog.

---

## 📋 Notes

- Always ensure the folders **wwwroot/uploads/** and **Logs/** exist before running.
- Update the **Connection String** inside `appsettings.json` based on your local SQL Server setup.
- Structured logging and error capturing is handled using **NLog**.
- Date formatting across the UI is handled through a custom **HTML Helper**.

---

## 🧑‍💻 Author

Developed with ❤️ using modern **ASP.NET Core** practices, clean **N-Tier Architecture**, and **Best Coding Standards**.

---

## 📢 Future Enhancements (Optional)

- 📨 Add **Email Notification System** (for new employee creation)
- 🌐 Deploy on **Azure**, **AWS**, or **DigitalOcean**
- 🚀 Dockerize the entire application
- 🧠 Setup **Unit Testing** and **Integration Testing**
- 📊 Add a **Dashboard** with Employee Statistics and Charts

---

👉 *This project is clean, maintainable, and production-ready!*

