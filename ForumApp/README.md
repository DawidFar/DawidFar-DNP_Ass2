# ForumSolution (Assignment 1 — Entities and Repositories)

## Projects
- **Server/Entities**  
  Contains domain models:  
  - `User` (Id, Username, Password)  
  - `Post` (Id, Title, Body, UserId)  
  - `Comment` (Id, Body, UserId, PostId)  

- **Server/RepositoryContracts**  
  Interfaces defining CRUD operations:  
  - `IUserRepository`  
  - `IPostRepository`  
  - `ICommentRepository`  

- **Server/InMemoryRepositories**  
  In-memory list-based implementations for each repository.  

- **ForumApp**  
  Console app demonstrating how to use the repositories.  

---

## How to Build & Run

1. Install **.NET 9 SDK**.  
2. Open a terminal in the solution folder (`ForumSolution`).  
3. Run the following commands:

```powershell
cd C:\Users\fadum\Documents\ForumSolution
dotnet clean
dotnet build
dotnet run --project ForumApp/ForumApp.csproj
