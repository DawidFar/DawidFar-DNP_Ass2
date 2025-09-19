using System.Linq;
using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public void Add(User user) => _users.Add(user);

    public void Update(User user)
    {
        var existing = _users.FirstOrDefault(u => u.Id == user.Id);
        if (existing != null)
        {
            existing.Username = user.Username;
            existing.Password = user.Password;
        }
    }

    public void Delete(int id) => _users.RemoveAll(u => u.Id == id);

    public User? GetById(int id) => _users.FirstOrDefault(u => u.Id == id);

    public List<User> GetAll() => new List<User>(_users);
}
