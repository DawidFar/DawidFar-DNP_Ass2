using Entities;

namespace RepositoryContracts;

public interface IPostRepository
{
    void Add(Post post);
    void Update(Post post);
    void Delete(int id);
    Post? GetById(int id);
    List<Post> GetAll();
}
