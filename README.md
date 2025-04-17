# 📦 API + MVC Frontend App

This project consists of a **.NET 8 Web API** and an **MVC front-end** application. It demonstrates basic authentication using JWT, API consumption from a front-end app, and includes seed/dummy data for testing purposes.

---

## ✅ Requirements

- **.NET 8 SDK** installed on your machine.
- A **local database instance** (e.g., SQL Server or LocalDB).
- Visual Studio or another IDE that supports **startup project selection** and HTTPS profiles.

---

## ⚙️ Setup Instructions

1. **Clone the repository**  
   Make sure you have .NET 8 installed before proceeding.

2. **Database Connection**  
   Update the `appsettings.development.json` file in the Web API project:
   ```json
   "ConnectionStrings": {
     "API": "Your_Local_DB_Connection_String"
   }

3. **Start-up Configuration**
In Visual Studio:
Right-click the solution > Set Startup Projects...
Choose Multiple startup projects
Set both the Web API and the MVC app to Start
Ensure both projects are set to run using HTTPS

4. **Run the Application**
Launch the app:
The MVC frontend should open in your browser
The Web API will be available at a URL like:
https://localhost:xxxx/swagger

---

## 🔐 Authentication (JWT)
1. **Go to `/login` in Swagger UI**  
   Access the login endpoint via Swagger.

2. **Use any username and password**  
   This will return a **token** for authentication.  
   > ⚠️ This is a mock login — not connected to a real user store.

3. **Click "Authorize" in Swagger**  
   Use the "Authorize" button at the top of the Swagger UI.

4. **Enter your token in the following format:**
   Bearer your_token_here
   
---

## 🧪 Dummy Data

- Dummy data is seeded into the database
- Test basic CRUD operations and UI integration

---

## 📋 Additional Notes

- Code comments are minimal (prototype state)
- This setup demonstrates **Web API + MVC app** integration
- **Swagger UI** is enabled for easy testing

---

## 🛠 Planned Improvements

- Full user authentication
- Password hashing
- Role-based authorization
- Refactored architecture and cleaner code
- Better Error Handling

---

