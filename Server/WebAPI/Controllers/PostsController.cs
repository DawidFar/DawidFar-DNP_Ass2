using Microsoft.AspNetCore.Mvc;
using ApiContracts.Posts;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository _postRepo;
    private readonly IUserRepository _userRepo;

    public PostsController(IPostRepository postRepo, IUserRepository userRepo)
    {
        _postRepo = postRepo;
        _userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> Create([FromBody] CreatePostDto dto)
    {
        var author = await _userRepo.GetAsync(dto.AuthorId);
        if (author == null) return BadRequest("Author not found.");

        var post = new Post(dto.AuthorId, dto.Title, dto.Content);
        var created = await _postRepo.AddAsync(post);

        var result = new PostDto
        {
            Id = created.Id,
            AuthorId = created.AuthorId,
            Title = created.Title,
            Content = created.Content,
            CreatedAt = created.CreatedAt,
            AuthorName = author.UserName
        };

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PostDto>> GetById(Guid id)
    {
        var p = await _postRepo.GetAsync(id);
        if (p == null) return NotFound();
        var author = await _userRepo.GetAsync(p.AuthorId);
        return new PostDto { Id = p.Id, AuthorId = p.AuthorId, Title = p.Title, Content = p.Content, CreatedAt = p.CreatedAt, AuthorName = author?.UserName };
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetMany([FromQuery] string? title, [FromQuery] Guid? authorId, [FromQuery] string? authorName, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var posts = (await _postRepo.GetAllAsync()).AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
            posts = posts.Where(p => p.Title != null && p.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

        if (authorId.HasValue)
            posts = posts.Where(p => p.AuthorId == authorId.Value);

        if (!string.IsNullOrWhiteSpace(authorName))
        {
            var users = await _userRepo.GetAllAsync();
            var matching = users.Where(u => u.UserName != null && u.UserName.Contains(authorName, StringComparison.OrdinalIgnoreCase)).Select(u => u.Id).ToHashSet();
            posts = posts.Where(p => matching.Contains(p.AuthorId));
        }

        var pageItems = posts.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var usersAll = (await _userRepo.GetAllAsync()).ToDictionary(u => u.Id, u => u.UserName);

        var result = pageItems.Select(p => new PostDto
        {
            Id = p.Id,
            AuthorId = p.AuthorId,
            Title = p.Title,
            Content = p.Content,
            CreatedAt = p.CreatedAt,
            AuthorName = usersAll.ContainsKey(p.AuthorId) ? usersAll[p.AuthorId] : null
        });

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await _postRepo.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}