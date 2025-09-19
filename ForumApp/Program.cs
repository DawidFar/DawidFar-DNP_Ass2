using Entities;
using RepositoryContracts;
using InMemoryRepositories;

namespace ForumApp;

class Program
{
    static void Main()
    {
        IPostRepository postRepo = new InMemoryPostRepository();
        IUserRepository userRepo = new InMemoryUserRepository();
        ICommentRepository commentRepo = new InMemoryCommentRepository();

        var u = new User { Id = 1, Username = "fadum", Password = "1234" };
        userRepo.Add(u);

        var p = new Post { Id = 1, Title = "Hello", Body = "My first post", UserId = u.Id };
        postRepo.Add(p);

        var c = new Comment { Id = 1, Body = "Nice post!", UserId = u.Id, PostId = p.Id };
        commentRepo.Add(c);

        Console.WriteLine("== Users ==");
        foreach (var user in userRepo.GetAll())
            Console.WriteLine($"{user.Id}: {user.Username}");

        Console.WriteLine("\n== Posts ==");
        foreach (var post in postRepo.GetAll())
            Console.WriteLine($"{post.Id}: {post.Title} - {post.Body} (User {post.UserId})");

        Console.WriteLine("\n== Comments ==");
        foreach (var com in commentRepo.GetAll())
            Console.WriteLine($"{com.Id}: {com.Body} (User {com.UserId} on Post {com.PostId})");
    }
}
