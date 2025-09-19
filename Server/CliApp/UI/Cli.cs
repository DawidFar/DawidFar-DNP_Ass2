using System.Linq;
using RepositoryContracts;
using Entities;

namespace Server.CliApp.UI;

public class Cli
{
    private readonly IUserRepository _users;
    private readonly IPostRepository _posts;
    private readonly ICommentRepository _comments;

    public Cli(IUserRepository users, IPostRepository posts, ICommentRepository comments)
    {
        _users = users;
        _posts = posts;
        _comments = comments;
    }

    public void Run()
    {
        PrintWelcome();

        while (true)
        {
            Console.Write("\n> ");
            var cmd = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

            switch (cmd)
            {
                case "help": PrintHelp(); break;
                case "exit": return;

                case "user": CreateUser(); break;
                case "post": CreatePost(); break;
                case "comment": AddComment(); break;

                case "posts": ShowPostsOverview(); break;
                case "view": ViewSinglePost(); break;

                default:
                    Console.WriteLine("Unknown command. Type 'help' to see options.");
                    break;
            }
        }
    }

    private void PrintWelcome()
    {
        Console.WriteLine("=== Forum CLI ===");
        PrintHelp();
    }

    private void PrintHelp()
    {
        Console.WriteLine(@"
Commands:
  user            -> create a new user
  post            -> create a new post (title, content, userId)
  comment         -> add a comment (postId, userId, body)
  posts           -> list all posts (id + title)
  view            -> view one post
  help            -> show this help
  exit            -> quit
");
    }

    private void CreateUser()
    {
        Console.Write("Username: ");
        string username = Console.ReadLine()!.Trim();
        Console.Write("Password: ");
        string password = Console.ReadLine()!.Trim();

        int nextId = _users.GetAll().Any() ? _users.GetAll().Max(u => u.Id) + 1 : 1;
        var user = new User { Id = nextId, Username = username, Password = password };
        _users.Add(user);

        Console.WriteLine($"Created user #{user.Id} ({user.Username}).");
    }

    private void CreatePost()
    {
        Console.Write("Title: ");
        string title = Console.ReadLine()!.Trim();
        Console.Write("Content: ");
        string content = Console.ReadLine()!.Trim();
        Console.Write("User Id (author): ");
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid user id.");
            return;
        }

        var author = _users.GetById(userId);
        if (author is null)
        {
            Console.WriteLine($"No user with id {userId}.");
            return;
        }

        int nextId = _posts.GetAll().Any() ? _posts.GetAll().Max(p => p.Id) + 1 : 1;
        var post = new Post { Id = nextId, Title = title, Body = content, UserId = userId };
        _posts.Add(post);

        Console.WriteLine($"Created post #{post.Id} by {author.Username}.");
    }

    private void AddComment()
    {
        Console.Write("Post Id: ");
        if (!int.TryParse(Console.ReadLine(), out int postId))
        {
            Console.WriteLine("Invalid post id.");
            return;
        }

        Console.Write("User Id: ");
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid user id.");
            return;
        }

        Console.Write("Body: ");
        string body = Console.ReadLine()!.Trim();

        var post = _posts.GetById(postId);
        var user = _users.GetById(userId);
        if (post is null || user is null)
        {
            Console.WriteLine("Invalid user or post id.");
            return;
        }

        int nextId = _comments.GetAll().Any() ? _comments.GetAll().Max(c => c.Id) + 1 : 1;
        var comment = new Comment { Id = nextId, PostId = postId, UserId = userId, Body = body };
        _comments.Add(comment);

        Console.WriteLine($"Added comment #{comment.Id} to post #{postId} by {user.Username}.");
    }

    private void ShowPostsOverview()
    {
        var all = _posts.GetAll().OrderBy(p => p.Id).ToList();
        if (all.Count == 0)
        {
            Console.WriteLine("(no posts yet)");
            return;
        }

        Console.WriteLine("Posts:");
        foreach (var p in all)
            Console.WriteLine($"  {p.Id}: {p.Title}");
    }

    private void ViewSinglePost()
    {
        Console.Write("Post Id: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid id.");
            return;
        }

        var p = _posts.GetById(id);
        if (p is null)
        {
            Console.WriteLine($"No post with id {id}.");
            return;
        }

        var author = _users.GetById(p.UserId);
        Console.WriteLine($"\n[{p.Id}] {p.Title}");
        Console.WriteLine($"By: {(author?.Username ?? "Unknown")}");
        Console.WriteLine(p.Body);


        var postComments = _comments.GetAll().Where(c => c.PostId == p.Id).OrderBy(c => c.Id).ToList();
        if (postComments.Count == 0)
        {
            Console.WriteLine("\n(no comments)");
            return;
        }

        Console.WriteLine("\nComments:");
        foreach (var c in postComments)
        {
            var u = _users.GetById(c.UserId);
            Console.WriteLine($"  #{c.Id} by {(u?.Username ?? "Unknown")}: {c.Body}");
        }
    }
}
