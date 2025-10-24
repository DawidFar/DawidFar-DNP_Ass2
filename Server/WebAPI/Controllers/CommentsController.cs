using Microsoft.AspNetCore.Mvc;
using ApiContracts.Comments;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _commentRepo;
    private readonly IUserRepository _userRepo;
    private readonly IPostRepository _postRepo;

    public CommentsController(ICommentRepository commentRepo, IUserRepository userRepo, IPostRepository postRepo)
    {
        _commentRepo = commentRepo;
        _userRepo = userRepo;
        _postRepo = postRepo;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create([FromBody] CreateCommentDto dto)
    {
        var user = await _userRepo.GetAsync(dto.UserId);
        if (user == null) return BadRequest("User not found.");
        var post = await _postRepo.GetAsync(dto.PostId);
        if (post == null) return BadRequest("Post not found.");

        var comment = new Comment(dto.PostId, dto.UserId, dto.Content);
        var created = await _commentRepo.AddAsync(comment);

        var result = new CommentDto { Id = created.Id, PostId = created.PostId, UserId = created.UserId, Content = created.Content, CreatedAt = created.CreatedAt, UserName = user.UserName };
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CommentDto>> GetById(Guid id)
    {
        var c = await _commentRepo.GetAsync(id);
        if (c == null) return NotFound();
        var user = await _userRepo.GetAsync(c.UserId);
        return new CommentDto { Id = c.Id, PostId = c.PostId, UserId = c.UserId, Content = c.Content, CreatedAt = c.CreatedAt, UserName = user?.UserName };
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetMany([FromQuery] Guid? userId, [FromQuery] string? userName, [FromQuery] Guid? postId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var comments = (await _commentRepo.GetAllAsync()).AsQueryable();

        if (userId.HasValue) comments = comments.Where(c => c.UserId == userId.Value);

        if (!string.IsNullOrWhiteSpace(userName))
        {
            var users = await _userRepo.GetAllAsync();
            var matching = users.Where(u => u.UserName != null && u.UserName.Contains(userName, StringComparison.OrdinalIgnoreCase)).Select(u => u.Id).ToHashSet();
            comments = comments.Where(c => matching.Contains(c.UserId));
        }

        if (postId.HasValue) comments = comments.Where(c => c.PostId == postId.Value);

        var pageItems = comments.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var usersAll = (await _userRepo.GetAllAsync()).ToDictionary(u => u.Id, u => u.UserName);

        var result = pageItems.Select(c => new CommentDto
        {
            Id = c.Id,
            PostId = c.PostId,
            UserId = c.UserId,
            Content = c.Content,
            CreatedAt = c.CreatedAt,
            UserName = usersAll.ContainsKey(c.UserId) ? usersAll[c.UserId] : null
        });

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await _commentRepo.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}