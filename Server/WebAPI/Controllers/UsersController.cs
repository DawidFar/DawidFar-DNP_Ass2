using Microsoft.AspNetCore.Mvc;
using ApiContracts.Users;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepo;

    public UsersController(IUserRepository userRepo) => _userRepo = userRepo;

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.UserName))
            return BadRequest("UserName required.");

        var existing = await _userRepo.GetByUserNameAsync(dto.UserName);
        if (existing != null) return Conflict("Username already exists.");

        var user = new User(dto.UserName, dto.Password);
        var created = await _userRepo.AddAsync(user);

        var result = new UserDto { Id = created.Id, UserName = created.UserName };
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> GetById(Guid id)
    {
        var u = await _userRepo.GetAsync(id);
        if (u == null) return NotFound();
        return new UserDto { Id = u.Id, UserName = u.UserName };
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetMany([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var users = (await _userRepo.GetAllAsync()).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            users = users.Where(u => u.UserName != null && u.UserName.Contains(search, StringComparison.OrdinalIgnoreCase));

        var items = users.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(u => new UserDto { Id = u.Id, UserName = u.UserName });

        return Ok(items);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await _userRepo.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}