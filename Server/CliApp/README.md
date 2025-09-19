# Assignment 2 – Command Line Interface (CLI)

## Overview
This project extends **Assignment 1** by adding an interactive Command Line Interface (CLI).  
Users can interact with the forum system directly from the terminal by creating users, writing posts, and adding comments.

The CLI is built on top of the same domain model (`User`, `Post`, `Comment`) and repository pattern (`IUserRepository`, `IPostRepository`, `ICommentRepository`) implemented in Assignment 1.

---


### Run the CLI
Open a terminal in the solution folder and run:
```powershell
dotnet run --project Server/CliApp/CliApp.csproj

=== Forum CLI ===
Commands:
  user            -> create a new user
  post            -> create a new post (title, content, userId)
  comment         -> add a comment (postId, userId, body)
  posts           -> list all posts (id + title)
  view            -> view one post
  help            -> show this help
  exit            -> quit


> user
Username: fadum
Password: 1234
Created user #1 (fadum).

> post
Title: Hello
Content: My first post
User Id (author): 1
Created post #1 by fadum.

> comment
Post Id: 1
User Id: 1
Body: Nice post!
Added comment #1 to post #1 by fadum.

> view
Post Id: 1

[1] Hello
By: fadum
My first post

Comments:
  #1 by fadum: Nice post!

> exit

