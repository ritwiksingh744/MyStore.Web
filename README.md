# 🏬 MyStore MVC Project (.NET Core 9.0)

This is an **ASP.NET Core 9 MVC application** built using **Entity Framework Core (Code-First + Data Seeding)**.  
It demonstrates a clean architecture with product categories, items, and orders management.

---

## 🚀 Getting Started

### 1. Prerequisites
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) with **.NET Core 9 SDK**
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB / Express / Full)
- EF Core tools in Visual Studio

---

### 2. Setup Instructions
1. **Clone or Download**
   ```bash
   git clone https://github.com/your-repo/MyStore.git
Or unzip the provided source code.

2. **Open the Project**
  - Launch Visual Studio
  - Open the .sln file
3. **Check Connection String**
  - Open appsettings.json
  - Update:
  
    ```bash
    "ConnectionStrings": {"DefaultConnection": "Server=YOUR_SERVER_NAME;Database=MyStoreDb;Trusted_Connection=True;MultipleActiveResultSets=true"}
## 
4. **Apply Migrations**
   - The project uses EF Core Code-First.
   - Open Package Manager Console in Visual Studio
   - Set Default Project → MyStore.Data
    - Run:
      update-database
5. **Run the App**
  - Press F5 or Ctrl+F5
  - The app will open in your browser 🎉
## 🛠️ Tech Stack
- ASP.NET Core 9.0 MVC
- Entity Framework Core (Code-First + Seeding)
- SQL Server
- LINQ
- Bootstrap + jQuery DataTables
##
📷 Demo
🏠 Home Page
📦 Items Management
🛒 Orders Management
##
**⚡ Notes**
- If update-database fails, recheck your SQL Server instance in appsettings.json.
- Default seed includes categories, items, and demo orders.
##
**📜 License**
- This project is licensed under the MIT License
