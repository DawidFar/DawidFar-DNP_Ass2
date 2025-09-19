using System.Linq;
using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class InMemoryCommentRepository : ICommentRepository
{
    private readonly List<Comment> _comments = new();

    public void Add(Comment comment) => _comments.Add(comment);

    public void Update(Comment comment)
    {
        var existing = _comments.FirstOrDefault(c => c.Id == comment.Id);
        if (existing != null)
        {
            existing.Body = comment.Body;
            existing.UserId = comment.UserId;
            existing.PostId = comment.PostId;
        }
    }

    public void Delete(int id) => _comments.RemoveAll(c => c.Id == id);

    public Comment? GetById(int id) => _comments.FirstOrDefault(c => c.Id == id);

    public List<Comment> GetAll() => new List<Comment>(_comments);
}
