using System.Linq;
using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class InMemoryPostRepository : IPostRepository
{
    private readonly List<Post> _posts = new();

    public void Add(Post post) => _posts.Add(post);

    public void Update(Post post)
    {
        var existing = _posts.FirstOrDefault(p => p.Id == post.Id);
        if (existing != null)
        {
            existing.Title = post.Title;
            existing.Body = post.Body;
            existing.UserId = post.UserId;
        }
    }

    public void Delete(int id) => _posts.RemoveAll(p => p.Id == id);

    public Post? GetById(int id) => _posts.FirstOrDefault(p => p.Id == id);

    public List<Post> GetAll() => new List<Post>(_posts);
}
