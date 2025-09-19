using Entities;

namespace RepositoryContracts;

public interface IUserRepository
{
    void Add(User user);
    void Update(User user);
    void Delete(int id);
    User? GetById(int id);
    List<User> GetAll();
}
