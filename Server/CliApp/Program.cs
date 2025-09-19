using InMemoryRepositories;
using RepositoryContracts;
using Server.CliApp.UI;

namespace Server.CliApp;

class Program
{
    static void Main()
    {
        // In-memory repositories
        IUserRepository userRepo = new InMemoryUserRepository();
        IPostRepository postRepo = new InMemoryPostRepository();
        ICommentRepository commentRepo = new InMemoryCommentRepository();

        // Start CLI
        var app = new Cli(userRepo, postRepo, commentRepo);
        app.Run();
    }
}
