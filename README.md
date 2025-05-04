# Healthy Harmony — Full Project Documentation

## 1. Project Overview

**Healthy Harmony** is a modern web-based recipe management system developed using the ASP.NET Core MVC framework and SQL Server. The application provides users with a convenient, feature-rich platform to explore, evaluate, save, and interact with cooking recipes. Designed with clean UI/UX principles and an emphasis on usability, the system includes full CRUD support, role-based access control, and integrated data analytics via visualizations.

The platform supports three main user roles:

* **Guest**: can view recipes, filter by category or ingredients, and download PDFs.
* **Registered User**: gains access to commenting, rating, favoriting recipes, and reporting inappropriate content.
* **Administrator**: has full access to content management, user control, and statistical reporting.

---

## 2. Objectives and Goals

### Main Objective:

To design and implement a user-centric web application that simplifies the process of browsing and interacting with a structured database of culinary recipes.

### Goals:

* Implement user authentication and role-based authorization.
* Enable advanced filtering and full-text recipe search.
* Allow users to comment on, rate, and save recipes.
* Provide PDF export functionality for offline recipe access.
* Equip administrators with tools for moderation, reporting, and analytics.
* Ensure data integrity and structured storage using Entity Framework Core.

---

## 3. Features

### Minimum Viable Product (MVP):

* Browse recipes
* Register/login with email verification
* Add comments to recipes
* Rate dishes from 1 to 5 stars
* Save dishes to favorites
* Admin can add/edit/delete recipes

### Extended Features:

* PDF generation for recipes
* Full statistical dashboard (likes, ratings, comments)
* Reporting system for inappropriate content
* Filtering by ingredient, time, and category
* Admin report generation (most active users, most rated dishes)

---

## 4. Technology Stack

| Technology            | Purpose                             |
| --------------------- | ----------------------------------- |
| ASP.NET Core MVC      | Web application framework           |
| C#                    | Backend logic and models            |
| Entity Framework Core | ORM for SQL Server                  |
| SQL Server            | Relational database                 |
| HTML/CSS, Bootstrap   | Frontend and layout                 |
| JavaScript            | Interactivity and modals            |
| Google Charts API     | Visual representation of statistics |
| SelectPdf             | Generating PDF files                |

---

## 5. System Architecture

### Subsystems:

* **Public Subsystem**: accessible to all users (guests), includes basic browsing and filtering.
* **User Subsystem**: provides personalized interaction, including favorites, ratings, and comments.
* **Admin Subsystem**: includes full management of content, users, reports, and statistical outputs.

### MVC Structure:

* **Models**: define data schemas and relationships
* **Views**: Razor-based UI pages with responsive design
* **Controllers**: handle HTTP requests, business logic, and return appropriate responses

---

## 6. Database Design

Healthy Harmony uses a normalized SQL Server database managed via Entity Framework Core. All tables have defined primary and foreign keys to maintain data integrity.

### Main Tables:

* **Users**: stores personal and role data
* **Dishes**: contains all information about recipes
* **Ingredients**: central registry of ingredients
* **DishIngredients**: junction table between dishes and ingredients
* **Comments**: user feedback on dishes
* **Ratings**: 1–5 star values linked to users and dishes
* **FavoriteDishes**: saves user-favorite recipes
* **CommentReports**: user reports about comment violations

---

## 7. System Requirements

### Client:

* Compatible browsers: Chrome (90+), Firefox (85+), Safari (14+), Edge
* Device support: Desktop, tablet, mobile (responsive)
* Language: Ukrainian (default), English (extensible)

### Server:

* OS: Windows 10+ (local), Azure or IIS (deployment)
* Framework: .NET 6+
* Database: SQL Server Express or Azure SQL
* Tools: Visual Studio 2022 or VS Code

---

## 8. Installation & Deployment

1. Clone the repository:

```bash
git clone https://github.com/your-username/HealthyHarmony.git
```

2. Open in Visual Studio 2022
3. Create the SQL Server database (or use LocalDB)
4. Apply migrations:

```bash
dotnet ef database update
```

5. Run the application:

```bash
dotnet run
```

---

## 9. Testing

* Validation on both client and server sides
* Admin and user role workflows tested manually
* Edge cases tested: empty fields, duplicate emails, unauthorized access
* PDF and chart generation verified
* Layout tested on multiple resolutions

---

## 10. Conclusion

The Healthy Harmony project successfully delivers on its goal of creating a full-featured, user-friendly recipe management platform. Through the use of modern development frameworks and design principles, the system provides intuitive interaction, structured data access, and robust administrative tools. It stands as a demonstration of practical skills in backend development, frontend implementation, database design, and secure role-based access control.

The architecture and codebase allow for future extensions such as user profiles, recipe sharing, or AI-powered meal recommendations.

---

## 11. Authors

* Yvgieniia Riabichenko
* Developed as part of Laboratory Assignment #1
* Institution: Taras Shevchenko National University of Kyiv
* Course: 2
