using FileRepositories;
using RepositoryContracts;
using Server.CliApp.UI;

namespace Server.CliApp;

class Program
{
    static void Main()
    {
        // In-memory repositories
        ICommentRepository commentRepo = new CommentFileRepository();
            IPostRepository postRepo = new PostFileRepository();
            IUserRepository userRepo = new UserFileRepository();

        // Start CLI
        var app = new Cli(userRepo, postRepo, commentRepo);
        app.Run();
    }
}
